using Asset.Data.Model;
using Asset.ViewModel.BranchVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Contract
{
    public interface IBranch
    {
        Task<IEnumerable<LibraryBranch>> GetAllBranch();
        Task AddLibBranch(BranchCreateIndex branchIndex);

        Task UpdateLibBranch(BranchCreateIndex branchIndex);
        Task<LibraryBranch> GetBranchById(int id);
        bool IsBranchOpen(int id);
        IEnumerable<string> GetBranchHours(int branchId);
    }
}
