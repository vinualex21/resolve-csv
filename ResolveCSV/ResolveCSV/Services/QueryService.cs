using ResolveCSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResolveCSV.Services
{
    public class QueryService : IQueryService
    {
        public List<Person> SetPosition(List<Person> persons)
        {
            persons.Select((p,i)=> p.Position = i+1).ToList();
            return persons;
        }
    }
}
