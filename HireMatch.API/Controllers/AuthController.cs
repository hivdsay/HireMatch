using HireMatch.Application.Commands.Auth;
using HireMatch.Application.DTOs.Auth;
using HireMatch.Application.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace HireMatch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterCandidateHandler _registerCandidateHandler;
    private readonly RegisterEmployerHandler _registerEmployerHandler;

    public AuthController(
        RegisterCandidateHandler registerCandidateHandler,
        RegisterEmployerHandler registerEmployerHandler)
    {
        _registerCandidateHandler = registerCandidateHandler;
        _registerEmployerHandler = registerEmployerHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterCandidate(
        RegisterCandidateRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCandidateCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        var userId = await _registerCandidateHandler.Handle(
            command,
            cancellationToken);

        return Ok(new
        {
            UserId = userId
        });
    }

    [HttpPost("register/employer")]
    public async Task<IActionResult> RegisterEmployer(
        RegisterEmployerRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterEmployerCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        var userId = await _registerEmployerHandler.Handle(
            command,
            cancellationToken);

        return Ok(new
        {
            UserId = userId
        });
    }
}