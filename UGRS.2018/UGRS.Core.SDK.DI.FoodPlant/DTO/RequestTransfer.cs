/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Stock Transfers Request
Date: 04/09/2018
Company: Qualisys
*/

namespace UGRS.Core.SDK.DI.FoodPlant.DTO {
    /// <summary>
    /// Stock Transfer Request
    /// </summary>
    public class RequestTransfer {
        public string Code { get; set; }
        public int Folio { get; set; }
        public string FromWhs { get; set; }
        public string ToWhs { get; set; }
        public string IFromWhs { get; set; }
        public string IToWhs { get; set; }
        public int LineNum { get; set; }
        public string Observations { get; set; }
        public string  Item { get; set; }
        public string  Desc { get; set; }
        public double  Quantity { get; set; }
    }
}


