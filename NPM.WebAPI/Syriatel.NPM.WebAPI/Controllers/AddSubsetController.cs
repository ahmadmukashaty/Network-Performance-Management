using FTPConnection;
using Syriatel.NPM.BoManager.Designer;
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
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Syriatel.NPM.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AddSubsetController : ApiController
    {
        int? userID, actionID, fileID;

        [HttpPost]
        // POST api/addsubset
        public ResponseJson Post([FromBody]AddSubsetResponseModelView response)
        {
            try
            {
                userID = OracleHelper.GetUserAliasID(response.userAlias.ToLower());
                if (userID == null)
                {
                    return new ResponseJson(Constants.FAILED, Constants.Messages.UNAUTHORIZED_USER_ERROR, null);
                }

                actionID = OracleHelper.GetActionID(Constants.ADD_SUBSET_ACTION);
                if (actionID == null)
                {
                    OracleHelper.InsertDataInHistoryLog((int)userID, Constants.NO, Constants.Messages.FAILED_ACTION_ERROR);
                    return new ResponseJson(Constants.FAILED, Constants.Messages.FAILED_ACTION_ERROR, null);
                }

                //save response info in file table in db
                string jsonData = new JavaScriptSerializer().Serialize(response.data);
                OracleHelper.AddFileIntoDB(jsonData, Constants.ADDSUBSET_TYPE, (int)userID, response.file_name);

                fileID = OracleHelper.GetFileID(response.file_name);
                if (fileID == null)
                {
                    OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, Constants.NO, Constants.Messages.FAILED_SAVE_FILE_ERROR);
                    OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, response.data.subsetID, Constants.EMPTY_STRING, Constants.EMPTY_STRING, Constants.NO, Constants.Messages.FAILED_SAVE_FILE_ERROR);
                    return new ResponseJson(Constants.FAILED, Constants.Messages.FAILED_SAVE_FILE_ERROR, null);
                }

                //add subset and counters info in db
                bool existSubset = OracleHelper.DBHasSubset(response.data.subsetID);
                if (existSubset)
                {
                    response.data.AddSubset(existSubset, (int)userID, (int)fileID);
                }
                else
                {
                    IU2000FTPFileManager ftpClient = null;
                    ftpClient = U200FtpMonitor.GetInstance();
                    bool activatedSubset = ftpClient.FileIsExistInDirectory(Constants.DIMENTION_DIRECTORY_PATH, response.data.subsetID);
                    if(!activatedSubset)
                    {
                        OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, response.data.subsetID, Constants.EMPTY_STRING, fileID.ToString(), Constants.NO, Constants.Messages.UNACTIVATE_SUBSET_ERROR);
                        OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, (int)fileID, Constants.NO, Constants.Messages.UNACTIVATE_SUBSET_ERROR);
                        return new ResponseJson(Constants.FAILED, Constants.Messages.UNACTIVATE_SUBSET_ERROR, null);
                    }
                }
                response.data.AddSubset(existSubset, (int)userID, (int)fileID);
                OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, response.data.subsetID, Constants.EMPTY_STRING, fileID.ToString(), Constants.YES, Constants.EMPTY_STRING);
                OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, (int)fileID, Constants.YES, Constants.EMPTY_STRING);
                return new ResponseJson(Constants.SUCCESSED, Constants.Messages.EMPTY_MESSAGE, null);
            }
            catch(Exception ex)
            {
                if (userID != null)
                {
                    if (actionID == null)
                    {
                        OracleHelper.InsertDataInHistoryLog((int)userID, Constants.NO, ex.Message);
                        OracleHelper.InsertDataInHistoryFileLog((int)userID, Constants.NO, ex.Message);
                    }
                    else
                    {
                        if (fileID == null)
                        {
                            OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, response.data.subsetID, Constants.EMPTY_STRING, Constants.EMPTY_STRING, Constants.NO, ex.Message);
                            OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, Constants.NO, ex.Message);
                        }
                        else
                        {
                            if (response.data != null)
                            {
                                OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, response.data.subsetID, Constants.EMPTY_STRING, fileID.ToString(), Constants.NO, ex.Message);
                                OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, (int)fileID, Constants.NO, ex.Message);
                            }
                            else
                            {
                                OracleHelper.InsertDataInHistoryLog((int)userID, (int)actionID, null, Constants.EMPTY_STRING, fileID.ToString(), Constants.NO, ex.Message);
                                OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, (int)fileID, Constants.NO, ex.Message);
                            }
                        }
                    }
                }
                return new ResponseJson(Constants.FAILED, Constants.Messages.UNEXPECTED_ERROR, null);
            }
        }
    }
}
