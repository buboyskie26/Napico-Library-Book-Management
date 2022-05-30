using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Data.Model
{
    public class CheckoutRequest
    {
        public int Id { get; set; }
        public bool IsHit { get; set; }

        [ForeignKey("LibraryAssetId")]
        public virtual LibraryAsset LibraryAsset { get; set; }
        public int LibraryAssetId { get; set; }

/*        [ForeignKey("LibraryTopicId")]
        public virtual LibraryTopic LibraryTopic { get; set; }
        public int LibraryTopicId { get; set; }*/

        [ForeignKey("LibraryCardId")]
        public virtual LibraryCard LibraryCard { get; set; }
        public int LibraryCardId { get; set; }

        public DateTime Until { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime Start { get; set; }
        public bool? Approved { get; set; }


        [ForeignKey("AdminId")]
        public virtual Patron Admin { get; set; }
        public string AdminId { get; set; }


        [ForeignKey("UserPatronId")]
        public virtual Patron UserPatron { get; set; }
        public string UserPatronId { get; set; }
    }
}
