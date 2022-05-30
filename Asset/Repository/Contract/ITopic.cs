using Asset.Data.Model;
using Asset.ViewModel.TopicVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Contract
{
    public interface ITopic
    {
        IEnumerable<LibraryTopic> GetAllLibraryTopic();
        Task AddLibraryTopic(LibraryTopic libraryTopic);
        LibraryTopic GetLibraryTopic(int id);
        int AssetCheckoutHistoryCount(int assetId);
        int CHCount(int topicId);

        IEnumerable<string> GetHistory(int topicId);
        IEnumerable<CheckoutHistory> GetPatronInBooksCheckout(int topicId);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int topicId);
        IEnumerable<LibraryAsset> SearchTopicAsset(LibraryTopic topic, string searchQuery);
        CheckoutHistory TopicObject(int topicId);

    }
}
