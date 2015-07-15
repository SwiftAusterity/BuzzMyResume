using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Ninject;

using Data.API.Interfaces.DO;
using Data.API.Interfaces.DB;

using Infuz.Utilities;

namespace Data.DB
{
    public class RejectedWordsRepository : DBRepository, IRejectedWordsRepositoryBackingStore
    {
        [Inject]
        public IRejectedWordsRepositoryBackingStore BackingStore { get; set; }

        //Store
        public String Insert(String word)
        {
            var id = UntilDovesCryScalar<long>(CommandType.Text
                        , String.Format("INSERT INTO dbo.RejectedWord({0}) VALUES(@Word) "
                                      + "SELECT SCOPE_IDENTITY()", rejectedWordsInsertList)
                        , Utility.Parameter("@Word", word, true, 2000));

            return word;
        }

        //Get ALL THE THINGS
        public IEnumerable<String> GetAll()
        {
            return GetAll(true);
        }

        public IEnumerable<String> GetAll(bool initialLoad)
        {
            return UntilDovesCry<String>(CommandType.Text
                , String.Format("SELECT {0} FROM dbo.RejectedWord", rejectedWordsSelectList)
                , initialLoad
                , AppendFromRow);
        }

        private static void AppendFromRow(IDataReader reader, IList<String> list)
        {
            int columnIndex = 0;
            var newData = GetFromReader(ref columnIndex, reader);
            list.Add(newData);
        }

        private static String GetFromReader(ref int columnIndex, IDataReader reader)
        {
            var word = reader.ColumnValue(columnIndex++, String.Empty);

            return word;
        }
    }
}
