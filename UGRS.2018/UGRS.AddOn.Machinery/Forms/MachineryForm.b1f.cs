using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UGRS.AddOn.Machinery.Enums;
using UGRS.AddOn.Machinery.Utilities;
using UGRS.Core.Extension.Enum;
using UGRS.Core.SDK.DI.Machinery;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery.Enums;
using UGRS.Core.SDK.DI.Machinery.Tables;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.Utility;

namespace UGRS.AddOn.Machinery.Forms
{
    [FormAttribute("UGRS.AddOn.Machinery.Forms.MachineryForm", "Forms/MachineryForm.b1f")]
    class MachineryForm : UserFormBase
    {
        #region Properties
        private MachinerySeviceFactory mObjMachineryServiceFactory = new MachinerySeviceFactory();
        private Rise mObjRise = null;
        private string mStrClientCode = string.Empty;
        string mStrSupervisorCode = string.Empty;
        string mStrRelationFolio = string.Empty;
        private frmCFLFolios mObjFrmFolios = null;
        private frmCFLOrdersSale mObjFrmContractsList = null;
        private frmContracts mObjFrmContracts = null;
        private frmTravelExpenses mObjFrmTravelExpenses = null;
        private frmGoodIssue mFrmGoodIssue;
        private int mIntFirstInvReqDocEntry;
        private string mStrMatrixSelected = string.Empty;
        private int mIntMatrixRowSelected = 0;
        private List<string> mLstEmployeesToDelete = new List<string>();
        private List<string> mLstContractsToDelete = new List<string>();
        private bool mBolForSearchMode = false;
        private bool mBolIsOperationsUser = false;
        #endregion

