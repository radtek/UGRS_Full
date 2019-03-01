using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.AddOn.Finances.Utils;
using UGRS.Core.SDK.DI.Finances.DTO;
using UGRS.Core.SDK.DI.Finances.DAO;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Utility;

namespace UGRS.AddOn.Finances.Forms
{
    [FormAttribute("UGRS.AddOn.Finances.Forms.frmBouncedChecks", "Forms/BouncedChecks.b1f")]
    class BouncedChecks : UserFormBase
    {
        private SAPbouiCOM.StaticText mTxtMoveType;
        private SAPbouiCOM.StaticText mTxtDateStart;
        private SAPbouiCOM.StaticText mTxtDateFinal;
        private SAPbouiCOM.ComboBox mCmbMvType;
        private SAPbouiCOM.EditText mEdtDateStart;
        private SAPbouiCOM.StaticText mTxtCheckNum;
        private SAPbouiCOM.EditText mEdtCheckNum;
        private SAPbouiCOM.EditText mEdtDateFinal;
        private SAPbouiCOM.Button mBtnSearch;
        private SAPbouiCOM.Matrix mMtxChecks;
        private SAPbouiCOM.Button mBtnSave;

        private SAPbouiCOM.DataTable mDtChecks;

        private CheckDAO mCheckDAO = new CheckDAO();
        private QueryManager mObjQueryManager = new QueryManager();

        public BouncedChecks()
        {
        }

