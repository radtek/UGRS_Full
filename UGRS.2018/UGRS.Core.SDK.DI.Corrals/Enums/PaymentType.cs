/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: PaymentType Emuneration
Date: 16/08/2018
Company: Qualisys
*/
using System.ComponentModel;

namespace UGRS.AddOn.Purchases.Enums{

        public enum PaymentType : int{

            [Description("Cobro Normal")]
            Normal = 0,

            [Description("Cobro Cierre")]
            Closure = 1,
    
            [Description("Facturación Subasta")]
            Auction = 2,

    }
}
