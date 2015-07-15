using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.API.Interfaces.DO;

namespace Data.API.Interfaces.DB
{
    public interface ISynonymRepository
    {
        ISynonym Insert(String source, String result);
        IEnumerable<ISynonym> GetAll();
        IEnumerable<ISynonym> GetBySource(String search);
    }

    public interface ISynonymRepositoryBackingStore : ISynonymRepository
    {
        IEnumerable<ISynonym> GetAll(bool initialLoad);
    }
}
