﻿using DatingApp.API.Helper.Pageing;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public interface IDataRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();

        Task<PagedList<User>> GetUsers(PageBaseModel pageBaseModel);
        Task<User> GetUser(int user_id);
        Task<Photo> GetPhoto(int photo_id);

        Task<Photo> GetMainPhoto(int user_id);
    }
}
