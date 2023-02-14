using RetirementIncomePlannerLogic;

namespace RetirementIncomePlannerConsoleSample
{
    internal partial class Program
    {
        static void Main(string[] args)
        {

           

            DataInputModel testViewModel = GetTestData();
            YearRowModel[] testDataList = PensionCalcs.RunPensionCalcs(testViewModel);
            Console.WriteLine("Year,Indexation Multiplier,Client1 Age,Client1 State Pension,Client1 Salary,Client2 Age,Client2 State Pension,Client2 Other Pension,Total Required Drawdown,Fund Before Drawdown,Total Drawdown,Total Fund Value");

            foreach (YearRowModel row in testDataList)
            {
                Console.WriteLine($"{row.Year},{row.IndexationMultiplier},{row.Clients[0].Age},{row.Clients[0].StatePension},{row.Clients[0].Salary},{row.Clients[1].Age},{row.Clients[1].StatePension},{row.Clients[1].OtherPension},{row.TotalRequiredDrawdown},{row.FundBeforeDrawdown},{row.TotalDrawdown},{row.TotalFundValue}");
            }
        }

        static DataInputModel GetTestData()
        {
            DataInputModel output = new DataInputModel();

            output.NumberOfYears = 35;
            output.NumberOfClients = 2;
            output.Indexation = 0.02M;
            output.RetirementPot = 196000M;
            output.InvestmentGrowth = 0.03M;

            output.Clients[0].Age = 58;

            output.Clients[0].SalaryDetails = new SalaryInputModel
            {
                FullSalaryAmount= 22000M
            };

            
            output.Clients[0].RetirementAge = 60;
            output.Clients[0].StatePensionAmount = 9360M;
            output.Clients[0].StatePensionAge = 67;
            output.Clients[0].RetirementIncomeLevel = 15000M;

            output.Clients[1].Age = 58;
            output.Clients[1].RetirementAge = 68;
            output.Clients[1].StatePensionAmount = 9360M;
            output.Clients[1].StatePensionAge = 67;

            output.Clients[1].OtherPensionDetails = new AgeAmountInputModel
            {
                Age = 58,
                Amount = 8285M
            };
           
            output.Clients[1].RetirementIncomeLevel = 15000;

            return output;
        }
    }
}