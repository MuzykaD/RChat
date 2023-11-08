using Radzen;

namespace RChat.UI.Common.ComponentHelpers
{
    public interface IListComponentBase<T>    
    {
        public IEnumerable<T> EntityList { get; set; }
        protected int Count { get; set; }
        protected int Size { get; set; }
        protected Task OnPageChangedAsync(PagerEventArgs args);
    }
}
