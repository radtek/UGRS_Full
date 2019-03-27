using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UGRS.AddOn.Purchases.Services;
using UGRS.Core.SDK.DI;
using servTim = UGRS.AddOn.Purchases.TimbradoSoap33Prodigia;
using System.Threading.Tasks;
using System.Threading;

namespace UGRS.Tests {
    [TestClass]
    public class PurchasesTests {

        [TestMethod]
        public void CheckXMLStatusTest() {

            DIApplication.DIConnect((SAPbobsCOM.Company)UGRSap.GetCompany());

            try {

                var file = @"C:\Users\ssandoval\Desktop\Qualisys Saul\PROJECTS\Union Ganadera\Compras\XML Enero- Feb\1b123a60-karla consuelo franco guitierrez.xml";
                var lObjReadXML = new ReadXMLService();
                var objectXML = lObjReadXML.ReadXML(file);
                var result = lObjReadXML.CheckVoucherStatus(objectXML);

                Assert.IsTrue(result);
                return;

            }
            catch(Exception ex) {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void CheckCFDIByUUID() {
            try {
                var mObjTimbradorp = new servTim.PadeTimbradoServiceClient();
                var lStrCFDI = mObjTimbradorp.cfdiPorUUID("1d3027c6-5c49-11e3-a2a4-109add4fad20", "factugrs", "A123456789$", "9E5B72AA-11F4-4E2E-BAF2-E4D83465784B");
                var xmlBase64 = XDocument.Parse(lStrCFDI).Document.Descendants("servicioConsulta").Elements("xmlBase64").FirstOrDefault().Value;

                var decodedBase64XML = DecodeBase64(xmlBase64);

                Assert.AreNotEqual(decodedBase64XML, null);
                Assert.AreNotEqual(decodedBase64XML, String.Empty);
                Assert.IsTrue(decodedBase64XML.Contains("<?xml"));
                return;
            }
            catch(Exception ex) {
                Assert.Fail(ex.Message);
            }
        }

        public string DecodeBase64(string base64Encoded) {

            byte[] data = System.Convert.FromBase64String(base64Encoded);
            return ASCIIEncoding.ASCII.GetString(data);
        }
    }
}








