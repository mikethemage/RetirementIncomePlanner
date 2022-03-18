using System.Collections.Generic;

namespace RetirementIncomePlannerLibrary
{
    public class DataForGraph
    {
        public List<string> seriesHeaders { get; private set; } = new List<string>();
        public List<List<decimal>> seriesValues { get; private set; } = new List<List<decimal>>();

        public int seriesCount { get; private set; } = 0;

        public void AddSeries(string seriesName, List<decimal> series)
        {
            seriesHeaders.Add(seriesName);
            seriesValues.Add(series);
            seriesCount++;
        }
    }
}
