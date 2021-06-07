using Oracle.ManagedDataAccess.Client;
using Syriatel.NPM.BoManager;
using Syriatel.NPM.WebAPI.Models.ActionsModels;
using Syriatel.NPM.WebAPI.Models.BoModels;
using Syriatel.NPM.WebAPI.Models.BoTreeModels;
using Syriatel.NPM.WebAPI.Models.Helper;
using Syriatel.NPM.WebAPI.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Syriatel.NPM.WebAPI.Models.OracleConnections
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

        public static void AddFileIntoDB(string data, string file_type, int userID, string file_name)
        {
            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                //const string TABLENAME = "HOURLYFILES.NPM_FILES";
                //string VALUES = "'" + file_name + "','" + data + "','" + file_type + "' ," + userID;

                //using (var cmd = oraConnection.CreateCommand())
                //{
                //    cmd.CommandText = "INSERT INTO " + TABLENAME + " (NAME,FILE_DATA,FILE_TYPE,USERID) VALUES (" + VALUES + ")";
                //    cmd.ExecuteReader();
                //}

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.InitialLONGFetchSize = 1000;
                    cmd.CommandText = "INSERTCLOB";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("file_name", OracleDbType.Varchar2).Value = file_name;
                    cmd.Parameters.Add("file_type", OracleDbType.Varchar2).Value = file_type;
                    cmd.Parameters.Add("userid", OracleDbType.Int64).Value = userID;
                    cmd.Parameters.Add("l_clob", OracleDbType.Clob).Value = data;
                    cmd.ExecuteNonQuery();

                }


                CloseConnection();
            }
        }

        public static int? GetFileID(string file_name)
        {
            int? fileID = null;
            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                const string DATA_SELECTED = "ID";
                const string TABLENAME = "HOURLYFILES.NPM_FILES";
                string CONDITION = "NAME = '" + file_name + "'";

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT " + DATA_SELECTED + " FROM " + TABLENAME + " WHERE " + CONDITION;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fileID = reader.IsDBNull(reader.GetOrdinal("ID")) ? null : (int?)reader.GetInt64(reader.GetOrdinal("ID"));
                        }
                    }
                }

                CloseConnection();

                return fileID;
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

        public static void DeactivateCounter(CounterDescriptor counter)
        {
            const string TABLENAME = "HOURLYFILES.HUAWEICOUNTERS";
            const string VALUES = "SHOW = 'N' ";
            string CONDITION = "FILECOUNTERNAME='" + counter.value + "' AND UNV=" + "'" + counter.Subset.Universe + "' AND ID=" + "'" + counter.Subset.SubsetID + "'";

            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "UPDATE " + TABLENAME + " SET " + VALUES + " WHERE " + CONDITION;
                    cmd.ExecuteReader();
                }

                CloseConnection();
            }
        }

        public static void ChangeCounterPath(ChangePathModelView counterInfo)
        {
            const string TABLENAME = "HOURLYFILES.HUAWEICOUNTERS";
            string VALUES = "PATH = '" + counterInfo.newPath + "' ";
            string CONDITION = "FILECOUNTERNAME='" + counterInfo.counter.value + "' AND UNV=" + "'" + counterInfo.counter.Subset.Universe + "' AND ID=" + "'" + counterInfo.counter.Subset.SubsetID + "'";

            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "UPDATE " + TABLENAME + " SET " + VALUES + " WHERE " + CONDITION;
                    cmd.ExecuteReader();
                }

                CloseConnection();
            }
        }

        public static Boolean DBHasSubset(string subsetID)
        {
            const string DATA_SELECTED = "*";
            const string TABLENAME = "HOURLYFILES.HUAWEISUBSETS";
            string CONDITION = "ID='" + subsetID + "'";

            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

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

                    CloseConnection();
                    return false;
                }
            }
        }

        public static void AddSubsetProcedure(string functionName, string schemaName, string universe, string subsetID, string CsV_Dims_Names, string CsV_Dims_Pathes, int userID, int fileID)
        {
            using (oraConnection = new OracleConnection(schemaName))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {

                    cmd.InitialLONGFetchSize = 1000;
                    cmd.CommandText = functionName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("universe", OracleDbType.Varchar2).Value = universe;
                    cmd.Parameters.Add("subsetID", OracleDbType.Varchar2).Value = subsetID;
                    cmd.Parameters.Add("ID_prefix", OracleDbType.Varchar2).Value = "X";
                    cmd.Parameters.Add("ReadMethod", OracleDbType.Varchar2).Value = "X";
                    cmd.Parameters.Add("CsV_Dims_Names", OracleDbType.Varchar2).Value = CsV_Dims_Names;
                    cmd.Parameters.Add("CsV_Dims_Pathes", OracleDbType.Varchar2).Value = CsV_Dims_Pathes;
                    cmd.Parameters.Add("USERID", OracleDbType.Int64).Value = userID;
                    cmd.Parameters.Add("FILEID", OracleDbType.Varchar2).Value = fileID.ToString();
                    cmd.ExecuteNonQuery();
                }

                CloseConnection();
            }
        }

        public static void AddMeasurementProcedure(CounterModelView cmv, string functionName, string schemaName, string subsetID, int userID, int fileID)
        {
            using (oraConnection = new OracleConnection(schemaName))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.InitialLONGFetchSize = 1000;
                    cmd.CommandText = functionName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("subsetID", OracleDbType.Varchar2).Value = subsetID;
                    cmd.Parameters.Add("CounterID", OracleDbType.Varchar2).Value = cmv.counterID;
                    cmd.Parameters.Add("subsetTableName", OracleDbType.Varchar2).Value = "M" + subsetID;
                    cmd.Parameters.Add("CounterName", OracleDbType.Varchar2).Value = cmv.counterName;
                    cmd.Parameters.Add("CounterSortCut", OracleDbType.Varchar2).Value = cmv.shortName;
                    cmd.Parameters.Add("CounterPath", OracleDbType.Varchar2).Value = cmv.path;
                    cmd.Parameters.Add("USERID", OracleDbType.Int64).Value = userID;
                    cmd.Parameters.Add("FILEID", OracleDbType.Varchar2).Value = fileID.ToString();
                    cmd.ExecuteNonQuery();
                }

                CloseConnection();
            }
        }

        public static void AddDimentionProcedure(DimantionModelView dmv, string functionName, string schemaName, string subsetID, int userID, int fileID)
        {
            using (oraConnection = new OracleConnection(schemaName))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.InitialLONGFetchSize = 1000;
                    cmd.CommandText = functionName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("subsetID", OracleDbType.Varchar2).Value = subsetID;
                    cmd.Parameters.Add("CounterSortCut", OracleDbType.Varchar2).Value = dmv.name.Split(':')[1];
                    cmd.Parameters.Add("DataType", OracleDbType.Varchar2).Value = "Varchar(200)";
                    cmd.Parameters.Add("CounterSortCut_path", OracleDbType.Varchar2).Value = dmv.path;
                    cmd.Parameters.Add("dim_str", OracleDbType.Varchar2).Value = dmv.name;
                    cmd.Parameters.Add("USERID", OracleDbType.Int64).Value = userID;
                    cmd.Parameters.Add("FILEID", OracleDbType.Varchar2).Value = fileID.ToString();
                    cmd.ExecuteNonQuery();

                }

                CloseConnection();
            }
        }

        public static List<CounterDescriptor> GetCountersFromDB(string universe)
        {
            List<CounterDescriptor> Counters = new List<CounterDescriptor>();

            const string DATA_SELECTED = "*";
            const string TABLENAME = "HOURLYFILES.HUAWEICOUNTERS";
            string CONDITION = "SHOW = 'Y' AND UNV = '" + universe + "'";

            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT " + DATA_SELECTED + " FROM " + TABLENAME + " WHERE " + CONDITION;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CounterDescriptor counterDesc = new CounterDescriptor();
                            counterDesc.counterID = reader.IsDBNull(reader.GetOrdinal("COUNTERID")) ? null : (int?)reader.GetInt64(reader.GetOrdinal("COUNTERID"));
                            counterDesc.value = reader.IsDBNull(reader.GetOrdinal("FILECOUNTERNAME")) ? null : reader.GetString(reader.GetOrdinal("FILECOUNTERNAME"));
                            counterDesc.tableCounterName = reader.IsDBNull(reader.GetOrdinal("TABLECOUNTERNAME")) ? null : reader.GetString(reader.GetOrdinal("TABLECOUNTERNAME"));
                            counterDesc.tableName = reader.IsDBNull(reader.GetOrdinal("TABLENAME")) ? null : reader.GetString(reader.GetOrdinal("TABLENAME"));
                            counterDesc.valueType = reader.IsDBNull(reader.GetOrdinal("VALUETYPE")) ? ObjectType_Enum.None : API_Helper.GetObjectType(reader.GetString(reader.GetOrdinal("VALUETYPE")));
                            counterDesc.path = reader.IsDBNull(reader.GetOrdinal("PATH")) ? null : API_Helper.InvertPath(reader.GetString(reader.GetOrdinal("PATH")));
                            counterDesc.reference = reader.IsDBNull(reader.GetOrdinal("REFRENCE")) ? "" : reader.GetString(reader.GetOrdinal("REFRENCE"));
                            counterDesc.functionType = reader.IsDBNull(reader.GetOrdinal("FUNCTION_VALUE")) ? ObjectFunction_Enum.None : API_Helper.GetFunctionType(reader.GetString(reader.GetOrdinal("FUNCTION_VALUE")));

                            SubsetDescriptor subsetDesc = new SubsetDescriptor();
                            subsetDesc.SubsetID = reader.IsDBNull(reader.GetOrdinal("ID")) ? null : reader.GetString(reader.GetOrdinal("ID"));

                            counterDesc.Subset = subsetDesc;
                            Counters.Add(counterDesc);
                        }
                    }
                }

                CloseConnection();
            }

            return Counters;
        }

        public static List<SubsetDescriptor> GetSubsetsFromDB(string universe)
        {
            List<SubsetDescriptor> Subsets = new List<SubsetDescriptor>();

            const string DATA_SELECTED = "*";
            const string TABLENAME = "HOURLYFILES.HUAWEISUBSETS";
            string CONDITION = "UNV = '" + universe + "'";

            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT " + DATA_SELECTED + " FROM " + TABLENAME + " WHERE " + CONDITION;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SubsetDescriptor subsetDesc = new SubsetDescriptor();

                            subsetDesc.SubsetID = reader.IsDBNull(reader.GetOrdinal("ID")) ? null : reader.GetString(reader.GetOrdinal("ID"));
                            subsetDesc.SubsetName = reader.IsDBNull(reader.GetOrdinal("NAME")) ? null : reader.GetString(reader.GetOrdinal("NAME"));
                            subsetDesc.Universe = reader.IsDBNull(reader.GetOrdinal("UNV")) ? null : reader.GetString(reader.GetOrdinal("UNV"));
                            subsetDesc.IsActive = reader.IsDBNull(reader.GetOrdinal("ACTIVE")) ? SubsetActivation_Enum.INACTIVE : API_Helper.GetSubsetActivation(reader.GetString(reader.GetOrdinal("ACTIVE")));

                            Subsets.Add(subsetDesc);
                        }
                    }
                }

                CloseConnection();
            }

            return Subsets;
        }

        public static List<SubsetCounterModelView> GetDashboardData(string universe)
        {
            List<SubsetCounterModelView> subsetCounterModelView = new List<SubsetCounterModelView>();
            const string DATA_SELECTED = "p.RELEASE,p.UNV,p.SUBSETID,p.COUNTERID,p.COUNTERNAME,p.SHORTNAME,hs.ACTIVE,hcs.SHOW,hcs.PATH,hcs.REFRENCE,hcs.FUNCTION_VALUE";
            const string TABLENAME = "HOURLYFILES.PERFORMANCE_DATA p left outer join   HOURLYFILES.HUAWEICOUNTERS hcs on p.COUNTERID = hcs.COUNTERID left outer join   HOURLYFILES.HUAWEISUBSETS hs on p.SUBSETID = hs.ID";
            string CONDITION = "P.UNV = '" + universe + "'";

            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT " + DATA_SELECTED + " FROM " + TABLENAME + " WHERE " + CONDITION;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SubsetCounterModelView scmv = new SubsetCounterModelView();

                            scmv.release = reader.IsDBNull(reader.GetOrdinal("RELEASE")) ? null : reader.GetString(reader.GetOrdinal("RELEASE"));
                            scmv.unv = reader.IsDBNull(reader.GetOrdinal("UNV")) ? null : reader.GetString(reader.GetOrdinal("UNV"));
                            scmv.subsetID = reader.IsDBNull(reader.GetOrdinal("SUBSETID")) ? null : reader.GetString(reader.GetOrdinal("SUBSETID"));
                            scmv.counterID = reader.IsDBNull(reader.GetOrdinal("COUNTERID")) ? null : reader.GetString(reader.GetOrdinal("COUNTERID"));
                            scmv.counterName = reader.IsDBNull(reader.GetOrdinal("COUNTERNAME")) ? null : reader.GetString(reader.GetOrdinal("COUNTERNAME"));
                            scmv.shortName = reader.IsDBNull(reader.GetOrdinal("SHORTNAME")) ? null : reader.GetString(reader.GetOrdinal("SHORTNAME"));
                            scmv.active = reader.IsDBNull(reader.GetOrdinal("ACTIVE")) ? null : reader.GetString(reader.GetOrdinal("ACTIVE"));
                            scmv.path = reader.IsDBNull(reader.GetOrdinal("PATH")) ? null : API_Helper.InvertPath(reader.GetString(reader.GetOrdinal("PATH")));
                            scmv.reference = reader.IsDBNull(reader.GetOrdinal("REFRENCE")) ? null : reader.GetString(reader.GetOrdinal("REFRENCE"));
                            scmv.function_value = reader.IsDBNull(reader.GetOrdinal("FUNCTION_VALUE")) ? null : reader.GetString(reader.GetOrdinal("FUNCTION_VALUE"));
                            scmv.show = reader.IsDBNull(reader.GetOrdinal("SHOW")) ? null : reader.GetString(reader.GetOrdinal("SHOW"));

                            subsetCounterModelView.Add(scmv);
                        }
                    }
                }

                CloseConnection();
            }

            return subsetCounterModelView;  
        }

        public static List<DimantionModelView> GetSubsetDimentions(string universe)
        {
            List<DimantionModelView> Dimentions = new List<DimantionModelView>();

            const string DATA_SELECTED = "*";
            const string TABLENAME = "HOURLYFILES.HUAWEICOUNTERS";
            string CONDITION = "UNV = '" + universe + "' AND VALUETYPE = 'D'" ;

            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT " + DATA_SELECTED + " FROM " + TABLENAME + " WHERE " + CONDITION;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DimantionModelView dmv = new DimantionModelView();

                            dmv.subsetID = reader.IsDBNull(reader.GetOrdinal("ID")) ? null : reader.GetString(reader.GetOrdinal("ID"));
                            dmv.name = reader.IsDBNull(reader.GetOrdinal("FILECOUNTERNAME")) ? null : reader.GetString(reader.GetOrdinal("FILECOUNTERNAME"));
                            dmv.path = reader.IsDBNull(reader.GetOrdinal("PATH")) ? null : reader.GetString(reader.GetOrdinal("PATH"));

                            Dimentions.Add(dmv);
                        }
                    }
                }

                CloseConnection();
            }

            return Dimentions;
        }

        public static List<ActionModelView> GetUserActions(int userID)
        {
            List<ActionModelView> userActions = new List<ActionModelView>();

            const string DATA_SELECTED = "ac.NAME, hl.PARAMETER1, hl.PARAMETER2, hl.PARAMETER3, fl.NAME FILE_NAME, TO_CHAR(hl.CREATED_DATE,'MM/dd/yyyy hh:mm') CREATED_DATE ,hl.SUCCESSED";
            const string TABLENAME = " HOURLYFILES.NPM_HISTORY_LOGS  hl join   HOURLYFILES.NPM_ACTIONS ac on hl.ACTIONID = ac.ID  join HOURLYFILES.NPM_FILES fl on hl.PARAMETER3 = fl.ID";
            string CONDITION = " hl.USERID = " + userID + " AND ACTIONID IN (3,4,5,6,41)";
            const string ORDER = " ORDER BY hl.CREATED_DATE DESC";

            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

                using (var cmd = oraConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT " + DATA_SELECTED + " FROM " + TABLENAME + " WHERE " + CONDITION + ORDER;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ActionModelView action = new ActionModelView();

                            action.ActionName = reader.IsDBNull(reader.GetOrdinal("NAME")) ? null : reader.GetString(reader.GetOrdinal("NAME"));
                            action.SubsetId = reader.IsDBNull(reader.GetOrdinal("PARAMETER1")) ? null : reader.GetString(reader.GetOrdinal("PARAMETER1"));
                            action.CounterId = reader.IsDBNull(reader.GetOrdinal("PARAMETER2")) ? null : reader.GetString(reader.GetOrdinal("PARAMETER2"));
                            action.FileId = reader.IsDBNull(reader.GetOrdinal("PARAMETER3")) ? null : reader.GetString(reader.GetOrdinal("PARAMETER3"));
                            action.FileName = reader.IsDBNull(reader.GetOrdinal("FILE_NAME")) ? null : reader.GetString(reader.GetOrdinal("FILE_NAME"));
                            action.Date = reader.IsDBNull(reader.GetOrdinal("CREATED_DATE")) ? null : (reader.GetString(reader.GetOrdinal("CREATED_DATE")).ToString());
                            action.SuccessInDB = reader.IsDBNull(reader.GetOrdinal("SUCCESSED")) ? null : reader.GetString(reader.GetOrdinal("SUCCESSED"));

                            userActions.Add(action);
                        }
                    }
                }

                CloseConnection();
            }

            return userActions;  
        }

        public static bool CheckFileBoStatus(string fileID)
        {
            const string DATA_SELECTED = "*";
            const string TABLENAME = "HOURLYFILES.NPM_FILES_LOGS";
            string CONDITION = "FILEID='" + fileID + "' AND ACTIONID = 9 AND SUCCESSED = 'Y' ";

            using (oraConnection = new OracleConnection(Constants.CONNECTION_HOURLYFILES_INFO))
            {
                OpenConnection();

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

                    CloseConnection();
                    return false;
                }
            }
        }


        public static void InsertDataInHistoryLog(int userID, char successed, string note)
        {
            const string TABLENAME = "HOURLYFILES.NPM_HISTORY_LOGS";
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

        public static void InsertDataInHistoryLog(int userID, int actionID, char successed, string note)
        {
            const string TABLENAME = "HOURLYFILES.NPM_HISTORY_LOGS";
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

        public static void InsertDataInHistoryLog(int userID, int actionID, string param1, string param2, string param3, char successed, string note)
        {
            const string TABLENAME = "HOURLYFILES.NPM_HISTORY_LOGS";
            const string TABLECOLUMNS = " (USERID, ACTIONID, PARAMETER1, PARAMETER2, PARAMETER3, SUCCESSED, NOTE) ";
            string TABLEVALUES = "(" + userID + "," + actionID + ",'" + param1 + "','" + param2 + "','" + param3 + "','" + successed + "','" + note + "')";

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