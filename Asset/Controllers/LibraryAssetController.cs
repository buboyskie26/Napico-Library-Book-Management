using Asset.Data.Model;
using Asset.Repository.Contract;
using Asset.Repository.Contract.ApiContract;
using Asset.ViewModel.AssetVM;
using Asset.ViewModel.CheckoutRequestVM;
using Asset.ViewModel.HoldVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Controllers
{
    public class LibraryAssetController : Controller
    {
        private readonly ILibAsset _asset;
        private readonly ICheckout _checkout;
        private readonly IHold _hold;
        private readonly UserManager<Patron> _userManager;
        private readonly IPatron _patron;
        private readonly ITopic _topic;
        private readonly ICheckoutApi _checkoutServ;

        // todo
        // create a dynamic number of copies
        // if the user cheked an item, minus - 1 and if item are checked in return back to originak
        // , if hold notthing to do
        public LibraryAssetController(ILibAsset libAsset, IPatron patron, ITopic topic, ICheckoutApi checkoutApi,    
            UserManager<Patron> user, IHold hold,
            ICheckout checkout)
        {
            _checkoutServ = checkoutApi;
            _topic = topic;
            _patron = patron;
            _hold = hold;
            _checkout = checkout;
            _userManager = user;

            _asset = libAsset;
        }
        public IActionResult Index()
        {
            var asset = _asset.GetAll();

            var obj = asset.Select(n => new AssetIndex
            {
                Year=n.Year,
                Description = n.Description,
                Title=n.Title,
                Cost=n.Cost,
                NumberOfCopies=n.NumberOfCopies,
                ImageUrl=n.ImageUrl,
                StatusName=n.Status.Name,
                Id=n.Id

            });
            var model = new AssetIndecList
            {
                ListAssetIndex = obj
            };


            return View(model);
        }
        [Authorize]
        public IActionResult Checkout(int id)
        {
            var a = _asset.GetById(id);

            // getting the user current library card id
            // that will show to the input box
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            var patronid = _checkout.UserCardId(user.Id);

            var model = new CheckoutIndex
            {
                UserCardId = patronid,
                Title = a.Title,
                ImageUrl = a.ImageUrl,
                Description = a.Description,
                AssetId = id,
                LibraryCardId = "",
                NumberOfCopies = a.NumberOfCopies,
                Cost=a.Cost
            };

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Detail(int id)
        {
            var a = _asset.GetById(id);

            var hold = _checkout.GetCurrentHold(id);

            var holdModel = hold.Select(p => new HoldModel
            {
                Id = p.Id,
                PatronName = _checkout.PatronHoldName(p.Id),
                HoldPlaced = _checkout.HoldDate(p.Id),
                Issued = p.IsIssued
            });
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;
            var userName = user.FirstName + " " + user.LastName;

            int sampName =  holdModel.Count(q => q.PatronName == userName);
            var patron = _checkout.GetAllCheckoutHistory(id,user.Id);

            var model = new AssetIndex()
            {
                dataenum= _checkoutServ.PatronCheckoutById(userId, 9),
            /*FirstListHold = _checkout.FirstInHoldList(id),*/
                CheckedinLostBook = _checkout.CheckedInlostBook(id,userId),
                ReturnLost = _checkout.ReturnButWasLost(id,userId),
                LostCount = _checkout.CheckedoutLostCount(id),
                OrigCount = _checkout.AssetOrigCopies(id),
                CheckoutUserEqualsLoginUser = _checkout.CheckedoutTheSingleAsset(id, userId),
                UserLostTheBook = _checkout.LostTheBook(id, userId),
                CheckoutCount = _checkout.GetAllCheckout(id).Count(),
                PenaltyCount = _patron.CheckoutPenalty(user.Id),
                IsCheckedoutAsset = _patron.AssetBeenCheckedout(userId),
                Diff = _checkout.HistorySpan(id),
                CheckoutNames = _checkout.CheckoutNames(id),
                InSpan = _checkout.CheckInSpan(id),
                Recent = _checkout.GetAllCheckoutHistory(id,user.Id).FirstOrDefault()?.IsRecent,
                HoldName = _checkout.HoldPatronName(id, user.Id),
                HistoryPatronName = _checkout.CheckoutHistoryPatronName(id),
                UserName = userName,
                IsValid = _checkout.HoldPatronStatus(id),
                SelfPatron = true,

                NumberOfCopies = a.ReferenceCopies,
                ImageUrl = a.ImageUrl,
                Title=a.Title,
                StatusName = a.Status.Name,

                Id= id,
                CheckoutHistories = _checkout.GetAllCheckoutHistory(id, user.Id),
                CurrentHold = holdModel,
                PatronName = _checkout.GetCurrentCheckoutName(id, userId),
                HoldNameApperance = sampName
                
            };

            return View(model);
        }
        public IActionResult CheckLost(int id)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;
            var userName = user.FirstName + " " + user.LastName;


            _checkout.PlaceCheckLost(id, userId, userName);

            return RedirectToAction("Detail", new { id = id });

        }
        public IActionResult MarkAsFound(int id)
        {

            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;
            var userName = user.FirstName + " " + user.LastName;


            _checkout.PlaceMarkAsFound(id, userId, userName);

            return RedirectToAction("Detail", new { id = id });

        }

        public IActionResult Hold(int id)
        {
            var a = _asset.GetById(id);

            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;


            var patronid = _checkout.UserCardId(user.Id);

            var model = new CheckoutIndex
            {
                UserCardId = patronid,
                Title = a.Title,
                ImageUrl = a.ImageUrl,
                AssetId = id,
                LibraryCardId = ""
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult PlaceHold(int assetId, int userCardId)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            _checkout.PlaceHold(assetId, userCardId, user.Id);
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceCheckout(int assetId, int userCardId)
        {
            var uuserId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(uuserId).Result;

            var asset = _asset.GetById(assetId);



            _checkout.PlaceCheckout(assetId, userCardId, user.Id, asset.LibraryTopicId);
            return RedirectToAction("Detail", new { id = assetId });
        }
        public IActionResult Checkin(int id)
        {
  
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;
            var userName = user.FirstName + " " + user.LastName;

            _checkout.PlaceCheckin(id, user.Id, userName);

            return RedirectToAction("Detail", new { id = id });
        }


        public async Task<IActionResult> UpdatingPlaceHold(int id)
        {
            // If the user apply for place hold,
            // assume that the book was available, 
            // the user who take the first place hold will be checked in the book.



            var hold = await _hold.GetHoldId(id);

            var obj = new HoldIndex()
            {
                Id = id,
            };


            return View(obj);
        }

        public async Task<IActionResult> ApproveHold(int id)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            var hold = await _hold.GetHoldId(id);
            hold.IsIssued = true;
            // whom update
            hold.AdminId = user.Id;

            await _hold.UpdateHold(hold);
            return RedirectToAction("HoldIndex");
        }
 
        public async Task<IActionResult> DeclinedHold(int id)
        {
            var hold = await _hold.GetHoldId(id);

            hold.IsIssued = false;

            await _hold.UpdateHold(hold);
            return RedirectToAction("HoldIndex");
        }
        public IActionResult HoldIndex()
        {
            var userId = _userManager.GetUserId(User);

            var hold = _hold.GetAllHold();
            var patronHold = hold.Where(q => q.UserPatronId == userId);
            var obj = patronHold.Select(q => new HoldIndex()
            {
                Id=q.Id,
                AssetId=q.LibraryAsset.Id,
                HoldIssued=q.HoldPlaced,
                Issue = q.IsIssued,
                CardId= q.LibraryCard.Id,
                AssetName=q.LibraryAsset.Title,
                TopicName = q.LibraryTopic.Title
            });

            var model = new ListHoldIndex()
            {
                ListIndex = obj
            };

            return View(model);
        }
        [Authorize(Roles ="Admin")]
        public IActionResult AllHoldList()
        {
            var userId = _userManager.GetUserId(User);

            var hold = _hold.GetAllHold();
            var obj = hold.Select(q => new HoldIndex()
            {
                Id = q.Id,
                HoldIssued = q.HoldPlaced,
                Issue = q.IsIssued,
                CardId = q.LibraryCard.Id,
                AssetName = q.LibraryAsset.Title,
                TopicName = q.LibraryTopic.Title
            });

            var model = new ListHoldIndex()
            {
                ListIndex = obj
            };

            return View(model);
        }


        public IActionResult CheckRequest(int id)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            var patronid = _checkout.UserCardId(user.Id);

            var now = DateTime.Now;

            var asset = _asset.GetById(id);

            var obj = new RequestIndex()
            {
                UserId = user.Id,
                AssetName = asset.Title,
                LibraryAssets = BuildAsset(asset),
                Start = now,
                CardId = patronid,
                LibraAssetId = id,
            };
            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> CheckRequest(RequestIndex request)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            var now = DateTime.Now;
            var output = new CheckoutRequest()
            {
                RequestDate = now,
                UserPatronId = user.Id,
                Approved = null,
                LibraryAssetId = request.LibraAssetId,
                LibraryCardId = request.CardId
            };
 
            await _checkout.AddRequest(output);
            return RedirectToAction("CheckRequestIndex");
        }

        public IActionResult ViewCheckoutNames(int id)
        {
            var names = _checkout.GetAllCheckout(id);

            var hmm = names.Select(item => new CheckoutNames()
            {
                AssetName = item.LibraryAsset.Title,
                ImageUrl = item.UserPatron.ImageUrl,
                PatronName = item.UserPatron.FirstName + " " + item.UserPatron.LastName,
                CheckoutedDate = item.Since
            });

            var model = new ListCheckoutNames()
            {
                ListNames = hmm
            };

            var index = new CheckoutNames();
            foreach (var item in names)
            {
                index = new CheckoutNames()
                {
                    AssetName = item.LibraryAsset.Title,
                    ImageUrl = item.UserPatron.ImageUrl,
                    PatronName = item.UserPatron.FirstName + " " + item.UserPatron.LastName,
                    CheckoutedDate=item.Since
                };

            }


            return View(model);
        }

        private AssetBuild BuildAsset(LibraryAsset f)
        {
            return new AssetBuild()
            {
                AssetName = f.Title,
                AssetCost = f.Cost
            };
        }
        public async Task<IActionResult> CheckRequestIndex()
        {
            var obj = await _checkout.GetAllRequest();

            var requestObj = obj.ToList();

            var assetObj = _asset.GetAll();
             
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            obj = obj.Where(q => q.UserPatron.Id == user.Id);

            var index = obj.Select(q => new RequestIndex()
            {
                IsClick = q.IsHit,
                AssetId = q.LibraryAsset.Id,
                
                Until = q.Until,
                UserName = q.UserPatron.FirstName,
                Id = q.Id,
                Approve = q.Approved,
                AssetName = q.LibraryAsset.Title,
                LibraAssetId = q.LibraryAssetId,
                CardId = q.LibraryCardId,
                Start = q.Start,
                RequestDate = q.RequestDate,
                Ratified = obj.Where(q=> q.Approved == true).Count(),
                Declined = obj.Where(q=> q.Approved == false).Count(),
                Pending = obj.Where(q=> q.Approved == null).Count(),
                LibraryAssets = BuildAsset(q.LibraryAsset),
                
                TopicObj = BuildObj(q.LibraryAsset.LibraryTopic)
            });

            var model = new ListRequest()
            {
                RequestList = index,

            };

            return View(model);
        }
        public async Task<IActionResult> AdminCheckRequest()
        {
            var obj = await _checkout.GetAllRequest();

            var requestObj = obj.ToList();

            var assetObj = _asset.GetAll();

           
            var index = obj.Select(q => new RequestIndex()
            {
                IsClick = q.IsHit,
                AssetId = q.LibraryAsset.Id,

                Until = q.Until,
                UserName = q.UserPatron.FirstName,
                Id = q.Id,
                Approve = q.Approved,
                AssetName = q.LibraryAsset.Title,
                LibraAssetId = q.LibraryAssetId,
                CardId = q.LibraryCardId,
                Start = q.Start,
                Ratified = obj.Where(q => q.Approved == true).Count(),
                Declined = obj.Where(q => q.Approved == false).Count(),
                Pending = obj.Where(q => q.Approved == null).Count(),
                LibraryAssets = BuildAsset(q.LibraryAsset),

                TopicObj = BuildObj(q.LibraryAsset.LibraryTopic)
            });

            var model = new ListRequest()
            {
                RequestList = index,

            };

            return View(model);
        }

        private TopicRequestObj BuildObj(LibraryTopic w)
        {
            return new TopicRequestObj
            {
                 TopicName=w.Title,
                 TopicDesc=w.Description,
                 TopicId=w.Id
            };
        }

        public IActionResult Results(string searchQuery)
        {

            var asset = _asset.SingleLibrary(searchQuery);

            bool emptySearch = string.IsNullOrEmpty(searchQuery) || asset.Any() == false;

            var obj = asset.Select(n => new AssetIndex
            {
                Year = n.Year,
                Title = n.Title,
                Cost = n.Cost,
                Description = n.Description,
                NumberOfCopies = n.NumberOfCopies,
                ImageUrl = n.ImageUrl,
                StatusName = n.Status.Name,
                Id = n.Id

            });

            var model = new AssetSearchList()
            {
                ListAssetIndex = obj,
                EmptySearch=emptySearch,
                SearchQuery = searchQuery
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Search(string searchQuery)
        {


            return RedirectToAction("Results", new { searchQuery } );
        }
        //Admin Role
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CheckRequestAdminIndex()
        {
            var obj = await _checkout.GetAllRequest();

            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;
 
            // todo make exception to view the books for ADMIN

            var email = user.UserName;
 
            var index = obj.Select(q => new RequestIndex()
            {
                Until = q.Until,
                Id = q.Id,
                UserName = q.UserPatron.FirstName + " "+q.UserPatron.LastName,
                IsClick = q.IsHit,
                Approve = q.Approved,
                AssetName = q.LibraryAsset.Title,
                LibraAssetId = q.LibraryAssetId,
                CardId = q.LibraryCardId,
                Start = q.Start,
                Ratified = obj.Where(q => q.Approved == true).Count(),
                Declined = obj.Where(q => q.Approved == false).Count(),
                Pending = obj.Where(q => q.Approved == null).Count(),
                LibraryAssets = BuildAsset(q.LibraryAsset)
            });

            var model = new ListRequest()
            {
                RequestList = index,
            };

            return View(model);
        }
        public async Task<IActionResult> CreateAsset(int id)
        {
            var dropDown = await _asset.ListDropdown();

            ViewBag.StatusId = new SelectList(dropDown.Statuses, "Id", "Name");
            ViewBag.BranchId = new SelectList(dropDown.LibraryBranches, "Id", "Name");
            ViewBag.TopicId = new SelectList(dropDown.LibraryTopics, "Id", "Title");

            var topic = _topic.GetLibraryTopic(id);

            var obj = new CreateIndex()
            {
                LibraryTopicId = topic.Id
            };

            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsset(CreateIndex assetIndex)
        {
            var dropDown = await _asset.ListDropdown();

            ViewBag.StatusId = new SelectList(dropDown.Statuses, "Id", "Name");
            ViewBag.BranchId = new SelectList(dropDown.LibraryBranches, "Id", "Name");
            ViewBag.TopicId = new SelectList(dropDown.LibraryTopics, "Id", "Title");



            await _asset.AddAsset(assetIndex);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateAsset(int id)
        {
            var dropDown = await _asset.ListDropdown();

            ViewBag.StatusId = new SelectList(dropDown.Statuses, "Id", "Name");
            ViewBag.BranchId = new SelectList(dropDown.LibraryBranches, "Id", "Name");

            var k = _asset.GetById(id);

            var model = new AssetIndex()
            {
                Id = k.Id,
                Title = k.Title,
                Description = k.Description,
                Cost = k.Cost,
                NumberOfCopies = k.NumberOfCopies,
                Year = k.Year,
                ReferenceCopies = k.NumberOfCopies,
                LocationId = k.LocationId,
                StatusId = k.StatusId,
                ImageUrl = k.ImageUrl,
                LibraryTopicId = k.LibraryTopicId,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsset(CreateIndex k)
        {
            var dropDown = await _asset.ListDropdown();

            ViewBag.StatusId = new SelectList(dropDown.Statuses, "Id", "Name");
            ViewBag.BranchId = new SelectList(dropDown.LibraryBranches, "Id", "Name");





            await _asset.UpdateAsset(k);
            return RedirectToAction("Genre", "Topic", new { id = k.LibraryTopicId });
        }
        public async Task<IActionResult> UpdateRequest(int id)
        {
            var r = await _checkout.GetRequestId(id);


            var obj = new RequestIndex()
            {
                
                Until = r.Until,
                Start = r.Start,
                Id = id,
                AssetName = r.LibraryAsset.Title,
                CardId = r.LibraryCard.Id
            };

            return View(obj);
        }
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var request = await _checkout.GetRequestId(id);
            var now = DateTime.Now;

            request.Start = now;
            request.Approved = true;
            request.AdminId = user.Id;

            await _checkout.UpdateRequest(request);

            if(user.Role == "Admin")
            
                return RedirectToAction("CheckRequestAdminIndex");
            else
                return RedirectToAction("CheckRequestIndex");


        }
        public async Task<IActionResult> DeclinedRequest(int id)
        {
            var request = await _checkout.GetRequestId(id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            request.Approved = false;
            request.AdminId = user.Id;

            await _checkout.UpdateRequest(request);

            return RedirectToAction("CheckRequestIndex");
        }


    }
}
