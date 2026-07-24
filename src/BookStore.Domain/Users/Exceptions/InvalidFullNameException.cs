using BookStore.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class InvalidFullNameException : DomainException
    {
        public InvalidFullNameException(string fullName)
            : base($"'{fullName}' is not a valid full name.")
        {
        }
    }
}
