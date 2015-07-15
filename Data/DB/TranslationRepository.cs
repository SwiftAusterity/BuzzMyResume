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
    public class TranslationRepository : DBRepository, ITranslationRepositoryBackingStore
    {
        [Inject]
        public ITranslationRepositoryBackingStore BackingStore { get; set; }

        //Store
        public ITranslation Insert(IEnumerable<String> unknowns, IEnumerable<String> rejected, Dictionary<String, String> results, String source)
        {
            var id = UntilDovesCryScalar<long>(CommandType.Text
                        , String.Format("INSERT INTO dbo.Translation({0}) VALUES(@Unknowns, @Rejected, @Results, @Source) "
                                      + "SELECT SCOPE_IDENTITY()", translationInsertList)
                        , Utility.Parameter("@Unknowns", unknowns.ToStringAndJoin("|"), true, 2000)
                        , Utility.Parameter("@Rejected", rejected.ToStringAndJoin("|"), true, 2000)
                        , Utility.Parameter("@Results", EncodeTranslationResults(results), true, 2000)
                        , Utility.Parameter("@Source", source, true, 2000));

            var thing = new Data.DO.Translation
            {
                ID = id,
                Unknowns = unknowns,
                Rejected = rejected,
                Results = results,
                Source = source,
                Created = DateTime.UtcNow
            };

            return thing;
        }

        //Get ALL THE THINGS
        public IEnumerable<ITranslation> GetAll()
        {
            return GetAll(true);
        }

        public ITranslation GetByID(long id)
        {
            var translations = GetTranslations();

            return translations.SingleOrDefault(translation => translation.ID.Equals(id));
        }

        public IEnumerable<ITranslation> GetBySource(String search)
        {
            var translations = GetTranslations();

            return translations.Where(translation => translation.Source.Equals(search, StringComparison.InvariantCultureIgnoreCase));
        }

        private IEnumerable<ITranslation> GetTranslations()
        {
            var translations = BackingStore.GetAll();

            if (translations == null || translations.Count() == 0)
                return Enumerable.Empty<ITranslation>();

            return translations;
        }

        public IEnumerable<ITranslation> GetAll(bool initialLoad)
        {
            return UntilDovesCry<ITranslation>(CommandType.Text
                , String.Format("SELECT {0} FROM dbo.Translation", translationSelectList)
                , initialLoad
                , AppendFromRow);
        }

        private static void AppendFromRow(IDataReader reader, IList<ITranslation> list)
        {
            int columnIndex = 0;
            var newData = GetFromReader(ref columnIndex, reader);
            list.Add(newData);
        }

        private static ITranslation GetFromReader(ref int columnIndex, IDataReader reader)
        {
            var id = reader.ColumnValue(columnIndex++, default(long));
            var unknowns = reader.ColumnValue(columnIndex++, String.Empty).Split('|');
            var rejected = reader.ColumnValue(columnIndex++, String.Empty).Split('|');
            var results = DecodeTranslationResults(reader.ColumnValue(columnIndex++, String.Empty));
            var source = reader.ColumnValue(columnIndex++, String.Empty);
            var created = reader.ColumnValue(columnIndex++, DateTime.MaxValue);

            var thing = new Data.DO.Translation
            {
                ID = id,
                Unknowns = unknowns,
                Rejected = rejected,
                Results = results,
                Source = source,
                Created = created
            };

            return thing;
        }

        private static Dictionary<String, String> DecodeTranslationResults(String resultsBlob)
        {
            var returnResult = new Dictionary<String, String>();
            
            foreach(String pair in resultsBlob.Split(','))
            {
                var splitPair = pair.Split('|');

                returnResult.Add(splitPair[0], splitPair[1]);
            }

            return returnResult;
        }

        private static String EncodeTranslationResults(Dictionary<String, String> results)
        {
            var returnResult = new StringBuilder();

            foreach (KeyValuePair<String, String> pair in results)
                returnResult.AppendFormat("{0}|{1},", pair.Key, pair.Value);

            if (returnResult.Length > 0)
                returnResult.Length--;

            return returnResult.ToString();
        }
    }
}
