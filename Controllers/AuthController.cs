using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KlicKitApi.Data;
using KlicKitApi.Dtos;
using KlicKitApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KlicKitApi.Controllers
{
    [ApiController]    
    [Route("[Controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        public AuthController(IAuthRepository repo,IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _config = config;
            _repo = repo;
            _db = unitOfWork; 
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            var createdUser = _repo.Register(userToCreate, userForRegisterDto.Password);
            
            _db.Add<User>(createdUser);
            if(!await _db.SaveAll())
                return BadRequest("Not Saved"); 

            var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);
            return CreatedAtRoute("GetUser", new {controller = "Auth", id = createdUser.Id}, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForDetailedDto>(userFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var userFromRepo = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(userFromRepo);

            return Ok(userToReturn);
        }
        
    }
}