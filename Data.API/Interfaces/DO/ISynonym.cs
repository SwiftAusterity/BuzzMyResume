using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.API.Interfaces.DO
{
    public interface ISynonym
    {
        long ID { get; set; }
        String Source { get; set; }
        String Result { get; set; }
        DateTime Created { get; set; }

        bool IsPhrase();
    }
}
