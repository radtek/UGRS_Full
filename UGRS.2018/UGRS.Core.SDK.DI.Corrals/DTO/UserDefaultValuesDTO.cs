/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: User Default Values Data Object
Date: 16/08/2018
Company: Qualisys
*/
using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI;

namespace UGRS.Core.SDK.DI.Corrals.DTO {
    /// <summary>
    /// Delivery of Food in Corrals
    /// </summary>
    public class UserValues {

        public UserValues() {
            Name = DIApplication.Company.UserName;
            AppraisalValidation = false;
           
        }

        public string Name { get; set; }
        public string WhsCode { get; set; }
        public int Series { get; set; }
        public string Area { get; set; }
        public bool AppraisalValidation { get; set; }
    }

}
