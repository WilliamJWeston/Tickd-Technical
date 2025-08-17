using Ardalis.Result;
using CsvHelper;
using CsvHelper.Configuration;
using Models.Models;
using System.Globalization;

namespace API.Services
{
    public class MeterReadingService
    {
        public MeterReadingService() { }

        public async Task<Result> UploadFile(IFormFile file)
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

            // Read the CSV header
            await csv.ReadAsync();
            csv.ReadHeader();

            // Iterate through the CSV records
            while (await csv.ReadAsync()) 
            { 
                rowNumber++;
                try
                {
                    var record = csv.GetRecord<MeterReading>();
                    successfulRecords.Add(record);
                }
                catch (Exception ex)
                { 
                    failedRecords.Add($"{rowNumber}: {ex.Message}");
                }
            }

            return Result.Success();
        }
    }
}
