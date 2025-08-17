using Ardalis.Result;
using Microsoft.AspNetCore.Components.Forms;
using Polly;
using System.ComponentModel.Design;

namespace Frontend.Services
{
    public class MeterClientSvc
    {
        public readonly HttpService _httpService;

        public MeterClientSvc(HttpService httpService) 
        {
            _httpService = httpService;
        }

        public async Task<Result> UploadMeterReadings(IBrowserFile file)
        {
            if (file == null)
            {
                return Result.Error("No file provided");
            }

            var result = await _httpService.PostFileAsync("MeterReading/meter-reading-uploads", file);
            return result;
        }
    }
}
