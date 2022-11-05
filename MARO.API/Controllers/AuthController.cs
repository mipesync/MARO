using MARO.API.Models;
using MARO.Application.Aggregate.Models.DTOs;
using MARO.Application.Aggregate.Models.ResponseModels;
using MARO.Application.Common.Exceptions;
using MARO.Application.Interfaces;
using MARO.Application.Repository.AuthRepo;
using MARO.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
        private readonly IAuthRepository _authRepository;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthController> logger, ITokenManager tokenManager, IAuthRepository authRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenManager = tokenManager;
            _authRepository = authRepository;
        }
        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <remarks> 
        /// Выдача токена доступа и, если "запомнить меня" = true, выдача токена обновления. Пример запроса:
        /// 
        ///     POST: /api/auth/login
        ///     {
        ///         "arg": "user@example.com" (или "71234567890"),
        ///         "password": "Abcd_123",
        ///         "rememberMe": true,
        ///         "returnUrl": "http://example.com/catalog"
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Возврат: <see cref="LoginResponseModel"/></returns>
        /// <response code="400">Некорректные данные</response>
        /// <response code="400">Неудачная попытка входа</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="403">Аккаунт не подтверждён</response>
        /// <response code="403">Аккаунт заблокирован</response>
        /// <response code="200">Удачно</response>

        [HttpPost("login")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(LoginResponseModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status403Forbidden, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid) return BadRequest(new Error { Message = "Некорректные данные" });

            var user = await _userManager.FindByNameAsync(model.Arg);

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

                var accessToken = await _tokenManager.CreateAccessTokenAsync(user,  _userManager.GetRolesAsync(user).Result.FirstOrDefault()!);

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

                return BadRequest(new Error { Message = "Неправильный логин или пароль" });
            }
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <remarks>
        /// Пароль должен состоять не менее чем из 8 символов, содержать буквенно-цифровые символы и символы в верхнем регистре.
        /// Клиент должен обязательно содержать страницу ".../confirm". Пример: http://example.com/confirm_email
        /// Пример запроса:
        /// 
        ///     POST: /api/auth/register
        ///     {
        ///         "arg": "user@example.com" (или "71234567890"),
        ///         "password": "Abcd_123",
        ///         "host": "http://example.com"
        ///         "returnUrl": "http://example.com/catalog"
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Возврат <see cref="RegisterResponseModel"/></returns>
        /// <response code="400">Некорректные данные</response>
        /// <response code="400">Если пользователь уже существует</response>
        /// <response code="400">Что-то пошло не так</response>
        /// <response code="200">Удачно</response>
        /// <response code="404">Роль не существует</response>

        [HttpPost("register")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(RegisterResponseModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new Error { Message = "Некорректные данные" });

                if (model.Arg.All(u => char.IsDigit(u)))
                {
                    //TODO: На релизе оставить userId
                    var result = await _authRepository.PhoneRegister(model.Arg, model.Password, model.ReturnUrl!);
                    return Ok(new { userId = result[0], code = result[1] });
                }
                else if (model.Arg.Contains('@'))
                {
                    await _authRepository.EmailRegister(model.Arg, model.Password, model.Host, model.ReturnUrl!);
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

        /// <summary>
        /// Подтверждение аккаунта
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST: /api/auth/confirm
        ///     {
        ///         "userId": "user@example.com",
        ///         "code": "your code",
        ///         "returnUrl": "http://example.com/catalog"
        ///     }
        /// </remarks>
        /// <returns>Возврат <see cref="ConfirmResponseModel"/></returns>
        /// <response code="400">Параметры UserId и Code обязательны</response>
        /// <response code="400">Что-то пошло не так</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="200">Удачно</response>
        /// <response code="400">Ошибка подтверждения аккаунта</response>

        [HttpPost("confirm")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(ConfirmResponseModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
        public async Task<IActionResult> Confirm(ConfirmDto model)
        {
            try
            {
                if (model.UserId == Guid.Empty || model.Code == null) return BadRequest(new Error { Message = "Параметры UserId и Code обязательны!" });

                if (model.Code.Length == 6)
                {
                    return Ok(await _authRepository.PhoneConfirm(model.UserId.ToString(), model.Code, model.ReturnUrl));
                }
                else if (model.Code.Length > 6)
                {
                    return Ok(await _authRepository.EmailConfirm(model.UserId.ToString(), model.Code, model.ReturnUrl));
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

        /// <summary>
        /// Забыл пароль
        /// </summary>
        /// <remarks>
        /// Отправить код для сброса пароля.
        /// Клиент должен обязательно содержать страницу ".../reset-password ". Пример: http://example.com/reset_password 
        /// Пример запроса: 
        /// 
        ///     GET: /api/auth/forgot_password?arg=user@example.com&amp;host=http://emxaple.com
        /// </remarks>
        /// <param name="arg">Адрес электронной почты или номер телефона пользователя, на который будет отправлено письмо с кодом подтверждения</param>
        /// <param name="host">Хост клиента, с которого идёт запрос</param>
        /// <response code="400">Некорректные данные</response>
        /// <response code="400">Если пользователь не существует или аккаунт заблокирован</response>
        /// <response code="200">Удачно</response>

        [HttpGet("forgot_password")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        public async Task<IActionResult> ForgotPassword(string arg, string host)
        {
            try
            {
                if (arg == null) return BadRequest(new Error { Message = "Поле Arg обязательно!" });

                var response = string.Empty;

                if (arg.All(u => char.IsDigit(u)))
                {
                    response = await _authRepository.PhoneForgotPassword(arg);
                }
                else if (arg.Contains('@'))
                {
                    await _authRepository.EmailForgotPassword(arg, host);
                }

                //TODO: На релизе убрать параметры
                return Ok(new {message = response});
            }
            catch(ArgumentException e)
            {
                return BadRequest(new Error { Message = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new Error { Message = e.Message });
            }
        }

        /// <summary>
        /// Сброс пароля
        /// </summary>
        /// <remarks>
        /// Проверяет код подтверждения и сбрасывает пароль. 
        /// Пример запроса:
        /// 
        ///     POST: /api/auth/reset_password
        ///     {
        ///         "arg": "user@example.com",
        ///         "code": "your code",
        ///         "password": "Abcd_123"
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="400">Некорректные данные</response>
        /// <response code="400">Ошибка сброса пароля</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="200">Удачно</response>

        [HttpPost("reset_password")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
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

        /// <summary>
        /// Обновление токена обновления и доступа
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PUT: /api/auth/refresh_token
        ///     {
        ///         "access_token": "jwt token",
        ///         "refresh_token": "your refresh token"
        ///     }
        /// </remarks>
        /// <returns>Возврат <see cref="RefreshTokenResponseModel"/></returns>
        /// <response code="400">Некорректные данные</response>
        /// <response code="400">Недействительный токен доступа</response>
        /// <response code="400">Недействительный токен доступа или обновления</response>
        /// <response code="200">Удачно</response>

        [HttpPut("refresh_token")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(RefreshTokenResponseModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
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

        /// <summary>
        /// Отзыв токена обновления
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     DELETE: /api/auth/revoke/4C2C522E-F785-4EB4-8ED7-260861453330
        /// </remarks>
        /// <param name="userId">Id пользователя, у которого нужно отозвать токен обновления</param>
        /// <response code="200">Удачно</response>
        /// <response code="400">Ошибка отзыва</response>
        /// <response code="400">Что-то пошло не так...</response>
        /// <response code="404">Пользователь не найден</response>

        [HttpDelete("revoke/{userId}")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
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

            return BadRequest(new Error { Message = "Что-то пошло не так..." });
        }

        /// <summary>
        /// Отзыв токена обновления
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST: /api/auth/login_as_guest
        /// </remarks>
        /// <response code="200">Удачно</response>
        /// <response code="400">Что-то пошло не так...</response>
        /// <response code="404">Роль не найдена</response>

        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
        [HttpPost("login_as_guest")]
        public async Task<IActionResult> LoginAsGuest()
        {
            try
            {
                var result = await _authRepository.LoginAsGuest();

                return Ok(new LoginAsGuestResponseModel { GuestId = result });
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

        /*
        [HttpPost("asf")]
        public async Task<IActionResult> ASdsd(string email)
        {
            _emailSender.Subject = "Сброс пароля";
            _emailSender.To = email;
            //TODO: Вставить HTML
            _emailSender.Message = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"Content-Type\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Сброс пароля</title>\r\n</head>\r\n\r\n<body style=\"margin: 0; box-sizing: border-box; font-family: Roboto, sans-serif;\">\r\n    <div style=\"\r\n    display: grid;\r\n    max-width: 500px;\r\n    min-height: 500px;\r\n    border-radius: 20px;\r\n    margin: 0 auto;;\r\n    \">\r\n\r\n        <div style=\"\r\n        display: grid;\r\n        place-items: center;\r\n        text-align: center;\r\n        height: 50px;\r\n        border-radius: 20px 20px 0 0;\r\n        background: #ED2B38;\r\n        font-weight: 500;\r\n        font-size: 20px;\r\n        line-height: 23px;        \r\n        text-transform: uppercase;\r\n        color: #FFFFFF;\">\r\n                <sup style=\"display: grid;\r\n                place-items: center;\r\n                text-align: center;\r\n        \">Сброс пароля</sup>\r\n        </div>\r\n            \r\n            <div style=\"\r\n            display: grid;\r\n            justify-items: center;\r\n            place-items: center;\r\n            text-align: center;\r\n            margin: 0px 100px 30px 100px;\r\n            background-color: #FFFFFF;\">\r\n                <label style=\"\r\n                display: grid;\r\n                place-items: center;\r\n                align-self: center;\r\n                text-align: center;\r\n                font-weight: 400;\r\n                font-size: 20px;\r\n                line-height: 23px;\r\n                color: #282828;\r\n                    \"><b>Здравствуйте, username!</b>\r\n                </label>\r\n                <label style=\"\r\n                display: grid;\r\n                place-items: center;\r\n                align-self: center;\r\n                text-align: center;\r\n                margin-top: 30px;\r\n                font-weight: 400;\r\n                font-size: 15px;\r\n                line-height: 17px;\r\n                text-align: center;\r\n                color: #999999;\">Забыли пароль? Создайте новый ниже!</label>    \r\n                <a href=\"http://google.com/\">\r\n                    <button style=\"\r\n                    width:200px;\r\n                    height:30px;\r\n                    padding:5px 10px;\r\n                    text-align:center;\r\n                    font-weight:500;\r\n                    font-size:12px;\r\n                    line-height:14px;\r\n                    text-transform:uppercase;\r\n                    background-color:#ed2b38;\r\n                    color:#ffffff;\r\n                    border-radius:10px;\r\n                    border:0px;\r\n                    margin: 30px 50px 30px 50px;\"\r\n                    onmouseover=\"this.style.backgroundColor='#B2212B';\"\r\n                    onmouseout=\"this.style.backgroundColor='#ED2B38';\">\r\n                        <span style=\"display: grid;\r\n                        place-items: center;\r\n                        align-self: center;\r\n                        text-align: center;\r\n                            \">Сбросить пароль\r\n                        </span> \r\n                    </button> \r\n                </a>       \r\n                <label style=\"\r\n                display: grid;\r\n                place-items: center;\r\n                align-self: center;\r\n                text-align: center;\r\n                font-weight: 400;\r\n                font-size: 15px;\r\n                line-height: 17px;\r\n                text-align: center;\r\n                color: #999999;\">Если вы не хотели сбрасывать пароль, то просто игнорируйте это сообщение!</label>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n</body>\r\n</html>\r\n\r\n\r\n";

            await _emailSender.SendAsync();

            return Ok();
        }*/
    }
}
