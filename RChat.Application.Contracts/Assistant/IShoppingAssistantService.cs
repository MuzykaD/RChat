using RChat.Domain.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Assistant
{
    public interface IShoppingAssistantService : IAssistantInitializer
    {
        public Task<List<ShoppingProduct>> GetListOfShoppingProductsAsync(string htmlCards);
        public Task<List<ShoppingProduct>> GetListOfShoppingProductsAsync(string[] htmlCards);
    }
}
