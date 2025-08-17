using API.Interfaces;
using Ardalis.Result;
using Models.Models;

namespace API.Services
{
    public class RepositoryService : IRepositoryService
    {
        public Task<Result> SaveMeterReadingAsync(MeterReading reading)
        {
            return Task.FromResult(Result.Success());
        }
    }
}
