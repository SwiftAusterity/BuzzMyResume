using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.API.Interfaces.DO;

namespace Data.API.Interfaces.DB
{
    public interface IFeedbackRepository
    {
        IFeedback Insert(String contact, String name, String body);
        IEnumerable<IFeedback> GetAll();
        IEnumerable<IFeedback> GetByContact(String search);
        IEnumerable<IFeedback> GetByName(String search);
        IEnumerable<IFeedback> GetByAny(String search);
    }

    public interface IFeedbackRepositoryBackingStore : IFeedbackRepository
    {
        IEnumerable<IFeedback> GetAll(bool initialLoad);
    }
}
