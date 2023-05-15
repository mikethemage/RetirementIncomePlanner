namespace RetirementIncomePlannerLogic
{
    public class DataInputModel
    {
        public required int NumberOfYears { get; set; } = 35;
        public int NumberOfClients
        {
            get
            {
                return Clients.Count;
            }

            set
            {
                while (value > Clients.Count)
                {
                    AddClient();
                }

                while (value < Clients.Count && Clients.Count > 1)
                {
                    RemoveClient();
                }
            }
        }

        public required decimal Indexation { get; set; } = 0.02M;

        public required decimal RetirementPot { get; set; } = 0M;
        public required decimal InvestmentGrowth { get; set; } = 0.03M;

        public required List<ClientInputModel> Clients { get; set; } = new List<ClientInputModel>();

        public DataInputModel()
        {
            //Clients.Add(new ClientInputModel());
        }

        public void AddClient()
        {
            Clients.Add(new ClientInputModel
            {
                ClientNumber = Clients.Count + 1,
                Age = 0,
                RetirementAge = 0,
                StatePensionAmount = 0.0M,
                StatePensionAge = 0,
                RetirementIncomeLevel=0.0M
            });
        }

        public void RemoveClient()
        {
            if (Clients.Count > 1)
            {
                Clients.Remove(Clients[^1]);
            }
        }
    }


}