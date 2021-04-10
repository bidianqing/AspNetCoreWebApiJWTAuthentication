using AspNetCoreWebApiJWTAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AspNetCoreWebApiJWTAuthentication.Controllers
{
    public class TokenController : Controller
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public IActionResult Post([FromBody]LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserIdFromCredentials(loginModel);
                if (userId == -1)
                {
                    return Unauthorized();
                }

                var claims = new[]
                {
                    new Claim("username", loginModel.Username),
                    new Claim("jti", Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Email, "bidianqing@qq.com"),
                    new Claim("age", "26"),
                    new Claim("phone", "18515278856"),
                    new Claim("qq", "243527176"),
                    new Claim("birth", "1991-09-16"),
                    new Claim("userId", "1"),
                    new Claim(JwtRegisteredClaimNames.Gender,"男"),
                    new Claim(ClaimTypes.Name,"1111"),
                };

                var jwtSecurityToken = new JwtSecurityToken
                (
                    issuer: _configuration["Issuer"],                       // 发行人
                    audience: _configuration["Audience"],                   // 接收方
                    claims: claims,                                         // 声明
                    expires: DateTime.Now.AddDays(30).Date.AddHours(4),     // 过期时间 只在凌晨4点过期
                    notBefore: DateTime.Now,                                // 定义在什么时间之前，该jwt都是不可用的
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SigningKey"])),
                         SecurityAlgorithms.HmacSha256)
                );

                string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                return Ok(new LoginReponseModel
                {
                    Token = token,
                    Id = 830334368416268288
                });
            }

            return BadRequest();
        }

        private int GetUserIdFromCredentials(LoginModel loginModel)
        {
            var userId = -1;
            if (loginModel.Username == "sa" && loginModel.Password == "sa")
            {
                userId = 5;
            }

            return userId;
        }
    }
}
