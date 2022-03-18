using System;
using System.Collections.Generic;

namespace RetirementIncomePlannerLibrary
{

    public class IncomePlannerModel
    {
        public const int DefaultNumberOfYears = 35;
        public const decimal DefaultIndexation = 0.02M;

        public AgeValue NumberOfYears { get; set; } = new AgeValue();
        public int NumberOfClients { get; private set; }
        public List<Client> ClientList { get; private set; } = new List<Client>();
        public PercentageValue Indexation { get; set; } = new PercentageValue();
        public PotMethodEnum PotMethod { get; set; } = PotMethodEnum.Combined;
        public RetirementPot CombinedPot { get; set; } = new RetirementPot();

        public IncomePlannerModel(PotMethodEnum potMethod = PotMethodEnum.Combined, int IntialNumberOfClients = 1)
        {
            NumberOfYears.ItemValue = DefaultNumberOfYears;

            NumberOfClients = IntialNumberOfClients;

            for (int i = 0; i < NumberOfClients; i++)
            {
                ClientList.Add(new Client($"Client {i+1}", potMethod));
            }

            Indexation.ItemValue = DefaultIndexation;

            PotMethod = potMethod;

            //if (PotMethod == PotMethodEnum.Combined)
            //{
                CombinedPot = new RetirementPot { PotName = "Combined Retirement Pot" };
            //}
        }

        public void AddClient()
        {
            ClientList.Add(new Client($"Client {NumberOfClients + 1}", PotMethod));
            NumberOfClients++;
        }

        public void RemoveClient(Client clientToRemove)
        {
            ClientList.Remove(clientToRemove);
            NumberOfClients--;
        }

        public DataForGraph GetData()
        {
            DataForGraph dataForGraph = new DataForGraph();
            List<decimal> yearNumber = new List<decimal>();
            for (int i = 0; i <= NumberOfYears.ItemValue; i++)
            {
                yearNumber.Add(i);
            }
            dataForGraph.AddSeries("Year", yearNumber);

            List<ClientData> clientDatas = new List<ClientData>();
            List<List<decimal>> FundList = new List<List<decimal>>();
            List<List<decimal>> DrawdownList = new List<List<decimal>>();
            foreach (Client client in ClientList)
            {
                ClientData currentClientData = client.GetClientData(NumberOfYears.ItemValue, Indexation.ItemValue);
                clientDatas.Add(currentClientData);

                if (currentClientData.HasStatePensions)
                {
                    dataForGraph.AddSeries($"{client.ClientName} State Pension",currentClientData.StatePensions);
                }

                if (currentClientData.HasOtherPensions)
                {
                    dataForGraph.AddSeries($"{client.ClientName} Other Pension", currentClientData.OtherPensions);
                }

                if (currentClientData.HasSalary)
                {
                    dataForGraph.AddSeries($"{client.ClientName} Salary", currentClientData.Salary);
                }

                if (currentClientData.HasOtherIncome)
                {
                    dataForGraph.AddSeries($"{client.ClientName} Other Income", currentClientData.OtherIncome);
                }

                if (PotMethod == PotMethodEnum.Individual)
                {
                    //need to process each client's Pot
                    List<decimal> currentFundList = new List<decimal>();
                    List<decimal> currentDrawdownList = new List<decimal>();
                    Fund fund = new Fund(client.IndividualPot);
                    List<ClientData> currentClientDataList = new List<ClientData>();
                    currentClientDataList.Add(currentClientData);
                    for (int i = 0; i <= NumberOfYears.ItemValue; i++)
                    {
                        fund.AddContributions(currentClientDataList, i);
                        currentDrawdownList.Add(fund.TakeDrawDown(currentClientDataList, i));
                        currentFundList.Add(fund.FundValue);
                    }
                    FundList.Add(currentFundList);
                    DrawdownList.Add(currentDrawdownList);
                }
            }
            if (PotMethod == PotMethodEnum.Combined)
            {
                List<decimal> combinedFundList = new List<decimal>();
                List<decimal> combinedDrawdownList = new List<decimal>();
                Fund fund = new Fund(CombinedPot);
                for (int i = 0; i <= NumberOfYears.ItemValue; i++)
                {
                    fund.AddContributions(clientDatas, i);
                    combinedDrawdownList.Add(fund.TakeDrawDown(clientDatas, i));
                    combinedFundList.Add(fund.FundValue);
                }
                FundList.Add(combinedFundList);
                DrawdownList.Add(combinedDrawdownList);
            }

            List<decimal> TotalFundList = new List<decimal>();
            List<decimal> TotalDrawdownList = new List<decimal>();

            for (int i = 0; i <= NumberOfYears.ItemValue; i++)
            {
                TotalFundList.Add(0.0M);
                foreach(List<decimal> item in FundList)
                {
                    TotalFundList[i] += item[i];
                }

                TotalDrawdownList.Add(0.0M);
                foreach (List<decimal> item in DrawdownList)
                {
                    TotalDrawdownList[i] += item[i];
                }
            }
            
            dataForGraph.AddSeries("Total Drawdown",TotalDrawdownList);

            dataForGraph.AddSeries("Total Fund Value", TotalFundList);

            return dataForGraph;

        }

    }
}
