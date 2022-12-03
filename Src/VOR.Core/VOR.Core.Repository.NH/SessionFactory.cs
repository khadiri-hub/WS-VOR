using VOR.Core.Repository.NH.SessionStorage;
using NHibernate;
using NHibernate.Cfg;
using System;

namespace VOR.Core.Repository.NH
{
    public class SessionFactory
    {
        public static ISessionFactory _SessionFactory;

        private static void Init()
        {
            Configuration config = new Configuration();
            config.DataBaseIntegration(db =>
               {
                   db.Driver<NHibernate.Driver.SqlClientDriver>();
                   db.ConnectionStringName = "SAOASConnectionString";
                   db.Dialect<VOR.Core.Repository.NH.GestourDialect>();
                   db.LogFormattedSql = true;
                   db.LogSqlInConsole = true;
                   db.BatchSize = 500;
               })
               .AddAssembly("VOR.Core.Repository.NH")
               .SessionFactory()
               .GenerateStatistics();

            log4net.Config.XmlConfigurator.Configure();

            _SessionFactory = config.BuildSessionFactory();
        }

        private static ISessionFactory GetSessionFactory()
        {
            if (_SessionFactory == null)
                Init();

            return _SessionFactory;
        }

        private static ISession GetNewSession()
        {
            return GetSessionFactory().OpenSession();
        }

        public static ISession GetCurrentSession()
        {
            ISessionStorageContainer _sessionStorageContainer = SessionStorageFactory.GetStorageContainer();

            ISession currentSession = _sessionStorageContainer.GetCurrentSession();

            if (currentSession == null)
            {
                currentSession = GetNewSession();
                _sessionStorageContainer.Store(currentSession);
            }

            return currentSession;
        }

        public static void InitSession()
        {
            if (_SessionFactory == null)
            {
                Init();
            }
        }

        public static Boolean GetBooleanByNamedQuery(string queryName, object[] values, string[] parametersName)
        {
            Boolean results = false;

            ISession s = GetCurrentSession();
            try
            {
                IQuery query = s.GetNamedQuery(queryName);
                if ((null != values) && (null != parametersName))
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        query.SetParameter(parametersName[i], values[i]);
                    }
                }
                results = Convert.ToBoolean(query.UniqueResult());
            }
            catch { }

            return results;
        }


        public static Boolean GetBooleanByNamedQuery(string queryName, object value, string parameterName)
        {
            object[] values = new object[] { value };
            string[] parametersName = new string[] { parameterName };
            return GetBooleanByNamedQuery(queryName, values, parametersName);
        }
    }
}