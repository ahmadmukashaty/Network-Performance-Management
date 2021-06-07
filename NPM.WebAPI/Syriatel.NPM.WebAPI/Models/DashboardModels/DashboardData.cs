using Oracle.ManagedDataAccess.Client;
using Syriatel.NPM.WebAPI.Models.OracleConnections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Syriatel.NPM.WebAPI.Models.BoModels
{
    public class DashboardData
    {
        public List<SubsetModelView> subsetModelView = new List<SubsetModelView>();
        private List<SubsetCounterModelView> subsetCounterModelView = null;
        private List<DimantionModelView> dimentionModelView = null;

        public DashboardData(string universe)
        {
            this.subsetCounterModelView = OracleHelper.GetDashboardData(universe);
            this.dimentionModelView = OracleHelper.GetSubsetDimentions(universe);
            SetAllSubsets();
        }

        private void SetAllSubsets()
        {
            foreach(SubsetCounterModelView scmv in subsetCounterModelView)
            {
                int index = GetSubsetIndex(scmv.subsetID);
                if(index == -1)
                {
                    SubsetModelView smv = new SubsetModelView();
                    smv.subsetID = scmv.subsetID;
                    smv.release = scmv.release;
                    smv.unv = scmv.unv;
                    smv.active = scmv.active;

                    CounterModelView cmv = new CounterModelView();
                    cmv.counterID = scmv.counterID;
                    cmv.counterName = scmv.counterName;
                    cmv.function_value = scmv.function_value;
                    cmv.path = scmv.path;
                    cmv.reference = scmv.reference;
                    cmv.shortName = scmv.shortName;
                    cmv.show = scmv.show;


                    smv.measurements.Add(cmv);

                    subsetModelView.Add(smv);
                }
                else
                {
                    CounterModelView cmv = new CounterModelView();
                    cmv.counterID = scmv.counterID;
                    cmv.counterName = scmv.counterName;
                    cmv.function_value = scmv.function_value;
                    cmv.path = scmv.path;
                    cmv.reference = scmv.reference;
                    cmv.shortName = scmv.shortName;
                    cmv.show = scmv.show;

                    subsetModelView[index].measurements.Add(cmv);
                }
            }

            foreach(DimantionModelView dmv in dimentionModelView)
            {
                if(dmv.subsetID != null)
                {
                    int index = GetSubsetIndex(dmv.subsetID);
                    if(index != -1)
                    {
                        subsetModelView[index].dimentions.Add(dmv);
                    }
                }
            }
            
        }

        private int GetSubsetIndex(string subsetID)
        {
            for (int i = 0; i < subsetModelView.Count; i++)
            {
                if (subsetModelView[i].subsetID.Equals(subsetID))
                    return i;
            }
            return -1;
        }


        
    }
}