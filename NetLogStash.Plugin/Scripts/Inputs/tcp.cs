using NetLogStash.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetLogStash.Config;
using NetLogStash;

public class tcpInput : AbstractInput, IDisposable
    {
        public string Host { get; set; } = "0.0.0.0";
        public int Port { get; set; } = 9999;

        public string codec { get; set; } = "line";



        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public override IObservable<Event> Execute()
        {
            return null;
        }

        public override void Initialize(string typename, Dictionary<string, ParaItem> paras)
        {
            foreach (var item in paras)
            {
                Console.WriteLine(item.Value.Values.FirstOrDefault());
            }
          
        }
    }
