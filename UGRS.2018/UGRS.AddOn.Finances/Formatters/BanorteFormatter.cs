using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UGRS.AddOn.Finances.Entities;

namespace UGRS.AddOn.Finances.Formatters
{
    class BanorteFormatter : ExtractFormatter
    {

        public override string GetFileDialogFilter()
        {
            return "Archivos de texto|*.txt";
        }

        public override string GetFileDialogTitle()
        {
            return "Selecciona el archivo de Banorte";
        }

        public override IList<BankStatement> ParseFile(string pPath, string pAcctCode)
        {
            string[] lArrStrLines = System.IO.File.ReadAllLines(pPath);
            DateTime dateValue;
            IList<BankStatement> lLstObjResult = new List<BankStatement>();
            for (int i = 0; i < lArrStrLines.Length; i++)
            {
                string[] lArrStrColumns = lArrStrLines[i].Split('|');
                if (DateTime.TryParse(lArrStrColumns[1], out dateValue))   //valide
                {
                    BankStatement lObjExtractBanking = new BankStatement();

                    lObjExtractBanking.AccountCode = pAcctCode;
                    lObjExtractBanking.Date = Convert.ToDateTime(lArrStrColumns[1]);
                    lObjExtractBanking.Reference = lArrStrColumns[3];
                    lObjExtractBanking.Detail = lArrStrColumns[4];
                    lObjExtractBanking.DebitAmount = Convert.ToDouble(Regex.Replace(lArrStrColumns[7], @"[$\,]", ""));
                    lObjExtractBanking.CreditAmount = Convert.ToDouble(Regex.Replace(lArrStrColumns[8], @"[$\,]", ""));

                    lLstObjResult.Add(lObjExtractBanking);
                }
            }
            return lLstObjResult;
        }
    }
}
