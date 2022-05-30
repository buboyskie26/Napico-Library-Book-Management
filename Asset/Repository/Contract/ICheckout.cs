using Asset.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Contract
{
    public interface ICheckout
    {

        void Add(Checkout checkout);
        Task AddRequest(CheckoutRequest history);
        Task<CheckoutRequest> GetRequestId(int history);

        Task UpdateRequest(CheckoutRequest history);

        Checkout GetCheckoutId(int id);
        IEnumerable<Checkout> GetAllCheckout(int id);
        Task<IEnumerable<CheckoutRequest>> GetAllRequest();
        IEnumerable<CheckoutHistory> GetAllCheckoutHistory(int id,string userId);

        TimeSpan HistorySpan(int id);
        DateTime CheckInSpan(int id);

        IEnumerable<Hold> GetCurrentHold(int id);

        int HoldPendingCount(int id);
        string HoldPatronName(int id, string userId);
        bool? HoldPatronStatus(int id);

        string CheckoutHistoryPatronName(int id);
        void PlaceCheckout(int assetId, int libraryCardId,string userId, int topicId);
        void PlaceHold(int assetId, int libraryCardId, string userId);
        string GetCurrentCheckoutName(int assetId,string userId);
        List<string> CheckoutNames(int assetId);

        int CurrentCountHoldName(int assetId);
        DateTime HoldDate(int assetId);
        string PatronHoldName(int assetId);
        void PlaceCheckin(int assetId,string userId,string patronName);

        int UserCardId(string userid);
        void PlaceCheckLost(int assetId, string userId, string patronName);
        void PlaceMarkAsFound(int assetId, string userId, string patronName);

        bool HistoryCheckDateReturn(int id, string userId);
        bool LostTheBook(int assetId, string userId);
        bool CheckedInlostBook(int assetId, string userId);
        bool ReturnButWasLost(int assetId, string userId);



        bool CheckedoutTheSingleAsset(int assetId, string userId);
        int CheckedoutLostCount(int assetId);
        int AssetOrigCopies(int assetId);


        Hold FirstInHoldList(int assetId);




    }
}


