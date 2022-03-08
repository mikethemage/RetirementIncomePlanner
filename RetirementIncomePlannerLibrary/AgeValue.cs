namespace RetirementIncomePlannerLibrary
{
    public class AgeValue : IFormattedValue
    {
        public const int AgeMin = 0;
        public const int AgeMax = 200;

        private int _itemValue;
        public int ItemValue { get
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
                int temp;
                if(int.TryParse(value, out temp))
                {
                    ItemValue = temp;
                }
                else
                {
                    ItemValue = 0;
                }
            }
        }
        
        public bool ValuePresent { get ; set ; }
    }
}
