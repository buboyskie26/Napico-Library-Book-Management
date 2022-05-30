using Asset.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModel.PatronVM
{
    public class PatronIndex
    {
        public int PenaltyCount { get; set; }

        public string FirstName { get; set; }
        public string ImageUrl { get; set; }
        public string Id { get; set; }
        public bool IsOverDue { get; set; }

        public string LastName { get; set; }
        public DateTime DateLogin { get; set; }
        public string Role { get; set; }

        public DateTime Created { get; set; }

        public string Address { get; set; }

        public int LibraryCardId { get; set; }

        public IEnumerable<Checkout> GetCheckoutWithPatron { get; set; }
        public IEnumerable<CheckoutHistory> GetCheckoutHistoryWithPatron { get; set; }
        public IEnumerable<Hold> GetHoldWithPatron { get; set; }

    }
    public class PatronListIndex
    {
        public IEnumerable<PatronIndex> ListPatron { get; set; }
        public string SearchQuery { get; set; }

    }
    public class PatronIndexSearch
    {
        public string SearchQuery { get; set; }
        public bool IsEmpty { get; set; }
        public IEnumerable<PatronIndex> ListPatron { get; set; }

    }
}
