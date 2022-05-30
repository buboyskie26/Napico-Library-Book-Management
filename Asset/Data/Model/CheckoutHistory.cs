using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Data.Model
{
    public class CheckoutHistory
    {
        public int Id { get; set; }
        public virtual LibraryAsset LibraryAsset { get; set; }
        public virtual LibraryCard LibraryCard { get; set; }
        public DateTime CheckedOut { get; set; }

        // deprecated
        public DateTime SpanTime { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckInLostBook { get; set; }

        [ForeignKey("PatronId")]
        public virtual Patron Patron { get; set; }
        public string PatronId { get; set; }
        public bool? IsRecent { get; set; }
        public bool IsLost { get; set; }


        [ForeignKey("LibraryTopicId")]
        public virtual LibraryTopic LibraryTopic { get; set; }
        public int LibraryTopicId { get; set; }
    }
}
