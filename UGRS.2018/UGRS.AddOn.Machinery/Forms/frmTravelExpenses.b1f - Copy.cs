using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.DI.Machinery;
using UGRS.Core.SDK.DI.Machinery.Enums;
using UGRS.AddOn.Machinery.Enums;
using UGRS.Core.Extension.Enum;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.Machinery.DTO;

namespace UGRS.AddOn.Machinery.Forms
{
    [FormAttribute("UGRS.AddOn.Machinery.Forms.frmTravelExpenses", "Forms/frmTravelExpenses.b1f")]
    class frmTravelExpenses : UserFormBase
    {
        #region Properties
        private string mStrClientCode = string.Empty;
        private int mIntRiseFolio;
        private string mStrSupervisorCode = string.Empty;
        private string mStrSupervisorName = string.Empty;
        private string mStrCostCenter = string.Empty;
        private string mStrViaticExpAcct = string.Empty;
        private string mStrExpDayAcct = string.Empty;
        private string mStrMinorPurchAcct = string.Empty;
        private string mStrViaticPaymAcct = string.Empty;
        private double mDblViaticPrice = 0;
        private double mDblExpDayPrice = 0;
        private MachinerySeviceFactory mObjMachinerySeviceFactory = null;
        private SAPbouiCOM.DataTable mDtEmployees = null;
        private int mIntDaysTotal = 0;
        public TravelExpensesDTO mObjTravelExpensesDTO = null;
        private bool mBolIsFirstTravExp = false;
        #endregion

        #region Constructor
        public frmTravelExpenses(int pIntRiseFolio, string pStrSupervisorCode, string pStrSupervisorName, int pIntDaysTotal, SAPbouiCOM.DataTable pDtEmployees, bool pBolIsFirstTravExp)
        {
            mObjMachinerySeviceFactory = new MachinerySeviceFactory();

            LoadEvents();

            mIntRiseFolio = pIntRiseFolio;
            mStrSupervisorCode = pStrSupervisorCode;
            mStrSupervisorName = pStrSupervisorName;
            mDtEmployees = pDtEmployees;
            mIntDaysTotal = pIntDaysTotal;
            mBolIsFirstTravExp = pBolIsFirstTravExp;

            mStrCostCenter = mObjMachinerySeviceFactory.GetUsersService().GetUserCenterCost(Application.SBO_Application.Company.UserName);
            mStrViaticExpAcct = mObjMachinerySeviceFactory.GetConfigurationsService().GetAccountCode(ConfigurationsEnum.ViaticExpenses);
            mStrExpDayAcct = mObjMachinerySeviceFactory.GetConfigurationsService().GetAccountCode(ConfigurationsEnum.DayExpenses);
            mStrMinorPurchAcct = mObjMachinerySeviceFactory.GetConfigurationsService().GetAccountCode(ConfigurationsEnum.MinorExpenses);
            mStrViaticPaymAcct = mObjMachinerySeviceFactory.GetConfigurationsService().GetAccountCode(ConfigurationsEnum.DeudoresViaticos);
            mDblViaticPrice = double.Parse(mObjMachinerySeviceFactory.GetConfigurationsService().GetConfigurationByName(ConfigurationsEnum.ViaticPrice).Value);
            mDblExpDayPrice = double.Parse(mObjMachinerySeviceFactory.GetConfigurationsService().GetConfigurationByName(ConfigurationsEnum.DayExpensesPrice).Value);

            LoadInitialsControls();
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lblFolio = ((SAPbouiCOM.StaticText)(this.GetItem("lblFolV").Specific));
            this.lblRiseFolio = ((SAPbouiCOM.StaticText)(this.GetItem("lblFRiseV").Specific));
            this.lblDate = ((SAPbouiCOM.StaticText)(this.GetItem("lblDateV").Specific));
            this.lblSupervisor = ((SAPbouiCOM.StaticText)(this.GetItem("lblSupV").Specific));
            this.txtFolio = ((SAPbouiCOM.EditText)(this.GetItem("txtFolV").Specific));
            this.txtRiseFolio = ((SAPbouiCOM.EditText)(this.GetItem("txtFRiseV").Specific));
            this.txtDate = ((SAPbouiCOM.EditText)(this.GetItem("txtDateV").Specific));
            this.txtSupervisor = ((SAPbouiCOM.EditText)(this.GetItem("txtSupV").Specific));
            this.mtxTravelExp = ((SAPbouiCOM.Matrix)(this.GetItem("mtxTravE").Specific));
            this.lblDeudorAmount = ((SAPbouiCOM.StaticText)(this.GetItem("lblImpDeu").Specific));
            this.txtDeudorAmount = ((SAPbouiCOM.EditText)(this.GetItem("txtImpDeu").Specific));
            this.lblSubtotal = ((SAPbouiCOM.StaticText)(this.GetItem("lblSTotV").Specific));
            this.txtSubtotal = ((SAPbouiCOM.EditText)(this.GetItem("txtSTotV").Specific));
            this.lblTotal = ((SAPbouiCOM.StaticText)(this.GetItem("lblTotV").Specific));
            this.txtTotal = ((SAPbouiCOM.EditText)(this.GetItem("txtTotV").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("btnCreV").Specific));
            this.mtxTravelExp.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.mtxXML_ValidateBefore);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
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

