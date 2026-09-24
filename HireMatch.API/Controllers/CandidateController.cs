using System.Security.Claims;
using HireMatch.Application.Abstractions.Services;
using HireMatch.Application.DTOs.Candidate;
using HireMatch.Application.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireMatch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Candidate")]
public class CandidateController : ControllerBase
{
    private readonly GetCandidateProfileHandler _getCandidateProfileHandler;
    private readonly UpdateCandidateProfileHandler _updateCandidateProfileHandler;
    private readonly CreateResumeHandler _createResumeHandler;
    private readonly IFileStorageService _fileStorageService;
    private readonly IResumeTextExtractor _resumeTextExtractor;
    private readonly ExtractResumeSkillsHandler _extractResumeSkillsHandler;

    public CandidateController(
        GetCandidateProfileHandler getCandidateProfileHandler,
        UpdateCandidateProfileHandler updateCandidateProfileHandler,
        CreateResumeHandler createResumeHandler,
        IFileStorageService fileStorageService,
        IResumeTextExtractor resumeTextExtractor,
        ExtractResumeSkillsHandler extractResumeSkillsHandler)
    {
        _getCandidateProfileHandler = getCandidateProfileHandler;
        _updateCandidateProfileHandler = updateCandidateProfileHandler;
        _createResumeHandler = createResumeHandler;
        _fileStorageService = fileStorageService;
        _resumeTextExtractor = resumeTextExtractor;
        _extractResumeSkillsHandler = extractResumeSkillsHandler;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var profile = await _getCandidateProfileHandler.Handle(
            Guid.Parse(userId),
            cancellationToken);

        if (profile is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            profile.Id,
            profile.UserId,
            profile.Headline,
            profile.Bio,
            profile.YearsOfExperience,
            profile.CreatedAt
        });
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(
        UpdateCandidateProfileRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        await _updateCandidateProfileHandler.Handle(
            Guid.Parse(userId),
            request,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("resume")]
    public async Task<IActionResult> CreateResume(
        CreateResumeRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var resumeId = await _createResumeHandler.Handle(
            Guid.Parse(userId),
            request,
            null,
            cancellationToken);

        return Ok(new
        {
            ResumeId = resumeId
        });
    }

    [HttpPost("resume/upload")]
    public async Task<IActionResult> UploadResume(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        if (file.Length == 0)
        {
            return BadRequest("File cannot be empty.");
        }

        const long maxFileSize = 10 * 1024 * 1024;

        if (file.Length > maxFileSize)
        {
            return BadRequest(
                "File size cannot exceed 10 MB.");
        }

        var allowedExtensions = new[]
        {
            ".pdf",
            ".docx"
        };

        var extension = Path.GetExtension(file.FileName);

        if (!allowedExtensions.Contains(
                extension,
                StringComparer.OrdinalIgnoreCase))
        {
            return BadRequest(
                "Only PDF and DOCX files are allowed.");
        }

        await using var fileStream = file.OpenReadStream();

        var extractedText =
            await _resumeTextExtractor.ExtractTextAsync(
                fileStream,
                file.FileName,
                cancellationToken);

        fileStream.Position = 0;

        var fileUrl = await _fileStorageService.SaveAsync(
            fileStream,
            file.FileName,
            cancellationToken);

        var request = new CreateResumeRequest
        {
            FileName = file.FileName,
            FileUrl = fileUrl
        };

        var resumeId = await _createResumeHandler.Handle(
            Guid.Parse(userId),
            request,
            extractedText,
            cancellationToken);

        await _extractResumeSkillsHandler.Handle(
            resumeId,
            cancellationToken);

        return Ok(new
        {
            ResumeId = resumeId,
            FileUrl = fileUrl,
            ExtractedText = extractedText
        });
    }
}