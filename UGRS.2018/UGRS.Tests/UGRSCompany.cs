using SAPbobsCOM;

namespace UGRS.Tests {

    public class UGRSap {

        Company company;

        public static Company GetCompany() {

            Company company = new Company();
            company.Server = "SAPB192PL10";
            company.CompanyDB = "UGRS_TEST";
            company.DbUserName = "sa";
            company.DbPassword = "Qualisys123";
            company.UserName = "operaciones";
            company.Password = "sap123";
            company.DbServerType = BoDataServerTypes.dst_MSSQL2012;

            return company.Connect() == 0 ? company : null;

        }
    }
}
