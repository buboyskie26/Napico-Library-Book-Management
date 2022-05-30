using Asset.ViewModel.CheckHistoryVM;
using Asset.ViewModel.HoldHistoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Contract.ApiContract
{
    public interface ICheckoutApi
    {
        public List<CheckHistoryIndex> PatronCheckoutById(string patientId, int assetId);
        public List<CheckHistoryIndex> PatronCheckoutById(string patientId);

    }
}
