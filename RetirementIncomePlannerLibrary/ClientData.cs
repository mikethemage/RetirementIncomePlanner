using System.Collections.Generic;

namespace RetirementIncomePlannerLibrary
{
    public class ClientData
    {
        public List<int> ClientAge { get; set; } = new List<int>();
        public List<decimal> StatePensions { get; set; } = new List<decimal>();
        public List<decimal> OtherPensions { get; set; } = new List<decimal>();
        public List<decimal> Salary { get; set; } = new List<decimal>();
        public List<decimal> OtherIncome { get; set; } = new List<decimal>();

        public List<decimal> Contributions { get; set; } = new List<decimal>();

        public List<decimal> RequiredDrawdown { get; set; } = new List<decimal>();

        public bool HasStatePensions { get; set; } = false;
        public bool HasOtherPensions { get; set; } = false;
        public bool HasSalary { get; set; } = false;
        public bool HasOtherIncome { get; set; } = false;
        public bool HasContributions { get; set; } = false; 
    }

}
