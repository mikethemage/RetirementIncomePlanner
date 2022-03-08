namespace RetirementIncomePlannerLibrary
{
    public class Employment
    {
        public string EmploymentName { get; set; } = string.Empty;
        public AmountValue Salary { get; set; } = new AmountValue();
        public AgeValue PartialRetirementAge { get; set; } = new AgeValue();
        public AmountValue PartialRetirementSalary { get; set; } = new AmountValue();
        public AgeValue RetirementAge { get; set; } = new AgeValue();
    }
    
}
