using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helper;
using API.Interfaces;
using API.Repository.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController(IUnitOfWork unitOfWork,
        IMapper mapper) : BaseController
    {

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var userName = User.GetUserName();

            if (userName == createMessageDto.RecipientUserName.ToLower())
            {
                return BadRequest("You cannot message yourself");
            }

            var sender = await unitOfWork.UserRepository.GetByUserNameAsync(userName);
            var recipient = await unitOfWork.UserRepository.GetByUserNameAsync(createMessageDto.RecipientUserName);

            if (recipient == null || sender == null) 
            {
                return BadRequest("Cannot send message at this time");
            }

            var message = new Message
            {
                Content = createMessageDto.Content,
                Recipient = recipient,
                Sender = sender,
                SenderUserName = userName,
                RecipientUserName = createMessageDto.RecipientUserName,
            };

            unitOfWork.MessageRepository.AddMessage(message);

            if (await unitOfWork.Complete()) return Ok(mapper.Map<MessageDto>(message));

            return BadRequest("Message failed to send");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.UserName = User.GetUserName();

            var messages = await unitOfWork.MessageRepository.GetMessageForUser(messageParams);

            Response.AddPaginationHeader(messages);

            return Ok(messages);
        }

        [HttpGet("thread/{recipientUserName}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string recipientUserName)
        {
            var currentUserName = User.GetUserName();
            var messages = await unitOfWork.MessageRepository.GetMessageThread(currentUserName, recipientUserName);
            return Ok(messages);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userName = User.GetUserName();

            if (userName == null) return BadRequest("User not found");

            var message = await unitOfWork.MessageRepository.GetMessage(id);

            if (message is null) return BadRequest("Message is not found");

            if (message.SenderUserName != userName && message.RecipientUserName != userName) return Forbid("You are not authorized to delete the message");

            if (message.SenderUserName == userName) message.SenderDeleted = true;

            if (message.RecipientUserName == userName) message.RecipientDeleted = true;

            if(message is { SenderDeleted: true, RecipientDeleted: true })
            {
                unitOfWork.MessageRepository.DeleteMessage(message);
            }

            if (await unitOfWork.Complete()) return Ok();

            return BadRequest("Message deleting is failed");
        }
    }
}
