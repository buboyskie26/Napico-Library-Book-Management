using Asset.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModel.DropDownVM
{
    public class DropDown
    {
        public DropDown()
        {
            Statuses = new List<Status>();
            LibraryBranches = new List<LibraryBranch>();
            LibraryTopics = new List<LibraryTopic>();
        }
        public List<Status> Statuses { get; set; }
        public List<LibraryBranch> LibraryBranches { get; set; }
        public List<LibraryTopic> LibraryTopics { get; set; }
    }
}
