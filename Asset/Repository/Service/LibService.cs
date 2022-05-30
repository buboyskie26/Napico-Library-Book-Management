using Asset.Data;
using Asset.Data.Model;
using Asset.Repository.Contract;
using Asset.ViewModel.AssetVM;
using Asset.ViewModel.DropDownVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Service
{
    public class LibService : ILibAsset
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _web;

        public LibService(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _context = applicationDbContext;
            _web = webHostEnvironment;
        }

        public async Task Add(LibraryAsset libraryAsset)
        {


            await _context.LibraryAssets.AddAsync(libraryAsset);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsset(CreateIndex k)
        {


            var obj = new LibraryAsset()
            {
                Title = k.Title,
                Description = k.Description,
                Cost = k.Cost,
                NumberOfCopies = k.NumberOfCopies,
                Year = k.Year,
                ReferenceCopies = k.NumberOfCopies,
                LocationId = k.LocationId,
                StatusId = k.StatusId,
                LibraryTopicId = k.LibraryTopicId
            };

            if (k.ImageUrl.Length > 0 && k.ImageUrl != null)
            {
                var imageRoute = @"images/employees";
                var fileName = Path.GetFileNameWithoutExtension(k.ImageUrl.FileName);
                var extension = Path.GetExtension(k.ImageUrl.FileName);
                var webroot = _web.WebRootPath;

                fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;

                var path = Path.Combine(webroot, imageRoute, fileName);

                await k.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                obj.ImageUrl = "/" + imageRoute + "/" + fileName;

            }



            await _context.LibraryAssets.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<LibraryAsset> GetAll()
        {
            var asd = _context.LibraryAssets
                .Include(q => q.Status)
                .Include(q => q.LibraryTopic)
                .Include(q => q.CheckoutHistories)
                .Include(q => q.CheckoutHistories).ThenInclude(q => q.Patron)
                .Include(q => q.CheckoutHistories).ThenInclude(q => q.LibraryAsset)
                .Include(q => q.CheckoutHistories).ThenInclude(q => q.LibraryCard);

            return asd;

        }
 
     
        public LibraryAsset GetById(int id)
        {
            return GetAll()
               .FirstOrDefault(q => q.Id == id);

        }

        public string GetType(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DropDown> ListDropdown()
        {
            var obj = new DropDown()
            {
                Statuses = await _context.Status.OrderBy(q=> q.Name).ToListAsync(),
                LibraryBranches = await _context.LibraryBranches.OrderBy(q=> q.Name).ToListAsync(),
                LibraryTopics = await _context.LibraryTopics.OrderBy(q=> q.Title).ToListAsync(),
            };
            return obj;
        }

        public IEnumerable<LibraryAsset> SingleLibrary(string search)
        {
            var asset = GetAll();

            return asset.Where(q => q.Title.ToLower().Contains(search.ToLower()) ||
            q.Description.ToLower().Contains(search.ToLower()));
        }

        public async Task UpdateAsset(CreateIndex p)
        {
            var o = GetById(p.Id);

            o.Title = p.Title;
            o.Description = p.Description;
            o.Cost = p.Cost;
            o.LocationId = p.LocationId;
            o.StatusId = p.StatusId;
            o.Year = p.Year;
            o.NumberOfCopies = p.NumberOfCopies;
            o.ReferenceCopies = o.NumberOfCopies;

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


            _context.LibraryAssets.Update(o);
            await _context.SaveChangesAsync();
        }
    }
}
