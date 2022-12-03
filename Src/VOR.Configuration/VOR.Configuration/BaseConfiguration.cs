//#define CreateCache_MISSING_VALUE

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace VOR.Configuration
{
    public abstract class BaseConfiguration
    {
        #region Private Members

        private Cache _cache;
        protected int? _siteId = null;
        private string _name = null;
        private string _baseKey = null;
        private BaseConfiguration _parent = null;
        private ConfigurationController _controller = null;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConfiguration"/> class.
        /// </summary>
        /// <param name="siteId">The site id.</param>
        public BaseConfiguration(int? siteId)
            : this(siteId, null)
        {
            this._cache = HttpRuntime.Cache;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConfiguration"/> class.
        /// </summary>
        /// <param name="siteId">The site id.</param>
        /// <param name="name">The name of the configuration node (the name of the class if not provided).</param>
        public BaseConfiguration(int? siteId, string name)
            : this(siteId, name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConfiguration"/> class.
        /// </summary>
        /// <param name="siteId">The site id.</param>
        /// <param name="name">The name of the configuration node (the name of the class if not provided).</param>
        /// <param name="configurationParent">The configuration parent.</param>
        public BaseConfiguration(int? siteId, string name, BaseConfiguration configurationParent)
        {
            _siteId = siteId ?? 0;

            if (string.IsNullOrEmpty(name))
            {
                _name = GetType().Name;
            }
            else
            {
                _name = name;
            }

            ConfigurationParent = configurationParent;

            _controller = ConfigurationController.Instance;
        }

        #endregion Constructors

        #region Protected Properties

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> with the specified key.
        /// </summary>
        /// <value></value>
        public string this[string key]
        {
            get
            {
                string value = _controller.GetConfigurationValue(GenerateUniqueKey(key), _siteId);
                if (value == null && this._parent != null)
                {
                    value = this._parent[key];
                    if (value != null)
#if CreateCache_MISSING_VALUE
                        _cache.Insert(_controller.GenerateCacheUniqueKey(key, _siteId), value, null, DateTime.UtcNow.AddHours(1), TimeSpan.FromHours(1), CacheItemPriority.Normal, null);
#else //Create in DB missing Value
                        _controller.Create(GenerateUniqueKey(key), _siteId, value);
#endif
                }
                return value;
            }
            set
            {
                if (_controller.GetConfigurationValue(GenerateUniqueKey(key), _siteId) != null && value != null)
                    _controller.Update(GenerateUniqueKey(key), _siteId, value);
                else
                    _controller.Create(GenerateUniqueKey(key), _siteId, value);
            }
        }

        /// <summary>
        /// Gets or sets the configuration parent.
        /// </summary>
        /// <value>The configuration parent.</value>
        public BaseConfiguration ConfigurationParent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                if (_parent == null)
                {
                    _baseKey = _name;
                }
                else
                {
                    var parts = _parent._baseKey.Split('.');
                    if (parts.Length > 1 && parts[parts.Length - 1].Contains("!"))
                    {
                        for (int i = 0; i < parts.Length - 1; ++i)
                        {
                            _baseKey += parts[i] + ".";
                        }
                        _baseKey += parts[parts.Length - 1].Split('!')[0] + "s" + "." + _name;
                    }
                    else
                        _baseKey = _parent._baseKey + "." + _name;
                }
            }
        }

        /// <summary>
        /// Gets the site id which will be used for getting the configuration value.
        /// </summary>
        /// <value>The site id.</value>
        public int? SiteId
        {
            get
            {
                return _siteId;
            }
        }

        #endregion Protected Properties

        #region Protected Methods

        /// <summary>
        /// Generates the unique key for the request to the database.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected virtual string GenerateUniqueKey(string key)
        {
            return _baseKey + "." + key;
        }

        /// <summary>
        /// Adds a child to the current configuration.
        /// </summary>
        /// <param name="configurationChild">The configuration child.</param>
        public void AddConfigurationChild(BaseConfiguration configurationChild)
        {
            if (configurationChild == null)
            {
                throw new NullReferenceException("configurationChild cannot be null");
            }

            if (configurationChild._siteId != this._siteId)
            {
                throw new ConfigurationErrorsException("configurationChild cannot have a siteId different from his parent");
            }

            configurationChild.ConfigurationParent = this;
        }

        /// <summary>
        /// Gets the string value for the string passed as argument.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string GetStringValue(string key, string defaultValue)
        {
            string value = this[key];

            return value ?? defaultValue;
        }

        /// <summary>
        /// Gets the bool value for the string passed as argument.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public bool GetBoolValue(string key, bool defaultValue)
        {
            string value = this[key];

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return bool.Parse(value);
        }

        /// <summary>
        /// Gets the int value for the string passed as argument.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public int GetIntValue(string key, int defaultValue)
        {
            string value = this[key];

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// Gets the long value for the string passed as argument.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public long GetLongValue(string key, long defaultValue)
        {
            string value = this[key];

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// Gets the time span value for the string passed as argument.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public TimeSpan GetTimeSpanValue(string key, TimeSpan defaultValue)
        {
            string value = this[key];

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return TimeSpan.Parse(value);
        }

        /// <summary>
        /// Gets the date time value for the string passed as argument.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public DateTime GetDateTimeValue(string key, DateTime defaultValue)
        {
            string value = this[key];

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return DateTime.Parse(value);
        }

        /// <summary>
        /// Get the value casted to the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Config key</param>
        /// <param name="defaultValue">default value returned if the key is not found</param>
        /// <returns>The value of the config key or the specified defaultvalue</returns>
        public T GetValue<T>(string key, T defaultValue)
        {
            string value = this[key];

            if (string.IsNullOrEmpty(value))
                return defaultValue;
            else return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Get the value casted to the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Config key</param>
        /// <returns>The value of the config key or the default value of the type</returns>
        public T GetValue<T>(string key)
        {
            string value = this[key];

            if (string.IsNullOrEmpty(value))
                return default(T);
            else return (T)Convert.ChangeType(value, typeof(T));
        }

        #endregion Protected Methods
    }
}