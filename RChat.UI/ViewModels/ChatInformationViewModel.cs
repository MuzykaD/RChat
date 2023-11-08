namespace RChat.UI.ViewModels
{
    public class ChatInformationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public int UsersCount { get; set; }
        public int MessagesCount { get; set; }
    }
}
