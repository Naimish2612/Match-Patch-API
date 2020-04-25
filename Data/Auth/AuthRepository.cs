using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;

        public AuthRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<User> Login(string user_name, string password)
        {
            user_name = user_name.ToLower();
            var user =await _dataContext.Users.Include(p=>p.Photos).FirstOrDefaultAsync(x => x.user_name == user_name);

            if (user == null)
                return null;
            if (!VerifyPassowrdHash(password, user.passowrd_hash, user.passowrd_salt))
                return null;

            return user;
        }

        private bool VerifyPassowrdHash(string password, byte[] passowrd_hash, byte[] passowrd_salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passowrd_salt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (computeHash[i] != passowrd_hash[i]) return false;
                }
            }
            return true;
        }

        public async Task<User> SignUp(User user, string password)
        {
            byte[] password_hash, password_salt;
            CreatePasswordHash(password, out password_hash, out password_salt);

            user.passowrd_hash = password_hash;
            user.passowrd_salt = password_salt;
            user.create_at = DateTime.Now;

            await _dataContext.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] password_hash, out byte[] password_salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                password_salt = hmac.Key;
                password_hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string user_name)
        {
            if (await _dataContext.Users.AnyAsync(x => x.user_name == user_name))
                return true;

            return false;

        }
    }
}
