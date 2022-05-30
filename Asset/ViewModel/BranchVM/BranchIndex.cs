using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModel.BranchVM
{
    public class BranchIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }
        public IEnumerable<string> LibraryTime { get; set; }
        public bool IsLibraryOpened { get; set; }

        public string Description { get; set; }
        public int AssetNumber { get; set; }
        public string ImageUrl { get; set; }
        public DateTime OpenDate { get; set; }
    }
    public class BranchCreateIndex
    {
        [Display(Name = "Photo")]
        public IFormFile ImageUrl { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public IEnumerable<string> LibraryTime { get; set; }
        public bool IsLibraryOpened { get; set; }
        public string Description { get; set; }
        public int AssetNumber { get; set; }
        public DateTime OpenDate { get; set; }
    }
    public class BranchListIndex
    {
        public IEnumerable<BranchIndex> ListBranch { get; set; }
    }
}
