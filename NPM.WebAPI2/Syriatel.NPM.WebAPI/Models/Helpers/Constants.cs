using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syriatel.NPM.WebAPI.Models.Helper
{
    public static class Constants
    {
        public static class Messages
        {
            public const string UNAUTHORIZED_USER_ERROR = "This user is unauthrized";
            public const string FAILED_ACTION_ERROR = "Failed to catch action type";
            public const string FAILED_SAVE_FILE_ERROR = "Failed to save file information for this action";
            public const string UNEXPECTED_ERROR = "Failed to complete this action";
            public const string UNACTIVATE_SUBSET_ERROR = "UnactivatedSubset";
            public const string EMPTY_MESSAGE = "";
        }

        public const string ADDSUBSET_TYPE = "ADDSUBSET";
        public const string DEACTIVATE_COUNTER_TYPE = "DEACTIVATECOUNTER";
        public const string CHANGE_COUNTER_PATH_TYPE = "CHNAGECOUNTERPATH";

        public const string DEACTIVATE_COUNTER_ACTION = "DEACTIVATE_COUNTER_IN_DB";
        public const string ADD_DIMENTION_ACTION = "ADD_DIMENTIONS_TO_DB";
        public const string ADD_SUBSET_ACTION = "ADD_SUBSET_TO_DB";
        public const string ADD_COUNTER_ACTION = "ADD_COUNTER_TO_DB";
        public const string UPLOAD_FILE_ACTION = "UPLOAD_FILE_TO_DB";
        public const string LOGIN_ACTION = "LOGIN_USER";
        public const string GET_DATA_ACTION = "GET_DATA_ACTION";
        public const string CHANGE_COUNTER_PATH_ACTION = "CHANGE_COUNTER_PATH_IN_DB";

        public const int SUCCESSED = 1;
        public const int FAILED = -1;

        public const char NO = 'N';
        public const char YES = 'Y';

        public const string DIMENTION_DIRECTORY_PATH = "MISDBA/Pulled";
        public const string EMPTY_STRING = "";

        public const string H6900 = "H69";
        public const string NSS = "NSS";
        public const string GSN = "GSN";
        public const string H6900_TABLENAME = "H6900";
        public const string NSS_TABLENAME = "HUAWEINSS";
        public const string GSN_TABLENAME = "HUAWEIGSN";
        public const string ADD_SUBSET_PROCEDURE = "Add_Subset_tree";
        public const string ADD_MEASUREMENT_PROCEDURE = "Add_Counter_tree";
        public const string ADD_DIMENTION_PROCEDURE = "Add_Dimension_tree";

        public const string CONNECTION_HOURLYFILES_INFO = @"Data Source=(DESCRIPTION= (ADDRESS= (PROTOCOL=TCP) (HOST=10.253.23.104) (PORT=1521)) (CONNECT_DATA= (SERVICE_NAME=Counters)));User Id=HOURLYFILES;Password=tis";
        public const string CONNECTION_H6900_INFO = @"Data Source=(DESCRIPTION= (ADDRESS= (PROTOCOL=TCP) (HOST=10.253.23.104) (PORT=1521)) (CONNECT_DATA= (SERVICE_NAME=Counters)));User Id=H6900;Password=tis";
        public const string CONNECTION_HUAWEINSS_INFO = @"Data Source=(DESCRIPTION= (ADDRESS= (PROTOCOL=TCP) (HOST=10.253.23.104) (PORT=1521)) (CONNECT_DATA= (SERVICE_NAME=Counters)));User Id=HUAWEINSS;Password=tis";
        public const string CONNECTION_HUAWEIGSN_INFO = @"Data Source=(DESCRIPTION= (ADDRESS= (PROTOCOL=TCP) (HOST=10.253.23.104) (PORT=1521)) (CONNECT_DATA= (SERVICE_NAME=Counters)));User Id=HUAWEIGSN;Password=tis";
    }
}