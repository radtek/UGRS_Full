using UGRS.Core.SDK.DI.CyC.Services;

namespace UGRS.Core.SDK.DI.CyC
{
    public class CyCServiceFactory
    {
        public CyCServices GetCyCServices()
        {
            return new CyCServices();
        }

        public PaymentService GetPaymentService()
        {
            return new PaymentService();
        }

        public SetupService GetSetupService()
        {
            return new SetupService();
        }

        public ComentsService GetComentService()
        {
            return new ComentsService();
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
