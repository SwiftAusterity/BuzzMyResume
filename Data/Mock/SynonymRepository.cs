using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.API.Interfaces.DB;
using Data.API.Interfaces.DO;

namespace Data.Mock
{
    class SynonymRepository : ISynonymRepositoryBackingStore
    {
        public IEnumerable<ISynonym> GetAll(bool initialLoad)
        {
            throw new NotImplementedException();
        }

        public ISynonym Insert(string source, string result)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISynonym> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISynonym> GetBySource(String search)
        {
            throw new NotImplementedException();
        }
    }
}
