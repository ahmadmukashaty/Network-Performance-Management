using Syriatel.NPM.WebAPI.Models.BoModels;
using Syriatel.NPM.WebAPI.Models.BoTreeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syriatel.NPM.WebAPI.Models.Response
{
    public class ChangePathResponseModelView : ResponseModelView
    {
        public ChangePathModelView data { get; set; }
    }
}