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
                return _itemValue;
            }
            set
            {
                _itemValue = value;
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
                decimal temp;
                if (decimal.TryParse(value, out temp))
                {
                    ItemValue = temp;
                }
                else
                {
                    ItemValue = 0;
                }
            }
        }

        public bool ValuePresent { get; set; }
    }
}
