using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.API.Interfaces.DO
{
    public interface IFeedback
    {
        long ID { get; set; }
        String Body { get; set; }
        String Name { get; set; }
        String Contact { get; set; }
        DateTime Created { get; set; }
    }
}
