namespace RetirementIncomePlannerLogic
{
    public class ClientInputModel
    {
        public int ClientNumber { get; set; } = 1;
        public int Age { get; set; }

        public SalaryInputModel? SalaryDetails { get; set; } = null;

        public int RetirementAge { get; set; }
        public decimal StatePensionAmount { get; set; }
        public int StatePensionAge { get; set; }

        public AgeAmountInputModel? OtherPensionDetails { get; set; } = null;

        public decimal? OtherIncome { get; set; } = null;
        public decimal RetirementIncomeLevel { get; set; }

        public List<AgeAmountInputModel> AdhocTransactions { get; set; } = new List<AgeAmountInputModel>();

        public void AddAdhoc()
        {
            AdhocTransactions.Add(new AgeAmountInputModel());
        }

        public void RemoveAdhoc()
        {
            if (AdhocTransactions.Count > 1)
            {
                AdhocTransactions.Remove(AdhocTransactions[-1]);
            }
        }
    }


}