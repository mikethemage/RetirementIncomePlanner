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
        private YearRowModel[] _outputData = null!;

        public required DataInputModel InputData { get; set; }
        public required YearRowModel[] OutputData
        {
            get
            {
                return _outputData;
            }

            set
            {
                _outputData = value;
                Chart.BuildChart(_outputData);                                             
            }
        }

        public ChartModel Chart { get; set; } = new ChartModel();       

    }
}
