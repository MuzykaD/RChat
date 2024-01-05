using RChat.Application.Common;
using RChat.Application.Contracts.Assistant;
using RChat.Application.Contracts.Chats;
using RChat.Application.Contracts.Common;
using RChat.Domain;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Dto;
using RChat.Domain.Repsonses;
using RChat.Domain.Users;
using RChat.Infrastructure.Contracts.Common;
using RChat.Infrastructure.Contracts.UnitOfWork;
using System.Data.Entity;


namespace RChat.Application.Chats
{
    public class ChatService : IChatService
    {
        private IUnitOfWork _unitOfWork;
        private IAssistantFactory _assistantFactory;
        private IRepository<Chat, int> _chatRepository;
        private IRepository<User, int> _userRepository;

        public ChatService(IUnitOfWork unitOfWork, IAssistantFactory factory)
        {
            _unitOfWork = unitOfWork;
            _chatRepository = _unitOfWork.GetRepository<Chat, int>();
            _userRepository = _unitOfWork.GetRepository<User, int>();
            _assistantFactory = factory;
        }

        public async Task<bool> CreatePublicGroupAsync(string groupName, int creatorId, IEnumerable<int> membersId)
        {

            var chatUsers = await _userRepository.GetAllAsync(u => membersId.Contains(u.Id));
            var assistantResponse = await _assistantFactory.CreateAssistantForGroupAsync(groupName);

            var chat = new Chat()
            {
                Name = groupName,
                IsGroupChat = true,
                Users = chatUsers.ToList(),
                CreatorId = creatorId,
                Assistant = new()
                {
                    Id = assistantResponse.Id,
                    Name = assistantResponse.Name,
                    Instructions = assistantResponse.Instructions
                }
            };

            await _chatRepository.CreateAsync(chat);
            await _unitOfWork.SaveChangesAsync();
            return chat.Id != 0;
        }

        public async Task<GridListDto<ChatInformationDto>> GetChatsInformationListAsync(SearchArguments searchArguments)
        {
            var infoQuery = _chatRepository
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

        public async Task<Chat> GetGroupChatByIdAsync(int currentUserId, int chatId)
        {
            var chat = _chatRepository
                .GetAllIncluding(c => c.Users, c => c.Messages, c => c.Assistant)
                .FirstOrDefault(c => c.Id == chatId);

            if (!chat.Users.Any(c => c.Id == currentUserId))
                await AddUserToGroupChat(currentUserId, chatId);

            return await Task.FromResult(chat);
        }

        private async Task AddUserToGroupChat(int currentUserId, int chatId)
        {
            var user = await _userRepository.GetByIdAsync(currentUserId);
            var chat = _chatRepository
                .GetAllIncluding(c => c.Users)
                .FirstOrDefault(c => c.Id == chatId);

            chat.Users.Add(user);

            _chatRepository.Update(chat);
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

            var requiredChat = _chatRepository.GetAllIncluding(c => c.Users, c => c.Messages, c => c.Assistant).FirstOrDefault(
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
                var assistantResponse = await _assistantFactory.CreateAssistantForGroupAsync(newChat.Name);
                newChat.Assistant = new() { Id = assistantResponse.Id, Name = assistantResponse.Name, Instructions = assistantResponse.Instructions };
                await _chatRepository.CreateAsync(newChat);
                await _unitOfWork.SaveChangesAsync();
                return newChat;
            }
            return requiredChat;
        }

        public async Task<IEnumerable<int>> GetGroupsIdentifiersAsync(int currentUserId)
        {
            var result = _chatRepository
                .GetAllIncluding(c => c.Users)
                .Where(c => c.Users.Any(u => u.Id.Equals(currentUserId)))
                .Select(c => c.Id).ToList();

            return await Task.FromResult(result);
        }
    }
}
