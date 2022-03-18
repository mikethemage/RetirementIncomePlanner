using System.Collections.Generic;

namespace RetirementIncomePlannerLibrary
{
    public class Fund
    {
        private RetirementPot InitialPot;
        public decimal FundValue { get; private set; }

        public Fund(RetirementPot initialPot)
        {
            InitialPot = initialPot;
            FundValue = InitialPot.PotAmount.ItemValue;
        }

        public void AddContributions(List<ClientData> dataList, int year)
        {
            foreach (ClientData clientData in dataList)
            {
                if (clientData.HasContributions)
                {
                    FundValue += clientData.Contributions[year] / 2;
                }

            }
            FundValue *= (1.0M + InitialPot.InvestmentGrowth.ItemValue);

            foreach (ClientData clientData in dataList)
            {
                if (clientData.HasContributions)
                {
                    FundValue += clientData.Contributions[year] / 2;
                }
            }
            return;
        }

        public decimal TakeDrawDown(List<ClientData> dataList, int year)
        {
            decimal totalDrawdown = 0.0M;
            foreach (ClientData clientData in dataList)
            {
                totalDrawdown += clientData.RequiredDrawdown[year];
            }
            if (totalDrawdown > FundValue)
            {
                totalDrawdown = FundValue;
                FundValue = 0.0M;
            }
            else
            {
                FundValue -= totalDrawdown;
            }
            return totalDrawdown;
        }
    }
}
