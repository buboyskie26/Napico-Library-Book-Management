using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Data.Model
{
    public class Checkout
    {
        public int Id { get; set; }
        public virtual LibraryAsset LibraryAsset { get; set; }
        public virtual LibraryCard LibraryCard { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
        public DateTime SpanTime { get; set; }

        [ForeignKey("UserPatronId")]
        public virtual Patron UserPatron { get; set; }
        public string UserPatronId { get; set; }


    }
}
