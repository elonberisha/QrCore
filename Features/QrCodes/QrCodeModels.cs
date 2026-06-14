using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QrEventApi.Features.QrCodes;

public sealed class QrCodeRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(128)]
    public string? EventName { get; set; }

    [MaxLength(256)]
    public string? Location { get; set; }

    [Required]
    [MaxLength(4000)]
    public string Content { get; set; } = string.Empty;

    [MaxLength(16)]
    public string ForegroundColor { get; set; } = "#000000";

    [MaxLength(16)]
    public string BackgroundColor { get; set; } = "#FFFFFF";

    public int Size { get; set; } = 512;
    public bool HasLogo { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

public sealed class CreateQrCodeRequest
{
    [MaxLength(128)]
    public string? EventName { get; set; }

    [MaxLength(256)]
    public string? Location { get; set; }

    [Required]
    [MaxLength(4000)]
    public string Content { get; set; } = string.Empty;

    [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$")]
    [DefaultValue("#000000")]
    public string ForegroundColor { get; set; } = "#000000";

    [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$")]
    [DefaultValue("#FFFFFF")]
    public string BackgroundColor { get; set; } = "#FFFFFF";

    [Range(128, 2048)]
    [DefaultValue(512)]
    public int Size { get; set; } = 512;

    public string? LogoBase64 { get; set; }

    [Range(8, 30)]
    [DefaultValue(18)]
    public int LogoSizePercent { get; set; } = 18;

    [Range(0, 8)]
    [DefaultValue(4)]
    public int QuietZoneModules { get; set; } = 4;

    [DefaultValue(false)]
    public bool IncludeEventDetails { get; set; }

    [RegularExpression("^(L|M|Q|H)$")]
    [DefaultValue("H")]
    public string ErrorCorrection { get; set; } = "H";
}

public sealed class QrCodeMetadataResponse
{
    public Guid Id { get; set; }
    public string? EventName { get; set; }
    public string? Location { get; set; }
    public string Content { get; set; } = string.Empty;
    public string ForegroundColor { get; set; } = "#000000";
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public int Size { get; set; }
    public bool HasLogo { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
