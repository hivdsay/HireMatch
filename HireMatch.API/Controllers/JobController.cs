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
    private readonly GetJobPostsHandler _getJobPostsHandler;
    private readonly GetJobPostByIdHandler _getJobPostByIdHandler;

    public JobController(
        CreateJobPostHandler createJobPostHandler,
        GetJobPostsHandler getJobPostsHandler,
        GetJobPostByIdHandler getJobPostByIdHandler)
    {
        _createJobPostHandler = createJobPostHandler;
        _getJobPostsHandler = getJobPostsHandler;
        _getJobPostByIdHandler = getJobPostByIdHandler;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetJobPosts(
        [FromQuery] JobPostFilterRequest filter,
        CancellationToken cancellationToken)
    {
        var jobPosts = await _getJobPostsHandler.Handle(
            filter,
            cancellationToken);

        return Ok(jobPosts);
    }
    
    [HttpGet("{jobPostId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetJobPostById(
        Guid jobPostId,
        CancellationToken cancellationToken)
    {
        var jobPost = await _getJobPostByIdHandler.Handle(
            jobPostId,
            cancellationToken);

        return Ok(jobPost);
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