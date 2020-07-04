using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookieAuthN.Models
{
    public class User : IEquatable<User>
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool Equals(User compare)
        {
            if (compare == null)
            {
                return false;
            }
            if (compare.UserName != UserName || compare.Password != Password)
            {
                return false;
            }

            //This is ignoring the data of birth and everything else....lalal!

            return true;
        }

    }

}
