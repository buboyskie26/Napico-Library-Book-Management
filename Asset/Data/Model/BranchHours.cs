using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Data.Model
{
    public class BranchHours
    {
        public int Id { get; set; }
        public LibraryBranch Branch { get; set; }
        [Range(0, 6, ErrorMessage = "Day of Week must be between 0 and 6")]
        public int DaysOfWeek { get; set; }
        [Range(0, 23, ErrorMessage = "Hour opened must be between 0 and 23")]
        public int OpenTime { get; set; }
        [Range(0, 23, ErrorMessage = "Hour closed must be between 0 and 23")]
        public int CloseTime { get; set; }

    }
}
