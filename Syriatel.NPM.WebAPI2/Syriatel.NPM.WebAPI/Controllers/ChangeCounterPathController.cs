using Syriatel.NPM.WebAPI.Models.BoModels;
using Syriatel.NPM.WebAPI.Models.Helper;
using Syriatel.NPM.WebAPI.Models.OracleConnections;
using Syriatel.NPM.WebAPI.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace Syriatel.NPM.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ChangeCounterPathController : ApiController
    {
        int? userID, actionID, fileID;
        // POST api/changecounterpath
        [HttpPost]
        public ResponseJson Post([FromBody]ChangePathResponseModelView response)
        {
            try
            {
                userID = OracleHelper.GetUserAliasID(response.userAlias.ToLower());
                if (userID == null)
                {
                    return new ResponseJson(Constants.FAILED, Constants.Messages.UNAUTHORIZED_USER_ERROR, null);
                }

                actionID = OracleHelper.GetActionID(Constants.CHANGE_COUNTER_PATH_ACTION);
                if (actionID == null)
                {
                    OracleHelper.InsertDataInHistoryLog((int)userID, Constants.NO, Constants.Messages.FAILED_ACTION_ERROR);
                    return new ResponseJson(Constants.FAILED, Constants.Messages.FAILED_ACTION_ERROR, null);
                }

                string jsonData = new JavaScriptSerializer().Serialize(response.data);
                OracleHelper.AddFileIntoDB(jsonData, Constants.CHANGE_COUNTER_PATH_TYPE, (int)userID, response.file_name);

                fileID = OracleHelper.GetFileID(response.file_name);
                if (fileID == null)
                {
                    OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, Constants.NO, Constants.Messages.FAILED_SAVE_FILE_ERROR);
                    OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, response.data.counter.Subset.SubsetID, response.data.counter.counterID.ToString(), "", Constants.NO, Constants.Messages.FAILED_SAVE_FILE_ERROR);
                    return new ResponseJson(Constants.FAILED, Constants.Messages.FAILED_SAVE_FILE_ERROR, null);
                }

                 OracleHelper.ChangeCounterPath(response.data);

                OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, response.data.counter.Subset.SubsetID, response.data.counter.counterID.ToString(), fileID.ToString(), Constants.YES, Constants.EMPTY_STRING);
                OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, (int)fileID, Constants.YES, Constants.EMPTY_STRING);
                return new ResponseJson(Constants.SUCCESSED, Constants.Messages.EMPTY_MESSAGE, null);

            }catch(Exception ex)
            {
                if (userID != null)
                {
                    if (actionID == null)
                    {
                        OracleHelper.InsertDataInHistoryLog((int)userID, Constants.NO, ex.Message);
                        OracleHelper.InsertDataInHistoryFileLog((int)userID, Constants.NO, ex.Message);
                    }
                    else
                        if (fileID == null)
                        {
                            OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, response.data.counter.Subset.SubsetID, response.data.counter.counterID.ToString(), "", Constants.NO, ex.Message);
                            OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, Constants.NO, ex.Message);
                        }
                        else
                        {
                            if (response.data != null && response.data.counter != null && response.data.counter.Subset != null)
                            {
                                OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, response.data.counter.Subset.SubsetID, response.data.counter.counterID.ToString(), fileID.ToString(), Constants.NO, ex.Message);
                                OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, (int)fileID, Constants.NO, ex.Message);
                            }
                            else
                            {
                                OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, null, null, fileID.ToString(), Constants.NO, ex.Message);
                                OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, (int)fileID, Constants.NO, ex.Message);
                            }
                        }
                }
                return new ResponseJson(Constants.FAILED, Constants.Messages.UNEXPECTED_ERROR, null);
            }
        }
    }
}