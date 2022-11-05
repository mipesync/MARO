using MARO.API.Models;
using MARO.Application.Aggregate.Models.DTOs;
using MARO.Application.Common.Exceptions;
using MARO.Application.Repository.UserRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MARO.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Добавить информацию о пользователе
        /// </summary>
        /// <remarks>
        /// Добавляет имя, фамилию и возраст пользователя. Пример запроса:
        /// 
        ///     POST: /api/user/add_user_details/4C2C522E-F785-4EB4-8ED7-260861453330
        ///     {
        ///         "firstname": "Иван",
        ///         "lastname": "Петров",
        ///         "age": 25
        ///     }
        /// </remarks>
        /// <response code="200">Удачно</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="404">Пользователь не найден</response>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
        [Authorize]
        [HttpPost("add_user_details/{userId}")]
        public async Task<IActionResult> AddUserDetails(UserDetailsDto model, Guid userId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new Error { Message = "Некорректные данные" });

                await _userRepository.AddUserDetails(model, userId.ToString());

                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(new Error { Message = e.Message });
            }
        }

        /// <summary>
        /// Обновить список пожеланий пользователя.
        /// </summary>
        /// <remarks>
        /// Добавляет пожелания пользователя. Id принимаются переведённые из бинарной системы в десятичную. Пример запроса:
        /// 
        ///     PUT: /api/user/update_items/4C2C522E-F785-4EB4-8ED7-260861453330?itemIds=83
        /// </remarks>
        /// <response code="200">Удачно</response>
        /// <response code="400">Обязательны все параметры</response>
        /// <response code="400">Переданы неверные Id пожеланий</response>
        /// <response code="404">Пользователь не найден</response>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
        [HttpPut("update_items/{userId}")]
        public async Task<IActionResult> UpdateUserItems(Guid userId, long itemIds)
        {
            try
            {
                if (userId == Guid.Empty || itemIds == 0) return BadRequest(new Error { Message = "Обязательны все параметры" });

                await _userRepository.UpdateUserCriteria(userId, itemIds);

                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(new Error { Message = e.Message });
            }
            catch (IndexOutOfRangeException e)
            {
                return BadRequest(new Error { Message = e.Message });
            }
        }

        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        /// <remarks>
        /// Выводит имя, фамилию, возраст, полное имя, контактную информацию, роль на сайте. Пример запроса:
        /// 
        ///     POST: /api/user/user_details/4C2C522E-F785-4EB4-8ED7-260861453330
        /// </remarks>
        /// <response code="200">Удачно</response>
        /// <response code="400">Поле UserId обязательно</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="404">Роль не найдена</response>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
        [Authorize]
        [HttpGet("user_details/{userId}")]
        public async Task<IActionResult> UserGetails(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty) return BadRequest(new {message = "Поле UserId обязательно"});

                var result = await _userRepository.UserDetails(UserId);

                return Ok(result);
            }
            catch (NotFoundException e)
            {
                return NotFound(new Error { Message = e.Message });
            }
        }
    }
}
