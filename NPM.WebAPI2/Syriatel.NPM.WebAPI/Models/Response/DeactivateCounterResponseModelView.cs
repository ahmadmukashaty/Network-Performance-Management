using Syriatel.NPM.BoManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syriatel.NPM.WebAPI.Models.BoModels
{
    public class DeactivateCounterResponseModelView : ResponseModelView
    {
        public CounterDescriptor data { get; set; }
    }
}