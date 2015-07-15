using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.API.Interfaces.DO;

namespace Data.DO
{
    public class Synonym : ISynonym
    {
        public long ID { get; set; }
        public String Source { get; set; }
        public String Result { get; set;}
        public DateTime Created { get; set;}

        public bool IsPhrase()
        {
            var split = Source.Split(' ');

            return split.Count() > 1;
        }
    }
}
