using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syriatel.NPM.BoManager
{

    //SubsetID is priamry Key
    public class SubsetDescriptor
    {
        public string SubsetID { get; set; }
        
        public string SubsetName { get; set; }

        public string Universe { get; set; }

        public SubsetActivation_Enum IsActive { get; set; }

    }
}
