using NUnit.Framework;
using FluentAssertions;
using ResolveCSV.Services;
using ResolveCSV.Models;
using System.IO;
using System;

namespace ResolveCSVTests.ServiceTests
{
    public class ParserServiceTests
    {
        private IParserService parserService;

        [SetUp]
        public void Setup()
        {
            parserService = new ParserService();
        }

        [Test]
        public void ParsePersonDeails_FileNotFound_ShouldThrowFileNotFoundException()
        {
            string folderName = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $"{Environment.CurrentDirectory}\\Data\\input0.csv";
            string delimiter = ",";

            parserService.Invoking(y => y.ParsePersonDetails<Person>(filePath, delimiter))
                                .Should().Throw<FileNotFoundException>();
        }
    }
}
