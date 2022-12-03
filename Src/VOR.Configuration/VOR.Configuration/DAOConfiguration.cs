using System;
using System.Collections.Generic;
using System.Text;

namespace VOR.Configuration
{
    public class ConfigurationObject
    {
        private string _key;
        private int? _idSite;
        private string _value;

        public string Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        public int? IDSite
        {
            get
            {
                return this._idSite;
            }
            set
            {
                this._idSite = value;
            }
        }

        public string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public ConfigurationObject(string key, int? idSite, string value)
        {
            this._key = key;
            this._idSite = idSite;
            this._value = value;
        }
    }
}
