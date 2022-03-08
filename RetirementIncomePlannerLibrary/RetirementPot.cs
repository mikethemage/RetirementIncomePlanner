namespace RetirementIncomePlannerLibrary
{
    public class RetirementPot
    {
        public string PotName { get; set; } = string.Empty;
        public AmountValue PotAmount { get; set; } = new AmountValue();
        public PercentageValue InvestmentGrowth { get; set; } = new PercentageValue();
    }
    
}
