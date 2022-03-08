namespace RetirementIncomePlannerLibrary
{
    public class AgeValue : IFormattedValue
    {
        public const int AgeMin = 0;
        public const int AgeMax = 200;

        private int _itemValue;
        public int ItemValue
        {
            get
            {
                if (ValuePresent)
                {
                    return _itemValue;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value >= AgeMin && value <= AgeMax)
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
                    int temp;
                    if (int.TryParse(value, out temp))
                    {
                        ItemValue = temp;
                    }
                }
            }
        }

        public bool ValuePresent { get; set; }

        public AgeValue()
        {
            ItemValue=0;
            ValuePresent = false;
        }
    }
}
