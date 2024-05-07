using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RetirementIncomePlannerBlazorWebApp
{
    public class PercentageFieldViewModel : ViewModelBase
    {
        private readonly CultureInfo _culture = CultureInfo.CreateSpecificCulture("en-GB");
        private readonly NumberStyles _numberStyle = NumberStyles.Number;

        private bool _isRequired=false;

        public bool IsRequired
        {
            get
            {
                return _isRequired;
            }
            set
            {
                _isRequired = value;
                OnPropertyChanged(nameof(IsRequired));
            }
        }


        private decimal _percentageValue = 0M;

        public decimal PercentageValue
        {
            get
            {
                return _percentageValue;
            }
            set
            {
                _percentageValue = value;
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
                    return _percentageValue.ToString("P", _culture);
                }
            }
            set
            {
                if (value == string.Empty)
                {
                    IsBlank = true;
                    IsValid = true;
                    _percentageValue = 0M;
                }
                else
                {
                    IsValid = decimal.TryParse(value.Replace(_culture.NumberFormat.PercentSymbol, ""), _numberStyle, _culture, out _percentageValue);
                    if (IsValid)
                    {
                        _percentageValue /= 100;
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

        private bool _isValid = true;

        public bool IsValid
        {
            get { return _isValid; }
            private set
            {
                _isValid = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool _isBlank = true;

        public bool IsBlank
        {
            get { return _isBlank; }
            private set
            {
                _isBlank = value;
                OnPropertyChanged(nameof(IsBlank));
            }
        }
    }
}
