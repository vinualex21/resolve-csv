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
            List<Person> persons = null;
            try
            {
                string relativeFilePath = _configuration[Constants.Configuration.InputFilePath];
                string filePath = $"{Environment.CurrentDirectory}{relativeFilePath}";
                persons = _parserService.ParseFileData<Person>(filePath, ",");
            }
            catch(FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return persons;
        }
    }
}
