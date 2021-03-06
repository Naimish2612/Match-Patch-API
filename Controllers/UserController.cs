﻿using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTO.Users;
using DatingApp.API.Helper;
using DatingApp.API.Helper.Pageing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;

        public UserController(IDataRepository dataRepository, IMapper mapper)
        {
            this._dataRepository = dataRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]PageBaseModel pageBaseModel)
        {
            var users = await _dataRepository.GetUsers(pageBaseModel);

            var user_to_returm = _mapper.Map<IEnumerable<UserListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount,
                users.TotalPages, users.HasPreviousPage, users.HasNextPage);

            return Ok(user_to_returm);
        }

        [HttpGet("{id}", Name = "GetUser")]
        //[Route("getuser")]
        public async Task<IActionResult> GetUser(int Id)
        {
            var user = await _dataRepository.GetUser(Id);

            var user_to_return = _mapper.Map<UserDetailDto>(user);

            return Ok(user_to_return);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserProfile(int id, UserUpdateDto userUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _dataRepository.GetUser(id);
            _mapper.Map(userUpdateDto, userFromRepo);

            if (await _dataRepository.SaveAll())
                return NoContent();

            throw new Exception($"while updating user failed on save.");
        }
    }
}