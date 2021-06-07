using Syriatel.NPM.Desktop.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syriatel.NPM.Desktop.Models.Classes
{
    public class BoFile
    {
        public int userID { get; set; }
        public int fileID { get; set; }
        public string fileName { get; set; }
        public FileModelView fileData { get; set; }
        public string fileType { get; set; }

        public ResponseDesigner InsertDataToBusinesObjects()
        {
            if (fileData.GetType() == typeof(FileTypeDeactivateCounter))
            {
                FileTypeDeactivateCounter deactivateCounterType = (FileTypeDeactivateCounter)this.fileData;
                return deactivateCounterType.InsertDataToBo();
            }

            if (fileData.GetType() == typeof(FileTypeAddSubset))
            {
                FileTypeAddSubset addSubsetType = (FileTypeAddSubset)this.fileData;
                return addSubsetType.InsertDataToBo();
            }

            return new ResponseDesigner(Constants.FAILED, null, Constants.Messages.FILE_TYPE_ERROR);
        }
    }
}
