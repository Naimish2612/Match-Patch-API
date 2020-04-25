using DatingApp.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static void SeedUsers(DataContext dataContext)
        {
            if (!dataContext.Users.Any())
            {
                //read user seed
                var userData = System.IO.File.ReadAllText("Data/user_data_seeds.json");
                //convert user seed to user object
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    byte[] password_hash, password_salt;

                    CreatePasswordHash("password", out password_hash, out password_salt);
                    user.passowrd_hash = password_hash;
                    user.passowrd_salt = password_salt;
                    user.user_name = user.user_name.ToLower();
                    user.create_at = DateTime.Now;
                    dataContext.Add(user);
                }
                dataContext.SaveChanges();
            }
        }


        private static void CreatePasswordHash(string password, out byte[] password_hash, out byte[] password_salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                password_salt = hmac.Key;
                password_hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
