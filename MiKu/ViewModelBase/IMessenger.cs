using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiKu
{
    public interface IMessenger
    {
        void Register<T>(Action<T> action);
        void Register<T>(Action<T> action, object token);
        void Send<T>(T message);
        void Send<T>(T message, object token);
        void Unregister<T>(Action<T> action);
        void Unregister<T>(Action<T> action, object token);
    }
}