        private void LoadEvents()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void UnloadEvents()
        {
            Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (!FormUID.Equals(this.UIAPIRawForm.UniqueID))
            {
                return;
            }

            try
            {
                if (!pVal.BeforeAction)
                {
                    switch (pVal.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_CLICK:
                            if (pVal.ItemUID == mBtnSearch.Item.UniqueID)
                            {
                                SearchChecks();
                            }
                            if (pVal.ItemUID == mBtnSave.Item.UniqueID)
                            {
                                SaveChecks();
                            }
                            break;
                        case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED:
                            if (pVal.ItemUID == mMtxChecks.Item.UniqueID && pVal.ColUID == "C_Selected")
                            {
                                if (pVal.Row > mMtxChecks.RowCount)
                                {
                                    return;
                                }
                                this.FlushValueToSource(mMtxChecks, pVal.ColUID, pVal.Row);
                            }
                            break;
                        case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                            UnloadEvents();
                            break;
                    }
                }
                else
                {
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[BouncedChecks - SBO_Application_ItemEvent] Error: {0}", lObjException.Message));
                UIApplication.ShowMessageBox(lObjException.Message);
            }
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mTxtMoveType = ((SAPbouiCOM.StaticText)(this.GetItem("txtMvType").Specific));
            this.mTxtDateStart = ((SAPbouiCOM.StaticText)(this.GetItem("txtDtStr").Specific));
            this.mTxtDateFinal = ((SAPbouiCOM.StaticText)(this.GetItem("txtDtFnl").Specific));
            this.mCmbMvType = ((SAPbouiCOM.ComboBox)(this.GetItem("cbMvType").Specific));
            this.mEdtDateStart = ((SAPbouiCOM.EditText)(this.GetItem("edtDtStr").Specific));
            this.mTxtCheckNum = ((SAPbouiCOM.StaticText)(this.GetItem("txtChkNum").Specific));
            this.mEdtCheckNum = ((SAPbouiCOM.EditText)(this.GetItem("edtChkNum").Specific));
            this.mEdtDateFinal = ((SAPbouiCOM.EditText)(this.GetItem("edtDtFnl").Specific));
            this.mBtnSearch = ((SAPbouiCOM.Button)(this.GetItem("btnSearch").Specific));
            this.mMtxChecks = ((SAPbouiCOM.Matrix)(this.GetItem("mtxChecks").Specific));
            this.mBtnSave = ((SAPbouiCOM.Button)(this.GetItem("btnSave").Specific));
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
            LoadEvents();

            mCmbMvType.Item.DisplayDesc = true;
            mCmbMvType.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;

            mDtChecks = this.UIAPIRawForm.DataSources.DataTables.Item("DT_Checks");
            mDtChecks.BindToMatrix(mMtxChecks);
            mMtxChecks.AutoResizeColumns();
        }

        private void SearchChecks()
        {
            try
            {
                IList<CheckDTO> lLstChecks = mCheckDAO.GetChecks();
                int i = 0;
                mDtChecks.Rows.Clear();
                string lStrSelected = mCmbMvType.Value;
                DateTime lStartDate = this.GetDataSourceValue<DateTime>("UD_DtStr");
                DateTime lEndDate = this.GetDataSourceValue<DateTime>("UD_DtFnl");
                foreach (CheckDTO lObjCheck in lLstChecks)
                {
                    if (mEdtCheckNum.Value != "" && mEdtCheckNum.Value != lObjCheck.CheckNum.ToString())
                    {
                        continue;
                    }
                    // Skip checks that are not deposited
                    if (lStrSelected == "Y" && lObjCheck.Deposited != "C")
                    {
                        continue;
                    }
                    // Skip checks taht are deposited
                    if (lStrSelected == "N" && lObjCheck.Deposited == "C")
                    {
                        continue;
                    }
                    // Filter checks newer than start date
                    if (lStartDate > lObjCheck.CheckDate)
                    {
                        continue;
                    }
                    // Filter older than end date
                    if (lEndDate < lObjCheck.CheckDate && lEndDate != default(DateTime))
                    {
                        continue;
                    }
                    mDtChecks.Rows.Add();
                    mDtChecks.SetValue("#", i, i + 1);
                    mDtChecks.SetValue("C_CheckNum", i, lObjCheck.CheckNum);
                    mDtChecks.SetValue("C_CheckSum", i, lObjCheck.CheckSum);
                    mDtChecks.SetValue("C_CardName", i, lObjCheck.CardName);
                    mDtChecks.SetValue("C_CheckDt", i, lObjCheck.CheckDate);
                    mDtChecks.SetValue("C_CheckCur", i, lObjCheck.Currency);
                    // Not visible
                    mDtChecks.SetValue("C_CheckKey", i, lObjCheck.CheckKey);
                    mDtChecks.SetValue("C_CardCode", i, lObjCheck.CardCode);
                    mDtChecks.SetValue("C_BankAcc", i, lObjCheck.BankAcct);
                    if (lObjCheck.Deposited == "C")
                    {
                        mDtChecks.SetValue("C_Status", i, "Depositado");
                    }
                    else
                    {
                        mDtChecks.SetValue("C_Status", i, "Ventanilla");
                    }
                    i++;
                }
                mMtxChecks.LoadFromDataSource();
                mMtxChecks.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[BouncedChecks - SearchChecks] Error: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        private void SaveChecks()
        {
            try
            {
                string mStrReturnedChecksAcct = mObjQueryManager.GetValue("U_VALUE", "Name", "FZ_ACCTCHEQUE", Constants.STR_CONFIG_TABLE);
                string mStrItemCode = mObjQueryManager.GetValue("U_VALUE", "Name", "FZ_ITEMCHEQDELV", Constants.STR_CONFIG_TABLE);
                for (int i = 0; i < mDtChecks.Rows.Count; i++)
                {
                    if (mDtChecks.GetCellValue<string>("C_Selected", i) == "Y")
                    {
                        string lStrCheckAcct = mDtChecks.GetCellValue<string>("C_BankAcc", i);
                        // If the check is not deposited, a deposit is generated first
                        if (mDtChecks.GetCellValue<string>("C_Status", i) == "Ventanilla")
                        {
                            lStrCheckAcct = mStrReturnedChecksAcct;
                            SAPbobsCOM.CompanyService lObjService = DIApplication.Company.GetCompanyService();
                            SAPbobsCOM.DepositsService lObjDepositService = lObjService.GetBusinessService(SAPbobsCOM.ServiceTypes.DepositsService) as SAPbobsCOM.DepositsService;
                            SAPbobsCOM.Deposit lObjDeposit = lObjDepositService.GetDataInterface(SAPbobsCOM.DepositsServiceDataInterfaces.dsDeposit) as SAPbobsCOM.Deposit;

                            lObjDeposit.DepositType = SAPbobsCOM.BoDepositTypeEnum.dtChecks;
                            lObjDeposit.DepositAccountType = SAPbobsCOM.BoDepositAccountTypeEnum.datBankAccount;
                            //lObjDeposit.TotalLC = mDtChecks.GetCellValue<double>("C_CheckSum", i);
                            lObjDeposit.ReconcileAfterDeposit = SAPbobsCOM.BoYesNoEnum.tYES;
                            lObjDeposit.DepositAccount = mStrReturnedChecksAcct;

                            SAPbobsCOM.CheckLine lObjCheckLine = lObjDeposit.Checks.Add();
                            lObjCheckLine.CheckKey = mDtChecks.GetCellValue<int>("C_CheckKey", i);

                            SAPbobsCOM.DepositParams lObjDepositParams = lObjDepositService.AddDeposit(lObjDeposit);
                        }

                        SAPbobsCOM.Documents lObjDebitNote = DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices) as SAPbobsCOM.Documents;
                        lObjDebitNote.DocObjectCode = SAPbobsCOM.BoObjectTypes.oInvoices;
                        lObjDebitNote.DocumentSubType = SAPbobsCOM.BoDocumentSubType.bod_DebitMemo;
                        lObjDebitNote.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;
                        lObjDebitNote.DocDate = DateTime.Now;
                        lObjDebitNote.CardCode = mDtChecks.GetCellValue<string>("C_CardCode", i);
                        lObjDebitNote.NumAtCard = mDtChecks.GetCellValue<string>("C_CheckKey", i);

                        lObjDebitNote.Lines.ItemCode = mStrItemCode;
                        lObjDebitNote.Lines.Quantity = 1;
                        lObjDebitNote.Lines.UnitPrice = mDtChecks.GetCellValue<double>("C_CheckSum", i);
                        lObjDebitNote.Lines.TaxCode = "VE";
                        lObjDebitNote.Lines.AccountCode = mStrReturnedChecksAcct;
                        lObjDebitNote.Lines.Add();

                        lObjDebitNote.EDocGenerationType = SAPbobsCOM.EDocGenerationTypeEnum.edocNotRelevant;

                        int lLongErr = lObjDebitNote.Add();
                        string lStrErrMsg;
                        if (lLongErr != 0)
                        {
                            DIApplication.Company.GetLastError(out lLongErr, out lStrErrMsg);
                            UIApplication.ShowError(lStrErrMsg);
                        }
                        else
                        {
                            UIApplication.ShowMessage("Success");
                        }
                        SearchChecks();
                    }
                }
            }
            catch (Exception pObjException)
            {
                LogUtility.WriteError(string.Format("[BouncedChecks - SaveChecks] Error al guardar cheque: {0}", pObjException.Message));
                UIApplication.ShowMessageBox(string.Format("Error al guardar cheque: {0}", pObjException.Message));
            }
        }
    }
}
