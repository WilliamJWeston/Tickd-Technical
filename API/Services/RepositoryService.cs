using API.Interfaces;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace API.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly TickdDbContext _dbContext;

        public RepositoryService(TickdDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> SaveMeterReadingAsync(MeterReading reading)
        {
            try
            {
                await _dbContext.MeterReadings.AddAsync(reading);
                await _dbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error();
            }
        }

        public async Task<bool> AccountExists(int accountId)
        {
            var account = await _dbContext.Accounts.SingleOrDefaultAsync(s => s.AccountId == accountId);
            if (account != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RecordExists(MeterReading reading)
        {
            var record = await _dbContext.MeterReadings
                .Where(s => s.AccountId == reading.AccountId)
                .Where(s => s.MeterReadingDateTime == reading.MeterReadingDateTime)
                .SingleOrDefaultAsync();

            if (record != null)
            {
                return true;
            }

            return false;
        }
    }
}
