namespace RetirementIncomePlannerLibrary
{
    public class AdhocContribution
    {
        public AgeValue Age { get; set; } = new AgeValue();
        public AmountValue AdhocAmount { get; set; } = new AmountValue();
        public RetirementPot RetirementPot { get; set; } = null;

        public decimal GetContributionForAge(int age)
        {
            if (Age.ValuePresent == false || AdhocAmount.ValuePresent == false)
            {
                return 0.0M;
            }
            else
            {
                if (age == Age.ItemValue)
                {
                    return AdhocAmount.ItemValue;
                }
                else
                {
                    return 0.0M;
                }
            }
        }

    }
}
