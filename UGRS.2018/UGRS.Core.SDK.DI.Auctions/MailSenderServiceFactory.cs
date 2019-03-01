using UGRS.Core.SDK.DI.Auctions.Services;

namespace UGRS.Core.SDK.DI.Auctions
{
    public class MailSenderServiceFactory
    {
        public MailSenderService GetMailSenderService()
        {
            return new MailSenderService();
        }

        public AuctionSellersService GetAuctionSellersService()
        {
            return new AuctionSellersService();
        }
    }
}
