/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Transfer Corrals to Auction Form
Date: 29/08/2018
Company: Qualisys
*/

using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Corrals.DAO;
using UGRS.Core.SDK.DI.Corrals.DTO;
using UGRS.Core.SDK.DI.Corrals.Services;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.AddOn.Corrals.Forms {

    [FormAttribute("UGRS.AddOn.Corrals.Forms.frmTransfer", "Forms/frmTransfer.b1f")]
    class frmTransfer : UserFormBase {

        #region Properties
        Dictionary<string, BoFieldsType> mtxColumns;
        TransferDAO transfer = new TransferDAO();
        List<LivestockDTO> livestockInCorals = new List<LivestockDTO>();
        List<BatchDTO> batches = new List<BatchDTO>();
        UserValues user = new UserValues();
        #endregion

        #region Constructor
        public frmTransfer() {

            try {

                Parallel.Invoke(
                    LoadEvents,
                    LoadChoosesFromList,
                    PrepareMatrix,
                    SetDefaultValues);
                //, SetFilters)// Marcaba error al momento de cargar otras ventanas
            }
            catch(AggregateException ae) {
                ae.Handle(e => {
                    HandleException(e, "Constructor");
                    return true;
                });
            }
        }
        #endregion

        #region DefaultValues
        public void SetDefaultValues() {
            DistributionDAO distributionDAO = new DistributionDAO();
            Task.Factory.StartNew(() => { user.Area = distributionDAO.GetUserCostCenter(); });
            Task.Factory.StartNew(() => { return distributionDAO.GetUserDefaultWarehouse(); })
                    .ContinueWith((t) => { user.WhsCode = t.Result; user.Series = distributionDAO.GetSeries(t.Result, SAPbobsCOM.BoObjectTypes.oStockTransfer.ToString()); });
        }
        #endregion

        #region Matrix
        public void PrepareMatrix() {

            mtxColumns = new Dictionary<string, BoFieldsType>() { { "#", BoFieldsType.ft_AlphaNumeric }, { "Code", BoFieldsType.ft_AlphaNumeric }, { "Name", BoFieldsType.ft_AlphaNumeric }, { "Corral", BoFieldsType.ft_AlphaNumeric }, { "Exist", BoFieldsType.ft_Float }, { "Quantity", BoFieldsType.ft_Float }, { "ItemCode", BoFieldsType.ft_AlphaNumeric }, { "ItemName", BoFieldsType.ft_AlphaNumeric }, { "AuctDate", BoFieldsType.ft_AlphaNumeric }, { "Result", BoFieldsType.ft_AlphaNumeric } };
            dt0 = CreateDataTable("DT0", mtxColumns);
            mtx0.Columns.Item("C_Result").Visible = false;
            mtx0.AutoResizeColumns();
        }


        private SAPbouiCOM.DataTable CreateDataTable(string tableID, Dictionary<string, BoFieldsType> columns) {

            DataTable dataTable = null;
            try {
                this.UIAPIRawForm.DataSources.DataTables.Add(tableID);
                dataTable = this.UIAPIRawForm.DataSources.DataTables.Item(tableID);

                columns.AsParallel().ForAll(column => {
                    dataTable.Columns.Add("C_" + column.Key, column.Value);
                });
            }
            catch(Exception ex) {
                HandleException(ex, "(Create DataTable)");
            }

            return dataTable;
        }


        private void FillMatrix0(string tableID, DataTable dataTable, bool processed) {

            try {

                dataTable.Rows.Clear();

                var columns = new List<string>(mtxColumns.Keys);
                var livestock = transfer.GetLivestockInCorrals(txtClient.Value, txtDate.Value);

                foreach (var column in columns)
                {
                    if (column == "AuctDate")
                        continue;

                    mtx0.Columns.Item(string.Format("C_{0}", column)).TitleObject.Sortable = true;
                    mtx0.Columns.Item(string.Format("C_{0}", column)).TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Ascending);
                }

                if(!processed) {
                    livestockInCorals = livestock;
                }

                if(livestock.Count > 0) {

                    Parallel.For(0, livestock.Count, row => {
                        dataTable.Rows.Add();
                    });

                    Task.Factory.StartNew(() => {
                        Parallel.For(0, livestock.Count, row => {
                            dataTable.SetValue("C_#", row, row + 1);
                        });
                    });

                    Parallel.ForEach(Partitioner.Create(0, livestock.Count), (range, state) => {
                        for(int i = range.Item1; i < range.Item2; i++) {
                            Parallel.ForEach(columns.Skip(1).Take(columns.Count - 2), column => {
                                dataTable.SetValue("C_" + column, i, livestock[i].GetType().GetProperty(column).GetValue(livestock[i], null));
                            });
                        }
                    });
                    BindDataMatrix(mtx0, tableID, columns);
                }
                else {
                    ClearMtx(mtx0);
                }
            }
            catch(AggregateException ae) {
                ae.Handle(e => {
                    HandleException(e, "(AE)");
                    return true;

                });
            }
            catch(Exception ex) {
                HandleException(ex, "(FillMatrix0)");
            }
        }

        public void BindDataMatrix(Matrix mtx, string tableID, List<string> columns) {

            this.UIAPIRawForm.Freeze(true);
            columns.AsParallel().ForAll(column => {
                if(column != "AuctDate")
                    mtx.Columns.Item("C_" + column).DataBind.Bind(tableID, "C_" + column);
            });

            mtx.LoadFromDataSource();
            mtx.AutoResizeColumns();
            this.UIAPIRawForm.Freeze(false);
        }

        private void BindResultColumn(ConcurrentDictionary<string, ResultDTO> results) {


            try {
                foreach(KeyValuePair<string, ResultDTO> result in results) {
                    Parallel.ForEach(Partitioner.Create(0, livestockInCorals.Count), (range, state) => {
                        for(int i = range.Item1; i < range.Item2; i++) {
                            if(livestockInCorals[i].Code == result.Key && livestockInCorals[i].Quantity > 0) {
                                dt0.SetValue("C_Result", i, result.Value.Message);
                            }
                        }
                    });
                }

                this.UIAPIRawForm.Freeze(true);
                mtx0.Columns.Item("C_Result").Visible = true;
                mtx0.LoadFromDataSource();
                mtx0.AutoResizeColumns();
                btnCreate.Item.Enabled = false;

                if(cbAll.Checked)
                    cbAll.Checked = false;
            }
            catch(Exception ex) {

                HandleException(ex, "BindResultColumn");
            }
            finally {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void ClearMtx(Matrix mtx) {
            this.UIAPIRawForm.Freeze(true);
            mtx.Clear();
            this.UIAPIRawForm.Freeze(false);
        }
        #endregion

        #region Events
        private void btnCancel_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent) {
            BubbleEvent = true;

            this.UnLoadEvents();
            this.UIAPIRawForm.Close();
        }

        private void btnSearch_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent) {
            BubbleEvent = true;

            if(!btnCreate.Item.Enabled)
                btnCreate.Item.Enabled = true;

            if(mtx0.Columns.Item("C_Result").Visible) {
                mtx0.Columns.Item("C_Result").Visible = false;
                mtx0.AutoResizeColumns();
            }

            Task.Factory.StartNew(() => FillMatrix0("DT0", dt0, false)).Wait();
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent) {
            BubbleEvent = true;
            try {

                if(FormUID.Equals(this.UIAPIRawForm.UniqueID)) {
                    if(!pVal.BeforeAction) {
                        switch(pVal.EventType) {
                            case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST:
                            ChooseFromListAfterEvent(pVal);
                            break;
                        }
                    }
                }
                else if(String.IsNullOrEmpty(pVal.ItemUID) && pVal.FormType == 10000075) {
                    if(this.UIAPIRawForm.Selected) {
                        switch(pVal.EventType) {
                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                            if(pVal.ActionSuccess) {
                                FillMatrix0("DT0", dt0, false);
                            }
                            break;
                        }
                    }
                }
            }
            catch(Exception ex) {
                HandleException(ex, "SBO_Application_ItemEvent");
            }
        }

        private void LoadEvents() {
            SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent += new _IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void UnLoadEvents() {
            SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent -= new _IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        public override void OnInitializeFormEvents() {
            this.UnloadAfter += new UnloadAfterHandler(this.Form_UnloadAfter);

        }

        private void OnCustomInitialize() { }
        #endregion

        #region OnInitializeComponent
        public override void OnInitializeComponent() {
            this.lblDate = ((SAPbouiCOM.StaticText)(this.GetItem("lblDate").Specific));
            this.lblClient = ((SAPbouiCOM.StaticText)(this.GetItem("lblClient").Specific));
            this.txtClient = ((SAPbouiCOM.EditText)(this.GetItem("txtClient").Specific));
            this.txtDate = ((SAPbouiCOM.EditText)(this.GetItem("txtDate").Specific));
            this.btnCreate = ((SAPbouiCOM.Button)(this.GetItem("btnCreate").Specific));
            this.btnCreate.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCreate_ClickBefore);
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCancel").Specific));
            this.btnCancel.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCancel_ClickBefore);
            this.mtx0 = ((SAPbouiCOM.Matrix)(this.GetItem("mtx0").Specific));
            this.btnSearch = ((SAPbouiCOM.Button)(this.GetItem("btnSearch").Specific));
            this.btnSearch.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnSearch_ClickBefore);
            this.cbAll = ((SAPbouiCOM.CheckBox)(this.GetItem("cbAll").Specific));
            this.OnCustomInitialize();

        }
        #endregion

        #region Controls
        private StaticText lblDate;
        private StaticText lblClient;
        private EditText txtClient;
        private EditText txtDate;
        private Button btnCreate;
        private Button btnCancel;
        private Button btnSearch;
        private Matrix mtx0;
        private DataTable dt0;
        private CheckBox cbAll;
        #endregion

        #region ChooseFromList
        private void LoadChoosesFromList() {
            ChooseFromList lObjCFLItem = InitChooseFromLists(false, "2", "CFL_Client", this.UIAPIRawForm.ChooseFromLists);
            SetChooseFromListToTxt();
        }

        public ChooseFromList InitChooseFromLists(bool pbol, string pStrType, string pStrID, SAPbouiCOM.ChooseFromListCollection pObjCFLs) {
            ChooseFromList lObjoCFL = null;
            try {
                ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = (ChooseFromListCreationParams)UIApplication.GetApplication().CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams);

                oCFLCreationParams.MultiSelection = pbol;
                oCFLCreationParams.ObjectType = pStrType;
                oCFLCreationParams.UniqueID = pStrID;

                lObjoCFL = pObjCFLs.Add(oCFLCreationParams);

                this.UIAPIRawForm.DataSources.UserDataSources.Add(pStrID, BoDataType.dt_SHORT_TEXT, 254);
            }
            catch(Exception ex) {
                UIApplication.ShowMessageBox(string.Format("InitCustomerChooseFromListException: {0}", ex.Message));

            }
            return lObjoCFL;
        }

        private void ChooseFromListAfterEvent(SAPbouiCOM.ItemEvent pObjValEvent) {

            if(pObjValEvent.Action_Success) {
                SAPbouiCOM.IChooseFromListEvent lObjCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pObjValEvent;
                SAPbouiCOM.DataTable lObjDataTable = lObjCFLEvento.SelectedObjects;
                if(lObjCFLEvento.SelectedObjects == null)
                    return;

                this.UIAPIRawForm.DataSources.UserDataSources.Item("CFL_Client").ValueEx = Convert.ToString(lObjDataTable.GetValue(0, 0));
            }
        }

        private void SetChooseFromListToTxt() {
            txtClient.DataBind.SetBound(true, "", "CFL_Client");
            txtClient.ChooseFromListUID = "CFL_Client";
        }
        #endregion

        #region SetLivestockTransferQuantities
        public void SetLivestockTransferQuantities(string column) {
            Task.Factory.StartNew(() => {
                Parallel.ForEach(Partitioner.Create(1, mtx0.RowCount + 1), (range, state) => {
                    for(int j = range.Item1; j < range.Item2; j++) {
                        livestockInCorals[j - 1].Quantity = Convert.ToDouble(((EditText)mtx0.Columns.Item(column).Cells.Item(j).Specific).Value == "" ? "0" : ((EditText)mtx0.Columns.Item(column).Cells.Item(j).Specific).Value);
                    }
                });
            }).Wait();
        }
        #endregion

        #region TransferCorralsToAuction
        private void btnCreate_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent) {
            BubbleEvent = true;

            try {
                if(!Validations()) {
                    return;
                }

                TransferCorralsToAuction();
            }
            catch(AggregateException ae) {
                ae.Handle(e => {
                    HandleException(e, "LivestockTransfer");
                    return true;
                });
            }
            catch(Exception ex) {
                HandleException(ex, "LivestockTransfer");
            }
        }

        private void TransferCorralsToAuction() {

            MassInvoicingDAO massInvoicingDAO = new MassInvoicingDAO();
            DistributionDAO distributionDAO = new DistributionDAO();

            LogService.WriteInfo("Begin Transfer Corrals to Actions");
            var results = new ConcurrentDictionary<string, ResultDTO>();
            livestockInCorals.Select(l => l.Code).Distinct().AsParallel().ForAll(client => {
                var transferLivestock = livestockInCorals.Where(l => l.Code == client && l.Quantity > 0).AsParallel().ToList();
                var batches = massInvoicingDAO.GetBatches(client, user.WhsCode, "N");
                results.TryAdd(client, LivestockTransfer.CreateStockTransfer(transferLivestock, batches, user.Series));
            });

            //another implemetation with same result
            /*var results = livestockInCorals.Select(l => l.Code).Distinct().AsParallel().Select(client => {
                var transferLivestock = livestockInCorals.Where(l => l.Code == client && l.Quantity > 0).AsParallel().ToList();
                var batches = massInvoicingDAO.GetBatches(client, user.WhsCode, "N");
                return new KeyValuePair<string, ResultDTO>(client, LivestockTransfer.CreateStockTransfer(transferLivestock, batches, user.Series));
            }).ToDictionary(t => t.Key, t => t.Value);*/

            Task.Factory.StartNew(() => { 
                BindResultColumn(results);
                LogService.WriteInfo("Begin Transfer Corrals to Actions");
            });
        }
        #endregion

        #region Validations
        bool Validations() {

            if(livestockInCorals.Count == 0 || !btnCreate.Item.Enabled) {
                return false;
            }

            if(cbAll.Checked)
                if(UIApplication.ShowOptionBox("Estas seguro de trasladar todo el ganado a subsata") == 2)
                    return false;

            SetLivestockTransferQuantities(cbAll.Checked ? "C_Exist" : "C_Quantity");

            if(livestockInCorals.Where(l => l.Quantity == 0).AsParallel().Count() == livestockInCorals.Count) {
                UIApplication.ShowMessageBox("No se ha ingresado cantidad para dar traslado al ganado");
                return false;
            }

            if(!Task.Factory.StartNew(() => {
                return Parallel.ForEach(Partitioner.Create(0, livestockInCorals.Count), (range, state) => {
                    for(int i = range.Item1; i < range.Item2; i++)
                        if(livestockInCorals[i].Quantity > livestockInCorals[i].Exist)
                            state.Stop();
                }).IsCompleted;
            }).Result) {
                UIApplication.ShowMessageBox("La cantidad no debe ser mayor que la existencia");
                return false;
            }
            return true;
        }
        #endregion

        private void Form_UnloadAfter(SBOItemEventArg pVal) {
            UnLoadEvents();
        }

        #region Handle Exception

        public void HandleException(Exception ex, string section) {

            UIApplication.ShowMessageBox(ex.Message);
            LogService.WriteError(String.Format("{0}: {1}, {2}", section, ex.Message, ex.StackTrace));
        }
        #endregion
    }
}
