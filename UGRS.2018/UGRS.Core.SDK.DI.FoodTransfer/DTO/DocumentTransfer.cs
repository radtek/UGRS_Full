/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Transfers For Food Plant Data Object
Date: 31/08/2018
Company: Qualisys
*/



namespace UGRS.Core.SDK.DI.FoodTransfer.DTO {
    /// <summary>
    /// Food Plant Transfer
    /// </summary>
    public class DocumentTransfer {

        public PendingTransfer Document { get; set; }
        public TransferItem[] Lines { get; set; }
        public SeriesNumber[] Series { get; set; }
    }
}
