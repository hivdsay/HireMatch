namespace HireMatch.Application.Abstractions.Services;

public interface IFileStorageService
{
    Task<string> SaveAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default);
}