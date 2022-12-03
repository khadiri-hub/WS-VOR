using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Caching;
using System.Web;
using System.Collections.Generic;
using VOR.Configuration;

namespace VOR.Configuration
{
    ///<summary>
    /// Controlleur recuperant juste les valeurs utiles au chargement des applis FFT sans se base sur la table T_CONFIGURATION
    ///</summary>
    public class BasicConfigurationController : DAOHelper
    {
        public InitialValues GetInitialValues()
        {
            var ret = new InitialValues();

            string query = @"SELECT E.EV_ID AS EventNum
                             ,E.EV_TYPE AS EventType
                             ,E.EV_LIB AS EventName
                                FROM T_EVEN AS E
                                INNER JOIN T_PARAMETRES AS P
	                                ON P.Libelle ='NUM EVENEMENT EN COURS'
                                        AND P.Valeur = E.EV_ID";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.Text;
                conn.Open();

                try
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.FieldCount < 3)
                        {
                            throw new Exception("Missing values in DB");
                        }

                        ret.EventNum = reader.GetInt32(0);
                        ret.EventType = reader.GetString(1);
                        ret.EventName = reader.GetString(2);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("GetInitialValues :{0}", ex.Message));
                    throw ex.InnerException != null ? ex.InnerException : ex;
                }
            }
            return ret;
        }

         #region Private Static Members

        private static BasicConfigurationController _controller = new BasicConfigurationController();
        private static string _cacheKey = null;

        #endregion

        #region Static Constructor

        /// <summary>
        /// Initializes the <see cref="ConfigurationController"/> class.
        /// </summary>
        static BasicConfigurationController()
        {
            _controller = new BasicConfigurationController();
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets one instance of the class.
        /// </summary>
        /// <value>The instance.</value>
        public static BasicConfigurationController Instance
        {
            get { return _controller; }
        }

        #endregion

        #region Private Members

        private const string _cacheName = "Configuration";

        private TimeSpan _cacheDuration = TimeSpan.Zero;

        private Cache _cache = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicConfigurationController"/> class.
        /// </summary>
        private BasicConfigurationController()
        {
            _cache = HttpRuntime.Cache;

            string duration = "60";
            TimeSpan.TryParse(duration, out _cacheDuration);

        }

        #endregion

        #region Public Methods

        #region GetConfigurationValue


        /// <summary>
        /// Gets the configuration value for the specified key and site.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="siteId">The site id.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public string GetConfigurationValue(string key, int? siteId)
        {
            string returnValue = null;

            // We check if the value is in the cache, so we can avoid an access to the database
            if ((returnValue = _cache[GenerateCacheUniqueKey(key, siteId)] as string) != null)
            {
                return returnValue;
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                string query = "P_CONF_GetOldConfigurationValueByKey";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(CreateInputParameter("@Key", key));
                    conn.Open();//

                    try
                    {
                        // on exécute
                        returnValue = cmd.ExecuteScalar() as string;

                    }
                    catch (Exception exc)
                    {
                        Log.Error("P_CONF_GetOldConfigurationValueByKey(" + key + ", " + siteId == null ? "0" : siteId.ToString() + ") : " + exc.Message);
                        throw exc.InnerException;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            if (!string.IsNullOrEmpty(returnValue)
                && !_cacheDuration.Equals(TimeSpan.Zero))
            {
                _cache.Insert(GenerateCacheUniqueKey(key, siteId), returnValue, null, DateTime.Now.Add(_cacheDuration), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the configuration keys corresponding to value
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="siteId">The site id.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        public bool TryGetConfigurationKeysByValue(string value, out Dictionary<string, string> results)
        {
            bool returnValue = false;
            results = new Dictionary<string, string>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                string query = "P_CONF_TryGetOldConfigurationKeysByValue";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(CreateInputParameter("@Value", value));
                    conn.Open();

                    try
                    {
                        using(SqlDataReader sr = cmd.ExecuteReader())
                        {
                            while (sr.Read())
                            {
                                results.Add(sr["Libelle"] as string, sr["Valeur"] as string);
                            }
                        
                        }
                        if (results.Count > 0)
                            returnValue = true;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return returnValue;
        }

        public ConfigurationObject Create(string key, int? idSite, string value)
        {
            throw new Exception("Not implemented");

            //ConfigurationObject retour = null;
            //using (SqlConnection conn = new SqlConnection(ConnectionString))
            //{
            //    // TODO ecrire la requete si besoin / actuellement pas utilisé
            //    using (SqlCommand cmd = new SqlCommand("P_CONF_OldStorage_Insert", conn))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add(CreateInputParameter("@Key", key));
            //        cmd.Parameters.Add(CreateInputParameter("@IDSite", idSite));
            //        cmd.Parameters.Add(CreateInputParameter("@Value", value));
            //        // on ouvre la connection
            //        conn.Open();

            //        try
            //        {
            //            // on exécute
            //            cmd.ExecuteNonQuery();

            //            retour = new ConfigurationObject(key, idSite ?? 0, value);

            //            _cache.Insert(GenerateCacheUniqueKey(key, idSite), value, null, DateTime.Now.Add(_cacheDuration), TimeSpan.Zero, CacheItemPriority.Normal, null);

            //        }
            //        catch (Exception exc)
            //        {
            //            Log.Error("Echec P_CONF_NewConfiguration() : " + exc.Message);
            //            throw exc.InnerException;
            //        }
            //        finally
            //        {
            //            conn.Close();
            //        }
            //    }
            //}
            //return retour;
        }


        public bool Update(string key, int? idSite, string value)
        {
            throw new Exception("Not implemented");
            //bool retour = false;
            //using (SqlConnection conn = new SqlConnection(ConnectionString))
            //{
            //    using (SqlCommand cmd = new SqlCommand("P_CONF_SetConfiguration", conn))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add(CreateInputParameter("@Key", key));
            //        cmd.Parameters.Add(CreateInputParameter("@IDSite", idSite ?? (object)DBNull.Value));
            //        cmd.Parameters.Add(CreateInputParameter("@Value", value));

            //        conn.Open();

            //        try
            //        {
            //            retour = cmd.ExecuteNonQuery() > 0 ? true : false;
            //            _cache.Remove(GenerateCacheUniqueKey(key, idSite));
            //            _cache.Insert(GenerateCacheUniqueKey(key, idSite), value, null, DateTime.Now.Add(_cacheDuration), TimeSpan.Zero, CacheItemPriority.Normal, null);
            //        }
            //        catch (Exception exc)
            //        {
            //            Log.Error("Echec P_CONF_SetConfiguration() : " + exc.Message);
            //            throw exc.InnerException;
            //        }
            //        finally
            //        {
            //            conn.Close();
            //        }
            //    }
            //}
            //return retour;
        }

       

        #endregion

        /// <summary>
        /// Flushes the cache.
        /// </summary>
        public void FlushCache()
        {
            if (_cacheKey != null)
            _cache.Remove(_cacheKey);
        }

        /// <summary>
        /// Generates the cache unique key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="siteId">The site id.</param>
        /// <returns></returns>
        public string GenerateCacheUniqueKey(string key, int? siteId)
        {
            _cacheKey = string.Format("TPARAMS[{0}][{1}]", key, siteId.ToString());
            return _cacheKey;
        }

        #endregion
    }
}