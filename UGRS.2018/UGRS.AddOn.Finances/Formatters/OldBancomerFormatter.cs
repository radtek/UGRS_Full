using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UGRS.AddOn.Finances.Entities;

namespace UGRS.AddOn.Finances.Formatters
{
    class OldBancomerFormatter
    {
        public string GetFileExtension()
        {
            return ".txt";
        }

        public string GetFileDialogFilter()
        {
            return "Hoja de cálculo|*.xls";
        }

        public string GetFileDialogTitle()
        {
            return "Selecciona el archivo de Bancomer";
        }

        public IList<BankStatement> ParseLines(string[] pArrStrLines, string pAcctCode)
        {
            return new List<BankStatement>();
        }

        public IList<BankStatement> ParseLines(System.Data.DataTable pDtbStrLines, string pAcctCode)
        {
            DateTime dateValue;
            IList<BankStatement> lLstObjResult = new List<BankStatement>();
            for (int i = 0; i < pDtbStrLines.Rows.Count; i++)
            {
                if (DateTime.TryParse(pDtbStrLines.Rows[i].ItemArray[0].ToString(), out dateValue))   //valide
                {
                    BankStatement lObjExtractBanking = new BankStatement();

                    string lStrFecha = pDtbStrLines.Rows[i].ItemArray[0].ToString();
                    string lStrConcepto = pDtbStrLines.Rows[i].ItemArray[1].ToString();
                    string lStrReferencia = pDtbStrLines.Rows[i].ItemArray[2].ToString();
                    string lStrCargo = pDtbStrLines.Rows[i].ItemArray[4].ToString();
                    string lStrAbono = pDtbStrLines.Rows[i].ItemArray[5].ToString();

                    lObjExtractBanking.AccountCode = pAcctCode;
                    lObjExtractBanking.Date = Convert.ToDateTime(lStrFecha);
                    lObjExtractBanking.Reference = lStrReferencia;
                    lObjExtractBanking.Detail = lStrConcepto;
                    if (lStrCargo != "")
                        lObjExtractBanking.DebitAmount = Convert.ToDouble(Regex.Replace(lStrCargo, @"[$\,]", ""));
                    if (lStrCargo == "")
                        lObjExtractBanking.DebitAmount = 0;
                    if (lStrAbono != "")
                        lObjExtractBanking.CreditAmount = Convert.ToDouble(Regex.Replace(lStrAbono, @"[$\,]", ""));
                    if (lStrAbono == "")
                        lObjExtractBanking.CreditAmount = 0;

                    lLstObjResult.Add(lObjExtractBanking);
                }
            }
            return lLstObjResult;
        }

        public DataTable GetDtbFile(string pStrPath)
        {
            string lStrConexion = "", lStrSheetName = "";
            int lIntCountSheets = 0;
            OleDbConnection lOleConnection = new OleDbConnection();
            OleDbCommand lOleCommand = new OleDbCommand();
            DataTable lDtbSheet = new DataTable();
            DataTable lDtbFile = new DataTable();
            OleDbDataAdapter lOleDataAdapter = new OleDbDataAdapter();

            if (System.IO.Path.GetExtension(pStrPath) == ".xlsx")
                lStrConexion = "Provider=Microsoft.ACE.OLEDB.12.0; Extended Properties=Excel 12.0 XML; Data Source=" + pStrPath + ";";
            using (lOleConnection = new OleDbConnection(lStrConexion))
            {
                lOleConnection.Open();
                lOleCommand.Connection = lOleConnection;
                lDtbSheet = lOleConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);   // Get all Sheets in Excel File                    
                lIntCountSheets = 1;
                foreach (DataRow dr in lDtbSheet.Rows) // Loop through all Sheets to get data
                {
                    if (lIntCountSheets == 1)
                    {
                        lStrSheetName = dr["TABLE_NAME"].ToString();
                        if (!lStrSheetName.EndsWith("$"))
                            continue;
                        lOleCommand.CommandText = "SELECT * FROM [" + lStrSheetName + "]";  // Get all rows from the Sheet
                        lDtbFile.TableName = lStrSheetName;
                        lOleDataAdapter = new OleDbDataAdapter(lOleCommand);
                        lOleDataAdapter.Fill(lDtbFile);
                    }
                    lIntCountSheets++;
                }
            }
            return lDtbFile;
        }
    }
}
