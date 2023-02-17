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
    public class PercentageFieldViewModel : ViewModelBase
    {
        private readonly CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");
        private readonly NumberStyles numberStyle = NumberStyles.Number;

        private bool isRequired=false;

        public bool IsRequired
        {
            get
            {
                return isRequired;
            }
            set
            {
                isRequired = value;
                OnPropertyChanged(nameof(IsRequired));
            }
        }


        private decimal percentageValue = 0M;

        public decimal PercentageValue
        {
            get
            {
                return percentageValue;
            }
            set
            {
                percentageValue = value;
                IsBlank = false;
                IsValid = true;
                OnPropertyChanged(nameof(PercentageText));
            }
        }

        public string PercentageText
        {
            get
            {
                if (IsBlank)
                {
                    return string.Empty;
                }
                else
                {
                    return percentageValue.ToString("P", culture);
                }
            }
            set
            {
                if (value == string.Empty)
                {
                    IsBlank = true;
                    IsValid = true;
                    percentageValue = 0M;
                }
                else
                {
                    IsValid = decimal.TryParse(value.Replace(culture.NumberFormat.PercentSymbol, ""), numberStyle, culture, out percentageValue);
                    if (IsValid)
                    {
                        percentageValue /= 100;
                        IsBlank = false;
                        OnPropertyChanged(nameof(PercentageText));
                    }
                    else
                    {
                        IsBlank = true;
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
