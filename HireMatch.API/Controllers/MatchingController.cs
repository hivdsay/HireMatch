using System.Security.Claims;
using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireMatch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MatchingController : ControllerBase
{
    private readonly IJobMatchingService _jobMatchingService;
    private readonly IResumeRepository _resumeRepository;

    public MatchingController(
        IJobMatchingService jobMatchingService,
        IResumeRepository resumeRepository)
    {
        _jobMatchingService = jobMatchingService;
        _resumeRepository = resumeRepository;
    }

    [HttpGet("resume/{resumeId}/job/{jobPostId}")]
    public async Task<IActionResult> Match(
        Guid resumeId,
        Guid jobPostId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var resume = await _resumeRepository.GetByIdAndUserIdAsync(
            resumeId,
            Guid.Parse(userId),
            cancellationToken);

        if (resume is null)
        {
            return NotFound("Resume not found.");
        }

        var result = await _jobMatchingService.MatchAsync(
            resumeId,
            jobPostId,
            cancellationToken);

        return Ok(result);
    }
}