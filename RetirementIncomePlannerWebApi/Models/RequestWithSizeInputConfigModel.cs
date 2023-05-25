using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetirementIncomePlannerLogic;
using RetirementIncomePlannerLogic.InputModels;

namespace RetirementIncomePlannerWebApi.Models
{
    public class RequestWithSizeInputConfigModel
    {
        public required DataInputModel PensionInputData { get; set; }
        public PensionChartColorModel? PensionChartColors { get; set;}
        public ImageSizeModel? ImageSize { get; set; }
    }
}
