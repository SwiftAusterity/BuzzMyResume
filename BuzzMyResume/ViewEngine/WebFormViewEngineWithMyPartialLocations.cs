using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Ninject;
using Data.API;

namespace BuzzMyResume.ViewEngine
{
    /// <summary>
    /// Updates the standard WebFormViewEngine with additional locations to search for
    /// views & partial views (cause that Shared Folder would get pretty messy otherwise)
    /// </summary>
    /// <remarks>
    /// If you add a new folder for views, simply add the path to the myFormats list below.
    /// </remarks>
    public class WebFormViewEngineWithMyPartialLocations : WebFormViewEngine
    {
        public WebFormViewEngineWithMyPartialLocations(IKernel Kernel) 
        {
            // Combine my custom locations with the standard ones
            var myFormats = new[]
                                {
                                    "~/Views/Shared/Content/{0}.aspx",
                                    "~/Views/Shared/Content/{0}.ascx",
                                    "~/Views/Admin/{0}.aspx"
                                }
                                .Union(PartialViewLocationFormats)
                                .ToArray();

            PartialViewLocationFormats = myFormats;
        }
    }
}
