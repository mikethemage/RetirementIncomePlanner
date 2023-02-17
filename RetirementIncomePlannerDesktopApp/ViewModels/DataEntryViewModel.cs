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
using Microsoft.Win32;

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
                Clients[^1].PropertyChanged -= ChildPropertyChanged;
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
            PreviewChartCommand = new RelayCommand(ViewChart, CanViewChart);
        }

        private void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AgeFieldViewModel.AgeText) ||
                e.PropertyName == nameof(CurrencyFieldViewModel.CurrencyText) ||
                e.PropertyName == nameof(PercentageFieldViewModel.PercentageText)
                )
            {
                ExportReportCommand.RaiseCanExecuteChanged();
                PreviewChartCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand PreviewChartCommand { get; private set; }
        public RelayCommand ExportReportCommand { get; private set; }
        public void ExportReport()
        {
            if (CanExportReport())
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    RestoreDirectory = false,
                    Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
                    FilterIndex = 1
                };

                if (saveFileDialog1.ShowDialog() == true)
                {
                    DataInputModel inputModel = CreateModel();
                    YearRowModel[] outputModel = PensionCalcs.RunPensionCalcs(inputModel);
                    ChartModel chart = new ChartModel();
                    chart.BuildChart(outputModel);
                    try
                    {
                        PensionCalcs.BuildReport(inputModel, chart, saveFileDialog1.FileName);
                    }
                    catch
                    {
                        MessageBox.Show($"Error opening file: {saveFileDialog1.FileName}.\n\nEnsure the file is not currently open and you have read/write permissions to the folder.");
                    }
                }                
            }
            else
            {
                MessageBox.Show("Validation Error!");
            }
        }

        public void ViewChart()
        {
            if (CanExportReport())
            {
                DataInputModel inputModel = CreateModel();
                YearRowModel[] outputModel = PensionCalcs.RunPensionCalcs(inputModel);

                ReportViewModel report = new ReportViewModel
                {
                    InputData = inputModel,
                    OutputData = outputModel
                };

                ReportView reportView = new ReportView
                {
                    DataContext = report,
                    Owner = Application.Current.MainWindow
                };

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

        public bool CanViewChart()
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
            DataInputModel output = new DataInputModel
            {
                InvestmentGrowth = InvestmentGrowth.PercentageValue,
                RetirementPot = RetirementPot.CurrencyValue,
                Indexation = Indexation.PercentageValue
            };

            foreach (ClientViewModel client in Clients)
            {
                output.Clients.Add(client.CreateModel());
            }
            return output;
        }
    }
}
