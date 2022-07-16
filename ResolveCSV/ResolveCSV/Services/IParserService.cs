using ResolveCSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResolveCSV.Services
{
    public interface IParserService
    {
        public List<T> ParseFileData<T>(string filePath, string delimiter) where T : IPopulatedFromCsv, new();

    }
}
