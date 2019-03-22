using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.interfaces
{
    public interface IRepeatInfoFormat
    {
        RepeatInfoFormatModel FormatRepeatInfo(int quoteGroup, string identity);
    }

    public class RepeatInfoFormatModel
    {
        public int CompositeRepeatType { get; set; }
        public int RepeatType { get; set; }
        public string RepeatMsg { get; set; }
        public Dictionary<int, int> RepeatPerCompany { get; set; } 
    }
}