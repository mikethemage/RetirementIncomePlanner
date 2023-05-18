using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetirementIncomePlannerLogic
{
    internal class PensionChartSKColorValues
    {
        public PensionChartSKColorValues(PensionChartColorModel pensionChartColorModel)
        {
            TotalDrawdownColor = SKColor.Parse(pensionChartColorModel.TotalDrawdownColor);
            StatePensionPrimaryColor = SKColor.Parse(pensionChartColorModel.StatePensionPrimaryColor); 
            StatePensionSecondaryColor = SKColor.Parse(pensionChartColorModel.StatePensionSecondaryColor);
            OtherPensionPrimaryColor = SKColor.Parse(pensionChartColorModel.OtherPensionPrimaryColor); 
            OtherPensionSecondaryColor = SKColor.Parse(pensionChartColorModel.OtherPensionSecondaryColor); 
            SalaryPrimaryColor = SKColor.Parse(pensionChartColorModel.SalaryPrimaryColor);
            SalarySecondaryColor = SKColor.Parse(pensionChartColorModel.SalarySecondaryColor); 
            OtherIncomePrimaryColor = SKColor.Parse(pensionChartColorModel.OtherIncomePrimaryColor);
            OtherIncomeSecondaryColor = SKColor.Parse(pensionChartColorModel.OtherIncomeSecondaryColor); 
            TotalFundValueColor = SKColor.Parse(pensionChartColorModel.TotalFundValueColor); 
        }

        public SKColor TotalDrawdownColor { get; set; }
        public SKColor StatePensionPrimaryColor { get; set; } 
        public SKColor StatePensionSecondaryColor { get; set; } 
        public SKColor OtherPensionPrimaryColor { get; set; } 
        public SKColor OtherPensionSecondaryColor { get; set; }
        public SKColor SalaryPrimaryColor { get; set; } 
        public SKColor SalarySecondaryColor { get; set; } 
        public SKColor OtherIncomePrimaryColor { get; set; } 
        public SKColor OtherIncomeSecondaryColor { get; set; } 
        public SKColor TotalFundValueColor { get; set; } 
    }
}
