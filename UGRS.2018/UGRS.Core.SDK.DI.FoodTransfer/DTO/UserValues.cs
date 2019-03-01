/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: User Default Values Data Object
Date: 04/09/2018
Company: Qualisys
*/
using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.FoodTransfer.DAO;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.FoodTransfer.DTO {
    /// <summary>
    /// Delivery of Food in Corrals
    /// </summary>
    public class User {

        FoodTransferDAO foodPlantDAO = new FoodTransferDAO();

        public User(string objectCode) {

            Name = DIApplication.Company.UserName;
            var task = Task.Factory.StartNew(() => { return foodPlantDAO.GetUserDefaultWarehouse(); })
                     .ContinueWith((t) => {
                         WhsCode = t.Result.Equals("OFGE") ? "CUNO" : t.Result;
                         Series = 0; //foodPlantDAO.GetSeries(t.Result, objectCode, "Series");
                         IsFoodPlant = foodPlantDAO.GetUserType(DIApplication.Company.UserName);
                     });
            var task2 = Task.Factory.StartNew(() => { Area = foodPlantDAO.GetUserCostCenter(); });
            Task.WaitAll(task, task2);   
        }

        public string Name { get; set; }
        public string WhsCode { get; set; }
        public int Series { get; set; }
        public string Area { get; set; }
        public string FormID { get; set; }
        public bool IsFoodPlant { get; set; }
    }

}
