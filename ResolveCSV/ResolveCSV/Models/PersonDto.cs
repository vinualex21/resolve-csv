﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ResolveCSV.Models
{
    public class PersonDto
    {
        public int Count { get; set; }
        public List<string> PersonDetails { get; set; }
    }
}
