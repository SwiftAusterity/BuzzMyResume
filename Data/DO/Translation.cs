using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.API.Interfaces.DO;

namespace Data.DO
{
    public class Translation : ITranslation
    {
        public long ID { get; set; }
        public String Source { get; set; }
        public Dictionary<String, String> Results { get; set;}
        public IEnumerable<String> Rejected { get; set;}
        public IEnumerable<String> Unknowns { get; set;}
        public DateTime Created { get; set;}
    }
}
