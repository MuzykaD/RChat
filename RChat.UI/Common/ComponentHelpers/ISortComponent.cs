namespace RChat.UI.Common.ComponentHelpers
{
    public interface ISortComponent
    {
        protected IEnumerable<string> SortingFieldDropDown { get; set; }
        protected IEnumerable<string> SortingTypeDropDown { get; set; }
        protected string OrderBy { get; set; }
        protected string OrderByType { get; set; }
        protected bool IsSortingDisabled { get; set; }
        public Task OnSortSwitchAsync();
    }
}