        #region Events
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
                                if (pVal.ItemUID.Equals("btnCreV"))
                                {
                                    DoPayment();
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST:
                                ChooseFromListAfterEvent(pVal);
                                break;
                            case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS:
                                if (pVal.ItemUID.Equals("mtxCont"))
                                {
                                    if (pVal.ColUID.Equals("ColHPH") || pVal.ColUID.Equals("ColPrice"))
                                    {
                                        //CalculateLineTotal(pVal.Row);
                                    }
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_FORM_LOAD:
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
                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }

        private void ChooseFromListAfterEvent(SAPbouiCOM.ItemEvent pObjValEvent)
        {
            if (pObjValEvent.Action_Success)
            {
                SAPbouiCOM.IChooseFromListEvent lObjCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pObjValEvent;
                if (lObjCFLEvento.SelectedObjects == null)
                    return;

                SAPbouiCOM.DataTable lObjDataTable = lObjCFLEvento.SelectedObjects;

                if (lObjDataTable.UniqueID == "CFLSup")
                {
                    mStrClientCode = Convert.ToString(lObjDataTable.GetValue(0, 0));
                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = lObjDataTable.GetValue(1, 0).ToString();
                }
            }
        }

        private void mtxXML_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (pVal.ColUID == "ColPrcV" || pVal.ColUID == "ColDiasV")
                {
                    string lStrPrice = (mtxTravelExp.Columns.Item(5).Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
                    string lStrDays = (mtxTravelExp.Columns.Item(3).Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();

                    int lIntDays = string.IsNullOrEmpty(lStrDays) ? 0 : int.Parse(lStrDays);
                    double lDblPrice = string.IsNullOrEmpty(lStrPrice) ? 0 : double.Parse(lStrPrice);

                    dtTravelExp.SetValue("TravDays", pVal.Row - 1, lIntDays);
                    dtTravelExp.SetValue("TravPrice", pVal.Row - 1, lDblPrice);
                    dtTravelExp.SetValue("TravImp", pVal.Row - 1, (lIntDays * lDblPrice));

                    mtxTravelExp.LoadFromDataSource();
                    mtxTravelExp.AutoResizeColumns();

                    CalculateTotal();
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
        #endregion

        #region Functions
        private void LoadInitialsControls()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                txtFolio.Value = mObjMachinerySeviceFactory.GetTravelExpensesService().GetNexFolio(mStrCostCenter).ToString();
                txtRiseFolio.Value = mIntRiseFolio.ToString(); //BALLESTEROS GUTIERREZ
                txtDate.Value = DateTime.Now.ToString("dd/MM/yyyy");

                CreateTravelExpensesDatatable();
                LoadChoosesFromList();
                SetCFLToTxt();

                if (mBolIsFirstTravExp)
                {
                    AddTravelExpenses(new TravelExpMovTypesEnum.TravelExpenses(), 1);
                    AddTravelExpenses(new TravelExpMovTypesEnum.DayExpenses(), mDtEmployees.Rows.Count);
                }

                AddTravelExpenses(new TravelExpMovTypesEnum.MinorExpenses(), 1);

                CalculateTotal();

                this.UIAPIRawForm.DataSources.UserDataSources.Item("CFLSup").ValueEx = mStrSupervisorName;
            }
            catch (Exception lObjException)
            {
                Application.SBO_Application.MessageBox(string.Format("Error al cargar los controles iniciales: {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void DoPayment()
        {
            try
            {
                if (ValidateEmptyControls())
                {
                    Application.SBO_Application.SetStatusBarMessage("Verificar campos vacíos", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                if (dtTravelExp.Rows.Count <= 0)
                {
                    Application.SBO_Application.SetStatusBarMessage("No puede crear un pago sin líneas", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                if (string.IsNullOrEmpty(mStrExpDayAcct) || string.IsNullOrEmpty(mStrMinorPurchAcct) || string.IsNullOrEmpty(mStrViaticExpAcct) || string.IsNullOrEmpty(mStrViaticPaymAcct))
                {
                    Application.SBO_Application.SetStatusBarMessage("Alguna de las cuentas de configuración no tiene valor, favor de verificar", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                this.UIAPIRawForm.Freeze(true);

                SAPbobsCOM.Payments lObjOutgoingPayment = (SAPbobsCOM.Payments)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
                lObjOutgoingPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_OutgoingPayments;
                lObjOutgoingPayment.DocDate = DateTime.Now;
                lObjOutgoingPayment.DocType = SAPbobsCOM.BoRcptTypes.rAccount;
                lObjOutgoingPayment.CashAccount = mStrViaticPaymAcct;
                lObjOutgoingPayment.UserFields.Fields.Item("U_GLO_PaymentType").Value = ((int)PaymentsTypesEnum.TravelExpenses).ToString();
                lObjOutgoingPayment.UserFields.Fields.Item("U_GLO_CodeMov").Value = txtFolio.Value; //folio de solicitud de viáticos
                lObjOutgoingPayment.UserFields.Fields.Item("U_GLO_CostCenter").Value = "MQ_MAQUI"; //centro de costo
                lObjOutgoingPayment.CashSum = double.Parse(txtTotal.Value);

                for (int i = 0; i < dtTravelExp.Rows.Count; i++)
                {
                    int lIntMovTypeCode = int.Parse(dtTravelExp.GetValue(9, i).ToString());
                    double lDblLineImport = double.Parse(dtTravelExp.GetValue(8, i).ToString());
                    string lStrEmployeeType = dtTravelExp.GetValue(10, i).ToString();
                    string lStrEmployeeCode = dtTravelExp.GetValue(5, i).ToString();
                    double lDblDayPrice = double.Parse(dtTravelExp.GetValue(7, i).ToString());

                    lObjOutgoingPayment.AccountPayments.SetCurrentLine(i);
                    lObjOutgoingPayment.AccountPayments.SumPaid = lDblLineImport;
                    lObjOutgoingPayment.AccountPayments.UserFields.Fields.Item("U_GLO_TypeViatic").Value = lIntMovTypeCode.ToString();
                    lObjOutgoingPayment.AccountPayments.UserFields.Fields.Item("U_MQ_TypeEmp").Value = lStrEmployeeType;
                    lObjOutgoingPayment.AccountPayments.UserFields.Fields.Item("U_MQ_Aux").Value = lStrEmployeeCode; //code empleado
                    lObjOutgoingPayment.AccountPayments.UserFields.Fields.Item("U_MQ_DayPrice").Value = lDblDayPrice;
                    
                    switch (lIntMovTypeCode)
                    {
                        case (int)TravelExpMovTypesEnum.DayExpenses:
                            lObjOutgoingPayment.AccountPayments.AccountCode = mStrExpDayAcct;
                            break;
                        case (int)TravelExpMovTypesEnum.MinorExpenses:
                            lObjOutgoingPayment.AccountPayments.AccountCode = mStrMinorPurchAcct;
                            break;
                        case (int)TravelExpMovTypesEnum.TravelExpenses:
                            lObjOutgoingPayment.AccountPayments.AccountCode = mStrViaticExpAcct;
                            break;
                        default:
                            break;
                    }
                    lObjOutgoingPayment.AccountPayments.Add();
                }

                if (lObjOutgoingPayment.Add() != 0)
                {
                    string lStrLastError = DIApplication.Company.GetLastErrorDescription();
                    UIApplication.ShowMessageBox(string.Format("Error al generar el pago: {0}", DIApplication.Company.GetLastErrorDescription()));
                }
                else
                {
                    int lIntDocEntry = int.Parse(DIApplication.Company.GetNewObjectKey());

                    mObjTravelExpensesDTO = mObjMachinerySeviceFactory.GetTravelExpensesService().GetPayment(lIntDocEntry);

                    this.UIAPIRawForm.Close();
                }
            }
            catch (Exception lObjException)
            {
                Application.SBO_Application.MessageBox(string.Format("Error al realizar el pago efectuado: {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        public bool ValidateEmptyControls()
        {
            bool lBolEmpty = false;

            if (string.IsNullOrEmpty(txtFolio.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(txtRiseFolio.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(mStrSupervisorCode))
            {
                lBolEmpty = true;
            }

            return lBolEmpty;
        }

        private void CalculateTotal()
        {
            try
            {
                double lDblSubtotal = 0;
                string lStrDeudorImport = txtDeudorAmount.Value;

                double outr = 0;
                if (double.TryParse(lStrDeudorImport, out outr) || string.IsNullOrEmpty(lStrDeudorImport))
                {
                    double lDblDeudorImport = string.IsNullOrEmpty(lStrDeudorImport) ? 0 : double.Parse(lStrDeudorImport);

                    for (int i = 0; i < mtxTravelExp.RowCount; i++)
                    {
                        double lDblImport = double.Parse(dtTravelExp.GetValue(8, i).ToString());

                        lDblSubtotal += lDblImport;
                    }

                    txtSubtotal.Value = lDblSubtotal.ToString();
                    txtTotal.Value = (lDblSubtotal + lDblDeudorImport).ToString();
                }
                else
                {
                    txtDeudorAmount.Value = string.Empty;
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al calcular el total: {0}", lObjException.Message));
            }
        }

        private void AddTravelExpenses(TravelExpMovTypesEnum pEnumMovType, int pIntRepeat, int pIntDays = 1)
        {
            try
            {
                for (int i = 0; i < pIntRepeat; i++)
                {
                    dtTravelExp.Rows.Add();
                    dtTravelExp.SetValue("#", dtTravelExp.Rows.Count - 1, dtTravelExp.Rows.Count + 1);
                    dtTravelExp.SetValue("MovType", dtTravelExp.Rows.Count - 1, pEnumMovType);
                    dtTravelExp.SetValue("MovCode", dtTravelExp.Rows.Count - 1, ((int)pEnumMovType).ToString());
                    dtTravelExp.SetValue("Commts", dtTravelExp.Rows.Count - 1, string.Empty);
                    dtTravelExp.SetValue("TravDays", dtTravelExp.Rows.Count - 1, mIntDaysTotal);

                    if (pEnumMovType == TravelExpMovTypesEnum.DayExpenses.Value) //operadores
                    {
                        dtTravelExp.SetValue("EmpCode", dtTravelExp.Rows.Count - 1, (mDtEmployees.GetValue(1, i).ToString() == "0") ? mStrSupervisorCode : mDtEmployees.GetValue(1, i));
                        dtTravelExp.SetValue("EmpName", dtTravelExp.Rows.Count - 1, string.IsNullOrEmpty(mDtEmployees.GetValue(2, i).ToString()) ? mStrSupervisorName : mDtEmployees.GetValue(2, i));
                        dtTravelExp.SetValue("AcctCode", dtTravelExp.Rows.Count - 1, mStrExpDayAcct);
                        dtTravelExp.SetValue("TravPrice", dtTravelExp.Rows.Count - 1, mDblExpDayPrice);
                        dtTravelExp.SetValue("TravImp", dtTravelExp.Rows.Count - 1, mIntDaysTotal * mDblExpDayPrice);
                        dtTravelExp.SetValue("EmpType", dtTravelExp.Rows.Count - 1, EmployeesTypesEnum.Operators.GetDescription());
                    }
                    else if (pEnumMovType == TravelExpMovTypesEnum.TravelExpenses) //supervisor
                    {
                        dtTravelExp.SetValue("EmpCode", dtTravelExp.Rows.Count - 1, mStrSupervisorCode);
                        dtTravelExp.SetValue("EmpName", dtTravelExp.Rows.Count - 1, mStrSupervisorName);
                        dtTravelExp.SetValue("AcctCode", dtTravelExp.Rows.Count - 1, mStrViaticExpAcct);
                        dtTravelExp.SetValue("TravPrice", dtTravelExp.Rows.Count - 1, mDblViaticPrice);
                        dtTravelExp.SetValue("TravImp", dtTravelExp.Rows.Count - 1, mIntDaysTotal * mDblViaticPrice);
                        dtTravelExp.SetValue("EmpType", dtTravelExp.Rows.Count - 1, EmployeesTypesEnum.Supervisors.GetDescription());
                    }
                    else if (pEnumMovType == TravelExpMovTypesEnum.MinorExpenses) //supervisor
                    {
                        dtTravelExp.SetValue("EmpCode", dtTravelExp.Rows.Count - 1, mStrSupervisorCode);
                        dtTravelExp.SetValue("EmpName", dtTravelExp.Rows.Count - 1, mStrSupervisorName);
                        dtTravelExp.SetValue("AcctCode", dtTravelExp.Rows.Count - 1, mStrMinorPurchAcct);
                        dtTravelExp.SetValue("TravPrice", dtTravelExp.Rows.Count - 1, 0);
                        dtTravelExp.SetValue("TravImp", dtTravelExp.Rows.Count - 1, 0);
                        dtTravelExp.SetValue("EmpType", dtTravelExp.Rows.Count - 1, EmployeesTypesEnum.Supervisors.GetDescription());
                    }
                }

                mtxTravelExp.LoadFromDataSource();
                mtxTravelExp.AutoResizeColumns();

                if (pEnumMovType == TravelExpMovTypesEnum.MinorExpenses) //supervisor
                {
                    SAPbouiCOM.CommonSetting lObjCmmnSetting = mtxTravelExp.CommonSetting;
                    lObjCmmnSetting.SetCellEditable(dtTravelExp.Rows.Count, 3, true);
                    lObjCmmnSetting.SetCellEditable(dtTravelExp.Rows.Count, 5, true);
                }
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al agregar la solicitud de consumible {0}", lObjException.Message));
            }
        }

        private void CreateTravelExpensesDatatable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTTravExp");
            dtTravelExp = this.UIAPIRawForm.DataSources.DataTables.Item("DTTravExp");
            dtTravelExp.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtTravelExp.Columns.Add("AcctCode", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTravelExp.Columns.Add("MovType", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTravelExp.Columns.Add("Commts", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTravelExp.Columns.Add("TravDays", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtTravelExp.Columns.Add("EmpCode", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTravelExp.Columns.Add("EmpName", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTravelExp.Columns.Add("TravPrice", SAPbouiCOM.BoFieldsType.ft_Price);
            dtTravelExp.Columns.Add("TravImp", SAPbouiCOM.BoFieldsType.ft_Price);
            dtTravelExp.Columns.Add("MovCode", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtTravelExp.Columns.Add("EmpType", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            FillTravelExpensesMatrix();
        }

        private void FillTravelExpensesMatrix()
        {
            mtxTravelExp.Columns.Item("#").DataBind.Bind("DTTravExp", "#");
            mtxTravelExp.Columns.Item("ColTipMov").DataBind.Bind("DTTravExp", "MovType");
            mtxTravelExp.Columns.Item("ColComV").DataBind.Bind("DTTravExp", "Commts");
            mtxTravelExp.Columns.Item("ColDiasV").DataBind.Bind("DTTravExp", "TravDays");
            mtxTravelExp.Columns.Item("ColEmpV").DataBind.Bind("DTTravExp", "EmpName");
            mtxTravelExp.Columns.Item("ColPrcV").DataBind.Bind("DTTravExp", "TravPrice");
            mtxTravelExp.Columns.Item("ColImpV").DataBind.Bind("DTTravExp", "TravImp");

            mtxTravelExp.AutoResizeColumns();
        }

        private void SetCFLToTxt()
        {
            txtSupervisor.DataBind.SetBound(true, "", "CFLSup");
            txtSupervisor.ChooseFromListUID = "CFLSup";
        }

        private void LoadChoosesFromList()
        {
            SAPbouiCOM.ChooseFromList lObjCFLClients = InitChooseFromLists(false, "171", "CFLSup", this.UIAPIRawForm.ChooseFromLists);
            AddConditionSupervisorCFL(lObjCFLClients);
        }

        private SAPbouiCOM.ChooseFromList InitChooseFromLists(bool pbol, string pStrType, string pStrID, SAPbouiCOM.ChooseFromListCollection pObjCFLs) //
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
                UIApplication.ShowMessageBox(string.Format("Error al inicializar el choose from list: {0} {1}", pStrID, ex.Message));

            }
            return lObjoCFL;
        }

        private void AddConditionSupervisorCFL(SAPbouiCOM.ChooseFromList pCFL)
        {
            SAPbouiCOM.Condition lObjCon = null;
            SAPbouiCOM.Conditions lObjCons = new SAPbouiCOM.Conditions();

            lObjCon = lObjCons.Add();
            lObjCon.Alias = "position";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "4";

            lObjCon.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

            lObjCon = lObjCons.Add();
            lObjCon.Alias = "dept";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "12";

            pCFL.SetConditions(lObjCons);
        }
        #endregion

        #region Controls
        #region Labels
        private SAPbouiCOM.StaticText lblFolio;
        private SAPbouiCOM.StaticText lblRiseFolio;
        private SAPbouiCOM.StaticText lblDate;
        private SAPbouiCOM.StaticText lblDeudorAmount;
        private SAPbouiCOM.StaticText lblSubtotal;
        private SAPbouiCOM.StaticText lblTotal;
        private SAPbouiCOM.StaticText lblSupervisor;
        #endregion

        #region TextBoxs
        private SAPbouiCOM.EditText txtFolio;
        private SAPbouiCOM.EditText txtRiseFolio;
        private SAPbouiCOM.EditText txtDate;
        private SAPbouiCOM.EditText txtSupervisor;
        private SAPbouiCOM.EditText txtDeudorAmount;
        private SAPbouiCOM.EditText txtSubtotal;
        private SAPbouiCOM.EditText txtTotal;
        #endregion

        #region Matrixs
        private SAPbouiCOM.Matrix mtxTravelExp;
        #endregion

        #region Buttons
        private SAPbouiCOM.Button btnSave;
        #endregion

        #region Datatables
        private SAPbouiCOM.DataTable dtTravelExp;
        #endregion
        #endregion
    }
}
