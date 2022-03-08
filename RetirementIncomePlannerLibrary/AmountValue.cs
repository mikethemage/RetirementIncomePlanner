namespace RetirementIncomePlannerLibrary
{
    public class AmountValue : IFormattedValue
    {
        public const decimal AmountMin = 0;
        public const decimal ContributionAmountMin = -400000000000M;
        public const decimal AmountMax = 400000000000M;

        public bool ContributionAmount { get; set; } = false;

        private decimal _itemValue;
        public decimal ItemValue
        {
            get
            {
                if (ValuePresent)
                {
                    return _itemValue;
                }
                else
                {
                    return 0.0M;
                }
            }
            set
            {
                if ( ((value >= ContributionAmountMin && ContributionAmount==true) ||
                      (value >= AmountMin && ContributionAmount == false))                    
                    && value <= AmountMax)
                {
                    _itemValue = value;
                    ValuePresent = true;
                }
            }
        }

        public string Text
        {
            get
            {
                return ItemValue.ToString();
            }
            set
            {
                if (value == string.Empty)
                {
                    ValuePresent = false;
                }
                else
                {
                    decimal temp;
                    if (decimal.TryParse(value, out temp))
                    {
                        ItemValue = temp;
                    }
                }
            }
        }

        public bool ValuePresent { get; set; }

        public AmountValue(bool contributionAmount=false)
        {
            ItemValue = 0.0M;
            ValuePresent = false;
            ContributionAmount = contributionAmount;
        }
    }
}
