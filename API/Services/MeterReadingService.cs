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
        private readonly RepositoryService _repository;
        public MeterReadingService(RepositoryService repository) 
        {
            _repository = repository;
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
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                MissingFieldFound = null
            });
            csv.Context.RegisterClassMap<MeterReadingMap>();

            // Read the CSV header
            await csv.ReadAsync();
            csv.ReadHeader();

            // Iterate through the CSV records
            while (await csv.ReadAsync()) 
            { 
                rowNumber++;
                try
                {
                    // Try reading the record
                    var record = csv.GetRecord<MeterReading>();
                    if (record == null)
                    {
                        failedRecords.Add($"Row {rowNumber}: Invalid record format.");
                    }

                    // Save the record to DB
                    var saveResult = await _repository.SaveMeterReadingAsync(record);
                    if (saveResult.IsSuccess)
                    {
                        successfulRecords.Add(record);
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
    }
}
