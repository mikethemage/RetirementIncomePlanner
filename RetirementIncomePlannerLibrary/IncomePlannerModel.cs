using System;
using System.Collections.Generic;

namespace RetirementIncomePlannerLibrary
{

    public class IncomePlannerModel
    {
        public AgeValue NumberOfYears;
        public int NumberOfClients;
        public List<int> ClientList;
        public PercentageValue Indexation;
        public PotMethodEnum PotMethod = PotMethodEnum.Combined;
        public List<int> CombinedPotList;
        public int GenerateGraphData()
        {
            return 0;
        }
    }
    
}
