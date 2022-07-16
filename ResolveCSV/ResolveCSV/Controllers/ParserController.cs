using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using ResolveCSV.Models;
using ResolveCSV.Services;

namespace ResolveCSV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IParserService _parserService;

        public ParserController(IConfiguration configuration, IParserService parserService)
        {
            _configuration = configuration;
            _parserService = parserService;
        }

        [HttpGet]
        public ActionResult<List<Person>> GetPersons()
        {
            string relativeFilePath = _configuration[Constants.Configuration.InputFilePath];
            string filePath = $"{Environment.CurrentDirectory}{relativeFilePath}";
            var persons = _parserService.ParseFileData<Person>(filePath, ",");
            return persons;
        }
    }
}
