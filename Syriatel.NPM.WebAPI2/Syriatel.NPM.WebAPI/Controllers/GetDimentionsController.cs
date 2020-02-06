using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FTPConnection;
using System.Web.Http.Cors;
using Syriatel.NPM.WebAPI.Models.BoModels;
using Syriatel.NPM.WebAPI.Models.Helper;
using Syriatel.NPM.WebAPI.Models.OracleConnections;
using System.Reflection;

namespace Syriatel.NPM.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GetDimentionsController : ApiController
    {
        // GET api/getdimentions/67109391
        //api/getdimentions/?subsetID=67109391
        int? userID, actionID;
        public ResponseJson Get(string userAlias, string subsetID)
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

                IU2000FTPFileManager ftpClient = null;
                ftpClient = U200FtpMonitor.GetInstance();
                List<string> dims = null;
                bool activatedSubset = ftpClient.GetDimentionsFromDirectory(Constants.DIMENTION_DIRECTORY_PATH, subsetID, ref dims);
                if (activatedSubset)
                {
                    return new ResponseJson(Constants.SUCCESSED, Constants.Messages.EMPTY_MESSAGE, dims);
                }
                else
                {
                    OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, Constants.NO, Constants.Messages.UNACTIVATE_SUBSET_ERROR);
                    return new ResponseJson(Constants.FAILED, Constants.Messages.UNACTIVATE_SUBSET_ERROR, null);
                }

            }catch(Exception ex)
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
