using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieAuthN.Models;

namespace CookieAuthN.Service
{
    public interface IUserService
    {
        Task<bool> ValidateUserCredentialsAsync(User user);

    }
}
