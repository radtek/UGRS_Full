/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description:Service Result Data Object
Date: 16/08/2018
Company: Qualisys
*/
namespace UGRS.Core.SDK.DI.FoodTransfer.DTO {
    /// <summary>
    /// Operation Result
    /// </summary>
    public class Result {

        public bool Success { get; set; }
        public string Message { get; set; }
        public int DocEntry { get; set; }
        public double DocTotal { get; set; }
    }
}
