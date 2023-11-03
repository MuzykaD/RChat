namespace RChat.UI.Common.ComponentHelpers
{
    public interface IFormComponentBase<T>
    {
         T ViewModel { get; set; }
         Task SubmitFormAsync();
    }
}
