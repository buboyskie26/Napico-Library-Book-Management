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
    public class PatronService:IPatron
    {
        private readonly ApplicationDbContext _context;

        public PatronService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<IEnumerable<Patron>> GetAllPatron()
        {
            return await _context.Patrons
                .Include(q=> q.LibraryCard)
                .ToListAsync();
        }
        public async Task<Patron> GetPatronById(string id)
        {
            var obj = await GetAllPatron();
            return obj.FirstOrDefault(q => q.Id == id);
        }
        public async Task UpdatePatron(Patron id)
        {
            _context.Update(id);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Checkout>> GetCheckoutsPatron(string id)
        {
            var patron = await GetPatronById(id);

            var cardId = patron.LibraryCard.Id;

            return _context.Checkouts
                .Include(q=>q.LibraryCard)
                .Include(q=>q.LibraryAsset)
                .Where(q => q.LibraryCard.Id == cardId);
        }

        public async Task<IEnumerable<Hold>> GetHoldsPatron(string id)
        {
            var patron = await GetPatronById(id);

            var cardId = patron.LibraryCard.Id;

            return _context.Holds
                .Include(q => q.LibraryCard)
                .Include(q => q.LibraryAsset)
                .Where(q => q.LibraryCard.Id == cardId);

        }

        public async Task<IEnumerable<CheckoutHistory>> GetCheckoutHistoryPatron(string id)
        {
            var patron = await GetPatronById(id);

            var cardId = patron.LibraryCard.Id;

            return _context.CheckoutHistory
                .Include(q => q.LibraryCard)
                .Include(q => q.LibraryAsset)
                .Where(q => q.LibraryCard.Id == cardId);

        }

        public bool IsCheckoutBeyond(string userId)
        {
            var HoursNow = DateTime.Now;
      
            var user = _context.Patrons
                .FirstOrDefault(q=> q.Id==userId);

            bool asd = user.CheckoutSpan != new DateTime();
            if (asd)
            {
                if (HoursNow >= user.CheckoutSpan)
                {
                    return true;
                }
            }
            return false;
        }

        public int CheckoutPenalty(string userId)
        {
            var user = _context.Patrons
                .Include(q=> q.LibraryCard)
                .FirstOrDefault(q => q.Id == userId);

            var cardId = user.LibraryCard.Id;

            var penaltyObj = _context.CheckoutPenalties
                .Include(q => q.LibraryCard)
                .Include(q => q.LibraryAsset)
                .FirstOrDefault(q => q.LibraryCard.Id == cardId);

            // 8:30 
            // now = 9:30 ++
            // now = 10:30 ++
            var now = DateTime.Now;
 
            // 8:30  8:45 
            // now == 8:45
            // if 8:45 and the user doesnt checkedout the book
            // penalty comes in.
            //
            // datetime penalty will be run
            // if the penalty datetime is greater than 1 day to checkoutspan datetime 
            // penalty count will be added + 1
            // 
            // if the penalty datetime is greater than 2 day to checkoutspan datetime 
            // penalty count will be added + 1


            bool diff = now >= user.CheckoutSpan && user.isReturn == null;

            if (diff)
            {
                // it will run
                user.Penalty = now;
                user.isReturn = false;

             /*   penaltyObj.PenaltyDate = user.Penalty;
                penaltyObj.PenaltyCheckoutSpan = user.CheckoutSpan;

                _context.CheckoutPenalties.Update(penaltyObj);*/
                _context.Patrons.Update(user);
            }

            var sum = Math.Round((now - user.CheckoutSpan).TotalMinutes);
            if (user.CheckoutSpan == new DateTime()) return 0;
            // If the user returned the checkoud book.
            // get the total minutes
            if (user.Penalty == new DateTime() && user.isReturn == true)
            {
                return 0;
            }

            _context.SaveChanges();

            return (int)sum;
           /* if(diff && user.isReturn == null)
            {
                return (int)sum;
            }
            return 0;*/


            // Once you checked in the book,
            // The remaining penalty per minutes will be stored, that aligned to that book name
            // The admin can update the penalty of the user


        }

        public bool AssetBeenCheckedout(string userId)
        {
            var obj = _context.Patrons.Include(q => q.LibraryCard)
                 .FirstOrDefault(q => q.Id == userId);

           return obj.IsAssetCheckout == true;
        }

        public async Task<IEnumerable<Patron>> SinglePatron(string name)
        {
            var obj =await GetAllPatron();
            return obj.Where(q => q.FirstName.ToLower().Contains(name.ToLower()) ||
                q.LastName.ToLower().Contains(name.ToLower()));
        }
    }
}
