using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.Services;

namespace UGRS.Core.SDK.DI.Machinery
{
    public class MachinerySeviceFactory
    {
        public SetupService GetSetupService()
        {
            return new SetupService();
        }

        public RiseService GetRiseService()
        {
            return new RiseService();
        }

        public UsersService GetUsersService()
        {
            return new UsersService();
        }

        public AuthorizationService GetAuthorizationService()
        {
            return new AuthorizationService();
        }

        public ConsumablesService GetConsumablesService()
        {
            return new ConsumablesService();
        }

        public ContractsService GetContractsService()
        {
            return new ContractsService();
        }

        public ConstructionService GetConstructionService()
        {
            return new ConstructionService();
        }

        public MunicipalitiesService GetMunicipalitiesService()
        {
            return new MunicipalitiesService();
        }

        public CommitteesServices GetCommitteesServices()
        {
            return new CommitteesServices();
        }

        public EquipmentsService GetEquipmentsService()
        {
            return new EquipmentsService();
        }

        public ArticlesService GetArticlesService()
        {
            return new ArticlesService();
        }

        public ClientsService GetClientsService()
        {
            return new ClientsService();
        }

        public SectionsService GetSectionsService()
        {
            return new SectionsService();
        }

        public ConfigurationsService GetConfigurationsService()
        {
            return new ConfigurationsService();
        }

        public AddressService GetAddressService()
        {
            return new AddressService();
        }

        public TravelExpensesService GetTravelExpensesService()
        {
            return new TravelExpensesService();
        }

        public EmployeesService GetEmployeesService()
        {
            return new EmployeesService();
        }

        public InitialRecordsService GetInitialRecordsService()
        {
            return new InitialRecordsService();
        }

        public PurchasesOrdersService GetPurchasesOrdersService()
        {
            return new PurchasesOrdersService();
        }

        public FinalsRecordsService GetFinalsRecordsService()
        {
            return new FinalsRecordsService();
        }

        public TotalsRecordsService GetTotalsRecordsService()
        {
            return new TotalsRecordsService();
        }

        public HoursRecordsService GetHoursRecordsService()
        {
            return new HoursRecordsService();
        }

        public TransitHoursRecordsService GetTransitHoursRecordsService()
        {
            return new TransitHoursRecordsService();
        }

        public PerformanceService GetPerformanceService()
        {
            return new PerformanceService();
        }

        public GoodIssuesService GetGoodIssuesService()
        {
            return new GoodIssuesService();
        }

        public CommissionsService GetCommissionsService()
        {
            return new CommissionsService();
        }
    }
}
