using Microsoft.AspNetCore.Mvc;
using QrEventApi.Features.QrCodes;

namespace QrEventApi.Features.Images;

[ApiController]
[Route("api/images")]
public sealed class ImageUploadController(
    IImageUploadService uploadService,
    IQrCodeGeneratorService qrGenerator) : ControllerBase
{
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [Produces("image/png", "application/json")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { message = "Imazhi mungon ose eshte bosh." });

        string imageUrl;
        try
        {
            imageUrl = await uploadService.UploadAsync(file);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { message = $"Ngarkimi ne imgbb deshtoi: {ex.Message}" });
        }

        var png = qrGenerator.GeneratePng(new CreateQrCodeRequest { Content = imageUrl });

        Response.Headers["X-Image-Url"] = imageUrl;
        return File(png, "image/png", "image-qr.png");
    }
}
