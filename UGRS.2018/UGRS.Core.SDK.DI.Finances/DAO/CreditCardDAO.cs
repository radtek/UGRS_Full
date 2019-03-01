using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.Finances.DTO;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Finances.DAO
{
    public class CreditCardDAO
    {
        public IList<CreditCardDTO> GetCreditCards()
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<CreditCardDTO> lLstObjCreditCards = new List<CreditCardDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetCreditCards");
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        CreditCardDTO lObjCreditCard = new CreditCardDTO();
                        lObjCreditCard.CreditCard = Convert.ToInt32(lObjResults.Fields.Item("CreditCard").Value.ToString());
                        lObjCreditCard.CardName = lObjResults.Fields.Item("CardName").Value.ToString();
                        lObjCreditCard.AcctCode = lObjResults.Fields.Item("AcctCode").Value.ToString();
                        lObjCreditCard.Country = lObjResults.Fields.Item("Country").Value.ToString();
                        lLstObjCreditCards.Add(lObjCreditCard);
                        lObjResults.MoveNext();
                    }
                }
                return lLstObjCreditCards;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[CreditCardDAO - GetCreditCards] Error al obtener las tarjetas de credito: {0}", e.Message));
                throw new Exception(string.Format("Error al obtener las tarjetas de crédito: {0}", e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }
    }
}
