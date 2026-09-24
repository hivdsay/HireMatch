using HireMatch.Application.Commands.Auth;
using HireMatch.Application.DTOs.Auth;
using HireMatch.Application.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace HireMatch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly LoginHandler _loginHandler;

    public LoginController(LoginHandler loginHandler)
    {
        _loginHandler = loginHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _loginHandler.Handle(
            new LoginCommand(
                request.Email,
                request.Password),
            cancellationToken);

        return Ok(new
        {
            AccessToken = token
        });
    }
}