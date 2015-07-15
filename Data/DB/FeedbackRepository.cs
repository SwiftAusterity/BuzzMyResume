using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Data.API.Interfaces.DO;
using Data.API.Interfaces.DB;

using Infuz.Utilities;

using Ninject;

namespace Data.DB
{
    public class FeedbackRepository : DBRepository, IFeedbackRepositoryBackingStore
    {
        [Inject]
        public IFeedbackRepositoryBackingStore BackingStore { get; set; }

        //Store
        public IFeedback Insert(String contact, String name, String body)
        {
            var id = UntilDovesCryScalar<long>(CommandType.Text
                        , String.Format("INSERT INTO dbo.Feedback({0}) VALUES(@Contact, @Name, @Body) "
                                      + "SELECT SCOPE_IDENTITY()", feedbackInsertList)
                        , Utility.Parameter("@Contact", contact, true, 255)
                        , Utility.Parameter("@Name", name, true, 100)
                        , Utility.Parameter("@Body", body, true, 2000));

            var thing = new Data.DO.Feedback
            {
                ID = id,
                Contact = contact,
                Name = name,
                Body = body,
                Created = DateTime.UtcNow
            };

            return thing;
        }

        //Get ALL THE THINGS
        public IEnumerable<IFeedback> GetAll()
        {
            return GetAll(true);
        }

        public IEnumerable<IFeedback> GetByContact(String search)
        {
            var feedbacks = GetFeedbacks();

            return feedbacks.Where(feedback => feedback.Contact.Equals(search, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<IFeedback> GetByName(String search)
        {
            var feedbacks = GetFeedbacks();

            return feedbacks.Where(feedback => feedback.Name.Equals(search, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<IFeedback> GetByAny(String search)
        {
            var feedbacks = GetFeedbacks();

            return feedbacks.Where(feedback => feedback.Contact.Equals(search, StringComparison.InvariantCultureIgnoreCase)
                                            || feedback.Name.Equals(search, StringComparison.InvariantCultureIgnoreCase));
        }

        private IEnumerable<IFeedback> GetFeedbacks()
        {
            var feedbacks = BackingStore.GetAll();

            if (feedbacks == null || feedbacks.Count() == 0)
                return Enumerable.Empty<IFeedback>();

            return feedbacks;
        }

        public IEnumerable<IFeedback> GetAll(bool initialLoad)
        {
            return UntilDovesCry<IFeedback>(CommandType.Text
                , String.Format("SELECT {0} FROM dbo.Feedback", feedbackSelectList)
                , initialLoad
                , AppendFromRow);
        }

        private static void AppendFromRow(IDataReader reader, IList<IFeedback> list)
        {
            int columnIndex = 0;
            var newData = GetFromReader(ref columnIndex, reader);
            list.Add(newData);
        }

        private static IFeedback GetFromReader(ref int columnIndex, IDataReader reader)
        {
            var id = reader.ColumnValue(columnIndex++, default(long));
            var contact = reader.ColumnValue(columnIndex++, String.Empty);
            var name = reader.ColumnValue(columnIndex++, String.Empty);
            var body = reader.ColumnValue(columnIndex++, String.Empty);
            var created = reader.ColumnValue(columnIndex++, DateTime.MaxValue);

            var thing = new Data.DO.Feedback
            {
                ID = id,
                Contact = contact,
                Name = name,
                Body = body,
                Created = created
            };

            return thing;
        }
    }
}
