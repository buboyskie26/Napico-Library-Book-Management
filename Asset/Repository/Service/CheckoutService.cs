using Asset.Data;
using Asset.Data.Model;
using Asset.Repository.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Service
{
    public class CheckoutService : ICheckout
    {
        private readonly ApplicationDbContext _context;
        private readonly DateTime now = DateTime.Now;
        private readonly IPatron _patron;

        public CheckoutService(ApplicationDbContext applicationDbContext, IPatron patron)
        {
            _context = applicationDbContext;
            _patron = patron;

            
        }

        public void Add(Checkout checkout)
        {
            _context.Checkouts.Add(checkout);
            _context.SaveChanges();
        }

        public IEnumerable<Checkout> GetAllCheckout(int id)
        {
  
            return _context.Checkouts
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Include(q => q.UserPatron)
                .Where(q=> q.LibraryAsset.Id == id);
        }

        public IEnumerable<CheckoutHistory> GetAllCheckoutHistory(int id,string userId)
        {
           /* var asset = _context.LibraryAssets
                .Include(q => q.Status)
                .Where(q => q.Id == id);*/

            return _context.CheckoutHistory
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Include(q => q.Patron)
                .Where(q=> q.LibraryAsset.Id == id)
                .Where(q=> q.Patron.Id == userId);
        }

        public Checkout GetCheckoutId(int id)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Hold> GetCurrentHold(int id)
        {

            return _context.Holds
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Where(q=> q.LibraryAsset.Id == id);
        }

        public void PlaceCheckin(int assetId, string userId, string patronName)
        {
            var asset = _context.LibraryAssets
              .Include(q => q.Status)
              .FirstOrDefault(q => q.Id == assetId);

            if(asset.ReferenceCopies < asset.NumberOfCopies)
                asset.ReferenceCopies += 1;

            _context.LibraryAssets.Update(asset);

            var checkout = _context.Checkouts
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Where(q=> q.UserPatronId == userId)
                .FirstOrDefault(q => q.LibraryAsset.Id == assetId);

            if (checkout != null)  _context.Remove(checkout);

            var cHistory = _context.CheckoutHistory
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Where(q => q.LibraryAsset.Id == assetId);

            foreach (var item in cHistory)
            {
                
                item.SpanTime = new DateTime();
                item.IsRecent = null;

                _context.Update(item);

            }

            // Find the login user match to the user who checkedout the book
           
            var checkoutHistory = _context.CheckoutHistory
               .Include(q => q.LibraryAsset)
               .Include(q => q.LibraryCard)
               .Where(q=> q.PatronId == userId)
               .FirstOrDefault(q => q.LibraryAsset.Id == assetId && q.CheckIn == null && q.CheckInLostBook == null);

            if (checkoutHistory != null)
            {
                checkoutHistory.CheckIn = now;
                checkoutHistory.SpanTime = now.AddSeconds(30);
                checkoutHistory.IsRecent = true;

                _context.Update(checkoutHistory);
            }



          /*  foreach (var item in history)
            {
                item.Notification += 1;
                _context.HoldHistories.Update(item);

            }*/

            // check if the hold user name is match to the log in user
            // if he want to checked in the book nad hold user name is matched to the log in user
            // update the holdEnd to now
            /*history.HoldEnd = DateTime.Now;*/

            // holds
            var holds = _context.Holds
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Where(q => q.LibraryAsset.Id == assetId);

            var holdDefault = holds.FirstOrDefault();

            var holdHistory = _context.HoldHistories
                 .Include(q => q.LibraryAsset)
                 .Include(q => q.LibraryCard)
                 .Include(q => q.UserPatron)
                 .Include(q => q.UserPrevious)
                 .FirstOrDefault(q => q.LibraryAsset.Id == assetId);

      
            // check the current holder the same with the login user
            // Chech the Current holder if he`s the login User
            // then remove the hold
            //
            // If you`re in the checkedout process and still you are in the
            // hold list, Automatically the hold list you`re in will be deleted 
            if (holds.Any() && PatronHoldName(holdDefault.Id) == patronName)
            {
                var holdObj = holds.OrderBy(q => q.HoldPlaced).FirstOrDefault();

                var cardId = holdObj.LibraryCard.Id;

                _context.Holds.Remove(holdObj);

                var holdHistoryUser = _context.HoldHistories
                       .Include(q => q.LibraryAsset)
                       .Include(q => q.LibraryCard)
                       .Include(q => q.UserPatron)
                       .Include(q => q.UserPrevious)
                       .Where(q=> q.UserPatronId == userId)
                       .FirstOrDefault(q => q.LibraryAsset.Id == assetId);

                holdHistoryUser.IsRead = true;

                _context.HoldHistories.Update(holdHistoryUser);

                _context.SaveChanges();

                if(holds.Count() >= 1)
                {

                    var nextToFirstHoldList = _context.CheckoutHistory
                        .Include(q => q.LibraryAsset)
                        .Include(q => q.LibraryCard)
                        .FirstOrDefault(q => q.LibraryAsset.Id == assetId && q.PatronId == userId);

                    var logInUser = userId;

                    var second = holds.FirstOrDefault();
                    second.IsIssued = true;

                    var holdHistoryObj = _context.HoldHistories
                       .Include(q => q.LibraryAsset)
                       .Include(q => q.LibraryCard)
                       .Include(q => q.UserPatron)
                       .FirstOrDefault(q => q.LibraryAsset.Id == assetId && q.UserPatronId == second.UserPatronId);

                    if (holdHistoryObj == null) return;

                    holdHistoryObj.Notification++;
                    holdHistoryObj.UserPreviousId = nextToFirstHoldList.PatronId;

                    _context.HoldHistories.Update(holdHistory);
                    _context.Holds.Update(second);
                    _context.SaveChanges();

                }
            }
            else if (holds.Any() && PatronHoldName(holdDefault.Id) != patronName)
            {

                holdDefault.IsIssued = true;

                holdHistory.Notification++;
                holdHistory.UserPreviousId = checkoutHistory.PatronId;

                _context.Holds.Update(holdDefault);
                _context.HoldHistories.Update(holdHistory);
                _context.SaveChanges();

            }

            var libcardId = checkout.LibraryCard?.Id;

            var patron = _context.Patrons
                .Include(q => q.LibraryCard)
                .FirstOrDefault(q => q.LibraryCard.Id == libcardId);

            // Once you checkout in
            // The user penalty will be store to the Penalty Object Amount
            var objPenalty = _context.CheckoutPenalties
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .FirstOrDefault(q => q.LibraryAsset.Id == assetId && q.AmountPenalty == 0);

            var patronPenalty = patron.Penalty;
            var patronCheckoutSpan = patron.CheckoutSpan;

            if (objPenalty != null)
            {
                objPenalty.AmountPenalty = _patron.CheckoutPenalty(userId);
                objPenalty.PenaltyDate = patronPenalty;
                objPenalty.PenaltyCheckoutSpan = patronCheckoutSpan;

                _context.CheckoutPenalties.Update(objPenalty);
                _context.SaveChanges();

            }
            // Patron checked in his checkedout book
            //
            if (patron != null)
            {
                patron.isReturn = true;
                patron.Penalty = new DateTime();
                patron.CheckoutSpan = new DateTime();
                patron.IsAssetCheckout = false;

                // All the count of penalty will be stored in Overdue Fee
                /*patron.OverDueFee = _patron.CheckoutPenalty(userId);*/

                _context.Patrons.Update(patron);
            }

            var request = _context.CheckoutRequests
              .Include(q => q.LibraryAsset)
              .Include(q => q.LibraryCard)
              .Where(q => q.UserPatron.Id == userId)
              .FirstOrDefault(q => q.LibraryAsset.Id == assetId && q.IsHit == false);

            // If the user Checkout an book, the checkoutrequest object will be changed.
            if (request != null)
            {
                request.IsHit = true;
                // Make sure it is equivalent.
                request.Until = checkout.SpanTime;

                _context.CheckoutRequests.Update(request);

            }

            UpdateAssetStatus(asset, "Available");

            _context.SaveChanges();
        }

        public void PlaceCheckLost(int assetId, string userId, string patronName)
        {
            var checkout = _context.Checkouts
             .Include(q => q.LibraryAsset)
             .Include(q => q.LibraryCard)
             .Where(q => q.UserPatronId == userId)
             .FirstOrDefault(q => q.LibraryAsset.Id == assetId);

            if (checkout != null) _context.Remove(checkout);
            // Find the login user match to the user who checkedout the book
            var checkoutHistory = _context.CheckoutHistory
               .Include(q => q.LibraryAsset)
               .Include(q => q.LibraryCard)
               .Where(q => q.PatronId == userId)
               .FirstOrDefault(q => q.LibraryAsset.Id == assetId && q.CheckIn == null && q.CheckInLostBook == null);


            var holds = _context.Holds
              .Include(q => q.LibraryAsset)
              .Include(q => q.LibraryCard)
              .Where(q => q.LibraryAsset.Id == assetId);

            var holdDefault = holds.FirstOrDefault();

            if (holds.Any() && PatronHoldName(holdDefault.Id) == patronName)
            {
                var holdObj = holds.OrderBy(q => q.HoldPlaced).FirstOrDefault();

                var cardId = holdObj.LibraryCard.Id;

                _context.Holds.Remove(holdObj);

                _context.SaveChanges();

                /*   PlaceCheckout(assetId, cardId, userId);*/
                /* return;*/
            }


            if (checkoutHistory != null)
            {
                checkoutHistory.IsLost = true;

                _context.Update(checkoutHistory);
            }
            _context.SaveChanges();
        }

        public void PlaceMarkAsFound(int assetId, string userId, string patronName)
        {
            var asset = _context.LibraryAssets
                .Include(q => q.Status)
                .FirstOrDefault(q => q.Id == assetId);

            _context.LibraryAssets.Update(asset);
            // Find the login user match to the user who checkedout the book
            var checkoutHistory = _context.CheckoutHistory
               .Include(q => q.LibraryAsset)
               .Include(q => q.LibraryCard)
               .Where(q => q.PatronId == userId)
               .FirstOrDefault(q => q.LibraryAsset.Id == assetId && q.CheckInLostBook == null);

            if (checkoutHistory != null)
            {
                checkoutHistory.CheckInLostBook = now;
                checkoutHistory.IsLost = false;

                _context.CheckoutHistory.Update(checkoutHistory);
            }


            if (asset.ReferenceCopies == 0)
            {
                asset.Status = _context.Status.FirstOrDefault(q => q.Name == "Available");
            }
            asset.ReferenceCopies++;


          

            _context.SaveChanges();
        }

        public void PlaceCheckout(int assetId, int libraryCardId, string userId, int topicId)
        {
            var asset = _context.LibraryAssets
                .Include(q => q.Status)
                .FirstOrDefault(q => q.Id == assetId);


            // Borrow copies logic.

            /*            int reference = asset.ReferenceCopies;
                        reference = asset.NumberOfCopies;*/

            // Use IsCheckoutAsset as VIP card to checkedout n checkedin the book.
            asset.ReferenceCopies--;



            _context.LibraryAssets.Update(asset);


            var libraryCards = _context.LibraryCards
                  .Include(q => q.Checkout)
                  .FirstOrDefault(q => q.Id == libraryCardId);

            var checkout = new Checkout()
            {
                LibraryAsset = asset,
                LibraryCard = libraryCards,
                Since = now,
                Until = now.AddDays(2),
                SpanTime = now.AddMinutes(1),
                UserPatronId = userId,
            };
            var penaltyObj = new CheckoutPenalty()
            {
                LibraryAsset = asset,
                LibraryCard = libraryCards,
                PatronId = userId
            };
            var patron = _context.Patrons
                    .Include(q => q.LibraryCard)
                    .FirstOrDefault(q => q.LibraryCard.Id == libraryCardId);

            // Patron 

            if (patron != null)
            {
                patron.CheckoutSpan = checkout.SpanTime;
                patron.isReturn = null;
                patron.IsAssetCheckout = true;
                _context.Patrons.Update(patron);

            }

            _context.Add(checkout);
            _context.CheckoutPenalties.Add(penaltyObj);

            var checkoutHistory = new CheckoutHistory()
            {
                LibraryAsset = asset,
                LibraryCard = libraryCards,
                CheckedOut = now,
                PatronId = userId,
                LibraryTopicId = topicId
            };

        

           /* var request = _context.CheckoutRequests
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Where(q=> q.UserPatron.Id == userId)
                .FirstOrDefault(q => q.LibraryAsset.Id == assetId && q.IsHit == false);

            // If the user Checkout an book, the checkoutrequest object will be changed.
            if(request != null)
            {
                request.IsHit = true;
                // Make sure it is equivalent.
                request.Until = checkout.SpanTime;

                _context.CheckoutRequests.Update(request);

            }*/


            if(asset.ReferenceCopies == 0)
            {
                asset.Status = _context.Status.FirstOrDefault(q => q.Name == "Checked Out");

            }
             

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }
        public void PlaceHold(int assetId, int libraryCardId, string userId)
        {
            var asset = _context.LibraryAssets
              .Include(q => q.Status)
              .Include(q => q.LibraryTopic)
              .FirstOrDefault(q => q.Id == assetId);

            var topicId = asset.LibraryTopicId;

            _context.Update(asset);

            var libraryCards = _context.LibraryCards
                  .Include(q => q.Checkout)
                  .FirstOrDefault(q => q.Id == libraryCardId);

            var hold = new Hold()
            {
                LibraryAsset = asset,
                LibraryCard = libraryCards,
                HoldPlaced = now,
                IsCheckedOut = false,
                UserPatronId = userId,
                LibraryTopicId=topicId
            };

            var history = new HoldHistory()
            {
                LibraryAsset = asset,
                LibraryCard = libraryCards,
                HoldStart = hold.HoldPlaced,
                IsRead = false,
                UserPatronId = userId,
                LibraryTopicId = topicId
            };

            _context.HoldHistories.Add(history);
            _context.Holds.Add(hold);
            _context.SaveChanges();
        }

        private void UpdateAssetStatus(LibraryAsset asset, string v)
        {

            var assetf = asset.Status = _context.Status.FirstOrDefault(q => q.Name == v);
            _context.Update(assetf);
        }

        public DateTime HoldDate(int assetId)
        {
            return _context.Holds
               .Include(q => q.LibraryCard)
               .Include(q => q.LibraryAsset)
               .FirstOrDefault(q => q.Id == assetId).HoldPlaced;
        }

        public string PatronHoldName(int id)
        {
            var obj = _context.Holds
            .Include(q => q.LibraryCard)
            .Include(q => q.LibraryAsset)
            .FirstOrDefault(q => q.Id == id);

            if (obj == null) return "Unknown";
            var cardId = obj.LibraryCard?.Id;

            var patron = _context.Patrons
                .Include(q=> q.LibraryCard)
                .FirstOrDefault(q => q.LibraryCard.Id == cardId);

            return patron.FirstName + " " + patron.LastName;
        }

        public int UserCardId(string userId)
        {
 
            var pat = _context.Patrons
                .Where(q=> q.Id == userId)
                .Include(q => q.LibraryCard)
                .Select(q => q.LibraryCard.Id).FirstOrDefault();

            return pat;
        }

        public async Task AddRequest(CheckoutRequest request)
        {
            await _context.CheckoutRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CheckoutRequest>> GetAllRequest()
        {
            return await _context.CheckoutRequests
               .Include(q => q.LibraryAsset).ThenInclude(q => q.LibraryTopic)
               .Include(q => q.LibraryCard)
               .Include(q => q.UserPatron)
               .Include(q => q.Admin)
               .OrderByDescending(q=> q.LibraryAsset.Title)
               .ToListAsync();
        }

        public async Task UpdateRequest(CheckoutRequest history)
        {
            _context.CheckoutRequests.Update(history);
            await _context.SaveChangesAsync();

        }

        public async Task<CheckoutRequest> GetRequestId(int id)
        {
            return await _context.CheckoutRequests
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Include(q => q.UserPatron)
                .Include(q => q.Admin)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public bool HistoryCheckDateReturn(int id, string userId)
        {
            var obj = _context.CheckoutHistory
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Include(q => q.Patron)
                .Where(q => q.LibraryAsset.Id == id);

            bool valid = false;

            if(obj.Count() >= 1)
            {
                  valid = obj.Where(q => q.CheckIn == null).Any();

            }

            return valid;
        }

        public string CheckoutHistoryPatronName(int assetId)
        {

            /*var history =  _context.CheckoutHistory
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Include(q => q.Patron)
                .FirstOrDefault(q => q.LibraryAsset.Id == id);

            var hIstoryCardId = history.LibraryCard.Id;

            var libraryCardId = _context.LibraryCards
                .FirstOrDefault(q => q.Id == hIstoryCardId);

            var patron = _context.Patrons
               .Include(q => q.LibraryCard)
               .FirstOrDefault(q => q.LibraryCard.Id == libraryCardId.Id);

            return patron.FirstName + " " + patron.LastName;*/

            var checkout = _context.Checkouts
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .FirstOrDefault(q => q.LibraryAsset.Id == assetId);

            if (checkout == null) return "Unkown";

            var checkoutId = checkout.LibraryCard.Id;

            var patron = _context.Patrons
                .Include(q => q.LibraryCard)
                .FirstOrDefault(q => q.LibraryCard.Id == checkoutId);

            return patron.FirstName + " " + patron.LastName;

        }

        public string HoldPatronName(int id, string userId)
        {
            /*
                        var hold = _context.Holds
                          .Include(q => q.LibraryAsset)
                          .Include(q => q.LibraryCard)
                          .FirstOrDefault(q => q.LibraryAsset.Id == id);
            */


            /*    var checkout = _context.Checkouts
                 .Include(q => q.LibraryAsset)
                 .Include(q => q.LibraryCard)
                 .FirstOrDefault(q => q.LibraryAsset.Id == assetId);

                   if (checkout == null) return "Unkown";

                   var checkoutId = checkout.LibraryCard.Id;
            */


            var hold = _context.Holds
              .Include(q => q.LibraryAsset)
              .Include(q => q.LibraryCard)
              .Include(q=> q.UserPatron)
              .FirstOrDefault(q => q.LibraryAsset.Id == id && q.UserPatronId == userId);

            if (hold == null) return "No Holder On this Book";

            var holdId = hold.LibraryCard?.Id;

            var patron = _context.Patrons
                .Include(q => q.LibraryCard)
                .FirstOrDefault(q => q.LibraryCard.Id == holdId);

            return patron.FirstName + " " + patron.LastName;


        }

        public bool? HoldPatronStatus(int id)
        {
            var hold = _context.Holds
             .Include(q => q.LibraryAsset)
             .Include(q => q.LibraryCard)

             .Where(q => q.LibraryAsset.Id == id);

            if (hold.Any() == false) return null;

            var pbj = hold.FirstOrDefault();
            
            if (pbj.IsIssued == true)
            {
                return true;
            }
            else if (pbj.IsIssued == false)
                return false;
            else
                return null;    
 
        }

        public TimeSpan HistorySpan(int id)
        {
            var history = _context.Checkouts
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Where(q => q.LibraryAsset.Id == id);

            var obj = history.FirstOrDefault();
            if (obj == null) return new TimeSpan();

             
            var time =  obj.SpanTime - now;

            return time;
        }

        public int CurrentCountHoldName(int assetId)
        {
            var hold = _context.Holds
               .Include(q => q.LibraryAsset)
               .Include(q => q.LibraryCard)
               .FirstOrDefault(q => q.LibraryAsset.Id == assetId);

            if (hold == null) return 0;

            return 0;

        }

        public DateTime CheckInSpan(int id)
        {
   
            var history = _context.CheckoutHistory
              .Include(q => q.LibraryAsset)
              .Include(q => q.LibraryCard)
              .Where(q => q.LibraryAsset.Id == id && q.SpanTime == now.AddSeconds(30));

            var obj = history.FirstOrDefault();
            if (obj == null) return new DateTime();
 

            return obj.SpanTime;
        }

        private void BorrowBook(int assetId, string userId, string patronName)
        {
            var asset = _context.LibraryAssets
                .Include(q => q.Status)
                .FirstOrDefault(q => q.Id == assetId);

            int reference = asset.NumberOfCopies;

            // Get reference
            // once you checked out the book, the reference will throw to the View not the actual number of copies



        }
        public string GetCurrentCheckoutName(int assetId,string userId)
        {
            var checkout = _context.Checkouts
             .Include(q => q.LibraryAsset)
             .Include(q => q.LibraryCard)
             .Where(q=> q.UserPatronId == userId)
             .FirstOrDefault(q => q.LibraryAsset.Id == assetId);

            var checkouts = _context.Checkouts
            .Include(q => q.LibraryAsset)
            .Include(q => q.LibraryCard)
            .Where(q => q.LibraryAsset.Id == assetId);
 
            if (checkout == null) return "Unknown";

            var checkoutId = checkout.LibraryCard.Id;

            var patron = _context.Patrons
                .Include(q => q.LibraryCard)
                .FirstOrDefault(q => q.LibraryCard.Id == checkoutId);

            return patron.FirstName + " " + patron.LastName;
        }

        public List<string> CheckoutNames(int assetId)
        {
            var str = new List<string>();
            var checkouts = _context.Checkouts
              .Include(q => q.LibraryAsset)
              .Include(q => q.LibraryCard)
              .Where(q => q.LibraryAsset.Id == assetId);


            var obj = checkouts.Select(q => q.UserPatron).ToList();

            string name = "";
            foreach (var item in obj)
            {
                name = item.FirstName + " " + item.LastName;
                str.Add(name);
            }


            return str;

        }

        public int HoldPendingCount(int id)
        {
            throw new NotImplementedException();
        }

        public bool LostTheBook(int assetId, string userId)
        {

            var checkoutHistory = _context.CheckoutHistory
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Where(q => q.PatronId == userId)
                .Where(q => q.LibraryAsset.Id == assetId && q.IsLost==true).Any();

            return checkoutHistory;
        }

        public bool CheckedoutTheSingleAsset(int assetId, string userId)
        {
            var asd= _context.Checkouts
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Where(q => q.UserPatronId == userId)
                .Where(q => q.LibraryAsset.Id == assetId).Any();
            return asd;
        }

        public int CheckedoutLostCount(int assetId)
        {
            var asd = _context.CheckoutHistory
               .Include(q => q.LibraryAsset)
               .Include(q => q.LibraryCard)
               .Where(q => q.LibraryAsset.Id == assetId && q.IsLost == true).Count();

            return asd;
        }

        public int AssetOrigCopies(int assetId)
        {
            var asset = _context.LibraryAssets
               .Include(q => q.Location)
               .Where(q => q.Id == assetId);

            int number = 0;
            foreach (var item in asset)
            {
                number = item.NumberOfCopies;
            }

            return number;
        }

        public bool ReturnButWasLost(int assetId, string userId)
        {

            var checkoutHistory = _context.CheckoutHistory
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Where(q => q.PatronId == userId)
                .Where(q => q.CheckInLostBook != null)
                .Where(q => q.LibraryAsset.Id == assetId && q.IsLost == false).Any();
            return checkoutHistory;
        }

        public bool CheckedInlostBook(int assetId, string userId)
        {

            var checkoutHistory = _context.CheckoutHistory
               .Include(q => q.LibraryAsset)
               .Include(q => q.LibraryCard)
               .Where(q => q.PatronId == userId)
               .Where(q => q.CheckInLostBook == null)
               .Where(q => q.LibraryAsset.Id == assetId && q.IsLost == true).Any();
            return checkoutHistory;
        }

        public Hold FirstInHoldList(int assetId)
        {

            var checkout = _context.Checkouts
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .Where(q => q.LibraryAsset.Id == assetId);
            var hold = _context.Holds
                .Include(q => q.LibraryAsset)
                .Include(q => q.LibraryCard)
                .FirstOrDefault(q => q.LibraryAsset.Id == assetId);

            int c = checkout.Count();

            if (c == 0)
            {

                hold.IsIssued = true;
             
                _context.Holds.Update(hold);

            }



            return hold;
        }
 
    }

}
