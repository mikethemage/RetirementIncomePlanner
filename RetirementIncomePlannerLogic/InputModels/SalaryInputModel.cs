namespace RetirementIncomePlannerLogic
{
    public class SalaryInputModel
    {
        public decimal FullSalaryAmount { get; set; }
        public AgeAmountInputModel? PartialRetirementDetails { get; set; } = null;
    }


}