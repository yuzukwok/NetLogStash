using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace NetLogStash
{
   public class LogstashconfigVisitorImpl:LogstashconfigBaseVisitor<string>
    {
        public Config Config { get; set; }
        public string curplugtype { get; set; }
        public string curplug { get; set; }

        public override string VisitConfig([NotNull] LogstashconfigParser.ConfigContext context)
        {
            //初始化配置对象
            Config = new Config();
            Config.Plugins = new Dictionary<string, IList<Plugin>>();
            return base.VisitConfig(context);
        }

        public override string VisitPlugin_section([NotNull] LogstashconfigParser.Plugin_sectionContext context)
        {
            //初始化各个配置节对象
            var type = Visit(context.plugin_type());
            if (!Config.Plugins.ContainsKey(type))
            {
                Config.Plugins.Add(type, new List<Plugin>());
            }
            curplugtype = type;
            return base.VisitPlugin_section(context);
        }

        public override string VisitPlugin([NotNull] LogstashconfigParser.PluginContext context)
        {
            //获取插件的名字
            var pluginname = Visit(context.name());
            //获取插件的类型
            var type = curplugtype;
            if (!Config.Plugins.ContainsKey(type))
            {
                Config.Plugins.Add(type, new List<Plugin>());                
            }
            Config.Plugins[type].Add(new Plugin() { Name = pluginname });
            curplug = pluginname;
            return base.VisitPlugin(context);
        }

        public override string VisitName([NotNull] LogstashconfigParser.NameContext context)
        {
            return context.GetText();
        }

        public override string VisitPlugin_type([NotNull] LogstashconfigParser.Plugin_typeContext context)
        {
           return context.GetText();           
        }

        public override string VisitString([NotNull] LogstashconfigParser.StringContext context)
        {
            return context.GetText();
        }

        public override string VisitValue([NotNull] LogstashconfigParser.ValueContext context)
        {
            return context.GetText();
        }


        public override string VisitNumber([NotNull] LogstashconfigParser.NumberContext context)
        {
            return context.GetText();
        }

      

        public override string VisitAttribute([NotNull] LogstashconfigParser.AttributeContext context)
        {
            //参数名字
            var paraname= Visit(context.name());
            //参数结果
            var paravalue = Visit(context.value());

            ParaItem item = new ParaItem() { Name = paraname };
            item.IsArray = paravalue.Contains("[") && paravalue.Contains("]");
            item.IsHash = paravalue.Contains("=>");
            if (item.IsArray)
            {
               item.Values=new List<string>(paravalue.Replace("[", "").Replace("]", "").Replace("\"", "").Split(','));
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
            if (plug.Params==null)
            {
                plug.Params = new Dictionary<string, ParaItem>();
            }
            if (!plug.Params.ContainsKey(paraname))
            {
                plug.Params.Add(paraname, item);
            }
            return base.VisitAttribute(context);
        }




    }
}
