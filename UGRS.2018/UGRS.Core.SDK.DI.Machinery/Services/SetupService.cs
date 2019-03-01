using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Exceptions;
using UGRS.Core.SDK.DI.Machinery.Tables;
using UGRS.Core.Services;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class SetupService
    {
        private TableDAO<Rise> mObjRiseDAO;
        private TableDAO<Employees> mObjRiseEmployeesDAO;
        private TableDAO<Contracts> mObjRiseContractsDAO;
        private TableDAO<Consumables> mObjRiseConsumablesDAO;
        private TableDAO<TravelExpenses> mObjRiseTravelExpensesDAO;
        private TableDAO<InitialRecords> mObjInitialRecordsDAO;
        private TableDAO<PurchaseOrders> mObjPurchaseOrdersDAO;
        private TableDAO<FinalsRecords> mObjFinalsRecordsDAO;
        private TableDAO<TotalRecords> mObjTotalRecordsDAO;
        private TableDAO<HoursRecords> mObjHoursRecordsDAO;
        private TableDAO<TransitHoursRecords> mObjTransitHoursRecordsDAO;
        private TableDAO<Performance> mObjPerformanceDAO;
        private TableDAO<DocumentsHistory> mObjDocumentsHistoryDAO;
        private TableDAO<Authorizations> mObjAuthorizationsDAO;
        private TableDAO<Commissions> mObjCommissionsDAO;

        public SetupService()
        {
            mObjRiseDAO = new TableDAO<Rise>();
            mObjRiseEmployeesDAO = new TableDAO<Employees>();
            mObjRiseContractsDAO = new TableDAO<Contracts>();
            mObjRiseConsumablesDAO = new TableDAO<Consumables>();
            mObjRiseTravelExpensesDAO = new TableDAO<TravelExpenses>();
            mObjInitialRecordsDAO = new TableDAO<InitialRecords>();
            mObjPurchaseOrdersDAO = new TableDAO<PurchaseOrders>();
            mObjFinalsRecordsDAO = new TableDAO<FinalsRecords>();
            mObjTotalRecordsDAO = new TableDAO<TotalRecords>();
            mObjHoursRecordsDAO = new TableDAO<HoursRecords>();
            mObjTransitHoursRecordsDAO = new TableDAO<TransitHoursRecords>();
            mObjPerformanceDAO = new TableDAO<Performance>();
            mObjDocumentsHistoryDAO = new TableDAO<DocumentsHistory>();
            mObjAuthorizationsDAO = new TableDAO<Authorizations>();
            mObjCommissionsDAO = new TableDAO<Commissions>();
        }

        public void InitializeTables()
        {
            LogService.WriteInfo(string.Format("[SetupService - InitializeTables]: {0}", "Inicializando tablas"));
            mObjRiseDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Subidas inicializada"));
            mObjRiseEmployeesDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Operadores inicializada"));
            mObjRiseContractsDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Contratos inicializada"));
            mObjRiseConsumablesDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Consumibles inicializada"));
            mObjRiseTravelExpensesDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Solicitud de Viáticos inicializada"));
            mObjInitialRecordsDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Regitros Inciales inicializada"));
            mObjPurchaseOrdersDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Compras/Traspasos inicializada"));
            mObjFinalsRecordsDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Registros Finales inicializada"));
            mObjTotalRecordsDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Registros Totales inicializada"));
            mObjHoursRecordsDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Horas inicializada"));
            mObjTransitHoursRecordsDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Horas Tránsito inicializada"));
            mObjPerformanceDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Rendimiento inicializada"));
            mObjDocumentsHistoryDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Historial de Subidas inicializada"));
            mObjAuthorizationsDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Autorizaciones inicializada"));
            mObjCommissionsDAO.Initialize();
            LogService.WriteSuccess(string.Format("[SetupService - InitializeTables]: {0}]", "Tabla Comisiones inicializada"));
            //InitializeRiseCommissionField();
        }

        private void InitializeRiseCommissionField()
        {
            SAPbobsCOM.UserFieldsMD lObjUserField = (SAPbobsCOM.UserFieldsMD)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
            try
            {
                if (!ExistsUFD("@UG_TBL_MQ_RISE", "HasCommission"))
                {
                    lObjUserField.TableName = "@UG_TBL_MQ_RISE";
                    lObjUserField.Name = "HasCommission";
                    lObjUserField.Description = "Comisión";
                    lObjUserField.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                    lObjUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_None;
                    lObjUserField.Size = 1;
                    lObjUserField.EditSize = 1;
                    lObjUserField.ValidValues.Value = "Y";
                    lObjUserField.ValidValues.Description = "Si";
                    lObjUserField.ValidValues.Add();
                    lObjUserField.ValidValues.Value = "N";
                    lObjUserField.ValidValues.Description = "No";
                    lObjUserField.ValidValues.Add();
                    lObjUserField.DefaultValue = "N";

                    lObjUserField.Add();
                }
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjUserField);
            }
        }

        public bool ExistsUFD(string tableName, string ufdName)
        {
            SAPbobsCOM.Recordset rs = DIApplication.GetRecordset();
            try
            {
                rs.DoQuery(string.Format("SELECT \"AliasID\" FROM \"CUFD\" WHERE \"TableID\" = '{0}' AND \"AliasID\" = '{1}'", tableName, ufdName));
                if (rs.RecordCount > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                //UIApplication.ShowError(string.Format("LabelServiceException: {0}", ex.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(rs);
            }
            return false;
        }
    }
}
