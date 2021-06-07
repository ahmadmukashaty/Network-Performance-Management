using FTPConnection;
using Oracle.ManagedDataAccess.Client;
using Syriatel.NPM.WebAPI.Models.Helper;
using Syriatel.NPM.WebAPI.Models.OracleConnections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace Syriatel.NPM.WebAPI.Models.BoModels
{
    public class SubsetModelView
    {
        public string release { get; set; }
        public string unv { get; set; }
        public string subsetID { get; set; }
        public string active { get; set; }
        public List<CounterModelView> measurements { get; set; }
        public List<DimantionModelView> dimentions { get; set; }

        public SubsetModelView()
        {
            measurements = new List<CounterModelView>();
            dimentions = new List<DimantionModelView>();
        }

        public void AddSubset(bool subsetExist, int userID, int fileID)
        {
            if (subsetExist)
            {
                if (dimentions != null && dimentions.Count != 0)
                {
                    foreach (DimantionModelView dmv in dimentions)
                        CallingDimentionProcedure(dmv, userID, fileID);
                }
            }
            else
            {
                CallingSubsetProcedure(userID, fileID);
            }
            if (measurements != null && measurements.Count != 0)
            {
                foreach (CounterModelView cmv in measurements)
                    CallingMeasurementProcedure(cmv, userID, fileID);
            }
        }

        private void CallingSubsetProcedure(int userID, int fileID)
        {
            string functionName = GetUniverseTableName() + "." + Constants.ADD_SUBSET_PROCEDURE;
            string schemaName = GetSchemaName();
            string CsV_Dims_Names = "";
            string CsV_Dims_Pathes = "";

            if (dimentions != null && dimentions.Count != 0)
            {
                CsV_Dims_Names = FormatDimentionsNames();
                CsV_Dims_Pathes = FormatDimentionsPathes();
            }

            OracleHelper.AddSubsetProcedure(functionName, schemaName, this.unv, this.subsetID, CsV_Dims_Names, CsV_Dims_Pathes, userID, fileID);
        }

        private void CallingMeasurementProcedure(CounterModelView cmv, int userID, int fileID)
        {
            string functionName = GetUniverseTableName() + "." + Constants.ADD_MEASUREMENT_PROCEDURE;
            string schemaName = GetSchemaName();
            OracleHelper.AddMeasurementProcedure(cmv, functionName, schemaName, this.subsetID, userID, fileID);
        }

        private void CallingDimentionProcedure(DimantionModelView dmv, int userID, int fileID)
        {
            string functionName = GetUniverseTableName() + "." + Constants.ADD_DIMENTION_PROCEDURE;
            string schemaName = GetSchemaName();
            OracleHelper.AddDimentionProcedure(dmv, functionName, schemaName, this.subsetID, userID, fileID);
        }

        private string FormatDimentionsNames()
        {
            string dims = "";
            for (int i = 0; i < dimentions.Count; i++)
            {
                dims += ((i == 0) ? dimentions[i].name : "," + dimentions[i].name);
            }
            return dims;
        }

        private string FormatDimentionsPathes()
        {
            string dims = "";
            for (int i = 0; i < dimentions.Count; i++)
            {
                dims += ((i == 0) ? dimentions[i].path : "," + dimentions[i].path);
            }
            return dims;
        }

        private string GetUniverseTableName()
        {
            string universeName = null;
            
            if (this.unv.ToUpper() == Constants.H6900)
            {
                universeName = Constants.H6900_TABLENAME;
            }
            else if (this.unv.ToUpper() == Constants.GSN)
            {
                universeName = Constants.GSN_TABLENAME;
            }
            else if (this.unv.ToUpper() == Constants.NSS)
            {
                universeName = Constants.NSS_TABLENAME;
            }

            return universeName;
        }

        private string GetSchemaName()
        {
            string schemaName = null;

            if (this.unv.ToUpper() == Constants.H6900)
            {
                schemaName = Constants.CONNECTION_H6900_INFO;
            }
            else if (this.unv.ToUpper() == Constants.GSN)
            {
                schemaName = Constants.CONNECTION_HUAWEIGSN_INFO;
            }
            else if (this.unv.ToUpper() == Constants.NSS)
            {
                schemaName = Constants.CONNECTION_HUAWEINSS_INFO;
            }

            return schemaName;
        }
    }
}