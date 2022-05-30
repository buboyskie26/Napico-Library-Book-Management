using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Data.Model
{
    public class LibraryAsset
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
        public int StatusId { get; set; }
        [Required]
        [Display(Name = "Cost of Replacement")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
        public int ReferenceCopies { get; set; }
        public string Description { get; set; }

        [ForeignKey("LocationId")]
        public LibraryBranch Location { get; set; }
        public int LocationId { get; set; }


        [ForeignKey("LibraryTopicId")]
        public LibraryTopic LibraryTopic { get; set; }
        public int LibraryTopicId { get; set; }


        public virtual IEnumerable<CheckoutHistory> CheckoutHistories { get; set; }

    }
}
