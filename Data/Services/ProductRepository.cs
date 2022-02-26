using System;
using System.Linq;
using System.Threading.Tasks;
using KlicKitApi.Helpers;
using KlicKitApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KlicKitApi.Data.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProduct(Guid id)
        {
            var product = await _context.Products
                                .Include(us => us.Users)
                                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public async Task<PagedList<Product>> GetProducts(ProductParams productParams)
        {
            var products = _context.Products
                            .Include(us => us.Users)
                            .AsQueryable();                             
            
            if (!string.IsNullOrEmpty(productParams.OrderBy))
            {
                switch (productParams.OrderBy)
                {
                    case "price":
                        products = products.OrderByDescending(p => p.Price);
                        break;
                    default:                        
                        break;
                }
            }

            return await PagedList<Product>.CreateAsync(products, productParams.PageNumber, productParams.PageSize);
        }

        public async Task<PagedList<UserProducts>> GetUserProducts(UserProductsParams userProductsParams)
        {
            var userProducts = _context.UserProducts.AsQueryable();  

            if(userProductsParams.UserId != null)
                userProducts = userProducts.Where(up => up.UserId != userProductsParams.UserId);

            if (!string.IsNullOrEmpty(userProductsParams.OrderBy))
            {
                switch (userProductsParams.OrderBy)
                {
                    case "RequestTime":
                        userProducts = userProducts.OrderByDescending(p => p.RequestTime);
                        break;
                    default:                        
                        break;
                }
            }
            return await PagedList<UserProducts>.CreateAsync(userProducts, userProductsParams.PageNumber, userProductsParams.PageSize);
        }

        public async Task<PagedList<UserProducts>> GetUsersRequests(UserProductsParams userProductsParams)
        {
            var usersRequests = _context.UserProducts
                                .Include(us => us.User)
                                .Include(pr => pr.Product)
                                .Where(up => up.IsChecked == false).AsQueryable();                             
            
            if (!string.IsNullOrEmpty(userProductsParams.OrderBy))
            {
                switch (userProductsParams.OrderBy)
                {
                    case "RequestTime":
                        usersRequests = usersRequests.OrderByDescending(p => p.RequestTime);
                        break;
                    default:                        
                        break;
                }
            }
            return await PagedList<UserProducts>.CreateAsync(usersRequests, userProductsParams.PageNumber, userProductsParams.PageSize);
        }

        public async Task<UserProducts> GetUserProduct(Guid id)
        {
            var userProduct = await _context.UserProducts.FirstOrDefaultAsync(p => p.Id == id);
            return userProduct;
        }

    }
}