using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RetirementIncomePlannerDesktopApp
{
    public class CurrencyFieldViewModel : ViewModelBase
    {
        private readonly CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");
        private readonly NumberStyles numberStyle = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;

        private decimal currencyValue = 0M;

        public decimal CurrencyValue
        {
            get
            {
                return currencyValue;
            }
            set
            {
                currencyValue = value;
                IsBlank = false;
                IsValid = true;
                OnPropertyChanged(nameof(CurrencyText));
            }
        }

        public string CurrencyText
        {
            get
            {
                if (IsBlank)
                {
                    return string.Empty;
                }
                else
                {
                    return currencyValue.ToString("C", culture);
                }
            }
            set
            {
                if (value == string.Empty)
                {
                    IsBlank = true;
                    IsValid = true;
                    currencyValue = 0M;
                }
                else
                {
                    IsValid = decimal.TryParse(value, numberStyle, culture, out currencyValue);
                    if (IsValid)
                    {
                        currencyValue = Math.Round(currencyValue, 2, MidpointRounding.ToZero);
                        IsBlank = false;
                        OnPropertyChanged(nameof(CurrencyText));
                    }
                    else
                    {
                        IsBlank= true;
                    }
                }
            }
        }

        private bool isValid = true;

        public bool IsValid
        {
            get { return isValid; }
            private set
            {
                isValid = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool isBlank = true;

        public bool IsBlank
        {
            get { return isBlank; }
            private set
            {
                isBlank = value;
                OnPropertyChanged(nameof(IsBlank));
            }
        }       
    }
}
