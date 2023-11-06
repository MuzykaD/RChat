namespace RChat.UI.Common.ComponentHelpers
{
    public interface IListComponentBase<T>    
    {
        public IEnumerable<T> EntityList { get; set; }
        public string? SearchValue { get; set; }
        protected int Count { get; set; }
        protected int PageSize { get; set; }
    }
}
