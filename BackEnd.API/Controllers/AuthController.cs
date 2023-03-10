using BackEnd.API.Models.Dtos;
using BackEnd.API.Utils;
using BackEnd.API.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BackEnd.API.Models;
using BackEnd.API.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginCredentialsRequest credentials)
        {
            var user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
                return BadRequest("Invalid email or password");
            if (!await userManager.CheckPasswordAsync(user, credentials.Password))
                return BadRequest("Invalid email or password");

            var accessToken = TokenHelper.GenerateAccessToken(user);
            var refreshToken = TokenHelper.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(AuthOptions.REFRESH_VALID_DAYS);

            await userManager.UpdateAsync(user);
            var response = new Tokens { AccessToken = accessToken, RefreshToken = refreshToken };
            return Ok(response);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterCredentialsRequest credentials)
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

            var accessToken = TokenHelper.GenerateAccessToken(user);
            var refreshToken = TokenHelper.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(AuthOptions.REFRESH_VALID_DAYS);

            await userManager.UpdateAsync(user);
            var response = new Tokens { AccessToken = accessToken, RefreshToken = refreshToken };
            return Ok(response);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken(Tokens tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = TokenHelper.GetPrincipalFromExpiredToken(accessToken);
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

            var newAccessToken = TokenHelper.GenerateAccessToken(user);
            var newRefreshToken = TokenHelper.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(user);

            var response = new Tokens { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
            return Ok(response);
        }

        [HttpPost]
        [Route("revoke/{userName}")]
        public async Task<IActionResult> Revoke(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
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
    }
}
