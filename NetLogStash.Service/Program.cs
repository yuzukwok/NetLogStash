using Antlr4.Runtime;
using NetLogStash.Grok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace NetLogStash.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            GrokCollection.Load("GrokPattern");
            var p = "%{IP:client} \\[%{TIMESTAMP_ISO8601:timestamp}\\] \"%{WORD:method} %{URIHOST:site}%{URIPATHPARAM:url}\" %{INT:code} %{INT:request} %{INT:response} - %{NUMBER:took} \\[%{DATA:cache}\\] \"%{DATA:mtag}\" \"%{DATA:agent}\"";
            var str = "65.19.138.33 [2015-05-13T08:04:43+10:00] \"GET datasymphony.com.au/ru/feed/\" 304 385 0 - 0.140 [HIT] \"-\" \"Feedly/1.0 (+http://www.feedly.com/fetcher.html; like FeedFetcher-Google)\"\n";

            var patterns = new GrokCollection().createPattern(p);
            patterns.Parse(str);
          
            //HostFactory.Run(x =>
            //{
            //    x.Service<NetLogStashSrv>(s =>
            //    {
            //        s.ConstructUsing(name => new NetLogStashSrv());
            //        s.WhenStarted(svc => svc.Start());
            //        s.WhenStopped(svc => svc.Stop());

            //    });
            //    x.RunAsLocalSystem();
            //    x.SetDescription("NetLogStashSrv");
            //    x.SetDisplayName("NetLogStashSrv");
            //    x.SetServiceName("NetLogStashSrv");
            //});
        }
    }
}
