﻿
using RetirementIncomePlannerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetirementIncomePlannerBlazorWebApp
{
    public class ReportViewModel : ViewModelBase
    {
        private YearRowModel[] outputData=null!;

        public DataInputModel InputData { get; set; } = null!;
        public YearRowModel[] OutputData
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
