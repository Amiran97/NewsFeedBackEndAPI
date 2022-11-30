using BackEnd.Infrastructure.Models.Dtos;
using BackEnd.API.Utils;
using BackEnd.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Infrastructure.Models;
using BackEnd.API.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private readonly NewsFeedContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;

        public AuthController(NewsFeedContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [Route("ping")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Ping()
        {
            return Ok("ping");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginCredentialsRequestDTO credentials)
        {
            var user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
                return BadRequest("Invalid email or password");
            if (!await userManager.CheckPasswordAsync(user, credentials.Password))
                return BadRequest("Invalid email or password");

            var accessToken = TokenGenerator.GenerateAccessToken(user);
            var refreshToken = TokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(AuthOptions.REFRESH_VALID_DAYS);

            await userManager.UpdateAsync(user);
            var response = new TokensDTO { AccessToken = accessToken, RefreshToken = refreshToken };
            return Ok(response);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterCredentialsRequestDTO credentials)
        {
            var user = new User
            {
                Email = credentials.Email,
                UserName = credentials.UserName
            };

            var result = await userManager.CreateAsync(user, credentials.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            user = await userManager.FindByEmailAsync(credentials.Email);

            var accessToken = TokenGenerator.GenerateAccessToken(user);
            var refreshToken = TokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(AuthOptions.REFRESH_VALID_DAYS);

            await userManager.UpdateAsync(user);
            var response = new TokensDTO { AccessToken = accessToken, RefreshToken = refreshToken };
            return Ok(response);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken(TokensDTO tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            string username = principal.Identity.Name;

            var user = await userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = TokenGenerator.GenerateAccessToken(user);
            var newRefreshToken = TokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(user);

            var response = new TokensDTO { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
            return Ok(response);
        }

        [HttpPost]
        [Route("revoke/{username}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;
            await userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpPost]
        [Route("revoke-all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RevokeAll()
        {
            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await userManager.UpdateAsync(user);
            }

            return NoContent();
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = AuthOptions.AUDIENCE,
                ValidateIssuer = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(AuthOptions.LIFETIME)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
