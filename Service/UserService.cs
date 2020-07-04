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

        public UserService(User[] users)
        {
            _users = users;
        }

        public Task<bool> ValidateUserCredentialsAsync(User user)
        {
            return Task.FromResult(_users.Any(user.Equals));
        }

        public Task<bool> ValidateUserCredentialsAsync(string UserName, string Password)
        {
            var user = new User() {UserName = UserName, Password = Password};

            return ValidateUserCredentialsAsync(user);
        }

        public Task<User> GetUser(string UserName)
        {
            if (_users == null || !_users.Any())
            {
                return Task.FromResult(new User());
            }
            var user = _users.FirstOrDefault(x => x.UserName == UserName) ?? new User();
            return Task.FromResult(user);
        }
    }
}
