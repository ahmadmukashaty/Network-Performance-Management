using Oracle.ManagedDataAccess.Client;
using Syriatel.NPM.Desktop.Models.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Syriatel.NPM.Desktop.Models.Helper
{
    public class OracleHelper
    {
        private static OracleConnection oraConnection = null;

        private static void OpenConnection()
        {
            if (oraConnection.State != ConnectionState.Open)
            {
                oraConnection.Open();
            }
        }

        private static void CloseConnection()
        {
            if (oraConnection.State == ConnectionState.Open)
            {
                oraConnection.Close();
            }
        }

        public static int? GetUserAliasID(string userAlias)
        {
            int? userID = null;
            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                const string DATA_SELECTED = "ID";
                const string TABLENAME = "HOURLYFILES.NPM_USERS";
                string CONDITION = "NAME = '" + userAlias + "'";

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT " + DATA_SELECTED + " FROM " + TABLENAME + " WHERE " + CONDITION;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userID = reader.IsDBNull(reader.GetOrdinal("ID")) ? null : (int?)reader.GetInt64(reader.GetOrdinal("ID"));
                        }
                    }
                }

                CloseConnection();

                return userID;
            }
        }

        public static string GetUserAliasRole(string userAlias)
        {
            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                const string DATA_SELECTED = " ro.ROLE_TYPE ";
                const string TABLENAME = " HOURLYFILES.NPM_USERS ur left outer join   HOURLYFILES.NPM_ROLES ro on ur.ROLEID = ro.ID ";
                string CONDITION = "ur.NAME= '" + userAlias + "'";

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT " + DATA_SELECTED + " FROM " + TABLENAME + " WHERE " + CONDITION;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.IsDBNull(reader.GetOrdinal("ROLE_TYPE")) ? null : reader.GetString(reader.GetOrdinal("ROLE_TYPE"));
                        }
                    }
                }

                CloseConnection();

                return null;
            }
        }

        public static BoFile GetBoFileData(string file_name)
        {
            BoFile boFile = null;
            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                const string DATA_SELECTED = "*";
                const string TABLENAME = "HOURLYFILES.NPM_FILES";
                string CONDITION = "NAME = '" + file_name + "'";
                string ORDERBY = " ORDER BY CREATED_DATE DESC";

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT " + DATA_SELECTED + " FROM " + TABLENAME + " WHERE " + CONDITION + ORDERBY;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JavaScriptSerializer j = new JavaScriptSerializer();

                            boFile = new BoFile();
                            boFile.fileID = (int)reader.GetInt64(reader.GetOrdinal("ID"));
                            boFile.fileName = reader.GetString(reader.GetOrdinal("NAME"));
                            boFile.fileType = reader.GetString(reader.GetOrdinal("FILE_TYPE"));
                            
                            string jsonString = reader.GetString(reader.GetOrdinal("FILE_DATA"));
                            if (boFile.fileType == Constants.ADDSUBSET_TYPE)
                            {
                                boFile.fileData = (FileTypeAddSubset)j.Deserialize(jsonString, typeof(FileTypeAddSubset));
                            }
                            else if (boFile.fileType == Constants.DEACTIVATE_COUNTER_TYPE)
                            {
                                boFile.fileData = (FileTypeDeactivateCounter)j.Deserialize(jsonString, typeof(FileTypeDeactivateCounter));
                            }
                            else
                            {
                                boFile = null;
                                break;
                            }
                            break;
                        }
                    }
                }

                CloseConnection();

                return boFile;
            }
        }

        public static int? GetActionID(string action_name)
        {
            int? actionID = null;
            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                const string DATA_SELECTED = "ID";
                const string TABLENAME = "HOURLYFILES.NPM_ACTIONS";
                string CONDITION = "NAME = '" + action_name + "'";

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT " + DATA_SELECTED + " FROM " + TABLENAME + " WHERE " + CONDITION;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            actionID = reader.IsDBNull(reader.GetOrdinal("ID")) ? null : (int?)reader.GetInt64(reader.GetOrdinal("ID"));
                        }
                    }
                }

                CloseConnection();

                return actionID;
            }
        }

        public static bool CheckSubsetExist(string subsetID)
        {
            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                const string DATA_SELECTED = "*";
                const string TABLENAME = "HOURLYFILES.HUAWEISUBSETS";
                string CONDITION = "ID='" + subsetID + "'";

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT " + DATA_SELECTED + " FROM " + TABLENAME + " WHERE " + CONDITION;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CloseConnection();
                            return true;
                        }
                    }
                }

                CloseConnection();
            }
            
            return false;
        }

        public static string GetSubsetTableName(string subsetID)
        {
            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                const string DATA_SELECTED = "*";
                const string TABLENAME = "HOURLYFILES.HUAWEISUBSETS";
                string CONDITION = "ID='" + subsetID + "'";

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT " + DATA_SELECTED + " FROM " + TABLENAME + " WHERE " + CONDITION;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string Name = reader.IsDBNull(reader.GetOrdinal("NAME")) ? null : reader.GetString(reader.GetOrdinal("NAME"));
                            CloseConnection();
                            return Name;
                        }
                    }
                }

                CloseConnection();
            }

            return null;
        }

        public static void InsertDataInHistoryFileLog(int userID, char successed, string note)
        {
            const string TABLENAME = "HOURLYFILES.NPM_FILES_LOGS";
            const string TABLECOLUMNS = " (USERID, SUCCESSED, NOTE) ";
            string TABLEVALUES = "(" + userID + ",'" + successed + "','" + note + "')";

            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO " + TABLENAME + TABLECOLUMNS + " VALUES " + TABLEVALUES;
                    cmd.ExecuteReader();
                }

                CloseConnection();
            }
        }

        public static void InsertDataInHistoryFileLog(int userID, int actionID, char successed, string note)
        {
            const string TABLENAME = "HOURLYFILES.NPM_FILES_LOGS";
            const string TABLECOLUMNS = " (USERID, ACTIONID, SUCCESSED, NOTE) ";
            string TABLEVALUES = "(" + userID + "," + actionID + ",'" + successed + "','" + note + "')";

            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO " + TABLENAME + TABLECOLUMNS + " VALUES " + TABLEVALUES;
                    cmd.ExecuteReader();
                }

                CloseConnection();
            }
        }

        public static void InsertDataInHistoryFileLog(int userID, int actionID, int fileID, char successed, string note)
        {
            const string TABLENAME = "HOURLYFILES.NPM_FILES_LOGS";
            const string TABLECOLUMNS = " (USERID, ACTIONID, FILEID, SUCCESSED, NOTE) ";
            string TABLEVALUES = "(" + userID + "," + actionID + "," + fileID + ",'" + successed + "','" + note + "')";

            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO " + TABLENAME + TABLECOLUMNS + " VALUES " + TABLEVALUES;
                    cmd.ExecuteReader();
                }

                CloseConnection();
            }
        }
    }
}
