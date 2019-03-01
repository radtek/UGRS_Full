using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.Core.SDK.DI.Machinery;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.DI;
using UGRS.AddOn.Machinery.Enums;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.Utility;

namespace UGRS.AddOn.Machinery.Forms
{
    [FormAttribute("UGRS.AddOn.Machinery.Forms.frmStockTransfer", "Forms/frmStockTransfer.b1f")]
    class frmStockTransfer : UserFormBase
    {
        #region Properties
        private MachinerySeviceFactory mObjMachineryServiceFactory = null;
        private frmCFLFolios mObjFrmFolios = null;
        #endregion

        #region Constructor
        public frmStockTransfer()
        {
            mObjMachineryServiceFactory = new MachinerySeviceFactory();

            LoadEvents();
            LoadInitialsControls();
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lblRiseFolio = ((SAPbouiCOM.StaticText)(this.GetItem("lblFolio").Specific));
            this.txtRiseFolio = ((SAPbouiCOM.EditText)(this.GetItem("txtFolio").Specific));
            this.btnSearch = ((SAPbouiCOM.Button)(this.GetItem("btnSearch").Specific));
            this.btnSearch.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnSearch_ClickBefore);
            this.btnCreate = ((SAPbouiCOM.Button)(this.GetItem("btnCreate").Specific));
            this.btnCreate.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCreate_ClickBefore);
            this.mtxItems = ((SAPbouiCOM.Matrix)(this.GetItem("mtxItems").Specific));
            this.lblDate = ((SAPbouiCOM.StaticText)(this.GetItem("lblDate").Specific));
            this.txtDate = ((SAPbouiCOM.EditText)(this.GetItem("txtDate").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.CloseBefore += new SAPbouiCOM.Framework.FormBase.CloseBeforeHandler(this.Form_CloseBefore);
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);

        }

        private void OnCustomInitialize()
        {

        }
        #endregion

