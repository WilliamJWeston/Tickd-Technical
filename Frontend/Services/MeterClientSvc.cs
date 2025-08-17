namespace Frontend.Services
{
    public class MeterClientSvc
    {
        public readonly HttpClient _httpClient;

        public MeterClientSvc(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task UploadMeterReadings()
        {
            var result = _httpClient.P
        }
    }
}
