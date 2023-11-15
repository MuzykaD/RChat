using RChat.Application.Common;
using RChat.Application.Contracts.Chats;
using RChat.Application.Contracts.Common;
using RChat.Domain;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Dto;
using RChat.Domain.Repsonses;
using RChat.Domain.Users;
using RChat.Infrastructure.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Chats
{
    public class ChatService : IChatService
    {
        private IUnitOfWork _unitOfWork;

        public ChatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreatePublicGroupAsync(string groupName, int creatorId, IEnumerable<int> membersId)
        {
            var userRepo = _unitOfWork.GetRepository<User, int>();
            var chatRepos = _unitOfWork.GetRepository<Chat, int>();
            var chatUsers = await userRepo.GetAllAsync(u => membersId.Contains(u.Id));
            var chat = new Chat()
            {
                Name = groupName,
                IsGroupChat = true,
                Users = chatUsers.ToList(),
                CreatorId = creatorId,
            };
            await chatRepos.CreateAsync(chat);
            await _unitOfWork.SaveChangesAsync();
            return chat.Id != 0;
        }

        public async Task<GridListDto<ChatInformationDto>> GetChatsInformationListAsync(SearchArguments searchArguments)
        {
            var chatRepository = _unitOfWork.GetRepository<Chat, int>();
            var infoQuery = chatRepository
                .GetAllAsQueryable()
                .Select(c =>
            new ChatInformationDto()
            {
                Id = c.Id,
                Name = c.Name,
                CreatorName = c.Creator == null ? null : c.Creator.UserName,
                UsersCount = c.Users.Count(),
                IsGroup = c.IsGroupChat,
            });
            if (searchArguments.SearchRequired)
                infoQuery = QueryBuilder<ChatInformationDto>.BuildSearchQuery(infoQuery, searchArguments.Value!);
            var totalCount = infoQuery.Count();
            if (searchArguments.OrderByRequired)
                infoQuery = QueryBuilder<ChatInformationDto>.BuildOrderByQuery(infoQuery, searchArguments.OrderBy!, searchArguments.OrderByType!);

            return new GridListDto<ChatInformationDto>()
            {
                SelectedEntities = infoQuery.Skip(searchArguments.Skip).Take(searchArguments.Take).ToList(),
                TotalCount = totalCount
            };
        }

        public async Task<Chat> GetGroupChatByIdAsync(int currentUserId,int chatId)
        {
            var chatRepository = _unitOfWork.GetRepository<Chat, int>();
            var chat =  chatRepository
                .GetAllIncluding(c => c.Users, c => c.Messages)
                .FirstOrDefault(c => c.Id == chatId);
            if (!chat.Users.Any(c => c.Id == currentUserId))
                await AddUserToGroupChat(currentUserId, chatId);
            return await Task.FromResult(chat);
        }

        private async Task AddUserToGroupChat(int currentUserId, int chatId)
        {
            var userRepo = _unitOfWork.GetRepository<User, int>();
            var chatRepos = _unitOfWork.GetRepository<Chat, int>();
            var user = await userRepo.GetByIdAsync(currentUserId);
            var chat =  chatRepos
                .GetAllIncluding(c => c.Users)
                .FirstOrDefault(c => c.Id == chatId);
            chat.Users.Add(user);
            chatRepos.Update(chat);
            await _unitOfWork.SaveChangesAsync();   
        }

        /// <summary>
        /// Get`s private chat with users by their id
        /// Creates a new private chat between users if does not exist
        /// </summary>
        /// <param name="currentUserId">Logged in user id</param>
        /// <param name="secondUserId">Second user for private chat</param>
        /// <returns> required private chat </returns>
        public async Task<Chat?> GetPrivateChatByUsersIdAsync(int currentUserId, int secondUserId)
        {
            var chatRepository = _unitOfWork.GetRepository<Chat, int>();
            var requiredChat = chatRepository.GetAllIncluding(c => c.Users, c => c.Messages).FirstOrDefault(
                c => !c.IsGroupChat &&
                c.Users.All(u => u.Id == currentUserId || u.Id == secondUserId));

            if (requiredChat == null)
            {
                var newChat = new Chat()
                {
                    IsGroupChat = false,
                    Users = _unitOfWork.GetRepository<User, int>()
                    .GetAllAsQueryable()
                    .Where(u => u.Id == currentUserId || u.Id == secondUserId).ToList()
                };
                newChat.Name = $"{string.Join("-", newChat.Users.Select(u => u.UserName))}";
                await chatRepository.CreateAsync(newChat);
                await _unitOfWork.SaveChangesAsync();
                return newChat;
            }
            return requiredChat;
        }
    }
}
