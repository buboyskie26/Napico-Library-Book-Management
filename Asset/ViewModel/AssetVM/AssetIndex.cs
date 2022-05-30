using Asset.Data.Model;
using Asset.ViewModel.CheckHistoryVM;
using Asset.ViewModel.TopicVM;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModel.AssetVM
{
    public class AssetIndex
    {

        public string TopicName { get; set; }

        public int PenaltyCount { get; set; }
        public Hold FirstListHold { get; set; }
        public DateTime? CheckInStatus { get; set; }
        public int CheckoutCount { get; set; }
        public int ReferenceCopies { get; set; }
        public int StatusId { get; set; }
        public int LibraryTopicId { get; set; }
        public int LocationId { get; set; }
        public int Id { get; set; }
        public int LostCount { get; set; }
        public int OrigCount { get; set; }
        public bool? Recent { get; set; }
        public bool IsCheckedoutAsset { get; set; }
        public bool ReturnLost { get; set; }
        public bool CheckoutUserEqualsLoginUser { get; set; }
        public bool UserLostTheBook { get; set; }
        public bool CheckedinLostBook { get; set; }
        public List<string> CheckoutNames { get; set; }

        public TimeSpan Diff { get; set; }
        public DateTime InSpan { get; set; }
        public string ImageUrl { get; set; }
        public string HoldName { get; set; }
        public int HoldNameApperance { get; set; }
        public string HistoryPatronName { get; set; }
        public string UserName { get; set; }
        public bool? IsValid { get; set; }
        public bool SelfPatron { get; set; }

        public string Title { get; set; }
         public int Year { get; set; }
        public string StatusName { get; set; }
        public string Description { get; set; }
        public string PatronName { get; set; }


        [Display(Name = "Cost of Replacement")]
        
        public decimal Cost { get; set; }
        public int NumberOfCopies { get; set; }

        public IEnumerable<Checkout> Checkouts { get; set; }
        public IEnumerable<CheckoutHistory> CheckoutHistories { get; set; }
        public IEnumerable<HoldModel> CurrentHold { get; set; }
        public List<CheckHistoryIndex> dataenum { get; set; }


    }
    public class DataEnum
    {
        public CheckHistoryIndex dataenumm { get; set; }
    }
    public class CreateIndex
    {
        [Display(Name = "Photo")]
        public IFormFile ImageUrl { get; set; }
        public int Year { get; set; }
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public int NumberOfCopies { get; set; }
        public int ReferenceCopies { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public int LibraryTopicId { get; set; }
        public int LocationId { get; set; }
    }
    public class CheckoutNames
    {
        public string PatronName { get; set; }
        public string ImageUrl { get; set; }
        public string AssetName { get; set; }
        public DateTime CheckoutedDate { get; set; }
    }
    public class ListCheckoutNames
    {
        public IEnumerable<CheckoutNames> ListNames { get; set; }
    }
    public class HoldModel
    {
        public int Id { get; set; }
        public DateTime HoldPlaced { get; set; }
        public bool? Issued { get; set; }
        public string PatronName { get; set; }

    }
    public class CheckoutIndex
    {
 

        public string ImageUrl { get; set; }
        public string LibraryCardId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserCardId { get; set; }
        public int NumberOfCopies { get; set; }
        public decimal Cost { get; set; }

        public int AssetId { get; set; }
    }
    public class AssetIndecList
    {
        public string SearchQuery { get; set; }

        public IEnumerable<AssetIndex> ListAssetIndex { get; set; }
    }
    public class AssetSearchList
    {
        public string SearchQuery { get; set; }
        public IEnumerable<AssetIndex> ListAssetIndex { get; set; }
        public bool EmptySearch { get; set; }

    }
    public class TopicSearchIndex
    {
        public string TopicTitle { get; set; }
        public int TopicId { get; set; }

    }

    public class TopicGenre
    {
        public TopicIndex Topic { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int AssetId { get; set; }
        public int TopicId { get; set; }
        public int TotalHistoryCount { get; set; }
        public string Description { get; set; }
        public int NumberOfCopies { get; set; }
        public int ReferenceCopies { get; set; }
        public decimal Costs { get; set; }

        public int CheckHistoryCount { get; set; }

    }
}
