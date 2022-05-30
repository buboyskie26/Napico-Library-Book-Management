using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModel.HoldVM
{
    public class HoldIndex
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public string TopicName { get; set; }
        public int CardId { get; set; }
        public DateTime HoldIssued { get; set; }
        public bool? Issue { get; set; }
    }
    public class CheckHistoryIndex
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public DateTime CheckedOut { get; set; }
        // deprecated
        public DateTime SpanTime { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckInLostBook { get; set; }
        public string PatronName { get; set; }
        public bool? IsRecent { get; set; }
        public bool IsLost { get; set; }
        public string TopicName { get; set; }
        public string AssetName { get; set; }
    }
    public class ListHoldIndex
    {
        public IEnumerable<HoldIndex> ListIndex { get; set; }
        public IEnumerable<CheckHistoryIndex> ListCheckout { get; set; }

    }
}
