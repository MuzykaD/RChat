namespace RChat.UI.Common.ComponentHelpers
{
    public interface IListComponentBase<T>    
    {
        public IEnumerable<T> EntityList { get; set; }
    }
}
