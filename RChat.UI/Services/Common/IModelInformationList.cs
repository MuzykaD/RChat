using RChat.Domain.Repsonses;
using RChat.UI.Common;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Services.Common
{
    public interface IModelInformationList<T>
    {
        Task<ApiRequestResult<GridListDto<T>>> GetInformationListAsync(int page, int size, string? value = null, string? orderBy = null, string? orderByType = null);
    }
}
