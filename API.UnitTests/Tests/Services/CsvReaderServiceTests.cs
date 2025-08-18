using API.Services;
using FluentAssertions;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.UnitTests.Tests.Services
{
    public class CsvReaderServiceTests
    {
        [Fact]
        public async Task ValidCsv_IsParsedSuccessfully()
        {
            // Arrange: 
            var csvContent = "AccountId,MeterReadingDateTime,MeterReadValue\n" + "4321,01/01/2025 11:11,1234\n";

            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvContent));
            var service = new CsvReaderService();
            service.Initialise(stream);

            // Act
            var hasRow = await service.ReadAsync();
            var record = service.GetRecord<MeterReading>();

            // Assert
            hasRow.Should().BeTrue();
            record.AccountId.Should().Be(4321);
            record.MeterReadValue.Should().Be(1234);
            record.MeterReadingDateTime.Should().Be(new DateTime(2025, 1, 1, 11, 11, 0));
        }
    }
}
