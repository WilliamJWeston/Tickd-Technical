using Ardalis.Result;
using Microsoft.AspNetCore.Components.Forms;
using Polly;
using System.ComponentModel.Design;

namespace Frontend.Services
{
    public class MeterClientSvc
    {
        public readonly HttpService _httpClient;

        public MeterClientSvc(HttpService httpService) 
        {
            _httpClient = httpService;
        }

        public async Task<Result> UploadMeterReadings(IBrowserFile file)
        {
            if (file == null)
            {
                return Result.Error("No file provided");
            }

            return await _httpClient.PostFileAsync("MeterReading/meter-reading-uploads ", file);
        }
    }
}
