using System;
using System.Collections.Generic;

namespace Eknowledger.Language.Xpress
{
    public class XpressRuntimeContext : Dictionary<string, string>
    {
        public XpressRuntimeContext() : base(StringComparer.InvariantCultureIgnoreCase) { }

        public bool Exists(string key)
        {
            return ContainsKey(key);
        }

        public string Get(string variable)
        {
            if (!ContainsKey(variable))
                throw new Exception($"Variable [{variable}] was not found.");
            return this[variable];
        }
    }
}
