/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Transfer Corrals to Action Data Access
Date: 29/08/2018
Company: Qualisys
*/


using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Corrals.DTO;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Corrals.DAO {

    public class TransferDAO {

        #region GetLivestockInCorrals
        public List<LivestockDTO> GetLivestockInCorrals(string client, string auctionDate) {

            Recordset recordset = null;
            var parameters = new Dictionary<string, string>();
            var livestockInCorrals = new List<LivestockDTO>();
            DistributionDAO distributionDAO = new DistributionDAO();

            try {
                var query = this.GetSQL("GetLivestock").Inject(parameters);
                var location = distributionDAO.GetUserDefaultWarehouse();
                parameters.Add("Location", location);
                query = ReplaceQueryConditions("AuctionDate", auctionDate, "AND T1.ExpDate = '{AuctionDate}'", parameters, query);
                query = ReplaceQueryConditions("CardCode", client, "AND T5.CardCode = '{CardCode}'", parameters, query);

                recordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery(query.Inject(parameters));

                if (recordset.RecordCount > 0) {
                    for (int i = 0; i < recordset.RecordCount; i++) {
                        var livestock = new LivestockDTO();
                        Parallel.ForEach(recordset.Fields.OfType<Field>(), field => {
                            livestock.GetType().GetProperty(field.Name).SetValue(livestock, field.Value);
                        });
                        livestockInCorrals.Add(livestock);
                        recordset.MoveNext();
                    }
                }
            }
            catch (Exception ex) {
                HandleException(ex, "GetLivestockInCorrals");
            }
            return livestockInCorrals.OrderBy(l => l.Name).ToList();
        }
        #endregion

        #region Other Methods
        private string ReplaceQueryConditions(string fieldName, string fieldValue, string queryCondition, Dictionary<string, string> parameters, string query) {
            if (!String.IsNullOrEmpty(fieldValue)) {
                parameters.Add(fieldName, fieldValue);
                return query;
            }
            else {
                return query.Replace(queryCondition, String.Empty);
            }
        }

        public static void HandleException(Exception ex, string section) {
            UIApplication.ShowMessageBox(ex.Message);
            LogService.WriteError(String.Format("{0}: {1}, {2}", section, ex.Message, ex.StackTrace)); ;
        }
        #endregion
    }
}
