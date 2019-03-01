using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Corrals.DAO;
using UGRS.Core.SDK.DI.Corrals.DTO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Corrals.Services
{
    public class DraftService
    {
        public void DeleteDrafts(string pStrType)
        {
            try
            {
                DraftDAO lObjDraftDAO = new DraftDAO();
                List<DraftDTO> lLstDraft = lObjDraftDAO.GetDraftInvoices(pStrType);

                LogService.WriteInfo(string.Format("Iniciando proceso de eliminar preliminares de tipo {0}. Se procesarán {1} preliminares", pStrType, lLstDraft.Count));
                foreach (var lObjDraft in lLstDraft)
                {
                    //Load your original draft invoice.
                    SAPbobsCOM.Documents lObjDraftInvoice = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                    lObjDraftInvoice.GetByKey(lObjDraft.DocEntry);

                    if (lObjDraftInvoice.Remove() == 0)
                    {
                        System.Console.WriteLine(string.Format("Se eliminó correctamente el Preliminar con DocEntry {0}", lObjDraft.DocEntry));
                        LogService.WriteSuccess(string.Format("Se eliminó correctamente el Preliminar con DocEntry {0}", lObjDraft.DocEntry));
                    }
                    else
                    {
                        string lStrLastError = DIApplication.Company.GetLastErrorDescription();

                        System.Console.WriteLine(string.Format("Error al eliminar el Preliminar con DocEntry {0}: {1}", lObjDraft.DocEntry, lStrLastError));
                        LogService.WriteError(string.Format("Error al eliminar el Preliminar con DocEntry {0}: {1}", lObjDraft.DocEntry, lStrLastError));
                    }
                }

                LogService.WriteInfo("Proceso de eliminar preliminares terminado");
            }
            catch (Exception lObjException)
            {
                HandleException(lObjException, "GetDraftInvoices");
                throw lObjException;
            }
        }

        #region Handle Exception
        /// <summary>
        /// Handle Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="section"></param>
        public static void HandleException(Exception ex, string section)
        {
            UIApplication.ShowMessageBox(ex.Message);
            LogService.WriteError("DeliveryDI (CreateDocument) " + DIApplication.Company.GetLastErrorDescription());
        }
        #endregion
    }
}
