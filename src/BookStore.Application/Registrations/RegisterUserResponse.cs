namespace BookStore.Application.Registration
{
    public sealed record RegisterUserResponse(Guid Id, string Email, string FullName);
}
