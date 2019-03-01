using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UGRS.AddOn.Finances.Entities;

namespace UGRS.AddOn.Finances.Formatters
{
    class ScotiabankFormatter : ExtractFormatter
    {

        public override string GetFileDialogFilter()
        {
            return "Archivos de texto|*.txt";
        }

        public override string GetFileDialogTitle(){
            return "Selecciona el archivo de ScotiaBank";
        }

        public override IList<BankStatement> ParseFile(string pPath, string pAcctCode)
        {
            string[] lArrStrLines = System.IO.File.ReadAllLines(pPath);
            IList<BankStatement> lLstObjResult = new List<BankStatement>();
            for (int i = 0; i < lArrStrLines.Length; i++)
            {
                BankStatement lObjExtractBanking = new BankStatement();

                string lStrFecha = lArrStrLines[i].Substring(26, 10);
                string lStrReferencia = lArrStrLines[i].Substring(36, 10);
                string lStrImporte = lArrStrLines[i].Substring(46, 17);
                string lStrCargoAbono = lArrStrLines[i].Substring(63, 5);
                string lStrSaldo = lArrStrLines[i].Substring(68, 17);
                string lStrTransaccion = lArrStrLines[i].Substring(85, 50);

                lObjExtractBanking.AccountCode = pAcctCode;
                lObjExtractBanking.Date = Convert.ToDateTime(lStrFecha);
                lObjExtractBanking.Reference = lStrReferencia;
                lObjExtractBanking.Detail = lStrTransaccion;
                if (lStrCargoAbono.ToUpper() == "CARGO")
                {
                    lObjExtractBanking.DebitAmount = Convert.ToDouble(lStrImporte);
                }
                else
                {
                    lObjExtractBanking.CreditAmount = Convert.ToDouble(lStrImporte);
                }
                lLstObjResult.Add(lObjExtractBanking);
            }
            return lLstObjResult;
        }
    }
}
