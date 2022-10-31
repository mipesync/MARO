using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MARO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        protected string UserId => !User.Identity!.IsAuthenticated
            ? String.Empty
            : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        protected string UrlRaw => $"{Request.Scheme}://{Request.Host}";
    }
}
