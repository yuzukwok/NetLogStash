using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Interface
{
    public interface IInput : IInitializable, IDisposable
    {
        string Type { get; set; }
        string Alias { get; set; }

        IObservable<Event> Execute();
    }
}
