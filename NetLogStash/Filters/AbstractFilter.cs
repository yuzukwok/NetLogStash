using NetLogStash.Config;
using NetLogStash.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Filters
{
    public abstract class AbstractFilter : IFilter
    {
        public virtual Func<Event, bool> Predicate { get; set; }

        public abstract Event Execute(Event value);
        public abstract void Initialize(string name, Dictionary<string, ParaItem> para);

        public virtual void Dispose() { }
    }
}
