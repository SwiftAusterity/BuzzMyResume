using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Data.API;
using Data.API.Interfaces.DO;

namespace BuzzMyResume.ViewModels
{
    public class TranslationResult
    {
        public ITranslation Translation { get; set; }
    }
}
