using NetLogStash.Config;
using NetLogStash.Interface;
using NetLogStash.Scripts;
using NetLogStash.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Filters
{
    public class ConfigurationFilter : AbstractFilter
    {
        private IFilter _filter;

        public override Func<Event, bool> Predicate
        {
            get
            {
                return (_filter != null) ? _filter.Predicate : null;
            }
            set { }
        }

        public override Event Execute(Event value)
        {
            return _filter.Execute(value);
        }

        public override void Initialize(string name, Dictionary<string, ParaItem> para)
        {
           // _filter = args[0].ConstructFilter(args[1], args.SubArray(2));
        }

        public override void Dispose()
        {
            if (_filter != null) _filter.Dispose();
            base.Dispose();
        }
    }
}
