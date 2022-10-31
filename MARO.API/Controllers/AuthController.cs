using MARO.API.Models;
using MARO.Application.Aggregate.Models;
using MARO.Application.Interfaces;
using MARO.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;

namespace MARO.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenManager _tokenManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ILogger<AuthController> logger, ITokenManager tokenManager, IEmailSender emailSender, ISmsSender smsSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _tokenManager = tokenManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid) return BadRequest(new Error { Message = "Model is not valid." });

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null) return NotFound(new Error { Message = "User not found" });

            if (!user.EmailConfirmed || !user.PhoneNumberConfirmed)
            {
                _logger.LogWarning($"UserID: {UserId}. Account not confirmed.");
                return StatusCode((int)HttpStatusCode.Forbidden, new Error { Message = "Account not confimed." });
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

        //TODO: Регулярка на почту
        [HttpPost("email_register")]
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

            await _userManager.AddToRoleAsync(user, "user");

            if (result.Succeeded)
            {
                _logger.LogInformation($"UserID: {UserId}. User created a new account with password.");

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = $"{UrlRaw}/confirm_email?userId={userId}&code={code}&returnUrl={model.ReturnUrl}";

                _emailSender.Subject = "Подтвердите Вашу почту";
                _emailSender.To = model.Username;
                _emailSender.Message = $"Подтвердите свою почту, <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>перейдя по этой ссылке</a>.";

                await _emailSender.SendAsync();

                return Ok(new RegisterResponseModel { UserId = userId, ReturnUrl = model.ReturnUrl! });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    return BadRequest(new Error { Message = error.Description });
                }
            }

            return BadRequest(new Error { Message = "Something went wrong..." });
        }

        //TODO:Форматировать номер телефона, если пришёл не 7 ### ### ## ##
        [HttpPost("phone_register")]
        public async Task<IActionResult> PhoneRegister(RegisterDto model)
        {
            if (!ModelState.IsValid) return BadRequest(new Error { Message = "Model is not valid." });

            var user = new User
            {
                UserName = model.Username,
                PhoneNumber = model.Username
            };

            var getUser = await _userManager.FindByNameAsync(model.Username);

            if (getUser is not null) return BadRequest(new Error { Message = $"A user with this phone number already exists." });

            var result = await _userManager.CreateAsync(user, model.Password);

            await _userManager.AddToRoleAsync(user, "user");

            if (result.Succeeded)
            {
                _logger.LogInformation($"UserID: {UserId}. User created a new account with password.");

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

                user.PhoneConfirmationCode = code;

                await _userManager.UpdateAsync(user);

                //ERROR: НА РЕЛИЗЕ РАСКОМЕНТИТЬ!!!!
                /*_smsSender.Message = $"Ваш код: {code}";
                await _smsSender.SendAsync();

                return Ok(new RegisterResponseModel { UserId = userId, ReturnUrl = model.ReturnUrl! });*/
                return Ok(new {code = code});
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    return BadRequest(new Error { Message = error.Description });
                }
            }

            return BadRequest(new Error { Message = "Something went wrong..." });
        }

        [HttpPost("confirm_email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code, string? returnUrl)
        {
            if (userId == null || code == null)
            {
                return NoContent();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new Error { Message = "User not found." });
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return Ok(new 
                {
                    message = "Email has been confirmed",
                    returnUrl = returnUrl
                });
            }
            else return BadRequest(new Error { Message = "Error confirming email" });
        }

        [HttpPost("confirm_phone")]
        public async Task<IActionResult> ConfirmPhone(string userId, string code, string? returnUrl)
        {
            if (userId == null || code == null)
            {
                return NoContent();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new Error { Message = "User not found." });
            }

            if (user.PhoneConfirmationCode == code)
            {
                user.PhoneConfirmationCode = null;
                user.PhoneNumberConfirmed = true;

                await _userManager.UpdateAsync(user);

                return Ok(new 
                {
                    message = "Phone number has been confirmed",
                    returnUrl = returnUrl
                });
            }
            else return BadRequest(new Error { Message = "Error confirming phone number" });
        }

        /*[HttpPost("add_user_details")]
        public async Task<IActionResult> AddUserDetails()
        {

        }

        [HttpPost("login_as_guest")]
        public async Task<IActionResult> LoginAsGuest()
        {

        }*/
    }
}
