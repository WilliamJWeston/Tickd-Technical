using API.Interfaces;
using Ardalis.Result;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Models.Dtos;
using Models.Maps;
using Models.Models;
using System.Globalization;

namespace API.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IRepositoryService _repository;
        private readonly ICsvReaderService _csvReader;

        public MeterReadingService(IRepositoryService repository, ICsvReaderService csvReaderService) 
        {
            _repository = repository;
            _csvReader = csvReaderService;
        }

        public async Task<Result<MeterReadingUploadResultDto>> UploadFile(IFormFile file)
        {
            // Check file
            if (file == null || file.Length == 0)
            {
                return Result.Error("No file uploaded.");
            }

            // Setup
            List<MeterReading> successfulRecords = new List<MeterReading>();
            List<string> failedRecords = new List<string>();
            int rowNumber = 0;

            using var stream = file.OpenReadStream();
            _csvReader.Initialise(stream);

            // Iterate through the CSV records (Would maybe want to batch this if I had enought time? Timeout concerns.)
            while (await _csvReader.ReadAsync()) 
            { 
                rowNumber++;
                try
                {
                    // Try reading the record
                    var record = _csvReader.GetRecord<MeterReading>();
                    if (record == null)
                    {
                        failedRecords.Add($"Row {rowNumber}: Invalid record format.");
                    }

                    // Ensure that the record is valid
                    var validationResult = await ValidateMeterReading(record);
                    if (!validationResult)
                    {
                        failedRecords.Add($"Row {rowNumber}: Invalid record content.");
                        continue;
                    }

                    // Check for duplicate values
                    var recordExists = await _repository.RecordExists(record);
                    if (recordExists)
                    {
                        failedRecords.Add($"Row {rowNumber}: Record exists already.");
                        continue;
                    }

                    // Save the record to DB
                    var saveResult = await _repository.SaveMeterReadingAsync(record);
                    if (saveResult.IsSuccess)
                    {
                        successfulRecords.Add(record);
                        continue;
                    }
                    else
                    {
                        failedRecords.Add($"Row {rowNumber}: Failed to save - {saveResult.Errors.FirstOrDefault() ?? "Unknown error"}");
                    }
                }
                catch (Exception ex)
                { 
                    failedRecords.Add($"{rowNumber}: {ex.Message}");
                }
            }

            MeterReadingUploadResultDto meterReadingUploadResult = new MeterReadingUploadResultDto() 
            { 
                SuccessfulRecords = successfulRecords.Count,
                FailedRecords = failedRecords.Count,
                TotalRecordsProcessed = rowNumber
            };

            return Result.Success(meterReadingUploadResult);
        }

        private async Task<bool> ValidateMeterReading(MeterReading record)
        {
            var accountExists = await _repository.AccountExists(record.AccountId);
            if (accountExists && record.MeterReadValue >= 0 && record.MeterReadValue.ToString().Length < 6)
            {
                return true;
            }

            return false;
        }
    }
}
