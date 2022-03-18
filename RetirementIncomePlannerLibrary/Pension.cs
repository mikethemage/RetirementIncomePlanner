namespace RetirementIncomePlannerLibrary
{
    public class Pension
    {
        public string PensionName { get; set; } = string.Empty;
        public string PensionType { get; set; } = string.Empty;
        public AgeValue PensionAge { get; set; } = new AgeValue();
        public AmountValue PensionAmount { get; set; } = new AmountValue();

        public Pension(string pensionType)
        {
            PensionType = pensionType;
            PensionName = PensionType;
        }

        public decimal GetPensionForAge(int age)
        { 
            if(PensionAge.ValuePresent==false || PensionAmount.ValuePresent==false)
            {
                return 0.0M;
            }
            else
            {
                if(age>=PensionAge.ItemValue)
                {
                    return PensionAmount.ItemValue;
                }
                else
                {
                    return 0.0M;
                }
            }
        }
    }
    
}
