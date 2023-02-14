using RetirementIncomePlannerLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.IO;

namespace RetirementIncomePlannerDesktopApp
{

    public class DataEntryViewModel : ViewModelBase
    {
        public int NumberOfClients
        {
            get
            {
                return Clients.Count;
            }

            set
            {
                while (value > Clients.Count)
                {
                    AddClient();
                }

                while (value < Clients.Count && Clients.Count > 1)
                {
                    RemoveClient();
                }
            }
        }

        public void AddClient()
        {
            ClientViewModel clientToAdd = new ClientViewModel
            {
                ClientNumber = Clients.Count + 1
            };

            Clients.Add(clientToAdd);

            clientToAdd.PropertyChanged += ChildPropertyChanged;
        }

        public void RemoveClient()
        {
            if (Clients.Count > 1)
            {
                Clients[^1].PropertyChanged-= ChildPropertyChanged;
                Clients.Remove(Clients[^1]);
            }
        }

        public int NumberOfYears { get; set; } = 35;

        public PercentageFieldViewModel Indexation { get; private set; } = new PercentageFieldViewModel { PercentageValue = 0.02M };
        public CurrencyFieldViewModel RetirementPot { get; private set; } = new CurrencyFieldViewModel();
        public PercentageFieldViewModel InvestmentGrowth { get; private set; } = new PercentageFieldViewModel { PercentageValue = 0.03M };

        public ObservableCollection<ClientViewModel> Clients { get; set; } = new ObservableCollection<ClientViewModel>();

        public List<int> YearsList { get; set; } = new List<int>();
        public List<int> ClientNumbersList { get; set; } = new List<int> { 1, 2 };

        public DataEntryViewModel()
        {
            Clients.Add(new ClientViewModel());

            for (int i = 1; i <= 35; i++)
            {
                YearsList.Add(i);
            }

            Indexation.PropertyChanged += ChildPropertyChanged;
            RetirementPot.PropertyChanged += ChildPropertyChanged;
            InvestmentGrowth.PropertyChanged += ChildPropertyChanged;
            Clients[0].PropertyChanged += ChildPropertyChanged;

            ExportReportCommand = new RelayCommand(ExportReport, CanExportReport);
        }

        private void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AgeFieldViewModel.AgeText) ||
                e.PropertyName == nameof(CurrencyFieldViewModel.CurrencyText) ||
                e.PropertyName == nameof(PercentageFieldViewModel.PercentageText)                 
                )
            {
                ExportReportCommand.RaiseCanExecuteChanged();
            }

        }

        public RelayCommand ExportReportCommand { get; private set; }
        public void ExportReport()
        {
            if (CanExportReport())
            {
                
                DataInputModel inputModel = CreateModel();
                YearRowModel[] outputModel = PensionCalcs.RunPensionCalcs(inputModel);

                ReportViewModel report = new ReportViewModel
                {
                    InputData=inputModel,
                    OutputData=outputModel
                };


                //using (StreamWriter outputFile = new(@"C:\Users\Mike\Documents\TestRIOutput.csv", false))
                //{
                //    outputFile.WriteLine("Year,Indexation Multiplier,Client1 Age,Client1 State Pension,Client1 Salary,Client2 Age,Client2 State Pension,Client2 Other Pension,Total Required Drawdown,Fund Before Drawdown,Total Drawdown,Total Fund Value");

                //    foreach (YearRowModel row in outputModel)
                //    {
                //        outputFile.WriteLine($"{row.Year},{row.IndexationMultiplier},{row.Clients[0].Age},{row.Clients[0].StatePension},{row.Clients[0].Salary},{row.Clients[1].Age},{row.Clients[1].StatePension},{row.Clients[1].OtherPension},{row.TotalRequiredDrawdown},{row.FundBeforeDrawdown},{row.TotalDrawdown},{row.TotalFundValue}");
                //    }
                //}

                ReportView reportView = new ReportView();
                reportView.DataContext= report;
                reportView.Owner = Application.Current.MainWindow;
                reportView.ShowDialog();

               
            }
            else
            {
                MessageBox.Show("Validation Error!");
            }
        }

        public bool CanExportReport()
        {
            return CanCreateModel();
        }

        public bool CanCreateModel()
        {
            return Indexation.IsValid && !Indexation.IsBlank &&
                RetirementPot.IsValid && !RetirementPot.IsBlank &&
                InvestmentGrowth.IsValid && !InvestmentGrowth.IsBlank &&
                !Clients.Where(x => !x.CanCreateModel()).Any();
        }

        public DataInputModel CreateModel()
        {
            DataInputModel output = new DataInputModel();
            output.InvestmentGrowth = InvestmentGrowth.PercentageValue;
            output.RetirementPot = RetirementPot.CurrencyValue;
            output.Indexation = Indexation.PercentageValue;
            foreach (ClientViewModel client in Clients)
            {
                output.Clients.Add(client.CreateModel());
            }
            return output;
        }


    }
}
