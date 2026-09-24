using HireMatch.Application.Abstractions.Services;

namespace HireMatch.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _uploadPath;

    public LocalFileStorageService()
    {
        _uploadPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "uploads");

        Directory.CreateDirectory(_uploadPath);
    }

    public async Task<string> SaveAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var uniqueFileName =
            $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";

        var filePath = Path.Combine(
            _uploadPath,
            uniqueFileName);

        await using var outputStream = new FileStream(
            filePath,
            FileMode.Create);

        await fileStream.CopyToAsync(
            outputStream,
            cancellationToken);

        return $"/uploads/{uniqueFileName}";
    }
}