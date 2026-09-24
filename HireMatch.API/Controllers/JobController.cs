using System.Security.Claims;
using HireMatch.Application.Commands.Job;
using HireMatch.Application.DTOs.Job;
using HireMatch.Application.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireMatch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Employer,Admin")]
public class JobController : ControllerBase
{
    private readonly CreateJobPostHandler _createJobPostHandler;

    public JobController(
        CreateJobPostHandler createJobPostHandler)
    {
        _createJobPostHandler = createJobPostHandler;
    }

    [HttpPost]
    public async Task<IActionResult> CreateJobPost(
        CreateJobPostRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var command = new CreateJobPostCommand(
            request.Title,
            request.Description,
            request.Location,
            request.JobType,
            request.WorkMode,
            request.CompanyId,
            request.RequiredSkills);

        var jobPostId = await _createJobPostHandler.Handle(
            Guid.Parse(userId),
            command,
            cancellationToken);

        return Ok(new
        {
            JobPostId = jobPostId
        });
    }
}