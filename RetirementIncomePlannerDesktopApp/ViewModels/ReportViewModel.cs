using Microsoft.Windows.Themes;
using RetirementIncomePlannerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetirementIncomePlannerDesktopApp
{
    public class ReportViewModel : ViewModelBase
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private YearRowModel[] outputData;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public required DataInputModel InputData { get; set; }
        public required YearRowModel[] OutputData
        {
            get
            {
                return outputData;
            }

            set
            {
                outputData = value;
                Chart.BuildChart(outputData);                                             
            }
        }

        public ChartModel Chart { get; set; } = new ChartModel();       

    }
}
