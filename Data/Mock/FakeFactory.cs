using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Infuz.Utilities;
using Ninject;
using Data.API;
using Data.DO;
using Data.Ninject;

namespace Data.Mock
{
    internal class FakeFactory
    {
        [Inject]
        public IKernel Kernel { get; set; }
    }
}
