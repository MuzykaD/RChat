namespace RChat.UI.ViewModels.InformationViewModels
{
    public class UserInformationViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "Loading";
        public string Email { get; set; } = "Loading";
        public string? PhoneNumber { get; set; } = "Loading";
    }
}
