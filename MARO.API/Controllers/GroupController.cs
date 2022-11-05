using MARO.API.Models;
using MARO.Application.Aggregate.Models.DTOs;
using MARO.Application.Common.Exceptions;
using MARO.Application.Repository.GroupRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MARO.API.Controllers
{
    [Authorize]
    public class GroupController : BaseController
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IWebHostEnvironment _environment;

        public GroupController(IGroupRepository groupRepository, IWebHostEnvironment environment)
        {
            _groupRepository = groupRepository;
            _environment = environment;
        }

        /// <summary>
        /// Создать группу пользователей
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST: /api/group/create_group
        ///     {
        ///         "userId": "4C2C522E-F785-4EB4-8ED7-260861453330",
        ///         "host": "http://example.com"
        ///     }
        /// </remarks>
        /// <response code="200">Удачно</response>
        /// <response code="400">Поле UserId обязательно</response>
        /// <response code="400">У пользователя уже есть группа</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="404">Роль не найдена</response>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
        [HttpPost("create_group")]
        public async Task<IActionResult> CreateGroup(CreateGroupDto model)
        {
            try
            {
                if (model.UserId == Guid.Empty) return BadRequest(new Error { Message = "Поле UserId обязательно" });

                var result = await _groupRepository.CreateGroup(model.UserId, _environment.WebRootPath, model.Host);

                result.QRLink = result.QRLink.Insert(0, UrlRaw);

                return Ok(result);
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
        /// Присоединиться к существующей группе
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST: /api/group/join_group
        ///     {
        ///         "userId": "4C2C522E-F785-4EB4-8ED7-260861453330",
        ///         "groupId": "4C2C522E-F785-4EB4-8ED7-260861453330"
        ///     }
        /// </remarks>
        /// <response code="200">Удачно</response>
        /// <response code="400">Поле UserId обязательно</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="404">Роль не найдена</response>
        /// <response code="404">Группа не найдена</response>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
        [HttpPost("join_group")]
        public async Task<IActionResult> JoinGroup(JoinTheGroupDto model)
        {
            try
            {
                if (model.UserId == Guid.Empty) return BadRequest(new Error { Message = "Поле UserId обязательно" });

                await _groupRepository.JoinGroup(model.GroupId, model.UserId);

                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(new Error { Message = e.Message });
            }
        }

        /// <summary>
        /// Удалить группу
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST: /api/group/delete_group/4C2C522E-F785-4EB4-8ED7-260861453330
        /// </remarks>
        /// <response code="200">Удачно</response>
        /// <response code="400">Поле GroupId обязательно</response>
        /// <response code="404">Группа не найдена</response>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
        [HttpDelete("delete_group/{groupId}")]
        public async Task<IActionResult> DeleteGroup(Guid groupId)
        {
            try
            {
                if (groupId == Guid.Empty) return BadRequest(new Error { Message = "Поле GroupId обязательно" });

                await _groupRepository.DeleteGroup(groupId, _environment.WebRootPath);

                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(new Error { Message = e.Message });
            }
        }

        /// <summary>
        /// Удалить группу
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST: /api/group/group_details/4C2C522E-F785-4EB4-8ED7-260861453330
        /// </remarks>
        /// <response code="200">Удачно</response>
        /// <response code="400">Поле GroupId обязательно</response>
        /// <response code="404">Группа не найдена</response>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
        [HttpGet("group_details/{groupId}")]
        public async Task<IActionResult> GroupDetails(Guid groupId)
        {
            try
            {
                if (groupId == Guid.Empty) return BadRequest(new Error { Message = "Поле GroupId обязательно" });

                var result = await _groupRepository.GroupDetails(groupId);

                result.QRLink = result.QRLink.Insert(0, UrlRaw);

                return Ok(result);
            }
            catch (NotFoundException e)
            {
                return NotFound(new Error { Message = e.Message });
            }
        }
    }
}
