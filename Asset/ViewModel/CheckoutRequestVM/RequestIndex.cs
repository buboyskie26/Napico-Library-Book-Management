using Asset.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModel.CheckoutRequestVM
{
    public class RequestIndex
    {
        public int Id { get; set; }
        public string TopicName { get; set; }
        public int AssetId { get; set; }
        public bool IsClick { get; set; }
        public int LibraAssetId { get; set; }
        public AssetBuild LibraryAssets { get; set; }
        public int CardId { get; set; }
        public bool? Approve { get; set; }
        public DateTime Start { get; set; }
        public DateTime RequestDate { get; set; }
        public string UserId { get; set; }
        public string AssetName { get; set; }
        public string UserName { get; set; }

        public int Ratified { get; set; }
        public int Declined { get; set; }
        public int Pending { get; set; }
        public DateTime Until { get; set; } = DateTime.UtcNow;

        public TopicRequestObj TopicObj { get; set; }

    }
    public class TopicRequestObj
    {
        public string TopicName { get; set; }
        public string TopicDesc { get; set; }
        public int TopicId { get; set; }

    }
    public class ListRequest
    {
        public IEnumerable<RequestIndex> RequestList { get; set; }
        public AssetBuild LibraryAssets { get; set; }
        public TopicRequestObj TopicObj { get; set; }

    }
    public class AssetBuild
    {
        public int Id { get; set; }
 
        public DateTime Start { get; set; }
        public string AssetName { get; set; }
        public decimal AssetCost { get; set; }

        public DateTime Until { get; set; }  
    }
}
