namespace RetirementIncomePlannerLibrary
{
    public class Employment
    {
        public string EmploymentName { get; set; } = string.Empty;
        public AmountValue Salary { get; set; } = new AmountValue();
        public AgeValue PartialRetirementAge { get; set; } = new AgeValue();
        public AmountValue PartialRetirementSalary { get; set; } = new AmountValue();
        public AgeValue RetirementAge { get; set; } = new AgeValue();

        public Employment()
        {

        }

        public decimal GetSalaryForAge(int age)
        {
            if(Salary.ValuePresent==false || RetirementAge.ValuePresent==false)
            {
                return 0.0M;
            }    
            else
            {
                if(PartialRetirementAge.ValuePresent==true && PartialRetirementSalary.ValuePresent==true)
                {
                    if(age < PartialRetirementAge.ItemValue)
                    {
                        return Salary.ItemValue;
                    }
                    else if(age < RetirementAge.ItemValue)
                    {
                        return PartialRetirementSalary.ItemValue;
                    }
                    else
                    {
                        return 0.0M;
                    }
                }
                else
                {
                    if (age < RetirementAge.ItemValue)
                    {
                        return Salary.ItemValue;
                    }
                    else
                    {
                        return 0.0M;
                    }
                }
            }
        }
    }
    
}
