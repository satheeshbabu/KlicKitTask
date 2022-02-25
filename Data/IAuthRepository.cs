using System;
using System.Threading.Tasks;
using KlicKitApi.Helpers;
using KlicKitApi.Models;

namespace KlicKitApi.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> UserExists(string username);
         Task<User> GetUser(Guid id);         
    }
}