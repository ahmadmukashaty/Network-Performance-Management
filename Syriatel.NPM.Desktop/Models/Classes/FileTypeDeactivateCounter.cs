using Syriatel.NPM.Desktop.Designer;
using Syriatel.NPM.Desktop.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Syriatel.NPM.Desktop.Models.Helper;
using System.Windows.Forms;

namespace Syriatel.NPM.Desktop.Models.Classes
{
    public class FileTypeDeactivateCounter : FileModelView
    {
        public int? counterID { get; set; }

        public SubsetDescriptor Subset { get; set; }

        public string tableCounterName { get; set; }

        public string tableName { get; set; }

        public ObjectType_Enum valueType { get; set; }

        public string path { get; set; }

        public string reference { get; set; }

        public ObjectFunction_Enum functionType { get; set; }

        public override ResponseDesigner InsertDataToBo()
        {
            try
            {
                //open designer
                this.boDesigner = new BoDesigner(this.Subset.Universe);
                this.boDesigner.OpenApplication();
                this.boDesigner.OpenUniverse();

                //deactivate counter process
                this.boDesigner.HideItem(this.path, this.value);

                //close designer
                this.boDesigner.CloseUniverse();
                this.boDesigner.ExportUniverse();
                this.boDesigner.CloseApplication();

                return new ResponseDesigner(Constants.SUCCESSED, Constants.DEACTIVATE_COUNTER_TYPE, Constants.Messages.EMPTY_MESSAGE);
            }
            catch (Exception ex)
            {
                this.boDesigner.CloseApplication();
                return new ResponseDesigner(Constants.FAILED, Constants.DEACTIVATE_COUNTER_TYPE, ex.Message);
            }
        }
    }
}
