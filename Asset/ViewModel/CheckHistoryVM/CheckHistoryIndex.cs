using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModel.CheckHistoryVM
{
    public class CheckHistoryIndex
    {
        public int Id { get; set; }
        public string TopicName { get; set; }


        public DateTime CheckedOut { get; set; }

        // deprecated
        public DateTime SpanTime { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckInLostBook { get; set; }

      
        public string PatronId { get; set; }
        public bool? IsRecent { get; set; }
        public bool IsLost { get; set; }
 
        public int LibraryTopicId { get; set; }
    }
}
