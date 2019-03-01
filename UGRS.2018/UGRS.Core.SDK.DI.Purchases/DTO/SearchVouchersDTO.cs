using UGRS.Core.SDK.DI.Purchases.Tables;

namespace UGRS.Core.SDK.DI.Purchases.DTO
{
    public class SearchVouchersDTO : Vouchers
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SearchStatus { get; set; }
    }
}
