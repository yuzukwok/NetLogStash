using NetLogStash.Config;
using NetLogStash.Interface;
using NetLogStash.Scripts;
using NetLogStash.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Input
{
    public class ConfigurationInput : AbstractInput
    {
        private IInput _input;

        public override string Type
        {
            get { return (_input != null) ? _input.Type : string.Empty; }
            set { throw new NotImplementedException(); }
        }

        public override string Alias
        {
            get { return (_input != null) ? _input.Alias : string.Empty; }
            set { throw new NotImplementedException(); }
        }

        public override IObservable<Event> Execute()
        {
            return _input.Execute();
        }

        public override void Initialize(string typename, Dictionary<string, ParaItem> paras)
        {
            _input = typename.ConstructInput(typename,paras);
        }

        public override void Dispose()
        {
            if (_input != null) _input.Dispose();
            base.Dispose();
        }
    }
}
