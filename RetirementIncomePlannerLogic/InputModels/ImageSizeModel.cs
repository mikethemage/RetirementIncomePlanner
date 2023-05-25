using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetirementIncomePlannerLogic.InputModels
{
    public class ImageSizeModel
    {
        [DefaultValue(1354)]
        public int Width { get; set; } = 1360;

        [DefaultValue(980)]
        public int Height { get; set; } = 980;
    }
}
