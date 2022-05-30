using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModel.PenaltyVM
{
    public class PenaltyIndex
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public int CardId { get; set; }
        public DateTime PenaltyDate { get; set; }
        public DateTime SpanTime { get; set; }
        public int AmountPenalty { get; set; }
        public string Username { get; set; }
    }
    public class PenaltyListIndex
    {
        public IEnumerable<PenaltyIndex> PenaltyList { get; set; }
    }
}
