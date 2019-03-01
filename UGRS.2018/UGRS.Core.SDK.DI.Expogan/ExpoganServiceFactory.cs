using UGRS.Core.SDK.DI.Expogan.Services;

namespace UGRS.Core.SDK.DI.Expogan
{
    public class ExpoganServiceFactory
    {
        public LocationService GetLocationService()
        {
            return new LocationService();
        }

        public SetupService GetSetupService()
        {
            return new SetupService();
        }

        public LocationTableService GetLocationTableService()
        {
            return new LocationTableService();
        }

        public PurchaseOrderService GetPurchaseOrderService()
        {
            return new PurchaseOrderService();
        }
    }
}
