using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.API.Interfaces.DO;

namespace Data.API.Interfaces.DB
{
    public interface ITranslationRepository
    {

        ITranslation Insert(IEnumerable<String> unknowns, IEnumerable<String> rejected, Dictionary<String, String> results, String source);
        IEnumerable<ITranslation> GetAll();
        IEnumerable<ITranslation> GetBySource(String search);
        ITranslation GetByID(long id);
    }

    public interface ITranslationRepositoryBackingStore : ITranslationRepository
    {
        IEnumerable<ITranslation> GetAll(bool initialLoad);
    }
}
