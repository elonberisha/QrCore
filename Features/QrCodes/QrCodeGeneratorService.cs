using QRCoder;
using SkiaSharp;

namespace QrEventApi.Features.QrCodes;

public interface IQrCodeGeneratorService
{
    byte[] GeneratePng(CreateQrCodeRequest request);
}

public sealed class QrCodeGeneratorService : IQrCodeGeneratorService
{
    public byte[] GeneratePng(CreateQrCodeRequest request)
    {
        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(BuildPayload(request), ToErrorCorrectionLevel(request.ErrorCorrection));

        var matrix = data.ModuleMatrix;
        var moduleCount = matrix.Count;
        var quietZone = request.QuietZoneModules;
        var totalModules = moduleCount + quietZone * 2;
        var pixelsPerModule = Math.Max(1, request.Size / totalModules);
        var imageSize = pixelsPerModule * totalModules;

        using var bitmap = new SKBitmap(imageSize, imageSize);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(ParseColor(request.BackgroundColor));

        using var darkPaint = new SKPaint
        {
            Color = ParseColor(request.ForegroundColor),
            Style = SKPaintStyle.Fill,
            IsAntialias = false
        };

        for (var y = 0; y < moduleCount; y++)
        {
            for (var x = 0; x < moduleCount; x++)
            {
                if (!matrix[y][x])
                {
                    continue;
                }

                canvas.DrawRect(
                    SKRect.Create((x + quietZone) * pixelsPerModule, (y + quietZone) * pixelsPerModule, pixelsPerModule, pixelsPerModule),
                    darkPaint);
            }
        }

        DrawLogoIfPresent(canvas, imageSize, request.LogoBase64, request.LogoSizePercent);

        using var image = SKImage.FromBitmap(bitmap);
        using var encoded = image.Encode(SKEncodedImageFormat.Png, 100);
        return encoded.ToArray();
    }

    private static string BuildPayload(CreateQrCodeRequest request)
    {
        if (!request.IncludeEventDetails)
        {
            return request.Content;
        }

        return string.Join(Environment.NewLine, new[]
        {
            request.EventName,
            request.Location,
            request.Content
        }.Where(value => !string.IsNullOrWhiteSpace(value)));
    }

    private static QRCodeGenerator.ECCLevel ToErrorCorrectionLevel(string value) =>
        value.ToUpperInvariant() switch
        {
            "L" => QRCodeGenerator.ECCLevel.L,
            "M" => QRCodeGenerator.ECCLevel.M,
            "Q" => QRCodeGenerator.ECCLevel.Q,
            _ => QRCodeGenerator.ECCLevel.H
        };

    private static SKColor ParseColor(string hex)
    {
        if (!SKColor.TryParse(hex, out var color))
        {
            throw new ArgumentException($"Ngjyra '{hex}' nuk eshte valide.");
        }

        return color;
    }

    private static void DrawLogoIfPresent(SKCanvas canvas, int imageSize, string? logoBase64, int logoSizePercent)
    {
        if (string.IsNullOrWhiteSpace(logoBase64) ||
            string.Equals(logoBase64, "null", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var commaIndex = logoBase64.IndexOf(',');
        var cleanBase64 = commaIndex >= 0 ? logoBase64[(commaIndex + 1)..] : logoBase64;
        var logoBytes = Convert.FromBase64String(cleanBase64);

        using var logo = TryDecodeLogo(logoBytes);
        if (logo is null)
        {
            throw new ArgumentException("LogoBase64 nuk mund te lexohet si imazh.");
        }

        var logoSize = imageSize * (logoSizePercent / 100f);
        var logoPadding = logoSize * 0.18f;
        var left = (imageSize - logoSize) / 2f;
        var top = (imageSize - logoSize) / 2f;

        using var backgroundPaint = new SKPaint
        {
            Color = SKColors.White,
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        canvas.DrawRoundRect(
            SKRect.Create(left - logoPadding, top - logoPadding, logoSize + logoPadding * 2, logoSize + logoPadding * 2),
            logoPadding,
            logoPadding,
            backgroundPaint);

        canvas.DrawBitmap(logo, SKRect.Create(left, top, logoSize, logoSize));
    }

    private static SKBitmap? TryDecodeLogo(byte[] logoBytes)
    {
        try
        {
            return SKBitmap.Decode(logoBytes);
        }
        catch
        {
            return null;
        }
    }
}
