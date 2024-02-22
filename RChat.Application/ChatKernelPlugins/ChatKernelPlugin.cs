using Microsoft.SemanticKernel;
using RChat.Domain.Chats;
using RChat.Domain.Messages;
using RChat.Domain.Users;
using RChat.Infrastructure.Contracts.Common;
using RChat.Infrastructure.Contracts.UnitOfWork;
using System.ComponentModel;


namespace RChat.Application.ChatKernelPlugins
{
    public sealed class ChatKernelPlugin
    {
        private IUnitOfWork _unitOfWork;
        private IRepository<User, int> _userRepository;
        private IRepository<Message, int> _messageRepository;
        private IRepository<Chat, int> _chatRepository;


        public ChatKernelPlugin(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<User, int>();
            _messageRepository = _unitOfWork.GetRepository<Message, int>();
            _chatRepository = _unitOfWork.GetRepository<Chat, int>();
        }

        [KernelFunction, Description("Send given message to user with given name")]
        public async Task<bool> SendMessageToUserByNameAsync(
               [Description("The body of message to send")] string messageInput,
               [Description("The name of the user to send a message")] string userName,
               [Description("My Id")] int myId)
        {
            var user = await _userRepository.GetByConditionAsync(u => u.UserName == userName && u.Chats.Any(c => c.Users.Count == 2 && c.Users.Any(u => u.Id == myId)));
            if (user == null || user.UserName != userName)
                return false;
            var requiredChat = _chatRepository.GetAllIncluding(c => c.Users, c => c.Messages, c => c.Assistant)
                .FirstOrDefault(
                c => !c.IsGroupChat &&
                c.Users.All(u => u.Id == myId || u.Id == user.Id));
            if (requiredChat == null) return false;

            var message = new Message
            {
                SenderId = myId,
                ChatId = requiredChat.Id,
                SentAt = DateTime.Now,
                IsAssistantGenerated = true,
                Content = messageInput,
            };
            await _messageRepository.CreateAsync(message);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
