using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using ResolveCSV.Models;
using ResolveCSV.Services;
using System.Text.RegularExpressions;

namespace ResolveCSV.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class ParserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IParserService _parserService;
        private readonly IQueryService _queryService;
        private readonly ILogger<ParserController> _logger;

        public ParserController(IConfiguration configuration, IParserService parserService,
                                    IQueryService queryService, ILogger<ParserController> logger)
        {
            _configuration = configuration;
            _parserService = parserService;
            _queryService = queryService;
            _logger = logger;
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
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return persons;
        }

        [HttpGet("company")]
        public ActionResult<PersonDto> GetCompanyNameContains(string companyName)
        {
            var personDetails = FetchPersonDetails();
            var filteredByCompany = personDetails.Where(p => p.CompanyName.Contains(companyName));
            if (filteredByCompany.Count() == 0)
                return NotFound("The query yielded no results. Please check and try again.");
            return TransformPersonDetails(filteredByCompany);
        }

        [HttpGet("county")]
        public ActionResult<PersonDto> GetPeopleByCounty(string countyName)
        {
            var personDetails = FetchPersonDetails();
            var filteredByCounty = personDetails.Where(p => p.County.Contains(countyName));
            if (filteredByCounty.Count() == 0)
                return NotFound("The query yielded no results. Please check and try again.");
            return TransformPersonDetails(filteredByCounty);
        }

        [HttpGet("houseNumber")]
        public ActionResult<PersonDto> GetPeopleByHouseNumberDigitCount(int digits)
        {
            var personDetails = FetchPersonDetails();
            var filteredByDigits = personDetails.Where(p => p.Address.TakeWhile(char.IsDigit).ToArray().Count() == digits);
            if (filteredByDigits.Count() == 0)
                return NotFound("The query yielded no results. Please check and try again.");
            return TransformPersonDetails(filteredByDigits);
        }

        [HttpGet("website")]
        public ActionResult<PersonDto> GetPeopleByWebsiteLength(int length)
        {
            var personDetails = FetchPersonDetails();
            var filteredByWebsiteLength = personDetails.Where(p => p.Web.Length > length);
            if (filteredByWebsiteLength.Count() == 0)
                return NotFound("The query yielded no results. Please check and try again.");
            return TransformPersonDetails(filteredByWebsiteLength);
        }

        [HttpGet("postcode")]
        public ActionResult<PersonDto> GetPeopleByPostCodeArea(int areaDigits)
        {
            var personDetails = FetchPersonDetails();
            var filteredByAreaCodeDigits = personDetails.Where(p => Regex.Match(p.Postal.Split(' ')[0], @"\d+").Value.Length == areaDigits );
            if (filteredByAreaCodeDigits.Count() == 0)
                return NotFound("The query yielded no results. Please check and try again.");
            return TransformPersonDetails(filteredByAreaCodeDigits);
        }

        [HttpGet("phoneNumber")]
        public ActionResult<PersonDto> GetPeopleByPhoneNumberDifference()
        {
            var personDetails = FetchPersonDetails();
            var filteredByPhoneNumber = personDetails.Where(p => 
                    (int.TryParse(p.Phone1.Replace("-",""),out int x)?x:0) > (int.TryParse(p.Phone2.Replace("-", ""), out int y) ? y : 0));
            if (filteredByPhoneNumber.Count() == 0) 
                return NotFound("The query yielded no results. Please check and try again.");
            return TransformPersonDetails(filteredByPhoneNumber);
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

        private PersonDto TransformPersonDetails(IEnumerable<Person> persons)
        {
            if(persons == null || persons.Count() == 0)
                return null;

            PersonDto personDto = new PersonDto();
            personDto.PersonDetails = persons.Select(p => $"{p.Position} - {p.FirstName} {p.LastName} - {p.CompanyName} ").ToList();
            personDto.Count = personDto.PersonDetails.Count();

            return personDto;
        }
    }
}
