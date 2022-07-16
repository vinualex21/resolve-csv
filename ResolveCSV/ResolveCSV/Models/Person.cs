using ResolveCSV.CustomParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResolveCSV.Models
{
    public class Person : IPopulatedFromCsv
    {
        [ColumnMap(Header = "first_name")]
        public string FirstName { get; set; }
        [ColumnMap(Header = "last_name")]
        public string LastName { get; set; }
        [ColumnMap(Header = "company_name")]
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postal { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
    }
}
