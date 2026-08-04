using BookStore.Application.Registrations.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace BookStore.Infrastructure.Security.Otp;

internal sealed class HmacOtpHasher : IOtpHasher
{
    private readonly byte[] _secret;

    public HmacOtpHasher(IOptions<OtpHashOptions> options)
    {
        _secret = Encoding.UTF8.GetBytes(options.Value.SecretKey);
    }

    public string Hash(string otp)
    {
        using var hmac = new HMACSHA256(_secret);

        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(otp));

        return Convert.ToBase64String(hash);
    }

    public bool Verify(string otp, string hash)
    {
        string computed = Hash(otp);

        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(computed),
            Convert.FromBase64String(hash));
    }
}