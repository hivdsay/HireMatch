namespace HireMatch.Application.Abstractions.Services;

public interface IResumeTextExtractor
{
    Task<string> ExtractTextAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default);
}