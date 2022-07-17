using ResolveCSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResolveCSV.Services
{
    public interface IQueryService
    {
        public List<Person> SetPosition(List<Person> persons);
    }
}
