/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Food Plant Data Access Object
Date: 04/09/2018
Company: Qualisys
*/


using SAPbobsCOM;
using System;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.Permissions.DTO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Permissions.DAO {

    public class PendingSalesOrdersDAO {

        #region GetPendingSalesOrders
        public PendingOrderDTO[] GetPendingSalesOrders() {

            var recordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            PendingOrderDTO[] pendingOrders = null;

            try {

                var query = this.GetSQL("GetPendingSalesOrders");
                recordset.DoQuery(query);

                if(recordset.RecordCount > 0) {

                    pendingOrders = new PendingOrderDTO[recordset.RecordCount];
                    for(int i = 0; i < recordset.RecordCount; i++) {
                        var pendingOrder = new PendingOrderDTO();
                        foreach(Field field in recordset.Fields) {
                            pendingOrder.GetType().GetProperty(field.Name).SetValue(pendingOrder, field.Value);
                        }

                        pendingOrders[i] = pendingOrder;
                        recordset.MoveNext();
                    }
                }
            }
            catch(Exception ex) {
                HandleException(ex, "GetPendingSalesOrders");
            }
            finally {
                MemoryUtility.ReleaseComObject(recordset);
            }


            return pendingOrders;
        }
        #endregion

        #region HandleException
        public static void HandleException(Exception ex, string section) {
            UIApplication.ShowMessageBox(String.Format("Error: {0}", ex.Message));
            LogService.WriteError(String.Format("{0}: {1}, {2}", section, ex.Message, ex.StackTrace));
        }
        #endregion
    }
}
