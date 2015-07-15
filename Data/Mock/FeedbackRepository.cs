using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.API.Interfaces.DB;
using Data.API.Interfaces.DO;

namespace Data.Mock
{
    class FeedbackRepository : IFeedbackRepositoryBackingStore
    {
        public IEnumerable<IFeedback> GetAll(bool initialLoad)
        {
            throw new NotImplementedException();
        }

        public IFeedback Insert(string contact, string name, string body)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFeedback> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFeedback> GetByContact(String search)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFeedback> GetByName(String search)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFeedback> GetByAny(String search)
        {
            throw new NotImplementedException();
        }
    }
}
