using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.API.Interfaces.DO
{
    public interface ITranslation
    {
        long ID { get; set; }
        String Source { get; set; }
        Dictionary<String, String> Results { get; set; }
        IEnumerable<String> Rejected { get; set; }
        IEnumerable<String> Unknowns { get; set; }
        DateTime Created { get; set; }
    }
}
