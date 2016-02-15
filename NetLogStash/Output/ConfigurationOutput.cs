using NetLogStash.Config;
using NetLogStash.Interface;
using NetLogStash.Scripts;
using NetLogStash.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Output
{
    public class ConfigurationOutput : AbstractOutput
    {
        private IOutput _output;

        public override void Execute(Event value)
        {
            _output.Execute(value);
        }

        public override void Initialize(string name, Dictionary<string, ParaItem> para)
        {
           // _output = args[0].ConstructOutput(args.SubArray(1));
        }

        public override void Dispose()
        {
            if (_output != null) _output.Dispose();
            base.Dispose();
        }
    }
}
