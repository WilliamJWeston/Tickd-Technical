using API.Interfaces;
using API.Services;
using Ardalis.Result;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Models.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.UnitTests.Tests.Services
{
    public class MeterReadingServiceTests
    {
        [Fact]
        public async Task UploadFile_Fails_ForNullInput()
        {
            // Arrange
            var mockRepo = new Mock<IRepositoryService>();
            var mockCsv = new Mock<ICsvReaderService>();
            var service = new MeterReadingService(mockRepo.Object, mockCsv.Object);

            // Act
            var result = await service.UploadFile(null);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("No file uploaded.");
        }

    }
}
