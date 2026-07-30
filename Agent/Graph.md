# BookStore — Source Code Map (Graph)

> **Cập nhật lần cuối:** 2026-07-30
> Bản đồ toàn bộ cấu trúc file/folder source code thực tế.

---

## Tổng quan Solution

```
BookStore.slnx
├── src/
│   ├── BookStore.Domain
│   ├── BookStore.Application
│   ├── BookStore.Infrastructure
│   ├── BookStore.WebApi
│   └── BookStore.SharedKernel
└── tests/         ← (chưa có file)
```

---

## BookStore.Domain

**Không có external dependency (Pure Domain).**

```
BookStore.Domain/
│
├── BookStore.Domain.csproj        ← Không package nào
│
├── Common/
│   ├── Identifiers/
│   │   └── StronglyTypedId.cs     ← abstract, Guid Value, sealed Equals/GetHashCode/ToString
│   │
│   ├── Intefaces/                 ← [NOTE: typo — thiếu 'r' trong Interfaces]
│   │   ├── IDomainEvent.cs        ← Guid EventId, DateTime OccurredOnUtc
│   │   └── IHasDomainEvents.cs    ← IReadOnlyList<IDomainEvent>, ClearDomainEvents()
│   │
│   ├── Primitives/
│   │   ├── AggregateRoot.cs       ← AggregateRoot<TId> : Entity<TId>, IHasDomainEvents
│   │   ├── DomainEvent.cs         ← abstract, implements IDomainEvent
│   │   ├── DomainException.cs     ← abstract : Exception
│   │   ├── Entity.cs              ← Entity<TId> where TId : StronglyTypedId
│   │   └── ValueObject.cs         ← abstract, GetEqualityComponents(), ==, !=
│   │
│   └── Services/
│       ├── IPasswordHasher.cs     ← Hash(string), Verify(string, string)
│       └── ISystemClock.cs        ← DateTime UtcNow { get; }
│
└── Users/
    ├── User.cs                    ← sealed partial AggregateRoot<UserId>
    │                                 State + Constructor(private) + Factory(Register) + Helpers + EventHelpers
    ├── User.Account.cs            ← partial — ConfirmEmail(DateTime)
    ├── User.Role.cs               ← partial — AssignRole(RoleId,DateTime), RevokeRole(RoleId,DateTime)
    ├── User.Secutiry.cs           ← partial — [TYPO] ChangePassword, RecordFailed/SuccessLogin, EnableTwoFactor, DisableTwoFactor
    ├── User.Lifecycle.cs          ← partial — Deactivate(DateTime), Reactivate(DateTime)
    │
    ├── ChildEntity/
    │   └── UserRole.cs            ← sealed Entity<RoleId>, RoleId + AssignedAt, static Create()
    │
    ├── Enums/
    │   ├── TwoFactorMethod.cs     ← Email=1, Totp=2
    │   └── UserStatus.cs          ← PendingVerification=0, Active=1, Locked=2, Deactivated=3
    │
    ├── Events/
    │   ├── RoleAssignedToUserDomainEvent.cs    ← UserId, RoleId
    │   ├── RoleRevokedFromUserDomainEvent.cs   ← UserId, RoleId
    │   ├── TwoFactorDisabledDomainEvent.cs     ← UserId
    │   ├── TwoFactorEnabledDomainEvent.cs      ← UserId, TwoFactorMethod
    │   ├── UserDeactivatedDomainEvent.cs        ← UserId
    │   ├── UserEmailConfirmedDomainEvent.cs     ← UserId
    │   ├── UserLockedOutDomainEvent.cs          ← UserId, LockoutEndUtc
    │   ├── UserPasswordChangedDomainEvent.cs    ← UserId
    │   ├── UserReactivatedDomainEvent.cs        ← UserId
    │   └── UserRegisteredDomainEvent.cs         ← UserId, Email
    │
    ├── Exceptions/
    │   ├── CannotRemoveLastRoleException.cs
    │   ├── DuplicateUserRoleException.cs
    │   ├── InvalidEmailException.cs
    │   ├── InvalidFullNameException.cs
    │   ├── InvalidPasswordException.cs
    │   ├── InvalidPhoneNumberException.cs
    │   ├── InvalidUserIdException.cs
    │   ├── PasswordMustBeDifferentException.cs
    │   ├── TwoFactorMethodRequiredException.cs
    │   ├── UserInactiveException.cs
    │   └── UserLockedException.cs
    │
    ├── Identifiers/
    │   ├── RoleId.cs              ← sealed StronglyTypedId, Create(Guid), New(), implicit→Guid
    │   └── UserId.cs              ← sealed StronglyTypedId, Create(Guid), New(), implicit→Guid
    │
    └── ValueObjects/
        ├── Email.cs               ← regex validate, Trim().ToLowerInvariant()
        ├── FullName.cs            ← MaxLength=100, Trim(), collapse whitespace
        ├── PasswordHash.cs        ← NotNullOrWhiteSpace, no normalize
        └── PhoneNumber.cs         ← regex ^\+?[0-9]{8,15}$, strip spaces/dashes/parens
```

