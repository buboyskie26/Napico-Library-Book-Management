using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Data.Model
{
    public class CheckoutPenalty
    {
        public int Id { get; set; }
        public virtual LibraryAsset LibraryAsset { get; set; }
        public virtual LibraryCard LibraryCard { get; set; }
        public DateTime PenaltyDate { get; set; }
        // deprecated
        public DateTime PenaltyCheckoutSpan { get; set; }
        public int AmountPenalty { get; set; }

        [ForeignKey("PatronId")]
        public virtual Patron Patron { get; set; }
        public string PatronId { get; set; }
    }
}
