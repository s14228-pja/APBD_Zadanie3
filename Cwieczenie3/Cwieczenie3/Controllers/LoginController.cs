using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Cwieczenie3.DAL;
using Cwieczenie3.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cwieczenie3.Controllers
{
    public class LoginController : Controller
    {
        private readonly IDbServices _dbService;
        public IConfiguration Configuration { get; set; }
        public LoginController(IConfiguration configuration, IDbServices dbServices)
        {
            Configuration = configuration;
            _dbService = dbServices;
        }

        [HttpPost]
        public IActionResult Login(LoginRequestDto request)
        {
            String pass = _dbService.Login(request.IndexNumber);
            var passFromReqest = Encoding.UTF8.GetString(Convert.FromBase64String(request.Haslo));
            var passFromReqestAfterHash = Encoding.UTF8.GetString(CreateHash(passFromReqest));
            if (pass != passFromReqestAfterHash)
            {
                return NotFound("Błedny login lub hasło");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, request.IndexNumber),
                new Claim(ClaimTypes.Role, "employee"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }

        public const int SALT_SIZE = 24; // size in bytes
        public const int HASH_SIZE = 24; // size in bytes
        public const int ITERATIONS = 10000; // number of pbkdf2 iterations

        public static byte[] CreateHash(string input)
        {
            // Generate a salt
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_SIZE];
            provider.GetBytes(salt);

            // Generate the hash
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, ITERATIONS);
            return pbkdf2.GetBytes(HASH_SIZE);
        }

    }
}
