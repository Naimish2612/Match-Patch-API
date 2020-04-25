using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data.Auth
{
    public interface IAuthRepository
    {
        Task<User> SignUp(User user, string password);
        Task<User> Login(string user_name, string password);
        Task<bool> UserExists(string user_name);
    }
}
