using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syriatel.NPM.Desktop.Models.Helper
{
    public class ResponseDesigner
    {
        public bool successed { get; set; }

        public string file_type { get; set; }

        public string errorMessage { get; set; }

        public ResponseDesigner() { }

        public ResponseDesigner(bool successed, string file_type, string errorMessage)
        {
            this.file_type = file_type;
            this.successed = successed;
            this.errorMessage = errorMessage;
        }
    }
}
