using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.API.Interfaces.DO;

namespace Data.API.Interfaces.DB
{
    public interface IRejectedWordsRepository
    {

        String Insert(String word);
        IEnumerable<String> GetAll();
    }

    public interface IRejectedWordsRepositoryBackingStore : IRejectedWordsRepository
    {
        IEnumerable<String> GetAll(bool initialLoad);
    }
}
