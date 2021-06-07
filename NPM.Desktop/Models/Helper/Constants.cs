using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syriatel.NPM.Desktop.Models.Helper
{
    public static class Constants
    {
        public static class Messages
        {
            public const string UNAUTHORIZED_USER_ERROR = "This user is unauthrized.";
            public const string FAILED_ACTION_ERROR = "Failed to catch action type.";
            public const string FAILED_SAVE_FILE_ERROR = "Failed to save file information for this action.";
            public const string DESIGNER_ALREADY_OPENED_ERROR = "Designer application is already opened, do you want to close it ?";
            public const string FAILED_DEACTIVATE_COUNTER_ERROR = "Failed to deactivate counter.";
            public const string FAILED_ADD_SUBSET_ERROR = "Failed to add subset.";
            public const string UNEXPECTED_ERROR = "Failed to complete this action.";
            public const string FILE_TYPE_ERROR = "Failed to get file type.";
            public const string FILE_NOT_FOUND_ERROR = "File not exist.";
            public const string NO_FILE_NAME_ERROR = "Please enter the name of file you want to import to Business Objects.";
            public const string EMPTY_MESSAGE = "";
            public const string SUCCESSFUL_MESSAGE = "File information added successfully.";
        }


        public const string ADDSUBSET_TYPE = "ADDSUBSET";
        public const string DEACTIVATE_COUNTER_TYPE = "DEACTIVATECOUNTER";

        public const string INSERT_FILE_TO_BO_ACTION = "INSERT_FILE_TO_BO";

        public const bool SUCCESSED = true;
        public const bool FAILED = false;

        public const char NO = 'N';
        public const char YES = 'Y';

        public const string EMPTY_STRING = "";

        public const string H6900 = "H69";
        public const string NSS = "NSS";
        public const string GSN = "GSN";

        public const string CONNECTION_HOURLYFILES_INFO = @"Data Source=(DESCRIPTION= (ADDRESS= (PROTOCOL=TCP) (HOST=10.253.23.104) (PORT=1521)) (CONNECT_DATA= (SERVICE_NAME=Counters)));User Id=HOURLYFILES;Password=tis";
        public const string DESIGNER_APPLICATION_NAME = "designer";
        public const string BUSINESS_OBJECTS = "Business Objects";
    }
}
