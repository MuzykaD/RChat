using RChat.Domain.Messages.Dto;
using RChat.Domain.Users.DTO;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.ViewModels.Chat
{
    public class ChatViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public HashSet<UserInformationViewModel> Users { get; set; } = new();
        public List<MessageInformationDto>? Messages { get; set; } = new();
        public bool IsGroupChat { get; set; }
    }
}
