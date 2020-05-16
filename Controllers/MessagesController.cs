using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTO.Message;
using DatingApp.API.Helper;
using DatingApp.API.Helper.MessageHelper;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogActivity))]
    [Authorize]
    [Route("api/user/{user_id}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;

        public MessagesController(IDataRepository dataRepository, IMapper mapper)
        {
            this._dataRepository = dataRepository;
            this._mapper = mapper;
        }

        [HttpGet("{message_id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int user_id, int message_id)
        {
            if (user_id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _dataRepository.GetMessage(message_id);

            if (messageFromRepo == null)
                return NotFound("Message not found.");

            return Ok(messageFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessageForUser(int user_id, [FromQuery]MessageParam messageParam)
        {
            if (user_id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParam.user_id = user_id;

            var messageFromRepo = await _dataRepository.GetMessages(messageParam);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messageFromRepo);

            //set pagination in header
            Response.AddPagination(messageFromRepo.CurrentPage, messageFromRepo.PageSize, messageFromRepo.TotalCount,
                messageFromRepo.TotalPages, messageFromRepo.HasPreviousPage, messageFromRepo.HasNextPage);

            return Ok(messages);
        }

        [HttpGet("thread/{recipient_id}")]
        public async Task<IActionResult> GetMessageThread(int user_id, int recipient_id)
        {
            try
            {
                if (user_id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

                var messageFromRepo = await _dataRepository.GetMessages(user_id, recipient_id);

                var messageToReturn = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messageFromRepo);

                return Ok(messageToReturn);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int user_id, MessageCreateDTO messageCreateDTO)
        {
            var sender = await _dataRepository.GetUser(user_id);

            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageCreateDTO.senderId = user_id;

            var recipient = await _dataRepository.GetUser(messageCreateDTO.recipientId);

            if (recipient == null)
                return BadRequest("Could not find user.");

            var message = _mapper.Map<Message>(messageCreateDTO);

            _dataRepository.Add(message);


            if (await _dataRepository.SaveAll())
            {
                var return_response = _mapper.Map<MessageToReturnDTO>(message);
                return CreatedAtRoute("GetMessage", new { message_id = message.Id }, return_response);
            }

            throw new Exception("Creating the message faild on save.");
        }

        [HttpPost("{message_id}")]
        public async Task<IActionResult> MessageDeleted(int message_id, int user_id)
        {
            if (user_id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _dataRepository.GetMessage(message_id);

            if (messageFromRepo.senderId == user_id)
                messageFromRepo.sender_deleted = true;

            if (messageFromRepo.recipientId == user_id)
                messageFromRepo.recipient_deleted = true;

            if (messageFromRepo.sender_deleted && messageFromRepo.recipient_deleted)
                _dataRepository.Delete(messageFromRepo);

            if (await _dataRepository.SaveAll())
                return NoContent();

            throw new Exception("Error while delete message.");

        }

        [HttpPost("{message_id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int message_id,int user_id)
        {
            if (user_id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await _dataRepository.GetMessage(message_id);

            if (message.recipientId != user_id)
                return Unauthorized();

            message.is_read = true;
            message.read_date = DateTime.Now;

            await _dataRepository.SaveAll();

            return NoContent();
        }
    }
}