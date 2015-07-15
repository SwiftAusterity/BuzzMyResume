using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Ninject;

using Data.API.Interfaces.DO;
using Data.API.Interfaces.DB;

using Infuz.Utilities;

namespace Data.DB
{
    public class SynonymRepository : DBRepository, ISynonymRepositoryBackingStore
    {
        [Inject]
        public ISynonymRepositoryBackingStore BackingStore { get; set; }

        //Store
        public ISynonym Insert(String source, String result)
        {
            var id = UntilDovesCryScalar<long>(CommandType.Text
                        , String.Format("INSERT INTO dbo.Synonym({0}) VALUES(@Source, @Result) "
                                      + "SELECT SCOPE_IDENTITY()", feedbackInsertList)
                        , Utility.Parameter("@Source", source, true, 2000)
                        , Utility.Parameter("@Result", result, true, 2000));

            var thing = new Data.DO.Synonym
            {
                ID = id,
                Source = source,
                Result = result,
                Created = DateTime.UtcNow
            };

            return thing;
        }

        //Get ALL THE THINGS
        public IEnumerable<ISynonym> GetAll()
        {
            return GetAll(true);
        }

        public IEnumerable<ISynonym> GetBySource(String search)
        {
            var synonyms = GetSynonyms();

            return synonyms.Where(synonym => synonym.Source.Equals(search, StringComparison.InvariantCultureIgnoreCase));
        }

        private IEnumerable<ISynonym> GetSynonyms()
        {
            var synonyms = BackingStore.GetAll();

            if (synonyms == null || synonyms.Count() == 0)
                return Enumerable.Empty<ISynonym>();

            return synonyms;
        }

        public IEnumerable<ISynonym> GetAll(bool initialLoad)
        {
            return UntilDovesCry<ISynonym>(CommandType.Text
                , String.Format("SELECT {0} FROM dbo.Synonym", translationSelectList)
                , initialLoad
                , AppendFromRow);
        }

        private static void AppendFromRow(IDataReader reader, IList<ISynonym> list)
        {
            int columnIndex = 0;
            var newData = GetFromReader(ref columnIndex, reader);
            list.Add(newData);
        }

        private static ISynonym GetFromReader(ref int columnIndex, IDataReader reader)
        {
            var id = reader.ColumnValue(columnIndex++, default(long));
            var source = reader.ColumnValue(columnIndex++, String.Empty);
            var result = reader.ColumnValue(columnIndex++, String.Empty);
            var created = reader.ColumnValue(columnIndex++, DateTime.MaxValue);

            var thing = new Data.DO.Synonym
            {
                ID = id,
                Source = source,
                Result = result,
                Created = created
            };

            return thing;
        }

    }
}
