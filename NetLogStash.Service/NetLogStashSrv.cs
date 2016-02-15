using System;
using System.IO;
using NetLogStash.EventCore;
using NetLogStash.Config;

namespace NetLogStash.Service
{
    public class NetLogStashSrv
    {

        private IDisposable _eventHerder;
        public void Start()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            string configLocation = "logstash.conf";

            _eventHerder = new EventHerder(typeof(JsonConfiguration), configLocation);

        }
        public void Stop()
        {
            if (_eventHerder != null) _eventHerder.Dispose();
        }
    }
}
