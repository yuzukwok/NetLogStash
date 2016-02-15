using NetLogStash.Config;
using NetLogStash.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Output
{
    public abstract class AbstractOutput : IOutput
    {
        public abstract void Execute(Event value);
        public abstract void Initialize(string name, Dictionary<string, ParaItem> para);

        public virtual void Dispose() { }
    }
}
