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
    public class GetUserRoleController : ApiController
    {
        int? userID, actionID;

        // get api/getuserrole
        public ResponseJson Get(string userAlias)
        {
            try
            {
                userID = OracleHelper.GetUserAliasID(userAlias.ToLower());
                if (userID == null)
                {
                    return new ResponseJson(Constants.FAILED, Constants.Messages.UNAUTHORIZED_USER_ERROR, null);
                }

                actionID = OracleHelper.GetActionID(Constants.LOGIN_ACTION);
                if (actionID == null)
                {
                    OracleHelper.InsertDataInHistoryLog((int)userID, Constants.NO, Constants.Messages.FAILED_ACTION_ERROR);
                    return new ResponseJson(Constants.FAILED, Constants.Messages.FAILED_ACTION_ERROR, null);
                }

                string userRole = OracleHelper.GetUserAliasRole(userAlias.ToLower());

                OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, Constants.YES, Constants.EMPTY_STRING);
                return new ResponseJson(Constants.SUCCESSED, Constants.Messages.EMPTY_MESSAGE, userRole);
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