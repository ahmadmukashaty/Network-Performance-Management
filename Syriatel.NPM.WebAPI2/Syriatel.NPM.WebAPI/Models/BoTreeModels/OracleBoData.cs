using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Syriatel.NPM.WebAPI.Models.OracleConnections;

namespace Syriatel.NPM.BoManager.OracleConnections
{
    public class OracleBoData
    {
        public List<CounterDescriptor> Counters = null;
        public List<SubsetDescriptor> Subsets = null;

        public void GetSubsetsCountersData(string universe)
        {
            Counters = OracleHelper.GetCountersFromDB(universe);
            Subsets = OracleHelper.GetSubsetsFromDB(universe);

            AssignSubsetsToCounters();
        }
         
        private void AssignSubsetsToCounters()
        {
            foreach(CounterDescriptor counter in Counters)
            {
                SubsetDescriptor subset = null;
                if (counter.Subset.SubsetID != null)
                {
                    string subsetID = counter.Subset.SubsetID;
                    subset = GetSubsetByID(subsetID);
                }
                counter.Subset = subset;
            }
        }

        private SubsetDescriptor GetSubsetByID(string subsetID)
        {
            foreach(SubsetDescriptor subset in Subsets)
            {
                if(subset.SubsetID != null)
                {
                    if (subset.SubsetID == subsetID)
                        return subset;
                }
            }
            return null;
        }
       
    }
}