---

## BookStore.Application

**Packages: MediatR 12.4.1, FluentValidation 12.1.1, Microsoft.Extensions.***

```
BookStore.Application/
│
├── BookStore.Application.csproj
│   └── ProjectRef → BookStore.Domain
│
├── Configuration/             ← TRỐNG (dự kiến: MediatR/FluentValidation DI setup)
│
├── Features/
│   └── Auth/                  ← TRỐNG (dự kiến: RegisterUserCommand, Handler, Validator)
│
└── Security/
    ├── Device/                ← TRỐNG
    ├── LockOut/               ← TRỐNG
    ├── Password/              ← TRỐNG
    └── UserName/              ← TRỐNG
```

---

## BookStore.Infrastructure

**Packages: EF Core 8, MassTransit.RabbitMQ, Redis, MailKit, Azure Blob**

```
BookStore.Infrastructure/
│
└── BookStore.Infrastructure.csproj
    ├── ProjectRef → BookStore.Application
    └── ProjectRef → BookStore.Domain
    [Toàn bộ các folder CHƯA TỒN TẠI — sẽ tạo theo roadmap]
    Dự kiến:
    ├── Persistence/
    │   ├── ApplicationDbContext.cs
    │   ├── Configurations/
    │   │   ├── UserConfiguration.cs
    │   │   └── UserRoleConfiguration.cs
    │   └── Repositories/
    │       └── UserRepository.cs
    ├── Security/
    │   └── BcryptPasswordHasher.cs
    ├── SystemClock/
    │   └── SystemClock.cs
    └── Outbox/
        └── OutboxMessage.cs
```

---

## BookStore.WebApi

**Packages: JWT, Serilog, AutoMapper, ApiVersioning, Swagger, FluentValidation.AspNetCore**

```
BookStore.WebApi/
│
├── BookStore.WebApi.csproj
│   └── ProjectRef → BookStore.Application
│
├── Program.cs                 ← Skeleton (AddControllers, Swagger, MapControllers)
├── appsettings.json
├── appsettings.Development.json
│
├── Controllers/               ← TRỐNG
│
├── Extensions/
│   ├── HostExtensions.cs
│   ├── ServiceExtensions.cs   ← ConfigureCors(), ConfigureSerilog()
│   └── Serilogger.cs
│
└── Properties/
    └── launchSettings.json
```

---

## BookStore.SharedKernel

```
BookStore.SharedKernel/
└── BookStore.SharedKernel.csproj  ← TRỐNG — dự phòng sau
```

---

## Dependency Graph

```
┌─────────────────────────────────────────┐
│              BookStore.WebApi            │
│  (JWT, Swagger, Serilog, AutoMapper)    │
└───────────────────┬─────────────────────┘
                    │ ProjectRef
                    ▼
┌─────────────────────────────────────────┐
│          BookStore.Application           │
│    (MediatR, FluentValidation)          │
└──────────┬──────────────────────────────┘
           │ ProjectRef
           ▼
┌─────────────────────────────────────────┐
│            BookStore.Domain              │
│         (Pure — No Dependencies)        │
└─────────────────────────────────────────┘
           ▲
           │ ProjectRef
┌──────────┴──────────────────────────────┐
│        BookStore.Infrastructure          │
│   (EF Core, Redis, RabbitMQ, MailKit)   │
└─────────────────────────────────────────┘
```

---

## Class Hierarchy

