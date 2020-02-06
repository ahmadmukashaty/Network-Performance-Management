using Oracle.ManagedDataAccess.Client;
using Syriatel.NPM.BoManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Syriatel.NPM.WebAPI.Models.BoModels
{
    public class ResponseModelView
    {
        public string file_name { get; set; }

        public string userAlias { get; set; }

    }
}