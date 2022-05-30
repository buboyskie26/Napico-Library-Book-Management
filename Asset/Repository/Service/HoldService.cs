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
    public class HoldService : IHold
    {
        private readonly ApplicationDbContext _context;
        public HoldService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task AddHold(Hold hold)
        {
            await _context.Holds.AddAsync(hold);
            await _context.SaveChangesAsync();
        }

        public async Task AddHoldHistory(HoldHistory hold)
        {
            await _context.HoldHistories.AddAsync(hold);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CheckoutHistory>> GetAllCheckoutHistory(string userId)
        {
            return await _context.CheckoutHistory
                .Include(q => q.LibraryTopic)
                .Include(q => q.Patron)
                .Include(q => q.LibraryAsset)
                .Where(q=> q.PatronId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CheckoutHistory>> GetAllCheckoutHistory()
        {
            return await _context.CheckoutHistory
               .Include(q => q.LibraryTopic)
               .Include(q => q.Patron)
               .Include(q => q.LibraryAsset)
               .ToListAsync();
        }

        public IEnumerable<Hold> GetAllHold()
        {
            return _context.Holds
            .Include(q => q.LibraryAsset)
            .Include(q => q.LibraryTopic)
            .Include(q => q.LibraryCard);
        }

        public IEnumerable<HoldHistory> GetAllHoldHistory()
        {
            return _context.HoldHistories
              .Include(q => q.LibraryAsset)
              .Include(q => q.UserPatron)
              .Include(q => q.UserPrevious)
              .Include(q => q.LibraryCard)
              .ToList();
        }
        public async Task<IEnumerable<HoldHistory>> GetAllHoldHistoryAsync()
        {
            return await _context.HoldHistories
              .Include(q => q.LibraryAsset)
              .Include(q => q.UserPatron)
              .Include(q => q.UserPrevious)
              .Include(q => q.LibraryCard)
              .ToListAsync();
        }

        public async Task<IEnumerable<Hold>> GetAllPendingHold()
        {
            return await _context.Holds
               .Include(q => q.LibraryAsset)
               .Include(q => q.LibraryCard)
               .Where(q => q.IsIssued == null)
               .ToListAsync();
        }

        public async Task<HoldHistory> GetHoldHistoryId(int hold)
        {
            return await _context.HoldHistories
            .Include(q => q.LibraryAsset)
            .Include(q => q.UserPatron)
            .Include(q => q.LibraryCard)
            .FirstOrDefaultAsync(q => q.Id == hold);
        }

        public async Task<Hold> GetHoldId(int hold)
        {
            return await _context.Holds
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .FirstOrDefaultAsync(q => q.Id == hold);
        }

        public bool GetIsReadBool(string userId)
        {
            var history = GetAllHoldHistory();

            return history
                .Where(q => q.UserPatronId == userId)
                .Where(q => q.IsRead == true).Any();
        }

        public int GetNotification(string userId)
        {
            var history = GetAllHoldHistory();

            return history
                .Where(q=> q.UserPatronId == userId)
                .Where(q => q.IsRead == false).Count();
        }

        public string NotificationImage(string userId)
        {
            var history = GetAllHoldHistory();
            string name = "";
            if(history != null)
            {
                var asd = history
                   .Where(q => q.UserPatronId == userId);

                if(asd.FirstOrDefault(q=> q.UserPrevious != null) != null)
                {
                    name = asd.Select(q => q.UserPrevious.ImageUrl).FirstOrDefault();
                }
                else
                {
                    name = "Unknown";
                }
            }
          
            return name;
        }

        public string NotificationName(string userId)
        {
            var history = GetAllHoldHistory();
            string name = "";
            if (history != null)
            {
                var asd = history
                   .Where(q => q.UserPatronId == userId);

                if (asd.FirstOrDefault(q => q.UserPrevious != null) != null)
                {
                    name = asd.Select(q => q.UserPrevious.FirstName +" "+q.UserPrevious.LastName).FirstOrDefault();
                }
                else
                {
                    name = "Unknown";
                }
            }

            return name;
        }

        public async Task UpdateHold(Hold hold)
        {
            _context.Holds.Update(hold);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateHoldHistory(HoldHistory hold)
        {
            _context.HoldHistories.Update(hold);
            await _context.SaveChangesAsync();
        }
    }
}
