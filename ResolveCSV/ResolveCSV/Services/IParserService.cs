using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResolveCSV.Services
{
    public interface IParserService
    {
        public List<T> ParsePersonDetails<T>(string filePath, string delimiter);

        public List<T> Parse<T>(string data, string delimiter);
    }
}
