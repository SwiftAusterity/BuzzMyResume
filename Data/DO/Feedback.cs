using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.API.Interfaces.DO;

namespace Data.DO
{
    public class Feedback : IFeedback
    {
        public long ID { get; set; }
        public String Body { get; set; }
        public String Name { get; set;}
        public String Contact { get; set;}
        public DateTime Created { get; set;}
    }
}
