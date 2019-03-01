using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.Core.SDK.DI.Machinery.Enums;
using UGRS.Core.Extension.Enum;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery;
using UGRS.AddOn.Machinery.Utilities;
using UGRS.Core.Utility;

namespace UGRS.AddOn.Machinery.Forms
{
    [FormAttribute("UGRS.AddOn.Machinery.Forms.frmRiseSearch", "Forms/frmRiseSearch.b1f")]
    class frmRiseSearch : UserFormBase
    {
        #region Properties
        private MachinerySeviceFactory mObjMachineryServiceFactory = new MachinerySeviceFactory();
        private string mStrClientCode = string.Empty;
        #endregion

        #region Constructor
        public frmRiseSearch()
        {
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
            this.lblContracts = ((SAPbouiCOM.StaticText)(this.GetItem("lblCont").Specific));
            this.lblClient = ((SAPbouiCOM.StaticText)(this.GetItem("lblClient").Specific));
            this.lblStatus = ((SAPbouiCOM.StaticText)(this.GetItem("lblStatus").Specific));
            this.txtContracts = ((SAPbouiCOM.EditText)(this.GetItem("txtCont").Specific));
            this.txtClient = ((SAPbouiCOM.EditText)(this.GetItem("txtClient").Specific));
            this.lblSDate = ((SAPbouiCOM.StaticText)(this.GetItem("lblSDate").Specific));
            this.txtStartDate = ((SAPbouiCOM.EditText)(this.GetItem("txtSDate").Specific));
            this.lblEDate = ((SAPbouiCOM.StaticText)(this.GetItem("lblEDate").Specific));
            this.txtEndDate = ((SAPbouiCOM.EditText)(this.GetItem("txtEDate").Specific));
            this.btnSearch = ((SAPbouiCOM.Button)(this.GetItem("btnSearch").Specific));
            this.mtxContracts = ((SAPbouiCOM.Matrix)(this.GetItem("mtxCont").Specific));
            this.mtxContracts.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtxContracts_LinkPressedBefore);
            this.mtxRise = ((SAPbouiCOM.Matrix)(this.GetItem("mtxRise").Specific));
            this.mtxRise.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtxRise_LinkPressedBefore);
            this.cboStatus = ((SAPbouiCOM.ComboBox)(this.GetItem("cboStatus").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.CloseBefore += new SAPbouiCOM.Framework.FormBase.CloseBeforeHandler(this.Form_CloseBefore);
            this.ResizeBefore += new SAPbouiCOM.Framework.FormBase.ResizeBeforeHandler(this.Form_ResizeBefore);
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
                                if (pVal.ItemUID.Equals("btnSearch"))
                                {
                                    InitSearch();
                                }
                                if (pVal.ItemUID.Equals("mtxCont"))
                                {
                                    if (pVal.Row <= 0)
                                        return;

                                    mtxContracts.SelectRow(pVal.Row, true, false);

                                    int lIntRow = mtxContracts.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_SelectionOrder);
                                    if (lIntRow <= 0)
                                        return;

                                    string lStrDocEntry = dtContracts.GetValue(1, lIntRow - 1).ToString();
                                    //string lStrDocEntry = (mtxContracts.Columns.Item(1).Cells.Item(lIntRow).Specific as SAPbouiCOM.EditText).Value.Trim();
                                    LoadRisesByContract(lStrDocEntry);
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST:
                                ChooseFromListAfterEvent(pVal);
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
                LogUtility.WriteError(string.Format("[frmRiseSearch - SBO_Application_ItemEvent] Error: {0}", ex.Message));

                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }
        #endregion

        #region Events


        private void Form_CloseBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            UnLoadEvents();
        }

