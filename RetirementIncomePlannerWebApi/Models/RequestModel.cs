using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetirementIncomePlannerLogic;

namespace RetirementIncomePlannerWebApi.Models
{
    public class RequestModel
    {
        public required DataInputModel PensionInputData { get; set; }
    }
}
