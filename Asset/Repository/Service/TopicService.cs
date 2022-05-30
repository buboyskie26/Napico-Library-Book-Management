using Asset.Data;
using Asset.Data.Model;
using Asset.Repository.Contract;
using Asset.ViewModel.TopicVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Service
{
    public class TopicService : ITopic
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _web;

        public TopicService(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _context = applicationDbContext;
            _web = webHostEnvironment;
        }

        public async Task AddLibraryTopic(LibraryTopic libraryTopic)
        {
            await _context.LibraryTopics.AddAsync(libraryTopic);
            await _context.SaveChangesAsync();
        }

        public int AssetCheckoutHistoryCount(int assetId)
        {
            var asset = _context.LibraryAssets
               .Where(q => q.Id == assetId);


            var history = _context.CheckoutHistory
                .Where(q => q.LibraryAsset.Id == assetId);

            var count = history.Count();

            return count;
    
        }

        public int CHCount(int topicId)
        {
            var co = _context.CheckoutHistory
                .Include(q => q.LibraryTopic)
                .Where(q => q.LibraryTopicId == topicId)
                .Count();

            return co;
        }

        public IEnumerable<LibraryTopic> GetAllLibraryTopic()
        {
            return _context.LibraryTopics
                .Include(q => q.LibraryAssets)
                .Include(q=> q.LibraryAssets).ThenInclude(q=> q.Location)
                .Include(q=> q.LibraryAssets).ThenInclude(q=> q.CheckoutHistories)
                .Include(q=> q.LibraryAssets).ThenInclude(q=> q.Status)
                .Include(q=> q.LibraryAssets).ThenInclude(q=> q.LibraryTopic)
                .ToList();
        }

        public IEnumerable<string> GetHistory(int topicId )
        {
            var history =  _context.CheckoutHistory
                 .Include(q => q.LibraryAsset)
                 .Include(q => q.LibraryCard)
                 .Include(q => q.Patron)
                 .ToList();

            var ch = history
                .Where(q => q.LibraryTopicId == topicId)
                .Select(q=> q.PatronId)
                .Distinct();

            return ch;
        }

        public LibraryTopic GetLibraryTopic(int id)
        {
            return GetAllLibraryTopic().FirstOrDefault(q => q.Id == id);
        }

        public IEnumerable<CheckoutHistory> GetPatronInBooksCheckout(int topicId)
        {
            var history = _context.CheckoutHistory
               .Include(q => q.LibraryAsset)
               .Include(q => q.LibraryTopic)
               .Include(q => q.LibraryCard)
               .Include(q => q.Patron)
               .ToList();


            /*            var results = persons.GroupBy(
                            p => p.PersonId,
                            p => p.car,
                            (key, g) => new { PersonId = key, Cars = g.ToList() });*/
            /*            var results = from p in persons
                          group p.car by p.PersonId into g
                          select new { PersonId = g.Key, Cars = g.ToList() };*/
            /*            var results = ch.GroupBy(n => new { n.PatronId, n.LibraryAsset.Title })
             .Select(g => new
             {
                 g.Key.PatronId,
                 g.Key.Title
             }).ToList();*/
            var ch = history
                .Where(q => q.LibraryTopicId == topicId);


 
            var results = ch.GroupBy(q => q.LibraryAsset.Title).Any(q => q.Count() > 1);


 
            return ch;
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int topicId)
        {
            var history = _context.CheckoutHistory
             .Include(q => q.LibraryAsset)
             .Include(q => q.LibraryCard)
             .Include(q => q.LibraryTopic)
             .Include(q => q.Patron)
             .ToList();

            var ch = history
                .Where(q => q.LibraryTopicId == topicId);

            return ch;
        }

        public CheckoutHistory TopicObject(int topicId)
        {
            var history = _context.CheckoutHistory
            .Include(q => q.LibraryTopic)
            .FirstOrDefault(q => q.LibraryTopicId == topicId);

            return history;
        }

        public IEnumerable<LibraryAsset> SearchTopicAsset(LibraryTopic topic, string searchQuery)
        {
            var obj = _context.LibraryAssets
                .Include(q => q.LibraryTopic)
                .ToList();

            return string.IsNullOrEmpty(searchQuery) ? topic.LibraryAssets : obj
                .Where(q => q.Title.ToLower().Contains(searchQuery.ToLower()) ||
                q.Description.ToLower().Contains(searchQuery.ToLower()));

        }
    }
}
