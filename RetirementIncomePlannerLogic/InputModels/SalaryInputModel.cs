namespace RetirementIncomePlannerLogic
{
    public class SalaryInputModel
    {
        public required decimal FullSalaryAmount { get; set; }
        public AgeAmountInputModel? PartialRetirementDetails { get; set; } = null;
    }


}