using NetLogStash.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Interface
{
    public interface IInitializable
    {
      void Initialize(string typename, Dictionary<string, ParaItem> paras);
    }
}
