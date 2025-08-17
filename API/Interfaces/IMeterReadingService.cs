using Ardalis.Result;
using Models.Dtos;

namespace API.Interfaces
{
    public interface IMeterReadingService
    {
        Task<Result<MeterReadingUploadResultDto>> UploadFile(IFormFile file);
    }
}
