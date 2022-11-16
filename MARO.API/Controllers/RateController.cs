using MARO.API.Models;
using MARO.Application.Aggregate.Models.DTOs;
using MARO.Application.Aggregate.Models.ResponseModels;
using MARO.Application.Common.Exceptions;
using MARO.Application.Repository.RateRepo;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MARO.API.Controllers
{
    public class RateController : BaseController
    {
        private readonly IRateRepository _rateRepository;

        public RateController(IRateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }

        /// <summary>
        /// Добавление оценок мест для обучения рекомендательной модели
        /// </summary>
        /// <remarks>
        /// Оценка должна быть от 1 до 5 включительно.
        /// Пример запроса:
        /// 
        ///     POST: /api/rate/add_rate
        ///     {
        ///         "userId": "4C2C522E-F785-4EB4-8ED7-260861453330",
        ///         "placeId": "B415AFB5-5DA4-485C-9EB0-A47A58A28731",
        ///         "rate": 4
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Возврат <see cref="RegisterResponseModel"/></returns>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="400">Некорректная оценка</response>
        /// <response code="200">Удачно</response>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(Error))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(Error))]
        [HttpPost("add_rate")]
        public async Task<IActionResult> AddRate(AddRateDto model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new Error { Message = "Некорректные данные" });

                await _rateRepository.AddRate(model);

                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(new Error { Message = e.Message });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new Error { Message = e.Message });
            }
        }
    }
}
