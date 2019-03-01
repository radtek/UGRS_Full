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
    public class PaymentMethodDAO
    {
        public IList<PaymentMethodDTO> GetPaymentMethods()
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<PaymentMethodDTO> lLstObjPaymentMethods = new List<PaymentMethodDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetPaymentMethods");
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        PaymentMethodDTO lObjPaymentMethodDTO = new PaymentMethodDTO();
                        lObjPaymentMethodDTO.PayMethCod = lObjResults.Fields.Item("PayMethCod").Value.ToString();
                        lObjPaymentMethodDTO.Descript = lObjResults.Fields.Item("Descript").Value.ToString();
                        lLstObjPaymentMethods.Add(lObjPaymentMethodDTO);
                        lObjResults.MoveNext();
                    }
                }
                return lLstObjPaymentMethods;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[PaymentMethodDAO - GetPaymentMethods] Error al obtener los metodos de pago: {0}", e.Message));
                throw new Exception(string.Format("Error al obtener los métodos de pago: {0}", e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }
    }
}
