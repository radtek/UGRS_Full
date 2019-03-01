using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using UGRS.AddOn.Finances.Entities;

namespace UGRS.AddOn.Finances.Formatters
{
    class BancomerFormatter : ExtractFormatter
    {
        public override string GetFileDialogFilter()
        {
            return "Archivo XML|*.xml|Archivo Excel|*.xls";
        }

        public override string GetFileDialogTitle()
        {
            return "Selecciona el archivo de Banamex";
        }
        
        public override IList<BankStatement> ParseFile(string pPath, string pAcctCode)
        {
            IList<BankStatement> lLstObjResult = new List<BankStatement>();
            string lStrContent = System.IO.File.ReadAllText(pPath);
            XmlDocument lObjXmlDoc = new XmlDocument();
            lObjXmlDoc.LoadXml(lStrContent);
            XmlNodeList lObjNodeRows = lObjXmlDoc.GetElementsByTagName("Row");
            int i = 0;
            foreach (XmlNode lObjNodeRow in lObjNodeRows)
            {
                if (i >= 2)
                {
                    if (lObjNodeRow.ChildNodes.Count < 7)
                    {
                        continue;
                    }
                    BankStatement lObjExtractBanking = new BankStatement();
                    lObjExtractBanking.AccountCode = pAcctCode;
                    lObjExtractBanking.Date = DateTime.ParseExact(lObjNodeRow.ChildNodes.Item(0).InnerText, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    lObjExtractBanking.Reference = lObjNodeRow.ChildNodes.Item(2).InnerText;
                    lObjExtractBanking.Detail = lObjNodeRow.ChildNodes.Item(3).InnerText;
                    Double lDblTemp = 0;
                    if (Double.TryParse(lObjNodeRow.ChildNodes.Item(4).InnerText, out lDblTemp))
                    {
                        lObjExtractBanking.DebitAmount = lDblTemp;
                    }
                    if (Double.TryParse(lObjNodeRow.ChildNodes.Item(5).InnerText, out lDblTemp))
                    {
                        lObjExtractBanking.CreditAmount = lDblTemp;
                    }
                    lLstObjResult.Add(lObjExtractBanking);
                }
                i++;

            }
            return lLstObjResult;
        }
    }
}
