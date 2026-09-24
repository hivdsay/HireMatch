using System.Security.Claims;
using HireMatch.Application.Commands.Job;
using HireMatch.Application.Handlers;
using HireMatch.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireMatch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Employer,Admin")]
public class EmployerController : ControllerBase
{
    private readonly GetJobApplicationsHandler _getJobApplicationsHandler;
    private readonly UpdateJobApplicationStatusHandler _updateJobApplicationStatusHandler;

    public EmployerController(
        GetJobApplicationsHandler getJobApplicationsHandler,
        UpdateJobApplicationStatusHandler updateJobApplicationStatusHandler)
    {
        _getJobApplicationsHandler = getJobApplicationsHandler;
        _updateJobApplicationStatusHandler =
            updateJobApplicationStatusHandler;
    }

    [HttpGet("jobs/{jobPostId:guid}/applications")]
    public async Task<IActionResult> GetJobApplications(
        Guid jobPostId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var applications =
            await _getJobApplicationsHandler.Handle(
                Guid.Parse(userId),
                jobPostId,
                cancellationToken);

        return Ok(applications);
    }
    
    [HttpPut("applications/{applicationId:guid}/status")]
    public async Task<IActionResult> UpdateApplicationStatus(
        Guid applicationId,
        ApplicationStatus status,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var command = new UpdateJobApplicationStatusCommand(
            applicationId,
            status);

        await _updateJobApplicationStatusHandler.Handle(
            Guid.Parse(userId),
            command,
            cancellationToken);

        return NoContent();
    }
}