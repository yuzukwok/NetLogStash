using Antlr4.Runtime;
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


            HostFactory.Run(x =>
            {
                x.Service<NetLogStashSrv>(s =>
                {
                    s.ConstructUsing(name => new NetLogStashSrv());
                    s.WhenStarted(svc => svc.Start());
                    s.WhenStopped(svc => svc.Stop());

                });
                x.RunAsLocalSystem();
                x.SetDescription("NetLogStashSrv");
                x.SetDisplayName("NetLogStashSrv");
                x.SetServiceName("NetLogStashSrv");
            });
        }
    }
}
