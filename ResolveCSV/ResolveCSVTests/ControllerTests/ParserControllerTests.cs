using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using ResolveCSV.Controllers;
using ResolveCSV.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.IO;
using FluentAssertions;
using ResolveCSV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ResolveCSVTests.ControllerTests
{
    public class ParserControllerTests
    {
        private ParserController _parserController;
        private IConfiguration _configuration;
        private Mock<IParserService> _parserService;
        private Mock<IQueryService> _queryService;
        private Mock<ILogger<ParserController>> _logger;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(@"appsettings.json", false, false)
               .AddEnvironmentVariables()
               .Build();
            _parserService = new Mock<IParserService>();
            _queryService = new Mock<IQueryService>();
            _logger = new Mock<ILogger<ParserController>>();
            _parserController = new ParserController(_configuration, _parserService.Object, _queryService.Object, _logger.Object);
        }

        [Test]
        public void GetPersons_GivenOneRecordInFile_ShouldReturnOneRecord()
        {
            //Arrange
            var parsedResult = new List<Person>()
            {
                new Person()
                {
                    FirstName = "Rueben",
                    LastName =  "Gastellum",
                    CompanyName = "Industrial Engineering Assocs",
                    Address = "4 Forrest St",
                    City = "Weston-Super-Mare",
                    County = "North Somerset",
                    Postal = "BS23 3HG",
                    Phone1 = "01976-755279",
                    Phone2 = "01956-535511",
                    Email = "rueben_gastellum@gastellum.co.uk",
                    Web = "http://www.industrialengineeringassocs.co.uk"
                }
            };
            _parserService.Setup(p=>p.ParseFileData<Person>(It.IsAny<string>(), It.IsAny<string>())).Returns(parsedResult);
            _queryService.Setup(p => p.SetPosition(It.IsAny<List<Person>>())).Returns(parsedResult);

            //Act
            var result = _parserController.GetPersons();

            //Assert
            result.Value.Should().HaveCount(1);
        }

        [Test]
        public void GetPersons_FileDoesNotExist_ReturnsNotFoundActionResult()
        {
            //Arrange
            _parserService.Setup(p=>p.ParseFileData<Person>(It.IsAny<string>(),It.IsAny<string>())).Throws<FileNotFoundException>();

            //Act
            var result = _parserController.GetPersons();

            //Assert
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
        }
    }
}
