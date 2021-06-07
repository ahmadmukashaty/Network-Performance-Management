using Syriatel.NPM.WebAPI.Models.OracleConnections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syriatel.NPM.WebAPI.Models.ActionsModels
{
    public class ActionsData
    {
        public List<ActionModelView> userActions = new List<ActionModelView>();

        public ActionsData(int userID)
        {
            userActions = OracleHelper.GetUserActions(userID);
            GetActionBoStatus();
        }

        private void GetActionBoStatus()
        {
            for(int i=0; i<userActions.Count; i++)
            {
                bool isDone = OracleHelper.CheckFileBoStatus(userActions[i].FileId);
                userActions[i].SuccessInBO = (isDone)? "Y" : "N";      
            }
        }
    }
}