using System;
using System.Collections.Generic;
using SAPbouiCOM.Framework;
using UGRS.Core.Utility;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.SDK.DI.Transports;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI;
using System.Threading;
using UGRS.Core.SDK.DI.Transports.Tables;

namespace UGRS.AddOn.Transports.Forms
{
    [FormAttribute("UGRS.AddOn.Transports.Forms.frmDriversCommissions", "Forms/frmDriversCommissions.b1f")]
    class frmDriversCommissions : UserFormBase
    {
        #region Properties
        TransportServiceFactory mObjTransportServiceFactory = new TransportServiceFactory();
        private frmCFLFolios mObjFrmFolios = null;
        private string mStrFilePath = string.Empty;
        #endregion

        #region Constructor
        public frmDriversCommissions()
        {
            LoadEvents();

            LoadInitialsControls();
        }
        #endregion

        #region InitializeComponent
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lblSearch = ((SAPbouiCOM.StaticText)(this.GetItem("lblSearch").Specific));
            this.txtFolio = ((SAPbouiCOM.EditText)(this.GetItem("txtFolio").Specific));
            this.lblStatus = ((SAPbouiCOM.StaticText)(this.GetItem("lblStatus").Specific));
            this.txtStatus = ((SAPbouiCOM.EditText)(this.GetItem("txtStatus").Specific));
            this.lblReference = ((SAPbouiCOM.StaticText)(this.GetItem("lblRef").Specific));
            this.txtReference = ((SAPbouiCOM.EditText)(this.GetItem("txtRef").Specific));
            this.btnFileDialog = ((SAPbouiCOM.Button)(this.GetItem("btnFile").Specific));
            this.btnFileDialog.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnFileDialog_ClickBefore);
            this.mtxCommissions = ((SAPbouiCOM.Matrix)(this.GetItem("mtxCmsn").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("btnSave").Specific));
            this.btnSave.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnSave_ClickBefore);
            this.lblBank = ((SAPbouiCOM.StaticText)(this.GetItem("lblBnk").Specific));
            this.cboAcct = ((SAPbouiCOM.ComboBox)(this.GetItem("cboAcct").Specific));
            this.lblAcct = ((SAPbouiCOM.StaticText)(this.GetItem("lblAcct").Specific));
            this.cboBnk = ((SAPbouiCOM.ComboBox)(this.GetItem("cboBnk").Specific));
            this.btnSearch = ((SAPbouiCOM.Button)(this.GetItem("btnSearch").Specific));
            this.btnSearch.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.OnCustomInitialize();

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

        #region ChooseFromList
        /// <summary>
        /// Fill choose from list.
        /// </summary>
        private void LoadChoosesFromList()
        {
            SAPbouiCOM.ChooseFromList lObjCFLSup = InitChooseFromLists(false, "1", "CFL_Acct", this.UIAPIRawForm.ChooseFromLists);
            AddConditionAccountCFL(lObjCFLSup);
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

        private void AddConditionAccountCFL(SAPbouiCOM.ChooseFromList pCFL)
        {
           // SAPbouiCOM.Condition lObjCon = null;
           // SAPbouiCOM.Conditions lObjCons = new SAPbouiCOM.Conditions();

            //lObjCon = lObjCons.Add();
            //lObjCon.Alias = "position";
            //lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            //lObjCon.CondVal = "4";

            //lObjCon.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

            //lObjCon = lObjCons.Add();
            //lObjCon.Alias = "dept";
            //lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            //lObjCon.CondVal = "12";

            //pCFL.SetConditions(lObjCons);
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
                //if (pVal.FormTypeEx.Equals("UGRS.AddOn.Machinery.Forms.frmCFLFolios"))
                //{
                //    if (!pVal.BeforeAction)
                //    {
                //        switch (pVal.EventType)
                //        {
                //            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                //                break;
                //        }
                //    }
                //}

                if (pVal.FormTypeEx.Equals("UGRS.AddOn.Transports.Forms.frmCFLFolios"))
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

                                    txtFolio.Value = mObjFrmFolios.mStrFolio;

                                    //Cargar operadores, contratos y registros iniciales (reg. finales de UDT)
                                    LoadCommissionDetails(txtFolio.Value);
                                    mObjFrmFolios = null;
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
                                ChooseFromListAfterEvent(pVal);
                                break;
                            case SAPbouiCOM.BoEventTypes.et_COMBO_SELECT:
                                // Selected Bank on ComboBox
                                if (pVal.ItemUID.Equals(cboBnk.Item.UniqueID))
                                {
                                    LoadAccounts();
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_FORM_RESIZE:
                                mtxCommissions.AutoResizeColumns();
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
                LogUtility.WriteError(String.Format("[frmDriversCommissions - SBO_Application_ItemEvent] Error: {0}", ex.Message));

                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
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

                if (lObjDataTable.UniqueID == "CFL_Acct")
                {
                    string lStrAccount = Convert.ToString(lObjDataTable.GetValue(0, 0));
                    //this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = Convert.ToString(lObjDataTable.GetValue(0, 0));
                }
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {

        }

        private void btnFileDialog_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            CreateFolderBroserThread();
        }

        private void btnSave_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (Application.SBO_Application.MessageBox("¿Desea crear la comisión?", 1, "Aceptar", "Cancelar", "") != 1)
                {
                    return;
                }

                this.UIAPIRawForm.Freeze(true);

                CreateJournalEntry();
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

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            mObjFrmFolios = new frmCFLFolios();
            mObjFrmFolios.Show();
        }
        #endregion

        #region Functions
        public void LoadInitialsControls()
        {
            try
            {
                //this.UIAPIRawForm.Freeze(true);

                LoadBanks();
                CreateCommissionsDatatable();

                //LoadChoosesFromList();
                //SetCFLToTxt();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[frmDriversCommissions - LoadInitialsControls] Error: {0}", lObjException.Message));
                Application.SBO_Application.SetStatusBarMessage(lObjException.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                //this.UIAPIRawForm.Freeze(false);
            }
        }

        public void LoadCommissionDetails(string pStrFolio)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (string.IsNullOrEmpty(pStrFolio))
                {
                    UIApplication.ShowError("Seleccione un folio de comisión");
                    return;
                }

                string lStrCmsDriverAcct = new QueryManager().GetValue("U_VALUE", "Name", "TR_ACC_LIQCHOF", "[@UG_Config]");
                if (string.IsNullOrEmpty(lStrCmsDriverAcct))
                {
                    UIApplication.ShowError("Agregue un valor en la configuración para el campo TR_ACC_LIQCHOF");
                    return;
                }

                ClearMatrix("DTCom", mtxCommissions);

                IList<CommissionDriverDetailsDTO> lLstCommissionDetails = mObjTransportServiceFactory.GetCommissionDriverService().GetCommissionDriversDetails(pStrFolio, lStrCmsDriverAcct);
                foreach (var lObjCommission in lLstCommissionDetails)
                {
                    AddRiseCommissionDetail(lObjCommission);
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[frmDriversCommissions - LoadCommissionDetails] Error: {0}", lObjException.Message));
                
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void AddRiseCommissionDetail(CommissionDriverDetailsDTO pObjCommissionDetail)
        {
            try
            {
                if (pObjCommissionDetail == null)
                    return;

                dtCommissions.Rows.Add();
                dtCommissions.SetValue("#", dtCommissions.Rows.Count - 1, dtCommissions.Rows.Count);
                dtCommissions.SetValue("FolioCms", dtCommissions.Rows.Count - 1, pObjCommissionDetail.Folio);
                dtCommissions.SetValue("EmpId", dtCommissions.Rows.Count - 1, pObjCommissionDetail.EmployeeId);
                dtCommissions.SetValue("EmpNm", dtCommissions.Rows.Count - 1, pObjCommissionDetail.Employee);
                dtCommissions.SetValue("Import", dtCommissions.Rows.Count - 1, pObjCommissionDetail.Import);
                dtCommissions.SetValue("Acct", dtCommissions.Rows.Count - 1, pObjCommissionDetail.Account);

                mtxCommissions.LoadFromDataSource();
                mtxCommissions.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el detalle de la comisión: {0}", lObjException.Message));
            }
        }

        private void CreateJournalEntry()
        {
            try
            {
                if (cboAcct.Selected == null)
                {
                    UIApplication.ShowError("Seleccione una cuenta");
                    return;
                }

                string lStrBankAccountNum = cboAcct.Selected.Value;
                if (string.IsNullOrEmpty(lStrBankAccountNum))
                {
                    UIApplication.ShowError("Seleccione una cuenta");
                    return;
                }

                string lStrBankAccountCode = mObjTransportServiceFactory.GetBankService().GetBankAccount(lStrBankAccountNum).GLAccount;
                string lStrFolio = txtFolio.Value;
                string lStrReference = txtReference.Value;
                string lStrCmsDriverAcct = new QueryManager().GetValue("U_VALUE", "Name", "TR_ACC_LIQCHOF", "[@UG_Config]");
                string lStrFilePath = string.Empty;

                if (string.IsNullOrEmpty(lStrFolio))
                {
                    UIApplication.ShowError("Seleccione un folio de comisión");
                    return;
                }

                if (string.IsNullOrEmpty(lStrBankAccountCode))
                {
                    UIApplication.ShowError(string.Format("No se encontró la cuenta para la cuenta bancaria {0}", lStrBankAccountNum));
                    return;
                }

                if (string.IsNullOrEmpty(lStrCmsDriverAcct))
                {
                    UIApplication.ShowError("Agregue un valor en la configuración para el campo TR_ACC_LIQCHOF");
                    return;
                }

                if (dtCommissions.Rows.Count == 0)
                {
                    UIApplication.ShowError("No puede crear la comisión sin líneas");
                    return;
                }

                if (!string.IsNullOrEmpty(mStrFilePath))
                {
                    //mObjTransportServiceFactory.GetAttachmentDI().AttachFile(mStrFilePath);
                    lStrFilePath = AttatchFile(mStrFilePath);
                }

                SAPbobsCOM.JournalEntries lObjJournalEntry = (SAPbobsCOM.JournalEntries)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
                lObjJournalEntry.TransactionCode = "TRPG";
                lObjJournalEntry.Reference = lStrFolio;
                lObjJournalEntry.Reference2 = lStrReference;
                lObjJournalEntry.ReferenceDate = DateTime.Now;
                lObjJournalEntry.TaxDate = DateTime.Now;
                lObjJournalEntry.DueDate = DateTime.Now;

                for (int i = 0; i < dtCommissions.Rows.Count; i++)
                {
                    string lStrEmpId = dtCommissions.GetValue("EmpId", i).ToString();
                    string lStrEmployee = dtCommissions.GetValue("EmpNm", i).ToString();
                    double lDblImport = double.Parse(dtCommissions.GetValue("Import", i).ToString());
                    string lStrFolioLine = dtCommissions.GetValue("FolioCms", i).ToString();
                    string lStrAcctLine = dtCommissions.GetValue("Acct", i).ToString();

                    lObjJournalEntry.Lines.SetCurrentLine(lObjJournalEntry.Lines.Count - 1);
                    lObjJournalEntry.Lines.TaxDate = DateTime.Now;
                    lObjJournalEntry.Lines.AccountCode = lStrCmsDriverAcct;
                    lObjJournalEntry.Lines.Debit = lDblImport;
                    lObjJournalEntry.Lines.Credit = 0;
                    lObjJournalEntry.Lines.CostingCode = "TR_TRANS";
                    lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_TypeAux").Value = "2";
                    lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_Auxiliar").Value = lStrEmpId; //code empleado
                    lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_CodeMov").Value = lStrFolio;
                    lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_File").Value = lStrFilePath;

                    lObjJournalEntry.Lines.Add();

                    lObjJournalEntry.Lines.SetCurrentLine(lObjJournalEntry.Lines.Count - 1);
                    lObjJournalEntry.Lines.TaxDate = DateTime.Now;
                    lObjJournalEntry.Lines.AccountCode = lStrBankAccountCode;
                    lObjJournalEntry.Lines.Debit = 0;
                    lObjJournalEntry.Lines.Credit = lDblImport;
                    lObjJournalEntry.Lines.CostingCode = "TR_TRANS";
                    lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_TypeAux").Value = "2";
                    lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_Auxiliar").Value = lStrEmpId; //code empleado
                    lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_CodeMov").Value = lStrFolio;
                    lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_File").Value = lStrFilePath;
                    
                    lObjJournalEntry.Lines.Add();
                }

                if (lObjJournalEntry.Add() != 0)
                {
                    string lStrLastError = DIApplication.Company.GetLastErrorDescription();
                    UIApplication.ShowMessageBox(string.Format("Error al generar el asiento de la comisión: {0}", DIApplication.Company.GetLastErrorDescription()));
                }
                else
                {
                    int lIntDocEntry = int.Parse(DIApplication.Company.GetNewObjectKey());

                    LogUtility.WriteSuccess(String.Format("[frmDriversCommissions - CreateJournalEntry] Comisión creada correctamente con el DocEntry {0} para el Folio: {1}", lIntDocEntry, lStrFolio));

                    Commissions lObjCommission = mObjTransportServiceFactory.GetCommissionService().GetCommission(lStrFolio);
                    lObjCommission.HasDriverCms = "Y";

                    if (mObjTransportServiceFactory.GetCommissionService().UpdateCommission(lObjCommission) != 0)
                    {
                        throw new Exception(string.Format("Error al modificar el estatus de la comisión {0}", lStrFolio));
                    }


                    //SaveCommissionsRecords(lIntDocEntry);

                    ClearControls();

                    UIApplication.ShowSuccess(string.Format("Comisión creada correctamente con el número de documento: {0}", lIntDocEntry));
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmDriversCommissions - CreateJournalEntry] Error al crear el asiento contable: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al crear el asiento contable: {0}", lObjException.Message));
            }
            finally
            {
                
            }
        }

        private string AttatchFile(string pStrFile)
        {
            int lIntAttachement = 0;
            string lStrAttach = string.Empty;
            string lStrAttachPath = mObjTransportServiceFactory.GetAttachmentDI().GetAttachPath();
            if (!string.IsNullOrEmpty(pStrFile))
            {
                lIntAttachement = mObjTransportServiceFactory.GetAttachmentDI().AttachFile(pStrFile);
                if (lIntAttachement > 0)
                {
                    lStrAttach = lStrAttachPath + System.IO.Path.GetFileName(pStrFile);
                }
                else
                {
                    LogUtility.WriteError("[InvoiceDI - AttachDocument] " + DIApplication.Company.GetLastErrorDescription());
                    UIApplication.ShowError(string.Format("[InvoiceDI - AttachDocument] : {0}", DIApplication.Company.GetLastErrorDescription()));
                    if (System.IO.File.Exists(pStrFile))
                    {
                        lStrAttach = pStrFile;
                    }
                    else
                    {
                        LogUtility.WriteError("[InvoiceDI - AttachDocument] Archivo \n" + pStrFile + " no encontrado");
                        UIApplication.ShowError("[InvoiceDI - AttachDocument] Archivo  \n" + pStrFile + " no encontrado");
                    }
                }
            }
            return lStrAttach;
        }

        private void LoadBanks()
        {
            try
            {
                //cboBnk.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
                //cboBnk.ExpandType = SAPbouiCOM.BoExpandType.et_ValueOnly;

                IList<BankDTO> lLstObjBanks = mObjTransportServiceFactory.GetBankService().GetBanks();
                foreach (BankDTO lObjBank in lLstObjBanks)
                {
                    cboBnk.ValidValues.Add(lObjBank.BankCode, lObjBank.BankName);
                }

                cboBnk.ValidValues.Add("", "");
                
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[frmDriversCommissions - LoadBanks] Error: {0}", ex.Message));
                UIApplication.ShowMessageBox(ex.Message);
            }
        }

        public void LoadAccounts()
        {
            try
            {
                //cboAcct.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
                //Remove old valid values
                CleanCboItems(cboAcct);

                cboAcct.Item.Enabled = true;

                IList<AccountDTO> ListAccounts = mObjTransportServiceFactory.GetBankService().GetBankAccounts(cboBnk.Value.ToString());
                foreach (AccountDTO lObjAccount in ListAccounts)
                {
                    cboAcct.ValidValues.Add(lObjAccount.Account, lObjAccount.Account);
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[frmDriversCommissions - LoadAccounts] Error: {0}", ex.Message));
                UIApplication.ShowMessageBox(ex.Message);
            }
        }

        public void CleanCboItems(SAPbouiCOM.ComboBox pObjCbo)
        {
            if (pObjCbo.ValidValues.Count > 0)
            {
                foreach (var item in pObjCbo.ValidValues)
                {
                    pObjCbo.ValidValues.Remove(pObjCbo.ValidValues.Count - 1, SAPbouiCOM.BoSearchKey.psk_Index);
                }

                //pObjCbo.Description = string.Empty;
                /*if (string.IsNullOrEmpty(pObjCbo.Value))
                    pObjCbo.ValidValues.Add(string.Empty, string.Empty);*/
            }
        }

        private void SetCFLToTxt()
        {
            //txtAccount.DataBind.SetBound(true, "", "CFL_Acct");
            //txtAccount.ChooseFromListUID = "CFL_Acct";
        }

        private void ClearMatrix(string pStrDTName, SAPbouiCOM.Matrix pObjMatrix)
        {
            if (!this.UIAPIRawForm.DataSources.DataTables.Item(pStrDTName).IsEmpty)
            {
                this.UIAPIRawForm.DataSources.DataTables.Item(pStrDTName).Rows.Clear();
                pObjMatrix.Clear();
            }
        }

        private void CreateCommissionsDatatable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTCom");
            dtCommissions = this.UIAPIRawForm.DataSources.DataTables.Item("DTCom");
            dtCommissions.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtCommissions.Columns.Add("EmpId", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtCommissions.Columns.Add("EmpNm", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtCommissions.Columns.Add("Import", SAPbouiCOM.BoFieldsType.ft_Float);
            dtCommissions.Columns.Add("FolioCms", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtCommissions.Columns.Add("Acct", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillContractsMatrix();
        }

        private void FillContractsMatrix()
        {
            mtxCommissions.Columns.Item("#").DataBind.Bind("DTCom", "#");
            mtxCommissions.Columns.Item("ColName").DataBind.Bind("DTCom", "EmpNm");
            mtxCommissions.Columns.Item("ColImport").DataBind.Bind("DTCom", "Import");

            mtxCommissions.AutoResizeColumns();
        }

        private void ClearControls()
        {
            txtFolio.Value = string.Empty;
            txtStatus.Value = string.Empty;
            txtReference.Value = string.Empty;
            mStrFilePath = string.Empty;
            mObjFrmFolios = null;
            ClearMatrix("DTCom", mtxCommissions);
        }
        #endregion

        #region OpenFile
        /// <summary>
        /// Crea hilo para abrir carpeta 
        /// </summary>
        private void CreateFolderBroserThread()
        {
            try
            {
                Thread ShowFolderBroserThread = new Thread(OpenFileBrowser);
                if (ShowFolderBroserThread.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    ShowFolderBroserThread.SetApartmentState(System.Threading.ApartmentState.STA);
                    ShowFolderBroserThread.Start();
                }
                else
                {
                    ShowFolderBroserThread.Start();
                    ShowFolderBroserThread.Join();

                }
                while (ShowFolderBroserThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(ex.Message);
                LogUtility.WriteError("[frmDriversCommissions - CreateFolderBroserThread] " + ex.Message);
                LogUtility.WriteError(ex.Message);
            }
        }

        /// <summary>
        /// Abre dialogo de seleccionar archivo
        /// </summary>
        private void OpenFileBrowser()
        {
            try
            {
                string lStrFileName = ShowFolderBrowser();
                mStrFilePath = lStrFileName;
            }
            catch (Exception ex)
            {
                LogUtility.WriteError("[frmDriversCommissions - OpenFileBrowser] " + ex.Message);
                LogUtility.WriteError(ex.Message);
            }
        }


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        public string ShowFolderBrowser()
        {
            string lStrfileName = "";
            using (System.Windows.Forms.OpenFileDialog lObjFile = new System.Windows.Forms.OpenFileDialog())
            {
                try
                {
                    IntPtr sapProc = GetForegroundWindow();
                    WindowWrapper MyWindow = null;

                    MyWindow = new WindowWrapper(sapProc);

                    lObjFile.Multiselect = false;

                    //GetFileDialogFilter(pStrBank, lObjFile);
                    //oFile.Filter = "Archivos Excel(*.xls)|*.xls|Archivos TXT(*.txt)|*.txt|Archivos CSV(*.csv)|*.csv";
                    lObjFile.FilterIndex = 0;
                    lObjFile.RestoreDirectory = true;
                    var dialogResult = lObjFile.ShowDialog(MyWindow);

                    if (dialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        lStrfileName = lObjFile.FileName;
                    }
                }
                catch (Exception ex)
                {
                    LogUtility.WriteError("[frmDriversCommissions - ShowFolderBrowser] " + ex.Message);
                    LogUtility.WriteError(ex.Message);
                }
            }
            return lStrfileName;
        }
        #endregion

        #region Controls
        private SAPbouiCOM.StaticText lblSearch;
        private SAPbouiCOM.EditText txtFolio;
        private SAPbouiCOM.StaticText lblStatus;
        private SAPbouiCOM.EditText txtStatus;
        private SAPbouiCOM.StaticText lblReference;
        private SAPbouiCOM.EditText txtReference;
        private SAPbouiCOM.Button btnFileDialog;
        private SAPbouiCOM.Matrix mtxCommissions;
        private SAPbouiCOM.Button btnSave;
        private SAPbouiCOM.StaticText lblBank;
        private SAPbouiCOM.ComboBox cboAcct;
        private SAPbouiCOM.StaticText lblAcct;
        private SAPbouiCOM.ComboBox cboBnk;
        private SAPbouiCOM.DataTable dtCommissions;
        private SAPbouiCOM.Button btnSearch;
        #endregion
    }
}
