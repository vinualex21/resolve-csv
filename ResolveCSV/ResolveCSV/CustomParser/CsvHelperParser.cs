using CsvHelper;
using CsvHelper.Configuration;
using ResolveCSV.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResolveCSV.CustomParser
{
    public class CsvHelperParser : ICustomParser
    {
        public List<T> Parse<T>(string filePath, string delimiter) where T : IPopulatedFromCsv, new()
        {
            List<T> dataItems = new List<T>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                dataItems = csv.GetRecords<T>().ToList();
            }

            return dataItems;
        }
    }

    public sealed class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Position).Ignore();
            Map(m => m.FirstName).Name("first_name");
        }
    }
}