        private void Form_ResizeBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void mtxContracts_LinkPressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ColUID == "ColCont")
            {
                string lStrDocEntry = dtContracts.GetValue("DocECont", pVal.Row - 1).ToString();

                if (string.IsNullOrEmpty(lStrDocEntry))
                    return;

                UIApplication.GetApplication().OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_Order, "", lStrDocEntry);
            }
        }

        private void mtxRise_LinkPressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ColUID == "ColRiseF")
            {
                string lStrRiseId = string.Empty;

                try
                {
                    this.UIAPIRawForm.Freeze(true);
                    //Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID).Freeze(true);

                    lStrRiseId = dtRise.GetValue("DocNmRise", pVal.Row - 1).ToString();

                    if (string.IsNullOrEmpty(lStrRiseId))
                        return;

                    //UIApplication.GetApplication().OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_Order, "", lStrDocEntry);
                    MachineryForm lObjMachinery = new MachineryForm(int.Parse(lStrRiseId));
                    lObjMachinery.Show();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Failed to create form. Please check the form attributes"))
                    {
                        if (Application.SBO_Application.MessageBox("La pantalla de subidas ya se encuentra abierta, ¿desea cerrar la actual?", 1, "Aceptar", "Cancelar", "") == 1)
                        {
                            UIApplication.GetApplication().Forms.Item("frmRise").Close();

                            if (string.IsNullOrEmpty(lStrRiseId))
                                return;

                            MachineryForm lObjMachinery = new MachineryForm(int.Parse(lStrRiseId));
                            lObjMachinery.Show();
                        }
                    }
                    else
                    {
                        UIApplication.ShowMessageBox(string.Format("Error al abrir la pantalla de la subida: {0}", ex.Message));
                    }
                }
                finally
                {
                    this.UIAPIRawForm.Freeze(false);
                    //Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID).Freeze(true);
                }
            }
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                //Width
                //mtxContracts.Item.Width = UIAPIRawForm.Width / 2;
                //mtxRise.Item.Width = UIAPIRawForm.Width / 2 - 300;


                //Height
                mtxContracts.Item.Height = UIAPIRawForm.Height / 3;
                mtxRise.Item.Height = UIAPIRawForm.Height / 4;
                mtxRise.Item.Top = mtxContracts.Item.Top + mtxContracts.Item.Height + 20;

                ////Left
                ////mtxContracts.Item.Left = mtxAuction.Item.Width + 50;
                ////mtxInv.Item.Left = mtxAuction.Item.Width + 50;

                ////Top
                //mtxRise.Item.Top = mtxContracts.Item.Top + mtxContracts.Item.Height + 20;

                mtxContracts.AutoResizeColumns();
                mtxRise.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[frmRiseSearch - Form_ResizeAfter] Error: {0}", ex.Message));
                UIApplication.ShowError("frmPayment (Form_ResizeAfter)" + ex.Message);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }
        #endregion

        #region Functions
        private void LoadInitialsControls()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                LoadChoosesFromList();
                SetCFLToTxt();
                CreateContractsDatatable();
                CreateRiseDataTable();
                LoadStatus();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[frmRiseSearch - LoadInitialsControls] Error: {0}", lObjException.Message));
                Application.SBO_Application.SetStatusBarMessage(lObjException.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void LoadStatus()
        {
            try
            {
                //List<RiseStatusEnum> lLstRiseStsEnm = Enum.GetValues(typeof(RiseStatusEnum)).Cast<RiseStatusEnum>().ToList();

                //cboStatus.ValidValues.Add("0", "");
                //foreach (var lEnmRiseStatus in lLstRiseStsEnm)
                //{
                //    cboStatus.ValidValues.Add(((int)lEnmRiseStatus).ToString(), lEnmRiseStatus.GetDescription());
                //}

                cboStatus.ValidValues.Add("", "Seleccione");
                cboStatus.ValidValues.Add("0", "Abierto");
                cboStatus.ValidValues.Add("1", "Cerrado");

                cboStatus.Item.DisplayDesc = true;
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[frmRiseSearch - LoadStatus] Error al obtener el listado de estatus de las subidas: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al obtener el listado de estatus de las subidas: {0}", lObjException.Message));
            }
        }

        private void InitSearch()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                SearchContracts();
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

        private void SearchContracts()
        {
            try
            {
                ClearMatrix(dtContracts.UniqueID, mtxContracts);

                string lStrContract = txtContracts.Value.Trim();
                string lStrStatus = cboStatus.Selected.Value.Trim();
                string lStrStartDate = string.IsNullOrEmpty(txtStartDate.Value) ? string.Empty : DateTimeUtility.StringToDateTime(txtStartDate.Value).ToString("yyyy/MM/dd");
                string lStrEndDate = string.IsNullOrEmpty(txtEndDate.Value) ? string.Empty : DateTimeUtility.StringToDateTime(txtEndDate.Value).ToString("yyyy/MM/dd");

                if (string.IsNullOrEmpty(txtClient.Value))
                {
                    mStrClientCode = string.Empty;
                }

                List<ContractsFiltersDTO> lLstContracts = mObjMachineryServiceFactory.GetContractsService().GetContracts(lStrContract, mStrClientCode, lStrStatus, lStrStartDate, lStrEndDate);

                foreach (var lObjContract in lLstContracts)
                {
                    AddContract(lObjContract);
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[frmRiseSearch - SearchContracts] Error al buscar los contratos: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al buscar los contratos: {0}", lObjException.Message));
            }
        }

        private void LoadRisesByContract(string pStrContractDocEntry)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                ClearMatrix(dtRise.UniqueID, mtxRise);

                List<RiseFiltersDTO> lLstRises = mObjMachineryServiceFactory.GetRiseService().GetRisesByContractId(pStrContractDocEntry);

                foreach (var lObjRise in lLstRises)
                {
                    AddRise(lObjRise);
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[frmRiseSearch - LoadRisesByContract] Error al buscar las subidas: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al buscar las subidas: {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void AddContract(ContractsFiltersDTO pObjContract)
        {
            try
            {
                if (pObjContract == null)
                    return;

                this.UIAPIRawForm.Freeze(true);

                dtContracts.Rows.Add();
                dtContracts.SetValue("#", dtContracts.Rows.Count - 1, dtContracts.Rows.Count);
                dtContracts.SetValue("DocECont", dtContracts.Rows.Count - 1, pObjContract.DocEntry);
                dtContracts.SetValue("DocNCont", dtContracts.Rows.Count - 1, pObjContract.DocNum);
                dtContracts.SetValue("Client", dtContracts.Rows.Count - 1, pObjContract.CardName);
                dtContracts.SetValue("HrsFt", dtContracts.Rows.Count - 1, pObjContract.HrsFeet);
                dtContracts.SetValue("InvExt", dtContracts.Rows.Count - 1, pObjContract.ExtrasInvoices);
                dtContracts.SetValue("Import", dtContracts.Rows.Count - 1, pObjContract.Import);
                dtContracts.SetValue("RealHrs", dtContracts.Rows.Count - 1, pObjContract.RealHrs);
                dtContracts.SetValue("Dif", dtContracts.Rows.Count - 1, pObjContract.Difference);
                dtContracts.SetValue("Status", dtContracts.Rows.Count - 1, pObjContract.StatusDescription);

                mtxContracts.LoadFromDataSource();
                mtxContracts.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[frmRiseSearch - AddContract] Error al agregar el contrato: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al agregar el contrato: {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void AddRise(RiseFiltersDTO pObjRiseFilter)
        {
            try
            {
                if (pObjRiseFilter == null)
                    return;

                this.UIAPIRawForm.Freeze(true);

                dtRise.Rows.Add();
                dtRise.SetValue("#", dtRise.Rows.Count - 1, dtRise.Rows.Count);
                dtRise.SetValue("DocNmRise", dtRise.Rows.Count - 1, pObjRiseFilter.IdRise);
                dtRise.SetValue("HrsFt", dtRise.Rows.Count - 1, pObjRiseFilter.HrsFeet);
                //dtRise.SetValue("DocNCont", dtRise.Rows.Count - 1, pObjRiseFilter.ContractDocEntry);

                mtxRise.LoadFromDataSource();
                mtxRise.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[frmRiseSearch - AddRise] Error al agregar la subida: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al agregar la subida: {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void SetCFLToTxt()
        {
            txtClient.DataBind.SetBound(true, "", "CFL_0");
            txtClient.ChooseFromListUID = "CFL_0";
        }

        /// <summary>
        /// Fill choose from list.
        /// </summary>
        private void LoadChoosesFromList()
        {
            SAPbouiCOM.ChooseFromList lObjCFLClients = InitChooseFromLists(false, "2", "CFL_0", this.UIAPIRawForm.ChooseFromLists);
            AddConditionClientCFL(lObjCFLClients);
        }

        public SAPbouiCOM.ChooseFromList InitChooseFromLists(bool pbol, string pStrType, string pStrID, SAPbouiCOM.ChooseFromListCollection pObjCFLs) //
        {
            SAPbouiCOM.ChooseFromList lObjoCFL = null;
            try
            {
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = (SAPbouiCOM.ChooseFromListCreationParams)UIApplication.GetApplication().CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams);

                oCFLCreationParams.MultiSelection = pbol;
                oCFLCreationParams.ObjectType = pStrType;
                oCFLCreationParams.UniqueID = pStrID;

                lObjoCFL = pObjCFLs.Add(oCFLCreationParams);

                this.UIAPIRawForm.DataSources.UserDataSources.Add(pStrID, SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 254);
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitCustomerChooseFromListException: {0}", ex.Message));

            }
            return lObjoCFL;
        }

        private void AddConditionClientCFL(SAPbouiCOM.ChooseFromList pCFL)
        {
            SAPbouiCOM.Condition lObjCon = null;
            SAPbouiCOM.Conditions lObjCons = new SAPbouiCOM.Conditions();

            lObjCon = lObjCons.Add();
            lObjCon.Alias = "CardType";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "C";

            lObjCon.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

            lObjCon = lObjCons.Add();
            lObjCon.Alias = "validFor";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "Y";

            pCFL.SetConditions(lObjCons);
        }

        private void CreateContractsDatatable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTCont");
            dtContracts = this.UIAPIRawForm.DataSources.DataTables.Item("DTCont");
            dtContracts.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtContracts.Columns.Add("DocECont", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtContracts.Columns.Add("DocNCont", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtContracts.Columns.Add("Client", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtContracts.Columns.Add("HrsFt", SAPbouiCOM.BoFieldsType.ft_Quantity);
            dtContracts.Columns.Add("InvExt", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtContracts.Columns.Add("Import", SAPbouiCOM.BoFieldsType.ft_Price);
            dtContracts.Columns.Add("RealHrs", SAPbouiCOM.BoFieldsType.ft_Quantity);
            dtContracts.Columns.Add("Dif", SAPbouiCOM.BoFieldsType.ft_Quantity);
            dtContracts.Columns.Add("Status", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillContractsMatrix();
        }

        private void CreateRiseDataTable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTRise");
            dtRise = this.UIAPIRawForm.DataSources.DataTables.Item("DTRise");
            dtRise.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtRise.Columns.Add("DocNmRise", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtRise.Columns.Add("HrsFt", SAPbouiCOM.BoFieldsType.ft_Price);

            FillRiseMatrix();
        }

        private void FillRiseMatrix()
        {
            mtxRise.Columns.Item("#").DataBind.Bind("DTRise", "#");
            mtxRise.Columns.Item("ColRiseF").DataBind.Bind("DTRise", "DocNmRise");
            mtxRise.Columns.Item("ColHrsFt").DataBind.Bind("DTRise", "HrsFt");

            SAPbouiCOM.LinkedButton oLink = (SAPbouiCOM.LinkedButton)mtxRise.Columns.Item("ColRiseF").ExtendedObject;
            //oLink.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_Order;

            mtxRise.AutoResizeColumns();
        }

        private void FillContractsMatrix()
        {
            mtxContracts.Columns.Item("#").DataBind.Bind("DTCont", "#");
            mtxContracts.Columns.Item("ColCont").DataBind.Bind("DTCont", "DocNCont");
            mtxContracts.Columns.Item("ColClient").DataBind.Bind("DTCont", "Client");
            mtxContracts.Columns.Item("ColHrsFt").DataBind.Bind("DTCont", "HrsFt");
            mtxContracts.Columns.Item("ColFact").DataBind.Bind("DTCont", "InvExt");
            mtxContracts.Columns.Item("ColImp").DataBind.Bind("DTCont", "Import");
            mtxContracts.Columns.Item("ColRHrs").DataBind.Bind("DTCont", "RealHrs");
            mtxContracts.Columns.Item("ColDif").DataBind.Bind("DTCont", "Dif");
            mtxContracts.Columns.Item("ColStatus").DataBind.Bind("DTCont", "Status");

            SAPbouiCOM.LinkedButton oLink = (SAPbouiCOM.LinkedButton)mtxContracts.Columns.Item("ColCont").ExtendedObject;
            //oLink.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_Order;

            mtxContracts.AutoResizeColumns();
        }

        private void ChooseFromListAfterEvent(SAPbouiCOM.ItemEvent pObjValEvent)
        {
            if (pObjValEvent.Action_Success)
            {
                SAPbouiCOM.IChooseFromListEvent lObjCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pObjValEvent;
                SAPbouiCOM.DataTable lObjDataTable = lObjCFLEvento.SelectedObjects;
                if (lObjCFLEvento.SelectedObjects == null)
                    return;

                //this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = Convert.ToString(lObjDataTable.GetValue(0, 0));

                if (lObjDataTable.UniqueID == "CFL_0")
                {
                    mStrClientCode = Convert.ToString(lObjDataTable.GetValue(0, 0));
                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = Convert.ToString(lObjDataTable.GetValue(1, 0));
                }
            }
        }

        private void ClearMatrix(string pStrDTName, SAPbouiCOM.Matrix pObjMatrix)
        {
            if (!this.UIAPIRawForm.DataSources.DataTables.Item(pStrDTName).IsEmpty)
            {
                this.UIAPIRawForm.DataSources.DataTables.Item(pStrDTName).Rows.Clear();
                pObjMatrix.Clear();
            }
        }
        #endregion

        #region Controls
        private SAPbouiCOM.StaticText lblContracts;
        private SAPbouiCOM.StaticText lblClient;
        private SAPbouiCOM.StaticText lblStatus;
        private SAPbouiCOM.EditText txtContracts;
        private SAPbouiCOM.EditText txtClient;
        private SAPbouiCOM.StaticText lblSDate;
        private SAPbouiCOM.EditText txtStartDate;
        private SAPbouiCOM.StaticText lblEDate;
        private SAPbouiCOM.EditText txtEndDate;
        private SAPbouiCOM.Button btnSearch;
        private SAPbouiCOM.Matrix mtxContracts;
        private SAPbouiCOM.Matrix mtxRise;
        private SAPbouiCOM.ComboBox cboStatus;
        private SAPbouiCOM.DataTable dtContracts;
        private SAPbouiCOM.DataTable dtRise;
        #endregion
    }
}
