
/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Series Number for Transfer Items
Date: 13/09/2018
Company: Qualisys
*/


namespace UGRS.Core.SDK.DI.FoodTransfer.DTO {
    /// <summary>
    /// Series Number
    /// </summary>
    public class SeriesNumber {
        public string ItemCode { get; set; }
        public string Number { get; set; }
        public double Quantity { get; set; }
        public int SysNumber { get; set; }
    }
}
