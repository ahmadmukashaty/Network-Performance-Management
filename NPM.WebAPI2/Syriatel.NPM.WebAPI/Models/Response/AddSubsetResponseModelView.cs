using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Syriatel.NPM.WebAPI.Models.BoModels
{
    public class AddSubsetResponseModelView : ResponseModelView
    {
        public SubsetModelView data { get; set; }
    }
}