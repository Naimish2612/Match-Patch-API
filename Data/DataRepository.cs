using DatingApp.API.Helper.Pageing;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly DataContext _dataContext;

        public DataRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }
        public void Add<T>(T entity) where T : class
        {
            this._dataContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            this._dataContext.Remove(entity);
        }

        public async Task<Photo> GetMainPhoto(int user_id)
        {
            return await _dataContext.Photos.Where(u => u.UserId == user_id).FirstOrDefaultAsync(p => p.is_main);
        }

        public async Task<Photo> GetPhoto(int photo_id)
        {
            var photo = await _dataContext.Photos.FirstOrDefaultAsync(p => p.Id == photo_id);

            return photo;
        }

        public async Task<User> GetUser(int user_id)
        {
            var user =await _dataContext.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Id == user_id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(PageBaseModel pageBaseModel)
        {
            //var users =await _dataContext.Users.Include(p => p.Photos).ToListAsync();
            //return users;

            var users =_dataContext.Users.Include(p => p.Photos);
            return await PagedList<User>.PagedListAsync(users,pageBaseModel.PageIndex,pageBaseModel.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
