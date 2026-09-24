using DocumentFormat.OpenXml.Packaging;
using HireMatch.Application.Abstractions.Services;
using UglyToad.PdfPig;

namespace HireMatch.Infrastructure.Services;

public class ResumeTextExtractor : IResumeTextExtractor
{
    public async Task<string> ExtractTextAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(fileName);

        return extension.ToLowerInvariant() switch
        {
            ".pdf" => await ExtractPdfTextAsync(
                fileStream,
                cancellationToken),

            ".docx" => await ExtractDocxTextAsync(
                fileStream,
                cancellationToken),

            _ => throw new InvalidOperationException(
                "Only PDF and DOCX files are supported.")
        };
    }

    private static Task<string> ExtractPdfTextAsync(
        Stream fileStream,
        CancellationToken cancellationToken)
    {
        using var document = PdfDocument.Open(fileStream);

        var text = string.Join(
            Environment.NewLine,
            document.GetPages()
                .Select(page => page.Text));

        return Task.FromResult(text);
    }

    private static async Task<string> ExtractDocxTextAsync(
        Stream fileStream,
        CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();

        await fileStream.CopyToAsync(
            memoryStream,
            cancellationToken);

        memoryStream.Position = 0;

        using var document = WordprocessingDocument.Open(
            memoryStream,
            false);

        var body = document.MainDocumentPart?
            .Document
            .Body;

        if (body is null)
        {
            return string.Empty;
        }

        return body.InnerText;
    }
}