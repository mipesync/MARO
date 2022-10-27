using MARO.API.Models;
using MARO.Application.Interfaces;
using MARO.Application.Models;
using MARO.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Encodings.Web;
using System.Text;

namespace MARO.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<User> _roleManager;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenManager _tokenManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<User> roleManager, ILogger<AuthController> logger, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _tokenManager = tokenManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid) return BadRequest(new Error { Message = "Model is not valid." });

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null) return NotFound(new Error { Message = "User not found" });

            if (!user.EmailConfirmed || !user.PhoneNumberConfirmed)
            {
                _logger.LogWarning($"UserID: {UserId}. Email not confirmed.");
                return StatusCode((int)HttpStatusCode.Forbidden, new Error { Message = "Email not confimed." });
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                _logger.LogInformation($"UserID: {UserId}. Logged in.");

                var accessToken = await _tokenManager.CreateAccessTokenAsync(user, _userManager.GetRolesAsync(user).Result.FirstOrDefault()!);
                var refreshToken = await _tokenManager.CreateRefreshTokenAsync();

                user.LockoutEnd = null;
                user.LockoutEnabled = false;

                if (model.RememberMe)
                {
                    user.RefreshToken = refreshToken.Token;
                    user.RefreshTokenExpiryTime = new JwtOptions().RefreshTokenExpires;
                }

                await _userManager.UpdateAsync(user);

                return Ok(new LoginResponseModel
                {
                    AccessToken = accessToken.Token,
                    Expires = accessToken.Expires,
                    RefreshToken = refreshToken.Token,
                    ReturnUrl = model.ReturnUrl
                });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning($"UserID: {UserId}. Account locked out.");

                user.AccessFailedCount = 0;
                await _userManager.UpdateAsync(user);

                return StatusCode((int)HttpStatusCode.Forbidden, new Error { Message = "Account locked out." });
            }
            else
            {
                user.AccessFailedCount++;

                if (user.AccessFailedCount >= 5)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.UtcNow + TimeSpan.FromMinutes(10);
                }

                await _userManager.UpdateAsync(user);

                return BadRequest(new Error { Message = "Invalid login attempt." });
            }
        }

        [HttpPost("emailRegister")]
        public async Task<IActionResult> EmailRegister(RegisterDto model)
        {
            if (!ModelState.IsValid) return BadRequest(new Error { Message = "Model is not valid." });

            var user = new User
            {
                UserName = model.Username,
                Email = model.Username
            };

            var getUser = await _userManager.FindByEmailAsync(model.Username);

            if (getUser is not null) return BadRequest(new Error { Message = $"A user with this email already exists." });

            var result = await _userManager.CreateAsync(user, model.Password);

            await _userManager.AddToRoleAsync(user, RoleEnum.user.ToString());

            if (result.Succeeded)
            {
                _logger.LogInformation($"UserID: {UserId}. User created a new account with password.");

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = $"{UrlRaw}/confirm_email?userId={userId}&code={code}&returnUrl={model.ReturnUrl}";

                await _emailSender.SendEmailAsync(model.Username, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.");

                return Ok(new RegisterResponseModel { UserId = userId, ReturnUrl = model.ReturnUrl! });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    return BadRequest(new Error { Message = error.Description });
                }
            }
        }
    }
}
