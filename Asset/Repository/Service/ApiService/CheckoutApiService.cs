using Asset.Data;
using Asset.Repository.Contract.ApiContract;
using Asset.ViewModel.CheckHistoryVM;
using Asset.ViewModel.HoldHistoryVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Service.ApiService
{
    public class CheckoutApiService: ICheckoutApi
    {
        private readonly ApplicationDbContext _db;
        public CheckoutApiService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<CheckHistoryIndex> PatronCheckoutById(string patientId, int assetId)
        {
            var history = _db.CheckoutHistory
                .Where(q => q.PatronId == patientId)
                .Where(q => q.LibraryAsset.Id == assetId)
                .Include(q => q.LibraryTopic)
                .Include(q => q.LibraryAsset);

            var obj = history.Select(q => new CheckHistoryIndex
            {
               CheckedOut= Convert.ToDateTime(q.CheckedOut.ToString("dd/MM/yyyy")),
               CheckIn = Convert.ToDateTime(q.CheckIn),
               Id=q.Id,
               TopicName = q.LibraryTopic.Title
               
            }).ToList();

            return obj;
        }
        public List<CheckHistoryIndex> PatronCheckoutById(string patientId)
        {
            var history = _db.CheckoutHistory
                .Where(q => q.PatronId == patientId)
                .Include(q => q.LibraryTopic)
                .Include(q => q.LibraryAsset);

            var obj = history.Select(q => new CheckHistoryIndex
            {
                CheckedOut = Convert.ToDateTime(q.CheckedOut.ToString("dd/MM/yyyy")),
                CheckIn = Convert.ToDateTime(q.CheckIn),
                Id = q.Id,
                TopicName = q.LibraryTopic.Title

            }).ToList();

            return obj;
        }

    }
}
