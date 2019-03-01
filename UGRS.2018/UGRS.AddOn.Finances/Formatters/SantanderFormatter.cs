using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UGRS.AddOn.Finances.Entities;

namespace UGRS.AddOn.Finances.Formatters
{
    class SantanderFormatter : ExtractFormatter
    {
        public override string GetFileDialogFilter()
        {
            return "Texto separado por comas|*.csv";
        }

        public override string GetFileDialogTitle()
        {
            return "Selecciona el archivo de Santander";
        }

        public override IList<BankStatement> ParseFile(string pPath, string pAcctCode)
        {
            string[] lArrStrLines = System.IO.File.ReadAllLines(pPath);
            int dateValue;
            IList<BankStatement> lLstObjResult = new List<BankStatement>();
            for (int i = 0; i < lArrStrLines.Length; i++)
            {
                string[] lArrStrColumns = lArrStrLines[i].Split(',');
                if (int.TryParse(Regex.Replace(lArrStrColumns[1].ToString(), @"[^\w]", ""), out dateValue))   //valide
                {
                    BankStatement lObjExtractBanking = new BankStatement();

                    string lStrConvertFecha = Regex.Replace(lArrStrColumns[1].ToString(), @"[^\w]", "");
                    string lStrCargoAbono = lArrStrColumns[5].ToString();
                    string lStrImporte = Regex.Replace(lArrStrColumns[6].ToString(), @"[^\w]", "");

                    lObjExtractBanking.AccountCode = pAcctCode;
                    lObjExtractBanking.Date = DateTime.ParseExact(lStrConvertFecha, "ddMMyyyy", CultureInfo.InvariantCulture);
                    lObjExtractBanking.Reference = Regex.Replace(lArrStrColumns[8].ToString(), @"[^\w]", "");
                    lObjExtractBanking.Detail = Regex.Replace(lArrStrColumns[4].ToString(), @"[^\w]", " ").Trim();

                    lStrCargoAbono = lStrCargoAbono.ToArray().Contains('+') ? "ABONO" : "CARGO";

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
            }
            return lLstObjResult;
        }
    }
}
