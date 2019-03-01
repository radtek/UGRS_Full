using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Exceptions;
using UGRS.Core.SDK.DI.Finances.Tables;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Finances.Services
{
    public class SetupService
    {
        private TableDAO<ExtractFormat> mObjBankDAO;
        private List<ExtractFormat> lLstOBjFormats = new List<ExtractFormat>(){
                new ExtractFormat(){RowCode="BANCOMER",RowName="BBVA Bancomer"},
                new ExtractFormat(){RowCode="SANTANDER",RowName="Santander"},
                new ExtractFormat(){RowCode="BANAMEX",RowName="Banamex"},
                new ExtractFormat(){RowCode="SCOTIABANK",RowName="Scotiabank"},
                new ExtractFormat(){RowCode="BANORTE",RowName="Banorte"},
        };

        public SetupService(){
            mObjBankDAO = new TableDAO<ExtractFormat>();
        }

        public void InitializeTables()
        {
            mObjBankDAO.Initialize();
            PopulateExtractTable();
            InitializeBankField();
        }

        private void AddExtractFormat(ExtractFormat pFormat)
        {
            SAPbobsCOM.UserTable lObjUserTable = mObjBankDAO.GetUserTable();
            try
            {
                
                lObjUserTable.Code = pFormat.RowCode;
                lObjUserTable.Name = pFormat.RowName;
                lObjUserTable.Add();
            }
            catch (Exception e)
            {
                throw new TableException(e.Message, e);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjUserTable);
            }
        }

        private void PopulateExtractTable()
        {
            
            foreach(ExtractFormat lObjFormat in lLstOBjFormats){
                try
                {
                    AddExtractFormat(lObjFormat);
                }
                catch (TableException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void InitializeBankField()
        {
            SAPbobsCOM.UserFieldsMD lObjUserField = (SAPbobsCOM.UserFieldsMD)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
            try
            {
                if (!Utils.ExistsUFD("ODSC", "FZ_ExtFormat"))
                {
                    lObjUserField.TableName = "ODSC";
                    lObjUserField.Name = "FZ_ExtFormat";
                    lObjUserField.Description = "Formato Extracto Bancario";
                    lObjUserField.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                    lObjUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_None;
                    lObjUserField.EditSize = 20;
                    lObjUserField.Size = 20;
                    lObjUserField.LinkedTable = "UG_FZ_BANKEXTRACTS";
                    lObjUserField.Add();
                }
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjUserField);
            }
        }
    }


}
