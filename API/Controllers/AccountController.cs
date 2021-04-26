using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Database;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private DataContext _Context { get; set; }

        public AccountController(DataContext context)
        {
            _Context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(string username, string password)
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };
            _Context.Users.Add(user);
            await _Context.SaveChangesAsync();

            return user;

        }

    }
}