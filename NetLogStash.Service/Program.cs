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
            var input = "input{file {path => [\"/var/log/*.log\", \"/var/log/message\"]        type => \"system\"        start_position => \"beginning\" } }filter {grok {match => {\"message\" => \"34343\"     }  }  }  output{stdout{codec=>rubydebug}}";

            AntlrInputStream inputStream = new AntlrInputStream(input);
            var lexer = new LogstashconfigLexer(inputStream);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            var parser = new LogstashconfigParser(tokens);            
            var tree = parser.config();
            Console.WriteLine(tree.ToStringTree(parser));
            LogstashconfigVisitorImpl vistor = new LogstashconfigVisitorImpl();
            vistor.Visit(tree);

            vistor.Config.ToString();
            Console.ReadKey();

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
