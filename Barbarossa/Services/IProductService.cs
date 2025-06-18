using Barbarossa.Models;
using System.Collections.ObjectModel;

namespace Barbarossa.Services
{
    public interface IProductService
    {
        Task<ObservableCollection<Product>> GetProductsAsync();
    }
}