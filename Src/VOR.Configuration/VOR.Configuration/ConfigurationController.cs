using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Caching;
using System.Web;

namespace VOR.Configuration
{
    ///<summary>
    /// Controller of all the configuration used
    ///</summary>
    public class ConfigurationController : DAOHelper
    {
        #region Private Static Members

        private static ConfigurationController _controller = new ConfigurationController();
        private static string _cacheKey = null;

        #endregion

        #region Static Constructor

        /// <summary>
        /// Initializes the <see cref="ConfigurationController"/> class.
        /// </summary>
        static ConfigurationController()
        {
            _controller = new ConfigurationController();
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets one instance of the class.
        /// </summary>
        /// <value>The instance.</value>
        public static ConfigurationController Instance
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
        /// Initializes a new instance of the <see cref="ConfigurationController"/> class.
        /// </summary>
        private ConfigurationController()
        {
            _cache = HttpRuntime.Cache;

            // We load the cache duration from the database
            string duration = GetConfigurationValue("Configuration.CacheDuration", null);
            TimeSpan.TryParse(duration, out _cacheDuration);
           // _cacheDuration = TimeSpan.Parse(duration);
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

                string query = "P_CONF_GetConfigurationValueByKeyBySiteId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(CreateInputParameter("@Key", key));
                    cmd.Parameters.Add(CreateInputParameter("@SiteId", siteId ?? 0));
                    conn.Open();//

                    try
                    {
                        // on exécute
                        returnValue = cmd.ExecuteScalar() as string;

                    }
                    catch (Exception exc)
                    {
                        Log.Error("P_CONF_GetConfigurationValueByKeyBySiteId(" + key + ", " + siteId == null ? "0" : siteId.ToString() + ") : " + exc.Message);
                        throw exc.InnerException;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            
            if (string.IsNullOrEmpty(returnValue)
                && siteId.HasValue)
            {
                // We check if a non site specific value is avalaible
                returnValue = GetConfigurationValue(key, null);
            }

            if (!string.IsNullOrEmpty(returnValue)
                && !_cacheDuration.Equals(TimeSpan.Zero))
            {
                _cache.Insert(GenerateCacheUniqueKey(key, siteId), returnValue, null, DateTime.Now.Add(_cacheDuration), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }

            return returnValue;
        }

        public ConfigurationObject Create(string key, int? idSite, string value)
        {
            ConfigurationObject retour = null;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                // TODO ecrire la requete si besoin / actuellement pas utilisé
                using (SqlCommand cmd = new SqlCommand("P_CONF_NewConfiguration", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(CreateInputParameter("@Key", key));
                    cmd.Parameters.Add(CreateInputParameter("@IDSite", idSite));
                    cmd.Parameters.Add(CreateInputParameter("@Value", value));
                    // on ouvre la connection
                    conn.Open();

                    try
                    {
                        // on exécute
                        cmd.ExecuteNonQuery();

                        retour = new ConfigurationObject(key, idSite ?? 0, value);

                        _cache.Insert(GenerateCacheUniqueKey(key, idSite), value, null, DateTime.Now.Add(_cacheDuration), TimeSpan.Zero, CacheItemPriority.Normal, null);

                    }
                    catch (Exception exc)
                    {
                        Log.Error("Echec P_CONF_NewConfiguration() : " + exc.Message);
                        throw exc.InnerException;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return retour;
        }


        public bool Update(string key, int? idSite, string value)
        {
            bool retour = false;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("P_CONF_SetConfiguration", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(CreateInputParameter("@Key", key));
                    cmd.Parameters.Add(CreateInputParameter("@IDSite", idSite ?? (object)DBNull.Value));
                    cmd.Parameters.Add(CreateInputParameter("@Value", value));

                    conn.Open();

                    try
                    {
                        retour = cmd.ExecuteNonQuery() > 0 ? true : false;
                        _cache.Remove(GenerateCacheUniqueKey(key, idSite));
                        _cache.Insert(GenerateCacheUniqueKey(key, idSite), value, null, DateTime.Now.Add(_cacheDuration), TimeSpan.Zero, CacheItemPriority.Normal, null);
                    }
                    catch (Exception exc)
                    {
                        Log.Error("Echec P_CONF_SetConfiguration() : " + exc.Message);
                        throw exc.InnerException;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return retour;
        }

        public List<ConfigurationObject> GetListConfigurationBySiteId(int? idSite)
        {
            List<ConfigurationObject> retour = new List<ConfigurationObject>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                // on créé la commande
                using (SqlCommand cmd = new SqlCommand("P_CONF_GetListConfigurationBySiteId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(CreateInputParameter("@IDSite", idSite));
                    // on ouvre la connection
                    conn.Open();

                    try
                    {
                        // on exécute
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // on lit la ligne
                            while (reader.Read())
                            {

                                retour.Add(
                                    new ConfigurationObject(reader["Key"] as string, (reader["IDSite"] as int?) == 0 ? null : (reader["IDSite"] as int?), reader["Value"] as string)
                                        );
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        Log.Error("P_CONF_GetListConfigurationBySiteId(" + idSite + ") : " + exc.Message);
                        throw exc.InnerException;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return retour;
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
            _cacheKey = string.Format("[{0}][{1}]", key, siteId.ToString());
            return _cacheKey;
        }

        #endregion
    }
}
