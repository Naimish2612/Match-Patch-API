using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data.Auth;
using DatingApp.API.DTO.Auth;
using DatingApp.API.DTO.Users;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configration;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository authRepository, IConfiguration configration, IMapper mapper)
        {
            this._authRepository = authRepository;
            this._configration = configration;
            this._mapper = mapper;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(UserDto userDto)
        {
            userDto.user_name = userDto.user_name.ToLower();

            if (await _authRepository.UserExists(userDto.user_name))
                return BadRequest("Username already exists");

            //var userToCreate = new User
            //{
            //    user_name = userDto.user_name
            //};

            var userToCreate = _mapper.Map<User>(userDto);

            var createdUser = await _authRepository.SignUp(userToCreate, userDto.password);
            var userToReturn = _mapper.Map<UserDto>(createdUser);
            return CreatedAtRoute("GetUser", new { controller = "User", id = createdUser.Id }, userToReturn);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var userRepo = await _authRepository.Login(loginDto.user_name, loginDto.password);

            if (userRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userRepo.user_name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = creds
            };

            var tokenHendler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHendler.CreateToken(tokenDescripter);

            var user = _mapper.Map<UserListDto>(userRepo);

            return Ok(new { token = tokenHendler.WriteToken(token), user });
        }
    }
}