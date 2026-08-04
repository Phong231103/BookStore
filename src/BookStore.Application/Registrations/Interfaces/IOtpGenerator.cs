namespace BookStore.Application.Registrations.Interfaces;

public interface IOtpGenerator
{
    string Generate(int length = 6);
}