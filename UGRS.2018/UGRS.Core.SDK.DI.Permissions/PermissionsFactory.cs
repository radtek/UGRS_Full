using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.AddOn.Permissions.Services;
using UGRS.Core.SDK.DI.Permissions.Services;

namespace UGRS.Core.SDK.DI.Permissions
{
    public class PermissionsFactory
    {
        public SetupService GetSetupService()
        {
            return new SetupService();
        }

        public EarringRanksService GetEarringRanksService()
        {
            return new EarringRanksService();
        }

        public PermissionsService GetPermissionsService()
        {
            return new PermissionsService();
        }

        public CreditNoteDI GetCreditNoteService()
        {
            return new CreditNoteDI();
        }
    }
}
