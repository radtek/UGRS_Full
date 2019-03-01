/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Transfers For Food Plant Data Object
Date: 31/08/2018
Company: Qualisys
*/

using System.Collections.Generic;


namespace UGRS.Core.SDK.DI.FoodPlant.DTO {
    /// <summary>
    /// Food Plant Transfer
    /// </summary>
    public class DocumentTransfer {

        public PendingTransfer Document { get; set; }
        public TransferItem[] Lines { get; set; }
        public string whs { get; set; }
    }
}
