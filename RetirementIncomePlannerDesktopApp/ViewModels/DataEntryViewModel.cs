﻿using RetirementIncomePlannerLogic;
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
using System.Reflection;

namespace RetirementIncomePlannerDesktopApp
{
    
    public class DataEntryViewModel : ViewModelBase
    {
        public string getRunningVersion
        {
            get
            {
                return $"Version: {Assembly.GetExecutingAssembly().GetName().Version}";
            }

        }

        private int numberOfYears = 35;

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

                OnPropertyChanged(nameof(NumberOfClients));

                ExportReportCommand.RaiseCanExecuteChanged();
                PreviewChartCommand.RaiseCanExecuteChanged();
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

        public int NumberOfYears
        {
            get
            {
                return numberOfYears;
            }
            set
            {
                numberOfYears = value;
                OnPropertyChanged(nameof(NumberOfYears));

            }
        }
        public PercentageFieldViewModel Indexation { get; private set; } = new PercentageFieldViewModel { PercentageValue = 0.02M, IsRequired = true };
        public CurrencyFieldViewModel RetirementPot { get; private set; } = new CurrencyFieldViewModel { IsRequired = true };
        public PercentageFieldViewModel InvestmentGrowth { get; private set; } = new PercentageFieldViewModel { PercentageValue = 0.03M, IsRequired = true };
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
                e.PropertyName == nameof(PercentageFieldViewModel.PercentageText) ||
                e.PropertyName == "IsBlank" ||
                e.PropertyName == "IsValid"
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
                        PensionCalcs.BuildReportAndSaveToFile(inputModel, chart, saveFileDialog1.FileName);
                    }
                    catch
                    {
                        MessageBox.Show($"Error writing to file: {saveFileDialog1.FileName}.\n\nEnsure the file is not currently open and you have read/write permissions to the folder.");
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
                NumberOfYears= NumberOfYears,
                InvestmentGrowth = InvestmentGrowth.PercentageValue,
                RetirementPot = RetirementPot.CurrencyValue,
                Indexation = Indexation.PercentageValue,
                Clients=new List<ClientInputModel>()
            };

            foreach (ClientViewModel client in Clients)
            {
                output.Clients.Add(client.CreateModel());
            }
            return output;
        }
    }
}
