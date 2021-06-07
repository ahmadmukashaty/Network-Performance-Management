using Syriatel.NPM.WebAPI.Models.ActionsModels;
using Syriatel.NPM.WebAPI.Models.BoModels;
using Syriatel.NPM.WebAPI.Models.Helper;
using Syriatel.NPM.WebAPI.Models.OracleConnections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Syriatel.NPM.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GetActionsController : ApiController
    {
        // GET api/getactions
        int? userID, actionID;
        public object Get(string userAlias)
        {
            try
            {
                userID = OracleHelper.GetUserAliasID(userAlias.ToLower());
                if (userID == null)
                {
                    return new ResponseJson(Constants.FAILED, Constants.Messages.UNAUTHORIZED_USER_ERROR, null);
                }

                actionID = OracleHelper.GetActionID(Constants.GET_DATA_ACTION);
                if (actionID == null)
                {
                    OracleHelper.InsertDataInHistoryLog((int)userID, Constants.NO, Constants.Messages.FAILED_ACTION_ERROR);
                    return new ResponseJson(Constants.FAILED, Constants.Messages.FAILED_ACTION_ERROR, null);
                }

                ActionsData actionsData = new ActionsData((int)userID);

                return new ResponseJson(Constants.SUCCESSED, Constants.Messages.EMPTY_MESSAGE, actionsData.userActions);

            }
            catch (Exception ex)
            {
                if (userID != null)
                {
                    if (actionID == null)
                        OracleHelper.InsertDataInHistoryLog((int)userID, Constants.NO, ex.Message);
                    else
                        OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, Constants.NO, ex.Message);
                }
                return new ResponseJson(Constants.FAILED, Constants.Messages.UNEXPECTED_ERROR, null);
            }
        }
    }
}
