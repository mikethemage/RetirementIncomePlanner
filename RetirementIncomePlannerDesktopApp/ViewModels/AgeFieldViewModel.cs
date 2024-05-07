using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetirementIncomePlannerDesktopApp
{
    public class AgeFieldViewModel : ViewModelBase
    {
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

        private int _ageValue = 0;

        public int AgeValue
        {
            get
            {
                return _ageValue;
            }
            set
            {
                _ageValue = value;
                IsBlank = false;
                IsValid = true;
                OnPropertyChanged(nameof(AgeText));
            }
        }

        public string AgeText
        {
            get
            {
                if (IsBlank)
                {
                    return string.Empty;
                }
                else
                {
                    return _ageValue.ToString();
                }
            }
            set
            {
                if (value == string.Empty)
                {
                    IsBlank = true;
                    IsValid = true;
                    _ageValue = 0;
                }
                else
                {
                    IsValid = int.TryParse(value, out _ageValue);
                    if (IsValid)
                    {                       
                        IsBlank = false;
                        OnPropertyChanged(nameof(AgeText));
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
