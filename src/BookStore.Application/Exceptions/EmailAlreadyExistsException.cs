namespace BookStore.Application.Exceptions
{
    public sealed class EmailAlreadyExistsException : ApplicationException
    {
        public EmailAlreadyExistsException()
            : base("Email is already registered.")
        {
        }
    }
}
