using System;
using System.Collections.Generic;

namespace RetirementIncomePlannerLibrary
{

    public class IncomePlannerModel
    {
        public AgeValue NumberOfYears { get; set; } = new AgeValue();
        public int NumberOfClients { get; set; }
        public List<Client> ClientList { get; set; } = new List<Client>();
        public PercentageValue Indexation { get; set; } = new PercentageValue();
        public PotMethodEnum PotMethod { get; set; } = PotMethodEnum.Combined;
        public List<RetirementPot> CombinedPotList { get; set; } = new List<RetirementPot>();
        public int GenerateGraphData()
        {
            return 0;
        }
    }
    
}
