﻿using API.Data;
using API.DTOs;
using API.Entities;
using API.Helper;
using API.Repository.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class MessageRepository(DataContext dataContext, IMapper mapper) : IMessageRepository
    {
        public void AddMessage(Message message)
        {
            dataContext.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            dataContext.Messages.Remove(message);
        }

        public async Task<Message?> GetMessage(int id)
        {
            return await dataContext.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = dataContext.Messages.OrderByDescending(x=>x.MessageSent).AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.UserName && x.RecipientDeleted == false),
                "Outbox" => query.Where(x => x.Sender.UserName == messageParams.UserName && x.SenderDeleted == false),
                _ => query.Where(x => x.Recipient.UserName == messageParams.UserName && x.DateRead == null)
            };

            var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName)
        {
            var messages = await dataContext.Messages
                    .Include(x => x.Sender).ThenInclude(x => x.Photos)
                    .Include(x => x.Recipient).ThenInclude(x => x.Photos)
                    .Where(
                            x => x.RecipientUserName == currentUserName && x.RecipientDeleted == false && x.SenderUserName == recipientUserName ||
                            x.SenderUserName == currentUserName && x.SenderDeleted == false && x.RecipientUserName == recipientUserName
                          )
                    .OrderBy(x => x.MessageSent).ToListAsync();

            var unreadMessage = messages.Where(x => x.RecipientUserName == currentUserName && x.DateRead == null).ToList();

            if (unreadMessage.Any())
            {
                unreadMessage.ForEach(x => x.DateRead = DateTime.UtcNow);
                await dataContext.SaveChangesAsync();
            }

            return mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllChangeAsync()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }
    }
}
