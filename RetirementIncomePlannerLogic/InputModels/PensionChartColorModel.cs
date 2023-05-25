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
    public class PensionChartColorModel
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
            if (!test(TotalFundValueColor)) return false;
            if (!test(StatePensionPrimaryColor)) return false;
            if (!test(StatePensionSecondaryColor)) return false;
            if (!test(OtherPensionPrimaryColor)) return false;
            if (!test(OtherPensionSecondaryColor)) return false;
            if (!test(SalaryPrimaryColor)) return false;
            if (!test(SalarySecondaryColor)) return false;
            if (!test(OtherIncomePrimaryColor)) return false;
            if (!test(OtherIncomeSecondaryColor)) return false;
            if (!test(TotalFundValueColor)) return false;

            return true;
        }

        private static bool test(string hc)
        {
            return Regex.IsMatch(hc, @"[#][0-9A-Fa-f]{6}\b");
        }
    }
}
