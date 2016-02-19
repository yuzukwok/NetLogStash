using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Grok
{
    public class GrokPattern
    {
        public string Id { get; set; }
        public string Expression { get; set; }
        public IList<string> Fields { get; set; }

        public string Regex { get; set; }

        public string Resolved { get;set;}

        public void Parse(string str)
        {
        }
    }
}
