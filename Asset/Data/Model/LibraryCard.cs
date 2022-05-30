using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Data.Model
{
    public class LibraryCard
    {
        public int Id { get; set; }

        [Display(Name = "Overdue Fees")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Fees { get; set; }

        [Display(Name = "Card Issued Date")]
        public DateTime Created { get; set; }
        public virtual IEnumerable<Checkout> Checkout { get; set; }
    }
}
