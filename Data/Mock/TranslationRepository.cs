using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.API.Interfaces.DB;
using Data.API.Interfaces.DO;

namespace Data.Mock
{
    class TranslationRepository : ITranslationRepositoryBackingStore
    {
        public IEnumerable<ITranslation> GetAll(bool initialLoad)
        {
            throw new NotImplementedException();
        }

        public Data.API.Interfaces.DO.ITranslation Insert(IEnumerable<string> unknowns, IEnumerable<string> rejected, Dictionary<string, string> results, string source)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITranslation> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITranslation> GetBySource(String search)
        {
            throw new NotImplementedException();
        }

        public ITranslation GetByID(long id)
        {
            throw new NotImplementedException();
        }
    }
}
