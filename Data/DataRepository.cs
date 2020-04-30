using CloudinaryDotNet.Actions;
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

        public async Task<Like> GetLike(int user_id, int recipient_id)
        {
            return await _dataContext.Likes.FirstOrDefaultAsync(l => l.liker_id == user_id && l.likee_id == recipient_id);
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
            var user = await _dataContext.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Id == user_id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(PageBaseModel pageBaseModel)
        {
            //var users =await _dataContext.Users.Include(p => p.Photos).ToListAsync();
            //return users;

            var users = _dataContext.Users.Include(p => p.Photos).AsQueryable();
            users = users.Where(u => u.Id != pageBaseModel.user_id && u.gender == pageBaseModel.gender);

            if (pageBaseModel.likers || pageBaseModel.likees)
            {
                var userLikers = await GetUserLikes(pageBaseModel.user_id, pageBaseModel.likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }
            //if (pageBaseModel.likees)
            //{
            //    var userLikees = await GetUserLikes(pageBaseModel.user_id, pageBaseModel.likers);
            //    users = users.Where(u => userLikees.Contains(u.Id));
            //}

            if (pageBaseModel.min_age != 18 || pageBaseModel.max_age != 99)
            {
                var minDob = DateTime.Today.AddYears(-pageBaseModel.max_age - 1);
                var maxDob = DateTime.Today.AddYears(-pageBaseModel.min_age);

                users = users.Where(u => u.date_of_birth >= minDob && u.date_of_birth <= maxDob);
            }


            return await PagedList<User>.PagedListAsync(users, pageBaseModel.PageIndex, pageBaseModel.PageSize);
        }

        private async Task<IEnumerable<int>> GetUserLikes(int user_id, bool likers)
        {
            var users =await _dataContext.Users.Include(x => x.likers).Include(x => x.likees).FirstOrDefaultAsync(u => u.Id == user_id);

            if (likers)
                return users.likers.Where(x => x.likee_id == user_id).Select(x => x.liker_id);
            else
                return users.likees.Where(x => x.liker_id == user_id).Select(x => x.likee_id);
        }

        public async Task<bool> SaveAll()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
