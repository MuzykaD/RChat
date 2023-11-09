namespace RChat.UI.ViewModels.InformationViewModels
{
    public class MessageInformationViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public int SenderId { get; set; }
        public int ChatId { get; set; }
    }
}
