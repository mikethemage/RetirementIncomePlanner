using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RetirementIncomePlannerLogic
{
    public partial class PensionChartColorModel
    {
        //DefaultValue attributes needed for Swagger:
        [DefaultValue("#305d7a")]
        public string TotalDrawdownColor { get; set; } ="#305d7a";

        [DefaultValue("#746aa3")]
        public string StatePensionPrimaryColor { get; set; } = "#746aa3";

        [DefaultValue("#c9c0e7")]
        public string StatePensionSecondaryColor { get; set; } = "#c9c0e7";

        [DefaultValue("#ca6ca2")]
        public string OtherPensionPrimaryColor { get; set; } = "#ca6ca2";

        [DefaultValue("#f2bbda")]
        public string OtherPensionSecondaryColor { get; set; } = "#f2bbda";

        [DefaultValue("#ff7d76")]
        public string SalaryPrimaryColor { get; set; } = "#ff7d76";

        [DefaultValue("#ffc1b9")]
        public string SalarySecondaryColor { get; set; } = "#ffc1b9";

        [DefaultValue("#ffb13e")]
        public string OtherIncomePrimaryColor { get; set; } = "#ffb13e";

        [DefaultValue("#ffd29f")]
        public string OtherIncomeSecondaryColor { get; set; } = "#ffd29f";

        [DefaultValue("#000000")]
        public string TotalFundValueColor { get; set; } = "#000000"; 

        public bool ValidateColors()
        {
            if (!Test(TotalFundValueColor)) return false;
            if (!Test(StatePensionPrimaryColor)) return false;
            if (!Test(StatePensionSecondaryColor)) return false;
            if (!Test(OtherPensionPrimaryColor)) return false;
            if (!Test(OtherPensionSecondaryColor)) return false;
            if (!Test(SalaryPrimaryColor)) return false;
            if (!Test(SalarySecondaryColor)) return false;
            if (!Test(OtherIncomePrimaryColor)) return false;
            if (!Test(OtherIncomeSecondaryColor)) return false;
            if (!Test(TotalFundValueColor)) return false;

            return true;
        }

        private static bool Test(string hc)
        {
            return MyRegex().IsMatch(hc);
        }

        [GeneratedRegex("[#][0-9A-Fa-f]{6}\\b")]
        private static partial Regex MyRegex();
    }
}
