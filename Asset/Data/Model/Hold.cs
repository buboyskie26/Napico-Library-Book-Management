using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Data.Model
{
    public class Hold
    {
        public int Id { get; set; }
        public bool? IsIssued { get; set; }
        public bool IsCheckedOut { get; set; }
        public virtual LibraryAsset LibraryAsset { get; set; }

        [ForeignKey("AdminId")]
        public virtual Patron Admin { get; set; }
        public string AdminId { get; set; }

        [ForeignKey("UserPatronId")]
        public virtual Patron UserPatron { get; set; }
        public string UserPatronId { get; set; }


        [ForeignKey("LibraryTopicId")]
        public virtual LibraryTopic LibraryTopic { get; set; }
        public int LibraryTopicId { get; set; }

  

        public virtual LibraryCard LibraryCard { get; set; }
        public DateTime HoldPlaced { get; set; }
    }
}
