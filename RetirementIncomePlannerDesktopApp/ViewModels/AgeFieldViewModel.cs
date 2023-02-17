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

        private int ageValue = 0;

        public int AgeValue
        {
            get
            {
                return ageValue;
            }
            set
            {
                ageValue = value;
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
                    return ageValue.ToString();
                }
            }
            set
            {
                if (value == string.Empty)
                {
                    IsBlank = true;
                    IsValid = true;
                    ageValue = 0;
                }
                else
                {
                    IsValid = int.TryParse(value, out ageValue);
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
