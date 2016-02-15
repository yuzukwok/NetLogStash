using NetLogStash.Config;
using NetLogStash.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Input
{
    public abstract class AbstractInput : IInput
    {
        public virtual string Type { get; set; }
        public virtual string Alias { get; set; }

        public abstract IObservable<Event> Execute();
        public abstract void Initialize(string typename,Dictionary<string,ParaItem> paras);

        public virtual void Dispose() { }
    }
}
