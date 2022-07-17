using ResolveCSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResolveCSV.CustomParser
{
    public interface ICustomParser
    {
        public List<T> Parse<T>(string filePath, string delimiter) where T : IPopulatedFromCsv, new();
    }
}
