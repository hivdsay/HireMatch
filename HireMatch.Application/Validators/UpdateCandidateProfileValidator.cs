using FluentValidation;
using HireMatch.Application.DTOs.Candidate;

namespace HireMatch.Application.Validators;

public class UpdateCandidateProfileValidator
    : AbstractValidator<UpdateCandidateProfileRequest>
{
    public UpdateCandidateProfileValidator()
    {
        RuleFor(x => x.Headline)
            .MaximumLength(200)
            .WithMessage("Headline cannot exceed 200 characters.");

        RuleFor(x => x.Bio)
            .MaximumLength(2000)
            .WithMessage("Bio cannot exceed 2000 characters.");

        RuleFor(x => x.YearsOfExperience)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Years of experience cannot be negative.")
            .LessThanOrEqualTo(50)
            .WithMessage("Years of experience cannot exceed 50.");
    }
}