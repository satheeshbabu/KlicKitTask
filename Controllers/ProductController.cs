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
    [Authorize]
    [Route("[Controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        public readonly IProductRepository _product;
        public readonly IAuthRepository _auth;
        private readonly IUnitOfWork _db;
        
        

        public ProductController(DataContext dbContext, ILogger<ProductController> logger, IUnitOfWork unitOfWork,
                                 IMapper mapper, IProductRepository product, IAuthRepository auth)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _product = product;
            _auth = auth;
            _db = unitOfWork;
        }
        
        

        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
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

        [HttpPut("UserRequest/{productId}")]
        public async Task<IActionResult> UserRequest(Guid productId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _auth.GetUser(userId);
            if (user == null)
                return Unauthorized();

            var productFromRepo = await _product.GetProduct(productId);
            if (productFromRepo == null)
                return BadRequest("Not Found");

            var userProduct = new UserProducts
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Product = productFromRepo,
                User = user,
                UserId = userId,
                RequestTime = DateTime.Now,
                IsApproved = false,
                IsChecked = false
            };
            _db.Add<UserProducts>(userProduct);            
            
            if (await _db.SaveAll())
                return Ok();

            throw new Exception($"Failed on save");
        }

        [HttpPut("ApproveRequest/{requestId}")]
        public async Task<IActionResult> ApproveRequest(Guid requestId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _auth.GetUser(userId);
            if (user == null || user.Role != 1)
                return Unauthorized();

            var userProductFromRepo = await _product.GetUserProduct(requestId);
            if (userProductFromRepo == null)
                return BadRequest("Not Found");
            
            userProductFromRepo.IsChecked = true;
            userProductFromRepo.IsApproved = true;

            if (await _db.SaveAll())
                return Ok();

            throw new Exception($"Failed on save");
        }

        [HttpPut("RejectRequest/{requestId}")]
        public async Task<IActionResult> RejectRequest(Guid requestId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _auth.GetUser(userId);
            if (user == null || user.Role != 1)
                return Unauthorized();

            var userProductFromRepo = await _product.GetUserProduct(requestId);
            if (userProductFromRepo == null)
                return BadRequest("Not Found");
            
            userProductFromRepo.IsChecked = true;
            userProductFromRepo.IsApproved = false;

            if (await _db.SaveAll())
                return Ok();

            throw new Exception($"Failed on save");
        }
    }
}