        #region Constructor
        public MachineryForm(int pIntRiseId = 0)
        {
            try
            {
                LogService.WriteInfo(string.Format("[MachineryForm]: {0}", "Abriendo forma de subidas"));

                this.UIAPIRawForm.Freeze(true);

                oForm = Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID);

                mBolIsOperationsUser = mObjMachineryServiceFactory
                        .GetAuthorizationService()
                        .IsOperationsUser(
                            mObjMachineryServiceFactory
                            .GetUsersService()
                            .GetUserId(Application.SBO_Application.Company.UserName)); //, this.UIAPIRawForm.UniqueID, (int)FunctionsEnum.OpenRise);

                ShowOrHideOpenBtn();

                LoadEvents();

                LoadInitialsControls();

                if (pIntRiseId > 0)
                {
                    CleanControls(true);
                    txtFolio.Value = pIntRiseId.ToString();
                    StartSearchMode();
                }
            }
            catch (Exception lObjException)
            {
                UIApplication.ShowError(string.Format("Error: {0}", lObjException.Message));
                LogUtility.WriteError(string.Format("[MachineryForm - MachineryForm] Error: {0}", lObjException.Message));
                throw lObjException;
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lblFolio = ((SAPbouiCOM.StaticText)(this.GetItem("lblFolio").Specific));
            this.lblDate = ((SAPbouiCOM.StaticText)(this.GetItem("lblDate").Specific));
            this.lblClient = ((SAPbouiCOM.StaticText)(this.GetItem("lblClient").Specific));
            this.lblSupervisor = ((SAPbouiCOM.StaticText)(this.GetItem("lblSuprv").Specific));
            this.txtFolio = ((SAPbouiCOM.EditText)(this.GetItem("txtFolio").Specific));
            this.txtFolio.KeyDownAfter += new SAPbouiCOM._IEditTextEvents_KeyDownAfterEventHandler(this.txtFolio_KeyDownAfter);
            this.txtDate = ((SAPbouiCOM.EditText)(this.GetItem("txtDate").Specific));
            this.txtClient = ((SAPbouiCOM.EditText)(this.GetItem("txtClient").Specific));
            this.txtSupervisor = ((SAPbouiCOM.EditText)(this.GetItem("txtSupv").Specific));
            this.txtFolioRelation = ((SAPbouiCOM.EditText)(this.GetItem("txtFolRel").Specific));
            this.lblFolioRel = ((SAPbouiCOM.StaticText)(this.GetItem("lblFolRel").Specific));
            this.txtStatus = ((SAPbouiCOM.EditText)(this.GetItem("txtStatus").Specific));
            this.lblStatus = ((SAPbouiCOM.StaticText)(this.GetItem("lblStatus").Specific));
            this.btnOpen = ((SAPbouiCOM.Button)(this.GetItem("btnOpen").Specific));
            this.btnClose = ((SAPbouiCOM.Button)(this.GetItem("btnClose").Specific));
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCancel").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("btnSave").Specific));
            this.btnSearchFolio = ((SAPbouiCOM.Button)(this.GetItem("btnSearch").Specific));
            this.tabGeneral = ((SAPbouiCOM.Folder)(this.GetItem("tabGen").Specific));
            this.tabCons = ((SAPbouiCOM.Folder)(this.GetItem("tabConsu").Specific));
            this.tabHours = ((SAPbouiCOM.Folder)(this.GetItem("tabHours").Specific));
            this.tabRend = ((SAPbouiCOM.Folder)(this.GetItem("tabRend").Specific));
            this.mtxEmployees = ((SAPbouiCOM.Matrix)(this.GetItem("mtxOpr").Specific));
            this.mtxEmployees.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.mtxEmployees_ValidateBefore);
            this.lblOperadores = ((SAPbouiCOM.StaticText)(this.GetItem("lblOpe").Specific));
            this.lblOV = ((SAPbouiCOM.StaticText)(this.GetItem("lblCont").Specific));
            this.mtxContracts = ((SAPbouiCOM.Matrix)(this.GetItem("mtxCont").Specific));
            this.mtxContracts.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtxContracts_LinkPressedBefore);
            this.btnVincOV = ((SAPbouiCOM.Button)(this.GetItem("btnVincOV").Specific));
            this.btnCreateOV = ((SAPbouiCOM.Button)(this.GetItem("btnCreate").Specific));
            this.lblConsumables = ((SAPbouiCOM.StaticText)(this.GetItem("lblCons").Specific));
            this.mtxConsumables = ((SAPbouiCOM.Matrix)(this.GetItem("mtxCons").Specific));
            //   this.mtxConsumables.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtxConsumables_LinkPressedBefore);
            this.btnConsumables = ((SAPbouiCOM.Button)(this.GetItem("btnCons").Specific));
            this.lblTravelExpenses = ((SAPbouiCOM.StaticText)(this.GetItem("lblViat").Specific));
            this.mtxTravelExpenses = ((SAPbouiCOM.Matrix)(this.GetItem("mtxViat").Specific));
            this.mtxTravelExpenses.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtxTravelExpenses_LinkPressedBefore);
            this.btnTravelExpenses = ((SAPbouiCOM.Button)(this.GetItem("btnSolVia").Specific));
            this.tabGeneral.Item.Click();
            this.txtStartDate = ((SAPbouiCOM.EditText)(this.GetItem("txtSDate").Specific));
            this.lblStarDate = ((SAPbouiCOM.StaticText)(this.GetItem("lblSDate").Specific));
            this.txtEndDate = ((SAPbouiCOM.EditText)(this.GetItem("txtEDate").Specific));
            this.lblEndDate = ((SAPbouiCOM.StaticText)(this.GetItem("lblEDate").Specific));
            this.lblInitialRecords = ((SAPbouiCOM.StaticText)(this.GetItem("lblIncRds").Specific));
            this.mtxInitialRecords = ((SAPbouiCOM.Matrix)(this.GetItem("mtxIncRds").Specific));
            this.mtxInitialRecords.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.mtxInitialRecords_ValidateBefore);
            this.lblPurchase = ((SAPbouiCOM.StaticText)(this.GetItem("lblPurch").Specific));
            this.mtxPurchase = ((SAPbouiCOM.Matrix)(this.GetItem("mtxPurch").Specific));
            this.lblFinalRecords = ((SAPbouiCOM.StaticText)(this.GetItem("lblFnlRds").Specific));
            this.lblConsumedTotal = ((SAPbouiCOM.StaticText)(this.GetItem("lblTotCon").Specific));
            this.btnExit = ((SAPbouiCOM.Button)(this.GetItem("btnExit").Specific));
            this.mtxFinalRecords = ((SAPbouiCOM.Matrix)(this.GetItem("mtxFinRds").Specific));
            this.mtxFinalRecords.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.mtxFinalRecords_ValidateBefore);
            this.mtxConsumedTotals = ((SAPbouiCOM.Matrix)(this.GetItem("mtxTotCon").Specific));
            this.mtxHours = ((SAPbouiCOM.Matrix)(this.GetItem("mtxHrs").Specific));
            this.mtxHours.ComboSelectAfter += new SAPbouiCOM._IMatrixEvents_ComboSelectAfterEventHandler(this.mtxHours_ComboSelectAfter);
            this.mtxHours.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.mtxHours_ValidateBefore);
            this.lblFootHoursTotal = ((SAPbouiCOM.StaticText)(this.GetItem("lblTHP").Specific));
            this.lblKmHrTotal = ((SAPbouiCOM.StaticText)(this.GetItem("lblTKmH").Specific));
            this.txtFootHoursTotal = ((SAPbouiCOM.EditText)(this.GetItem("txtTHP").Specific));
            this.txtKmHrTotal = ((SAPbouiCOM.EditText)(this.GetItem("txtTKmH").Specific));
            this.lblTransitHours = ((SAPbouiCOM.StaticText)(this.GetItem("lblHrsTran").Specific));
            this.mtxTransitHours = ((SAPbouiCOM.Matrix)(this.GetItem("mtxHrsTra").Specific));
            this.mtxTransitHours.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.mtxTransitHours_ValidateBefore);
            this.lblTotalsHours = ((SAPbouiCOM.StaticText)(this.GetItem("lblTotHT").Specific));
            this.txtTotalsHours = ((SAPbouiCOM.EditText)(this.GetItem("txtTotHT").Specific));
            this.btnCalculatePerf = ((SAPbouiCOM.Button)(this.GetItem("btnRenCal").Specific));
            this.mtxMachPerformance = ((SAPbouiCOM.Matrix)(this.GetItem("mtxRenMaq").Specific));
            this.mtxVclPerformance = ((SAPbouiCOM.Matrix)(this.GetItem("mtxRenVcl").Specific));
            this.btnSaveIR = ((SAPbouiCOM.Button)(this.GetItem("btnSavIR").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);

        }

        private void OnCustomInitialize()
        {
            UIAPIRawForm.EnableMenu("1293", true); //Borrar
        }
        #endregion

        #region Load & Unload Events
        private void LoadEvents()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            Application.SBO_Application.FormDataEvent += new SAPbouiCOM._IApplicationEvents_FormDataEventEventHandler(SBO_Application_FormEvent);
            UIApplication.GetApplication().MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(this.SBO_Application_MenuEvent);
            UIApplication.GetApplication().RightClickEvent += new SAPbouiCOM._IApplicationEvents_RightClickEventEventHandler(this.SBO_Application_RightClickEvent);
            UIApplication.GetApplication().MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(SBO_Application_ApplicationMenuEvent);

            //UIApplication.GetApplication().MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(SBO_Application_MenuEvent);
            //SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(ChooseFromListAfterEvent);
        }

        private void UnLoadEvents()
        {
            Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            Application.SBO_Application.FormDataEvent -= new SAPbouiCOM._IApplicationEvents_FormDataEventEventHandler(SBO_Application_FormEvent);
            UIApplication.GetApplication().MenuEvent -= new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(this.SBO_Application_MenuEvent);
            UIApplication.GetApplication().RightClickEvent -= new SAPbouiCOM._IApplicationEvents_RightClickEventEventHandler(this.SBO_Application_RightClickEvent);
            UIApplication.GetApplication().MenuEvent -= new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(SBO_Application_ApplicationMenuEvent);
            //SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(ChooseFromListAfterEvent);
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

                                    txtFolioRelation.Value = mObjFrmFolios.mStrFolio;

                                    //Cargar operadores, contratos y registros iniciales (reg. finales de UDT)
                                    LoadRiseRelControls();
                                    mObjFrmFolios = null;
                                }
                                break;
                        }
                    }
                }

                if (pVal.FormTypeEx.Equals("UGRS.AddOn.Machinery.Forms.frmCFLOrdersSale"))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                if (mObjFrmContractsList != null)
                                {
                                    AddContract(mObjFrmContractsList.mObjSelectedContract);
                                    SaveContracts();
                                }
                                break;
                        }
                    }
                }

                if (pVal.FormTypeEx.Equals("UGRS.AddOn.Machinery.Forms.frmContracts"))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                if (mObjFrmContracts != null)
                                {
                                    if (!mObjFrmContracts.mBolFromMenu)
                                    {
                                        AddContract(mObjFrmContracts.mObjCreatedContract);
                                        SaveContracts();
                                    }
                                }
                                break;
                        }
                    }
                }

                if (pVal.FormTypeEx.Equals("UGRS.AddOn.Machinery.Forms.frmTravelExpenses"))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                if (mObjFrmTravelExpenses != null)
                                {
                                    //AddTravelExpenses(mObjFrmTravelExpenses.mObjTravelExpensesDTO);
                                }
                                break;
                        }
                    }
                }

                if (pVal.FormTypeEx.Equals("UGRS.AddOn.Machinery.Forms.frmGoodIssue"))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                if (mFrmGoodIssue != null)
                                {
                                    if (mFrmGoodIssue.mIntDocEntry > 0)
                                    {
                                        SaveInitialsRecords();
                                        SavePurchasesInvReqRecords();
                                        SaveFinalsRecords();
                                        SaveTotalsRecords();

                                        mtxInitialRecords.Item.Enabled = false;
                                        mtxPurchase.Item.Enabled = false;
                                        mtxFinalRecords.Item.Enabled = false;
                                        mtxConsumedTotals.Item.Enabled = false;
                                        btnSaveIR.Item.Enabled = false;
                                    }
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
                                if (pVal.ItemUID.Equals("btnSave"))
                                {
                                    if (!btnSave.Item.Enabled)
                                        return;

                                    SaveRise();
                                }
                                if (pVal.ItemUID.Equals("btnCancel"))
                                {
                                    if (!btnCancel.Item.Enabled)
                                        return;

                                    CancelRise();
                                }
                                if (pVal.ItemUID.Equals("btnClose"))
                                {
                                    if (!btnClose.Item.Enabled)
                                        return;

                                    CloseRise();
                                }
                                if (pVal.ItemUID.Equals("btnOpen"))
                                {
                                    if (!btnOpen.Item.Enabled)
                                        return;

                                    OpenRise();
                                }
                                if (pVal.ItemUID.Equals("btnSearch"))
                                {
                                    if (!btnSearchFolio.Item.Enabled)
                                        return;

                                    OpenFoliosForm();
                                }
                                if (pVal.ItemUID.Equals("btnCons"))
                                {
                                    if (!btnConsumables.Item.Enabled)
                                        return;

                                    OpenInventoryRequestForm();
                                }
                                if (pVal.ItemUID.Equals("btnVincOV"))
                                {
                                    if (!btnVincOV.Item.Enabled)
                                        return;

                                    OpenContractsListForm();
                                }
                                if (pVal.ItemUID.Equals("btnCreate"))
                                {
                                    if (!btnCreateOV.Item.Enabled)
                                        return;

                                    OpenContractForm();
                                }
                                if (pVal.ItemUID.Equals("btnSolVia"))
                                {
                                    if (!btnTravelExpenses.Item.Enabled)
                                        return;

                                    OpenTravelExpensesForm();
                                }
                                if (pVal.ItemUID.Equals("tabConsu"))
                                {
                                    LoadConsumablesControls();
                                }
                                if (pVal.ItemUID.Equals("btnExit"))
                                {
                                    if (!btnExit.Item.Enabled)
                                        return;

                                    OpenGoodIssueForm();
                                }
                                if (pVal.ItemUID.Equals("tabHours"))
                                {
                                    LoadHoursControls();
                                }
                                if (pVal.ItemUID.Equals("btnRenCal"))
                                {
                                    if (!btnCalculatePerf.Item.Enabled)
                                        return;

                                    CalculatePerformance();
                                }
                                if (pVal.ItemUID.Equals("btnSavIR"))
                                {
                                    if (!btnSaveIR.Item.Enabled)
                                        return;

                                    SaveInitialsRecords();
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST:
                                ChooseFromListAfterEvent(pVal);
                                break;
                            case SAPbouiCOM.BoEventTypes.et_FORM_LOAD:
                                LoadInitialsControls();
                                break;
                            case SAPbouiCOM.BoEventTypes.et_VALIDATE:
                                if (pVal.ItemUID.Equals("txtFolio"))
                                {
                                    if (UIApplication.GetApplication().Forms.ActiveForm.Mode == SAPbouiCOM.BoFormMode.fm_FIND_MODE && UIApplication.GetApplication().Forms.ActiveForm.UniqueID == "frmRise")
                                    {
                                        this.UIAPIRawForm.Freeze(true);
                                        StartSearchMode();
                                        this.UIAPIRawForm.Freeze(false);
                                    }
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                UnLoadEvents();
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
                LogUtility.WriteError(string.Format("[MachineryForm - SBO_Application_ItemEvent] Error: {0}", ex.Message));
                UIApplication.ShowError(string.Format("Error: {0}", ex.Message));
                this.UIAPIRawForm.Freeze(false);

                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Menu event handler
        /// </summary>
        /// <param name="pVal"></param>
        /// <param name="BubbleEvent"></param>
        private void SBO_Application_ApplicationMenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (!pVal.BeforeAction)
                {
                    switch (pVal.MenuUID)
                    {
                        case "1281": // Search Record
                            if (UIApplication.GetApplication().Forms.ActiveForm.UniqueID == "frmRise")
                            {
                                this.UIAPIRawForm.Freeze(true);

                                CleanControls(true);
                                CleanCboItems((SAPbouiCOM.Column)mtxTransitHours.Columns.Item("ColMaqHT"));
                                CleanCboItems((SAPbouiCOM.Column)mtxTransitHours.Columns.Item("ColNEnHT"));
                                CleanCboItems((SAPbouiCOM.Column)mtxHours.Columns.Item("ColCOVH"));
                                CleanCboItems((SAPbouiCOM.Column)mtxHours.Columns.Item("ColNumEH"));
                                CleanCboItems((SAPbouiCOM.Column)mtxHours.Columns.Item("ColEqmH"));
                                CleanCboItems((SAPbouiCOM.Column)mtxHours.Columns.Item("ColOprH"));
                                CleanCboItems((SAPbouiCOM.Column)mtxHours.Columns.Item("ColSupH"));
                                CleanCboItems((SAPbouiCOM.Column)mtxHours.Columns.Item("ColTramoH"));
                                CleanCboItems((SAPbouiCOM.Column)mtxTransitHours.Columns.Item("ColOpdHT"));
                                CleanCboItems((SAPbouiCOM.Column)mtxTransitHours.Columns.Item("ColSupHT"));


                                /*SAPbouiCOM.Column lObjMachColumn = (SAPbouiCOM.Column)mtxTransitHours.Columns.Item("ColMaqHT");
                                SAPbouiCOM.Column lObjEcoNumColumn = (SAPbouiCOM.Column)mtxTransitHours.Columns.Item("ColNEnHT");
                                SAPbouiCOM.Column lObjColumn = (SAPbouiCOM.Column)mtxHours.Columns.Item("ColCOVH");
                                SAPbouiCOM.Column lObjEcoNumColumn = (SAPbouiCOM.Column)mtxHours.Columns.Item("ColNumEH");
                                SAPbouiCOM.Column lObjMachColumn = (SAPbouiCOM.Column)mtxHours.Columns.Item("ColEqmH");
                                SAPbouiCOM.Column lObjColumn = (SAPbouiCOM.Column)mtxHours.Columns.Item("ColOprH");
                                (SAPbouiCOM.Column)mtxHours.Columns.Item("ColSupH");*/
                            }
                            break;

                        case "1282": // Add New Record
                            if (UIApplication.GetApplication().Forms.ActiveForm.UniqueID == "frmRise")
                            {
                                BubbleEvent = false;
                            }
                            break;

                        case "1288": // Next Record
                            if (UIApplication.GetApplication().Forms.ActiveForm.UniqueID == "frmRise")
                            {
                                BubbleEvent = false;
                            }
                            break;

                        case "1289": // Pevious Record
                            if (UIApplication.GetApplication().Forms.ActiveForm.UniqueID == "frmRise")
                            {
                                BubbleEvent = false;
                            }
                            break;

                        case "1290": // First Record
                            if (UIApplication.GetApplication().Forms.ActiveForm.UniqueID == "frmRise")
                            {
                                BubbleEvent = false;
                            }
                            break;

                        case "1291": // Last record
                            if (UIApplication.GetApplication().Forms.ActiveForm.UniqueID == "frmRise")
                            {
                                BubbleEvent = false;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - SBO_Application_ApplicationMenuEvent] Error: {0}", ex.Message));
                UIApplication.ShowError(string.Format("MenuEventException: {0}", ex.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        /// <summary>
        /// Event handler for orders sales form.
        /// </summary>
        /// <param name="pVal"></param>
        /// <param name="BubbleEvent"></param>
        public void SBO_Application_FormEvent(ref SAPbouiCOM.BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.FormTypeEx.Equals("1250000940")) //CFL orders sales
                {
                    if (pVal.ActionSuccess)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD:
                                string lStrDocEntry = GetDocEntry(pVal.ObjectKey);
                                ConsumablesDTO lObjConsumable = mObjMachineryServiceFactory.GetConsumablesService().GetInventoryRequestById(int.Parse(lStrDocEntry));
                                AddConsumable(lObjConsumable);
                                break;
                        }
                    }
                }
                else if (pVal.FormTypeEx.Equals("426")) //Payments form
                {
                    if (pVal.ActionSuccess && !pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE:
                                //Actualizar listado de solicitudes de viáticos del grid
                                RefreshStatusTravelExpensesMatrix();
                                break;
                            case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD:
                                string lStrDocEntry = GetDocEntry(pVal.ObjectKey);
                                TravelExpensesDTO lObjTravelExpensesDTO = mObjMachineryServiceFactory.GetTravelExpensesService().GetPayment(int.Parse(lStrDocEntry));
                                AddTravelExpenses(lObjTravelExpensesDTO);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (pVal.FormTypeEx.Equals("139")) //Orders sales form
                {
                    if (pVal.ActionSuccess && !pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE:
                                //Actualizar listado de contratos del grid
                                RefreshContractsStatusMatrix();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - SBO_Application_FormEvent] Error: {0}", ex.Message));
                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }

        private void SBO_Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                mStrMatrixSelected = pVal.ItemUID;
                mIntMatrixRowSelected = pVal.Row;
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - SBO_Application_RightClickEvent] Error: {0}", ex.Message));
                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.MenuUID == "1293" && pVal.BeforeAction == true && UIApplication.GetApplication().Forms.ActiveForm.UniqueID == "frmRise")//Borrar
                {
                    if ((RiseStatusEnum)mObjRise.DocStatus == RiseStatusEnum.ReOpen)
                        return;

                    if (string.IsNullOrEmpty(mStrMatrixSelected) || mIntMatrixRowSelected < 0)
                        return;

                    if (SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("¿Desea elimiar el registro seleccionado?", 2, "Si", "No", "") == 1)
                    {
                        if (mStrMatrixSelected == mtxEmployees.Item.UniqueID) //matrix empleados
                        {
                            string lStrCode = dtEmployees.GetValue("CodeTEmp", mIntMatrixRowSelected - 1).ToString();
                            if (!string.IsNullOrEmpty(lStrCode))
                                mLstEmployeesToDelete.Add(lStrCode);

                            dtEmployees.Rows.Remove(mIntMatrixRowSelected - 1);
                        }
                        else if (mStrMatrixSelected == mtxContracts.Item.UniqueID) //matrix contratos
                        {
                            string lStrCode = dtContracts.GetValue("CodeTCont", mIntMatrixRowSelected - 1).ToString();
                            if (!string.IsNullOrEmpty(lStrCode))
                                mLstContractsToDelete.Add(lStrCode);

                            dtContracts.Rows.Remove(mIntMatrixRowSelected - 1);
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
                LogUtility.WriteError(string.Format("[MachineryForm - SBO_Application_MenuEvent] Error: {0}", ex.Message));
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

                if (lObjDataTable.UniqueID == "CFL_0")
                {
                    mStrClientCode = Convert.ToString(lObjDataTable.GetValue(0, 0));
                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = Convert.ToString(lObjDataTable.GetValue(1, 0));
                }
                else if (lObjDataTable.UniqueID == "CFL_Supv")
                {
                    mStrSupervisorCode = Convert.ToString(lObjDataTable.GetValue(0, 0));
                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = string.Format("{0} {1}", lObjDataTable.GetValue(1, 0).ToString(), lObjDataTable.GetValue(2, 0).ToString());
                }
                if (lObjDataTable.UniqueID == "CFL_Emp")
                {
                    if (ExistItemOnGrid(lObjDataTable.GetValue(0, 0).ToString(), dtEmployees, 1))
                    {
                        Application.SBO_Application.SetStatusBarMessage("No puede repetir el empleado", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                        return;
                    }

                    dtEmployees.SetValue("#", pObjValEvent.Row - 1, pObjValEvent.Row);
                    dtEmployees.SetValue("EmpId", pObjValEvent.Row - 1, lObjDataTable.GetValue(0, 0));
                    dtEmployees.SetValue("EmpName", pObjValEvent.Row - 1, string.Format("{0} {1}", lObjDataTable.GetValue(1, 0).ToString(), lObjDataTable.GetValue(2, 0).ToString()));

                    if (mtxEmployees.RowCount == pObjValEvent.Row)
                    {
                        dtEmployees.Rows.Add();
                        dtEmployees.SetValue("#", pObjValEvent.Row, pObjValEvent.Row + 1);
                    }

                    mtxEmployees.LoadFromDataSource();
                    mtxEmployees.AutoResizeColumns();
                }
            }
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                if (mtxEmployees == null)
                    return;

                this.UIAPIRawForm.Freeze(true);

                #region General
                //Width
                mtxEmployees.Item.Width = UIAPIRawForm.Height / 3;
                mtxEmployees.Item.Height = UIAPIRawForm.Height / 3;
                //Height
                mtxContracts.Item.Height = UIAPIRawForm.Height / 5;

                lblConsumables.Item.Top = mtxContracts.Item.Top + mtxContracts.Item.Height + 5;
                mtxConsumables.Item.Height = UIAPIRawForm.Height / 5;
                mtxConsumables.Item.Top = mtxContracts.Item.Top + mtxContracts.Item.Height + 20;
                btnConsumables.Item.Top = mtxContracts.Item.Top + mtxContracts.Item.Height + 50;

                lblTravelExpenses.Item.Top = mtxConsumables.Item.Top + mtxConsumables.Item.Height + 5;
                mtxTravelExpenses.Item.Height = UIAPIRawForm.Height / 5 - 10;
                mtxTravelExpenses.Item.Top = mtxConsumables.Item.Top + mtxConsumables.Item.Height + 20;
                btnTravelExpenses.Item.Top = mtxConsumables.Item.Top + mtxConsumables.Item.Height + 20;
                #endregion

                #region Consumbales
                mtxInitialRecords.Item.Height = UIAPIRawForm.Height / 7;

                //btnConsumables.Item.Top = mtxInitialRecords.Item.Top + mtxInitialRecords.Item.Height + 20;
                lblPurchase.Item.Top = mtxInitialRecords.Item.Top + mtxInitialRecords.Item.Height + 5;
                mtxPurchase.Item.Height = UIAPIRawForm.Height / 7;
                mtxPurchase.Item.Top = mtxInitialRecords.Item.Top + mtxInitialRecords.Item.Height + 20;

                lblFinalRecords.Item.Top = mtxPurchase.Item.Top + mtxPurchase.Item.Height + 5;
                mtxFinalRecords.Item.Height = UIAPIRawForm.Height / 7;
                mtxFinalRecords.Item.Top = mtxPurchase.Item.Top + mtxPurchase.Item.Height + 20;

                //lblFinalRecords.Item.Top = mtxPurchase.Item.Top + mtxPurchase.Item.Height + 5;
                mtxConsumedTotals.Item.Height = UIAPIRawForm.Height / 7;
                mtxConsumedTotals.Item.Top = mtxFinalRecords.Item.Top + mtxFinalRecords.Item.Height + 20;
                btnExit.Item.Top = mtxFinalRecords.Item.Top + mtxFinalRecords.Item.Height + 50;
                lblConsumedTotal.Item.Top = mtxFinalRecords.Item.Top + mtxFinalRecords.Item.Height + 50;
                #endregion

                #region Hours
                mtxHours.Item.Height = UIAPIRawForm.Height / 3;
                lblFootHoursTotal.Item.Top = mtxHours.Item.Top + mtxHours.Item.Height + 5;
                txtFootHoursTotal.Item.Top = mtxHours.Item.Top + mtxHours.Item.Height + 5;
                lblKmHrTotal.Item.Top = mtxHours.Item.Top + mtxHours.Item.Height + 5;
                txtKmHrTotal.Item.Top = mtxHours.Item.Top + mtxHours.Item.Height + 5;

                mtxTransitHours.Item.Height = UIAPIRawForm.Height / 4;
                mtxTransitHours.Item.Top = mtxHours.Item.Top + mtxHours.Item.Height + 35;
                lblTransitHours.Item.Top = mtxHours.Item.Top + mtxHours.Item.Height + 15;
                lblTotalsHours.Item.Top = mtxTransitHours.Item.Top + mtxTransitHours.Item.Height + 5;
                txtTotalsHours.Item.Top = mtxTransitHours.Item.Top + mtxTransitHours.Item.Height + 5;
                //mtxTransitHours.Item.Width = UIAPIRawForm.Height / 2 + 150;
                #endregion

                #region Performance
                mtxMachPerformance.Item.Height = UIAPIRawForm.Height / 3;
                //mtxMachPerformance.Item.Width = UIAPIRawForm.Height / 2 + 100;

                mtxVclPerformance.Item.Height = UIAPIRawForm.Height / 4;
                mtxVclPerformance.Item.Top = mtxMachPerformance.Item.Top + mtxMachPerformance.Item.Height + 20;
                //mtxVclPerformance.Item.Width = UIAPIRawForm.Height / 2 + 100;
                #endregion

                mtxEmployees.AutoResizeColumns();
                mtxContracts.AutoResizeColumns();
                mtxConsumables.AutoResizeColumns();
                mtxTravelExpenses.AutoResizeColumns();
                mtxInitialRecords.AutoResizeColumns();
                mtxPurchase.AutoResizeColumns();
                mtxFinalRecords.AutoResizeColumns();
                mtxConsumedTotals.AutoResizeColumns();
                mtxHours.AutoResizeColumns();
                mtxTransitHours.AutoResizeColumns();
                mtxMachPerformance.AutoResizeColumns();
                mtxVclPerformance.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[MachineryForm - Form_ResizeAfter]: {0}", lObjException.Message));
                UIApplication.ShowError(string.Format("Error al recalcular el tamaño de los controles: {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void mtxInitialRecords_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (pVal.ColUID == "ColDlMIR" || pVal.ColUID == "ColDlTIR" || pVal.ColUID == "ColGasIR" || pVal.ColUID == "ColKmhIR")
                {
                    string lStrDieselM = (mtxInitialRecords.Columns.Item("ColDlMIR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStrDieselT = (mtxInitialRecords.Columns.Item("ColDlTIR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStrGas = (mtxInitialRecords.Columns.Item("ColGasIR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStrKmHr = (mtxInitialRecords.Columns.Item("ColKmhIR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();

                    double lDblDieselM = string.IsNullOrEmpty(lStrDieselM) ? 0 : double.Parse(lStrDieselM);
                    double lDblDieselT = string.IsNullOrEmpty(lStrDieselT) ? 0 : double.Parse(lStrDieselT);
                    double lDblGas = string.IsNullOrEmpty(lStrGas) ? 0 : double.Parse(lStrGas);
                    double lDblKmHr = string.IsNullOrEmpty(lStrKmHr) ? 0 : double.Parse(lStrKmHr);

                    dtInitialRcords.SetValue("DslMIR", pVal.Row - 1, lDblDieselM);
                    dtInitialRcords.SetValue("DslTIR", pVal.Row - 1, lDblDieselT);
                    dtInitialRcords.SetValue("GasIR", pVal.Row - 1, lDblGas);
                    dtInitialRcords.SetValue("KmHrIR", pVal.Row - 1, lDblKmHr);

                    mtxInitialRecords.LoadFromDataSource();
                    mtxInitialRecords.AutoResizeColumns();

                    LoadConsumedTotals();
                }
            }
            catch (Exception ex)
            {
                this.UIAPIRawForm.Freeze(false);
                UIApplication.ShowError(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void txtFolio_KeyDownAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (pVal.CharPressed == 13)
            {
                if (UIApplication.GetApplication().Forms.ActiveForm.Mode == SAPbouiCOM.BoFormMode.fm_FIND_MODE && UIApplication.GetApplication().Forms.ActiveForm.UniqueID == "frmRise")
                {
                    this.UIAPIRawForm.Freeze(true);
                    StartSearchMode();
                    this.UIAPIRawForm.Freeze(false);
                    if (mObjRise != null)
                    {
                        txtFolio.Value = mObjRise.IdRise.ToString();
                    }
                }
            }
        }

        private void mtxEmployees_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (pVal.ColUID == "ColAct")
                {
                    bool lBolStatus = ((dynamic)mtxEmployees.Columns.Item("ColAct").Cells.Item(pVal.Row).Specific).Checked;
                    string lStrStatus = lBolStatus ? "Y" : "N";

                    dtEmployees.SetValue("EmpChk", pVal.Row - 1, lStrStatus);

                    mtxEmployees.LoadFromDataSource();
                    mtxEmployees.AutoResizeColumns();
                }
            }
            catch (Exception ex)
            {
                this.UIAPIRawForm.Freeze(false);
                UIApplication.ShowError(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void mtxFinalRecords_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (pVal.ColUID != "ColMacFR" || pVal.ColUID != "ColNEnFR")
                {
                    string lStrDieselM = (mtxFinalRecords.Columns.Item("ColDlMFR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStrDieselT = (mtxFinalRecords.Columns.Item("ColDlTFR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStrGas = (mtxFinalRecords.Columns.Item("ColGasFR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStr15W40 = (mtxFinalRecords.Columns.Item("Col15WFR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStrHid = (mtxFinalRecords.Columns.Item("ColHdlFR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStrSAE40 = (mtxFinalRecords.Columns.Item("ColSAEFR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStrTrans = (mtxFinalRecords.Columns.Item("ColTrmFR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStrGrasas = (mtxFinalRecords.Columns.Item("ColGrsFR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStrKmHr = (mtxFinalRecords.Columns.Item("ColKmhFR").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();

                    double lDblDieselM = string.IsNullOrEmpty(lStrDieselM) ? 0 : double.Parse(lStrDieselM);
                    double lDblDieselT = string.IsNullOrEmpty(lStrDieselT) ? 0 : double.Parse(lStrDieselT);
                    double lDblGas = string.IsNullOrEmpty(lStrGas) ? 0 : double.Parse(lStrGas);
                    double lDbl15W40 = string.IsNullOrEmpty(lStr15W40) ? 0 : double.Parse(lStr15W40);
                    double lDblHidraulic = string.IsNullOrEmpty(lStrHid) ? 0 : double.Parse(lStrHid);
                    double lDblSAE40 = string.IsNullOrEmpty(lStrSAE40) ? 0 : double.Parse(lStrSAE40);
                    double lDblTrans = string.IsNullOrEmpty(lStrTrans) ? 0 : double.Parse(lStrTrans);
                    double lDblGrasas = string.IsNullOrEmpty(lStrGrasas) ? 0 : double.Parse(lStrGrasas);
                    double lDblKmHr = string.IsNullOrEmpty(lStrKmHr) ? 0 : double.Parse(lStrKmHr);


                    dtFinalRecords.SetValue("DslMFR", pVal.Row - 1, lDblDieselM);
                    dtFinalRecords.SetValue("DslTFR", pVal.Row - 1, lDblDieselT);
                    dtFinalRecords.SetValue("GasFR", pVal.Row - 1, lDblGas);
                    dtFinalRecords.SetValue("15W40FR", pVal.Row - 1, lDbl15W40);
                    dtFinalRecords.SetValue("HidFR", pVal.Row - 1, lDblHidraulic);
                    dtFinalRecords.SetValue("SAE40FR", pVal.Row - 1, lDblSAE40);
                    dtFinalRecords.SetValue("TransFR", pVal.Row - 1, lDblTrans);
                    dtFinalRecords.SetValue("OilsFR", pVal.Row - 1, lDblGrasas);
                    dtFinalRecords.SetValue("KmHrFR", pVal.Row - 1, lDblKmHr);

                    mtxFinalRecords.LoadFromDataSource();
                    mtxFinalRecords.AutoResizeColumns();

                    LoadConsumedTotals();
                }
            }
            catch (Exception ex)
            {
                this.UIAPIRawForm.Freeze(false);
                UIApplication.ShowError(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void mtxHours_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                this.UIAPIRawForm.Freeze(true);

                SAPbouiCOM.EditText lEditTxt = null;
                string lStrDTColumn = string.Empty;

                if (pVal.ColUID == "ColHPH")
                {
                    string lStrFeetHrs = (mtxHours.Columns.Item("ColHPH").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();

                    double lDblFeetHrs = string.IsNullOrEmpty(lStrFeetHrs) ? 0 : double.Parse(lStrFeetHrs);

                    dtHours.SetValue("HrsFeetHr", pVal.Row - 1, lDblFeetHrs);

                    lEditTxt = txtFootHoursTotal;
                    lStrDTColumn = "HrsFeetHr";
                }
                else if (pVal.ColUID == "ColKmHrH")
                {
                    string lStrKmHrHct = (mtxHours.Columns.Item("ColKmHrH").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();

                    double lDblKmHrHct = string.IsNullOrEmpty(lStrKmHrHct) ? 0 : double.Parse(lStrKmHrHct);

                    dtHours.SetValue("KmHcHrs", pVal.Row - 1, lDblKmHrHct);

                    lEditTxt = txtKmHrTotal;
                    lStrDTColumn = "KmHcHrs";
                }
                else if (pVal.ColUID == "ColDateH")
                {
                    string lStrDate = (mtxHours.Columns.Item("ColDateH").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    dtHours.SetValue("DateHrs", pVal.Row - 1, DateTimeUtility.StringToDateTime(lStrDate));
                }
                else if (pVal.ColUID == "ColPendH")
                {
                    string lStrPend = (mtxHours.Columns.Item("ColPendH").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    double lDblPend = string.IsNullOrEmpty(lStrPend) ? 0 : double.Parse(lStrPend);
                    dtHours.SetValue("PendHrs", pVal.Row - 1, lDblPend);
                }
                else if (pVal.ColUID == "ColCloseH")
                {
                    bool lBolChkd = ((dynamic)mtxHours.Columns.Item("ColCloseH").Cells.Item(pVal.Row).Specific).Checked;
                    string lStrChkd = lBolChkd ? "Y" : "N";
                    dtHours.SetValue("CloseHrs", pVal.Row - 1, lStrChkd);
                }

                mtxHours.LoadFromDataSource();
                mtxHours.AutoResizeColumns();

                if (lEditTxt != null)
                {
                    if (lEditTxt.Item.UniqueID == "txtTHP")
                        CalculateTotal(lEditTxt, dtHours, "HrsFeetHr");
                    else
                        CalculateTotal(lEditTxt, dtHours, "KmHcHrs");
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - mtxHours_ValidateBefore] Error: {0}", ex.Message));
                this.UIAPIRawForm.Freeze(false);
                UIApplication.ShowError(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void mtxTransitHours_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (pVal.ColUID == "ColHrsHT")
                {
                    string lStrHrs = (mtxTransitHours.Columns.Item("ColHrsHT").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();

                    double lDblHrs = string.IsNullOrEmpty(lStrHrs) ? 0 : double.Parse(lStrHrs);

                    dtTransitHours.SetValue("HrsTH", pVal.Row - 1, lDblHrs);

                    mtxTransitHours.LoadFromDataSource();
                    mtxTransitHours.AutoResizeColumns();

                    CalculateTotal(txtTotalsHours, dtTransitHours, "HrsTH");
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - mtxTransitHours_ValidateBefore] Error: {0}", ex.Message));
                this.UIAPIRawForm.Freeze(false);
                UIApplication.ShowError(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void mtxTravelExpenses_LinkPressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ColUID == "ColFolPay")
            {
                int lIntStatus = int.Parse(dtTravelExpenses.GetValue("StsCTravE", pVal.Row - 1).ToString());
                string lStrDocEntry = dtTravelExpenses.GetValue("DocETravE", pVal.Row - 1).ToString();

                if (string.IsNullOrEmpty(lStrDocEntry))
                    return;

                if ((TravelExpStatusEnum)lIntStatus == TravelExpStatusEnum.Draft)
                {
                    UIApplication.GetApplication().OpenForm((SAPbouiCOM.BoFormObjectEnum)140, "", lStrDocEntry);
                }
                else
                {
                    UIApplication.GetApplication().OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_VendorPayment, "", lStrDocEntry);
                }
            }
        }

        private void mtxConsumables_LinkPressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ColUID == "ColFolCon")
            {
                string lStrDocEntry = dtConsumables.GetValue("FolRCons", pVal.Row - 1).ToString();

                if (string.IsNullOrEmpty(lStrDocEntry))
                    return;

                UIApplication.GetApplication().OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_StockTransfersRequest, "", lStrDocEntry);
            }
            else if (pVal.ColUID == "ColFolTra")
            {
                string lStrDocEntry = dtConsumables.GetValue("FolTCons", pVal.Row - 1).ToString();

                if (string.IsNullOrEmpty(lStrDocEntry))
                    return;

                UIApplication.GetApplication().OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_StockTransfers, "", lStrDocEntry);
            }
        }

        private void mtxContracts_LinkPressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ColUID == "ColDocEOV")
            {
                string lStrDocEntry = dtContracts.GetValue("DocECont", pVal.Row - 1).ToString();

                if (string.IsNullOrEmpty(lStrDocEntry))
                    return;

                UIApplication.GetApplication().OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_Order, "", lStrDocEntry);
            }
        }

        private void lObjColumn_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                SAPbouiCOM.ComboBox lObjColumn = null;
                if (pVal.ItemUID == "mtxHrs")
                {
                    lObjColumn = (mtxHours.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific as SAPbouiCOM.ComboBox);
                }
                else if (pVal.ItemUID == "mtxHrsTra")
                {
                    lObjColumn = (mtxTransitHours.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific as SAPbouiCOM.ComboBox);
                }

                //lObjColumn.ComboSelectAfter += lObjColumn_ComboSelectAfter;

                if (lObjColumn.Selected == null)
                {
                    return;
                }

                string lStrCode = lObjColumn.Selected.Value;
                string lStrDesc = lObjColumn.Selected.Description;

                switch (pVal.ColUID)
                {
                    //mtxHrs
                    case "ColCOVH":
                        dtHours.SetValue("ContOVHrs", pVal.Row - 1, lStrCode);
                        dtHours.SetValue("CDEOVHrs", pVal.Row - 1, lStrDesc);

                        string lStrClient = mObjMachineryServiceFactory.GetContractsService().
                                                DataTableToDTO(dtContracts)
                                                .Where(x => x.DocEntry.ToString() == lStrCode)
                                                .FirstOrDefault()
                                                .CardName;

                        dtHours.SetValue("Client", pVal.Row - 1, lStrClient);

                        AddTramosCbo(int.Parse(lStrCode), pVal.Row);
                        break;
                    case "ColSupH":
                        dtHours.SetValue("SupIdHrs", pVal.Row - 1, lStrCode);
                        dtHours.SetValue("SupNmHrs", pVal.Row - 1, lStrDesc);
                        break;
                    case "ColOprH":
                        dtHours.SetValue("OpdIdHrs", pVal.Row - 1, lStrCode);
                        dtHours.SetValue("OpdNmHrs", pVal.Row - 1, lStrDesc);
                        break;
                    case "ColEqmH":
                        dtHours.SetValue("EqpHrs", pVal.Row - 1, lStrCode);
                        dtHours.SetValue("NumEcnHrs", pVal.Row - 1, lStrDesc);
                        break;
                    case "ColNumEH":
                        dtHours.SetValue("NumEcnHrs", pVal.Row - 1, lStrCode);
                        dtHours.SetValue("EqpHrs", pVal.Row - 1, lStrDesc);
                        break;
                    case "ColTramoH":
                        dtHours.SetValue("SctnIdHrs", pVal.Row - 1, lStrCode);
                        dtHours.SetValue("SctnNnHrs", pVal.Row - 1, lStrDesc);
                        break;

                    //mtxHrsTra
                    case "ColMaqHT":
                        dtTransitHours.SetValue("MaqTHrs", pVal.Row - 1, lStrCode);
                        dtTransitHours.SetValue("NumEcoTH", pVal.Row - 1, lStrDesc);
                        break;
                    case "ColNEnHT":
                        dtTransitHours.SetValue("MaqTHrs", pVal.Row - 1, lStrCode);
                        dtTransitHours.SetValue("NumEcoTH", pVal.Row - 1, lStrDesc);
                        break;
                    case "ColOpdHT":
                        dtTransitHours.SetValue("OpdTH", pVal.Row - 1, lStrCode);
                        dtTransitHours.SetValue("OpdNmTH", pVal.Row - 1, lStrDesc);
                        break;

                    case "ColSupHT":
                        dtTransitHours.SetValue("SupTH", pVal.Row - 1, lStrCode);
                        dtTransitHours.SetValue("SupNmTH", pVal.Row - 1, lStrDesc);
                        break;
                    default:
                        break;
                }

                if (pVal.ItemUID == "mtxHrs")
                {
                    mtxHours.LoadFromDataSource();
                    mtxHours.SelectRow(pVal.Row, true, false);
                }
                else if (pVal.ItemUID == "mtxHrsTra")
                {
                    mtxTransitHours.LoadFromDataSource();
                    mtxTransitHours.SelectRow(pVal.Row, true, false);
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - lObjColumn_ComboSelectAfter] Error: {0}", ex.Message));
                UIApplication.ShowError(string.Format("lObjColumn_ComboSelectAfter: {0}", ex.Message));
                //QsLog.WriteError("frmPurchaseXML (lObjColumn_ComboSelectAfter): " + ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void lObjColumn_ComboClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                this.UIAPIRawForm.Freeze(true);

                SAPbouiCOM.ComboBox lObjColumn = null; //cboContracts
                if (pVal.ItemUID == "mtxHrs")
                {
                    lObjColumn = (mtxHours.Columns.Item("ColCOVH").Cells.Item(pVal.Row).Specific as SAPbouiCOM.ComboBox);
                }

                string lStrCode = lObjColumn.Selected == null ? string.Empty : lObjColumn.Selected.Value;
                string lStrDesc = lObjColumn.Selected == null ? string.Empty : lObjColumn.Selected.Description;

                SAPbouiCOM.ComboBox lObjComboBox = (SAPbouiCOM.ComboBox)mtxHours.Columns.Item("ColTramoH").Cells.Item(pVal.Row).Specific;
                if (!string.IsNullOrEmpty(lStrCode))
                {
                    //lObjComboBox.ComboSelectAfter += lObjColumn_ComboSelectAfter;
                    CleanCboItems(lObjComboBox);
                    if (lObjComboBox.ValidValues.Count <= 0)
                    {
                        List<SectionsDTO> lLstTramos = mObjMachineryServiceFactory.GetSectionsService().GetSectionsBySalesOrder(int.Parse(lStrCode));
                        foreach (var lObjTramo in lLstTramos)
                        {
                            lObjComboBox.ValidValues.Add(lObjTramo.Code.ToString(), lObjTramo.Name);
                        }

                        //lObjComboBox.Item.DisplayDesc = true;
                    }
                }
                else
                {
                    CleanCboItems(lObjComboBox);
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - lObjColumn_ComboClickBefore] Error: {0}", ex.Message));
                UIApplication.ShowError(string.Format("lObjColumn_ComboSelectAfter: {0}", ex.Message));
                //QsLog.WriteError("frmPurchaseXML (lObjColumn_ComboSelectAfter): " + ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        #endregion

        #region Functions
        #region Header
        public void LoadInitialsControls()
        {
            try
            {
                //this.UIAPIRawForm.Freeze(true);

                string lStrUsername = UIApplication.GetCompany().UserName;

                txtFolio.Value = mObjMachineryServiceFactory.GetRiseService().GetNexFolioId().ToString();
                txtDate.Value = DateTime.Now.ToString("dd/MM/yyyy");
                txtStatus.Value = RiseStatusEnum.Active.GetDescription();

                LoadChoosesFromList();
                CreateDatatableEmployees();
                CreateDatatableConsumables();
                CreateContractsDatatable();
                CreateTravelExpensesDatatable();
                CreateInitialRecordsDataTable();
                CreatePurchInvReqDataTable();
                CreateFinalRecordsDataTable();
                CreateTotalsRecordsDataTable();
                CreateHoursDataTable();
                CreateTransitHoursDataTable();
                CreateMachineryPerformanceDataTable();
                CreateVehiclePerformanceDataTable();
                SetCFLToTxt();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - LoadInitialsControls] Error: {0}", lObjException.Message));
                Application.SBO_Application.SetStatusBarMessage(lObjException.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                //this.UIAPIRawForm.Freeze(false);
            }
        }

        public void LoadRiseRelControls()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                RiseDTO lObjRelRise = mObjMachineryServiceFactory.GetRiseService().GetRiseById(int.Parse(txtFolioRelation.Value));
                if (lObjRelRise == null)
                {
                    UIApplication.ShowError("La subida no existe");
                    return;
                }

                LoadRiseOperators(int.Parse(txtFolioRelation.Value), true);
                LoadRiseContracts(int.Parse(txtFolioRelation.Value), true);

                mStrClientCode = lObjRelRise.Client;
                this.UIAPIRawForm.DataSources.UserDataSources.Item("CFL_0").ValueEx = lObjRelRise.ClientName;

                mStrSupervisorCode = lObjRelRise.SupervisorId.ToString();
                this.UIAPIRawForm.DataSources.UserDataSources.Item("CFL_Supv").ValueEx = lObjRelRise.SupervisorName;

                SaveRise();

                LoadRiseRelInitialRecords();

                for (int i = 0; i < 30; i++)
                {
                    AddHoursRecord(new HoursRecordsDTO());
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - LoadRiseRelControls] Error: {0}", lObjException.Message));
                Application.SBO_Application.MessageBox(lObjException.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void LoadRiseOperators(int pIntFolio, bool pBolisForRelatedRise = false)
        {
            try
            {
                var lLstOperators = mObjMachineryServiceFactory.GetEmployeesService().GetEmployeesByRiseId(pIntFolio);

                if (pBolisForRelatedRise)
                {
                    lLstOperators = lLstOperators.Select(c => { c.Code = string.Empty; return c; }).ToList();
                }

                foreach (var lObjEmployee in lLstOperators)
                {
                    AddEmployee(lObjEmployee);
                }

                dtEmployees.Rows.Add();
                dtEmployees.SetValue("#", dtEmployees.Rows.Count - 1, dtEmployees.Rows.Count);

                mtxEmployees.LoadFromDataSource();
                mtxEmployees.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al obtener los operadores de la subida: {0}", lObjException.Message));
            }
        }

        public void LoadRiseContracts(int pIntFolio, bool pBolisForRelatedRise = false)
        {
            try
            {
                var lLstContracts = mObjMachineryServiceFactory.GetContractsService().GetContractsByRiseId(pIntFolio);

                if (pBolisForRelatedRise)
                {
                    lLstContracts = lLstContracts.Select(c => { c.Code = string.Empty; return c; }).ToList();
                }

                foreach (var lObjContract in lLstContracts)
                {
                    if (pBolisForRelatedRise)
                    {
                        if (!mObjMachineryServiceFactory.GetContractsService().IsClosed(lObjContract.DocEntry))
                        {
                            AddContract(lObjContract);
                        }
                    }
                    else
                    {
                        AddContract(lObjContract);
                    }
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al obtener los contratos de la subida: {0}", lObjException.Message));
            }
        }

        public void LoadRiseConsumablesRequest(int pIntFolio, RiseStatusEnum pEnmRiseStatus = RiseStatusEnum.Active)
        {
            try
            {
                List<ConsumablesDTO> lLstConsumableReq = new List<ConsumablesDTO>();
                if (pEnmRiseStatus == RiseStatusEnum.Active)
                {
                    lLstConsumableReq = mObjMachineryServiceFactory.GetConsumablesService().GetConsumableRequestDocByRiseId(pIntFolio);
                }
                else
                {
                    lLstConsumableReq = mObjMachineryServiceFactory.GetConsumablesService().GetConsumableRequestUDTByRiseId(pIntFolio);
                }

                foreach (var lObjConsumable in lLstConsumableReq)
                {
                    AddConsumable(lObjConsumable);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al obtener las solicitudes de consumibles de la subida: {0}", lObjException.Message));
            }
        }

        public void LoadRiseTravelExpenses(int pIntFolio, RiseStatusEnum pEnmRiseStatus = RiseStatusEnum.Active)
        {
            try
            {

                List<TravelExpensesDTO> lLstTravelExpenses = new List<TravelExpensesDTO>();
                if (pEnmRiseStatus == RiseStatusEnum.Active)
                {
                    //lLstTravelExpenses = mObjMachineryServiceFactory.GetTravelExpensesService().GetDraftPaymentsByRiseId(pIntFolio, "frmTExp");
                    lLstTravelExpenses = mObjMachineryServiceFactory.GetTravelExpensesService().GetPaymentsByRiseId(pIntFolio, "frmTExp");
                }
                else
                {
                    //lLstTravelExpenses = mObjMachineryServiceFactory.GetTravelExpensesService().GetTravelExpensesByRiseId(pIntFolio, true);
                    lLstTravelExpenses = mObjMachineryServiceFactory.GetTravelExpensesService().GetTravelExpensesByRiseId(pIntFolio);
                }

                foreach (var lObjTravelExp in lLstTravelExpenses)
                {
                    AddTravelExpenses(lObjTravelExp);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al obtener las solicitudes de viáticos de la subida: {0}", lObjException.Message));
            }
        }

        public void LoadRiseRelInitialRecords()
        {
            try
            {

                /*var lLstFinalRecord = mObjMachineryServiceFactory
                                    .GetFinalsRecordsService()
                                    .GetByRelatedRiseId(int.Parse(txtFolioRelation.Value));*/

                var lLstFinalRecord = mObjMachineryServiceFactory
                                         .GetConsumablesService()
                                         .GetInitialRecordsByRiseId(int.Parse(txtFolio.Value)).Select(c => { c.Code = string.Empty; return c; }).ToList();
                //mObjMachineryServiceFactory.GetConsumablesService().GetFinalsRecordsUDTByRiseId(int.Parse(txtFolioRelation.Value)).Select(c => { c.Code = string.Empty; return c; }).ToList();

                foreach (var lObjContract in lLstFinalRecord)
                {
                    AddInitialRecord(lObjContract);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al obtener los registros iniciales de la subida relacionada: {0}", lObjException.Message));
            }
        }

        private static string GetDocEntry(string pStrObjectKey)
        {
            string lStrDocEntry = string.Empty;
            XmlDocument lObjDocument = new XmlDocument();
            lObjDocument.LoadXml(pStrObjectKey);

            foreach (XmlElement item in lObjDocument.ChildNodes.Item(1).ChildNodes)
            {
                lStrDocEntry = item.InnerText;
            }

            return lStrDocEntry;
        }

        #region Buttons
        public void SaveRise()
        {
            try
            {
                if (!ValidateRiseControls())
                {
                    /*if (!mObjMachinerySeviceFactory.GetRiseService().ExistsRise(int.Parse(txtFolio.Value)))
                    {
                        Application.SBO_Application.SetStatusBarMessage("La cabecera de la subida aún no existe, favor de guardarla para continuar", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                        return;
                    }*/

                    this.UIAPIRawForm.Freeze(true);

                    if (mObjRise == null)
                    {
                        mObjMachineryServiceFactory.GetRiseService().Add(GetRiseObject());
                        mObjRise.RowCode = mObjMachineryServiceFactory.GetRiseService().GetCode(mObjRise.IdRise);

                        //Application.SBO_Application.SetStatusBarMessage("Se agregó correctamente la subida", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                    }
                    else
                    {
                        mObjMachineryServiceFactory.GetRiseService().Update(mObjRise);

                        //Application.SBO_Application.SetStatusBarMessage("Se modificó correctamente la subida", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                    }

                    SaveEmployees();
                    SaveContracts();
                    RemoveEmployees(ref mLstEmployeesToDelete);
                    RemoveContracts(ref mLstContractsToDelete);
                    SaveHours();
                    SaveTransitHours();

                    Application.SBO_Application.SetStatusBarMessage("Se guardó correctamente la subida", SAPbouiCOM.BoMessageTime.bmt_Short, false);

                    //Hasta que se cierra subida se guardan datos en UDT
                    /*SaveConsumables(); 
                    SaveTravelExpenses();
                    SaveInitialsRecords();
                    SavePurchasesInvReqRecords();
                    SaveFinalsRecords();*/
                }
                else
                {
                    UIApplication.ShowError("Verificar campos vacíos");
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - SaveRise] Error al guardar subida: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al guardar subida: {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void SaveEmployees()
        {
            try
            {
                for (int i = 0; i < dtEmployees.Rows.Count - 1; i++)
                {
                    //bool lBolStatus = ((dynamic)mtxEmployees.Columns.Item(3).Cells.Item(i + 1).Specific).Checked;
                    int lIntStatus = dtEmployees.GetValue("EmpChk", i).ToString() == "Y" ? 1 : 0;
                    string lStrCode = dtEmployees.GetValue("CodeTEmp", i).ToString();
                    Employees lObjEmployees = new Employees
                    {
                        RowCode = lStrCode,
                        Employee = int.Parse(dtEmployees.GetValue(1, i).ToString()),
                        IdRise = mObjRise.IdRise,
                        Status = lIntStatus,
                    };

                    if (string.IsNullOrEmpty(lStrCode))
                    {
                        mObjMachineryServiceFactory.GetEmployeesService().Add(lObjEmployees);
                        dtEmployees.SetValue("CodeTEmp", i, mObjMachineryServiceFactory.GetEmployeesService().GetLastCode());
                    }
                    else
                    {
                        mObjMachineryServiceFactory.GetEmployeesService().Update(lObjEmployees);
                    }
                }

                UIApplication.GetApplication().SetStatusBarMessage("Se guardaron correctamente los empleados", SAPbouiCOM.BoMessageTime.bmt_Short, false);
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al guardar los operadores: {0}", lObjException.Message));
            }
        }

        private void SaveContracts()
        {
            try
            {
                for (int i = 0; i < dtContracts.Rows.Count; i++)
                {
                    string lStrCode = dtContracts.GetValue("CodeTCont", i).ToString();
                    Contracts lObjContracts = new Contracts
                    {
                        RowCode = lStrCode,
                        DocEntry = int.Parse(dtContracts.GetValue("DocECont", i).ToString()),
                        IdRise = mObjRise.IdRise,
                    };

                    if (string.IsNullOrEmpty(lStrCode))
                    {
                        mObjMachineryServiceFactory.GetContractsService().Add(lObjContracts);
                        dtContracts.SetValue("CodeTCont", i, mObjMachineryServiceFactory.GetContractsService().GetLastCode());
                    }
                    /*else
                    {
                        mObjMachinerySeviceFactory.GetContractsService().Update(lObjContracts);
                    }*/
                }

                UIApplication.GetApplication().SetStatusBarMessage("Se guardaron correctamente los contratos", SAPbouiCOM.BoMessageTime.bmt_Short, false);
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al guardar los contratos: {0}", lObjException.Message));
            }
        }

        private void SaveConsumables()
        {
            try
            {
                for (int i = 0; i < dtConsumables.Rows.Count; i++)
                {
                    string lStrCode = dtConsumables.GetValue(1, i).ToString();
                    Consumables lObjConsumables = new Consumables
                    {
                        RowCode = lStrCode,
                        DocEntry = int.Parse(dtConsumables.GetValue(3, i).ToString()),
                        IdRise = mObjRise.IdRise,
                    };

                    if (string.IsNullOrEmpty(lStrCode))
                    {
                        mObjMachineryServiceFactory.GetConsumablesService().Add(lObjConsumables);
                        dtConsumables.SetValue("CodeTCons", i, mObjMachineryServiceFactory.GetConsumablesService().GetLastCode());
                    }
                    /*else
                    {
                        mObjMachinerySeviceFactory.GetContractsService().Update(lObjContracts);
                    }*/

                    UIApplication.GetApplication().SetStatusBarMessage("Se guardaron correctamente las solicitudes de consumibles", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al guardar las solicitudes de consumibles: {0}", lObjException.Message));
            }
        }

        private void SaveTravelExpenses()
        {
            try
            {
                for (int i = 0; i < dtTravelExpenses.Rows.Count; i++)
                {
                    string lStrCode = dtTravelExpenses.GetValue("CodeTTrav", i).ToString();
                    TravelExpenses lObjTravelExpenses = new TravelExpenses
                    {
                        RowCode = lStrCode,
                        DocEntry = int.Parse(dtTravelExpenses.GetValue(3, i).ToString()),
                        IdRise = mObjRise.IdRise,
                    };

                    if (string.IsNullOrEmpty(lStrCode) || lStrCode.Equals("0"))
                    {
                        mObjMachineryServiceFactory.GetTravelExpensesService().Add(lObjTravelExpenses);
                        dtTravelExpenses.SetValue("CodeTTrav", i, mObjMachineryServiceFactory.GetTravelExpensesService().GetLastCode());
                    }
                    /*else
                    {
                        mObjMachinerySeviceFactory.GetContractsService().Update(lObjContracts);
                    }*/

                    UIApplication.GetApplication().SetStatusBarMessage("Se guardaron correctamente las solicitudes de viáticos", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al guardar las solicitudes de viáticos: {0}", lObjException.Message));
            }
        }

        private void SaveInitialsRecords()
        {
            try
            {
                if (dtInitialRcords.Rows.Count <= 0)
                    return;

                for (int i = 0; i < dtInitialRcords.Rows.Count; i++)
                {
                    string lStrCode = dtInitialRcords.GetValue("CodeTIR", i).ToString();
                    InitialRecords lObjInitialRecord = new InitialRecords
                    {
                        RowCode = lStrCode,
                        IdRise = mObjRise.IdRise,
                        PrcCode = dtInitialRcords.GetValue("ActCodIR", i).ToString(),
                        EcoNum = dtInitialRcords.GetValue("ActNumIR", i).ToString(),
                        DieselM = double.Parse(dtInitialRcords.GetValue("DslMIR", i).ToString()),
                        DieselT = double.Parse(dtInitialRcords.GetValue("DslTIR", i).ToString()),
                        Gas = double.Parse(dtInitialRcords.GetValue("GasIR", i).ToString()),
                        F15W40 = double.Parse(dtInitialRcords.GetValue("15W40IR", i).ToString()),
                        Hidraulic = double.Parse(dtInitialRcords.GetValue("HidIR", i).ToString()),
                        SAE40 = double.Parse(dtInitialRcords.GetValue("SAE40IR", i).ToString()),
                        Transmition = double.Parse(dtInitialRcords.GetValue("TransIR", i).ToString()),
                        Oils = double.Parse(dtInitialRcords.GetValue("OilsIR", i).ToString()),
                        KmHr = double.Parse(dtInitialRcords.GetValue("KmHrIR", i).ToString()),
                    };

                    if (string.IsNullOrEmpty(lStrCode))
                    {
                        mObjMachineryServiceFactory.GetInitialRecordsService().Add(lObjInitialRecord);
                        dtInitialRcords.SetValue("CodeTIR", i, mObjMachineryServiceFactory.GetInitialRecordsService().GetLastCode());
                    }
                    else
                    {
                        mObjMachineryServiceFactory.GetInitialRecordsService().Update(lObjInitialRecord);
                    }

                    UIApplication.GetApplication().SetStatusBarMessage("Se guardaron correctamente los registros iniciales", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al guardar los registros iniciales: {0}", lObjException.Message));
            }
        }

        private void SavePurchasesInvReqRecords()
        {
            try
            {
                for (int i = 0; i < dtPurchase.Rows.Count; i++)
                {
                    string lStrCode = dtPurchase.GetValue("CodeTPrch", i).ToString();
                    PurchaseOrders lObjInitialRecord = new PurchaseOrders
                    {
                        RowCode = lStrCode,
                        IdRise = mObjRise.IdRise,
                        Type = int.Parse(dtPurchase.GetValue("DocTyPrch", i).ToString()),
                        PrcCode = dtPurchase.GetValue("ActCodPrc", i).ToString(),
                        EcoNum = dtPurchase.GetValue("ActNumPrc", i).ToString(),
                        DieselM = 0,
                        DieselT = double.Parse(dtPurchase.GetValue("DslTPrch", i).ToString()),
                        Gas = double.Parse(dtPurchase.GetValue("GasPrch", i).ToString()),
                        F15W40 = double.Parse(dtPurchase.GetValue("15W40Prch", i).ToString()),
                        Hidraulic = double.Parse(dtPurchase.GetValue("HidPrch", i).ToString()),
                        SAE40 = double.Parse(dtPurchase.GetValue("SAE40Prch", i).ToString()),
                        Transmition = double.Parse(dtPurchase.GetValue("TransPrch", i).ToString()),
                        Oils = double.Parse(dtPurchase.GetValue("OilsPrch", i).ToString()),
                    };

                    if (string.IsNullOrEmpty(lStrCode))
                    {
                        mObjMachineryServiceFactory.GetPurchasesOrdersService().Add(lObjInitialRecord);
                        dtPurchase.SetValue("CodeTPrch", i, mObjMachineryServiceFactory.GetPurchasesOrdersService().GetLastCode());
                    }
                    else
                    {
                        mObjMachineryServiceFactory.GetPurchasesOrdersService().Update(lObjInitialRecord);
                    }

                    UIApplication.GetApplication().SetStatusBarMessage("Se guardaron correctamente los registros de compras y traspasos", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al guardar los registros de compras y traspasos: {0}", lObjException.Message));
            }
        }

        private void SaveFinalsRecords()
        {
            try
            {
                for (int i = 0; i < dtFinalRecords.Rows.Count; i++)
                {
                    string lStrCode = dtFinalRecords.GetValue("CodeTFR", i).ToString();
                    FinalsRecords lObjFinalRecord = new FinalsRecords
                    {
                        RowCode = lStrCode,
                        IdRise = mObjRise.IdRise,
                        PrcCode = dtFinalRecords.GetValue("ActCodFR", i).ToString(),
                        EcoNum = dtFinalRecords.GetValue("ActNumFR", i).ToString(),
                        DieselM = double.Parse(dtFinalRecords.GetValue("DslMFR", i).ToString()),
                        DieselT = double.Parse(dtFinalRecords.GetValue("DslTFR", i).ToString()),
                        Gas = double.Parse(dtFinalRecords.GetValue("GasFR", i).ToString()),
                        F15W40 = double.Parse(dtFinalRecords.GetValue("15W40FR", i).ToString()),
                        Hidraulic = double.Parse(dtFinalRecords.GetValue("HidFR", i).ToString()),
                        SAE40 = double.Parse(dtFinalRecords.GetValue("SAE40FR", i).ToString()),
                        Transmition = double.Parse(dtFinalRecords.GetValue("TransFR", i).ToString()),
                        Oils = double.Parse(dtFinalRecords.GetValue("OilsFR", i).ToString()),
                        KmHr = double.Parse(dtFinalRecords.GetValue("KmHrFR", i).ToString()),
                    };

                    if (string.IsNullOrEmpty(lStrCode))
                    {
                        mObjMachineryServiceFactory.GetFinalsRecordsService().Add(lObjFinalRecord);
                        dtFinalRecords.SetValue("CodeTFR", i, mObjMachineryServiceFactory.GetFinalsRecordsService().GetLastCode());
                    }
                    else
                    {
                        mObjMachineryServiceFactory.GetFinalsRecordsService().Update(lObjFinalRecord);
                    }

                    UIApplication.GetApplication().SetStatusBarMessage("Se guardaron correctamente los registros finales", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al guardar los registros finales: {0}", lObjException.Message));
            }
        }

        private void SaveTotalsRecords()
        {
            try
            {
                mObjMachineryServiceFactory.GetTotalsRecordsService().RemoveByRiseId(mObjRise.IdRise);

                for (int i = 0; i < dtConsumedTotals.Rows.Count; i++)
                {
                    string lStrCode = dtConsumedTotals.GetValue("CodeTTR", i).ToString();
                    TotalRecords lObjTotalRecords = new TotalRecords
                    {
                        RowCode = lStrCode,
                        IdRise = mObjRise.IdRise,
                        PrcCode = dtConsumedTotals.GetValue("ActCodTR", i).ToString(),
                        EcoNum = dtConsumedTotals.GetValue("ActNumTR", i).ToString(),
                        DieselM = double.Parse(dtConsumedTotals.GetValue("DslMTR", i).ToString()),
                        DieselT = double.Parse(dtConsumedTotals.GetValue("DslTTR", i).ToString()),
                        Gas = double.Parse(dtConsumedTotals.GetValue("GasTR", i).ToString()),
                        F15W40 = double.Parse(dtConsumedTotals.GetValue("15W40TR", i).ToString()),
                        Hidraulic = double.Parse(dtConsumedTotals.GetValue("HidTR", i).ToString()),
                        SAE40 = double.Parse(dtConsumedTotals.GetValue("SAE40TR", i).ToString()),
                        Transmition = double.Parse(dtConsumedTotals.GetValue("TransTR", i).ToString()),
                        Oils = double.Parse(dtConsumedTotals.GetValue("OilsTR", i).ToString()),
                        KmHr = double.Parse(dtConsumedTotals.GetValue("KmHrTR", i).ToString()),
                    };

                    if (string.IsNullOrEmpty(lStrCode))
                    {
                        mObjMachineryServiceFactory.GetTotalsRecordsService().Add(lObjTotalRecords);
                        dtConsumedTotals.SetValue("CodeTTR", i, mObjMachineryServiceFactory.GetTotalsRecordsService().GetLastCode());
                    }
                    else
                    {
                        mObjMachineryServiceFactory.GetTotalsRecordsService().Update(lObjTotalRecords);
                    }
                }

                UIApplication.GetApplication().SetStatusBarMessage("Se guardaron correctamente los registros totales", SAPbouiCOM.BoMessageTime.bmt_Short, false);
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al guardar los registros totales: {0}", lObjException.Message));
            }
        }

        private void DeleteTotalsRecords()
        {
            try
            {
                mObjMachineryServiceFactory.GetTotalsRecordsService().Add(new TotalRecords());
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al eliminar los registros totales: {0}", lObjException.Message));
            }
        }

        private void SaveHours()
        {
            try
            {
                for (int i = 0; i < dtHours.Rows.Count; i++)
                {
                    //int lIntContract = int.Parse(dtHours.GetValue("SupIdHrs", i).ToString());
                    int lIntSupId = int.Parse(dtHours.GetValue("SupIdHrs", i).ToString());
                    int lIntOpdId = int.Parse(dtHours.GetValue("OpdIdHrs", i).ToString());
                    string lStrEqmId = dtHours.GetValue("EqpHrs", i).ToString();
                    string lStrEcoNum = dtHours.GetValue("NumEcnHrs", i).ToString();
                    double lDblFeetHrs = double.Parse(dtHours.GetValue("HrsFeetHr", i).ToString());
                    string lStrDate = dtHours.GetValue("DateHrs", i).ToString();

                    if (lIntSupId > 0 && lIntOpdId > 0 && !string.IsNullOrEmpty(lStrEqmId) && !string.IsNullOrEmpty(lStrEcoNum) && lDblFeetHrs > 0 && !string.IsNullOrEmpty(lStrDate))
                    {
                        int lIntStatus = dtHours.GetValue("CloseHrs", i).ToString() == "Y" ? 1 : 0;
                        string lStrCode = dtHours.GetValue("CodeHrs", i).ToString();
                        HoursRecords lObjHoursRecords = new HoursRecords
                        {
                            RowCode = lStrCode,
                            IdRise = mObjRise.IdRise, //62
                            DocEntry = int.Parse(dtHours.GetValue("ContOVHrs", i).ToString()),
                            DateHour = DateTime.Parse(lStrDate),
                            Supervisor = lIntSupId,
                            Operator = lIntOpdId,
                            PrcCode = lStrEqmId,
                            EcoNum = lStrEcoNum,
                            HrFeet = lDblFeetHrs,
                            Section = int.Parse(dtHours.GetValue("SctnIdHrs", i).ToString()),
                            KmHt = double.Parse(dtHours.GetValue("KmHcHrs", i).ToString()),
                            Pending = double.Parse(dtHours.GetValue("PendHrs", i).ToString()),
                            Close = lIntStatus,
                        };

                        if (string.IsNullOrEmpty(lStrCode))
                        {
                            mObjMachineryServiceFactory.GetHoursRecordsService().Add(lObjHoursRecords);
                            dtHours.SetValue("CodeHrs", i, mObjMachineryServiceFactory.GetHoursRecordsService().GetLastCode());
                        }
                        else
                        {
                            mObjMachineryServiceFactory.GetHoursRecordsService().Update(lObjHoursRecords);
                        }
                    }
                }

                UIApplication.GetApplication().SetStatusBarMessage("Se guardaron correctamente los registros de horas", SAPbouiCOM.BoMessageTime.bmt_Short, false);
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al guardar los registros de horas: {0}", lObjException.Message));
            }
        }

        private void SaveTransitHours()
        {
            try
            {
                for (int i = 0; i < dtTransitHours.Rows.Count; i++)
                {
                    string lStrEqmId = dtTransitHours.GetValue("MaqTHrs", i).ToString();
                    string lStrEcoNum = dtTransitHours.GetValue("NumEcoTH", i).ToString();
                    double lDblHrs = double.Parse(dtTransitHours.GetValue("HrsTH", i).ToString());
                    int lIntOpdId = int.Parse(dtTransitHours.GetValue("OpdTH", i).ToString());
                    int lIntSupId = int.Parse(dtTransitHours.GetValue("SupTH", i).ToString());

                    if (string.IsNullOrEmpty(lStrEqmId) || string.IsNullOrEmpty(lStrEcoNum) || lDblHrs <= 0 || lIntOpdId <= 0)
                    {
                        UIApplication.ShowError("Verificar campos vacíos en horas tránsito");
                        return;
                    }

                    string lStrCode = dtTransitHours.GetValue("CodeTH", i).ToString();
                    TransitHoursRecords lObjTransitHours = new TransitHoursRecords
                    {
                        RowCode = lStrCode,
                        IdRise = mObjRise.IdRise,//62
                        PrcCode = lStrEqmId,
                        EcoNum = lStrEcoNum,
                        Hrs = lDblHrs,
                        Operator = lIntOpdId,
                        Supervisor = lIntSupId
                    };

                    if (string.IsNullOrEmpty(lStrCode))
                    {
                        mObjMachineryServiceFactory.GetTransitHoursRecordsService().Add(lObjTransitHours);
                        dtTransitHours.SetValue("CodeTH", i, mObjMachineryServiceFactory.GetTransitHoursRecordsService().GetLastCode());
                    }
                    else
                    {
                        mObjMachineryServiceFactory.GetTransitHoursRecordsService().Update(lObjTransitHours);
                    }
                }

                UIApplication.GetApplication().SetStatusBarMessage("Se guardaron correctamente los registros de tránsito de horas", SAPbouiCOM.BoMessageTime.bmt_Short, false);
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al guardar los registros de tránsito de horas: {0}", lObjException.Message));
            }
        }

        private void SavePerformance()
        {
            try
            {
                for (int i = 0; i < dtMachineryPerformance.Rows.Count; i++)
                {
                    string lStrCode = dtMachineryPerformance.GetValue("CodePFM", i).ToString();
                    Performance lObjPerformance = new Performance
                    {
                        RowCode = lStrCode,
                        IdRise = mObjRise.IdRise,
                        PrcCode = dtMachineryPerformance.GetValue("MaqPFM", i).ToString(),
                        EcoNum = dtMachineryPerformance.GetValue("NumEcoPFM", i).ToString(),
                        Type = int.Parse(dtMachineryPerformance.GetValue("TypeIdPFM", i).ToString()),
                        HrKm = double.Parse(dtMachineryPerformance.GetValue("HrsKmPFM", i).ToString()),
                        PerformanceF = double.Parse(dtMachineryPerformance.GetValue("PerfPFM", i).ToString()),
                    };

                    if (string.IsNullOrEmpty(lStrCode))
                    {
                        mObjMachineryServiceFactory.GetPerformanceService().Add(lObjPerformance);
                        dtMachineryPerformance.SetValue("CodePFM", i, mObjMachineryServiceFactory.GetPerformanceService().GetLastCode());
                    }
                }

                for (int i = 0; i < dtVehiclePerformance.Rows.Count; i++)
                {
                    string lStrCode = dtVehiclePerformance.GetValue("CodePFV", i).ToString();
                    Performance lObjPerformance = new Performance
                    {
                        RowCode = lStrCode,
                        IdRise = mObjRise.IdRise,
                        PrcCode = dtVehiclePerformance.GetValue("MaqPFV", i).ToString(),
                        EcoNum = dtVehiclePerformance.GetValue("NumEcoPFV", i).ToString(),
                        Type = int.Parse(dtVehiclePerformance.GetValue("TypeIdPFV", i).ToString()),
                        HrKm = double.Parse(dtVehiclePerformance.GetValue("PerfPFV", i).ToString()),
                        PerformanceF = double.Parse(dtVehiclePerformance.GetValue("PerfPFV", i).ToString()),
                    };

                    if (string.IsNullOrEmpty(lStrCode))
                    {
                        mObjMachineryServiceFactory.GetPerformanceService().Add(lObjPerformance);
                        dtVehiclePerformance.SetValue("CodePFV", i, mObjMachineryServiceFactory.GetPerformanceService().GetLastCode());
                    }
                }

                /*
                 * dtVehiclePerformance.SetValue("#", dtVehiclePerformance.Rows.Count - 1, this.UIAPIRawForm.DataSources.DataTables.Item("DTVPerformance").IsEmpty ? 1 : dtVehiclePerformance.Rows.Count + 1);
                    dtVehiclePerformance.SetValue("IdRisePFV", dtVehiclePerformance.Rows.Count - 1, mObjRise.IdRise);
                    dtVehiclePerformance.SetValue("MaqPFV", dtVehiclePerformance.Rows.Count - 1, pObjPerformanceRecord.PrcCode);
                    dtVehiclePerformance.SetValue("NumEcoPFV", dtVehiclePerformance.Rows.Count - 1, pObjPerformanceRecord.EcoNum);
                    dtVehiclePerformance.SetValue("TypeIdPFV", dtVehiclePerformance.Rows.Count - 1, pObjPerformanceRecord.Type);
                    dtVehiclePerformance.SetValue("HrsKmPFV", dtVehiclePerformance.Rows.Count - 1, pObjPerformanceRecord.HrKm);
                    dtVehiclePerformance.SetValue("PerfPFV", dtVehiclePerformance.Rows.Count - 1, pObjPerformanceRecord.PerformanceF);
                    dtVehiclePerformance.SetValue("CodePFV", dtVehiclePerformance.Rows.Count - 1, pObjPerformanceRecord.Code);
                 */

                UIApplication.GetApplication().SetStatusBarMessage("Se guardaron correctamente los registros de rendimientos", SAPbouiCOM.BoMessageTime.bmt_Short, false);
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al guardar los registros de rendimientos: {0}", lObjException.Message));
            }
        }

        private void RemoveEmployees(ref List<string> pLstEmployeesCodes)
        {
            try
            {
                foreach (var pStrCode in pLstEmployeesCodes)
                {
                    mObjMachineryServiceFactory.GetEmployeesService().Remove(pStrCode);
                }

                pLstEmployeesCodes.Clear();

                UIApplication.GetApplication().SetStatusBarMessage("Se eliminó correctamente el operador", SAPbouiCOM.BoMessageTime.bmt_Short, false);
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al eliminar el operador: {0}", lObjException.Message));
            }
        }

        private void RemoveContracts(ref List<string> pLstContractsCodes)
        {
            try
            {
                foreach (var pStrCode in pLstContractsCodes)
                {
                    mObjMachineryServiceFactory.GetContractsService().Remove(pStrCode);
                }

                pLstContractsCodes.Clear();

                UIApplication.GetApplication().SetStatusBarMessage("Se eliminó correctamente el contrato", SAPbouiCOM.BoMessageTime.bmt_Short, false);
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al eliminar el contrato: {0}", lObjException.Message));
            }
        }

        private Rise GetRiseObject()
        {
            int lIntOrigFolio = 0;
            if (!string.IsNullOrEmpty(txtFolioRelation.Value))
            {
                int lIntFolio = mObjMachineryServiceFactory.GetRiseService().GetOriginalFolio(int.Parse(txtFolioRelation.Value));
                if (lIntFolio == 0)
                {
                    lIntOrigFolio = int.Parse(txtFolioRelation.Value);
                }
                else
                {
                    lIntOrigFolio = lIntFolio;
                }
            }

            return mObjRise = new Rise
            {
                IdRise = int.Parse(txtFolio.Value),
                CreatedDate = DateTime.Parse(txtDate.Value),
                StartDate = DateTimeUtility.StringToDateTime(txtStartDate.Value),
                EndDate = DateTimeUtility.StringToDateTime(txtEndDate.Value),
                Client = mStrClientCode,
                Supervisor = int.Parse(mStrSupervisorCode),
                DocStatus = (int)RiseStatusEnum.Active,
                DocRef = string.IsNullOrEmpty(txtFolioRelation.Value) ? 0 : int.Parse(txtFolioRelation.Value),
                UserId = mObjMachineryServiceFactory.GetUsersService().GetUserId(Application.SBO_Application.Company.UserName),
                HasCommission = "N",
                HasStockTransfer = "N",
                OriginalFolio = lIntOrigFolio,
            };
        }

        public void CancelRise()
        {
            try
            {
                if (string.IsNullOrEmpty(txtFolio.Value))
                    return;

                if (!mObjMachineryServiceFactory.GetRiseService().ExistsRise(int.Parse(txtFolio.Value)))
                {
                    Application.SBO_Application.SetStatusBarMessage("La cabecera de la subida aún no existe, favor de guardarla para continuar", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                int lIntValue = Application.SBO_Application.MessageBox("¿Desea cancelar la subida?", 1, "Aceptar", "Cancelar", "");

                if (lIntValue != 1)
                {
                    return;
                }

                if (mObjRise != null)
                {
                    this.UIAPIRawForm.Freeze(true);

                    mObjRise.DocStatus = (int)RiseStatusEnum.Cancelled;
                    mObjMachineryServiceFactory.GetRiseService().Update(mObjRise);

                    txtStatus.Value = RiseStatusEnum.Cancelled.GetDescription();

                    UIApplication.GetApplication().Forms.ActiveForm.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE;
                    ShowOrHideOpenBtn();

                    //BlockControls();

                    Application.SBO_Application.SetStatusBarMessage("Se canceló correctamente la subida", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                }
                else
                {
                    Application.SBO_Application.SetStatusBarMessage("No se seleccionó una subida", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - CancelRise] Error al cancelar la subida: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al cancelar la subida {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void CloseRise()
        {
            try
            {
                if (string.IsNullOrEmpty(txtFolio.Value))
                    return;

                if (!mObjMachineryServiceFactory.GetRiseService().ExistsRise(int.Parse(txtFolio.Value)))
                {
                    Application.SBO_Application.SetStatusBarMessage("La cabecera de la subida aún no existe, favor de guardarla para continuar", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                if (dtHours.Rows.Count <= 0)
                {
                    UIApplication.ShowError("No puede cerrar la subida si no existen registros de horas");
                    return;
                }

                if (dtTransitHours.Rows.Count <= 0)
                {
                    UIApplication.ShowError("No puede cerrar la subida si no existen registros de horas tránsito");
                    return;
                }

                if (dtMachineryPerformance.Rows.Count <= 0 && dtVehiclePerformance.Rows.Count <= 0)
                {
                    UIApplication.ShowError("No puede cerrar la subida si no existen registros de rendimiento");
                    return;
                }

                int lIntValue = Application.SBO_Application.MessageBox("¿Desea cerrar la subida?", 1, "Aceptar", "Cancelar", "");

                if (lIntValue != 1)
                {
                    return;
                }

                if (mObjRise != null)
                {
                    this.UIAPIRawForm.Freeze(true);

                    mObjRise.DocStatus = (int)RiseStatusEnum.Close;
                    mObjMachineryServiceFactory.GetRiseService().Update(mObjRise);

                    SaveConsumables();
                    SaveTravelExpenses();
                    SaveHours();
                    SaveTransitHours();

                    //SaveInitialsRecords();
                    //SavePurchasesInvReqRecords();
                    //SaveFinalsRecords();
                    //SaveTotalsRecords(); //guardar información cuando se realice salida de mercancía

                    SavePerformance();

                    ShowOrHideOpenBtn();

                    txtStatus.Value = RiseStatusEnum.Close.GetDescription();

                    UIApplication.GetApplication().Forms.ActiveForm.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE;
                    ShowOrHideOpenBtn();

                    //BlockControls();

                    if (!mObjMachineryServiceFactory.GetGoodIssuesService().ExistsGoodIssue(mObjRise.IdRise))
                    {
                        SaveInitialsRecords();
                        SavePurchasesInvReqRecords();
                        SaveFinalsRecords();
                        SaveTotalsRecords();

                        mtxInitialRecords.Item.Enabled = false;
                        mtxPurchase.Item.Enabled = false;
                        mtxFinalRecords.Item.Enabled = false;
                        mtxConsumedTotals.Item.Enabled = false;
                        btnSaveIR.Item.Enabled = false;
                    }

                    Application.SBO_Application.SetStatusBarMessage("Se cerró correctamente la subida", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                }
                else
                {
                    Application.SBO_Application.SetStatusBarMessage("No se seleccionó una subida", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - CloseRise] Error al cerrar la subida: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al cerrar la subida {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void OpenRise()
        {
            try
            {
                if (string.IsNullOrEmpty(txtFolio.Value))
                    return;

                if (!mObjMachineryServiceFactory.GetRiseService().ExistsRise(int.Parse(txtFolio.Value)))
                {
                    Application.SBO_Application.SetStatusBarMessage("La cabecera de la subida aún no existe, favor de guardarla para continuar", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                int lIntValue = Application.SBO_Application.MessageBox("¿Desea abrir la subida?", 1, "Aceptar", "Cancelar", "");

                if (lIntValue != 1)
                {
                    return;
                }

                if (mObjRise != null)
                {
                    this.UIAPIRawForm.Freeze(true);

                    mObjRise.DocStatus = (int)RiseStatusEnum.ReOpen;
                    mObjMachineryServiceFactory.GetRiseService().Update(mObjRise);

                    txtStatus.Value = RiseStatusEnum.ReOpen.GetDescription();

                    UIApplication.GetApplication().Forms.ActiveForm.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE;

                    ShowOrHideOpenBtn();

                    UnBlockControlsForReopenRise();

                    Application.SBO_Application.SetStatusBarMessage("Se abrió correctamente la subida", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                }
                else
                {
                    Application.SBO_Application.SetStatusBarMessage("No se seleccionó una subida", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - OpenRise] Error al abrir la subida: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al abrir la subida {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void OpenFoliosForm()
        {
            try
            {
                if (string.IsNullOrEmpty(txtFolio.Value))
                    return;

                if (Application.SBO_Application.MessageBox("¿Desea relacionar la subida? Se actualizará la información capturada y se guardará la subida", 1, "Aceptar", "Cancelar", "") == 1)
                {
                    if (mObjMachineryServiceFactory.GetRiseService().ExistsRise(int.Parse(txtFolio.Value)))
                    {
                        UIApplication.ShowError("La cabecera de la subida ya existe, no es posible relacionarla");
                        return;
                    }

                    if (ValidateRiseControls())
                    {
                        UIApplication.ShowError("Verificar campos vacíos");
                        return;
                    }

                    this.UIAPIRawForm.Freeze(true);

                    ClearMatrix(dtEmployees.UniqueID, mtxEmployees);
                    ClearMatrix(dtContracts.UniqueID, mtxContracts);
                    ClearMatrix(dtInitialRcords.UniqueID, mtxInitialRecords);

                    mObjFrmFolios = new frmCFLFolios();
                    mObjFrmFolios.Show();
                }
            }
            catch (Exception lObjException)
            {
                if (lObjException.Message.Contains("Failed to create form. Please check the form attributes"))
                {
                    if (Application.SBO_Application.MessageBox("La pantalla de folios relacionados ya se encuentra abierta, ¿desea cerrar la actual?", 1, "Aceptar", "Cancelar", "") == 1)
                    {
                        UIApplication.GetApplication().Forms.Item("frmFolios").Close();
                    }
                }
                else
                {
                    throw new Exception(string.Format("Error al abrir la pantalla de folios relacionados: {0}", lObjException.Message));
                }
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void OpenInventoryRequestForm()
        {
            try
            {
                if (string.IsNullOrEmpty(txtFolio.Value))
                    return;

                if (!mObjMachineryServiceFactory.GetRiseService().ExistsRise(int.Parse(txtFolio.Value)))
                {
                    Application.SBO_Application.SetStatusBarMessage("La cabecera de la subida aún no existe, favor de guardarla para continuar", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                this.UIAPIRawForm.Freeze(true);

                String menuList = SAPbouiCOM.Framework.Application.SBO_Application.Menus.GetAsXML();
                Application.SBO_Application.ActivateMenuItem("3088");//2050

                SAPbouiCOM.EditText txtToWshCode = ((SAPbouiCOM.EditText)UIApplication.GetApplication().Forms.GetForm("1250000940", -1).Items.Item("1470000101").Specific);
                txtToWshCode.Value = "MQHEOBRA";

                if (!Application.SBO_Application.Menus.Item("6913").Checked)
                {
                    Application.SBO_Application.ActivateMenuItem("6913");//2050
                }
                SAPbouiCOM.Form lFrmSalesOrder = Application.SBO_Application.Forms.GetFormByTypeAndCount(-1250000940, -1);
                SAPbouiCOM.EditText txtSORiseFolio = ((SAPbouiCOM.EditText)(lFrmSalesOrder.Items.Item("U_MQ_Rise").Specific));
                txtSORiseFolio.Value = txtFolio.Value;
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al abrir la pantalla de solicitud de traslado {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void OpenContractsListForm()
        {
            try
            {
                if (string.IsNullOrEmpty(txtFolio.Value))
                    return;

                if (!mObjMachineryServiceFactory.GetRiseService().ExistsRise(int.Parse(txtFolio.Value)))
                {
                    Application.SBO_Application.SetStatusBarMessage("La cabecera de la subida aún no existe, favor de guardarla para continuar", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                this.UIAPIRawForm.Freeze(true);

                mObjFrmContractsList = new frmCFLOrdersSale();
                mObjFrmContractsList.Show();
            }
            catch (Exception lObjException)
            {
                if (lObjException.Message.Contains("Failed to create form. Please check the form attributes"))
                {
                    if (Application.SBO_Application.MessageBox("La pantalla de seleccionar contratos ya se encuentra abierta, ¿desea cerrar la actual?", 1, "Aceptar", "Cancelar", "") == 1)
                    {
                        UIApplication.GetApplication().Forms.Item("frmCFLOV").Close();
                    }
                }
                else
                {
                    throw new Exception(string.Format("Error al abrir la pantalla con el listado de contratos {0}", lObjException.Message));
                }
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void OpenContractForm()
        {
            try
            {
                if (string.IsNullOrEmpty(txtFolio.Value))
                    return;

                if (!mObjMachineryServiceFactory.GetRiseService().ExistsRise(int.Parse(txtFolio.Value)))
                {
                    Application.SBO_Application.SetStatusBarMessage("La cabecera de la subida aún no existe, favor de guardarla para continuar", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                mObjFrmContracts = new frmContracts(int.Parse(txtFolio.Value), mStrClientCode, txtClient.Value, txtStartDate.Value, txtEndDate.Value,
                                                    UsersTypeEnum.Machinery, ContractModeEnum.Purchase);
                mObjFrmContracts.Show();
            }
            catch (Exception lObjException)
            {
                if (lObjException.Message.Contains("Failed to create form. Please check the form attributes"))
                {
                    if (Application.SBO_Application.MessageBox("La pantalla de creación de contratos ya se encuentra abierta, ¿desea cerrar la actual?", 1, "Aceptar", "Cancelar", "") == 1)
                    {
                        UIApplication.GetApplication().Forms.Item("frmCont").Close();
                    }
                }
                else
                {
                    throw new Exception(string.Format("Error al abrir la pantalla de contratos {0}", lObjException.Message));
                }
            }
        }

        public void OpenTravelExpensesForm()
        {
            try
            {
                if (string.IsNullOrEmpty(txtFolio.Value))
                    return;

                if (!mObjMachineryServiceFactory.GetRiseService().ExistsRise(int.Parse(txtFolio.Value)))
                {
                    Application.SBO_Application.SetStatusBarMessage("La cabecera de la subida aún no existe, favor de guardarla para continuar", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                if (string.IsNullOrEmpty(mStrSupervisorCode))
                {
                    Application.SBO_Application.SetStatusBarMessage("Seleccione un supervisor para solicitar viáticos", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                if (dtEmployees.Rows.Count <= 0)
                {
                    Application.SBO_Application.SetStatusBarMessage("No cuenta con operadores para solicitar viáticos", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                if (string.IsNullOrEmpty(txtStartDate.Value) || string.IsNullOrEmpty(txtEndDate.Value))
                {
                    Application.SBO_Application.SetStatusBarMessage("Verificar las fechas de inicio y fin", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                int lIntTotalDays = DateTimeUtility.GetTotalDays(DateTimeUtility.StringToDateTime(txtEndDate.Value), DateTimeUtility.StringToDateTime(txtStartDate.Value));

                mObjFrmTravelExpenses = new frmTravelExpenses(int.Parse(txtFolio.Value), mStrSupervisorCode, txtSupervisor.Value, lIntTotalDays, dtEmployees, IsFirstTravelExpenses());
                mObjFrmTravelExpenses.Show();
            }
            catch (Exception lObjException)
            {
                if (lObjException.Message.Contains("Failed to create form. Please check the form attributes"))
                {
                    if (Application.SBO_Application.MessageBox("La pantalla de solicitud de viáticos ya se encuentra abierta, ¿desea cerrar la actual?", 1, "Aceptar", "Cancelar", "") == 1)
                    {
                        UIApplication.GetApplication().Forms.Item("frmTExp").Close();
                    }
                }
                else
                {
                    throw new Exception(string.Format("Error al abrir la pantalla de solicitud de viáticos {0}", lObjException.Message));
                }
            }
        }

        public void AddConsumable(ConsumablesDTO lObjConsumable)
        {
            try
            {
                if (lObjConsumable == null)
                    return;

                this.UIAPIRawForm.Freeze(true);

                //ConsumablesDTO lObjConsumable = mObjMachineryServiceFactory.GetConsumablesService().GetInventoryRequestById(int.Parse(pStrDocEntry));

                dtConsumables.Rows.Add();
                dtConsumables.SetValue("#", dtConsumables.Rows.Count - 1, dtConsumables.Rows.Count);
                dtConsumables.SetValue("CodeTCons", dtConsumables.Rows.Count - 1, lObjConsumable.Code);
                dtConsumables.SetValue("DateCons", dtConsumables.Rows.Count - 1, lObjConsumable.DocDate);
                dtConsumables.SetValue("FolRCons", dtConsumables.Rows.Count - 1, lObjConsumable.DocEntry);
                dtConsumables.SetValue("FolTCons", dtConsumables.Rows.Count - 1, lObjConsumable.TransferFolio);

                mtxConsumables.LoadFromDataSource();
                mtxConsumables.AutoResizeColumns();

                /*SAPbouiCOM.Form lFrmStockTransfReq = Application.SBO_Application.Forms.ActiveForm;
                lFrmStockTransfReq.Close();*/
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar la solicitud de consumible {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void AddEmployee(EmployeesDTO pObjEmployeesDTO)
        {
            try
            {
                if (pObjEmployeesDTO == null)
                    return;

                dtEmployees.Rows.Add();
                dtEmployees.SetValue("#", dtEmployees.Rows.Count - 1, dtEmployees.Rows.Count);
                dtEmployees.SetValue("CodeTEmp", dtEmployees.Rows.Count - 1, pObjEmployeesDTO.Code);
                dtEmployees.SetValue("EmpId", dtEmployees.Rows.Count - 1, pObjEmployeesDTO.Id);
                dtEmployees.SetValue("EmpName", dtEmployees.Rows.Count - 1, pObjEmployeesDTO.Employee);
                dtEmployees.SetValue("EmpChk", dtEmployees.Rows.Count - 1, pObjEmployeesDTO.Status);

                mtxEmployees.LoadFromDataSource();
                mtxEmployees.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el operador {0}", lObjException.Message));
            }
        }

        public void AddContract(ContractsDTO pObjContract)
        {
            try
            {
                if (pObjContract == null)
                    return;

                if (ExistItemOnGrid(pObjContract.DocEntry.ToString(), dtContracts, 1))
                {
                    Application.SBO_Application.SetStatusBarMessage("No puede repetir el contrato", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                this.UIAPIRawForm.Freeze(true);

                dtContracts.Rows.Add();
                dtContracts.SetValue("#", dtContracts.Rows.Count - 1, dtContracts.Rows.Count);
                dtContracts.SetValue("DocECont", dtContracts.Rows.Count - 1, pObjContract.DocEntry);
                dtContracts.SetValue("DocNCont", dtContracts.Rows.Count - 1, pObjContract.DocNum);
                dtContracts.SetValue("DateCont", dtContracts.Rows.Count - 1, pObjContract.DocDate);
                dtContracts.SetValue("TypeCont", dtContracts.Rows.Count - 1, pObjContract.TypeDescription);
                dtContracts.SetValue("StsCont", dtContracts.Rows.Count - 1, pObjContract.StatusDescription);
                dtContracts.SetValue("TypCCont", dtContracts.Rows.Count - 1, pObjContract.Type);
                dtContracts.SetValue("StsCCont", dtContracts.Rows.Count - 1, pObjContract.Status);
                dtContracts.SetValue("ImpCont", dtContracts.Rows.Count - 1, pObjContract.Import);
                dtContracts.SetValue("CodeTCont", dtContracts.Rows.Count - 1, pObjContract.Code);
                dtContracts.SetValue("CardName", dtContracts.Rows.Count - 1, pObjContract.CardName);
                dtContracts.SetValue("MunpId", dtContracts.Rows.Count - 1, pObjContract.MunicipalityCode);
                dtContracts.SetValue("Munp", dtContracts.Rows.Count - 1, pObjContract.Municipality);

                mtxContracts.LoadFromDataSource();
                mtxContracts.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el contrato {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void AddTravelExpenses(TravelExpensesDTO pObjTravelExpensesDTO)
        {
            try
            {
                if (pObjTravelExpensesDTO == null)
                    return;

                this.UIAPIRawForm.Freeze(true);

                dtTravelExpenses.Rows.Add();
                dtTravelExpenses.SetValue("#", dtTravelExpenses.Rows.Count - 1, dtTravelExpenses.Rows.Count);
                dtTravelExpenses.SetValue("DateTravE", dtTravelExpenses.Rows.Count - 1, pObjTravelExpensesDTO.DocDate);
                dtTravelExpenses.SetValue("FolTravE", dtTravelExpenses.Rows.Count - 1, pObjTravelExpensesDTO.Folio);
                dtTravelExpenses.SetValue("DocETravE", dtTravelExpenses.Rows.Count - 1, pObjTravelExpensesDTO.DocEntry);
                dtTravelExpenses.SetValue("DocNTravE", dtTravelExpenses.Rows.Count - 1, pObjTravelExpensesDTO.DocNum);
                dtTravelExpenses.SetValue("ImpTravE", dtTravelExpenses.Rows.Count - 1, pObjTravelExpensesDTO.Total);
                dtTravelExpenses.SetValue("StsTravE", dtTravelExpenses.Rows.Count - 1, pObjTravelExpensesDTO.StatusDescription);
                dtTravelExpenses.SetValue("StsCTravE", dtTravelExpenses.Rows.Count - 1, pObjTravelExpensesDTO.Status);
                dtTravelExpenses.SetValue("CodeTTrav", dtTravelExpenses.Rows.Count - 1, pObjTravelExpensesDTO.Code);

                mtxTravelExpenses.LoadFromDataSource();
                mtxTravelExpenses.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar la solicitud de viáticos {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void AddInitialRecord(ConsumablesDocumentsDTO pObjInitialRecordDTO)
        {
            try
            {
                if (pObjInitialRecordDTO == null)
                    return;

                //this.UIAPIRawForm.Freeze(true);

                dtInitialRcords.Rows.Add();
                dtInitialRcords.SetValue("#", dtInitialRcords.Rows.Count - 1, dtInitialRcords.Rows.Count);
                /*dtInitialRcords.SetValue("DocEIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.DocEntry);
                dtInitialRcords.SetValue("DocNIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.DocNum);*/
                dtInitialRcords.SetValue("CodeTIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.Code);
                dtInitialRcords.SetValue("IdRiIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.IdRise);
                dtInitialRcords.SetValue("ActCodIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.ActivoCode);
                dtInitialRcords.SetValue("ActNumIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.EcoNum);
                dtInitialRcords.SetValue("DslMIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.DieselM);
                dtInitialRcords.SetValue("DslTIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.DieselT);
                dtInitialRcords.SetValue("GasIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.Gas);
                dtInitialRcords.SetValue("15W40IR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.F15W40);
                dtInitialRcords.SetValue("HidIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.Hidraulic);
                dtInitialRcords.SetValue("SAE40IR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.SAE40);
                dtInitialRcords.SetValue("TransIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.Transmition);
                dtInitialRcords.SetValue("OilsIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.Oils);
                dtInitialRcords.SetValue("KmHrIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.KmHr);
                dtInitialRcords.SetValue("EqTypIR", dtInitialRcords.Rows.Count - 1, pObjInitialRecordDTO.EquipmentType);

                mtxInitialRecords.LoadFromDataSource();
                mtxInitialRecords.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el registro inicial {0}", lObjException.Message));
            }
            finally
            {
                //this.UIAPIRawForm.Freeze(false);
            }
        }

        public void AddFinalRecord(ConsumablesDocumentsDTO pObjFinalRecordDTO)
        {
            try
            {
                if (pObjFinalRecordDTO == null)
                    return;

                dtFinalRecords.Rows.Add();
                dtFinalRecords.SetValue("#", dtFinalRecords.Rows.Count - 1, dtFinalRecords.Rows.Count);
                dtFinalRecords.SetValue("CodeTFR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.Code);
                dtFinalRecords.SetValue("IdRiFR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.IdRise);
                dtFinalRecords.SetValue("ActCodFR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.ActivoCode);
                dtFinalRecords.SetValue("ActNumFR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.EcoNum);
                dtFinalRecords.SetValue("DslMFR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.DieselM);
                dtFinalRecords.SetValue("DslTFR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.DieselT);
                dtFinalRecords.SetValue("GasFR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.Gas);
                dtFinalRecords.SetValue("15W40FR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.F15W40);
                dtFinalRecords.SetValue("HidFR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.Hidraulic);
                dtFinalRecords.SetValue("SAE40FR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.SAE40);
                dtFinalRecords.SetValue("TransFR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.Transmition);
                dtFinalRecords.SetValue("OilsFR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.Oils);
                dtFinalRecords.SetValue("KmHrFR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.KmHr);
                dtFinalRecords.SetValue("EqTypFR", dtFinalRecords.Rows.Count - 1, pObjFinalRecordDTO.EquipmentType);

                mtxFinalRecords.LoadFromDataSource();
                mtxFinalRecords.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el registro final {0}", lObjException.Message));
            }
            finally
            {

            }
        }

        public void AddPurchaseInvReqRecord(ConsumablesDocumentsDTO pObjConsumableDocDTO)
        {
            try
            {
                if (pObjConsumableDocDTO == null)
                    return;

                dtPurchase.Rows.Add();
                dtPurchase.SetValue("#", dtPurchase.Rows.Count - 1, dtPurchase.Rows.Count);
                dtPurchase.SetValue("CodeTPrch", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.Code);
                dtPurchase.SetValue("DocTyPrch", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.DocType);
                dtPurchase.SetValue("DocTyDPrc", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.DocTypeDescription);
                dtPurchase.SetValue("IdRiPrch", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.IdRise);
                dtPurchase.SetValue("ActCodPrc", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.ActivoCode);
                dtPurchase.SetValue("ActNumPrc", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.EcoNum);
                dtPurchase.SetValue("GasPrch", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.Gas);
                dtPurchase.SetValue("15W40Prch", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.F15W40);
                dtPurchase.SetValue("HidPrch", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.Hidraulic);
                dtPurchase.SetValue("SAE40Prch", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.SAE40);
                dtPurchase.SetValue("TransPrch", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.Transmition);
                dtPurchase.SetValue("OilsPrch", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.Oils);
                dtPurchase.SetValue("EqTypPrch", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.EquipmentType);
                dtPurchase.SetValue("DslTPrch", dtPurchase.Rows.Count - 1, pObjConsumableDocDTO.DieselT);

                mtxPurchase.LoadFromDataSource();
                mtxPurchase.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el registro de compra {0}", lObjException.Message));
            }
            finally
            {

            }
        }

        public void AddTotalRecord(ConsumablesDocumentsDTO pObjTotalRecord)
        {
            try
            {
                if (pObjTotalRecord == null)
                    return;

                dtConsumedTotals.Rows.Add();
                dtConsumedTotals.SetValue("#", dtConsumedTotals.Rows.Count - 1, dtConsumedTotals.Rows.Count);
                dtConsumedTotals.SetValue("CodeTTR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.Code);
                dtConsumedTotals.SetValue("IdRiTR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.IdRise);
                dtConsumedTotals.SetValue("ActCodTR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.ActivoCode);
                dtConsumedTotals.SetValue("ActNumTR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.EcoNum);
                dtConsumedTotals.SetValue("DslMTR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.DieselM);
                dtConsumedTotals.SetValue("DslTTR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.DieselT);
                dtConsumedTotals.SetValue("GasTR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.Gas);
                dtConsumedTotals.SetValue("15W40TR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.F15W40);
                dtConsumedTotals.SetValue("HidTR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.Hidraulic);
                dtConsumedTotals.SetValue("SAE40TR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.SAE40);
                dtConsumedTotals.SetValue("TransTR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.Transmition);
                dtConsumedTotals.SetValue("OilsTR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.Oils);
                dtConsumedTotals.SetValue("KmHrTR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.KmHr);
                dtConsumedTotals.SetValue("EqTypTR", dtConsumedTotals.Rows.Count - 1, pObjTotalRecord.EquipmentType);

                mtxConsumedTotals.LoadFromDataSource();
                mtxConsumedTotals.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el registro total {0}", lObjException.Message));
            }
            finally
            {

            }
        }

        private void RefreshStatusTravelExpensesMatrix()
        {
            try
            {
                for (int i = 0; i < dtTravelExpenses.Rows.Count; i++)
                {
                    int lIntDocEntry = int.Parse(dtTravelExpenses.GetValue(3, i).ToString());

                    TravelExpensesDTO lObjTravelExpensesDTO = mObjMachineryServiceFactory.GetTravelExpensesService().GetPayment(lIntDocEntry);

                    dtTravelExpenses.SetValue("StsTravE", i, lObjTravelExpensesDTO.StatusDescription);
                    dtTravelExpenses.SetValue("StsCTravE", i, lObjTravelExpensesDTO.Status);
                }

                mtxTravelExpenses.LoadFromDataSource();
                mtxTravelExpenses.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al actualizar el listado de solicitudes de viáticos {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void RefreshContractsStatusMatrix()
        {
            try
            {
                for (int i = 0; i < dtContracts.Rows.Count; i++)
                {
                    int lIntDocEntry = int.Parse(dtContracts.GetValue(1, i).ToString());

                    ContractsDTO lObjContractsDTO = mObjMachineryServiceFactory.GetContractsService().GetContract(lIntDocEntry);

                    dtContracts.SetValue("StsCont", i, lObjContractsDTO.StatusDescription);
                    dtContracts.SetValue("StsCCont", i, lObjContractsDTO.Status);
                }

                mtxContracts.LoadFromDataSource();
                mtxContracts.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al actualizar el listado de contratos {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public bool ExistItemOnGrid(string pStrDocEntry, SAPbouiCOM.DataTable pObjDatatable, int pIntColumn)
        {
            bool lBolResult = false;

            for (int i = 0; i < pObjDatatable.Rows.Count; i++)
            {
                string lStrDocEntry = pObjDatatable.GetValue(pIntColumn, i).ToString();

                if (pStrDocEntry == lStrDocEntry)
                {
                    lBolResult = true;
                }
            }

            return lBolResult;
        }

        public bool ExistItemOnValidValues(SAPbouiCOM.Column pObjColumn, string pStrValue)
        {
            bool lBolResult = false;

            for (int i = 0; i < pObjColumn.ValidValues.Count; i++)
            {
                string lStrValueKey = pObjColumn.ValidValues.Item(i).Value;

                if (lStrValueKey == pStrValue)
                {
                    lBolResult = true;
                }
            }

            return lBolResult;
        }

        private bool IsFirstTravelExpenses()
        {
            bool lBolResult = true;

            for (int i = 0; i < dtTravelExpenses.Rows.Count; i++)
            {
                int lIntDocEntry = int.Parse(dtTravelExpenses.GetValue(7, i).ToString());

                if ((TravelExpStatusEnum)lIntDocEntry == TravelExpStatusEnum.Active)
                {
                    lBolResult = false;
                }
            }

            return lBolResult;
        }

        public void ShowOrHideOpenBtn()
        {
            if (mObjRise != null)
            {
                if (mObjRise.DocStatus == (int)RiseStatusEnum.Close) //Cerrado
                {
                    /*bool lBolResult = mObjMachineryServiceFactory
                        .GetAuthorizationService()
                        .IsOperationsUser(
                            mObjMachineryServiceFactory
                            .GetUsersService()
                            .GetUserId(Application.SBO_Application.Company.UserName));*/

                    if (mBolIsOperationsUser)
                    {
                        btnOpen.Item.Visible = true;
                        btnOpen.Item.Enabled = true;

                        return;
                    }
                }
            }

            btnOpen.Item.Visible = false;
        }

        public bool ValidateRiseControls()
        {
            bool lBolEmpty = false;

            if (string.IsNullOrEmpty(txtFolio.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(txtDate.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(txtStartDate.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(txtEndDate.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(txtClient.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(txtSupervisor.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(txtStatus.Value))
            {
                lBolEmpty = true;
            }

            return lBolEmpty;
        }

        public void BlockControls()
        {
            txtClient.Item.Enabled = false;
            txtSupervisor.Item.Enabled = false;
            btnOpen.Item.Enabled = false;
            btnClose.Item.Enabled = false;
            btnCancel.Item.Enabled = false;
            btnSave.Item.Enabled = false;
            btnSaveIR.Item.Enabled = false;
        }

        public void UnBlockControls()
        {
            /*txtClient.Item.Enabled = true;
            txtSupervisor.Item.Enabled = true;
            btnOpen.Item.Enabled = true;
            btnClose.Item.Enabled = true;
            btnCancel.Item.Enabled = true;
            btnSave.Item.Enabled = true;*/

            UIAPIRawForm.EnableMenu("1293", true); //Borrar

            //txt
            txtSupervisor.Item.Enabled = true;
            txtSupervisor.Item.Click();
            txtFolio.Item.Enabled = false;
            txtClient.Item.Enabled = true;
            txtStartDate.Item.Enabled = true;
            txtEndDate.Item.Enabled = true;
            txtTotalsHours.Item.Enabled = true;
            txtKmHrTotal.Item.Enabled = true;
            txtFootHoursTotal.Item.Enabled = true;

            //mtx
            mtxEmployees.Item.Enabled = true;
            mtxInitialRecords.Item.Enabled = true;
            mtxFinalRecords.Item.Enabled = true;
            mtxHours.Item.Enabled = true;
            mtxTransitHours.Item.Enabled = true;

            //btn
            btnSave.Item.Enabled = true;
            btnClose.Item.Enabled = true;
            btnCancel.Item.Enabled = true;
            btnVincOV.Item.Enabled = true;
            btnCreateOV.Item.Enabled = true;
            btnConsumables.Item.Enabled = true;
            btnTravelExpenses.Item.Enabled = true;
            btnSearchFolio.Item.Enabled = true;
            btnExit.Item.Enabled = true;
            btnCalculatePerf.Item.Enabled = true;
            btnSaveIR.Item.Enabled = true;
        }

        private void UnBlockControlsForReopenRise()
        {
            UIAPIRawForm.EnableMenu("1293", false); //Borrar

            //txt
            txtTotalsHours.Item.Enabled = true;
            txtTotalsHours.Item.Click();
            txtKmHrTotal.Item.Enabled = true;
            txtFootHoursTotal.Item.Enabled = true;

            //mtx
            mtxHours.Item.Enabled = true;
            mtxTransitHours.Item.Enabled = true;
            mtxEmployees.Item.Enabled = true;
            mtxContracts.Item.Enabled = true;

            //btn
            btnVincOV.Item.Enabled = true;
            btnCreateOV.Item.Enabled = true;
        }
        #endregion

        #region ChooseFromList
        /// <summary>
        /// Fill choose from list.
        /// </summary>
        private void LoadChoosesFromList()
        {
            SAPbouiCOM.ChooseFromList lObjCFLSup = InitChooseFromLists(false, "171", "CFL_Supv", this.UIAPIRawForm.ChooseFromLists);
            AddConditionSupervisorCFL(lObjCFLSup);

            SAPbouiCOM.ChooseFromList lObjCFLClients = InitChooseFromLists(false, "2", "CFL_0", this.UIAPIRawForm.ChooseFromLists);
            AddConditionClientCFL(lObjCFLClients);

            SAPbouiCOM.ChooseFromList lObjCFLEmployees = InitChooseFromLists(false, "171", "CFL_Emp", this.UIAPIRawForm.ChooseFromLists);
            AddConditionEmployeesCFL(lObjCFLEmployees);
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

        private void AddConditionSupervisorCFL(SAPbouiCOM.ChooseFromList pCFL)
        {
            SAPbouiCOM.Condition lObjCon = null;
            SAPbouiCOM.Conditions lObjCons = new SAPbouiCOM.Conditions();

            /*lObjCon = lObjCons.Add();
            lObjCon.Alias = "position";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "4";

            lObjCon.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;*/

            lObjCon = lObjCons.Add();
            lObjCon.Alias = "dept";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "12";

            pCFL.SetConditions(lObjCons);
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

        private void AddConditionEmployeesCFL(SAPbouiCOM.ChooseFromList pCFL)
        {
            SAPbouiCOM.Condition lObjCon = null;
            SAPbouiCOM.Conditions lObjCons = new SAPbouiCOM.Conditions();

            /*lObjCon = lObjCons.Add();
            lObjCon.Alias = "position";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "3";

            lObjCon.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;*/

            lObjCon = lObjCons.Add();
            lObjCon.Alias = "dept";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "12";

            pCFL.SetConditions(lObjCons);
        }
        #endregion

        #region TextBox
        private void SetCFLToTxt()
        {
            txtClient.DataBind.SetBound(true, "", "CFL_0");
            txtClient.ChooseFromListUID = "CFL_0";

            txtSupervisor.DataBind.SetBound(true, "", "CFL_Supv");
            txtSupervisor.ChooseFromListUID = "CFL_Supv";
        }
        #endregion

        #endregion

        #region Consumables tab
        private void LoadConsumablesControls()
        {
            try
            {
                if (string.IsNullOrEmpty(txtFolio.Value))
                    return;

                if (mObjMachineryServiceFactory.GetRiseService().ExistsRise(int.Parse(txtFolio.Value)))
                {
                    this.UIAPIRawForm.Freeze(true);

                    LoadInitialRecords(mObjRise.IdRise, (RiseStatusEnum)mObjRise.DocStatus);
                    LoadPurchasesRecords(mObjRise.IdRise, (RiseStatusEnum)mObjRise.DocStatus);
                    LoadFinalsRecords(mObjRise.IdRise, (RiseStatusEnum)mObjRise.DocStatus);
                    LoadConsumedTotals(mObjRise.IdRise, (RiseStatusEnum)mObjRise.DocStatus);
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - LoadConsumablesControls] Error: {0}", lObjException.Message));
                throw new Exception(lObjException.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void LoadInitialRecords(int pIntRiseId = 0, RiseStatusEnum pEnmRiseStatus = RiseStatusEnum.Active)
        {
            try
            {
                if (mtxInitialRecords.RowCount > 0)
                    return;

                List<ConsumablesDocumentsDTO> lLstInitialRecords = new List<ConsumablesDocumentsDTO>();

                if (pEnmRiseStatus == RiseStatusEnum.Active)
                {
                    /*if (string.IsNullOrEmpty(txtFolioRelation.Value)) //Sin folio relacionado
                    {
                    if (dtConsumables.Rows.Count <= 0)
                        return;*/

                    //mIntFirstInvReqDocEntry = int.Parse(dtConsumables.GetValue("FolRCons", 0).ToString());

                    lLstInitialRecords = mObjMachineryServiceFactory
                                         .GetConsumablesService()
                                         .GetInitialRecordsByRiseId(pIntRiseId);
                    /*}
                    else //Con folio relacionado
                    {
                        int lIntFolioRel = int.Parse(txtFolioRelation.Value);

                        //obtener registros finales de la subida relacionada y convertirlos en iniciales para mostrarlos en el grid de RI
                    }*/
                }
                else /*if (pEnmRiseStatus == RiseStatusEnum.Close)*/
                {
                    //lLstInitialRecords = mObjMachineryServiceFactory.GetInitialRecordsService().GetByRiseId(pIntRiseId);
                    lLstInitialRecords = mObjMachineryServiceFactory.GetConsumablesService().GetInitialRecordsUDTByRiseId(pIntRiseId);
                }

                foreach (var lObjInitialRecord in lLstInitialRecords)
                {
                    AddInitialRecord(lObjInitialRecord);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al cargar la información de registros iniciales {0}", lObjException.Message));
            }
            finally
            {

            }
        }

        private void LoadPurchasesRecords(int pIntRiseId = 0, RiseStatusEnum pEnmRiseStatus = RiseStatusEnum.Active)
        {
            try
            {
                if (mtxPurchase.RowCount > 0)
                    return;

                List<ConsumablesDocumentsDTO> lLstConsumablesDocuments = new List<ConsumablesDocumentsDTO>();

                if (pEnmRiseStatus == RiseStatusEnum.Active)
                {
                    //int lIntFolioRise = int.Parse(txtFolio.Value);

                    List<ConsumablesDocumentsDTO> lLstPurchases = mObjMachineryServiceFactory
                                                                 .GetConsumablesService()
                                                                 .GetPurchasesByRiseId(pIntRiseId);
                    /*List<ConsumablesDocumentsDTO> lLstInvRequest = mObjMachineryServiceFactory
                                                                  .GetConsumablesService()
                                                                  .GetOthersInventoryRequestsByRiseId(lIntFolioRise, mIntFirstInvReqDocEntry);*/

                    lLstConsumablesDocuments.AddRange(lLstPurchases);
                    //lLstConsumablesDocuments.AddRange(lLstInvRequest);
                }
                else /*if (pEnmRiseStatus == RiseStatusEnum.Close)*/
                {
                    //lLstConsumablesDocuments = mObjMachineryServiceFactory.GetPurchasesOrdersService().GetByRiseId(pIntRiseId);
                    lLstConsumablesDocuments = mObjMachineryServiceFactory.GetConsumablesService().GetPurchasesRecordsUDTByRiseId(pIntRiseId);
                }

                foreach (var lObjInitialRecord in lLstConsumablesDocuments)
                {
                    AddPurchaseInvReqRecord(lObjInitialRecord);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al cargar la información de compras {0}", lObjException.Message));
            }
            finally
            {

            }
        }

        private void LoadFinalsRecords(int pIntRiseId = 0, RiseStatusEnum pEnmRiseStatus = RiseStatusEnum.Active)
        {
            try
            {
                if (dtFinalRecords.Rows.Count > 0)
                {
                    return;
                }

                List<ConsumablesDocumentsDTO> lLstConsumablesDocuments = mObjMachineryServiceFactory.GetConsumablesService().GetFinalsRecordsByRiseId(pIntRiseId);

                foreach (var lObjConsmDoc in lLstConsumablesDocuments)
                {
                    AddFinalRecord(lObjConsmDoc);
                }

                #region OldVersion
                //if (pEnmRiseStatus == RiseStatusEnum.Active)
                //{
                //    if (dtFinalRecords.Rows.Count > 0)
                //    {
                //        return;
                //    }

                //    if (mObjMachineryServiceFactory.GetGoodIssuesService().ExistsGoodIssue(pIntRiseId))
                //    {
                //        List<ConsumablesDocumentsDTO> lLstConsumablesDocuments = mObjMachineryServiceFactory.GetConsumablesService().GetFinalsRecordsUDTByRiseId(pIntRiseId).ToList();

                //        foreach (var lObjConsmDoc in lLstConsumablesDocuments)
                //        {
                //            AddFinalRecord(lObjConsmDoc);
                //        }
                //    }
                //    else
                //    {
                //        for (int i = 0; i < dtInitialRcords.Rows.Count; i++)
                //        {
                //            string lStrActCode = dtInitialRcords.GetValue("ActCodIR", i).ToString();
                //            string lStrEcoNum = dtInitialRcords.GetValue("ActNumIR", i).ToString();
                //            string lStrEqmType = dtInitialRcords.GetValue("EqTypIR", i).ToString();

                //            if (!ExistItemOnGrid(lStrActCode, dtFinalRecords, 2))
                //            {
                //                ConsumablesDocumentsDTO lObjConsumablesDoc = new ConsumablesDocumentsDTO
                //                {
                //                    IdRise = mObjRise.IdRise,//46
                //                    ActivoCode = lStrActCode,
                //                    EcoNum = lStrEcoNum,
                //                    EquipmentType = lStrEqmType,
                //                };

                //                AddFinalRecord(lObjConsumablesDoc);
                //            }
                //        }

                //        for (int i = 0; i < dtPurchase.Rows.Count; i++)
                //        {
                //            string lStrActCode = dtPurchase.GetValue("ActCodPrc", i).ToString();
                //            string lStrEcoNum = dtPurchase.GetValue("ActNumPrc", i).ToString();
                //            string lStrEqmType = dtPurchase.GetValue("EqTypPrch", i).ToString();

                //            if (!ExistItemOnGrid(lStrActCode, dtFinalRecords, 2))
                //            {
                //                ConsumablesDocumentsDTO lObjConsumablesDoc = new ConsumablesDocumentsDTO
                //                {
                //                    IdRise = mObjRise.IdRise,//46
                //                    ActivoCode = lStrActCode,
                //                    EcoNum = lStrEcoNum,
                //                    EquipmentType = lStrEqmType,
                //                };

                //                AddFinalRecord(lObjConsumablesDoc);
                //            }
                //        }
                //    }
                //}
                //else /*if (pEnmRiseStatus == RiseStatusEnum.Close)*/
                //{
                //    if (dtFinalRecords.Rows.Count > 0)
                //    {
                //        return;
                //    }

                //    List<ConsumablesDocumentsDTO> lLstConsumablesDocuments = new List<ConsumablesDocumentsDTO>();
                //    //lLstConsumablesDocuments = mObjMachineryServiceFactory.GetFinalsRecordsService().GetByRelatedRiseId(pIntRiseId).ToList();
                //    lLstConsumablesDocuments = mObjMachineryServiceFactory.GetConsumablesService().GetFinalsRecordsUDTByRiseId(pIntRiseId).ToList();

                //    foreach (var lObjConsmDoc in lLstConsumablesDocuments)
                //    {
                //        AddFinalRecord(lObjConsmDoc);
                //    }
                //}
                #endregion
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al cargar la información de registros finales {0}", lObjException.Message));
            }
            finally
            {

            }
        }

        private void LoadConsumedTotals(int pIntRiseId = 0, RiseStatusEnum pEnmRiseStatus = RiseStatusEnum.Active)
        {
            try
            {
                List<ConsumablesDocumentsDTO> lLstConsumedTotals = new List<ConsumablesDocumentsDTO>();

                if (pEnmRiseStatus == RiseStatusEnum.Active)
                {
                    lLstConsumedTotals = mObjMachineryServiceFactory.GetConsumablesService().CalculateConsumedTotals(dtInitialRcords, dtPurchase, dtFinalRecords, dtConsumedTotals);

                    ClearMatrix(dtConsumedTotals.UniqueID, mtxConsumedTotals);
                }
                else/* if (pEnmRiseStatus == RiseStatusEnum.Close)*/
                {
                    if (mtxConsumedTotals.RowCount > 0)
                        return;

                    //lLstConsumedTotals = mObjMachineryServiceFactory.GetTotalsRecordsService().GetByRelatedRiseId(pIntRiseId).ToList();
                    lLstConsumedTotals = mObjMachineryServiceFactory.GetConsumablesService().GetTotalsRecordsUDTByRiseId(pIntRiseId).ToList();
                }

                foreach (var lObjConsumedTotal in lLstConsumedTotals)
                {
                    AddTotalRecord(lObjConsumedTotal);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al calcular los registros finales {0}", lObjException.Message));
            }
        }

        private void OpenGoodIssueForm()
        {
            try
            {
                if (dtConsumedTotals.Rows.Count <= 0)
                {
                    Application.SBO_Application.SetStatusBarMessage("No puede crear la salida de mercancía sin líneas", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                if (!mObjMachineryServiceFactory.GetRiseService().ExistsRise(int.Parse(txtFolio.Value)))
                {
                    Application.SBO_Application.SetStatusBarMessage("La cabecera de la subida aún no existe, favor de guardarla para continuar", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                if (mObjMachineryServiceFactory.GetConsumablesService().HasConsumedTotalsNegativeValues(dtConsumedTotals))
                {
                    Application.SBO_Application.SetStatusBarMessage("No puede crear la salida de mercancía con valores menores o iguales a 0 en las propiedades inventariables"
                                                                   , SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                this.UIAPIRawForm.Freeze(true);

                mFrmGoodIssue = new frmGoodIssue(int.Parse(txtFolio.Value), dtConsumedTotals);
                mFrmGoodIssue.Show();
            }
            catch (Exception lObjException)
            {
                throw new Exception(lObjException.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        #endregion

        #region Hours tab
        private void LoadHoursControls()
        {
            try
            {
                if (string.IsNullOrEmpty(txtFolio.Value))
                    return;

                if (mObjMachineryServiceFactory.GetRiseService().ExistsRise(int.Parse(txtFolio.Value)))
                {
                    this.UIAPIRawForm.Freeze(true);

                    LoadHoursRecords(mObjRise.IdRise, (RiseStatusEnum)mObjRise.DocStatus);
                    LoadTransitHoursRecords(mObjRise.IdRise, (RiseStatusEnum)mObjRise.DocStatus);

                    AddSupervisorsCbo();
                    AddOperatorsCbo();
                    AddMachineryHoursCbo();
                    AddContractsCbo();
                    AddMachineryTransitHoursCbo();
                    AddOperatorsTHCbo();
                    AddTramosCbo(0, 1);
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - LoadHoursControls] Error: {0}", lObjException.Message));
                throw new Exception(lObjException.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public void LoadHoursRecords(int pIntRiseId = 0, RiseStatusEnum pEnmRiseStatus = RiseStatusEnum.Active)
        {
            try
            {
                if (mtxHours.RowCount > 0)
                    return;

                /*if (pEnmRiseStatus != RiseStatusEnum.Active)
                {*/
                List<HoursRecordsDTO> lLstHoursRecords = mObjMachineryServiceFactory.GetHoursRecordsService().GetHoursRecordsByRiseId(pIntRiseId).ToList();

                foreach (var lObjHoursRecord in lLstHoursRecords)
                {
                    AddHoursRecord(lObjHoursRecord);
                }
                //}

                if (pEnmRiseStatus == RiseStatusEnum.Active)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        AddHoursRecord(new HoursRecordsDTO());
                    }
                }

                AddSupervisorsCbo();
                AddOperatorsCbo();
                AddMachineryHoursCbo();
                AddMachineryTransitHoursCbo();
                AddContractsCbo();
                //AddOperatorsTHCbo();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - LoadHoursRecords] Error: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al cargar el listado de contratos: {0}", lObjException.Message));
            }
        }

        public void LoadTransitHoursRecords(int pIntRiseId = 0, RiseStatusEnum pEnmRiseStatus = RiseStatusEnum.Active)
        {
            try
            {
                if (mtxTransitHours.RowCount > 0)
                    return;

                /*if (pEnmRiseStatus == RiseStatusEnum.Active)
                {
                }*/
                List<TransitHoursRecordsDTO> lLstTransitHoursRecords = mObjMachineryServiceFactory.GetTransitHoursRecordsService().GetUDTByRiseId(pIntRiseId);
                //mObjMachineryServiceFactory.GetTransitHoursRecordsService().GetByRiseId(pIntRiseId);

                foreach (var lObjTransitHours in lLstTransitHoursRecords)
                {
                    AddTransitHoursRecord(lObjTransitHours);
                }

                if (lLstTransitHoursRecords.Count <= 0)
                {
                    for (int i = 0; i < dtInitialRcords.Rows.Count; i++)
                    {
                        AddTransitHoursRecord(new TransitHoursRecordsDTO());
                    }
                }

                if (pEnmRiseStatus == RiseStatusEnum.Active)
                {
                    if (lLstTransitHoursRecords.Count < dtInitialRcords.Rows.Count)
                    {
                        for (int i = 0; i < dtInitialRcords.Rows.Count - lLstTransitHoursRecords.Count; i++)
                        {
                            if (dtTransitHours.Rows.Count < dtInitialRcords.Rows.Count)
                            {
                                AddTransitHoursRecord(new TransitHoursRecordsDTO());
                            }
                        }
                    }
                }

                //AddMachineryTransitHoursCbo();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al cargar el listado de horas tránsito: {0}", lObjException.Message));
            }
        }

        public void AddTransitHoursRecord(TransitHoursRecordsDTO pObjTransitHoursRecord)
        {
            try
            {
                if (pObjTransitHoursRecord == null)
                    return;

                dtTransitHours.Rows.Add();
                dtTransitHours.SetValue("#", dtTransitHours.Rows.Count - 1, dtTransitHours.Rows.Count);
                dtTransitHours.SetValue("IdRiTHrs", dtTransitHours.Rows.Count - 1, pObjTransitHoursRecord.IdRise);
                dtTransitHours.SetValue("MaqTHrs", dtTransitHours.Rows.Count - 1, pObjTransitHoursRecord.PrcCode);
                dtTransitHours.SetValue("NumEcoTH", dtTransitHours.Rows.Count - 1, pObjTransitHoursRecord.EcoNum);
                dtTransitHours.SetValue("HrsTH", dtTransitHours.Rows.Count - 1, pObjTransitHoursRecord.Hrs);
                dtTransitHours.SetValue("CodeTH", dtTransitHours.Rows.Count - 1, pObjTransitHoursRecord.Code);
                dtTransitHours.SetValue("OpdTH", dtTransitHours.Rows.Count - 1, pObjTransitHoursRecord.OperatorId);
                dtTransitHours.SetValue("OpdNmTH", dtTransitHours.Rows.Count - 1, pObjTransitHoursRecord.OperatorName);
                dtTransitHours.SetValue("SupTH", dtTransitHours.Rows.Count - 1, pObjTransitHoursRecord.SupervisorId);
                dtTransitHours.SetValue("SupNmTH", dtTransitHours.Rows.Count - 1, pObjTransitHoursRecord.SupervisorName);

                mtxTransitHours.LoadFromDataSource();
                mtxTransitHours.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw lObjException;
            }
        }

        public void AddHoursRecord(HoursRecordsDTO pObjHoursRecord)
        {
            try
            {
                if (pObjHoursRecord == null)
                    return;

                dtHours.Rows.Add();
                dtHours.SetValue("#", dtHours.Rows.Count - 1, dtHours.Rows.Count);
                dtHours.SetValue("DateHrs", dtHours.Rows.Count - 1, pObjHoursRecord.DateHour == new DateTime(0001, 01, 01) ? DateTime.Now : pObjHoursRecord.DateHour);
                dtHours.SetValue("IdRiHrs", dtHours.Rows.Count - 1, pObjHoursRecord.IdRise);
                dtHours.SetValue("ContOVHrs", dtHours.Rows.Count - 1, pObjHoursRecord.ContractEntry);
                dtHours.SetValue("CDEOVHrs", dtHours.Rows.Count - 1, pObjHoursRecord.ContractDocNum);
                dtHours.SetValue("SupIdHrs", dtHours.Rows.Count - 1, pObjHoursRecord.SupervisorId);
                dtHours.SetValue("SupNmHrs", dtHours.Rows.Count - 1, pObjHoursRecord.Supervisor);
                dtHours.SetValue("OpdIdHrs", dtHours.Rows.Count - 1, pObjHoursRecord.OperatorId);
                dtHours.SetValue("OpdNmHrs", dtHours.Rows.Count - 1, pObjHoursRecord.OperatorName);
                dtHours.SetValue("EqpHrs", dtHours.Rows.Count - 1, pObjHoursRecord.PrcCode);
                dtHours.SetValue("NumEcnHrs", dtHours.Rows.Count - 1, pObjHoursRecord.EcoNum);
                dtHours.SetValue("HrsFeetHr", dtHours.Rows.Count - 1, pObjHoursRecord.HrFeet);
                dtHours.SetValue("SctnIdHrs", dtHours.Rows.Count - 1, pObjHoursRecord.SectionId);
                dtHours.SetValue("SctnNnHrs", dtHours.Rows.Count - 1, pObjHoursRecord.Section);
                dtHours.SetValue("KmHcHrs", dtHours.Rows.Count - 1, pObjHoursRecord.KmHt);
                dtHours.SetValue("PendHrs", dtHours.Rows.Count - 1, pObjHoursRecord.Pending);
                dtHours.SetValue("CloseHrs", dtHours.Rows.Count - 1, string.IsNullOrEmpty(pObjHoursRecord.Close) || pObjHoursRecord.Close == "0" ? "N" : "Y");
                //dtHours.SetValue("CloseHrsC", dtHours.Rows.Count - 1, string.IsNullOrEmpty(pObjHoursRecord.Close) || pObjHoursRecord.Close == "0" ? "Y");
                dtHours.SetValue("CodeHrs", dtHours.Rows.Count - 1, pObjHoursRecord.Code);
                dtHours.SetValue("Client", dtHours.Rows.Count - 1, pObjHoursRecord.ContractClient);

                mtxHours.LoadFromDataSource();
                mtxHours.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                throw lObjException;
            }
        }

        private void AddSupervisorsCbo()
        {
            try
            {
                SAPbouiCOM.Column lObjColumn = (SAPbouiCOM.Column)mtxHours.Columns.Item("ColSupH");
                SAPbouiCOM.Cells lObjCells = lObjColumn.Cells;

                if (lObjColumn.ValidValues.Count == 0)
                {
                    if (!string.IsNullOrEmpty(mStrSupervisorCode))
                    {
                        lObjColumn.ValidValues.Add(mStrSupervisorCode, txtSupervisor.Value);
                    }
                    for (int i = 0; i < dtEmployees.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dtEmployees.GetValue("EmpName", i).ToString()))
                        {
                            if (!ExistItemOnValidValues(lObjColumn, dtEmployees.GetValue("EmpId", i).ToString()))
                            {
                                lObjColumn.ValidValues.Add(dtEmployees.GetValue("EmpId", i).ToString(), dtEmployees.GetValue("EmpName", i).ToString());
                            }
                        }
                    }

                    lObjColumn.DisplayDesc = true;
                }

                lObjColumn.ComboSelectAfter += lObjColumn_ComboSelectAfter;
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el supervisor al listado: {0}", lObjException.Message));
            }
        }

        private void AddOperatorsCbo()
        {
            try
            {
                SAPbouiCOM.Column lObjColumn = (SAPbouiCOM.Column)mtxHours.Columns.Item("ColOprH");
                SAPbouiCOM.Cells lObjCells = lObjColumn.Cells;

                if (lObjColumn.ValidValues.Count == 0)
                {
                    if (!string.IsNullOrEmpty(mStrSupervisorCode))
                    {
                        lObjColumn.ValidValues.Add(mStrSupervisorCode, txtSupervisor.Value);
                    }
                    for (int i = 0; i < dtEmployees.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dtEmployees.GetValue("EmpName", i).ToString()))
                        {
                            if (!ExistItemOnValidValues(lObjColumn, dtEmployees.GetValue("EmpId", i).ToString()))
                            {
                                lObjColumn.ValidValues.Add(dtEmployees.GetValue("EmpId", i).ToString(), dtEmployees.GetValue("EmpName", i).ToString());
                            }
                        }
                    }

                    lObjColumn.DisplayDesc = true;
                }

                lObjColumn.ComboSelectAfter += lObjColumn_ComboSelectAfter;
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el supervisor al listado: {0}", lObjException.Message));
            }
        }

        private void AddOperatorsTHCbo()
        {
            try
            {
                SAPbouiCOM.Column lObjColumn = (SAPbouiCOM.Column)mtxTransitHours.Columns.Item("ColOpdHT");
                SAPbouiCOM.Column lObjColumn2 = (SAPbouiCOM.Column)mtxTransitHours.Columns.Item("ColSupHT");

                SAPbouiCOM.Cells lObjCells = lObjColumn.Cells;

                if (lObjColumn.ValidValues.Count == 0)
                {
                    if (!string.IsNullOrEmpty(mStrSupervisorCode))
                    {
                        lObjColumn.ValidValues.Add(mStrSupervisorCode, txtSupervisor.Value);
                        lObjColumn2.ValidValues.Add(mStrSupervisorCode, txtSupervisor.Value);
                    }
                    for (int i = 0; i < dtEmployees.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dtEmployees.GetValue("EmpName", i).ToString()))
                        {
                            if (!ExistItemOnValidValues(lObjColumn, dtEmployees.GetValue("EmpId", i).ToString()))
                            {
                                lObjColumn.ValidValues.Add(dtEmployees.GetValue("EmpId", i).ToString(), dtEmployees.GetValue("EmpName", i).ToString());
                            }

                            if (!ExistItemOnValidValues(lObjColumn2, dtEmployees.GetValue("EmpId", i).ToString()))
                            {
                                lObjColumn2.ValidValues.Add(dtEmployees.GetValue("EmpId", i).ToString(), dtEmployees.GetValue("EmpName", i).ToString());
                            }
                        }
                    }

                    lObjColumn.DisplayDesc = true;
                    lObjColumn2.DisplayDesc = true;
                }

                lObjColumn.ComboSelectAfter += lObjColumn_ComboSelectAfter;
                lObjColumn2.ComboSelectAfter += lObjColumn_ComboSelectAfter;
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar los operadores de Horas Tránsito al listado: {0}", lObjException.Message));
            }
        }

        private void AddTramosCbo(int pIntSODocEntry, int pIntIndexRow)
        {
            try
            {
                SAPbouiCOM.Column lObjColumn = (SAPbouiCOM.Column)mtxHours.Columns.Item("ColTramoH");
                SAPbouiCOM.Cells lObjCells = lObjColumn.Cells;

                lObjColumn.ComboSelectAfter += lObjColumn_ComboSelectAfter;
                lObjColumn.ClickBefore += lObjColumn_ComboClickBefore;

                //CleanCboItems(lObjColumn);

                //SAPbouiCOM.ComboBox lObjComboBox = (SAPbouiCOM.ComboBox)mtxHours.Columns.Item("ColTramoH").Cells.Item(pIntIndexRow).Specific;

                //List<SectionsDTO> lLstTramos = mObjMachineryServiceFactory.GetSectionsService().GetSectionsBySalesOrder(pIntSODocEntry);
                //foreach(var lObjTramo in lLstTramos) {
                //    if(lObjComboBox.ValidValues.Count == 0)
                //    {
                //        lObjComboBox.ValidValues.Add(lObjTramo.Code.ToString(), lObjTramo.Name);
                //    }

                //    bool lBolExist = false;
                //    foreach (SAPbouiCOM.ValidValue lObjValidValues in lObjComboBox.ValidValues)
                //    {
                //        if (lObjValidValues.Value == lObjTramo.Code.ToString())
                //        {
                //            lBolExist = true;
                //        }
                //    }

                //    if (!lBolExist)
                //        lObjComboBox.ValidValues.Add(lObjTramo.Code.ToString(), lObjTramo.Name);
                //}

                ////lObjComboBox.DisplayDesc = true;

                //lObjComboBox.ComboSelectAfter += lObjColumn_ComboSelectAfter;
                //lObjComboBox.ClickBefore += lObjColumn_ComboClickBefore;
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar los tramos del contrato: {0}", lObjException.Message));
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

        public void CleanCboItems(SAPbouiCOM.Column pObjCbo)
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

        private void AddMachineryHoursCbo()
        {
            try
            {
                SAPbouiCOM.Column lObjMachColumn = (SAPbouiCOM.Column)mtxHours.Columns.Item("ColEqmH");
                SAPbouiCOM.Column lObjEcoNumColumn = (SAPbouiCOM.Column)mtxHours.Columns.Item("ColNumEH");

                if (lObjMachColumn.ValidValues.Count == 0)
                {
                    for (int i = 0; i < dtInitialRcords.Rows.Count; i++) //dtInitialRcords
                    {
                        if (!string.IsNullOrEmpty(dtInitialRcords.GetValue("ActCodIR", i).ToString()))
                        {
                            string lStrEqmType = dtInitialRcords.GetValue("EqTypIR", i).ToString();

                            if (lStrEqmType.ToUpper() == "MQ")
                            {
                                lObjMachColumn.ValidValues.Add(dtInitialRcords.GetValue("ActCodIR", i).ToString(), dtInitialRcords.GetValue("ActNumIR", i).ToString());
                            }
                        }
                    }

                    lObjMachColumn.DisplayDesc = false;
                }

                if (lObjEcoNumColumn.ValidValues.Count == 0)
                {
                    for (int i = 0; i < dtInitialRcords.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dtInitialRcords.GetValue("ActNumIR", i).ToString()))
                        {
                            string lStrEqmType = dtInitialRcords.GetValue("EqTypIR", i).ToString();

                            if (lStrEqmType.ToUpper() == "MQ")
                            {
                                lObjEcoNumColumn.ValidValues.Add(dtInitialRcords.GetValue("ActCodIR", i).ToString(), dtInitialRcords.GetValue("ActNumIR", i).ToString());
                            }
                        }
                    }

                    lObjEcoNumColumn.DisplayDesc = true;
                }

                lObjMachColumn.ComboSelectAfter += lObjColumn_ComboSelectAfter;
                lObjEcoNumColumn.ComboSelectAfter += lObjColumn_ComboSelectAfter;
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el listado maquinarias en horas: {0}", lObjException.Message));
            }
        }

        private void AddContractsCbo()
        {
            try
            {
                SAPbouiCOM.Column lObjColumn = (SAPbouiCOM.Column)mtxHours.Columns.Item("ColCOVH");

                if (lObjColumn.ValidValues.Count == 0)
                {
                    for (int i = 0; i < dtContracts.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dtContracts.GetValue("DocNCont", i).ToString()))
                        {
                            lObjColumn.ValidValues.Add(dtContracts.GetValue("DocECont", i).ToString(), dtContracts.GetValue("DocNCont", i).ToString());
                        }
                    }

                    lObjColumn.DisplayDesc = true;
                }

                lObjColumn.ComboSelectAfter += lObjColumn_ComboSelectAfter;
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el listado de contratos: {0}", lObjException.Message));
            }
        }

        private void AddMachineryTransitHoursCbo()
        {
            try
            {
                SAPbouiCOM.Column lObjMachColumn = (SAPbouiCOM.Column)mtxTransitHours.Columns.Item("ColMaqHT");
                SAPbouiCOM.Column lObjEcoNumColumn = (SAPbouiCOM.Column)mtxTransitHours.Columns.Item("ColNEnHT");

                if (lObjMachColumn.ValidValues.Count == 0)
                {
                    for (int i = 0; i < dtInitialRcords.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dtInitialRcords.GetValue("ActCodIR", i).ToString()))
                        {
                            string lStrEqmType = dtInitialRcords.GetValue("EqTypIR", i).ToString();

                            if (lStrEqmType.ToUpper() == "MQ")
                            {
                                if (!ExistItemOnValidValues(lObjMachColumn, dtInitialRcords.GetValue("ActCodIR", i).ToString()))
                                {
                                    lObjMachColumn.ValidValues.Add(dtInitialRcords.GetValue("ActCodIR", i).ToString(), dtInitialRcords.GetValue("ActNumIR", i).ToString());
                                }
                            }
                        }
                    }

                    lObjMachColumn.DisplayDesc = false;
                }

                if (lObjEcoNumColumn.ValidValues.Count == 0)
                {
                    for (int i = 0; i < dtInitialRcords.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dtInitialRcords.GetValue("ActNumIR", i).ToString()))
                        {
                            string lStrEqmType = dtInitialRcords.GetValue("EqTypIR", i).ToString();

                            if (lStrEqmType.ToUpper() == "MQ")
                            {
                                if (!ExistItemOnValidValues(lObjEcoNumColumn, dtInitialRcords.GetValue("ActCodIR", i).ToString()))
                                {
                                    lObjEcoNumColumn.ValidValues.Add(dtInitialRcords.GetValue("ActCodIR", i).ToString(), dtInitialRcords.GetValue("ActNumIR", i).ToString());
                                }
                            }
                        }
                    }

                    lObjEcoNumColumn.DisplayDesc = true;
                }

                lObjMachColumn.ComboSelectAfter += lObjColumn_ComboSelectAfter;
                lObjEcoNumColumn.ComboSelectAfter += lObjColumn_ComboSelectAfter;
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el listado de maquinas en horas tránsito: {0}", lObjException.Message));
            }
        }

        private void CalculateTotal(SAPbouiCOM.EditText pTxtEditText, SAPbouiCOM.DataTable pDataTable, string pStrColName)
        {
            try
            {
                double lDblSubtotal = 0;

                for (int i = 0; i < pDataTable.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(pDataTable.GetValue(pStrColName, i).ToString()))
                    {
                        double lDblImport = double.Parse(pDataTable.GetValue(pStrColName, i).ToString());

                        lDblSubtotal += lDblImport;
                    }
                }

                pTxtEditText.Value = (lDblSubtotal).ToString();
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al calcular el total: {0}", lObjException.Message));
            }
        }
        #endregion

        #region Performance
        private void CalculatePerformance()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                DeletePerfomanceRecords();
                CalculateMachineryPerformance();
                CalculateVehiclesPerformance();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - CalculatePerformance] Error: {0}", lObjException.Message));
                throw new Exception(lObjException.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void CalculateMachineryPerformance()
        {
            try
            {
                List<PerformanceDTO> lLstMachineryPerformance = mObjMachineryServiceFactory.GetPerformanceService().CalculateMachineryPerformance(
                                                                    mObjMachineryServiceFactory.GetConsumablesService().TotalsRecordsDataTableToDTO(dtConsumedTotals).Where(x => x.EquipmentType == "MQ").ToList(),
                                                                    mObjMachineryServiceFactory.GetHoursRecordsService().DataTableToDTO(dtHours).Where(x => !string.IsNullOrEmpty(x.PrcCode) && !string.IsNullOrEmpty(x.EcoNum)).ToList(),
                                                                    mObjMachineryServiceFactory.GetTransitHoursRecordsService().DataTableToDTO(dtTransitHours).Where(x => !string.IsNullOrEmpty(x.PrcCode) && !string.IsNullOrEmpty(x.EcoNum)).ToList());

                ClearMatrix(dtMachineryPerformance.UniqueID, mtxMachPerformance);

                foreach (var lObjPerf in lLstMachineryPerformance)
                {
                    AddPerformanceRecord(lObjPerf);
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - CalculateMachineryPerformance] Error: {0}", lObjException.Message));
                throw new Exception(lObjException.Message);
            }
        }

        private void CalculateVehiclesPerformance()
        {
            try
            {
                List<PerformanceDTO> lLstMachineryPerformance = mObjMachineryServiceFactory.GetPerformanceService().CalculateVehiclePerformance(
                                                                    mObjMachineryServiceFactory.GetConsumablesService()
                                                                    .TotalsRecordsDataTableToDTO(dtConsumedTotals).Where(x => x.EquipmentType == "VL").ToList());

                ClearMatrix(dtVehiclePerformance.UniqueID, mtxVclPerformance);

                foreach (var lObjPerf in lLstMachineryPerformance)
                {
                    AddPerformanceRecord(lObjPerf);
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - CalculateVehiclesPerformance] Error: {0}", lObjException.Message));
                throw new Exception(lObjException.Message);
            }
            finally
            {

            }
        }

        public void LoadPerformance()
        {
            try
            {
                List<PerformanceDTO> lLstPerfomance = mObjMachineryServiceFactory.GetPerformanceService().GetByRiseId(mObjRise.IdRise);

                foreach (var lObjPerformance in lLstPerfomance)
                {
                    AddPerformanceRecord(lObjPerformance);
                }
            }
            catch (Exception lObjException)
            {

                throw new Exception(string.Format("Error al obtener la información del rendimiento: {0}", lObjException.Message));
            }
        }

        public void DeletePerfomanceRecords()
        {
            //Vehículos
            for (int i = 0; i < dtVehiclePerformance.Rows.Count; i++)
            {
                string lStrCode = dtVehiclePerformance.GetValue("CodePFV", i).ToString();
                mObjMachineryServiceFactory.GetPerformanceService().Remove(lStrCode);
            }

            //Máquinas
            for (int i = 0; i < dtMachineryPerformance.Rows.Count; i++)
            {
                string lStrCode = dtMachineryPerformance.GetValue("CodePFM", i).ToString();
                mObjMachineryServiceFactory.GetPerformanceService().Remove(lStrCode);
            }
        }

        public void LoadRisePerformance(int pIntFolio, RiseStatusEnum pEnmRiseStatus = RiseStatusEnum.Active)
        {
            try
            {
                List<PerformanceDTO> lLstPerformance = new List<PerformanceDTO>();
                //if (pEnmRiseStatus == RiseStatusEnum.Active)
                //{
                //    //lLstPerformance = mObjMachineryServiceFactory.GetConsumablesService().GetConsumableRequestDocByRiseId(pIntFolio);
                //}
                //else
                //{
                //    lLstPerformance = mObjMachineryServiceFactory.GetPerformanceService().GetByRiseId(pIntFolio);
                //}

                lLstPerformance = mObjMachineryServiceFactory.GetPerformanceService().GetByRiseId(pIntFolio);

                foreach (var lObjPerformance in lLstPerformance)
                {
                    AddPerformanceRecord(lObjPerformance);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al obtener el rendimineto de la subida: {0}", lObjException.Message));
            }
        }

        public void AddPerformanceRecord(PerformanceDTO pObjPerformanceRecord)
        {
            try
            {
                if (pObjPerformanceRecord == null)
                    return;

                if (pObjPerformanceRecord.Type == 1) //maquinaria
                {
                    dtMachineryPerformance.Rows.Add();
                    dtMachineryPerformance.SetValue("#", dtMachineryPerformance.Rows.Count - 1, dtMachineryPerformance.Rows.Count);
                    dtMachineryPerformance.SetValue("IdRisePFM", dtMachineryPerformance.Rows.Count - 1, mObjRise.IdRise);
                    dtMachineryPerformance.SetValue("MaqPFM", dtMachineryPerformance.Rows.Count - 1, pObjPerformanceRecord.PrcCode);
                    dtMachineryPerformance.SetValue("NumEcoPFM", dtMachineryPerformance.Rows.Count - 1, pObjPerformanceRecord.EcoNum);
                    dtMachineryPerformance.SetValue("TypeIdPFM", dtMachineryPerformance.Rows.Count - 1, pObjPerformanceRecord.Type);
                    dtMachineryPerformance.SetValue("HrsKmPFM", dtMachineryPerformance.Rows.Count - 1, pObjPerformanceRecord.HrKm);
                    dtMachineryPerformance.SetValue("PerfPFM", dtMachineryPerformance.Rows.Count - 1, pObjPerformanceRecord.PerformanceF);
                    dtMachineryPerformance.SetValue("CodePFM", dtMachineryPerformance.Rows.Count - 1, pObjPerformanceRecord.Code);

                    mtxMachPerformance.LoadFromDataSource();
                    mtxMachPerformance.AutoResizeColumns();
                }
                else //vehículos
                {
                    dtVehiclePerformance.Rows.Add();
                    dtVehiclePerformance.SetValue("#", dtVehiclePerformance.Rows.Count - 1, dtVehiclePerformance.Rows.Count);
                    dtVehiclePerformance.SetValue("IdRisePFV", dtVehiclePerformance.Rows.Count - 1, mObjRise.IdRise);
                    dtVehiclePerformance.SetValue("MaqPFV", dtVehiclePerformance.Rows.Count - 1, pObjPerformanceRecord.PrcCode);
                    dtVehiclePerformance.SetValue("NumEcoPFV", dtVehiclePerformance.Rows.Count - 1, pObjPerformanceRecord.EcoNum);
                    dtVehiclePerformance.SetValue("TypeIdPFV", dtVehiclePerformance.Rows.Count - 1, pObjPerformanceRecord.Type);
                    dtVehiclePerformance.SetValue("HrsKmPFV", dtVehiclePerformance.Rows.Count - 1, pObjPerformanceRecord.HrKm);
                    dtVehiclePerformance.SetValue("PerfPFV", dtVehiclePerformance.Rows.Count - 1, pObjPerformanceRecord.PerformanceF);
                    dtVehiclePerformance.SetValue("CodePFV", dtVehiclePerformance.Rows.Count - 1, pObjPerformanceRecord.Code);

                    mtxVclPerformance.LoadFromDataSource();
                    mtxVclPerformance.AutoResizeColumns();
                }

                /*
                 * dtMachineryPerformance = this.UIAPIRawForm.DataSources.DataTables.Item("DTMPerformance");
            dtMachineryPerformance.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtMachineryPerformance.Columns.Add("IdRisePFM", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtMachineryPerformance.Columns.Add("MaqPFM", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtMachineryPerformance.Columns.Add("NumEcoPFM", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtMachineryPerformance.Columns.Add("TypeIdPFM", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtMachineryPerformance.Columns.Add("HrsKmPFM", SAPbouiCOM.BoFieldsType.ft_Float);
            dtMachineryPerformance.Columns.Add("PerfPFM", SAPbouiCOM.BoFieldsType.ft_Float);
            dtMachineryPerformance.Columns.Add("CodePFM", SAPbouiCOM.BoFieldsType.ft_Float);

            this.UIAPIRawForm.DataSources.DataTables.Add("DTVPerformance");
            dtVehiclePerformance = this.UIAPIRawForm.DataSources.DataTables.Item("DTVPerformance");
            dtVehiclePerformance.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtVehiclePerformance.Columns.Add("IdRisePFV", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtVehiclePerformance.Columns.Add("MaqPFV", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtVehiclePerformance.Columns.Add("NumEcoPFV", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtVehiclePerformance.Columns.Add("TypeIdPFV", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtVehiclePerformance.Columns.Add("HrsKmPFV", SAPbouiCOM.BoFieldsType.ft_Float);
            dtVehiclePerformance.Columns.Add("PerfPFV", SAPbouiCOM.BoFieldsType.ft_Float);
            dtVehiclePerformance.Columns.Add("CodePFV", SAPbouiCOM.BoFieldsType.ft_Float);
                 */
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar el registro de rendimiento de maquinaria {0}", lObjException.Message));
            }
        }
        #endregion

        #region Search mode
        private void StartSearchMode()
        {
            try
            {
                //if (SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("¿Desea entrar en modo búsqueda y perder los datos actuales?", 2, "Si", "No", "") == 1){}

                if (string.IsNullOrEmpty(txtFolio.Value))
                {
                    return;
                }
                else
                {
                    int n;
                    if (!int.TryParse(txtFolio.Value, out n))
                    {
                        UIApplication.ShowError("Solo puede ingresar valores numéricos en el folio de la subida");
                        return;
                    }
                    else
                    {
                        if (n <= 0)
                        {
                            UIApplication.ShowError("El folio de la subida debe ser mayor a 0");
                            return;
                        }
                    }
                }

                //this.UIAPIRawForm.Freeze(true);

                RiseDTO lObjRise = mObjMachineryServiceFactory.GetRiseService().GetRiseById(int.Parse(txtFolio.Value));
                if (lObjRise == null)
                {
                    UIApplication.ShowError("La subida no existe");
                    return;
                }

                mBolForSearchMode = false;
                UIApplication.GetApplication().Forms.ActiveForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;

                mObjRise = mObjMachineryServiceFactory.GetRiseService().ToEntity(lObjRise);

                LoadControlsForSeachMode(lObjRise);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - StartSearchMode] Error: {0}", lObjException.Message));
                throw new Exception(lObjException.Message);
            }
            finally
            {
                //this.UIAPIRawForm.Freeze(false);
            }
        }

        private void LoadControlsForSeachMode(RiseDTO pObjRise)
        {
            try
            {
                //txtFolio.Value = lObjRise.IdRise.ToString();
                txtDate.Value = pObjRise.CreatedDate.ToString("dd/MM/yyyy");

                mStrClientCode = pObjRise.Client;
                this.UIAPIRawForm.DataSources.UserDataSources.Item("CFL_0").ValueEx = pObjRise.ClientName;

                mStrSupervisorCode = pObjRise.SupervisorId.ToString();
                this.UIAPIRawForm.DataSources.UserDataSources.Item("CFL_Supv").ValueEx = pObjRise.SupervisorName;

                txtFolioRelation.Value = pObjRise.FolioRelation.ToString() == "0" ? string.Empty : pObjRise.FolioRelation.ToString();
                txtStatus.Value = pObjRise.DocStatus.GetDescription();
                txtStartDate.Value = pObjRise.StartDate.ToString("yyyyMMdd");
                txtEndDate.Value = pObjRise.EndDate.ToString("yyyyMMdd");

                txtSupervisor.Item.Enabled = true;
                txtSupervisor.Item.Click();
                txtFolio.Item.Enabled = false;
                txtClient.Item.Enabled = true;
                txtStartDate.Item.Enabled = true;
                txtEndDate.Item.Enabled = true;
                txtTotalsHours.Item.Enabled = true;
                txtKmHrTotal.Item.Enabled = true;
                txtFootHoursTotal.Item.Enabled = true;

                LoadRiseOperators(pObjRise.IdRise);
                LoadRiseContracts(pObjRise.IdRise);
                LoadRiseConsumablesRequest(pObjRise.IdRise, pObjRise.DocStatus);
                LoadRiseTravelExpenses(pObjRise.IdRise, pObjRise.DocStatus);
                LoadInitialRecords(pObjRise.IdRise, pObjRise.DocStatus);
                LoadPurchasesRecords(pObjRise.IdRise, pObjRise.DocStatus);
                LoadFinalsRecords(pObjRise.IdRise, pObjRise.DocStatus);
                LoadConsumedTotals(pObjRise.IdRise, pObjRise.DocStatus);
                LoadRisePerformance(pObjRise.IdRise, pObjRise.DocStatus);
                LoadHoursRecords(pObjRise.IdRise, pObjRise.DocStatus);
                LoadTransitHoursRecords(pObjRise.IdRise, pObjRise.DocStatus);
                //LoadPerformance();

                CalculateTotal(txtFootHoursTotal, dtHours, "HrsFeetHr");
                CalculateTotal(txtKmHrTotal, dtHours, "KmHcHrs");
                CalculateTotal(txtTotalsHours, dtTransitHours, "HrsTH");

                switch (pObjRise.DocStatus)
                {
                    case RiseStatusEnum.Active:
                        UIApplication.GetApplication().Forms.ActiveForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                        UnBlockControls();

                        if (mObjMachineryServiceFactory.GetGoodIssuesService().ExistsGoodIssue(pObjRise.IdRise))
                        {
                            mtxInitialRecords.Item.Enabled = false;
                            mtxPurchase.Item.Enabled = false;
                            mtxFinalRecords.Item.Enabled = false;
                            mtxConsumedTotals.Item.Enabled = false;
                            btnSaveIR.Item.Enabled = false;
                        }
                        break;
                    case RiseStatusEnum.Close:
                        UIApplication.GetApplication().Forms.ActiveForm.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE;
                        ShowOrHideOpenBtn();
                        break;
                    case RiseStatusEnum.Cancelled:
                        UIApplication.GetApplication().Forms.ActiveForm.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE;
                        break;
                    case RiseStatusEnum.ReOpen:
                        UIApplication.GetApplication().Forms.ActiveForm.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE;
                        ShowOrHideOpenBtn();
                        UnBlockControlsForReopenRise();
                        //AddMtrixEmpoyeesDefaultRow();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[MachineryForm - LoadControlsForSeachMode] Error: {0}", lObjException.Message));
                throw new Exception(lObjException.Message);
            }
        }

        private void CleanControls(bool pBolForSearchMode = false)
        {
            if (pBolForSearchMode)
            {
                mBolForSearchMode = true;

                txtFolio.Item.Enabled = true;
                txtFolio.Item.Click();

                txtSupervisor.Item.Enabled = false;
                txtClient.Item.Enabled = false;
                txtStartDate.Item.Enabled = false;
                txtEndDate.Item.Enabled = false;
                txtTotalsHours.Item.Enabled = false;
                txtKmHrTotal.Item.Enabled = false;
                txtFootHoursTotal.Item.Enabled = false;
            }

            txtTotalsHours.Value = string.Empty;
            txtKmHrTotal.Value = string.Empty;
            txtFootHoursTotal.Value = string.Empty;

            //Global variables
            mObjRise = null;
            mLstContractsToDelete.Clear();
            mLstEmployeesToDelete.Clear();

            //Txt folio rise
            txtFolio.Value = string.Empty;

            //Txt date
            txtDate.Value = string.Empty;
            txtStartDate.Value = string.Empty;
            txtEndDate.Value = string.Empty;

            //Txt supervisor
            txtSupervisor.Value = string.Empty;
            mStrSupervisorCode = string.Empty;

            //Txt client
            txtClient.Value = string.Empty;
            mStrClientCode = string.Empty;

            //Txt folio relation
            txtFolioRelation.Value = string.Empty;
            mStrRelationFolio = string.Empty;

            //Txt status
            txtStatus.Value = string.Empty;

            //General tab
            ClearMatrix("DTEmp", mtxEmployees);
            ClearMatrix("DTCons", mtxConsumables);
            ClearMatrix("DTCont", mtxContracts);
            ClearMatrix("DTTravExp", mtxTravelExpenses);

            //Consumables tab
            ClearMatrix("DTIniRds", mtxInitialRecords);
            ClearMatrix("DTPrcInv", mtxPurchase);
            ClearMatrix("DTFinalR", mtxFinalRecords);
            ClearMatrix("DTTotalR", mtxConsumedTotals);

            //Hours tab
            ClearMatrix("DTHours", mtxHours);
            ClearMatrix("DTTransH", mtxTransitHours);

            //Transit hours tab
            ClearMatrix("DTMPerformance", mtxHours);
            ClearMatrix("DTVPerformance", mtxMachPerformance);
        }
        #endregion

        #region General tab
        #region Matrix
        private void ClearMatrix(string pStrDTName, SAPbouiCOM.Matrix pObjMatrix)
        {
            if (!this.UIAPIRawForm.DataSources.DataTables.Item(pStrDTName).IsEmpty)
            {
                this.UIAPIRawForm.DataSources.DataTables.Item(pStrDTName).Rows.Clear();
                pObjMatrix.Clear();
            }
        }

        public void InitColumnCFLEmployees()
        {
            SAPbouiCOM.Column lObjColumnItem = mtxEmployees.Columns.Item("ColId");
            lObjColumnItem.DataBind.SetBound(true, "", "CFL_Emp");
            lObjColumnItem.ChooseFromListUID = "CFL_Emp";
        }

        private void CreateDatatableEmployees()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTEmp");
            dtEmployees = this.UIAPIRawForm.DataSources.DataTables.Item("DTEmp");
            dtEmployees.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtEmployees.Columns.Add("EmpId", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtEmployees.Columns.Add("EmpName", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtEmployees.Columns.Add("CodeTEmp", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtEmployees.Columns.Add("EmpChk", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            InitColumnCFLEmployees();
            FillMatrixEmployees();
        }

        private void CreateDatatableConsumables()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTCons");
            dtConsumables = this.UIAPIRawForm.DataSources.DataTables.Item("DTCons");
            dtConsumables.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtConsumables.Columns.Add("CodeTCons", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtConsumables.Columns.Add("DateCons", SAPbouiCOM.BoFieldsType.ft_Date);
            dtConsumables.Columns.Add("FolRCons", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtConsumables.Columns.Add("FolTCons", SAPbouiCOM.BoFieldsType.ft_Integer);

            FillMatrixConsumables();
        }

        private void CreateContractsDatatable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTCont");
            dtContracts = this.UIAPIRawForm.DataSources.DataTables.Item("DTCont");
            dtContracts.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtContracts.Columns.Add("DocECont", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtContracts.Columns.Add("DocNCont", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtContracts.Columns.Add("DateCont", SAPbouiCOM.BoFieldsType.ft_Date);
            dtContracts.Columns.Add("TypeCont", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtContracts.Columns.Add("StsCont", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtContracts.Columns.Add("TypCCont", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtContracts.Columns.Add("StsCCont", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtContracts.Columns.Add("ImpCont", SAPbouiCOM.BoFieldsType.ft_Price);
            dtContracts.Columns.Add("CodeTCont", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtContracts.Columns.Add("CardName", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtContracts.Columns.Add("MunpId", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtContracts.Columns.Add("Munp", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillContractsMatrix();
        }

        private void CreateTravelExpensesDatatable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTTravExp");
            dtTravelExpenses = this.UIAPIRawForm.DataSources.DataTables.Item("DTTravExp");
            dtTravelExpenses.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtTravelExpenses.Columns.Add("DateTravE", SAPbouiCOM.BoFieldsType.ft_Date);
            dtTravelExpenses.Columns.Add("FolTravE", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTravelExpenses.Columns.Add("DocETravE", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtTravelExpenses.Columns.Add("DocNTravE", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtTravelExpenses.Columns.Add("ImpTravE", SAPbouiCOM.BoFieldsType.ft_Price);
            dtTravelExpenses.Columns.Add("StsTravE", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTravelExpenses.Columns.Add("StsCTravE", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtTravelExpenses.Columns.Add("CodeTTrav", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillTravelExpensesMatrix();
        }

        private void CreateInitialRecordsDataTable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTIniRds");
            dtInitialRcords = this.UIAPIRawForm.DataSources.DataTables.Item("DTIniRds");
            dtInitialRcords.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            /*dtInitialRcords.Columns.Add("DocEIR", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtInitialRcords.Columns.Add("DocNIR", SAPbouiCOM.BoFieldsType.ft_Integer);*/
            dtInitialRcords.Columns.Add("IdRiIR", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtInitialRcords.Columns.Add("ActCodIR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtInitialRcords.Columns.Add("ActNumIR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtInitialRcords.Columns.Add("DslMIR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtInitialRcords.Columns.Add("DslTIR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtInitialRcords.Columns.Add("GasIR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtInitialRcords.Columns.Add("15W40IR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtInitialRcords.Columns.Add("HidIR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtInitialRcords.Columns.Add("SAE40IR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtInitialRcords.Columns.Add("TransIR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtInitialRcords.Columns.Add("OilsIR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtInitialRcords.Columns.Add("KmHrIR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtInitialRcords.Columns.Add("CodeTIR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtInitialRcords.Columns.Add("EqTypIR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillInitialsRecordsMatrix();
        }

        private void CreatePurchInvReqDataTable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTPrcInv");
            dtPurchase = this.UIAPIRawForm.DataSources.DataTables.Item("DTPrcInv");
            dtPurchase.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            /*dtPurchase.Columns.Add("DocEPrch", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtPurchase.Columns.Add("DocNPrch", SAPbouiCOM.BoFieldsType.ft_Integer);*/
            dtPurchase.Columns.Add("DocTyPrch", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtPurchase.Columns.Add("DocTyDPrc", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtPurchase.Columns.Add("IdRiPrch", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtPurchase.Columns.Add("ActCodPrc", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtPurchase.Columns.Add("ActNumPrc", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtPurchase.Columns.Add("GasPrch", SAPbouiCOM.BoFieldsType.ft_Float);
            dtPurchase.Columns.Add("15W40Prch", SAPbouiCOM.BoFieldsType.ft_Float);
            dtPurchase.Columns.Add("HidPrch", SAPbouiCOM.BoFieldsType.ft_Float);
            dtPurchase.Columns.Add("SAE40Prch", SAPbouiCOM.BoFieldsType.ft_Float);
            dtPurchase.Columns.Add("TransPrch", SAPbouiCOM.BoFieldsType.ft_Float);
            dtPurchase.Columns.Add("OilsPrch", SAPbouiCOM.BoFieldsType.ft_Float);
            dtPurchase.Columns.Add("CodeTPrch", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtPurchase.Columns.Add("EqTypPrch", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtPurchase.Columns.Add("DslTPrch", SAPbouiCOM.BoFieldsType.ft_Float);//ColDTPurc

            FillPurchReqInvMatrix();
        }

        private void CreateFinalRecordsDataTable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTFinalR");
            dtFinalRecords = this.UIAPIRawForm.DataSources.DataTables.Item("DTFinalR");
            dtFinalRecords.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtFinalRecords.Columns.Add("IdRiFR", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtFinalRecords.Columns.Add("ActCodFR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtFinalRecords.Columns.Add("ActNumFR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtFinalRecords.Columns.Add("DslMFR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtFinalRecords.Columns.Add("DslTFR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtFinalRecords.Columns.Add("GasFR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtFinalRecords.Columns.Add("15W40FR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtFinalRecords.Columns.Add("HidFR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtFinalRecords.Columns.Add("SAE40FR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtFinalRecords.Columns.Add("TransFR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtFinalRecords.Columns.Add("OilsFR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtFinalRecords.Columns.Add("KmHrFR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtFinalRecords.Columns.Add("CodeTFR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtFinalRecords.Columns.Add("EqTypFR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillFinalsRecordsMatrix();
        }

        private void CreateTotalsRecordsDataTable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTTotalR");
            dtConsumedTotals = this.UIAPIRawForm.DataSources.DataTables.Item("DTTotalR");
            dtConsumedTotals.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtConsumedTotals.Columns.Add("IdRiTR", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtConsumedTotals.Columns.Add("ActCodTR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtConsumedTotals.Columns.Add("ActNumTR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtConsumedTotals.Columns.Add("DslMTR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtConsumedTotals.Columns.Add("DslTTR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtConsumedTotals.Columns.Add("GasTR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtConsumedTotals.Columns.Add("15W40TR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtConsumedTotals.Columns.Add("HidTR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtConsumedTotals.Columns.Add("SAE40TR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtConsumedTotals.Columns.Add("TransTR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtConsumedTotals.Columns.Add("OilsTR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtConsumedTotals.Columns.Add("KmHrTR", SAPbouiCOM.BoFieldsType.ft_Float);
            dtConsumedTotals.Columns.Add("CodeTTR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtConsumedTotals.Columns.Add("EqTypTR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillTotalsRecordsMatrix();
        }

        private void CreateHoursDataTable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTHours");
            dtHours = this.UIAPIRawForm.DataSources.DataTables.Item("DTHours");
            dtHours.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtHours.Columns.Add("DateHrs", SAPbouiCOM.BoFieldsType.ft_Date);
            dtHours.Columns.Add("IdRiHrs", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtHours.Columns.Add("ContOVHrs", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtHours.Columns.Add("CDEOVHrs", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtHours.Columns.Add("SupIdHrs", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtHours.Columns.Add("SupNmHrs", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtHours.Columns.Add("OpdIdHrs", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtHours.Columns.Add("OpdNmHrs", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtHours.Columns.Add("EqpHrs", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtHours.Columns.Add("NumEcnHrs", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtHours.Columns.Add("HrsFeetHr", SAPbouiCOM.BoFieldsType.ft_Float);
            dtHours.Columns.Add("SctnIdHrs", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtHours.Columns.Add("SctnNnHrs", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtHours.Columns.Add("KmHcHrs", SAPbouiCOM.BoFieldsType.ft_Float);
            dtHours.Columns.Add("PendHrs", SAPbouiCOM.BoFieldsType.ft_Float);
            dtHours.Columns.Add("CloseHrs", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtHours.Columns.Add("CloseHrsC", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtHours.Columns.Add("CodeHrs", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtHours.Columns.Add("Client", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillHoursRecordsMatrix();
        }

        private void CreateTransitHoursDataTable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTTransH");
            dtTransitHours = this.UIAPIRawForm.DataSources.DataTables.Item("DTTransH");
            dtTransitHours.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtTransitHours.Columns.Add("MaqTHrs", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTransitHours.Columns.Add("NumEcoTH", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTransitHours.Columns.Add("IdRiTHrs", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtTransitHours.Columns.Add("HrsTH", SAPbouiCOM.BoFieldsType.ft_Float);
            dtTransitHours.Columns.Add("CodeTH", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTransitHours.Columns.Add("OpdTH", SAPbouiCOM.BoFieldsType.ft_Integer); //ColOpdHT
            dtTransitHours.Columns.Add("OpdNmTH", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTransitHours.Columns.Add("SupTH", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTransitHours.Columns.Add("SupNmTH", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillTransitHoursRecordsMatrix();
        }

        private void CreateMachineryPerformanceDataTable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTMPerformance");
            dtMachineryPerformance = this.UIAPIRawForm.DataSources.DataTables.Item("DTMPerformance");
            dtMachineryPerformance.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtMachineryPerformance.Columns.Add("IdRisePFM", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtMachineryPerformance.Columns.Add("MaqPFM", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtMachineryPerformance.Columns.Add("NumEcoPFM", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtMachineryPerformance.Columns.Add("TypeIdPFM", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtMachineryPerformance.Columns.Add("HrsKmPFM", SAPbouiCOM.BoFieldsType.ft_Float);
            dtMachineryPerformance.Columns.Add("PerfPFM", SAPbouiCOM.BoFieldsType.ft_Float);
            dtMachineryPerformance.Columns.Add("CodePFM", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillMachineryPerfRecordsMatrix();
        }

        private void CreateVehiclePerformanceDataTable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTVPerformance");
            dtVehiclePerformance = this.UIAPIRawForm.DataSources.DataTables.Item("DTVPerformance");
            dtVehiclePerformance.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtVehiclePerformance.Columns.Add("IdRisePFV", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtVehiclePerformance.Columns.Add("MaqPFV", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtVehiclePerformance.Columns.Add("NumEcoPFV", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtVehiclePerformance.Columns.Add("TypeIdPFV", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtVehiclePerformance.Columns.Add("HrsKmPFV", SAPbouiCOM.BoFieldsType.ft_Float);
            dtVehiclePerformance.Columns.Add("PerfPFV", SAPbouiCOM.BoFieldsType.ft_Float);
            dtVehiclePerformance.Columns.Add("CodePFV", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillVehiclePerfRecordsMatrix();
        }

        private void FillVehiclePerfRecordsMatrix()
        {
            mtxVclPerformance.Columns.Item("#").DataBind.Bind("DTVPerformance", "#");
            mtxVclPerformance.Columns.Item("ColMqnVcl").DataBind.Bind("DTVPerformance", "MaqPFV"); //cmb
            mtxVclPerformance.Columns.Item("ColNEVcl").DataBind.Bind("DTVPerformance", "NumEcoPFV");
            mtxVclPerformance.Columns.Item("ColKmVcl").DataBind.Bind("DTVPerformance", "HrsKmPFV");
            mtxVclPerformance.Columns.Item("ColRenVcl").DataBind.Bind("DTVPerformance", "PerfPFV");

            mtxVclPerformance.AutoResizeColumns();
        }

        private void FillMachineryPerfRecordsMatrix()
        {
            mtxMachPerformance.Columns.Item("#").DataBind.Bind("DTMPerformance", "#");
            mtxMachPerformance.Columns.Item("ColMqnRen").DataBind.Bind("DTMPerformance", "MaqPFM"); //cmb
            mtxMachPerformance.Columns.Item("ColENRen").DataBind.Bind("DTMPerformance", "NumEcoPFM");
            mtxMachPerformance.Columns.Item("ColHrsRen").DataBind.Bind("DTMPerformance", "HrsKmPFM");
            mtxMachPerformance.Columns.Item("ColRenRen").DataBind.Bind("DTMPerformance", "PerfPFM");

            mtxMachPerformance.AutoResizeColumns();
        }

        private void FillTransitHoursRecordsMatrix()
        {
            mtxTransitHours.Columns.Item("#").DataBind.Bind("DTTransH", "#");
            mtxTransitHours.Columns.Item("ColMaqHT").DataBind.Bind("DTTransH", "MaqTHrs"); //cmb
            mtxTransitHours.Columns.Item("ColNEnHT").DataBind.Bind("DTTransH", "NumEcoTH");
            mtxTransitHours.Columns.Item("ColHrsHT").DataBind.Bind("DTTransH", "HrsTH");
            mtxTransitHours.Columns.Item("ColSupHT").DataBind.Bind("DTTransH", "SupTH");
            mtxTransitHours.Columns.Item("ColOpdHT").DataBind.Bind("DTTransH", "OpdTH"); //ColOpdHT

            mtxTransitHours.AutoResizeColumns();
        }

        private void FillHoursRecordsMatrix()
        {
            mtxHours.Columns.Item("#").DataBind.Bind("DTHours", "#");
            mtxHours.Columns.Item("ColDateH").DataBind.Bind("DTHours", "DateHrs");
            mtxHours.Columns.Item("ColCOVH").DataBind.Bind("DTHours", "ContOVHrs"); //cmb
            mtxHours.Columns.Item("ColSupH").DataBind.Bind("DTHours", "SupNmHrs"); //cmb
            mtxHours.Columns.Item("ColOprH").DataBind.Bind("DTHours", "OpdNmHrs"); //cmb
            mtxHours.Columns.Item("ColEqmH").DataBind.Bind("DTHours", "EqpHrs"); //cmb
            mtxHours.Columns.Item("ColNumEH").DataBind.Bind("DTHours", "NumEcnHrs"); //cmb
            mtxHours.Columns.Item("ColHPH").DataBind.Bind("DTHours", "HrsFeetHr");
            mtxHours.Columns.Item("ColTramoH").DataBind.Bind("DTHours", "SctnNnHrs"); //cmb
            mtxHours.Columns.Item("ColKmHrH").DataBind.Bind("DTHours", "KmHcHrs");
            mtxHours.Columns.Item("ColPendH").DataBind.Bind("DTHours", "PendHrs");
            mtxHours.Columns.Item("ColCloseH").DataBind.Bind("DTHours", "CloseHrs");
            mtxHours.Columns.Item("ColClient").DataBind.Bind("DTHours", "Client");

            //mtxHours.Columns.Item("ColTramoH").ClickBefore += lObjColumn_ComboClickBefore;

            mtxHours.AutoResizeColumns();
        }

        private void FillTotalsRecordsMatrix()
        {
            mtxConsumedTotals.Columns.Item("#").DataBind.Bind("DTTotalR", "#");
            mtxConsumedTotals.Columns.Item("ColMacTot").DataBind.Bind("DTTotalR", "ActCodTR");
            mtxConsumedTotals.Columns.Item("ColNEnTot").DataBind.Bind("DTTotalR", "ActNumTR");
            mtxConsumedTotals.Columns.Item("ColDlMTot").DataBind.Bind("DTTotalR", "DslMTR");
            mtxConsumedTotals.Columns.Item("ColDlTTot").DataBind.Bind("DTTotalR", "DslTTR");
            mtxConsumedTotals.Columns.Item("ColGasTot").DataBind.Bind("DTTotalR", "GasTR");
            mtxConsumedTotals.Columns.Item("Col15WTot").DataBind.Bind("DTTotalR", "15W40TR");
            mtxConsumedTotals.Columns.Item("ColHdlTot").DataBind.Bind("DTTotalR", "HidTR");
            mtxConsumedTotals.Columns.Item("ColSAETot").DataBind.Bind("DTTotalR", "SAE40TR");
            mtxConsumedTotals.Columns.Item("ColTrmTot").DataBind.Bind("DTTotalR", "TransTR");
            mtxConsumedTotals.Columns.Item("ColGrsTot").DataBind.Bind("DTTotalR", "OilsTR");
            mtxConsumedTotals.Columns.Item("ColKmhTot").DataBind.Bind("DTTotalR", "KmHrTR");
            mtxConsumedTotals.Columns.Item("ColEqTTot").DataBind.Bind("DTTotalR", "EqTypTR");

            mtxConsumedTotals.AutoResizeColumns();
        }

        private void FillFinalsRecordsMatrix()
        {
            mtxFinalRecords.Columns.Item("#").DataBind.Bind("DTFinalR", "#");
            mtxFinalRecords.Columns.Item("ColMacFR").DataBind.Bind("DTFinalR", "ActCodFR");
            mtxFinalRecords.Columns.Item("ColNEnFR").DataBind.Bind("DTFinalR", "ActNumFR");
            mtxFinalRecords.Columns.Item("ColDlMFR").DataBind.Bind("DTFinalR", "DslMFR");
            mtxFinalRecords.Columns.Item("ColDlTFR").DataBind.Bind("DTFinalR", "DslTFR");
            mtxFinalRecords.Columns.Item("ColGasFR").DataBind.Bind("DTFinalR", "GasFR");
            mtxFinalRecords.Columns.Item("Col15WFR").DataBind.Bind("DTFinalR", "15W40FR");
            mtxFinalRecords.Columns.Item("ColHdlFR").DataBind.Bind("DTFinalR", "HidFR");
            mtxFinalRecords.Columns.Item("ColSAEFR").DataBind.Bind("DTFinalR", "SAE40FR");
            mtxFinalRecords.Columns.Item("ColTrmFR").DataBind.Bind("DTFinalR", "TransFR");
            mtxFinalRecords.Columns.Item("ColGrsFR").DataBind.Bind("DTFinalR", "OilsFR");
            mtxFinalRecords.Columns.Item("ColKmhFR").DataBind.Bind("DTFinalR", "KmHrFR");
            mtxFinalRecords.Columns.Item("ColEqTFR").DataBind.Bind("DTFinalR", "EqTypFR");

            mtxFinalRecords.AutoResizeColumns();
        }

        private void FillPurchReqInvMatrix()
        {
            mtxPurchase.Columns.Item("#").DataBind.Bind("DTPrcInv", "#");
            mtxPurchase.Columns.Item("ColMacPur").DataBind.Bind("DTPrcInv", "ActCodPrc");
            mtxPurchase.Columns.Item("ColNmePur").DataBind.Bind("DTPrcInv", "ActNumPrc");
            mtxPurchase.Columns.Item("ColTypPur").DataBind.Bind("DTPrcInv", "DocTyDPrc");
            mtxPurchase.Columns.Item("ColGasPur").DataBind.Bind("DTPrcInv", "GasPrch");
            mtxPurchase.Columns.Item("Col15WPur").DataBind.Bind("DTPrcInv", "15W40Prch");
            mtxPurchase.Columns.Item("ColHdlPur").DataBind.Bind("DTPrcInv", "HidPrch");
            mtxPurchase.Columns.Item("ColSAEPur").DataBind.Bind("DTPrcInv", "SAE40Prch");
            mtxPurchase.Columns.Item("ColTraPur").DataBind.Bind("DTPrcInv", "TransPrch");
            mtxPurchase.Columns.Item("ColGrsPur").DataBind.Bind("DTPrcInv", "OilsPrch");
            mtxPurchase.Columns.Item("ColEqTPur").DataBind.Bind("DTPrcInv", "EqTypPrch");
            mtxPurchase.Columns.Item("ColDTPurc").DataBind.Bind("DTPrcInv", "DslTPrch");

            mtxPurchase.AutoResizeColumns();
        }

        private void FillInitialsRecordsMatrix()
        {
            mtxInitialRecords.Columns.Item("#").DataBind.Bind("DTIniRds", "#");
            mtxInitialRecords.Columns.Item("ColMacIR").DataBind.Bind("DTIniRds", "ActCodIR");
            mtxInitialRecords.Columns.Item("ColNEnIR").DataBind.Bind("DTIniRds", "ActNumIR");
            mtxInitialRecords.Columns.Item("ColDlMIR").DataBind.Bind("DTIniRds", "DslMIR");
            mtxInitialRecords.Columns.Item("ColDlTIR").DataBind.Bind("DTIniRds", "DslTIR");
            mtxInitialRecords.Columns.Item("ColGasIR").DataBind.Bind("DTIniRds", "GasIR");
            mtxInitialRecords.Columns.Item("Col15WIR").DataBind.Bind("DTIniRds", "15W40IR");
            mtxInitialRecords.Columns.Item("ColHdlIR").DataBind.Bind("DTIniRds", "HidIR");
            mtxInitialRecords.Columns.Item("ColSAEIR").DataBind.Bind("DTIniRds", "SAE40IR");
            mtxInitialRecords.Columns.Item("ColTrmIR").DataBind.Bind("DTIniRds", "TransIR");
            mtxInitialRecords.Columns.Item("ColGrsIR").DataBind.Bind("DTIniRds", "OilsIR");
            mtxInitialRecords.Columns.Item("ColKmhIR").DataBind.Bind("DTIniRds", "KmHrIR");
            mtxInitialRecords.Columns.Item("ColEqTyp").DataBind.Bind("DTIniRds", "EqTypIR");

            mtxInitialRecords.AutoResizeColumns();
        }

        private void FillTravelExpensesMatrix()
        {
            mtxTravelExpenses.Columns.Item("#").DataBind.Bind("DTTravExp", "#");
            mtxTravelExpenses.Columns.Item("ColDateV").DataBind.Bind("DTTravExp", "DateTravE");
            mtxTravelExpenses.Columns.Item("ColFolVia").DataBind.Bind("DTTravExp", "FolTravE");
            mtxTravelExpenses.Columns.Item("ColFolPay").DataBind.Bind("DTTravExp", "DocNTravE");
            mtxTravelExpenses.Columns.Item("ColImpVia").DataBind.Bind("DTTravExp", "ImpTravE");
            mtxTravelExpenses.Columns.Item("ColEstVia").DataBind.Bind("DTTravExp", "StsTravE");

            SAPbouiCOM.LinkedButton oLink = (SAPbouiCOM.LinkedButton)mtxTravelExpenses.Columns.Item("ColFolPay").ExtendedObject;
            //oLink.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_VendorPayment; //(SAPbouiCOM.BoLinkedObject)140;

            mtxTravelExpenses.AutoResizeColumns();
        }

        private void FillContractsMatrix()
        {
            mtxContracts.Columns.Item("#").DataBind.Bind("DTCont", "#");
            mtxContracts.Columns.Item("ColDateOV").DataBind.Bind("DTCont", "DateCont");
            mtxContracts.Columns.Item("ColDocEOV").DataBind.Bind("DTCont", "DocNCont");
            mtxContracts.Columns.Item("ColTypeOV").DataBind.Bind("DTCont", "TypeCont");
            mtxContracts.Columns.Item("ColImpOV").DataBind.Bind("DTCont", "ImpCont");
            mtxContracts.Columns.Item("ColStsOV").DataBind.Bind("DTCont", "StsCont");
            mtxContracts.Columns.Item("ColCardNm").DataBind.Bind("DTCont", "CardName");
            mtxContracts.Columns.Item("ColMunOV").DataBind.Bind("DTCont", "Munp");

            SAPbouiCOM.LinkedButton oLink = (SAPbouiCOM.LinkedButton)mtxContracts.Columns.Item("ColDocEOV").ExtendedObject;
            //oLink.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_Order;

            mtxContracts.AutoResizeColumns();
        }

        private void FillMatrixEmployees()
        {
            mtxEmployees.Columns.Item("#").DataBind.Bind("DTEmp", "#");
            mtxEmployees.Columns.Item("ColId").DataBind.Bind("DTEmp", "EmpId");
            mtxEmployees.Columns.Item("ColEmp").DataBind.Bind("DTEmp", "EmpName");
            mtxEmployees.Columns.Item("ColAct").DataBind.Bind("DTEmp", "EmpChk");

            AddMtrixEmpoyeesDefaultRow();
        }

        private void AddMtrixEmpoyeesDefaultRow()
        {
            mtxEmployees.AddRow();
            dtEmployees.Rows.Add();
            dtEmployees.SetValue("#", 0, 1);
            mtxEmployees.AutoResizeColumns();
        }

        private void FillMatrixConsumables()
        {
            mtxConsumables.Columns.Item("#").DataBind.Bind("DTCons", "#");
            mtxConsumables.Columns.Item("ColDateC").DataBind.Bind("DTCons", "DateCons");
            mtxConsumables.Columns.Item("ColFolCon").DataBind.Bind("DTCons", "FolRCons");
            mtxConsumables.Columns.Item("ColFolTra").DataBind.Bind("DTCons", "FolTCons");

            SAPbouiCOM.LinkedButton oLink = (SAPbouiCOM.LinkedButton)mtxConsumables.Columns.Item("ColFolTra").ExtendedObject;
            oLink.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_StockTransfers;

            SAPbouiCOM.LinkedButton oLinkFol = (SAPbouiCOM.LinkedButton)mtxConsumables.Columns.Item("ColFolCon").ExtendedObject;
            oLinkFol.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_StockTransfersRequest;

            /*dtConsumables.Rows.Add();
            dtConsumables.SetValue("#", dtConsumables.Rows.Count - 1, dtConsumables.Rows.Count + 1);
            dtConsumables.SetValue("CodeCons", dtConsumables.Rows.Count - 1, 0);
            dtConsumables.SetValue("DateCons", dtConsumables.Rows.Count - 1, DateTime.Now);
            dtConsumables.SetValue("FolRCons", dtConsumables.Rows.Count - 1, 38);
            dtConsumables.SetValue("FolTCons", dtConsumables.Rows.Count - 1, 60);*/

            mtxConsumables.LoadFromDataSource();
            mtxConsumables.AutoResizeColumns();
        }
        #endregion
        #endregion
        #endregion

        #region Controls
        #region StaticText
        private SAPbouiCOM.StaticText lblFolio;
        private SAPbouiCOM.StaticText lblDate;
        private SAPbouiCOM.StaticText lblClient;
        private SAPbouiCOM.StaticText lblSupervisor;
        private SAPbouiCOM.StaticText lblFolioRel;
        private SAPbouiCOM.StaticText lblStatus;
        private SAPbouiCOM.StaticText lblOV;
        private SAPbouiCOM.StaticText lblOperadores;
        private SAPbouiCOM.StaticText lblConsumables;
        private SAPbouiCOM.StaticText lblTravelExpenses;
        private SAPbouiCOM.StaticText lblStarDate;
        private SAPbouiCOM.StaticText lblEndDate;
        private SAPbouiCOM.StaticText lblInitialRecords;
        private SAPbouiCOM.StaticText lblPurchase;
        private SAPbouiCOM.StaticText lblFinalRecords;
        private SAPbouiCOM.StaticText lblConsumedTotal;
        private SAPbouiCOM.StaticText lblFootHoursTotal;
        private SAPbouiCOM.StaticText lblKmHrTotal;
        private SAPbouiCOM.StaticText lblTransitHours;
        private SAPbouiCOM.StaticText lblTotalsHours;
        #endregion

        #region EditText
        private SAPbouiCOM.EditText txtFolio;
        private SAPbouiCOM.EditText txtDate;
        private SAPbouiCOM.EditText txtClient;
        private SAPbouiCOM.EditText txtSupervisor;
        private SAPbouiCOM.EditText txtFolioRelation;
        private SAPbouiCOM.EditText txtStatus;
        private SAPbouiCOM.EditText txtStartDate;
        private SAPbouiCOM.EditText txtEndDate;
        private SAPbouiCOM.EditText txtFootHoursTotal;
        private SAPbouiCOM.EditText txtKmHrTotal;
        private SAPbouiCOM.EditText txtTotalsHours;
        #endregion

        #region Buttons
        private SAPbouiCOM.Button btnOpen;
        private SAPbouiCOM.Button btnClose;
        private SAPbouiCOM.Button btnCancel;
        private SAPbouiCOM.Button btnSave;
        private SAPbouiCOM.Button btnSearchFolio;
        private SAPbouiCOM.Button btnVincOV;
        private SAPbouiCOM.Button btnCreateOV;
        private SAPbouiCOM.Button btnConsumables;
        private SAPbouiCOM.Button btnTravelExpenses;
        private SAPbouiCOM.Button btnExit;
        private SAPbouiCOM.Button btnCalculatePerf;
        private SAPbouiCOM.Button btnSaveIR;
        #endregion

        #region Forms
        private SAPbouiCOM.Form oForm;
        private SAPbouiCOM.ChooseFromList mObjClientChooseFromList;
        private SAPbouiCOM.ChooseFromList mObjSupervisorsChooseFromList;
        #endregion

        #region Tabs
        private SAPbouiCOM.Folder tabGeneral;
        private SAPbouiCOM.Folder tabCons;
        private SAPbouiCOM.Folder tabHours;
        private SAPbouiCOM.Folder tabRend;
        #endregion

        #region Matrix
        private SAPbouiCOM.Matrix mtxEmployees;
        private SAPbouiCOM.Matrix mtxContracts;
        private SAPbouiCOM.Matrix mtxConsumables;
        private SAPbouiCOM.Matrix mtxTravelExpenses;
        private SAPbouiCOM.Matrix mtxInitialRecords;
        private SAPbouiCOM.Matrix mtxPurchase;
        private SAPbouiCOM.Matrix mtxFinalRecords;
        private SAPbouiCOM.Matrix mtxConsumedTotals;
        private SAPbouiCOM.Matrix mtxHours;
        private SAPbouiCOM.Matrix mtxTransitHours;
        private SAPbouiCOM.Matrix mtxMachPerformance;
        private SAPbouiCOM.Matrix mtxVclPerformance;
        #endregion

        #region Datatables
        private SAPbouiCOM.DataTable dtEmployees;
        private SAPbouiCOM.DataTable dtConsumables;
        private SAPbouiCOM.DataTable dtContracts;
        private SAPbouiCOM.DataTable dtTravelExpenses;
        private SAPbouiCOM.DataTable dtInitialRcords;
        private SAPbouiCOM.DataTable dtPurchase;
        private SAPbouiCOM.DataTable dtFinalRecords;
        private SAPbouiCOM.DataTable dtConsumedTotals;
        private SAPbouiCOM.DataTable dtHours;
        private SAPbouiCOM.DataTable dtTransitHours;
        private SAPbouiCOM.DataTable dtMachineryPerformance;
        private SAPbouiCOM.DataTable dtVehiclePerformance;
        #endregion

        private void mtxHours_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            lObjColumn_ComboSelectAfter(sboObject, pVal);

        }



        #endregion

    }
}
