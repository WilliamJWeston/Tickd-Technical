using API.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using Models.Maps;
using System.Globalization;

namespace API.Services
{
    public class CsvReaderService : ICsvReaderService
    {
        private CsvReader _csvReader;
        private CsvConfiguration _csvConfiguration;

        public CsvReaderService()
        {
            _csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                MissingFieldFound = null
            };

            _csvReader = null;
        }

        public void Initialise(Stream stream)
        {
            var reader = new StreamReader(stream);
            _csvReader = new CsvHelper.CsvReader(reader, _csvConfiguration);
            _csvReader.Context.RegisterClassMap<MeterReadingMap>(); // Apply mapping (SRP)
            _csvReader.Read(); // Read the header as in original code
            _csvReader.ReadHeader();
        }

        public async Task<bool> ReadAsync()
        {
            return await _csvReader.ReadAsync();
        }

        public T GetRecord<T>() where T : class
        {
            return _csvReader.GetRecord<T>();
        }
    }
}
