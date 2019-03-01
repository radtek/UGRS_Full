/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Document
Date: 18/09/2018
Company: Qualisys
*/


namespace UGRS.Core.SDK.DI.FoodTransfer.DTO {
    /// <summary>
    /// Document
    /// </summary>
    public class DocumentProduction {

        public int DocEntry { get; set; }
        public string DocNum { get; set; }
        public Component[] Lines { get; set; }
    }
}
