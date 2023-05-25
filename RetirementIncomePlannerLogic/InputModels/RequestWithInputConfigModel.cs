using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetirementIncomePlannerLogic.InputModels
{
    public class RequestWithInputConfigModel
    {
        public required DataInputModel DataInputModel { get; set; }
        public PensionChartColorModel? PensionChartColorModel { get; set;}
    }
}
