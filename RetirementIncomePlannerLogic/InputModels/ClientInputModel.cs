﻿namespace RetirementIncomePlannerLogic
{
    public class ClientInputModel
    {
        public int ClientNumber { get; set; } = 1;
        public required int Age { get; set; }

        public SalaryInputModel? SalaryDetails { get; set; } = null;

        public required int RetirementAge { get; set; }
        public required decimal StatePensionAmount { get; set; }
        public required int StatePensionAge { get; set; }

        public AgeAmountInputModel? OtherPensionDetails { get; set; } = null;

        public decimal? OtherIncome { get; set; } = null;
        public required decimal RetirementIncomeLevel { get; set; }

        public List<AgeAmountInputModel> AdhocTransactions { get; set; } = new List<AgeAmountInputModel>();

        public void AddAdhoc()
        {
            AdhocTransactions.Add(new AgeAmountInputModel { Age=0, Amount=0.0M });
        }

        public void RemoveAdhoc()
        {
            if (AdhocTransactions.Count > 1)
            {
                AdhocTransactions.Remove(AdhocTransactions[^1]);
            }
        }
    }


}