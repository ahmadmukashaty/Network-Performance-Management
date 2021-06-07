using Syriatel.NPM.Desktop.Designer;
using Syriatel.NPM.Desktop.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syriatel.NPM.Desktop.Models.Classes
{
    public class FileTypeAddSubset : FileModelView
    {
        public string release { get; set; }
        public string unv { get; set; }
        public string subsetID { get; set; }
        public string active { get; set; }
        public List<CounterModelView> measurements { get; set; }
        public List<DimantionModelView> dimentions { get; set; }

        public override ResponseDesigner InsertDataToBo()
        {
            try
            {
                //open designer
                this.boDesigner = new BoDesigner(this.unv);
                this.boDesigner.OpenApplication();
                this.boDesigner.OpenUniverse();

                //add subset process
                string tableName = OracleHelper.GetSubsetTableName(this.subsetID);
                
                this.boDesigner.AddSubsetToBO(this, tableName);

                //close designer
                this.boDesigner.CloseUniverse();
                this.boDesigner.ExportUniverse();
                this.boDesigner.CloseApplication();

                return new ResponseDesigner(Constants.SUCCESSED, Constants.ADDSUBSET_TYPE, Constants.Messages.EMPTY_MESSAGE);

            }catch(Exception ex)
            {
                this.boDesigner.CloseApplication();
                return new ResponseDesigner(Constants.FAILED, Constants.ADDSUBSET_TYPE, ex.Message);
            }
        }
    }
}
