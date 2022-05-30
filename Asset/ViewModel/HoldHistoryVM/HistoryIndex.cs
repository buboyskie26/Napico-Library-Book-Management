using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModel.HoldHistoryVM
{
    public class HistoryIndex
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int Notification { get; set; }
        public bool IsRead { get; set; }
        public string AssetName { get; set; }
        public string PreviousUserName { get; set; }
        public int CardId { get; set; }
        public int Notif { get; set; }
        public DateTime HoldStart { get; set; }
        public DateTime HoldEnd { get; set; }
 
    }
    public class HoldIndexList
    {
        public IEnumerable<HistoryIndex> HistoryList{ get; set; }
    }
}
