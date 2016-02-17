using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//https://github.com/Beh01der/node-grok/blob/master/lib/index.js
namespace NetLogStash.Grok
{
    public class GrokCollection
    {
        string subPatternsRegex = "%{[A-Z0-9_]+(?::[a-z0-9_]+)?}"; // %{subPattern} or %{subPattern:fieldName}
        string nestedFieldNamesRegex = " ((?<([a-z0-9_]+)>)|(?:|(?>|(?!|(?<!|(|\\(|\\)|)|[|\\[|\\]|]";

        public Hashtable Patterns { get; set; }

        public Hashtable ResolvePattern(Hashtable patterns)
        {
            return null;
        }
        // TODO: support automatic type conversion (e.g., "%{NUMBER:duration:float}"; see: https://www.elastic.co/guide/en/logstash/current/plugins-filters-grok.html)
        public Hashtable resolveSubPatterns(GrokPattern pattern)
        {
            //if (pattern==null) { return pattern; }

            //var expression = pattern.Expression;
            //var subPatterns = expression.match(subPatternsRegex) || [];

            //subPatterns.forEach(function(matched) {
            //    // matched is: %{subPatternName} or %{subPatternName:fieldName}
            //    var subPatternName = matched.substr(2, matched.length - 3);

            //    var elements = subPatternName.split(':');
            //    subPatternName = elements[0];
            //    var fieldName = elements[1];

            //    var subPattern = patterns.get(subPatternName);
            //    if (!subPattern)
            //    {
            //        console.error('Error: pattern "' + subPatternName + '" not found!');
            //        return;
            //    }

            //    if (!subPattern.resolved)
            //    {
            //        resolvePattern(subPattern);
            //    }

            //    if (fieldName)
            //    {
            //        expression = expression.replace(matched, '(?<' + fieldName + '>' + subPattern.resolved + ')');
            //    }
            //    else {
            //        expression = expression.replace(matched, subPattern.resolved);
            //    }
            //});

            //pattern.resolved = expression;
            return pattern;
        }

        // create mapping table for the fieldNames to capture
        public GrokPattern resolveFieldNames(GrokPattern pattern)
        {
            //if (pattern==null) { return null; }

            //var nestLevel = 0;
            //var inRangeDef = 0;
            //var matched;
            //while ((matched = nestedFieldNamesRegex.exec(pattern.resolved)) !== null)
            //{
            //    switch (matched[0])
            //    {
            //        case '(': { if (!inRangeDef) { ++nestLevel; pattern.fields.push(null); } break; }
            //        case '\\(': break; // can be ignored
            //        case '\\)': break; // can be ignored
            //        case ')': { if (!inRangeDef) { --nestLevel; } break; }
            //        case '[': { ++inRangeDef; break; }
            //        case '\\[': break; // can be ignored
            //        case '\\]': break; // can be ignored
            //        case ']': { --inRangeDef; break; }
            //        case '(?:':  // fallthrough                              // group not captured
            //        case '(?>':  // fallthrough                              // atomic group
            //        case '(?!':  // fallthrough                              // negative look-ahead
            //        case '(?<!': { if (!inRangeDef) { ++nestLevel; } break; } // negative look-behind
            //        default: { ++nestLevel; pattern.fields.push(matched[2]); break; }
            //    }
            //}

            return pattern;
        }

        public void createPattern(string expression,string  id)
        {
            //id = id || 'pattern-' + patterns.length;
            //if (patterns.has(id))
            //{
            //    console.error('Error: pattern with id %s already exists', id);
            //}
            //else {
            //    var pattern = new GrokPattern(expression, id);
            //    return resolvePattern(pattern);
            //}
        }

        public void load (string filePath)
        {
            //var patternLineRegex = / ([A - Z0 - 9_] +)\s + (.+) /;

            //lineReader.eachLine(filePath, function(line) {
            //    var elements = patternLineRegex.exec(line);
            //    if (elements && elements.length > 2)
            //    {
            //        var pattern = new GrokPattern(elements[2], elements[1]);
            //        patterns.set(pattern.id, pattern);
            //    }
            //}).then(function() {
            //    next();
            //});
        }

       
    }
}
