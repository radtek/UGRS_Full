/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Livestock Data Object
Date: 16/08/2018
Company: Qualisys
*/
using System;

namespace UGRS.Core.SDK.DI.Corrals.DTO {
    /// <summary>
    /// Discribution of Food in Corrals 
    /// </summary>
    public class LivestockDTO {

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string Corral { get; set; }

        public string AuctDate { get; set; }

        public double Exist { get; set; }

        public double Quantity {get; set;}

        public double Import { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

    }
}
