namespace RChat.UI.Common.ComponentHelpers
{
    public interface ISearchComponent
    {
        public string? Value { get; set; }
        public Task OnSearchChangeAsync();
    }
}
