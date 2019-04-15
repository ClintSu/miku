using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiKu
{
    public class Messenger : IMessenger
    {
        private struct TypeTokenKey
        {
            public Type Type { get; private set; }
            public object Token { get; private set; }

            public static TypeTokenKey GetKey<T>(object token)
            {
                return new TypeTokenKey { Type = typeof(T), Token = token };
            }
        }

        private IDictionary<TypeTokenKey, List<object>> keyActionMap = new Dictionary<TypeTokenKey, List<object>>();

        public void Register<T>(Action<T> action)
        {
            this.Register<T>(action, null);
        }

        public void Register<T>(Action<T> action, object token)
        {
            var key = TypeTokenKey.GetKey<T>(token);

            if (keyActionMap.ContainsKey(key))
            {
                if (!keyActionMap[key].Any(e => e.Equals(action)))
                    keyActionMap[key].Add(action);
            }
            else
            {
                var list = new List<object>();
                list.Add(action);
                keyActionMap.Add(key, list);
            }

        }

        public void Unregister<T>(Action<T> action)
        {
            this.Unregister<T>(action, null);
        }
        public void Unregister<T>(Action<T> action, object token)
        {
            var key = TypeTokenKey.GetKey<T>(token);

            if (keyActionMap.ContainsKey(key))
                keyActionMap[key].Remove(action);
        }

        public void Send<T>(T message)
        {
            this.Send<T>(message, null);
        }
        public void Send<T>(T message, object token)
        {
            var key = TypeTokenKey.GetKey<T>(token);

            if (keyActionMap.ContainsKey(key))
                foreach (var action in keyActionMap[key])
                    ((Action<T>)action).Invoke(message);
        }
    }
}
