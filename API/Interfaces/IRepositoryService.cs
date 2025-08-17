using Ardalis.Result;
using Models.Models;

namespace API.Interfaces
{
    public interface IRepositoryService
    {
        Task<Result> SaveMeterReadingAsync(MeterReading reading);
    }
}
