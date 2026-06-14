using System.Text.Json.Serialization;

namespace QrEventApi.Features.Images;

public interface IImageUploadService
{
    Task<string> UploadAsync(IFormFile file);
}

public sealed class ImageUploadService(HttpClient http, IConfiguration config) : IImageUploadService
{
    public async Task<string> UploadAsync(IFormFile file)
    {
        var apiKey = config["ImgBb:ApiKey"]
            ?? throw new InvalidOperationException("ImgBb:ApiKey nuk eshte konfiguruar.");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var base64 = Convert.ToBase64String(ms.ToArray());

        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(apiKey), "key");
        content.Add(new StringContent(base64), "image");

        var response = await http.PostAsync("https://api.imgbb.com/1/upload", content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ImgBbResponse>()
            ?? throw new InvalidOperationException("Pergjigja nga imgbb nuk mund te lexohet.");

        return result.Data.Url;
    }
}

file sealed record ImgBbResponse(
    [property: JsonPropertyName("data")] ImgBbData Data);

file sealed record ImgBbData(
    [property: JsonPropertyName("url")] string Url);
