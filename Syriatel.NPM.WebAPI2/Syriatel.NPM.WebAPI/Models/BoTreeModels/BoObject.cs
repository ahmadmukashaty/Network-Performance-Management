using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syriatel.NPM.BoManager
{
    public abstract class BoObject
    {
        [JsonProperty(Order = 1)]
        public string value { get; set; }

        [JsonProperty(Order = 2)]
        public int id { get; set; }
    }
}
