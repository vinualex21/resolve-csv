using NUnit.Framework;
using FluentAssertions;
using ResolveCSV.Services;
using ResolveCSV.Models;
using System.IO;
using System;
using System.Collections.Generic;
using ResolveCSV.CustomParser;
using Moq;

namespace ResolveCSVTests.ServiceTests
{
    public class ParserServiceTests
    {
        private IParserService _parserService;
        private Mock<ICustomParser> _mockCustomParser;

        [SetUp]
        public void Setup()
        {
            _mockCustomParser = new Mock<ICustomParser>();
            _parserService = new ParserService(_mockCustomParser.Object);
        }

        [Test]
        public void ParseFileData_FileDoesNotExist_ShouldThrowFileNotFoundException()
        {
            //Arrange
            string filePath = $"{Environment.CurrentDirectory}\\InputData\\input0.csv";
            string delimiter = ",";

            //Assert
            _parserService.Invoking(y => y.ParseFileData<Person>(filePath, delimiter))
                                .Should().Throw<FileNotFoundException>();
        }

        [Test]
        public void ParseCsv_GivenPersonDetails_ShouldReturnListofPerson()
        {
            //Arrange
            string filePath = $"{Environment.CurrentDirectory}\\InputData\\input.csv";
            var delimiter = ",";
            var personDetails = GetMockPersonDetails();
            _mockCustomParser.Setup(b => b.Parse<Person>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(personDetails);

            //Act
            var result = _parserService.ParseFileData<Person>(filePath, delimiter);

            //Asert
            result.Should().BeOfType<List<Person>>();
        }

        private List<Person> GetMockPersonDetails()
        {
            var persons = new List<Person>();
            var person = new Person()
            {
                FirstName = "John",
                LastName = "Wick",
                Address = "Parliament Street",
                City = "York",
                CompanyName = "Continental",
                County = "North Yorkshire",
                Email = "daisy@gmail.com",
                Phone1 = "01835-689397",
                Phone2 = "01835-295729",
                Postal = "YO12 4ND",
                Web = "http://www.johnwickkills.co.uk"
            };
            persons.Add(person);

            return persons;
        }

    }
}
