using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeterReadingController : ControllerBase
    {

        [HttpPost("meter-reading-uploads")]
        public async Task<Result> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Result.Error("No file uploaded.");
            }

            using var stream = new Stream
        }
    }
}
