using BinaryRage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash
{
    public static class KeyStore
    {
        private static bool _isInitialized;
        private static string _location;

        public static void Initialize(string location)
        {
            _location = location;
            _isInitialized = !string.IsNullOrWhiteSpace(location);
        }

        public static void Insert<T>(string key, T id)
        {
            if (_isInitialized) DB.Insert<T>(key, id, _location);
        }

        public static T Get<T>(string key)
        {
            return (_isInitialized) ? DB.Get<T>(key, _location) : default(T);
        }

        public static bool Exists(string key)
        {
            return (_isInitialized) ? DB.Exists(key, _location) : false;
        }
    }
}
