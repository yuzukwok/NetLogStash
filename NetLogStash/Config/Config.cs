using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Config
{
   public class ConfigMeta
    {
        public Dictionary<string, IList<Plugin>> Plugins { get; set; }
      
    }

    public class Plugin
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public Dictionary<string, ParaItem> Params { get; set; }
    }

    public class ParaItem
    {
        public string Name { get; set; }
        public bool IsArray { get; set; }

        public bool IsHash { get; set; }

        public IList<string> Values { get; set; }
        public Hashtable Hash { get; set;}
        
    }
}
