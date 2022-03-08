namespace RetirementIncomePlannerLibrary
{
    public class PercentageValue : IFormattedValue
    {
        public const decimal PercentageMin = -4.0M;
        public const decimal PercentageMax = 4.0M;

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
