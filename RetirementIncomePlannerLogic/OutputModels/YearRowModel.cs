namespace RetirementIncomePlannerLogic
{
    public class YearRowModel
    {
        public int Year { get; set; }
        public decimal IndexationMultiplier { get; set; }
        public List<ClientRowModel> Clients { get; set; } = new List<ClientRowModel>();
        public decimal TotalRequiredDrawdown { get; set; }
        public decimal FundBeforeDrawdown { get; set; }
        public decimal TotalDrawdown { get; set; }
        public decimal TotalFundValue { get; set; }
    }

    
}