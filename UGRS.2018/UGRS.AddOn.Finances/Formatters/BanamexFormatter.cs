using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UGRS.AddOn.Finances.Entities;

namespace UGRS.AddOn.Finances.Formatters
{
    class BanamexFormatter : ExtractFormatter
    {
        public override string GetFileDialogFilter()
        {
            return "Text Files|*.txt";
        }

        public override string GetFileDialogTitle()
        {
            return "Selecciona el archivo de Banamex";
        }
        
        public override IList<BankStatement> ParseFile(string pPath, string pAcctCode)
        {
            string[] lArrStrLines = System.IO.File.ReadAllLines(pPath);
            DateTime dateValue;
            int lIntConsecutiveVal;
            IList<BankStatement> lLstObjResult = new List<BankStatement>();
            for (int i = 0; i < lArrStrLines.Length; i++)
            {
                string[] lArrStrColumns = lArrStrLines[i].Split('|');
                if ((DateTime.TryParse(lArrStrColumns[0], out dateValue)) && (lArrStrColumns.Count() < 5))  //valide
                {
                    BankStatement lObjExtractBanking = new BankStatement();

                    lObjExtractBanking.AccountCode = pAcctCode;
                    lObjExtractBanking.Date = Convert.ToDateTime(lArrStrColumns[0]);
                    lObjExtractBanking.Reference = lArrStrColumns[1].Substring(0, 16).Trim();
                    lObjExtractBanking.Detail = lArrStrColumns[1].Substring(17, 53).Trim();

                    if (lArrStrColumns[2] != "")
                    {
                        lObjExtractBanking.DebitAmount = Convert.ToDouble(lArrStrColumns[2]);
                    }
                    if (lArrStrColumns[3] != "")
                    {
                        lObjExtractBanking.CreditAmount = Convert.ToDouble(lArrStrColumns[3]);
                    }

                    lLstObjResult.Add(lObjExtractBanking);
                }
                else if (int.TryParse(lArrStrColumns[0], out lIntConsecutiveVal) && (lArrStrColumns.Count() <= 10))
                {
                    BankStatement lObjExtractBanking = new BankStatement();

                    lObjExtractBanking.AccountCode = pAcctCode;
                    lObjExtractBanking.Date = Convert.ToDateTime(lArrStrColumns[1]);
                    lObjExtractBanking.Reference = lArrStrColumns[9].ToString();
                    lObjExtractBanking.Detail = string.IsNullOrEmpty(lArrStrColumns[7].ToString()) ? string.Empty : lArrStrColumns[7].ToString();

                    if (lArrStrColumns[2] == "C")
                    {
                        lObjExtractBanking.DebitAmount = Convert.ToDouble(lArrStrColumns[8]);
                    }
                    if (lArrStrColumns[2] == "A")
                    {
                        lObjExtractBanking.CreditAmount = Convert.ToDouble(lArrStrColumns[8]);
                    }

                    lLstObjResult.Add(lObjExtractBanking);
                }
            }
            return lLstObjResult;
        }
    }
}
