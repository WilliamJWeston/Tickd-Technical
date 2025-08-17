using API.Services;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeterReadingController : ControllerBase
    {
        private readonly MeterReadingService _meterReadingService;

        public MeterReadingController(MeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }

        [HttpPost("meter-reading-uploads")]
        public async Task<Result> UploadFile(IFormFile file)
        {
            var result = await _meterReadingService.UploadFile(file);
            return result;
        }
    }
}
