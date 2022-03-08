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
        public List<Client> ClientList { get; set; } = new List<Client>();
        public PercentageValue Indexation { get; set; } = new PercentageValue();
        public PotMethodEnum PotMethod { get; set; } = PotMethodEnum.Combined;
        public List<RetirementPot> CombinedPotList { get; set; } = new List<RetirementPot>();

        public IncomePlannerModel(PotMethodEnum potMethod = PotMethodEnum.Combined, int IntialNumberOfClients = 1)
        {
            NumberOfYears.ItemValue = DefaultNumberOfYears;

            NumberOfClients = IntialNumberOfClients;

            for (int i = 0; i < NumberOfClients; i++)
            {
                ClientList.Add(new Client($"Client {i}", potMethod));
            }

            Indexation.ItemValue = DefaultIndexation;

            PotMethod = potMethod;

            if (PotMethod == PotMethodEnum.Combined)
            {
                RetirementPot DefaultPot = new RetirementPot { PotName = "Combined Retirement Pot" };

                CombinedPotList.Add(DefaultPot);
            }
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
    }

}
