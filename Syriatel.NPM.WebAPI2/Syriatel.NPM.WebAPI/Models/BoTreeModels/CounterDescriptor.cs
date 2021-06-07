using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syriatel.NPM.BoManager
{
    public class CounterDescriptor : BoObject
    {
        public int? counterID { get; set; }

        public SubsetDescriptor Subset { get; set; }

        public string tableCounterName { get; set; }

        public string tableName { get; set; }

        public ObjectType_Enum valueType { get; set; }

        public string path { get; set; }

        public string reference { get; set; }

        public ObjectFunction_Enum functionType { get; set; }
        
    }
}
