using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.Machinery.Enums;
using UGRS.Core.Utility;

namespace UGRS.AddOn.Machinery.Forms
{
    [FormAttribute("UGRS.AddOn.Machinery.Forms.frmGoodIssue", "Forms/frmGoodIssue.b1f")]
    class frmGoodIssue : UserFormBase
    {
        #region Properties
        private MachinerySeviceFactory mObjMachineryServiceFactory = new MachinerySeviceFactory();
        private SAPbouiCOM.DataTable mDTTotalsConsumed;
        private int mIntRiseId;
        List<InventoryItemsLinesDTO> mLstItems = null;
        private string mStrMatrixSelected = string.Empty;
        private int mIntMatrixRowSelected = 0;
        public int mIntDocEntry = 0;
        private string mStrUserCostCenter = string.Empty;
        #endregion

        #region Constructor
        public frmGoodIssue()
        {

        }

        public frmGoodIssue(int pIntRiseId, SAPbouiCOM.DataTable pDTTotalsConsumed)
        {
            mDTTotalsConsumed = pDTTotalsConsumed;
            mIntRiseId = pIntRiseId;

            LoadEvents();
            LoadInitialsControls();

            mLstItems = mObjMachineryServiceFactory.GetGoodIssuesService().GetItemsByRiseId(mIntRiseId);
            mStrUserCostCenter = mObjMachineryServiceFactory.GetUsersService().GetCostCenter(
                                                                               mObjMachineryServiceFactory
                                                                               .GetUsersService()
                                                                               .GetUserId(Application.SBO_Application.Company.UserName).ToString());

            if (string.IsNullOrEmpty(mStrUserCostCenter))
            {
                UIApplication.ShowError(string.Format("El usuario {0} no tiene asignado el centro de costo", Application.SBO_Application.Company.UserName));
                this.UIAPIRawForm.Close();
            }
        }
        #endregion


