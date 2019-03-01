/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: User Default Values Data Object
Date: 04/09/2018
Company: Qualisys
*/
using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.FoodPlant.DAO;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.FoodPlant.DTO {
    /// <summary>
    /// Delivery of Food in Corrals
    /// </summary>
    public class User {

        FoodPlantDAO foodPlantDAO = new FoodPlantDAO();

        public User() {
            Name = DIApplication.Company.UserName;
            Task.Factory.StartNew(() => { return foodPlantDAO.GetUserDefaultWarehouse(); })
                     .ContinueWith((t) => { WhsCode = t.Result; Series = foodPlantDAO.GetSeries(t.Result, SAPbobsCOM.BoObjectTypes.oStockTransfer.ToString()); });
            Task.Factory.StartNew(() => { Area = foodPlantDAO.GetUserCostCenter(); });
        }

        public string Name { get; set; }
        public string WhsCode { get; set; }
        public int Series { get; set; }
        public string Area { get; set; }
    }

}
