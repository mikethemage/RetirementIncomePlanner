namespace RetirementIncomePlannerLogic
{
    public class DataInputModel
    {
        public int NumberOfYears { get; set; } = 35;
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

        public decimal Indexation { get; set; } = 0.02M;

        public decimal RetirementPot { get; set; } = 0M;
        public decimal InvestmentGrowth { get; set; } = 0.03M;

        public List<ClientInputModel> Clients { get; set; } = new List<ClientInputModel>();

        public DataInputModel()
        {
            //Clients.Add(new ClientInputModel());
        }

        public void AddClient()
        {
            Clients.Add(new ClientInputModel
            {
                ClientNumber = Clients.Count + 1
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