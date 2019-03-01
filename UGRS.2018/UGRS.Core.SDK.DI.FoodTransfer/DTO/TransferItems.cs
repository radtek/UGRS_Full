/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Transfers Items Data Table Object
Date: 03/09/2018
Company: Qualisys
*/

namespace UGRS.Core.SDK.DI.FoodTransfer.DTO {
    /// <summary>
    /// Food Plant Transfer
    /// </summary>
    public class TransferItem {
        public string Item { get; set; }
        public string Desc { get; set; }
        public double Quantity { get; set; }
        public double Bags { get; set; }
        public string FromWhs { get; set; }
        public string ToWhs { get; set; }
    }
}
