using RetirementIncomePlannerLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetirementIncomePlannerBlazorWebApp
{
    public class AdhocItemViewModel : ViewModelBase
    {
        public AgeFieldViewModel Age { get; private set; } = new AgeFieldViewModel();
        public CurrencyFieldViewModel Amount { get; set; } = new CurrencyFieldViewModel();

        private void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                OnPropertyChanged(e.PropertyName);
            }
        }

        public AdhocItemViewModel()
        {
            Age.PropertyChanged += ChildPropertyChanged;
            Amount.PropertyChanged += ChildPropertyChanged;
        }

        public bool CanCreateModel()
        {
            return !Age.IsBlank && !Amount.IsBlank &&
                Age.IsValid && Amount.IsValid;
        }

        public bool IsBlank()
        {
            return Age.IsBlank && Amount.IsBlank;
        }

        public AgeAmountInputModel CreateModel()
        {
            return new AgeAmountInputModel
            {
                Age = Age.AgeValue,
                Amount = Amount.CurrencyValue
            };
        }
    }
}
