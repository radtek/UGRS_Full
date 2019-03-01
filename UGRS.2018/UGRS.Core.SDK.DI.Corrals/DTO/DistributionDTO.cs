/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Distribution Data Object
Date: 16/08/2018
Company: Qualisys
*/
namespace UGRS.Core.SDK.DI.Corrals.DTO {
    /// <summary>
    /// Discribution of Food in Corrals 
    /// </summary>
    public class DistributionDTO {

        public string Code { get; set; }
        public string Name { get; set; }
        public string Whs { get; set; }
        public string Type { get; set; }
        public double Exist { get; set; }
        public double Food { get; set; }
        public double Bags { get; set; }
        public string Deliv { get; set; }
    }
}
