using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash
{
    public sealed class Event : DynamicObject
    {
        private Dictionary<string, object> _dictionary = new Dictionary<string, object>();
        private string _eventAlias;

        public string EventType { get; set; }
        public string EventAlias
        {
            get { return (!string.IsNullOrEmpty(_eventAlias)) ? _eventAlias : EventType; }
            set { _eventAlias = value; }
        }
        public DateTime EventTimeStamp { get; set; }

        public object this[string key]
        {
            get { return GetMember(key); }
            set { SetMember(key, value); }
        }

        public Event()
        {
            EventTimeStamp = DateTime.Now;
        }

        public Event(string type, string alias)
            : this()
        {
            EventType = type;
            EventAlias = alias;
        }

        public Event(string type, string alias, DateTime timeStamp)
        {
            EventType = type;
            EventAlias = alias;
            EventTimeStamp = timeStamp;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name.ToLower();

            return _dictionary.TryGetValue(name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name.ToLower()] = value;

            return true;
        }

        public object GetMember(string propName)
        {
            var binder = Binder.GetMember(CSharpBinderFlags.None,
                  propName, this.GetType(),
                  new List<CSharpArgumentInfo>{
                       CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)});
            var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);

            return callsite.Target(callsite, this);
        }

        public void SetMember(string propName, object val)
        {
            var binder = Binder.SetMember(CSharpBinderFlags.None,
                   propName, this.GetType(),
                   new List<CSharpArgumentInfo>{
                       CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                       CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)});
            var callsite = CallSite<Func<CallSite, object, object, object>>.Create(binder);

            callsite.Target(callsite, this, val);
        }

        public void RemoveMember(string propName)
        {
            _dictionary.Remove(propName.ToLower());
        }

        public bool HasMember(string propName)
        {
            return (propName == "EventType" || propName == "EventAlias" || propName == "EventTimeStamp" || _dictionary.ContainsKey(propName.ToLower()));
        }

        public IEnumerable<KeyValuePair<string, object>> GetMembers(bool includeInstanceMembers = false)
        {
            if (includeInstanceMembers)
            {
                yield return new KeyValuePair<string, object>("EventType", EventType);
                yield return new KeyValuePair<string, object>("EventAlias", EventAlias);
                yield return new KeyValuePair<string, object>("EventTimeStamp", EventTimeStamp);
            }

            foreach (var kvp in _dictionary)
            {
                yield return kvp;
            }
        }

        public string AsJson(bool includeInstanceMembers = false)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();
                foreach (var item in GetMembers(includeInstanceMembers))
                {
                    writer.WritePropertyName(item.Key);
                    writer.WriteValue(item.Value);
                }
                writer.WriteEndObject();

                return sw.ToString();
            }
        }

        public string AsXml(bool includeInstanceMembers = false)
        {
            return JsonConvert.DeserializeXmlNode(AsJson(includeInstanceMembers), "Event").OuterXml;
        }

        public string AsString()
        {
            string source = EventType;
            if (EventAlias != EventType) source += ":" + EventAlias;
            return string.Format("[{0}] [{1}] {2}", EventTimeStamp, source, AsJson(false));
        }
    }
}
