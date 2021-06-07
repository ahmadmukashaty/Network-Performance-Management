using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syriatel.NPM.WebAPI.Models.BoModels
{
    public class CounterModelView
    {
        public string counterID { get; set; }
        public string counterName { get; set; }
        public string shortName { get; set; }
        public string path { get; set; }
        public string reference { get; set; }
        public string function_value { get; set; }
        public string show { get; set; }
    }
}