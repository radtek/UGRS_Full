/*
 * Autor: Abraham Saúl Sandoval Meneses
 * Descriptión: Corrals Unit Tests
 * Date: 29/08/2018 
 */

using System;
using UGRS.Core;
using UGRS.Core.SDK;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UGRS.Core.SDK.DI.Corrals.DAO;
using UGRS.Core.SDK.DI.Corrals.DTO;
using UGRS.Core.SDK.DI.Corrals.Services;
using System.Threading.Tasks;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework; 

namespace UGRS.Tests {
    

    [TestClass]
    public class CorralsTests {

        [TestMethod]
        public void Test() {

            DIApplication.DIConnect((SAPbobsCOM.Company)SAPbouiCOM.Framework.Application.SBO_Application.Company.GetDICompany());


            
       
            
         
        }

     


    }
}
