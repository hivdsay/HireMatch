using System.Security.Claims;
using HireMatch.Application.Commands.Company;
using HireMatch.Application.DTOs.Company;
using HireMatch.Application.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireMatch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Employer,Admin")]
public class CompanyController : ControllerBase
{
    private readonly CreateCompanyHandler _createCompanyHandler;

    public CompanyController(
        CreateCompanyHandler createCompanyHandler)
    {
        _createCompanyHandler = createCompanyHandler;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany(
        CreateCompanyRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var command = new CreateCompanyCommand(
            request.Name,
            request.Description,
            request.Website,
            request.Location);

        var companyId = await _createCompanyHandler.Handle(
            Guid.Parse(userId),
            command,
            cancellationToken);

        return Ok(new
        {
            CompanyId = companyId
        });
    }
}