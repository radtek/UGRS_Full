/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Transfers For Food Plant Data Object
Date: 31/08/2018
Company: Qualisys
*/

namespace UGRS.Core.SDK.DI.FoodPlant.DTO {
    /// <summary>
    /// Food Plant Transfer
    /// </summary>
    public class PendingTransfer {
        public int Folio { get; set; }
        public string DocDate { get; set; }
        public string Comments { get; set; }
        public string FromWhs { get; set; }
        public string ToWhs {get; set; }
    }
}
