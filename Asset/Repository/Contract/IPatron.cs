using Asset.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Contract
{
    public interface IPatron
    {
        Task<IEnumerable<Patron>> GetAllPatron();
        Task<IEnumerable<Patron>> SinglePatron(string name);
        Task<Patron> GetPatronById(string id);
        Task UpdatePatron(Patron id);
        Task<IEnumerable<Checkout>> GetCheckoutsPatron(string id);
        Task<IEnumerable<Hold>> GetHoldsPatron(string id);
        Task<IEnumerable<CheckoutHistory>> GetCheckoutHistoryPatron(string id);
        bool IsCheckoutBeyond(string userId);
        bool AssetBeenCheckedout(string userId);

        int CheckoutPenalty(string userId);


    }
}
