using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.Machinery;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.Utility;

namespace UGRS.AddOn.Machinery.Forms
{
    [FormAttribute("UGRS.AddOn.Machinery.Forms.frmCFLOrdersSale", "Forms/frmCFLOrdersSale.b1f")]
    class frmCFLOrdersSale : UserFormBase
    {
        #region Properties
        private MachinerySeviceFactory mObjMachinerySeviceFactory = new MachinerySeviceFactory();
        public ContractsDTO mObjSelectedContract = null;
        #endregion

        #region Constructor
        public frmCFLOrdersSale()
        {
            LoadEvents();
            CreateOrdersSalesDatatable();
            LoadContracts();
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtxOrdersSale = ((SAPbouiCOM.Matrix)(this.GetItem("mtxOV").Specific));
            this.btnSelect = ((SAPbouiCOM.Button)(this.GetItem("btnSelect").Specific));
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCancel").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);

        }

        private void OnCustomInitialize()
        {

        }
        #endregion

        #region Load & Unload Events
        private void LoadEvents()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }
        private void UnLoadEvents()
        {
            Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
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
                if (FormUID.Equals(this.UIAPIRawForm.UniqueID))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_CLICK:
                                if (pVal.ItemUID.Equals("btnSelect"))
                                {
                                    SelectContract();
                                }
                                if (pVal.ItemUID.Equals("btnCancel"))
                                {
                                    this.UIAPIRawForm.Close();
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                UnLoadEvents();
                                break;
                        }
                    }
                    else if (pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_CLICK:
                                if (pVal.ItemUID.Equals("mtxOV"))
                                {
                                    if (pVal.Row > 0)
                                        mtxOrdersSale.SelectRow(pVal.Row, true, false);
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK:
                                if (pVal.ItemUID.Equals("mtxOV"))
                                {
                                    SelectContract();
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(String.Format("[frmCFLOrdersSale - SBO_Application_ItemEvent] Error: {0}", ex.Message));
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            mtxOrdersSale.AutoResizeColumns();
        }
        #endregion

        #region Functions
        private void LoadContracts()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);
                Application.SBO_Application.StatusBar.SetText("Buscando contratos...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                ClearMatrix();

                List<ContractsDTO> lLstContracts = mObjMachinerySeviceFactory.GetContractsService().GetContracts();

                for (int i = 0; i < lLstContracts.Count; i++)
                {
                    dtOrdersSale.Rows.Add();
                    dtOrdersSale.SetValue("#", i, i + 1);
                    dtOrdersSale.SetValue("DocEntry", i, lLstContracts[i].DocEntry);
                    dtOrdersSale.SetValue("DocNum", i, lLstContracts[i].DocNum);
                    dtOrdersSale.SetValue("Date", i, lLstContracts[i].DocDate);
                    dtOrdersSale.SetValue("Type", i, lLstContracts[i].TypeDescription);
                    dtOrdersSale.SetValue("Status", i, lLstContracts[i].StatusDescription);
                    dtOrdersSale.SetValue("TypeCode", i, lLstContracts[i].Type);
                    dtOrdersSale.SetValue("StatusCod", i, lLstContracts[i].Status);
                    dtOrdersSale.SetValue("ImpCod", i, lLstContracts[i].Import);
                    dtOrdersSale.SetValue("CardName", i, lLstContracts[i].CardName);
                    dtOrdersSale.SetValue("MunpId", i, lLstContracts[i].MunicipalityCode);
                    dtOrdersSale.SetValue("Munp", i, lLstContracts[i].Municipality);
                }

                mtxOrdersSale.AutoResizeColumns();
                mtxOrdersSale.LoadFromDataSource();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmCFLOrdersSale - LoadContracts] Error: {0}", lObjException.Message));
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(string.Format("Error al obtener los contratos: {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void SelectContract()
        {
            try
            {
                int lIntRow = mtxOrdersSale.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_SelectionOrder);
                if (lIntRow >= 0)
                {
                    mObjSelectedContract = new ContractsDTO
                    {
                        DocEntry = int.Parse((mtxOrdersSale.Columns.Item(1).Cells.Item(lIntRow).Specific as SAPbouiCOM.EditText).Value.Trim()),
                        DocNum = int.Parse((mtxOrdersSale.Columns.Item(2).Cells.Item(lIntRow).Specific as SAPbouiCOM.EditText).Value.Trim()),
                        DocDate = DateTime.Parse(dtOrdersSale.GetValue(3, lIntRow - 1).ToString()), //DateTime.Parse((mtxOrdersSale.Columns.Item(3).Cells.Item(lIntRow).Specific as SAPbouiCOM.EditText).Value.Trim()),
                        Type = int.Parse(dtOrdersSale.GetValue(6, lIntRow - 1).ToString()),
                        Status = dtOrdersSale.GetValue(7, lIntRow - 1).ToString(),
                        Import = double.Parse(dtOrdersSale.GetValue(8, lIntRow - 1).ToString()),
                        CardName = dtOrdersSale.GetValue("CardName", lIntRow - 1).ToString(),
                        MunicipalityCode = dtOrdersSale.GetValue("MunpId", lIntRow - 1).ToString(),
                        Municipality = dtOrdersSale.GetValue("Munp", lIntRow - 1).ToString(),
                    };

                    this.UIAPIRawForm.Close();
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmCFLOrdersSale - SelectContract] Error: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al seleccionar el contrato: {0}", lObjException.Message));
            }
        }

        private void CreateOrdersSalesDatatable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTOrdersSales");
            dtOrdersSale = this.UIAPIRawForm.DataSources.DataTables.Item("DTOrdersSales");
            dtOrdersSale.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtOrdersSale.Columns.Add("DocEntry", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtOrdersSale.Columns.Add("DocNum", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtOrdersSale.Columns.Add("Date", SAPbouiCOM.BoFieldsType.ft_Date);
            dtOrdersSale.Columns.Add("Type", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtOrdersSale.Columns.Add("Status", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtOrdersSale.Columns.Add("TypeCode", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtOrdersSale.Columns.Add("StatusCod", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtOrdersSale.Columns.Add("ImpCod", SAPbouiCOM.BoFieldsType.ft_Price);
            dtOrdersSale.Columns.Add("CardName", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtOrdersSale.Columns.Add("MunpId", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtOrdersSale.Columns.Add("Munp", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillOrdersSalesMatrix();
        }

        private void FillOrdersSalesMatrix()
        {
            mtxOrdersSale.Columns.Item("#").DataBind.Bind("DTOrdersSales", "#");
            mtxOrdersSale.Columns.Item("ColDocE").DataBind.Bind("DTOrdersSales", "DocEntry");
            mtxOrdersSale.Columns.Item("ColDocN").DataBind.Bind("DTOrdersSales", "DocNum");
            mtxOrdersSale.Columns.Item("ColDocDat").DataBind.Bind("DTOrdersSales", "Date");
            mtxOrdersSale.Columns.Item("ColType").DataBind.Bind("DTOrdersSales", "Type");
            mtxOrdersSale.Columns.Item("ColStatus").DataBind.Bind("DTOrdersSales", "Status");
            mtxOrdersSale.Columns.Item("ColImp").DataBind.Bind("DTOrdersSales", "ImpCod");
            mtxOrdersSale.Columns.Item("ColCardNm").DataBind.Bind("DTOrdersSales", "CardName");
            mtxOrdersSale.Columns.Item("ColMunp").DataBind.Bind("DTOrdersSales", "Munp");

            mtxOrdersSale.AutoResizeColumns();
        }

        private void ClearMatrix()
        {
            if (!this.UIAPIRawForm.DataSources.DataTables.Item("DTOrdersSales").IsEmpty)
            {
                this.UIAPIRawForm.DataSources.DataTables.Item("DTOrdersSales").Rows.Clear();
                mtxOrdersSale.Clear();
            }
        }
        #endregion

        #region Controls
        #region Matrix
        private SAPbouiCOM.Matrix mtxOrdersSale;
        #endregion

        #region Buttons
        private SAPbouiCOM.Button btnSelect;
        private SAPbouiCOM.Button btnCancel;
        #endregion

        #region Datatables
        private SAPbouiCOM.DataTable dtOrdersSale;
        #endregion
        #endregion
    }
}
