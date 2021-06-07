using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syriatel.NPM.BoManager
{
    public class ClassDescriptor : BoObject
    {
         [JsonProperty(Order = 3)]
        public IList<BoObject> children;
    }
}
