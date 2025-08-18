using API.Interfaces;
using API.Services;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeterReadingController : ControllerBase
    {
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingController(IMeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }

        [HttpPost("meter-reading-uploads")]
        public async Task<Result<MeterReadingUploadResultDto>> UploadFile(IFormFile file)
        {
            var result = await _meterReadingService.UploadFile(file);
            return result;
        }
    }
}
