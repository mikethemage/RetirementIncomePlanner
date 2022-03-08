using System.Collections.Generic;

namespace RetirementIncomePlannerLibrary
{
    public class Client
    {
        public const int DefaultAge = 60;
        public string ClientName { get; set; } = string.Empty;
        public AgeValue CurrentAge { get; set; } = new AgeValue();        
        public AmountValue RetirementIncomeLevel { get; set; } = new AmountValue();
        public List<Employment> EmploymentList { get; set; } = new List<Employment>();
        public List<Pension> PensionList { get; set; } = new List<Pension>();
        public List<OtherIncome> OtherIncomeList { get; set; } = new List<OtherIncome>();
        public List<RetirementPot> IndividualPotList { get; set; } = new List<RetirementPot>();
        public List<AdhocContribution> AdhocContributionList { get; set; } = new List<AdhocContribution>();

        public Client(string clientName, PotMethodEnum potMethod = PotMethodEnum.Combined)
        {
            CurrentAge.ItemValue = DefaultAge;

            ClientName = clientName;

            Employment defaultEmployment = new Employment();
            EmploymentList.Add(defaultEmployment);

            Pension defaultStatePension=new Pension("State Pension");
            PensionList.Add(defaultStatePension);

            Pension defaultOtherPension=new Pension("Other Pension");
            PensionList.Add(defaultOtherPension);

            OtherIncome defaultOtherIncome = new OtherIncome("Other Income");
            OtherIncomeList.Add(defaultOtherIncome);

            if (potMethod == PotMethodEnum.Individual)
            {
                RetirementPot DefaultPot = new RetirementPot { PotName = $"{ClientName} Retirement Pot" };

                IndividualPotList.Add(DefaultPot);                
            }

        }
    }

}
