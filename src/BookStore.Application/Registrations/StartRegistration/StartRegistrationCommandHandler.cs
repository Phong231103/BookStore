using BookStore.Application.Exceptions;
using BookStore.Application.Registration.Common;
using BookStore.Application.Registration.Interfaces;
using BookStore.Application.Registrations.Interfaces;
using BookStore.Application.Users.Interfaces;
using BookStore.Domain.Common.Services;
using BookStore.Domain.Users.ValueObjects;
using MediatR;

namespace BookStore.Application.Registrations.StartRegistration
{
    internal sealed class StartRegistrationCommandHandler
    : IRequestHandler<
        StartRegistrationCommand,
        StartRegistrationResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPendingRegistrationStore _pendingRegistrationStore;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IOtpGenerator _otpGenerator;
        private readonly IOtpHasher _otpHasher;
        private readonly IRegistrationEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailSender _emailSender;
        private readonly ISystemClock _systemClock;
        private readonly IRegistrationSettings _registrationSettings;

        public StartRegistrationCommandHandler(
            IUserRepository userRepository,
            IPendingRegistrationStore pendingRegistrationStore,
            IPasswordHasher passwordHasher,
            IOtpGenerator otpGenerator,
            IOtpHasher otpHasher,
            IRegistrationEmailTemplateProvider emailTemplateProvider,
            IEmailSender emailSender,
            ISystemClock systemClock,
            IRegistrationSettings registrationSettings)
        {
            _userRepository = userRepository;
            _pendingRegistrationStore = pendingRegistrationStore;
            _passwordHasher = passwordHasher;
            _otpGenerator = otpGenerator;
            _otpHasher = otpHasher;
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
            _systemClock = systemClock;
            _registrationSettings = registrationSettings;
        }

        public async Task<StartRegistrationResponse> Handle(
            StartRegistrationCommand request,
            CancellationToken cancellationToken)
        {
            Email email = Email.Create(request.Email);

            bool exists = await _userRepository.ExistsByEmailAsync(
                email,
                cancellationToken);

            if (exists)
            {
                throw new EmailAlreadyExistsException();
            }

            PasswordHash passwordHash =
                PasswordHash.Create(
                    _passwordHasher.Hash(request.Password));

            Guid registrationId = Guid.NewGuid();

            DateTime now = _systemClock.UtcNow;

            DateTime expiresAtUtc = now.Add(_registrationSettings.Expiration);

            string otp = _otpGenerator.Generate();

            string otpHash = _otpHasher.Hash(otp);

            PendingRegistration pendingRegistration =
                PendingRegistration.Create(
                    registrationId,
                    email,
                    passwordHash,
                    FullName.Create(request.FullName),
                    string.IsNullOrWhiteSpace(request.PhoneNumber)
                        ? null
                        : PhoneNumber.Create(request.PhoneNumber),
                    otpHash,
                    expiresAtUtc);

            await _pendingRegistrationStore.SaveAsync(
                pendingRegistration,
                cancellationToken);

            var emailMessage =
                _emailTemplateProvider.CreateOtpEmail(
                    email,
                    otp,
                    expiresAtUtc);

            await _emailSender.SendAsync(
                emailMessage,
                cancellationToken);

            return new StartRegistrationResponse
            {
                RegistrationId = registrationId,
                ExpiresAtUtc = expiresAtUtc
            };
        }
    }
}
