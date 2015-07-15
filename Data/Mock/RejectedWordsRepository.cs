using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.API.Interfaces.DB;
using Data.API.Interfaces.DO;

namespace Data.Mock
{
    class IRejectedWordsRepository : IRejectedWordsRepositoryBackingStore
    {
        public IEnumerable<String> GetAll(bool initialLoad)
        {
            throw new NotImplementedException();
        }

        public String Insert(string word)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<String> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
