using UGRS.Core.SDK.DI.Auctions.Services;

namespace UGRS.Core.SDK.DI.Auctions
{
    public class PaymentServiceFactory
    {
        public PaymentService GetPaymentService()
        {
            return new PaymentService();
        }

        public JounalEntryDI CreateDocument()
        {
            return new  JounalEntryDI();
        }

        public AuctionService GetAuctionService()
        {
            return new AuctionService();
        }

        public AlertService GetAlertService()
        {
            return new AlertService();
        }

    }

}
