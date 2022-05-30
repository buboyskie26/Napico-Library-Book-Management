using Asset.Data.Model;
using Asset.Repository.Contract;
using Asset.ViewModel.PatronVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Controllers
{
    [Authorize]
    public class PatronController : Controller
    {

        private readonly ILibAsset _asset;
        private readonly ICheckout _checkout;
        private readonly IHold _hold;
        private readonly IPatron _patron;
        private readonly UserManager<Patron> _userManager;

        // todo
        // create a dynamic number of copies
        // if the user cheked an item, minus - 1 and if item are checked in return back to originak
        // , if hold notthing to do
        public PatronController(ILibAsset libAsset, IPatron patron,
            UserManager<Patron> user, IHold hold,
            ICheckout checkout)
        {
            _patron = patron;
            _hold = hold;
            _checkout = checkout;
            _userManager = user;

            _asset = libAsset;
        }

        public async Task<IActionResult> Index()
        {
            var patron = await _patron.GetAllPatron();
            var obj = patron.Select(p => new PatronIndex()
            {
                LibraryCardId=p.LibraryCard.Id,
                Address=p.Address,
                FirstName=p.FirstName,
                LastName=p.LastName,
                ImageUrl=p.ImageUrl,
                Role=p.Role,
                Created=p.Created,
                Id = p.Id,
            });
            var model = new PatronListIndex()
            {
               ListPatron = obj 
            };
            return View(model);
        }
        public async Task<IActionResult> Detail(string id)
        {
            var p = await _patron.GetPatronById(id);
            var PatronCheckout = await _patron.GetCheckoutsPatron(id);
            var patronHold = await _patron.GetHoldsPatron(id);
            var patronCheckoutHistory = await _patron.GetCheckoutHistoryPatron(id);



            var model = new PatronIndex()
            {
                PenaltyCount = _patron.CheckoutPenalty(id),
                FirstName=p.FirstName,
                LastName=p.LastName,
                IsOverDue = _patron.IsCheckoutBeyond(id),
                Address=p.Address,
                Created=p.Created,
                Id=id,
                ImageUrl=p.ImageUrl,
                LibraryCardId=p.LibraryCard.Id,
                Role=p.Role,
                GetCheckoutWithPatron = PatronCheckout,
                GetCheckoutHistoryWithPatron= patronCheckoutHistory,
                GetHoldWithPatron = patronHold

            };

            return View(model);
        }
        public async Task<IActionResult> FinedPayment(string id)
        {
            var p = await _patron.GetPatronById(id);
            var count = _patron.CheckoutPenalty(id);

           /* p.OverDueFee = count;
            await _patron.UpdatePatron(p);*/
            return RedirectToAction("Detail", new { id });
        }


        public async Task<IActionResult> Results(string searchQuery)
        {
            var patron = await _patron.SinglePatron(searchQuery);

            var isSearchEmpty = patron.Any() == false || string.IsNullOrEmpty(searchQuery) ;

            var obj = patron.Select(p => new PatronIndex()
            {
                LibraryCardId = p.LibraryCard.Id,
                Address = p.Address,
                FirstName = p.FirstName,
                LastName = p.LastName,
                ImageUrl = p.ImageUrl,
                Role = p.Role,
                Created = p.Created,
                Id = p.Id,
            });

            var model = new PatronIndexSearch()
            {
                ListPatron=obj,
                SearchQuery=searchQuery,
                IsEmpty=isSearchEmpty
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Search(string searchQuery)
        {

            return RedirectToAction("Results", new { searchQuery });
        }
    }
}
