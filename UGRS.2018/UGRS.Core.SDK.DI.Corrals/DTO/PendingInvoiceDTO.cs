/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Pending Invoice Data Object
Date: 16/08/2018
Company: Qualisys
*/
using System;

namespace UGRS.Core.SDK.DI.Corrals.DTO {
    /// <summary>
    /// Invoices Pending from Delivery
    /// </summary>
    public class PendingInvoiceDTO {

        public string Code { get; set; }
        public string Name { get; set; }
        public double Debt { get; set; }
        public string Invoiced { get; set; }
    }

}
