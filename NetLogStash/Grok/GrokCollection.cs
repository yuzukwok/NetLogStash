using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//https://github.com/Beh01der/node-grok/blob/master/lib/index.js
namespace NetLogStash.Grok
{
    public class GrokCollection
    {
        static  Regex subPatternsRegex =new Regex("%{[A-Z0-9_]+(?::[a-z0-9_]+)?}"); // %{subPattern} or %{subPattern:fieldName}
       // static Regex nestedFieldNamesRegex =new Regex("((?<([a-z0-9_]+)>)|(?:|(?>|(?!|(?<!|(|\\(|\\)|)|[|\\[|\\]|]");
        
        static Hashtable Patterns { get; set; }
        public GrokCollection()
        {
            if (Patterns==null)
            {
                Patterns = new Hashtable();
            }
        }
        public GrokPattern ResolvePattern(GrokPattern pattern)
        {
            pattern = resolveSubPatterns(pattern);
            pattern = resolveFieldNames(pattern);
            return pattern;
        }
        // TODO: support automatic type conversion (e.g., "%{NUMBER:duration:float}"; see: https://www.elastic.co/guide/en/logstash/current/plugins-filters-grok.html)
        private GrokPattern resolveSubPatterns(GrokPattern pattern)
        {
            var express = pattern.Expression;
           var subPatterns= subPatternsRegex.Matches(pattern.Expression);

            foreach (Match item in subPatterns)
            {  // matched is: %{subPatternName} or %{subPatternName:fieldName}
                var matched = item.Value;
                var subPatternName = matched.Substring(2, matched.Length - 3);

                var elements = subPatternName.Split(':');
                subPatternName = elements[0];
                var fieldName = "";
                if (elements.Length>1)
                {
                    fieldName    = elements[1];
                }
              
               
                if (!Patterns.ContainsKey(subPatternName))
                {
                    throw new Exception("不存在模式");
                }
                var subPattern =(GrokPattern) Patterns[subPatternName];

                if (string.IsNullOrEmpty(subPattern.Resolved))
                {
                    ResolvePattern(subPattern);
                }
                if (!string.IsNullOrEmpty(fieldName))
                {
                    express = express.Replace(matched, "(?<" + fieldName + ">" + subPattern.Resolved + ")");
                }
                else {
                    express = express.Replace(matched, subPattern.Resolved);
                }
            }
            pattern.Resolved = express;
            return pattern;
        }

        // create mapping table for the fieldNames to capture
        private GrokPattern resolveFieldNames(GrokPattern pattern)
        {
          
            //var nestLevel = 0;
            //var inRangeDef = 0;
            //var matched="";
            //var matches=nestedFieldNamesRegex.Matches(pattern.Resolved);
            //matched = matches[0].Value;
            //    switch (matched[0].ToString())
            //    {
            //        case "(": { if (inRangeDef>0) { ++nestLevel; pattern.Fields.Add(null); } break; }
            //        case "\\(": break; // can be ignored
            //        case "\\)": break; // can be ignored
            //        case ")": { if (inRangeDef>0) { --nestLevel; } break; }
            //        case "[": { ++inRangeDef; break; }
            //        case "\\[": break; // can be ignored
            //        case "\\]": break; // can be ignored
            //        case "]": { --inRangeDef; break; }
            //        case "(?:":  // fallthrough                              // group not captured
            //        case "(?>":  // fallthrough                              // atomic group
            //        case "(?!":  // fallthrough                              // negative look-ahead
            //        case "(?<!": { if (inRangeDef>0) { ++nestLevel; } break; } // negative look-behind
            //        default: { ++nestLevel; pattern.Fields.Add(matched[2].ToString()); break; }
            //    }
            

            return pattern;
        }

        public GrokPattern createPattern(string expression,string  id="")
        {
            var pid = id;
            if (string.IsNullOrEmpty(pid))
            {
                pid = "pattern-" + Patterns.Count;
            }
            if (Patterns.Contains(pid))
            {
                throw new Exception("已存在的模式id");
            }
            else
            {
                GrokPattern pattern = new GrokPattern() { Id = pid, Expression = expression };
                return ResolvePattern(pattern);
            }
           
        }

        public static void Load (string filePath)
        {
            if (Patterns == null)
            {
                Patterns = new Hashtable();
            }
            var files= Directory.GetFiles(filePath);
            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);
                foreach (var item in lines)
                {
                    if (!item.StartsWith("#")&&!item.StartsWith(" ")&&!string.IsNullOrEmpty(item))
                    {
                        var id = item.Split(' ')[0];
                    var express = item.Substring(item.IndexOf(' '));
                    var pattern = new GrokPattern() { Expression = express, Id = id };
                        if (!Patterns.ContainsKey(pattern.Id))
                        {
                            Patterns.Add(pattern.Id, pattern);
                        }
                   
                    }
                }
                
            }

         
        }

       
    }
}
