using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieAuthN.Models;

namespace CookieAuthN.Service
{
    public class UserService : IUserService
    {
        private readonly User[] _users;

        public Task<bool> ValidateUserCredentialsAsync(User user)
        {
            return Task.FromResult(_users.Any(x => x == user));
        }
    }
}
