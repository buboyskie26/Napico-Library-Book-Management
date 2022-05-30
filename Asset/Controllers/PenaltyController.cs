using Asset.Data.Model;
using Asset.Repository.Contract;
using Asset.ViewModel.PenaltyVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Controllers
{
    public class PenaltyController : Controller
    {
        private readonly IPenalty _penalty;
        private readonly UserManager<Patron> _userManager;
        public PenaltyController(IPenalty penalty, UserManager<Patron> userManager )
        {
            _penalty = penalty;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var pen = _penalty.GetAllPenalty();

            var obj = pen.Select(p => new PenaltyIndex()
            {
                Id = p.Id,
                CardId = p.LibraryCard.Id,
                AssetName = p.LibraryAsset.Title,
                PenaltyDate = p.PenaltyDate,
                AmountPenalty = p.AmountPenalty,
                Username = p.Patron.FirstName + " " + p.Patron.LastName
            });
            var model = new PenaltyListIndex()
            {
                PenaltyList=obj
            };
            return View(model);
        }
        public IActionResult MyPenalty()
        {

            var pen = _penalty.GetAllPenalty();

            var userId = _userManager.GetUserId(User);

            pen = pen.Where(q => q.PatronId == userId);

            var obj = pen.Select(p => new PenaltyIndex()
            {
                Id = p.Id,
                CardId = p.LibraryCard.Id,
                AssetName = p.LibraryAsset.Title,
                PenaltyDate = p.PenaltyDate,
                AmountPenalty = p.AmountPenalty,
                Username = p.Patron.FirstName + " " + p.Patron.LastName
            });
            var model = new PenaltyListIndex()
            {
                PenaltyList = obj
            };

            return View(model);
        }


    }
}
