/*
 * Autor: Abraham Saúl Sandoval Meneses
 * Descriptión: Corrals Unit Tests
 * Date: 29/08/2018 
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.Corrals.DAO;


namespace UGRS.Tests {

    [TestClass]
    public class CorralsTests {

        [TestMethod]
        public void GetInvoicesPendingTest() {

            var distributionDAO = new DistributionDAO();
            var massInvoicingDAO = new MassInvoicingDAO();
            var transferDAO = new TransferDAO();

            DIApplication.DIConnect((SAPbobsCOM.Company)UGRSap.GetCompany());
            var results = massInvoicingDAO.GetInvoicesPending("N");
            Assert.IsTrue(results.Count > 0);


        }
    }
}

