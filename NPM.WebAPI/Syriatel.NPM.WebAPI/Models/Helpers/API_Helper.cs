using Syriatel.NPM.BoManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syriatel.NPM.WebAPI.Models.Helpers
{
    public class API_Helper
    {
        public static string InvertPath(string path)
        {
            string[] words = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            string newPath = "";
            for (int i = words.Length - 1; i >= 0; i--)
            {
                newPath += (i == words.Length - 1) ? (words[i]) : ("/" + words[i]);
            }

            return newPath;
        }

        public static ObjectType_Enum GetObjectType(string objType)
        {
            if (objType.ToUpper() == "D")
                return ObjectType_Enum.DIMENSION;
            if (objType.ToUpper() == "M")
                return ObjectType_Enum.MEASUREMENT;
            return ObjectType_Enum.None;
        }

        public static ObjectFunction_Enum GetFunctionType(string objType)
        {
            if (objType.ToUpper() == "SUM")
                return ObjectFunction_Enum.Sum;
            if (objType.ToUpper() == "MAX")
                return ObjectFunction_Enum.Max;
            if (objType.ToUpper() == "MIN")
                return ObjectFunction_Enum.Min;
            if (objType.ToUpper() == "AVERAGE")
                return ObjectFunction_Enum.Average;
            return ObjectFunction_Enum.None;
        }

        public static SubsetActivation_Enum GetSubsetActivation(string activationType)
        {
            if (activationType.ToUpper() == "Y")
                return SubsetActivation_Enum.ACTIVE;
            return SubsetActivation_Enum.INACTIVE;
        }

    }
}