        #region Load & Unload Events
        private void LoadEvents()
        {
            UIApplication.GetApplication().ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void UnLoadEvents()
        {
            UIApplication.GetApplication().ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }
        #endregion

        #region EventsHandle
        /// <summary>
        /// SBO_Application_ItemEvent
        /// Metodo para controlar los eventos de la pantalla.
        /// @Author FranciscoFimbres
        /// </summary>
        /// <param name="FormUID"></param>
        /// <param name="pVal"></param>
        /// <param name="BubbleEvent"></param>
        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //string y = pVal.CharPressed.ToString();
            try
            {
                if (pVal.FormTypeEx.Equals("UGRS.AddOn.Machinery.Forms.frmCFLFolios"))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                if (mObjFrmFolios != null)
                                {
                                    if (string.IsNullOrEmpty(mObjFrmFolios.mStrFolio))
                                        return;

                                    txtRiseFolio.Value = mObjFrmFolios.mStrFolio;

                                    GetItemsDetails(txtRiseFolio.Value);
                                }
                                break;
                        }
                    }
                }

                if (FormUID.Equals(this.UIAPIRawForm.UniqueID))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_CLICK:
                                if (pVal.ItemUID.Equals("btnSearch"))
                                {
                                    //InitSearch();
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST:

                                break;
                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                UnLoadEvents();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[frmStockTransfer - SBO_Application_ItemEvent] Error: {0}", ex.Message));

                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }
        #endregion

        #region Events
        private void btnSearch_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                mObjFrmFolios = new frmCFLFolios(FoliosFormModeEnum.StockTransfer);
                mObjFrmFolios.Show();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[frmStockTransfer - btnSearch_ClickBefore] Error: {0}", lObjException.Message));

                if (lObjException.Message.Contains("Failed to create form. Please check the form attributes"))
                {
                    if (Application.SBO_Application.MessageBox("La pantalla de folios ya se encuentra abierta, ¿desea cerrar la actual?", 1, "Aceptar", "Cancelar", "") == 1)
                    {
                        UIApplication.GetApplication().Forms.Item("frmFolios").Close();

                        mObjFrmFolios = new frmCFLFolios(FoliosFormModeEnum.StockTransfer);
                        mObjFrmFolios.Show();
                    }
                }
                else
                {
                    UIApplication.ShowMessageBox(string.Format("Error al abrir la pantalla de folios: {0}", lObjException.Message));
                }
            }
        }

        private void btnCreate_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (Application.SBO_Application.MessageBox("¿Desea crear la entrada de consumibles?", 1, "Aceptar", "Cancelar", "") != 1)
                {
                    return;
                }

                this.UIAPIRawForm.Freeze(true);

                CreateStockTransfer();
            }
            catch (Exception lObjException)
            {
                UIApplication.ShowMessageBox(lObjException.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void Form_CloseBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            UnLoadEvents();
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            mtxItems.AutoResizeColumns();
        }
        #endregion

        #region Functions
        private void LoadInitialsControls()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                CreateItemsDatatable();

                txtDate.Value = DateTime.Now.ToString("dd/MM/yyy");
            }
            catch (Exception lObjException)
            {
                UIApplication.GetApplication().SetStatusBarMessage(lObjException.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void GetItemsDetails(string pStrRiseId)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (string.IsNullOrEmpty(pStrRiseId))
                {
                    UIApplication.ShowMessageBox("Seleccione una subida");
                    return;
                }

                ClearMatrix(dtItems.UniqueID, mtxItems);

                List<InventoryItemsLinesDTO> lLstRiseItems = mObjMachineryServiceFactory.GetGoodIssuesService().GetRiseItemsForStockTransfer(int.Parse(pStrRiseId));

                foreach (var lObjRiseItem in lLstRiseItems.Where(x => x.Category != "Unk" && x.Quantity > 0))
                {
                    AddRiseItemDetails(lObjRiseItem);
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[frmStockTransfer - GetItemsDetails] Error: {0}", lObjException.Message));
                UIApplication.ShowMessageBox(lObjException.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void AddRiseItemDetails(InventoryItemsLinesDTO pObjRiseItem)
        {
            try
            {
                if (pObjRiseItem == null)
                    return;

                this.UIAPIRawForm.Freeze(true);

                dtItems.Rows.Add();
                dtItems.SetValue("#", dtItems.Rows.Count - 1, dtItems.Rows.Count);
                dtItems.SetValue("ItemCode", dtItems.Rows.Count - 1, pObjRiseItem.ItemCode);
                dtItems.SetValue("ItemDesc", dtItems.Rows.Count - 1, pObjRiseItem.ItemName);
                dtItems.SetValue("Qty", dtItems.Rows.Count - 1, pObjRiseItem.Quantity);

                mtxItems.LoadFromDataSource();
                mtxItems.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[frmStockTransfer - AddRiseItemDetails] Error al agregar el detalle del artículo: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al agregar el detalle del artículo: {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void CreateStockTransfer()
        {
            try
            {
                if (string.IsNullOrEmpty(txtRiseFolio.Value))
                {
                    UIApplication.ShowError("No se seleccionó una subida");
                    return;
                }

                if (dtItems.Rows.Count <= 0)
                {
                    UIApplication.ShowError("Sin artículos para dar salida");
                    return;
                }

                SAPbobsCOM.StockTransfer lObjStockTransfer = (SAPbobsCOM.StockTransfer)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);
                lObjStockTransfer.FromWarehouse = "MQHEOBRA";
                lObjStockTransfer.ToWarehouse = "MQHE";
                lObjStockTransfer.UserFields.Fields.Item("U_MQ_Rise").Value = txtRiseFolio.Value;

                for (int i = 0; i < dtItems.Rows.Count; i++)
                {
                    string lStrItemCode = dtItems.GetValue("ItemCode", i).ToString();
                    double lDblQuantity = double.Parse(dtItems.GetValue("Qty", i).ToString());

                    if (lDblQuantity <= 0)
                    {
                        UIApplication.ShowError(string.Format("No puede dar entrada al artículo {0} si la cantidad es menor o igual a 0", lStrItemCode));
                        return;
                    }

                    lObjStockTransfer.Lines.SetCurrentLine(i);
                    lObjStockTransfer.Lines.ItemCode = lStrItemCode;
                    lObjStockTransfer.Lines.FromWarehouseCode = "MQHEOBRA";
                    lObjStockTransfer.Lines.WarehouseCode = "MQHE";
                    lObjStockTransfer.Lines.Quantity = lDblQuantity;
                    lObjStockTransfer.Lines.Add();
                }

                if (lObjStockTransfer.Add() != 0)
                {
                    string lStrLastError = DIApplication.Company.GetLastErrorDescription();
                    UIApplication.ShowMessageBox(string.Format("Error al generar la entrada de consumibles: {0}", DIApplication.Company.GetLastErrorDescription()));
                }
                else
                {
                    int lIntDocEntry = int.Parse(DIApplication.Company.GetNewObjectKey());

                    LogUtility.WriteSuccess(string.Format("[frmStockTransfer - CreateStockTransfer] StockTransfer creado correctamente con el DocEntry {0} para la Subida: {1}", lIntDocEntry, txtRiseFolio.Value));

                    mObjMachineryServiceFactory.GetRiseService().MarkRiseAsStockTransfer(int.Parse(txtRiseFolio.Value));
                    ClearControls();

                    UIApplication.ShowSuccess("Entrada de mercancía creada correctamente");
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[frmStockTransfer - CreateStockTransfer] Error al crear la entrada de consumibles: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al crear la entrada de consumibles: {0}", lObjException.Message));
            }
        }

        private void ClearControls()
        {
            ClearMatrix(dtItems.UniqueID, mtxItems);
            txtRiseFolio.Value = string.Empty;
            txtDate.Value = DateTime.Now.ToString("dd/MM/yyy");
        }

        private void ClearMatrix(string pStrDTName, SAPbouiCOM.Matrix pObjMatrix)
        {
            if (!this.UIAPIRawForm.DataSources.DataTables.Item(pStrDTName).IsEmpty)
            {
                this.UIAPIRawForm.DataSources.DataTables.Item(pStrDTName).Rows.Clear();
                pObjMatrix.Clear();
            }
        }

        private void CreateItemsDatatable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTItems");
            dtItems = this.UIAPIRawForm.DataSources.DataTables.Item("DTItems");
            dtItems.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtItems.Columns.Add("ItemCode", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtItems.Columns.Add("ItemDesc", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtItems.Columns.Add("Qty", SAPbouiCOM.BoFieldsType.ft_Quantity);

            FillItemsMatrix();
        }

        private void FillItemsMatrix()
        {
            mtxItems.Columns.Item("#").DataBind.Bind("DTItems", "#");
            mtxItems.Columns.Item("ColItem").DataBind.Bind("DTItems", "ItemCode");
            mtxItems.Columns.Item("ColItemD").DataBind.Bind("DTItems", "ItemDesc");
            mtxItems.Columns.Item("ColQty").DataBind.Bind("DTItems", "Qty");

            mtxItems.AutoResizeColumns();
        }
        #endregion

        #region Controls
        private SAPbouiCOM.StaticText lblRiseFolio;
        private SAPbouiCOM.EditText txtRiseFolio;
        private SAPbouiCOM.Button btnSearch;
        private SAPbouiCOM.Button btnCreate;
        private SAPbouiCOM.Matrix mtxItems;
        private SAPbouiCOM.StaticText lblDate;
        private SAPbouiCOM.EditText txtDate;
        private SAPbouiCOM.DataTable dtItems;
        #endregion
    }
}
