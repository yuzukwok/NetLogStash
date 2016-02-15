using Antlr4.Runtime;
using NetLogStash.Filters;
using NetLogStash.Input;
using NetLogStash.Interface;
using NetLogStash.Output;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Config
{
    public class JsonConfiguration : AbstractConfiguration
    {
        public JsonConfiguration(string fileName)
        {
            Inputs = new List<IInput>();
            Filters = new List<IFilter>();
            Outputs = new List<IOutput>();

            string json = File.ReadAllText(fileName);
            //parse
            AntlrInputStream inputStream = new AntlrInputStream(json);
            var lexer = new LogstashconfigLexer(inputStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            var parser = new LogstashconfigParser(tokens);
            var tree = parser.config();
            Console.WriteLine(tree.ToStringTree(parser));
            LogstashconfigVisitorImpl vistor = new LogstashconfigVisitorImpl();
            vistor.Visit(tree);
            var config = vistor.Config;

            KeyStoreLocation = "";


            foreach (Plugin input in config.Plugins["input"])
            {
                AddInput(input);
            }
            if (config.Plugins.ContainsKey("filter"))
            {
                foreach (Plugin filter in config.Plugins["filter"])
                {
                    AddFilter(filter);
                }
            }
                      
            foreach (Plugin output in config.Plugins["output"])
            {
                AddOutput(output);
            }
        }

        private void AddInput(Plugin value)
        {
            ConfigurationInput input = new ConfigurationInput();

            input.Initialize(value.Name,value.Params);

            Inputs.Add(input);
        }

        private void AddFilter(Plugin value)
        {
            ConfigurationFilter filter = new ConfigurationFilter();

            //filter.Initialize(GetArgs((string)value.type, (string)(value.predicate ?? string.Empty), value.args));
            Filters.Add(filter);
        }

        private void AddOutput(Plugin value)
        {
            ConfigurationOutput output = new ConfigurationOutput();

           // output.Initialize(GetArgs((string)value.type, value.args));
            Outputs.Add(output);
        }

        private string[] GetArgs(string v1, dynamic args)
        {
            List<string> argsList = new List<string>();
            argsList.Add(v1);
            if (args != null)
            {
                foreach (string value in args)
                {
                    argsList.Add(value);
                }
            }
            return argsList.ToArray();
        }

        private string[] GetArgs(string v1, string v2, dynamic args)
        {
            List<string> argsList = new List<string>();
            argsList.Add(v1);
            argsList.Add(v2);
            if (args != null)
            {
                foreach (string value in args)
                {
                    argsList.Add(value);
                }
            }
            return argsList.ToArray();
        }
    }
}
