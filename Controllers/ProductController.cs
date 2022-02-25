using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using KlicKitApi.Data;
using KlicKitApi.Dtos;
using KlicKitApi.Helpers;
using KlicKitApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KlicKitApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        public readonly IProductRepository _product;
        public readonly IAuthRepository _auth;
        
        

        public ProductController(DataContext dbContext, ILogger<ProductController> logger,
                                 IMapper mapper, IProductRepository product, IAuthRepository auth)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _product = product;
            _auth = auth;
        }
        
        

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var product = await _product.GetProduct(id);

            var productToReturn = _mapper.Map<ProductForDetailedDto>(product);

            return Ok(productToReturn);
        }


        [HttpGet("GetProductList")]
        public async Task<IActionResult> GetProducts([FromQuery]ProductParams productParams)
        {
            var products = await _product.GetProducts(productParams);

            var productsToReturn = _mapper.Map<IEnumerable<ProductForListDto>>(products);

            Response.AddPagination(products.CurrentPage, products.PageSize,
                products.TotalCount, products.TotalPages);

            return Ok(productsToReturn);
        }

        [HttpGet("GetUserProductsList")]
        public async Task<IActionResult> GetUserProductsList([FromQuery]UserProductsParams productParams)
        {
            var userProducts = await _product.GetUserProducts(productParams);

            var userProductsToReturn = _mapper.Map<IEnumerable<UserProductsForListDto>>(userProducts);

            Response.AddPagination(userProducts.CurrentPage, userProducts.PageSize,
                userProducts.TotalCount, userProducts.TotalPages);

            return Ok(userProductsToReturn);
        }

        [HttpGet("GetUsersRequestsList")]
        public async Task<IActionResult> GetUsersRequestsList([FromQuery]UserProductsParams productParams)
        {
            var usersRequests = await _product.GetUsersRequests(productParams);

            var usersRequestsToReturn = _mapper.Map<IEnumerable<UserProductsForListDto>>(usersRequests);

            Response.AddPagination(usersRequests.CurrentPage, usersRequests.PageSize,
                usersRequests.TotalCount, usersRequests.TotalPages);

            return Ok(usersRequestsToReturn);
        }
    }
}