using Asset.Data.Model;
using Asset.Repository.Contract;
using Asset.Repository.Contract.ApiContract;
using Asset.ViewModel.AssetVM;
using Asset.ViewModel.HoldHistoryVM;
using Asset.ViewModel.HoldVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Controllers
{
    public class HoldHistoryController : Controller
    {
        private readonly ILibAsset _asset;
        private readonly ICheckout _checkout;
        private readonly IHold _hold;
        private readonly UserManager<Patron> _userManager;
        private readonly ICheckoutApi _checkoutServ;

        // todo
        // create a dynamic number of copies
        // if the user cheked an item, minus - 1 and if item are checked in return back to originak
        // , if hold notthing to do
        public HoldHistoryController(ILibAsset libAsset, UserManager<Patron> user, IHold hold, ICheckoutApi checkoutApi,
            ICheckout checkout)
        {
            _checkoutServ = checkoutApi;
            _hold = hold;
            _checkout = checkout;
            _userManager = user;

            _asset = libAsset;
        }

        public async Task<IActionResult> PatronCheckoutList()
        {
           
            var userId = _userManager.GetUserId(User);
            var history = await _hold.GetAllCheckoutHistory(userId);

           
            var obj = new AssetIndex()
            {
                CheckoutHistories = history,
            };

            return View(obj);
        }
        public IActionResult ViewInCalendar()
        {
            var userId = _userManager.GetUserId(User);
            var obj = _checkoutServ.PatronCheckoutById(userId);

            return View(obj);
        }
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var hitory = _hold.GetAllHoldHistory();

            var userHistory = hitory.Where(q => q.UserPatronId == userId);

            var obj = userHistory.Select(p => new HistoryIndex()
            {
                AssetId = p.LibraryAsset.Id,
                AssetName=p.LibraryAsset.Title,
                CardId=p.LibraryCard.Id,
                HoldEnd=p.HoldEnd,
                IsRead = _hold.GetIsReadBool(userId),
                Notif = p.Notification,
                HoldStart=p.HoldStart,
                Id=p.Id,
                PreviousUserName = string.IsNullOrEmpty(p.UserPreviousId) ?  "Unknown" : p.UserPrevious.FirstName + " " + p.UserPrevious.LastName 
            });

            var model = new HoldIndexList()
            {
                HistoryList = obj
            };

            return View(model);
        }
 
        public async Task<IActionResult> MyListOfCheckout()
        {
            var userId = _userManager.GetUserId(User);
            var history = await _hold.GetAllCheckoutHistory(userId);
            history = history.ToList();

            var obj = history.Select(q => new CheckHistoryIndex
            {
                Id = q.Id,
                AssetId = q.LibraryAsset.Id,
                AssetName =q.LibraryAsset.Title,
                PatronName=q.Patron.FirstName+" "+q.Patron.LastName,
                TopicName=q.LibraryTopic.Title,
                CheckedOut=q.CheckedOut,
                CheckIn=q.CheckIn,
            });

            var model = new ListHoldIndex()
            {
                ListCheckout=obj
            };

            return View(model);
        }
        public async Task<IActionResult> AdminCheckoutView()
        {
            var history = await _hold.GetAllCheckoutHistory();
            history = history.ToList();

            var obj = history.Select(q => new CheckHistoryIndex
            {
                Id = q.Id,
                AssetId = q.LibraryAsset.Id,
                AssetName =q.LibraryAsset.Title,
                PatronName=q.Patron.FirstName+" "+q.Patron.LastName,
                TopicName=q.LibraryTopic.Title,
                CheckedOut=q.CheckedOut,
                CheckIn=q.CheckIn,
            });

            var model = new ListHoldIndex()
            {
                ListCheckout=obj
            };

            return View(model);
        }
        public async Task<IActionResult> ReserveUser()
        {
            var history = await _hold.GetAllCheckoutHistory();
            history = history.ToList();

            var obj = history.Select(q => new CheckHistoryIndex
            {
                Id = q.Id,
                AssetId = q.LibraryAsset.Id,
                AssetName = q.LibraryAsset.Title,
                PatronName = q.Patron.FirstName + " " + q.Patron.LastName,
                TopicName = q.LibraryTopic.Title,
                CheckedOut = q.CheckedOut,
                CheckIn = q.CheckIn,
            });

            var model = new ListHoldIndex()
            {
                ListCheckout = obj
            };

            return View(model);
        }


        public int GetNotif()
        {
            var userId = _userManager.GetUserId(User);
            return _hold.GetNotification(userId);

        }
    }
}
