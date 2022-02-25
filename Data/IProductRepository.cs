using System;
using System.Threading.Tasks;
using KlicKitApi.Helpers;
using KlicKitApi.Models;

namespace KlicKitApi.Data
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(Guid id);
        Task<PagedList<Product>> GetProducts(ProductParams userParams);
        Task<PagedList<UserProducts>> GetUserProducts(UserProductsParams userParams);
        Task<PagedList<UserProducts>> GetUsersRequests(UserProductsParams userParams);
        Task<UserProducts> GetUserProduct(Guid id);
    }
}