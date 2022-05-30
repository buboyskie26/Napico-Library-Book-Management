using Asset.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Contract
{
    public  interface IHold
    {
        Task AddHold(Hold hold);
        Task<Hold> GetHoldId(int hold);
        IEnumerable<Hold> GetAllHold();
        Task<IEnumerable<CheckoutHistory>> GetAllCheckoutHistory(string userId);
        Task<IEnumerable<CheckoutHistory>> GetAllCheckoutHistory();
        Task<IEnumerable<Hold>> GetAllPendingHold();
        Task UpdateHold(Hold hold);
        bool GetIsReadBool(string userId);

        int GetNotification(string userId);
        string NotificationName(string userId);
        string NotificationImage(string userId);

        Task AddHoldHistory(HoldHistory hold);
        Task<HoldHistory> GetHoldHistoryId(int hold);
        IEnumerable<HoldHistory> GetAllHoldHistory();
        Task<IEnumerable<HoldHistory>> GetAllHoldHistoryAsync();
        Task UpdateHoldHistory(HoldHistory hold);


    }
}