        #region Initialize
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtxItems = ((SAPbouiCOM.Matrix)(this.GetItem("mtxItems").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("btnSave").Specific));
            this.mtxGoodIssues = ((SAPbouiCOM.Matrix)(this.GetItem("mtxGI").Specific));
            this.mtxGoodIssues.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.mtxGoodIssues_ValidateBefore);
            this.lblQty = ((SAPbouiCOM.StaticText)(this.GetItem("lblQty").Specific));
            this.lblAF = ((SAPbouiCOM.StaticText)(this.GetItem("lblAF").Specific));
            this.txtQty = ((SAPbouiCOM.EditText)(this.GetItem("txtQty").Specific));
            this.cboAF = ((SAPbouiCOM.ComboBox)(this.GetItem("cboAF").Specific));
            this.btnAdd = ((SAPbouiCOM.Button)(this.GetItem("btnAdd").Specific));
            this.btnAdd.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAdd_ClickBefore);
            this.mtxCategories = ((SAPbouiCOM.Matrix)(this.GetItem("mtxCat").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.CloseAfter += new CloseAfterHandler(this.Form_CloseAfter);

        }

        private void OnCustomInitialize()
        {
            UIAPIRawForm.EnableMenu("1293", true); //Borrar
        }
        #endregion

        #region Load & Unload Events
        private void LoadEvents()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent_GoodIssue);
            Application.SBO_Application.RightClickEvent += new SAPbouiCOM._IApplicationEvents_RightClickEventEventHandler(this.SBO_Application_RightClickEvent_GoodIssue);
            Application.SBO_Application.MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(this.SBO_Application_MenuEvent_GoodIssue);
        }

        private void UnLoadEvents()
        {
            Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent_GoodIssue);
            Application.SBO_Application.RightClickEvent -= new SAPbouiCOM._IApplicationEvents_RightClickEventEventHandler(this.SBO_Application_RightClickEvent_GoodIssue);
            Application.SBO_Application.MenuEvent -= new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(this.SBO_Application_MenuEvent_GoodIssue);
        }
        #endregion

        #region Events
        /// <summary>
        /// SBO_Application_ItemEvent
        /// Metodo para controlar los eventos de la pantalla.
        /// @Author FranciscoFimbres
        /// </summary>
        /// <param name="FormUID"></param>
        /// <param name="pVal"></param>
        /// <param name="BubbleEvent"></param>
        private void SBO_Application_ItemEvent_GoodIssue(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //string y = pVal.CharPressed.ToString();
            try
            {
                if (FormUID.Equals(this.UIAPIRawForm.UniqueID))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_CLICK:
                                if (pVal.ItemUID.Equals("btnSave"))
                                {
                                    SaveGoodIssue();
                                }
                                if (pVal.ItemUID.Equals("btnAdd"))
                                {
                                    //AddGoodIssueLine();
                                }
                                if (pVal.ItemUID.Equals("mtxCat"))
                                {
                                    if (pVal.Row <= 0)
                                        return;

                                    mtxCategories.SelectRow(pVal.Row, true, false);

                                    int lIntRow = mtxCategories.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_SelectionOrder);
                                    if (lIntRow <= 0)
                                        return;

                                    string lStrCategory = (mtxCategories.Columns.Item(1).Cells.Item(lIntRow).Specific as SAPbouiCOM.EditText).Value.Trim();
                                    LoadItemMatrix(lStrCategory);
                                }
                                if (pVal.ItemUID.Equals("mtxItems"))
                                {
                                    if (pVal.Row <= 0)
                                        return;

                                    mtxItems.SelectRow(pVal.Row, true, false);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_VALIDATE:
                                if (pVal.ItemUID.Equals("txtFolio"))
                                {
                                    //StartSearchMode();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(String.Format("[frmGoodIssue - SBO_Application_ItemEvent_GoodIssue] Error: {0}", ex.Message));
                
                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            UnLoadEvents();
        }

        private void SBO_Application_RightClickEvent_GoodIssue(ref SAPbouiCOM.ContextMenuInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                /*if (pVal.BeforeAction && pVal.ActionSuccess)
                {*/
                mStrMatrixSelected = pVal.ItemUID;
                mIntMatrixRowSelected = pVal.Row;
                //}
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }

        public void SBO_Application_MenuEvent_GoodIssue(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.MenuUID == "1293" && pVal.BeforeAction == true && UIApplication.GetApplication().Forms.ActiveForm.UniqueID == "frmGI") //Borrar
                {
                    if (SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("¿Desea elimiar el registro seleccionado?", 2, "Si", "No", "") == 1)
                    {
                        if (mStrMatrixSelected == mtxGoodIssues.Item.UniqueID) //matrix empleados
                        {
                            dtGoodIssues.Rows.Remove(mIntMatrixRowSelected - 1);
                        }
                    }
                    else
                    {
                        BubbleEvent = false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }

        private void btnAdd_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                AddGoodIssueLine();
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(ex.Message);
            }
        }

        private void mtxGoodIssues_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (pVal.ColUID == "ColQty")
                {
                    string lStrQty = (mtxGoodIssues.Columns.Item("ColQty").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStrCategory = dtGoodIssues.GetValue("Category", pVal.Row - 1).ToString(); //(mtxGoodIssues.Columns.Item("ColCat").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();

                    double lDblQty = string.IsNullOrEmpty(lStrQty) ? 0 : double.Parse(lStrQty);

                    if (lDblQty > 0)
                    {
                        ValidateTotalByCategory(lStrCategory, lDblQty, pVal.Row - 1);

                        dtGoodIssues.SetValue("Quantity", pVal.Row - 1, lDblQty);
                    }

                    mtxGoodIssues.LoadFromDataSource();
                    mtxGoodIssues.AutoResizeColumns();
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(String.Format("[frmGoodIssue - mtxGoodIssues_ValidateBefore] Error: {0}", ex.Message));
                this.UIAPIRawForm.Freeze(false);
                UIApplication.ShowError(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        #endregion

        #region Functions
        private void LoadInitialsControls()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                CreateDatatableItems();
                CreateDatatableGoodIssues();
                CreateDatatableCategories();

                LoadCategotyMatrix();
                //LoadItemMatrix();
                AddSupervisorsCbo();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmGoodIssue - LoadInitialsControls] Error: {0}", lObjException.Message));
                Application.SBO_Application.SetStatusBarMessage(lObjException.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void SaveGoodIssue()
        {
            try
            {
                if (Application.SBO_Application.MessageBox("¿Esta seguro de crear la salida de mercancía?", 1, "Aceptar", "Cancelar", "") != 1)
                    return;

                if (dtGoodIssues.Rows.Count <= 0)
                {
                    UIApplication.ShowError("No se puede generar una salida de mercancía sin líneas");
                    return;
                }

                /*string lStrCostingCode = mObjMachineryServiceFactory.GetConfigurationsService().GetConfigurationByName(ConfigurationsEnum.GoodIssueCostingCode).Value;
                if (string.IsNullOrEmpty(lStrCostingCode))
                {
                    UIApplication.ShowError("La configuración del almacén para las salidas de mercancía no tiene asignado un valor");
                    return;
                }*/

                SAPbobsCOM.Documents lObjGoodIssue = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
                lObjGoodIssue.DocDate = DateTime.Now;
                lObjGoodIssue.TaxDate = DateTime.Now;
                lObjGoodIssue.UserFields.Fields.Item("U_MQ_Rise").Value = mIntRiseId.ToString();
                lObjGoodIssue.UserFields.Fields.Item("U_GLO_ObjTUG").Value = "frmGI";
                lObjGoodIssue.UserFields.Fields.Item("U_GLO_InMo").Value = "S-MER";

                for (int i = 0; i < dtGoodIssues.Rows.Count; i++)
                {
                    lObjGoodIssue.Lines.SetCurrentLine(i);
                    lObjGoodIssue.Lines.ItemCode = dtGoodIssues.GetValue("ItemCode", i).ToString();
                    lObjGoodIssue.Lines.Quantity = double.Parse(dtGoodIssues.GetValue("Quantity", i).ToString());
                    lObjGoodIssue.Lines.CostingCode = mStrUserCostCenter;
                    lObjGoodIssue.Lines.CostingCode2 = dtGoodIssues.GetValue("ActiveF", i).ToString();
                    //lObjGoodIssue.Lines.AccountCode = dtGoodIssues.GetValue("ActiveF", i).ToString();
                    lObjGoodIssue.Lines.WarehouseCode = "MQHEOBRA";

                    lObjGoodIssue.Lines.Add();
                }

                if (lObjGoodIssue.Add() != 0)
                {
                    string lStrLastError = DIApplication.Company.GetLastErrorDescription();
                    UIApplication.ShowMessageBox(string.Format("Error al generar la salida de mercancía: {0}", DIApplication.Company.GetLastErrorDescription()));
                }
                else
                {
                    mIntDocEntry = int.Parse(DIApplication.Company.GetNewObjectKey());

                    LogUtility.WriteSuccess(String.Format("[frmGoodIssue - SaveGoodIssue] Salida de mercancia creada correctamente con el DocEntry {0} para la Subida {1}", mIntDocEntry, mIntRiseId.ToString()));

                    this.UIAPIRawForm.Close();
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmGoodIssue - SaveGoodIssue] Error al crear la salida de mercancía: {0}", lObjException.Message));
                UIApplication.ShowMessageBox(string.Format("Error al crear la salida de mercancía: {0}", lObjException.Message));
            }
        }

        private void AddSupervisorsCbo()
        {
            try
            {
                for (int i = 0; i < mDTTotalsConsumed.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(mDTTotalsConsumed.GetValue("ActCodTR", i).ToString()))
                    {
                        cboAF.ValidValues.Add(mDTTotalsConsumed.GetValue("ActCodTR", i).ToString(), mDTTotalsConsumed.GetValue("ActNumTR", i).ToString());
                    }
                }

                cboAF.Item.DisplayDesc = true;
                if (cboAF.ValidValues.Count > 0)
                    cboAF.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al cargar el listado de activos fijos: {0}", lObjException.Message));
            }
        }

        private void LoadItemMatrix(string pStrCategory)
        {
            try
            {
                ClearMatrix(dtItems.UniqueID, mtxItems);

                List<InventoryItemsLinesDTO> lLstItems = mLstItems.Where(x => x.Category.Contains(pStrCategory)).ToList(); //mObjMachineryServiceFactory.GetGoodIssuesService().GetItemsByRiseId(mIntRiseId);

                foreach (var lObjItem in lLstItems)
                {
                    AddItem(lObjItem);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al obtener los artículos: {0}", lObjException.Message));
            }
        }

        private void LoadCategotyMatrix()
        {
            try
            {
                List<ConsumablesDocumentsDTO> lLstConsumbales = mObjMachineryServiceFactory.GetConsumablesService().TotalsRecordsDataTableToDTO(mDTTotalsConsumed);

                List<InventoryItemsLinesDTO> lLstTotals = mObjMachineryServiceFactory.GetGoodIssuesService().GetTotalsRiseId(mIntRiseId);

                double lDbl15W40Reg = lLstConsumbales.Sum(x => x.F15W40);
                double lDblHidraulicReg = lLstConsumbales.Sum(x => x.Hidraulic);
                double lDblSAE40Reg = lLstConsumbales.Sum(x => x.SAE40);
                double lDblTransmitionReg = lLstConsumbales.Sum(x => x.Transmition);
                double lDblOilsReg = lLstConsumbales.Sum(x => x.Oils);

                double lDbl15W40Disp = lLstTotals.Where(x => x.Category == "15W40").Sum(y => y.Quantity);
                double lDblHidraulicDisp = lLstTotals.Where(x => x.Category == "Hidraulico").Sum(y => y.Quantity);
                double lDblSAE40Disp = lLstTotals.Where(x => x.Category == "SAE40").Sum(y => y.Quantity);
                double lDblTransmitionDisp = lLstTotals.Where(x => x.Category == "Transmision").Sum(y => y.Quantity);
                double lDblOilsDisp = lLstTotals.Where(x => x.Category == "Grasas").Sum(y => y.Quantity);

                dtCategories.Rows.Add();
                dtCategories.SetValue("#", dtCategories.Rows.Count - 1, dtCategories.Rows.Count);
                dtCategories.SetValue("CatName", dtCategories.Rows.Count - 1, "15W40");
                dtCategories.SetValue("CatCode", dtCategories.Rows.Count - 1, string.Empty);
                dtCategories.SetValue("Quantity", dtCategories.Rows.Count - 1, lDbl15W40Reg);
                dtCategories.SetValue("ExtQty", dtCategories.Rows.Count - 1, lDbl15W40Reg - lDbl15W40Disp);

                dtCategories.Rows.Add();
                dtCategories.SetValue("#", dtCategories.Rows.Count - 1, dtCategories.Rows.Count);
                dtCategories.SetValue("CatName", dtCategories.Rows.Count - 1, "Hidraulico");
                dtCategories.SetValue("CatCode", dtCategories.Rows.Count - 1, string.Empty);
                dtCategories.SetValue("Quantity", dtCategories.Rows.Count - 1, lDblHidraulicReg);
                dtCategories.SetValue("ExtQty", dtCategories.Rows.Count - 1, lDblHidraulicReg - lDblHidraulicDisp);

                dtCategories.Rows.Add();
                dtCategories.SetValue("#", dtCategories.Rows.Count - 1, dtCategories.Rows.Count);
                dtCategories.SetValue("CatName", dtCategories.Rows.Count - 1, "SAE40");
                dtCategories.SetValue("CatCode", dtCategories.Rows.Count - 1, string.Empty);
                dtCategories.SetValue("Quantity", dtCategories.Rows.Count - 1, lDblSAE40Reg);
                dtCategories.SetValue("ExtQty", dtCategories.Rows.Count - 1, lDblSAE40Reg - lDblSAE40Disp);

                dtCategories.Rows.Add();
                dtCategories.SetValue("#", dtCategories.Rows.Count - 1, dtCategories.Rows.Count);
                dtCategories.SetValue("CatName", dtCategories.Rows.Count - 1, "Transmision");
                dtCategories.SetValue("CatCode", dtCategories.Rows.Count - 1, string.Empty);
                dtCategories.SetValue("Quantity", dtCategories.Rows.Count - 1, lDblTransmitionReg);
                dtCategories.SetValue("ExtQty", dtCategories.Rows.Count - 1, lDblTransmitionReg - lDblTransmitionDisp);

                dtCategories.Rows.Add();
                dtCategories.SetValue("#", dtCategories.Rows.Count - 1, dtCategories.Rows.Count);
                dtCategories.SetValue("CatName", dtCategories.Rows.Count - 1, "Grasas");
                dtCategories.SetValue("CatCode", dtCategories.Rows.Count - 1, string.Empty);
                dtCategories.SetValue("Quantity", dtCategories.Rows.Count - 1, lDblOilsReg);
                dtCategories.SetValue("ExtQty", dtCategories.Rows.Count - 1, lDblOilsReg - lDblOilsDisp);

                /*
                 * dtCategories.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtCategories.Columns.Add("CatName", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtCategories.Columns.Add("CatCode", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtCategories.Columns.Add("Quantity", SAPbouiCOM.BoFieldsType.ft_Float);
                 */

                mtxCategories.LoadFromDataSource();
                mtxCategories.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al obtener los artículos: {0}", lObjException.Message));
            }
        }

        public void AddItem(InventoryItemsLinesDTO pObjGoodIssuesLine)
        {
            try
            {
                if (pObjGoodIssuesLine == null)
                    return;

                this.UIAPIRawForm.Freeze(true);

                dtItems.Rows.Add();
                dtItems.SetValue("#", dtItems.Rows.Count - 1, dtItems.Rows.Count);
                dtItems.SetValue("ItemCode", dtItems.Rows.Count - 1, pObjGoodIssuesLine.ItemCode);
                dtItems.SetValue("ItemName", dtItems.Rows.Count - 1, pObjGoodIssuesLine.ItemName);
                dtItems.SetValue("Quantity", dtItems.Rows.Count - 1, pObjGoodIssuesLine.Quantity);
                dtItems.SetValue("OrigQty", dtItems.Rows.Count - 1, pObjGoodIssuesLine.OriginalQty);
                dtItems.SetValue("Category", dtItems.Rows.Count - 1, pObjGoodIssuesLine.Category);

                mtxItems.LoadFromDataSource();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el artículo {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void AddGoodIssueLine()
        {
            try
            {
                int lIntRow = mtxItems.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_SelectionOrder);
                if (lIntRow <= 0)
                {
                    UIApplication.ShowError("Seleccione un artículo");
                    return;
                }

                string lStrItemCode = (mtxItems.Columns.Item(1).Cells.Item(lIntRow).Specific as SAPbouiCOM.EditText).Value.Trim();
                string lStrItemName = (mtxItems.Columns.Item(2).Cells.Item(lIntRow).Specific as SAPbouiCOM.EditText).Value.Trim();
                double lDblItemQty = double.Parse((mtxItems.Columns.Item(3).Cells.Item(lIntRow).Specific as SAPbouiCOM.EditText).Value.Trim());
                string lStrCategory = dtItems.GetValue("Category", lIntRow - 1).ToString();

                string lStrActivoFijo = cboAF.Value;
                double lDblQty = 0;

                if (string.IsNullOrEmpty(lStrActivoFijo))
                {
                    UIApplication.ShowError("Seleccione un activo fijo");
                    return;
                }

                if (!Double.TryParse(txtQty.Value, out lDblQty))
                {
                    UIApplication.ShowError("Valor no válido para la cantidad");
                    return;
                }

                if (lDblQty <= 0)
                {
                    UIApplication.ShowError("Ingrese una cantidad mayor a 0");
                    return;
                }

                ValidateTotalByCategory(lStrCategory, lDblQty);

                /*if (lDblQty > lDblItemQty)
                {
                    UIApplication.ShowError(string.Format("No puede dar salida a una cantidad mayor de la permitida {0} para el artículo {1}", lDblItemQty, lStrItemCode));
                    return;
                }*/

                InventoryItemsLinesDTO pObjGoodIssuesLine = new InventoryItemsLinesDTO
                {
                    ItemCode = lStrItemCode,
                    ItemName = lStrItemName,
                    OriginalQty = 0,
                    Quantity = lDblQty,
                    ActivoFijo = lStrActivoFijo,
                    Category = lStrCategory
                };

                this.UIAPIRawForm.Freeze(true);

                dtGoodIssues.Rows.Add();
                dtGoodIssues.SetValue("#", dtGoodIssues.Rows.Count - 1, dtGoodIssues.Rows.Count);
                dtGoodIssues.SetValue("ItemCode", dtGoodIssues.Rows.Count - 1, pObjGoodIssuesLine.ItemCode);
                dtGoodIssues.SetValue("ItemName", dtGoodIssues.Rows.Count - 1, pObjGoodIssuesLine.ItemName);
                dtGoodIssues.SetValue("Quantity", dtGoodIssues.Rows.Count - 1, pObjGoodIssuesLine.Quantity);
                dtGoodIssues.SetValue("ActiveF", dtGoodIssues.Rows.Count - 1, pObjGoodIssuesLine.ActivoFijo);
                dtGoodIssues.SetValue("Category", dtGoodIssues.Rows.Count - 1, pObjGoodIssuesLine.Category);

                mtxGoodIssues.LoadFromDataSource();
                mtxGoodIssues.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmGoodIssue - AddGoodIssueLine] Error: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al agregar la línea de la salida de mercancía: {0}", lObjException.Message));
            }
            finally
            {
                txtQty.Value = string.Empty;

                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void ValidateTotalByCategory(string pStrCategory, double pDblItemTotal = 0, int? pIntIndex = null)
        {
            List<InventoryItemsLinesDTO> lLstGoodIssuesLines = mObjMachineryServiceFactory.GetGoodIssuesService().DataTableToDTO(dtGoodIssues);

            double lDblTotal = lLstGoodIssuesLines.Where(x => x.Category == pStrCategory).Sum(y => y.Quantity) + pDblItemTotal;
            double lDblTotalDisp = 0;
            double pDblTotalOrg = 0;

            for (int i = 0; i < dtCategories.Rows.Count; i++)
            {
                if (dtCategories.GetValue("CatName", i).ToString() == pStrCategory)
                {
                    lDblTotalDisp = double.Parse(dtCategories.GetValue("ExtQty", i).ToString());
                }
            }

            if (pIntIndex != null)
            {
                pDblTotalOrg = lLstGoodIssuesLines[(int)pIntIndex].Quantity;

                lDblTotal -= pDblTotalOrg;
                //lDblTotalDisp += pDblTotalOrg;
            }

            if (lDblTotalDisp < lDblTotal)
            {
                if (pIntIndex != null)
                {
                    dtGoodIssues.SetValue("Quantity", (int)pIntIndex, pDblTotalOrg);

                    mtxGoodIssues.LoadFromDataSource();
                }

                txtQty.Value = string.Empty;

                throw new Exception(string.Format("La cantidad máxima a dar salida para la categoría {0} es de {1}", pStrCategory, lDblTotalDisp));
            }

            /*switch (pStrCategory)
            {
                case "15W40":
                    dtCategories.SetValue("ExtQty", 0, lDblTotalDisp - lDblTotal);
                    break;
                case "Hidraulico":
                    dtCategories.SetValue("ExtQty", 1, lDblTotalDisp - lDblTotal);
                    break;
                case "SAE40":
                    dtCategories.SetValue("ExtQty", 2, lDblTotalDisp - lDblTotal);
                    break;
                case "Transmision":
                    dtCategories.SetValue("ExtQty", 3, lDblTotalDisp - lDblTotal);
                    break;
                case "Grasas":
                    dtCategories.SetValue("ExtQty", 4, lDblTotalDisp - lDblTotal);
                    break;
                default:
                    break;
            }*/

            mtxGoodIssues.LoadFromDataSource();
            mtxGoodIssues.AutoResizeColumns();
        }

        private void ClearMatrix(string pStrDTName, SAPbouiCOM.Matrix pObjMatrix)
        {
            if (!this.UIAPIRawForm.DataSources.DataTables.Item(pStrDTName).IsEmpty)
            {
                this.UIAPIRawForm.DataSources.DataTables.Item(pStrDTName).Rows.Clear();
                pObjMatrix.Clear();
            }
        }

        private void CreateDatatableItems()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTItems");
            dtItems = this.UIAPIRawForm.DataSources.DataTables.Item("DTItems");
            dtItems.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtItems.Columns.Add("ItemCode", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtItems.Columns.Add("ItemName", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtItems.Columns.Add("Quantity", SAPbouiCOM.BoFieldsType.ft_Float);
            dtItems.Columns.Add("OrigQty", SAPbouiCOM.BoFieldsType.ft_Float);
            dtItems.Columns.Add("Category", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillMatrixItems();
        }

        private void CreateDatatableGoodIssues()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTGI");
            dtGoodIssues = this.UIAPIRawForm.DataSources.DataTables.Item("DTGI");
            dtGoodIssues.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtGoodIssues.Columns.Add("ItemCode", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtGoodIssues.Columns.Add("ItemName", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtGoodIssues.Columns.Add("Quantity", SAPbouiCOM.BoFieldsType.ft_Float);
            dtGoodIssues.Columns.Add("ActiveF", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtGoodIssues.Columns.Add("Category", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillGoodIssuesItems();
        }

        private void CreateDatatableCategories()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTCat");
            dtCategories = this.UIAPIRawForm.DataSources.DataTables.Item("DTCat");
            dtCategories.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtCategories.Columns.Add("CatName", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtCategories.Columns.Add("CatCode", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtCategories.Columns.Add("Quantity", SAPbouiCOM.BoFieldsType.ft_Float);
            dtCategories.Columns.Add("ExtQty", SAPbouiCOM.BoFieldsType.ft_Float);

            FillCategories();
        }

        private void FillCategories()
        {
            mtxCategories.Columns.Item("#").DataBind.Bind("DTCat", "#");
            mtxCategories.Columns.Item("ColCat").DataBind.Bind("DTCat", "CatName");
            mtxCategories.Columns.Item("ColQty").DataBind.Bind("DTCat", "Quantity");
            mtxCategories.Columns.Item("ColTotExt").DataBind.Bind("DTCat", "ExtQty");

            mtxCategories.AutoResizeColumns();
        }

        private void FillGoodIssuesItems()
        {
            mtxGoodIssues.Columns.Item("#").DataBind.Bind("DTGI", "#");
            mtxGoodIssues.Columns.Item("ColItem").DataBind.Bind("DTGI", "ItemCode");
            mtxGoodIssues.Columns.Item("ColItemN").DataBind.Bind("DTGI", "ItemName");
            mtxGoodIssues.Columns.Item("ColQty").DataBind.Bind("DTGI", "Quantity");
            mtxGoodIssues.Columns.Item("ColAF").DataBind.Bind("DTGI", "ActiveF");

            mtxGoodIssues.AutoResizeColumns();
        }

        private void FillMatrixItems()
        {
            mtxItems.Columns.Item("#").DataBind.Bind("DTItems", "#");
            mtxItems.Columns.Item("ColItem").DataBind.Bind("DTItems", "ItemCode");
            mtxItems.Columns.Item("ColItemN").DataBind.Bind("DTItems", "ItemName");
            mtxItems.Columns.Item("ColQty").DataBind.Bind("DTItems", "Quantity");

            mtxItems.AutoResizeColumns();
        }
        #endregion

        #region Controls
        private SAPbouiCOM.Matrix mtxItems;
        private SAPbouiCOM.Button btnSave;
        private SAPbouiCOM.Matrix mtxGoodIssues;
        private SAPbouiCOM.StaticText lblQty;
        private SAPbouiCOM.StaticText lblAF;
        private SAPbouiCOM.EditText txtQty;
        private SAPbouiCOM.ComboBox cboAF;
        private SAPbouiCOM.Button btnAdd;
        private SAPbouiCOM.DataTable dtItems;
        private SAPbouiCOM.DataTable dtGoodIssues;
        private SAPbouiCOM.DataTable dtCategories;
        private SAPbouiCOM.Matrix mtxCategories;
        #endregion

    }
}
