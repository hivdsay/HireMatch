using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireMatch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MeController : ControllerBase
{
    [HttpGet]
    public IActionResult GetMe()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        var email = User.FindFirstValue(
            ClaimTypes.Email);

        var role = User.FindFirstValue(
            ClaimTypes.Role);

        return Ok(new
        {
            UserId = userId,
            Email = email,
            Role = role
        });
    }
}