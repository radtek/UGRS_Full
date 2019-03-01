using System.ComponentModel;

namespace UGRS.Core.SDK.DI.Transports.Enums
{
   
        public enum AuthorizerEnum : int
        {

            [Description("NoAut")]
            NoAut = 0,
            [Description("U_AutTrans")]
            AutTrans = 1,
            [Description("U_AutOperations")]
            AutOperations = 2,
            [Description("U_AutBanks")]
            AutBanks = 3
        }
    
}
