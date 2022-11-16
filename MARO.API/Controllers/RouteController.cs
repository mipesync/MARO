using MARO.API.Models;
using MARO.Application.Repository.RouteRepo;
using Microsoft.AspNetCore.Mvc;

namespace MARO.API.Controllers
{
    /*public class RouteController : BaseController
    {
        private readonly IRouteRepository _routeRepository;

        public RouteController(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        [HttpGet("build_route/{userId}")]
        public async Task<IActionResult> BuildRoute(Guid userId)
        {
            if (userId == Guid.Empty) return BadRequest(new Error { Message = "Поле UserId обязательно"});

            var routes = await _routeRepository.BuildRoute(userId);

            return Ok(routes);
        }
    }*/
}
