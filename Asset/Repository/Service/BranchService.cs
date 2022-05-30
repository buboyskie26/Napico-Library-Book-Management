using Asset.Data;
using Asset.Data.Model;
using Asset.Repository.Contract;
using Asset.ViewModel.BranchVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Service
{
    public class BranchService : IBranch
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _web;

        public BranchService(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _context = applicationDbContext;
            _web = webHostEnvironment;
        }

        public async Task AddLibBranch(BranchCreateIndex p)
        {
            var branch = new LibraryBranch
            {
                Name=p.Name,
                Telephone=p.Telephone,
                Address=p.Address,
                Description=p.Description,
                OpenDate=p.OpenDate,
                
            };
            if(p.ImageUrl != null&& p.ImageUrl.Length > 0)
            {
                var imageRoute = @"images/employees";
                var fileName = Path.GetFileNameWithoutExtension(p.ImageUrl.FileName);
                var extension = Path.GetExtension(p.ImageUrl.FileName);
                var webroot = _web.WebRootPath;

                fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;

                var path = Path.Combine(webroot, imageRoute, fileName);

                await p.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                branch.ImageUrl = "/" + imageRoute + "/" + fileName;
            }

            await _context.LibraryBranches.AddAsync(branch);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<LibraryBranch>> GetAllBranch()
        {
            return await _context.LibraryBranches
                .Include(q => q.LibraryAssets)
                .ToListAsync();
        }

        public async Task<LibraryBranch> GetBranchById(int id)
        {
            var branch = await GetAllBranch();

            return branch.FirstOrDefault(q => q.Id == id);
        }

        public IEnumerable<string> GetBranchHours(int branchId)
        {
            var branchHours = _context.BranchHours
                .Include(q => q.Branch)
                .Where(q => q.Branch.Id == branchId);

            var output = DataHelper.HumanizeBusinessHours(branchHours);
            
            return output;

        }

        public bool IsBranchOpen(int branchId)
        {

            var HoursNow = DateTime.Now.Hour;

            var daysOfWeek = (int)DateTime.Now.DayOfWeek + 1;

            var branchHours = _context.BranchHours
                .Include(q => q.Branch)
                .Where(q => q.Branch.Id == branchId);

            var branchDaysOfWeek = branchHours.FirstOrDefault(q => q.DaysOfWeek == daysOfWeek);

            if (branchDaysOfWeek == null) return false;

            return HoursNow > branchDaysOfWeek.OpenTime && HoursNow < branchDaysOfWeek.CloseTime;

        }

        public async Task UpdateLibBranch(BranchCreateIndex p)
        {
            var o = await GetBranchById(p.Id);

            if(o != null)
            {
                o.Address = p.Address;
                o.Description = p.Description;
                o.Name = p.Name;
                o.OpenDate = p.OpenDate;
                o.Telephone = p.Telephone;
            }
            if (p.ImageUrl != null && p.ImageUrl.Length > 0)
            {
                var imageRoute = @"images/employees";
                var fileName = Path.GetFileNameWithoutExtension(p.ImageUrl.FileName);
                var extension = Path.GetExtension(p.ImageUrl.FileName);
                var webroot = _web.WebRootPath;

                fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;

                var path = Path.Combine(webroot, imageRoute, fileName);

                await p.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                o.ImageUrl = "/" + imageRoute + "/" + fileName;
            }


            _context.LibraryBranches.Update(o);
            await _context.SaveChangesAsync();

        }
    }
}
