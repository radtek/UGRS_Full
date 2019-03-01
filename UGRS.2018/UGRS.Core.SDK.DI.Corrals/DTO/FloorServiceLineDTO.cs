/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Floor Service Line Data Object
Date: 16/08/2018
Company: Qualisys
*/
using System;
using System.Collections.Generic;

namespace UGRS.Core.SDK.DI.Corrals.DTO {
    /// <summary>
    /// Delivery of Food in Corrals
    /// </summary>
    public class FloorServiceLineDTO {

        public string Corral { get; set; }
        public string Batch { get; set; }

        public double Existence { get; set; }

        public double TotalDays { get; set; }

        public int DocEntry { get; set; }
    }
}
