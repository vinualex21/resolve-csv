using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResolveCSV.Services
{
    public class ParserService : IParserService
    {
        public List<T> ParsePersonDetails<T>(string filePath, string delimiter)
        {
            if(File.Exists(filePath))
            {
                List<T> persons = new List<T>();

                return persons;
            }
            else
            {
                throw new FileNotFoundException("File not found in the specified location. Please verify location and try again.");
            }
        }

        public List<T> Parse<T>(string data, string delimiter)
        {
            throw new NotImplementedException();
        }
    }
}
