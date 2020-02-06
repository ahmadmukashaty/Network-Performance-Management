using Syriatel.NPM.BoManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syriatel.NPM.WebAPI.Models.BoTreeModels
{
    public class ChangePathModelView
    {
        public string actionType { get; set; }

        public CounterDescriptor counter { get; set; }

        public string newPath { get; set; }
    }
}