using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Data.Model
{
    public class HoldHistory
    {
        [ForeignKey("LibraryTopicId")]
        public virtual LibraryTopic LibraryTopic { get; set; }
        public int LibraryTopicId { get; set; }
        public int Id { get; set; }
        public int Notification { get; set; }
        public bool IsRead { get; set; }
        public virtual LibraryAsset LibraryAsset { get; set; }
        public virtual LibraryCard LibraryCard { get; set; }
        public DateTime HoldStart { get; set; }
        public DateTime HoldEnd { get; set; }

        [ForeignKey("UserPatronId")]
        public virtual Patron UserPatron { get; set; }
        public string UserPatronId { get; set; }


        [ForeignKey("UserPreviousId")]
        public virtual Patron UserPrevious { get; set; }
        public string UserPreviousId { get; set; }
    }
}
