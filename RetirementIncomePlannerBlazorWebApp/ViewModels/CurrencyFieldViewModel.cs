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
    public class CurrencyFieldViewModel : ViewModelBase
    {
        private readonly CultureInfo _culture = CultureInfo.CreateSpecificCulture("en-GB");
        private readonly NumberStyles _numberStyle = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;

        private decimal _currencyValue = 0M;
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
        public decimal CurrencyValue
        {
            get
            {
                return _currencyValue;
            }
            set
            {
                _currencyValue = value;
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
                    return _currencyValue.ToString("C", _culture);
                }
            }
            set
            {
                if (value == string.Empty)
                {
                    IsBlank = true;
                    IsValid = true;
                    _currencyValue = 0M;
                }
                else
                {
                    IsValid = decimal.TryParse(value, _numberStyle, _culture, out _currencyValue);
                    if (IsValid)
                    {
                        _currencyValue = Math.Round(_currencyValue, 2, MidpointRounding.ToZero);
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
