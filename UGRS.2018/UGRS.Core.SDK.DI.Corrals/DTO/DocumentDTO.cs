/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Document Data Object
Date: 16/08/2018
Company: Qualisys
*/
using System;
using System.Collections.Generic;

namespace UGRS.Core.SDK.DI.Corrals.DTO {
    /// <summary>
    /// Invoices Pending from Delivery
    /// </summary>
    public class DocumentDTO {
        public PendingInvoiceDTO Document { get; set; }

        public List<LivestockDTO> Lines { get; set; }

        public List<FloorServiceLineDTO> FloorServiceLines { get; set; }

        public List<DeliveryLine> DeliveryLines { get; set; }

        public List<BatchDTO> Batches { get; set; }

        public bool AuthProcess { get; set; }

    }
}