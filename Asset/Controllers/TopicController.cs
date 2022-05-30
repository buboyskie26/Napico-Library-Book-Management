using Asset.Data.Model;
using Asset.Repository.Contract;
using Asset.ViewModel.AssetVM;
using Asset.ViewModel.TopicVM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Controllers
{
    public class TopicController : Controller
    {
        private readonly ITopic _topic;
        private readonly ILibAsset _asset;
        public TopicController(ITopic topic, ILibAsset libAsset)
        {
            _asset = libAsset;
            _topic = topic;
        }
        public IActionResult Index()
        {
            var obj = _topic.GetAllLibraryTopic();
 
            var model = obj.Select(q=> new TopicIndex()
            {
                Id=q.Id,
                Created=q.Created,
                Description=q.Description,
                ImageUrl=q.ImageUrl,
                Title=q.Title,
                AssetNumber = q.LibraryAssets.Count(),
                CheckoutCount = _topic.GetHistory(q.Id).Count(),

            });

            var p = new TopicListIndex
            {
                ListTopic = model,
            };

            return View(p);
        }

        public IActionResult Asset(int id, string searchQuery)
        {
            var topic = _topic.GetLibraryTopic(id);

            var asset = _topic.SearchTopicAsset(topic, searchQuery);

/*            bool isEmpty = string.IsNullOrEmpty(searchQuery) || asset.Any() == false;*/

            var obj = asset.Select(q => new TopicGenre
            {
                TopicId = id,
                TotalHistoryCount = _topic.AssetCheckoutHistoryCount(q.Id),
                AssetId=q.Id,
                Costs=q.Cost,
                Description=q.Description,
                Title=q.Title,
                ImageUrl=q.ImageUrl,
                NumberOfCopies=q.NumberOfCopies,
                ReferenceCopies=q.ReferenceCopies,
                Topic = BuildTopic(q),
                CheckHistoryCount = q.CheckoutHistories.Count(),
            });

            var model = new TopicGenreIndex
            {
                ListTopic = obj,
                Topic = BuildTopic(topic),
                SearchObject = BuildSearch(topic.LibraryAssets),
                SearchQuery = searchQuery,
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Asset", new { id, searchQuery });
        }

        private TopicSearchIndex BuildSearch(IEnumerable<LibraryAsset> asset)
        {
            var asd = new TopicSearchIndex();
            foreach (var item in asset)
            {
                return new TopicSearchIndex()
                {
                    TopicId = item.LibraryTopicId,
                    TopicTitle = item.LibraryTopic.Title
                };
            }
          

            return asd;
        }

        public IActionResult ViewCheckoutPatron(int id)
        {
            var asset = _asset.GetById(id);

            var obj = asset.CheckoutHistories.Select(q => new TopicCheckoutHistory
            {
                AssetName = q.LibraryAsset.Title,
                PatronName = q.Patron.FirstName + " " + q.Patron.LastName,
                LibraryCardId = q.LibraryCard.Id,
                ImageUrl = q.Patron.ImageUrl,
                Id = q.PatronId
            });
            var model = new TopicListCheckoutHistory()
            {
                HistoryList=obj.GroupBy(q=> q.PatronName).Select(q=> q.First()).ToList()
            };
            return View(model);
        }
        public IActionResult ViewCheckoutBooks(int id)
        {
            var topic = _topic.GetPatronInBooksCheckout(id);

            var topicObj = _topic.TopicObject(id);

            var hm  = topicObj?.LibraryTopic;

            var obj = topic.Select(w => new TopicCheckoutHistory()
            {
                PatronName = w.Patron.FirstName+" "+w.Patron.LastName,
                AssetName = w.LibraryAsset.Title,
                ImageUrl = w.Patron.ImageUrl,
                LibraryCardId = w.LibraryCard.Id,
                Id = w.Patron.Id,
                CheckInDate = w.CheckIn,
            });
            var model = new TopicListCheckoutHistory()
            {
                HistoryList = obj,
                TopicObj = BuildTopicObj(hm)
            };
            return View(model);
        }

       

        private TopicCheckoutHistory BuildTopicObj(LibraryTopic hm)
        {
            var obj = new TopicCheckoutHistory();
            if (hm != null)
            {
                return new TopicCheckoutHistory()
                {
                    TopicName = hm.Title
                };
            }

            return obj;
        }
        private TopicIndex BuildTopic(LibraryTopic q)
        {

            return new TopicIndex()
            {
                Title = q.Title,
                Description = q.Description,
                Id = q.Id,
                Created = q.Created,
                ImageUrl = q.ImageUrl,
            };
        }
        private TopicIndex BuildTopic(LibraryAsset o)
        {

            var q = o.LibraryTopic;


            return BuildTopic(q);
        }
    }
}
