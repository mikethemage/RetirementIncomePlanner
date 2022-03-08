namespace RetirementIncomePlannerLibrary
{
    public class AdhocContribution
    {
        public AgeValue Age { get; set; } = new AgeValue();
        public AmountValue AdhocAmount { get; set; } = new AmountValue();
        public RetirementPot RetirementPot { get; set; } = null;
    }   
    
}
