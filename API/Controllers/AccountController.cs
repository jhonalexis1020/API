using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Database;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<AppUser>> Register(RegisterDTO register)
        {
            if (await UserExisted(register.Username))
                return BadRequest("Usuario ya registrado");
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = register.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                PasswordSalt = hmac.Key
            };
            _Context.Users.Add(user);
            await _Context.SaveChangesAsync();
            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDTO login)
        {
            var user = await _Context.Users.SingleAsync(x => x.UserName == login.Username);
            if (user is null) return Unauthorized("Usuario Invalido");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Password Invalido");
            }
            return user;
        }


        private async Task<bool> UserExisted(string username)
        {
            return await _Context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }
}