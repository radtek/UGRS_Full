using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Transports.Services;

namespace UGRS.Core.SDK.DI.Transports
{
    public class TransportServiceFactory
    {
        public SetupService GetSetUpService()
        {
            return new SetupService();
        }

        public RoutesService GetRouteService()
        {
            return new RoutesService();
        }

        public VehiclesService GetVehiclesService()
        {
            return new VehiclesService();
        }

        public CFLService GetCFLService()
        {
            return new CFLService();
        }

        public JournalService GetJournalService()
        {
            return new JournalService();
        }

        public CommissionService GetCommissionService()
        {
            return new CommissionService();
        }

        public CommissionLineService GetCommissionLineService()
        {
            return new CommissionLineService();
        }

        public AlertService GetAlertService()
        {
            return new AlertService();
        }
    }
}
