using ResolveCSV.CustomParser;
using ResolveCSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ResolveCSV.Services
{
    public class ParserService : IParserService
    {
        private readonly ICustomParser _customParser;

        public ParserService(ICustomParser customParser)
        {
            _customParser = customParser;
        }

        #region Public Methods

        public List<T> ParseFileData<T>(string filePath, string delimiter) where T : IPopulatedFromCsv, new()
        {
            var data = GetFileData(filePath);

            return _customParser.Parse<T>(data, delimiter);
        }

        #endregion

        #region Private Methods

        private string GetFileData(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                throw new FileNotFoundException("File not found in the specified location. Please verify location and try again.");
            }
        }

        

        #endregion
    }

    
}
