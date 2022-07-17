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
        private readonly IQueryService _queryService;
        private readonly ILogger<ParserController> _logger;
        private List<Person> _personDetails;

        public ParserController(IConfiguration configuration, IParserService parserService, 
                                    IQueryService queryService, ILogger<ParserController> logger)
        {
            _configuration = configuration;
            _parserService = parserService;
            _queryService = queryService;
            _logger = logger;
            _personDetails = FetchPersonDetails();
        }

        /// <summary>
        /// Get details of every person in the input file
        /// </summary>
        /// <returns>Position in list, full name, company name</returns>
        [HttpGet]
        public ActionResult<List<Person>> GetPersons()
        {
            List<Person> persons = null;
            try
            {
                string relativeFilePath = _configuration[Constants.Configuration.InputFilePath];
                string filePath = $"{Environment.CurrentDirectory}{relativeFilePath}";
                persons = _parserService.ParseFileData<Person>(filePath, ",");
                persons = _queryService.SetPosition(persons);
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

        private List<Person> FetchPersonDetails()
        {
            List<Person> persons = null;
            try
            {
                string relativeFilePath = _configuration[Constants.Configuration.InputFilePath];
                string filePath = $"{Environment.CurrentDirectory}{relativeFilePath}";
                persons = _parserService.ParseFileData<Person>(filePath, ",");
                persons = _queryService.SetPosition(persons);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
            return persons;
        }
    }
}
