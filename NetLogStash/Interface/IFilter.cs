using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Interface
{
    public interface IFilter : IInitializable, IDisposable
    {
        Func<Event, bool> Predicate { get; set; }

        Event Execute(Event value);
    }
}
