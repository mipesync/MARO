using MARO.API.Models;
using MARO.Application.Aggregate.Models.DTOs;
using MARO.Application.Common.Exceptions;
using MARO.Application.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MARO.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

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

        [HttpPut("update_items/{userId}")]
        public async Task<IActionResult> UpdateUserItems(Guid userId, long itemIds)
        {
            try
            {
                if (userId == Guid.Empty || itemIds == 0) return BadRequest(new Error { Message = "Обязательны все параметры!" });

                await _userRepository.UpdateUserCriteria(userId, itemIds);

                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(new Error { Message = e.Message });
            }
        }
    }
}
