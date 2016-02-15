using NetLogStash.Config;
using NetLogStash.Interface;
using NetLogStash.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.EventCore
{
    public class EventHerder : IDisposable
    {
        private readonly IDisposable _pipeline;
        private readonly IDisposable _configuration;

        public EventHerder(NetLogStash.Config.IConfiguration configuration)
        {
            _configuration = configuration;

            // check configuration validity
            if (!IsConfigurationValid(configuration)) throw new NetLogStash.Config.InvalidConfigurationException(configuration);

            // initialize keystore
            KeyStore.Initialize(configuration.KeyStoreLocation);

            // create pipeline
            _pipeline = new Pipeline<Event>(
                    configuration.Inputs.Select(i => i.Execute()),
                    configuration.Filters.Select(f => new KeyValuePair<Func<Event, bool>, Func<Event, Event>>(f.Predicate, f.Execute)),
                    configuration.Outputs.Select<IOutput, Action<Event>>(o => o.Execute)
                );
        }

        public EventHerder(Type configurationType, params object[] args)
            : this(configurationType.Construct(args) as NetLogStash.Config.IConfiguration)
        {
        }

        public void Dispose()
        {
            if (_pipeline != null) _pipeline.Dispose();
            if (_configuration != null) _configuration.Dispose();
        }

        private bool IsConfigurationValid(NetLogStash.Config.IConfiguration configuration)
        {
            return (configuration != null && configuration.Inputs != null && configuration.Filters != null && configuration.Outputs != null && configuration.Inputs.Count > 0 && configuration.Outputs.Count > 0);
        }
    }
}
