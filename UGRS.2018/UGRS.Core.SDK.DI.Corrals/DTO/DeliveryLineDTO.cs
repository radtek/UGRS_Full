/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Delivery Line Data Object
Date: 16/08/2018
Company: Qualisys
*/
using System;
using System.Collections.Generic;

namespace UGRS.Core.SDK.DI.Corrals.DTO {
    /// <summary>
    /// Lines for Delivery Document that will be added to Invoice Document
    /// </summary>
    public class DeliveryLine {
        public int LineNum { get; set; }
        public string ItemCode {get; set;}
        public double Price { get; set; }
        public int DocEntry {get; set;}
        public double Quantity { get; set; }
    }

}