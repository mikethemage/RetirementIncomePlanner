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
                if (value >= PercentageMin && value <= PercentageMax)
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
                if (ValuePresent)
                {
                    return ItemValue.ToString();
                }
                else
                {
                    return string.Empty;
                }
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

        public PercentageValue()
        {
            ItemValue = 0.0M;
            ValuePresent = false;
        }
    }
}
