using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Interface
{
    public interface IOutput : IInitializable, IDisposable
    {
        void Execute(Event value);
    }
}
