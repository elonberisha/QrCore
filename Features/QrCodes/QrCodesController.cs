using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrEventApi.Infrastructure.Data;

namespace QrEventApi.Features.QrCodes;

[ApiController]
[Route("api/[controller]")]
public sealed class QrCodesController(AppDbContext dbContext, IQrCodeGeneratorService qrCodeGenerator) : ControllerBase
{
    [HttpPost]
    [Produces("image/png", "application/json")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Generate(CreateQrCodeRequest request)
    {
        var pngResult = TryGenerate(request);
        if (pngResult.ErrorMessage is not null)
        {
            return BadRequest(new { message = pngResult.ErrorMessage });
        }

        var record = ToRecord(request);
        dbContext.QrCodes.Add(record);
        await dbContext.SaveChangesAsync();

        Response.Headers["X-QR-Code-Id"] = record.Id.ToString();
        return File(pngResult.Png!, "image/png", $"{record.Id}.png");
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<QrCodeMetadataResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<QrCodeMetadataResponse>>> GetAll()
    {
        var records = await dbContext.QrCodes
            .OrderByDescending(qr => qr.CreatedAtUtc)
            .Select(qr => ToResponse(qr))
            .ToListAsync();

        return Ok(records);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(QrCodeMetadataResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QrCodeMetadataResponse>> GetById(Guid id)
    {
        var record = await dbContext.QrCodes.FindAsync(id);
        return record is null ? NotFound() : Ok(ToResponse(record));
    }

    [HttpGet("{id:guid}/png")]
    [Authorize]
    [Produces("image/png")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadSaved(Guid id)
    {
        var record = await dbContext.QrCodes.FindAsync(id);
        if (record is null)
        {
            return NotFound();
        }

        var png = qrCodeGenerator.GeneratePng(new CreateQrCodeRequest
        {
            EventName = record.EventName,
            Location = record.Location,
            Content = record.Content,
            ForegroundColor = record.ForegroundColor,
            BackgroundColor = record.BackgroundColor,
            Size = record.Size
        });

        return File(png, "image/png", $"{record.Id}.png");
    }

    [HttpDelete("history")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> ClearHistory()
    {
        var deleted = await dbContext.QrCodes.ExecuteDeleteAsync();
        return Ok(new { deleted });
    }

    private (byte[]? Png, string? ErrorMessage) TryGenerate(CreateQrCodeRequest request)
    {
        try
        {
            return (qrCodeGenerator.GeneratePng(request), null);
        }
        catch (FormatException)
        {
            return (null, "LogoBase64 duhet te jete Base64 valid ose null.");
        }
        catch (ArgumentException ex)
        {
            return (null, ex.Message);
        }
    }

    private static QrCodeRecord ToRecord(CreateQrCodeRequest request) =>
        new()
        {
            EventName = request.EventName,
            Location = request.Location,
            Content = request.Content,
            ForegroundColor = request.ForegroundColor,
            BackgroundColor = request.BackgroundColor,
            Size = request.Size,
            HasLogo = !string.IsNullOrWhiteSpace(request.LogoBase64) &&
                !string.Equals(request.LogoBase64, "null", StringComparison.OrdinalIgnoreCase)
        };

    private static QrCodeMetadataResponse ToResponse(QrCodeRecord record) =>
        new()
        {
            Id = record.Id,
            EventName = record.EventName,
            Location = record.Location,
            Content = record.Content,
            ForegroundColor = record.ForegroundColor,
            BackgroundColor = record.BackgroundColor,
            Size = record.Size,
            HasLogo = record.HasLogo,
            CreatedAtUtc = record.CreatedAtUtc
        };
}
