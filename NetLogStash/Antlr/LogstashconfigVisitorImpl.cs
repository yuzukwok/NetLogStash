using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using NetLogStash.Config;

namespace NetLogStash
{
   public class LogstashconfigVisitorImpl:LogstashconfigBaseVisitor<string>
    {
        public ConfigMeta Config { get; set; }
        public string curplugtype { get; set; }
        public string curplug { get; set; }

        public override string VisitConfig([NotNull] LogstashconfigParser.ConfigContext context)
        {
            //初始化配置对象
            Config = new ConfigMeta();
            Config.Plugins = new Dictionary<string, IList<Plugin>>();
            return base.VisitConfig(context);
        }

        public override string VisitStage_declaration([NotNull] LogstashconfigParser.Stage_declarationContext context)
        {
            //初始化各个配置节对象
            var type = "";
            var input = context.INPUT();
            if (input!=null)
            {
                type = input.GetText();
            }
            var filter = context.FILTER();
            if (filter != null)
            {
                type = filter.GetText();
            }
            var output = context.OUTPUT();
            if (output != null)
            {
                type = output.GetText();
            }
            if (!Config.Plugins.ContainsKey(type))
            {
                Config.Plugins.Add(type, new List<Plugin>());
            }
            curplugtype = type;
            return base.VisitStage_declaration(context);
        }

        public override string VisitPlugin_declaration([NotNull] LogstashconfigParser.Plugin_declarationContext context)
        {
           
            //获取插件的名字
            var pluginname = context.IDENTIFIER().GetText();
            //获取插件的类型
            Config.Plugins[curplugtype].Add(new Plugin() { Name = pluginname });
            curplug = pluginname;
            return base.VisitPlugin_declaration(context);
        }


        public override string VisitPlugin_attribute([NotNull] LogstashconfigParser.Plugin_attributeContext context)
        {
            //参数名字
            var paraname = context.IDENTIFIER().GetText();
            //参数结果
            var paravalue = context.plugin_attribute_value().GetText();

            ParaItem item = new ParaItem() { Name = paraname };
            item.IsArray = paravalue.Contains("[") && paravalue.Contains("]");
            item.IsHash = paravalue.Contains("=>");
            if (item.IsArray)
            {
                item.Values = new List<string>(paravalue.Replace("[", "").Replace("]", "").Replace("\"", "").Split(','));
            }
            else if (item.IsHash)
            {
                //TODO
                //Hashtable
            }
            else
            {
                item.Values = new List<string>() { paravalue };
            }
            //获取类型

            var plug = Config.Plugins[curplugtype].LastOrDefault();
            if (plug.Params == null)
            {
                plug.Params = new Dictionary<string, ParaItem>();
            }
            if (!plug.Params.ContainsKey(paraname))
            {
                plug.Params.Add(paraname, item);
            }
           
            return base.VisitPlugin_attribute(context);
        }




    }
}
