/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Delivery Data Object
Date: 16/08/2018
Company: Qualisys
*/
using System;
using System.Collections.Generic;

namespace UGRS.Core.SDK.DI.Corrals.DTO {
    /// <summary>
    /// Delivery of Food in Corrals
    /// </summary>
    public class DeliveryDTO {

        public DeliveryDTO() {
           this.DocLines = new List<DeliveryLines>();
        }
    
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public DateTime DocDate { get; set; }
        public List<DeliveryLines> DocLines { get; set; }
        public int Series { get; set; }
    }

    public class DeliveryLines {
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public double Quantity { get; set; }
        public string WhsCode { get; set; }
        public double BagsBales { get; set;}
        public string Corral { get; set; }
        public string Area { get; set; }
        public double Price { get; set; }
    }
}
