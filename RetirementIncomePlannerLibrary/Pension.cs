namespace RetirementIncomePlannerLibrary
{
    public class Pension
    {
        public string PensionName { get; set; } = string.Empty;
        public string PensionType { get; set; } = string.Empty;
        public AgeValue PensionAge { get; set; } = new AgeValue();
        public AmountValue PensionAmount { get; set; } = new AmountValue();
    }
    
}
