using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Data.API;
using Data.API.Interfaces.DO;

namespace BuzzMyResume.ViewModels
{
    public class CurationList
    {
        public IEnumerable<ISynonym> Synonyms { get; set; }
        public IEnumerable<ITranslation> ModerationList { get; set; }
        public IEnumerable<String> RejectedWords { get; set; }
    }
}
