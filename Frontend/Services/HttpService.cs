using Ardalis.Result;
using Frontend.Interfaces;
using Microsoft.AspNetCore.Components.Forms;

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

        public async Task<Result> PostFileAsync(string path, IBrowserFile file)
        {
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024)), "file", file.Name);

                // Post content
                var response = await _httpClient.PostAsync($"api/{path}", content);
                if (response.IsSuccessStatusCode)
                {
                    return Result.Success();
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