```
StronglyTypedId (abstract)
├── UserId (sealed)
└── RoleId (sealed)

ValueObject (abstract)
├── Email (sealed)
├── FullName (sealed)
├── PasswordHash (sealed)
└── PhoneNumber (sealed)

Entity<TId> (abstract, where TId : StronglyTypedId)
└── AggregateRoot<TId> (abstract, implements IHasDomainEvents)
    └── User (sealed partial)

Entity<RoleId>
└── UserRole (sealed) ← Child Entity của User

DomainEvent (abstract, implements IDomainEvent)
├── UserRegisteredDomainEvent
├── UserEmailConfirmedDomainEvent
├── UserPasswordChangedDomainEvent
├── UserLockedOutDomainEvent
├── RoleAssignedToUserDomainEvent
├── RoleRevokedFromUserDomainEvent
├── TwoFactorEnabledDomainEvent
├── TwoFactorDisabledDomainEvent
├── UserDeactivatedDomainEvent
└── UserReactivatedDomainEvent

DomainException (abstract : Exception)
├── CannotRemoveLastRoleException
├── DuplicateUserRoleException
├── InvalidEmailException
├── InvalidFullNameException
├── InvalidPasswordException
├── InvalidPhoneNumberException
├── InvalidUserIdException
├── PasswordMustBeDifferentException
├── TwoFactorMethodRequiredException
├── UserInactiveException
└── UserLockedException
```

---

## User Aggregate — Method Map

```
User (sealed partial)
│
├── [Factory]
│   └── static Register(UserId, Email, PasswordHash, FullName, PhoneNumber, RoleId, DateTime) → User
│
├── [Account Behavior] User.Account.cs
│   └── ConfirmEmail(DateTime confirmedAtUtc)
│
├── [Role Behavior] User.Role.cs
│   ├── AssignRole(RoleId roleId, DateTime utcNow)
│   └── RevokeRole(RoleId roleId, DateTime utcNow)
│
├── [Security Behavior] User.Secutiry.cs
│   ├── ChangePassword(PasswordHash passwordHash, DateTime utcNow)
│   ├── RecordFailedLogin(int maxAttempts, TimeSpan lockoutDuration, DateTime utcNow)
│   ├── RecordSuccessfulLogin(DateTime utcNow)
│   ├── EnableTwoFactor(TwoFactorMethod method, DateTime utcNow)
│   └── DisableTwoFactor(DateTime utcNow)
│
├── [Lifecycle Behavior] User.Lifecycle.cs
│   ├── Deactivate(DateTime utcNow)
│   └── Reactivate(DateTime utcNow)
│
├── [Private State Helpers] User.cs
│   ├── Touch(DateTime)
│   ├── HasRole(RoleId) → bool
│   ├── FindRole(RoleId) → UserRole?
│   ├── AddRole(UserRole)
│   ├── RemoveRole(UserRole)
│   ├── ResetFailedLoginState()
│   └── LockUntil(DateTime)
│
└── [Private Event Helpers] User.cs
    ├── RaiseRegisteredEvent()
    ├── RaiseEmailConfirmedEvent()
    ├── RaisePasswordChangedEvent()
    ├── RaiseLockedOutEvent()
    ├── RaiseRoleAssignedEvent(RoleId)
    ├── RaiseRoleRevokedEvent(RoleId)
    ├── RaiseTwoFactorEnabledEvent(TwoFactorMethod)
    ├── RaiseTwoFactorDisabledEvent()
    ├── RaiseUserDeactivatedEvent()
    └── RaiseUserReactivatedEvent()
```

---

## Namespace Map

| Namespace | Project | Folder |
|---|---|---|
| BookStore.Domain.Common.Identifiers | Domain | Common/Identifiers/ |
| BookStore.Domain.Common.Intefaces | Domain | Common/Intefaces/ |
| BookStore.Domain.Common.Primitives | Domain | Common/Primitives/ |
| BookStore.Domain.Common.Services | Domain | Common/Services/ |
| BookStore.Domain.Users | Domain | Users/ |
| BookStore.Domain.Users.ChildEntity | Domain | Users/ChildEntity/ |
| BookStore.Domain.Users.Enums | Domain | Users/Enums/ |
| BookStore.Domain.Users.Events | Domain | Users/Events/ |
| BookStore.Domain.Users.Exceptions | Domain | Users/Exceptions/ |
| BookStore.Domain.Users.Identifiers | Domain | Users/Identifiers/ |
| BookStore.Domain.Users.ValueObjects | Domain | Users/ValueObjects/ |
| BookStore.WebApi.Extensions | WebApi | Extensions/ |
