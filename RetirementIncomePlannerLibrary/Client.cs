using System.Collections.Generic;

namespace RetirementIncomePlannerLibrary
{
    public class Client
    {
        public const int DefaultAge = 60;
        public string ClientName { get; set; } = string.Empty;
        public AgeValue CurrentAge { get; set; } = new AgeValue();
        public AmountValue RetirementIncomeLevel { get; set; } = new AmountValue();
        public List<Employment> EmploymentList { get; set; } = new List<Employment>();
        public List<Pension> PensionList { get; set; } = new List<Pension>();
        public List<OtherIncome> OtherIncomeList { get; set; } = new List<OtherIncome>();
        public RetirementPot IndividualPot { get; set; } = new RetirementPot();
        public List<AdhocContribution> AdhocContributionList { get; set; } = new List<AdhocContribution>();

        public Client(string clientName, PotMethodEnum potMethod = PotMethodEnum.Combined)
        {
            CurrentAge.ItemValue = DefaultAge;

            ClientName = clientName;

            Employment defaultEmployment = new Employment();
            EmploymentList.Add(defaultEmployment);

            Pension defaultStatePension = new Pension("State Pension");
            PensionList.Add(defaultStatePension);

            Pension defaultOtherPension = new Pension("Other Pension");
            PensionList.Add(defaultOtherPension);

            OtherIncome defaultOtherIncome = new OtherIncome("Other Income");
            OtherIncomeList.Add(defaultOtherIncome);

            if (potMethod == PotMethodEnum.Individual)
            {
                IndividualPot = new RetirementPot { PotName = $"{ClientName} Retirement Pot" };

                
            }

        }

        public ClientData GetClientData(int maxYear, decimal indexation)
        {
            ClientData clientData = new ClientData();

            if (PensionList == null)
            {
                clientData.HasStatePensions = false;
                clientData.HasOtherPensions = false;
            }
            else
            {
                foreach (Pension pension in PensionList)
                {
                    if (pension.PensionType == "State Pension")
                    {
                        clientData.HasStatePensions = true;
                    }
                    else
                    {
                        clientData.HasOtherPensions = true;
                    }

                    if (clientData.HasStatePensions == true && clientData.HasOtherPensions == true)
                    {
                        break;
                    }
                }
            }

            if (EmploymentList == null)
            {
                clientData.HasSalary = false;
            }
            else
            {
                foreach (Employment employment in EmploymentList)
                {
                    if (employment.Salary.ValuePresent == true && employment.RetirementAge.ValuePresent == true)
                    {
                        clientData.HasSalary = true;
                        break;
                    }
                }
            }

            if (OtherIncomeList == null)
            {
                clientData.HasOtherIncome = false;
            }
            else
            {
                foreach (OtherIncome otherIncome in OtherIncomeList)
                {
                    if (otherIncome.IncomeAmount.ValuePresent == true)
                    {
                        clientData.HasOtherIncome = true;
                        break;
                    }
                }
            }

            if (AdhocContributionList == null)
            {
                clientData.HasContributions = false;
            }
            else
            {
                foreach (AdhocContribution adhocContribution in AdhocContributionList)
                {
                    if (adhocContribution.Age.ValuePresent == true && adhocContribution.AdhocAmount.ValuePresent == true)
                    {
                        clientData.HasContributions = true;
                        break;
                    }
                }
            }


            decimal currentIndexation;
            for (int currentYear = 0; currentYear <= maxYear; currentYear++)
            {
                if (currentYear == 0)
                {
                    currentIndexation = 1.0M;
                }
                else
                {
                    currentIndexation = (1.0M + indexation) * currentYear;
                }

                int ageForCurrentYear = CurrentAge.ItemValue + currentYear;
                clientData.ClientAge.Add(ageForCurrentYear);

                decimal totalStatePension = 0.0M;
                decimal totalOtherPension = 0.0M;
                decimal totalSalary = 0.0M;
                decimal totalOtherIncome = 0.0M;
                decimal totalContribution = 0.0M;
                decimal requiredDrawdown = 0.0M;

                if (RetirementIncomeLevel.ValuePresent)
                {
                    requiredDrawdown = (RetirementIncomeLevel.ItemValue * currentIndexation);
                }

                if (clientData.HasStatePensions)
                {
                    foreach (Pension pension in PensionList)
                    {
                        if (pension.PensionType == "State Pension")
                        {
                            totalStatePension += (pension.GetPensionForAge(ageForCurrentYear) * currentIndexation);
                        }
                    }
                    clientData.StatePensions.Add(totalStatePension);
                    requiredDrawdown -= totalStatePension;
                }

                if (clientData.HasOtherPensions)
                {
                    foreach (Pension pension in PensionList)
                    {
                        if (pension.PensionType == "Other Pension")
                        {
                            totalOtherPension += (pension.GetPensionForAge(ageForCurrentYear) * currentIndexation);
                        }
                    }
                    clientData.OtherPensions.Add(totalOtherPension);
                    requiredDrawdown -= totalOtherPension;
                }

                if (clientData.HasSalary)
                {
                    foreach (Employment employment in EmploymentList)
                    {
                        totalSalary += (employment.GetSalaryForAge(ageForCurrentYear)* currentIndexation);
                    }
                    clientData.Salary.Add(totalSalary);
                    requiredDrawdown -= totalSalary;
                }

                if (clientData.HasOtherIncome)
                {
                    foreach (OtherIncome otherIncome in OtherIncomeList)
                    {
                        if (otherIncome.IncomeAmount.ValuePresent)
                        {
                            totalOtherIncome += (otherIncome.IncomeAmount.ItemValue * currentIndexation);
                        }
                    }
                    clientData.OtherIncome.Add(totalOtherIncome);
                    requiredDrawdown -= totalOtherIncome;
                }

                if (clientData.HasContributions)
                {
                    foreach (AdhocContribution contribution in AdhocContributionList)
                    {
                        totalContribution += contribution.GetContributionForAge(ageForCurrentYear);
                    }
                    clientData.Contributions.Add(totalContribution);
                    requiredDrawdown -= totalContribution;
                }
                                
                if(requiredDrawdown < 0)
                {
                    requiredDrawdown = 0;
                }
                clientData.RequiredDrawdown.Add(requiredDrawdown);
            }


            return clientData;
        }
    }

}
