using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace CML.Utilities
{
    public class CacheManager
    {
        private static CacheManager m_utilsInstance;

        public static CacheManager Instance
        {
            get
            {
                if ( m_utilsInstance == null )
                    m_utilsInstance = new CacheManager();

                return m_utilsInstance;
            }
        }
        public CacheManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public void Add( string key, object value )
        {

            HttpRuntime.Cache.Add( key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null );
        }

        public bool Contains( string key )
        {
            return HttpRuntime.Cache.Get( key ) != null;
        }

        public int Count()
        {
            return HttpRuntime.Cache.Count;
        }

        public void Insert( string key, object value )
        {
            HttpRuntime.Cache.Insert( key, value );
        }

        public T Get<T>( string key )
        {
            return ( T )HttpRuntime.Cache.Get( key );
        }

        public void Remove( string key )
        {
            HttpRuntime.Cache.Remove( key );
        }

        public object this[string key]
        {
            get
            {
                return HttpRuntime.Cache[key];
            }
            set
            {
                HttpRuntime.Cache[key] = value;
            }
        }
    }
}