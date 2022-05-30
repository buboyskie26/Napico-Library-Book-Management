using Asset.Repository.Contract;
using Asset.ViewModel.BranchVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Controllers
{
    [Authorize]

    public class BranchController : Controller
    {
        private readonly IBranch _branch;
        public BranchController(IBranch branch)
        {
            _branch = branch;
        }

        public async Task<IActionResult> Index()
        {
            var branch = await _branch.GetAllBranch();
            var obj = branch.Select(q => new BranchIndex()
            {
                Address=q.Address,
                AssetNumber=q.LibraryAssets.Count(),
                Description=q.Description,
                Id=q.Id,
                OpenDate=q.OpenDate,
                Name=q.Name,
                Telephone=q.Telephone,
                ImageUrl=q.ImageUrl,
                IsLibraryOpened = _branch.IsBranchOpen(q.Id)
            });
            var model = new BranchListIndex()
            {
                ListBranch = obj
            };
            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var n = await _branch.GetBranchById(id);

            var model = new BranchIndex()
            {
                Address = n.Address,
                ImageUrl=n.ImageUrl,
                Id=n.Id,
                Name=n.Name,
                AssetNumber=n.LibraryAssets.Count(),
                Description=n.Description,
                OpenDate=n.OpenDate,
                Telephone=n.Telephone,
                LibraryTime = _branch.GetBranchHours(id)
            };
            
            return View(model);
        }

        public IActionResult CreateBranch()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBranch(BranchCreateIndex p)
        {
            await _branch.AddLibBranch(p);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateBranch(int id)
        {
            var o = await _branch.GetBranchById(id);

            var p = new BranchIndex
            {
                Name=o.Name,
                Address=o.Address,
                Description=o.Description,
                OpenDate=o.OpenDate,
                Telephone=o.Telephone,
                ImageUrl=o.ImageUrl,
            };

            return View(p);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBranch(BranchCreateIndex p)
        {
            await _branch.UpdateLibBranch(p);
            return RedirectToAction("Index");
        }
    }
}
