using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBot.Model
{
    public class Value
    {
        public string entity { get; set; }
        public string type { get; set; }
        public double score { get; set; }
    }

    public class Parameter
    {
        public string name { get; set; }
        public bool required { get; set; }
        public List<Value> value { get; set; }
    }

    public class Action
    {
        public bool triggered { get; set; }
        public string name { get; set; }
        public List<Parameter> parameters { get; set; }
    }

    public class IntentRoot
    {
        public string intent { get; set; }
        public double score { get; set; }
        public List<Action> actions { get; set; }
    }
}