using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Infuz.API.SQL;
using Infuz.Utilities;
using Ninject;
using Data.API;

namespace Data.DB
{
    public class DBRepository
    {
        [Inject]
        public ISqlServiceProvider SqlProvider { get; set; }

        #region Constants
        public const String translationSelectList = "[ID], [Unknowns], [Rejected], [Results], [Source], [Created]";
        public const String translationInsertList = "[Unknowns], [Rejected], [Results], [Source]";

        public const String synonymSelectList = "[ID], [Source], [Result], [Created]";
        public const String synonymInsertList = "[Source], [Result]";

        public const String feedbackSelectList = "[ID], [Contact], [Name], [Body], [Created]";
        public const String feedbackInsertList = "[Contact], [Name], [Body]";

        public const String rejectedWordsSelectList = "[Word]";
        public const String rejectedWordsInsertList = "[Word]";
        #endregion

        public IList<TElement> UntilDovesCry<TElement>(CommandType commandType
            , string sql
            , bool initialLoad
            , Action<IDataReader, IList<TElement>> appendAction
            , params SqlParameter[] parameters)
        {
            var maxChances = initialLoad ? 10 : 3;

            for (var chance = 0; chance < maxChances; chance++)
            {
                try
                {
                    var result = new List<TElement>();
                    SqlProvider.ExecuteDataReader(commandType
                                                  , sql
                                                  , reader => appendAction(reader, result)
                                                  , parameters);
                    return result;
                }
                catch (Exception ex)
                {
                    if (ex.IsWorthRetry())
                        Thread.Sleep(10);
                    else
                        throw;
                }
            }

            return null;
        }

        public IList<TElement> UntilDovesCry<TElement>(CommandType commandType
            , string sql
            , FreshnessRequest freshness
            , Action<IDataReader, IList<TElement>> appendAction
            , params SqlParameter[] parameters)
        {
            var maxChances = freshness == FreshnessRequest.AsyncBackfill ? 3 : 10;

            for (var chance = 0; chance < maxChances; chance++)
            {
                try
                {
                    var result = new List<TElement>();
                    SqlProvider.ExecuteDataReader(commandType
                                                  , sql
                                                  , reader => appendAction(reader, result)
                                                  , parameters);
                    return result;
                }
                catch (Exception ex)
                {
                    if (ex.IsWorthRetry())
                        Thread.Sleep(10);
                    else
                        throw;
                }
            }

            return null;
        }

        public Tuple<IList<TElementFirst>, IList<TElementSecond>> UntilDovesCry<TElementFirst, TElementSecond>(
            CommandType commandType
            , string sql
            , bool initialLoad
            , Action<IDataReader, IList<TElementFirst>> appendFirst
            , Action<IDataReader, IList<TElementSecond>> appendSecond
            , params SqlParameter[] parameters)
        {
            var maxChances = initialLoad ? 10 : 3;
            IList<TElementFirst> listFirst = null;
            IList<TElementSecond> listSecond = null;

            for (var chance = 0; chance < maxChances; chance++)
            {
                try
                {
                    listFirst = new List<TElementFirst>();
                    listSecond = new List<TElementSecond>();
                    SqlProvider.ExecuteDataReaders(commandType
                                                  , sql
                                                  , (reader, index) =>
                                                  {
                                                      if (index == 0)
                                                          appendFirst(reader, listFirst);
                                                      else
                                                          appendSecond(reader, listSecond);
                                                  }
                                                  , parameters);
                    return Tuple.New(listFirst, listSecond);
                }
                catch (Exception ex)
                {
                    if (ex.IsWorthRetry())
                        Thread.Sleep(10);
                    else
                        throw;
                }
            }

            return Tuple.New(listFirst, listSecond);
        }

        public int UntilDovesCryScalar(CommandType commandType, string sql, params SqlParameter[] parameters)
        {
            return UntilDovesCryScalar<int>(commandType, sql, parameters);
        }

        public T UntilDovesCryScalar<T>(CommandType commandType, string sql, params SqlParameter[] parameters)
        {
            var maxChances = 3;

            for (var chance = 0; chance < maxChances; chance++)
            {
                try
                {
                    return SqlProvider.ExecuteScalar<T>(commandType
                                                      , sql
                                                      , default(T)
                                                      , parameters);
                }
                catch (Exception ex)
                {
                    if (ex.IsWorthRetry())
                        Thread.Sleep(10);
                    else
                        throw;
                }
            }

            throw new InvalidOperationException("Unable to call " + sql);
        }

        public Tuple<TFirst, TSecond> UntilDovesCryScalars<TFirst, TSecond>(CommandType commandType
            , string sql
            , bool initialLoad
            , params SqlParameter[] parameters)
        {
            var maxChances = initialLoad ? 10 : 3;
            Tuple<TFirst, TSecond> result = null;

            for (var chance = 0; chance < maxChances; chance++)
            {
                try
                {
                    SqlProvider.ExecuteDataReader(commandType
                                                  , sql
                                                  , reader => GetReaderResult(reader, out result)
                                                  , parameters);

                    return result;
                }
                catch (Exception ex)
                {
                    if (ex.IsWorthRetry())
                        Thread.Sleep(10);
                    else
                        throw;
                }
            }

            throw new InvalidOperationException("Unable to call " + sql);
        }

        protected void GetReaderResult<TFirst, TSecond>(IDataReader reader, out Tuple<TFirst, TSecond> result)
        {
            var first = reader.ColumnValue(0, default(TFirst));
            var second = reader.ColumnValue(1, default(TSecond));
            result = Tuple.New(first, second);
        }

        public int UntilDovesCryNonQuery(string sql, params SqlParameter[] parameters)
        {
            return UntilDovesCryNonQuery(CommandType.Text, sql, parameters);
        }

        public int UntilDovesCryNonQuery(CommandType commandType, string sql, params SqlParameter[] parameters)
        {
            var maxChances = 3;

            for (var chance = 0; chance < maxChances; chance++)
            {
                try
                {
                    return SqlProvider.ExecuteNonQuery(commandType
                                                       , sql
                                                       , parameters);
                }
                catch (Exception ex)
                {
                    if (ex.IsWorthRetry())
                        Thread.Sleep(10);
                    else
                        throw;
                }
            }

            throw new InvalidOperationException("Unable to call " + sql);
        }
    }
}
