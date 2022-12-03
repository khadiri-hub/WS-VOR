using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace VOR.Core
{
    public class CachableObject<TypeOfPrimaryKey> : BaseObject<TypeOfPrimaryKey>
    {
        public virtual string CacheKey { get; set; }

        // Default Value
        public virtual int CacheDuration { get; protected set; }
        public virtual DateTime CacheExpiration { get; protected set; }
        public virtual TimeSpan CacheSlidingExpiration { get; protected set; }

        protected CachableObject()
        {
            CacheDuration = 60;
            CacheExpiration = Cache.NoAbsoluteExpiration;
            CacheSlidingExpiration = TimeSpan.Zero;
        }

        protected CachableObject(int cacheDuration, TimeSpan slidingExpiration)
        {
            CacheDuration = cacheDuration;
            CacheExpiration = cacheDuration > 0 ? DateTime.Now.AddMinutes(cacheDuration) : Cache.NoAbsoluteExpiration;
            CacheSlidingExpiration = slidingExpiration;
        }
    }
}