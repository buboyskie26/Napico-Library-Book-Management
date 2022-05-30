using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Data.Model
{
    public class LibraryTopic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string ImageUrl { get; set; }

        public virtual IEnumerable<LibraryAsset> LibraryAssets { get; set; }
        public virtual IEnumerable<CheckoutHistory> CheckoutHistory { get; set; }
    }
}
