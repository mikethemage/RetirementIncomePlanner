using System.Collections.Generic;

namespace RetirementIncomePlannerLibrary
{
    public class Client
    {
        public AgeValue CurrentAge { get; set; } = new AgeValue();
        public AmountValue RetirementIncomeLevel { get; set; } = new AmountValue();
        public List<Employment> EmploymentList { get; set; } = new List<Employment>();
        public List<Pension> PensionList { get; set; } = new List<Pension>();
        public List<OtherIncome> OtherIncomeList { get; set; } = new List<OtherIncome>();
        public List<RetirementPot> IndividualPotList { get; set; } = new List<RetirementPot>();
        public List<AdhocContribution> AdhocContributionList { get; set; } = new List<AdhocContribution>();
    }
    
}
