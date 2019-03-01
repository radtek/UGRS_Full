/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Batch Data Object
Date: 16/08/2018
Company: Qualisys
*/
using System;
using System.Collections.Generic;

namespace UGRS.Core.SDK.DI.Corrals.DTO {
    /// <summary>
    /// Delivery of Food in Corrals
    /// </summary>
    public class BatchDTO {
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public string Batch { get; set; }
        public string Corral { get; set; }
        public string AuctDate { get; set; }

    }
}
