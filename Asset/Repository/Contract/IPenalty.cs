using Asset.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Contract
{
    public interface IPenalty
    {
        IEnumerable<CheckoutPenalty> GetAllPenalty();
        CheckoutPenalty GetAllPenaltyById(int id);

    }
}
