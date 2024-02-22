namespace RChat.UI.Services.JarvisService
{
    public interface IJarvisService
    {
        Task<string> SendMessageToJarvisAsync(string message);
    }
}
