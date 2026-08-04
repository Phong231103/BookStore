using BookStore.Application.Registrations.Interfaces;
using System.Security.Cryptography;

namespace BookStore.Infrastructure.Security.Otp;

internal sealed class RandomOtpGenerator : IOtpGenerator
{
    public string Generate(int length = 6)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

        Span<char> chars = stackalloc char[length];

        for (int i = 0; i < length; i++)
        {
            chars[i] = (char)('0' + RandomNumberGenerator.GetInt32(10));
        }

        return new string(chars);
    }
}