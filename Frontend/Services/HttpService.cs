using Ardalis.Result;
using Frontend.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;

namespace Frontend.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpService> _logger;

        public HttpService(HttpClient httpClient, ILogger<HttpService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Result<T>> PostFileAsync<T>(string path, IBrowserFile file)
        {
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024)), "file", file.Name);

                // Post content
                var response = await _httpClient.PostAsync($"api/{path}", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<Result<T>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return result ?? Result.Error("Failed to deserialize server response");
                }
                else
                {
                    return Result.Error();
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Unexpected error posting meter reading file");
                return Result.Error($"Unexpected error: {ex.Message}");
            }
        }
    }
}
