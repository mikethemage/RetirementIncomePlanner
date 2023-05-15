using RetirementIncomePlannerLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace RetirementIncomePlannerDesktopApp
{
    public class ClientViewModel : ViewModelBase
    {
        public ClientViewModel()
        {
            Age.PropertyChanged += ChildPropertyChanged;
            Salary.PropertyChanged += ChildPropertyChanged;
            PartialRetirementAge.PropertyChanged += ChildPropertyChanged;
            PartialRetirementSalary.PropertyChanged += ChildPropertyChanged;
            RetirementAge.PropertyChanged += ChildPropertyChanged;
            StatePensionAmount.PropertyChanged += ChildPropertyChanged;
            StatePensionAge.PropertyChanged += ChildPropertyChanged;
            OtherPensionsAmount.PropertyChanged += ChildPropertyChanged;
            OtherPensionsAge.PropertyChanged += ChildPropertyChanged;
            OtherIncome.PropertyChanged += ChildPropertyChanged;
            RetirementIncomeLevel.PropertyChanged += ChildPropertyChanged;
        }

        private void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                OnPropertyChanged(e.PropertyName);
                if (e.PropertyName != "IsRequired")
                {
                    CheckLinkedRequiredFields();
                }
            }
        }

        private void CheckLinkedRequiredFields()
        {
            if (!PartialRetirementAge.IsBlank || !PartialRetirementSalary.IsBlank)
            {
                Salary.IsRequired = true;
                PartialRetirementAge.IsRequired = true;
                PartialRetirementSalary.IsRequired = true;
            }
            else
            {
                Salary.IsRequired = false;
                PartialRetirementAge.IsRequired = false;
                PartialRetirementSalary.IsRequired = false;
            }

            if (!OtherPensionsAge.IsBlank || !OtherPensionsAmount.IsBlank)
            {
                OtherPensionsAge.IsRequired = true;
                OtherPensionsAmount.IsRequired = true;
            }
            else
            {
                OtherPensionsAge.IsRequired = false;
                OtherPensionsAmount.IsRequired = false;
            }

        }

        //private readonly CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");

        private int clientNumber = 1;

        public int ClientNumber
        {
            get
            {
                return clientNumber;
            }

            set
            {
                clientNumber = value;
                OnPropertyChanged(nameof(ClientNumber));
                OnPropertyChanged(nameof(ClientNumberText));
            }
        }

        public string ClientNumberText { get { return "Client " + ClientNumber.ToString(); } }

        public AgeFieldViewModel Age { get; private set; } = new AgeFieldViewModel { IsRequired = true };

        public CurrencyFieldViewModel Salary { get; private set; } = new CurrencyFieldViewModel();

        public AgeFieldViewModel PartialRetirementAge { get; private set; } = new AgeFieldViewModel();

        public CurrencyFieldViewModel PartialRetirementSalary { get; private set; } = new CurrencyFieldViewModel();

        public AgeFieldViewModel RetirementAge { get; private set; } = new AgeFieldViewModel { IsRequired = true };

        public CurrencyFieldViewModel StatePensionAmount { get; private set; } = new CurrencyFieldViewModel { IsRequired = true };

        public AgeFieldViewModel StatePensionAge { get; private set; } = new AgeFieldViewModel { IsRequired = true };

        public CurrencyFieldViewModel OtherPensionsAmount { get; private set; } = new CurrencyFieldViewModel();

        public AgeFieldViewModel OtherPensionsAge { get; private set; } = new AgeFieldViewModel();

        public CurrencyFieldViewModel OtherIncome { get; private set; } = new CurrencyFieldViewModel();

        public CurrencyFieldViewModel RetirementIncomeLevel { get; private set; } = new CurrencyFieldViewModel { IsRequired = true };

        public ObservableCollection<AdhocItemViewModel> AdhocItems { get; set; } = new ObservableCollection<AdhocItemViewModel>();

        public bool CanCreateModel()
        {
            return !Age.IsBlank && Age.IsValid &&
                Salary.IsValid &&
                PartialRetirementAge.IsValid && PartialRetirementSalary.IsValid &&
                ((PartialRetirementAge.IsBlank && PartialRetirementSalary.IsBlank) || (!PartialRetirementAge.IsBlank && !PartialRetirementSalary.IsBlank && !Salary.IsBlank)) &&
                RetirementAge.IsValid && !RetirementAge.IsBlank &&
                !StatePensionAge.IsBlank && StatePensionAge.IsValid &&
                !StatePensionAmount.IsBlank && StatePensionAmount.IsValid &&
                OtherPensionsAge.IsValid && OtherPensionsAmount.IsValid &&
                ((OtherPensionsAge.IsBlank && OtherPensionsAmount.IsBlank) || (!OtherPensionsAge.IsBlank && !OtherPensionsAmount.IsBlank)) &&
                OtherIncome.IsValid &&
                RetirementIncomeLevel.IsValid && !RetirementIncomeLevel.IsBlank
                && !AdhocItems.Where(x => !x.CanCreateModel() && !x.IsBlank()).Any();
        }

        public ClientInputModel CreateModel()
        {
            ClientInputModel output = new ClientInputModel
            {
                ClientNumber = ClientNumber,
                Age = Age.AgeValue,
                RetirementAge = RetirementAge.AgeValue,
                StatePensionAge = StatePensionAge.AgeValue,
                StatePensionAmount = StatePensionAmount.CurrencyValue,
                RetirementIncomeLevel=0.0M
            };

            if (!Salary.IsBlank)
            {
                output.SalaryDetails = new SalaryInputModel
                {
                    FullSalaryAmount = Salary.CurrencyValue
                };

                if (!PartialRetirementAge.IsBlank && !PartialRetirementSalary.IsBlank)
                {
                    output.SalaryDetails.PartialRetirementDetails = new AgeAmountInputModel
                    {
                        Age = PartialRetirementAge.AgeValue,
                        Amount = PartialRetirementSalary.CurrencyValue
                    };
                }
            }

            if (!OtherPensionsAge.IsBlank && !OtherPensionsAmount.IsBlank)
            {
                output.OtherPensionDetails = new AgeAmountInputModel
                {
                    Age = OtherPensionsAge.AgeValue,
                    Amount = OtherPensionsAmount.CurrencyValue
                };
            }

            if (!OtherIncome.IsBlank)
            {
                output.OtherIncome = OtherIncome.CurrencyValue;
            }

            output.RetirementIncomeLevel = RetirementIncomeLevel.CurrencyValue;

            foreach (AdhocItemViewModel adhocItem in AdhocItems.Where(x => x.CanCreateModel()))
            {
                output.AdhocTransactions.Add(adhocItem.CreateModel());
            }

            return output;
        }
    }
}
