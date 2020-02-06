using Syriatel.NPM.Desktop.Designer;
using Syriatel.NPM.Desktop.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syriatel.NPM.Desktop.Models.Classes
{
    public abstract class FileModelView
    {
        protected BoDesigner boDesigner { get; set; }

        public string value { get; set; }

        public abstract ResponseDesigner InsertDataToBo();
    }
}
