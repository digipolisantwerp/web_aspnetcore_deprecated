using System;
using System.Collections.Generic;
using Digipolis.Toolbox.Errors.Exceptions;

namespace Digipolis.Toolbox.Web.Exceptions
{
    public class HttpStatusCodeMappings
    {
        private Dictionary<Type, int> _mappings;

        public HttpStatusCodeMappings()
        {
            _mappings = new Dictionary<Type, int>();

            InitializeWithDefaults();
        }

        private void InitializeWithDefaults()
        {
            _mappings.Add(typeof(NotFoundException), 404);
            _mappings.Add(typeof(ValidationException), 400);
            _mappings.Add(typeof(UnauthorizedException), 403);
        }

        public void Add(Type key, int value)
        {
            if (_mappings.ContainsKey(key))
                _mappings.Remove(key);

            _mappings.Add(key, value);
        }

        public void Add<T>(int value)
        {
            var type = typeof(T);
            Add(type, value);
        }

        public void AddRange(IEnumerable<KeyValuePair<Type, int>> mappings)
        {
            foreach (var item in mappings)
            {
                Add(item.Key, item.Value);
            }
        }

        public bool ContainsKey(Type key)
        {
            return _mappings.ContainsKey(key);
        }

        public int GetStatusCode(Type key)
        {
            return _mappings[key];
        }
    }
}