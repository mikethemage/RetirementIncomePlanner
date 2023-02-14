namespace RetirementIncomePlannerLogic
{
    public class ClientRowModel 
    {
        public int ClientNumber { get; set; } = 1;
        public int Age { get; set;}
        public decimal StatePension { get; set; } = 0M;

        public decimal OtherPension { get; set;}
        public decimal Salary { get; set; } = 0M;
        public decimal OtherIncome { get; set;}
        public decimal Contribution { get;set; }
    }

    
}