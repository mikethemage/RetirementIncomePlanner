namespace RetirementIncomePlannerLibrary
{
    public interface IFormattedValue
    {
        string Text { get; set; }
        bool ValuePresent { get; set; }
    }
}