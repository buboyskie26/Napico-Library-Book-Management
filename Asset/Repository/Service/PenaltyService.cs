using Asset.Data;
using Asset.Data.Model;
using Asset.Repository.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Service
{
    public class PenaltyService:IPenalty
    {
        private readonly ApplicationDbContext _context;
        public PenaltyService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

      

        public IEnumerable<CheckoutPenalty> GetAllPenalty()
        {
            return _context.CheckoutPenalties
              .Include(q => q.LibraryAsset)
              .Include(q => q.LibraryCard)
              .Include(q => q.Patron)
              .ToList();

        }
        public CheckoutPenalty GetAllPenaltyById(int id)
        {
            return GetAllPenalty().FirstOrDefault(q => q.Id == id);
        }
    }
}
