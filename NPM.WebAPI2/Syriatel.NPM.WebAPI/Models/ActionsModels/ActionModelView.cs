using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syriatel.NPM.WebAPI.Models.ActionsModels
{
    public class ActionModelView
    {
        public string ActionName { get; set; }

        public string SubsetId { get; set; }

        public string CounterId { get; set; }

        public string FileId { get; set; }

        public string FileName { get; set; }

        public string Date { get; set; }

        public string SuccessInDB { get; set; }

        public string SuccessInBO { get; set; }
    }
}