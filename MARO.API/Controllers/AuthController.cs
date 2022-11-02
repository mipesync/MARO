using AutoMapper;
using MARO.API.Models;
using MARO.Application.Aggregate.Models.DTOs;
using MARO.Application.Aggregate.Models.ResponseModels;
using MARO.Application.Common.Exceptions;
using MARO.Application.Interfaces;
using MARO.Application.Repository;
using MARO.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace MARO.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenManager _tokenManager;
        private readonly IEmailSender _emailSender;
        private readonly IAuthRepository _authRepository;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthController> logger, ITokenManager tokenManager, IEmailSender emailSender, IAuthRepository authRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenManager = tokenManager;
            _emailSender = emailSender;
            _authRepository = authRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid) return BadRequest(new Error { Message = "Некорректные данные" });

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null) return NotFound(new Error { Message = "Пользователь не найден" });

            if (!user.EmailConfirmed && !user.PhoneNumberConfirmed)
            {
                _logger.LogWarning($"UserID: {UserId}. Аккаунт не подтверждён");
                return StatusCode((int)HttpStatusCode.Forbidden, new Error { Message = "Аккаунт не подтверждён" });
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                _logger.LogInformation($"UserID: {UserId}. Вошёл в систему");

                var accessToken = await _tokenManager.CreateAccessTokenAsync(user, _userManager.GetRolesAsync(user).Result.FirstOrDefault()!);

                user.LockoutEnd = null;
                user.LockoutEnabled = false;

                if (model.RememberMe)
                {
                    var refreshToken = await _tokenManager.CreateRefreshTokenAsync();
                    user.RefreshToken = refreshToken.Token;
                    user.RefreshTokenExpiryTime = new JwtOptions().RefreshTokenExpires;
                }

                await _userManager.UpdateAsync(user);

                return Ok(new LoginResponseModel
                {
                    AccessToken = accessToken.Token,
                    Expires = accessToken.Expires,
                    RefreshToken = user.RefreshToken,
                    ReturnUrl = model.ReturnUrl
                });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning($"UserID: {UserId}. Аккаунт заблокирован");

                user.AccessFailedCount = 0;
                await _userManager.UpdateAsync(user);

                return StatusCode((int)HttpStatusCode.Forbidden, new Error { Message = "Аккаунт заблокирован" });
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

                return BadRequest(new Error { Message = "Неудачная попытка входа" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new Error { Message = "Некорректные данные" });

                if (model.Arg.All(u => char.IsDigit(u)))
                {
                    //TODO: На релизе убрать из Ok() параметры
                    return Ok(await _authRepository.PhoneForgotPassword(model.Arg));
                }
                else if (model.Arg.Contains('@'))
                {
                    await _authRepository.EmailForgotPassword(model.Arg, UrlRaw);
                    return Ok();
                }

                return BadRequest(new Error { Message = "Что-то пошло не так..." });
            }
            catch (NotFoundException e)
            {
                return NotFound(new Error { Message = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new Error { Message = e.Message });
            }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm(string userId, string code, string? returnUrl)
        {
            try
            {
                if (userId == null || code == null) return BadRequest(new Error { Message = "Параметры \"UserId\" и \"Code\" обязательны!" });

                if (code.Length == 6)
                {
                    return Ok(await _authRepository.PhoneConfirm(userId, code, returnUrl));
                }
                else if (code.Length > 6)
                {
                    return Ok(await _authRepository.EmailConfirm(userId, code, returnUrl));
                }

                return BadRequest(new Error { Message = "Что-то пошло не так..." });
            }
            catch(NotFoundException e)
            {
                return NotFound(new Error { Message = e.Message });
            }
            catch(Exception e)
            {
                return BadRequest(new Error { Message = e.Message });
            }
        }

        [HttpGet("forgot_password")]
        public async Task<IActionResult> ForgotPassword(string arg)
        {
            try
            {
                if (arg == null) return BadRequest(new Error { Message = "Поле \"Arg\" обязательно!" });

                var response = string.Empty;

                if (arg.All(u => char.IsDigit(u)))
                {
                    await _authRepository.PhoneForgotPassword(arg);
                }
                else if (arg.Contains('@'))
                {
                    await _authRepository.EmailForgotPassword(arg, UrlRaw);
                }

                //TODO: На релизе убрать параметры
                return Ok(new {message = response});
            }
            catch(ArgumentException e)
            {
                return BadRequest(new Error { Message = e.Message });
            }
        }

        [HttpPost("reset_password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new Error { Message = "Некорректные данные" });

                await _authRepository.ResetPassword(model);

                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(new Error { Message = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new Error { Message = e.Message });
            }
        }

        [HttpPut("refresh_token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto model)
        {
            if (!ModelState.IsValid) return BadRequest(new Error { Message = "Некорректные данные" });

            var accessToken = model.AccessToken;
            var refreshToken = model.RefreshToken;

            var principal = await _tokenManager.GetPrincipalFromExpiredTokenAsync(accessToken)!;

            if (principal is null) return BadRequest(new Error { Message = "Недействительный токен доступа" });

            var user = await _userManager.FindByIdAsync(principal.FindFirst(u => u.Type == ClaimTypes.NameIdentifier)!.Value);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest(new Error { Message = "Недействительный токен доступа или обновления" });
            }

            var roles = await _userManager.GetRolesAsync(user);

            var newAccessToken = await _tokenManager.CreateAccessTokenAsync(user, roles.Last());
            var newRefreshToken = await _tokenManager.CreateRefreshTokenAsync();

            user.RefreshToken = newRefreshToken.Token;
            user.RefreshTokenExpiryTime = new JwtOptions().RefreshTokenExpires;

            await _userManager.UpdateAsync(user);

            return Ok(new RefreshTokenResponseModel
            {
                AccessToken = newAccessToken.Token,
                Expires = newAccessToken.Expires,
                RefreshToken = newRefreshToken.Token
            });
        }

        [HttpDelete("revoke/{userId}")]
        public async Task<IActionResult> Revoke(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) return NotFound(new Error { Message = "Пользователь не найден" });

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = default(DateTime);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }
            foreach (var error in result.Errors)
            {
                return BadRequest(new Error { Message = error.Description });
            }

            return NoContent();
        }

        [HttpPost("asf")]
        public async Task<IActionResult> ASdsd(string email)
        {
            _emailSender.Subject = "Сброс пароля";
            _emailSender.To = email;
            //TODO: Вставить HTML
            _emailSender.Message = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"Content-Type\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Сброс пароля</title>\r\n</head>\r\n\r\n<body style=\"margin: 0; box-sizing: border-box; font-family: Roboto, sans-serif;\">\r\n    <div style=\"\r\n    display: grid;\r\n    max-width: 500px;\r\n    min-height: 500px;\r\n    border-radius: 20px;\r\n    margin: 0 auto;;\r\n    \">\r\n\r\n        <div style=\"\r\n        display: grid;\r\n        place-items: center;\r\n        text-align: center;\r\n        height: 50px;\r\n        border-radius: 20px 20px 0 0;\r\n        background: #ED2B38;\r\n        font-weight: 500;\r\n        font-size: 20px;\r\n        line-height: 23px;        \r\n        text-transform: uppercase;\r\n        color: #FFFFFF;\">\r\n                <sup style=\"display: grid;\r\n                place-items: center;\r\n                text-align: center;\r\n        \">Сброс пароля</sup>\r\n        </div>\r\n            \r\n            <div style=\"\r\n            display: grid;\r\n            justify-items: center;\r\n            place-items: center;\r\n            text-align: center;\r\n            margin: 0px 100px 30px 100px;\r\n            background-color: #FFFFFF;\">\r\n                <label style=\"\r\n                display: grid;\r\n                place-items: center;\r\n                align-self: center;\r\n                text-align: center;\r\n                font-weight: 400;\r\n                font-size: 20px;\r\n                line-height: 23px;\r\n                color: #282828;\r\n                    \"><b>Здравствуйте, username!</b>\r\n                </label>\r\n                <label style=\"\r\n                display: grid;\r\n                place-items: center;\r\n                align-self: center;\r\n                text-align: center;\r\n                margin-top: 30px;\r\n                font-weight: 400;\r\n                font-size: 15px;\r\n                line-height: 17px;\r\n                text-align: center;\r\n                color: #999999;\">Забыли пароль? Создайте новый ниже!</label>    \r\n                <a href=\"http://google.com/\">\r\n                    <button style=\"\r\n                    width:200px;\r\n                    height:30px;\r\n                    padding:5px 10px;\r\n                    text-align:center;\r\n                    font-weight:500;\r\n                    font-size:12px;\r\n                    line-height:14px;\r\n                    text-transform:uppercase;\r\n                    background-color:#ed2b38;\r\n                    color:#ffffff;\r\n                    border-radius:10px;\r\n                    border:0px;\r\n                    margin: 30px 50px 30px 50px;\"\r\n                    onmouseover=\"this.style.backgroundColor='#B2212B';\"\r\n                    onmouseout=\"this.style.backgroundColor='#ED2B38';\">\r\n                        <span style=\"display: grid;\r\n                        place-items: center;\r\n                        align-self: center;\r\n                        text-align: center;\r\n                            \">Сбросить пароль\r\n                        </span> \r\n                    </button> \r\n                </a>       \r\n                <label style=\"\r\n                display: grid;\r\n                place-items: center;\r\n                align-self: center;\r\n                text-align: center;\r\n                font-weight: 400;\r\n                font-size: 15px;\r\n                line-height: 17px;\r\n                text-align: center;\r\n                color: #999999;\">Если вы не хотели сбрасывать пароль, то просто игнорируйте это сообщение!</label>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n</body>\r\n</html>\r\n\r\n\r\n";

            await _emailSender.SendAsync();

            return Ok();
        }

        /*[HttpGet("get_list")]
        public async Task<IActionResult> GetList(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            var criteria = await _dbContext.Criteria.Where(u => u.Users!.Contains(user)).Include(u => u.Children).ToListAsync();

            return Ok(criteria);
        }

        [HttpPost("add_list")]
        public async Task<IActionResult> AddList(List<Criterion> criteria, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            user.Criteria = criteria;

            return Ok(user);
        }*/
    }
}
