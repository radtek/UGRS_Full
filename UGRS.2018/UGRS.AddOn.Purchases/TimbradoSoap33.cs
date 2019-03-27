using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using servTim = UGRS.AddOn.Purchases.TimbradoSoap33Prodigia;

namespace UGRS.AddOn.Purchases
{
    public class TimbradoSoap33
    {
        private string url;
        private string Contract;
        private string User;
        private string Passw;
        servTim.PadeTimbradoServiceClient mObjTimbradorp;
        private string pathCert;
        private string pathKey;
        private string passCert;


        public string GetCFDI(string pStrContract, string pStrUser, string pStrPassw, string pStrUUID)
        {
            string lStrCFDI = mObjTimbradorp.cfdiPorUUID(pStrContract, pStrUser, pStrPassw, pStrUUID);
            return lStrCFDI;
        }
    
        private Byte[] FileToBytesArray(FileStream file)
        {
            byte[] fileByte = new byte[file.Length];
            file.Read(fileByte, 0, Convert.ToInt32(file.Length));
            return fileByte;
        }
    }
}
