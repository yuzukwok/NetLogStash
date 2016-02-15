using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash
{
    internal class Pipeline<T> : IDisposable
    {
        private event EventHandler<EventArgs> _event;
        private readonly List<IDisposable> _inputSubscriptions = new List<IDisposable>();
        private readonly List<IDisposable> _outputSubscriptions = new List<IDisposable>();

        public Pipeline(IEnumerable<IObservable<T>> inputs, IEnumerable<KeyValuePair<Func<T, bool>, Func<T, T>>> filters, IEnumerable<Action<T>> outputs)
        {
            // create output source
            IObservable<T> outputSource = Observable.FromEventPattern<EventHandler<EventArgs>, EventArgs>(a => { _event += a; }, a => { _event -= a; })
                .Select(e => e.EventArgs.Value);

            // subscribe outputs
            outputs.ToList().ForEach(output => Subscribe(_outputSubscriptions, outputSource, output));

            // subscribe inputs
           // Subscribe(_inputSubscriptions, inputs.Merge(), input => Filter(filters, input));
        }

        public void Dispose()
        {
            _inputSubscriptions.ForEach(s => { if (s != null) s.Dispose(); });
            _outputSubscriptions.ForEach(s => { if (s != null) s.Dispose(); });
        }

        private void Subscribe(List<IDisposable> subscriptions, IObservable<T> source, Action<T> handler)
        {
            subscriptions.Add(source.Subscribe(handler));
        }

        private void Filter(IEnumerable<KeyValuePair<Func<T, bool>, Func<T, T>>> filters, T value)
        {
            foreach (KeyValuePair<Func<T, bool>, Func<T, T>> filter in filters)
            {
                if ((filter.Key != null) ? filter.Key(value) : true)
                {
                    value = filter.Value(value);
                    if (value == null)
                    {
                        return;
                    }
                }
            }
            _event(this, new EventArgs(value));
        }

        private class EventArgs
        {
            public T Value { get; private set; }

            public EventArgs(T value) { Value = value; }
        }
    }
}
