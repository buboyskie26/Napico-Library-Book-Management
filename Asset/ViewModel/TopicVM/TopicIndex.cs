using Asset.Data.Model;
using Asset.ViewModel.AssetVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModel.TopicVM
{
    public class TopicIndex
    {

        public IEnumerable<CheckoutHistory> HistoryEnum { get; set; }
        public int Id { get; set; }
        public int AssetNumber { get; set; }
        public int CheckoutCount { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string ImageUrl { get; set; }

        public IEnumerable<LibraryAsset> LibraryAssets { get; set; }


    }
    
    public class TopicListIndex
    {
        public IEnumerable<TopicIndex> ListTopic { get; set; }
    }
    public class SampHistory
    {
        public string PatronId { get; set; }
        public string LibraryTitle { get; set; }

    }
    public class TopicCheckoutHistory
    {
        public string AssetName { get; set; }
        public DateTime? CheckInDate { get; set; }
        public string PatronName { get; set; }
        public string ImageUrl { get; set; }
        public int LibraryCardId { get; set; }
        public string Id { get; set; }
        public string TopicName { get; set; }

    }

    public class TopicListCheckoutHistory
    {
       public IEnumerable<TopicCheckoutHistory> HistoryList { get; set; }
        public string SearchQuery { get; set; }
        public TopicCheckoutHistory TopicObj  { get; set; }

      
    }
    public class TopicGenreIndex
    {
        public IEnumerable<TopicGenre> ListTopic { get; set; }
        public TopicIndex Topic { get; set; }
        public string SearchQuery { get; set; }
        public bool EmptySearch { get; set; }
        public TopicSearchIndex SearchObject { get; set; }

    }
}
