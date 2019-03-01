using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using UGRS.AddOn.Finances.Utils;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Finances.DAO;
using UGRS.Core.SDK.DI.Finances.DTO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.AddOn.Finances.Forms
{
    [FormAttribute("UGRS.AddOn.Finances.Payments", "Forms/Payments.b1f")]
    class Payments : UserFormBase
    {
        private SAPbouiCOM.ChooseFromList mObjCFLClients;
        private SAPbouiCOM.ChooseFromList mObjCFLBankAcct;
        private SAPbouiCOM.ChooseFromList mObjCFLDepositAcct;
        private SAPbouiCOM.ChooseFromList mObjCFLTransfersGL;
        private SAPbouiCOM.ChooseFromList mObjCFLCashGL;
        private SAPbouiCOM.ChooseFromList mObjCFLChecksGL;
        private SAPbouiCOM.ChooseFromList mObjCFLCreditCardGL;

        private string mStrCardCode;
        private string mStrCardName;

        private SAPbouiCOM.DataTable mDtClientInvoices;
        private SAPbouiCOM.DataTable mDtClientChecks;
        private SAPbouiCOM.DataTable mDtPayChecks;
        private SAPbouiCOM.DataTable mDtPayCreditCards;
        private SAPbouiCOM.DataTable mDtClientAccountPayments;

        private QueryManager mQueryManager = new QueryManager();
        private DocumentDAO mDocumentDAO = new DocumentDAO();
        private CheckDraftDAO mCheckDraftDAO = new CheckDraftDAO();
        private CheckDAO mCheckDAO = new CheckDAO();
        private BankDAO mBankDAO = new BankDAO();
        private PaymentMethodDAO mPaymentMethodDAO = new PaymentMethodDAO();
        private AuctionDAO mAuctionDAO = new AuctionDAO();

        #region Form elements
        private SAPbouiCOM.StaticText mTxtName;
        private SAPbouiCOM.EditText mEdtClient;
        private SAPbouiCOM.EditText mEdtName;
        private SAPbouiCOM.EditText mEdtDate;
        private SAPbouiCOM.StaticText mTxtDate;
        private SAPbouiCOM.Folder Folder0;
        private SAPbouiCOM.Folder Folder1;
        private SAPbouiCOM.Folder Folder2;
        private SAPbouiCOM.Folder Folder3;
        private SAPbouiCOM.Matrix mMtxClientInvoices;
        private SAPbouiCOM.Matrix mMtxClientChecks;
        private SAPbouiCOM.EditText mEdtTotal;
        private SAPbouiCOM.StaticText mTxtTotal;
        private SAPbouiCOM.StaticText mTxtAdvance;
        private SAPbouiCOM.StaticText mTxtPending;
        private SAPbouiCOM.EditText mEdtAdvance;
        private SAPbouiCOM.EditText mEdtPending;
        private SAPbouiCOM.Button mBtnSave;
        private SAPbouiCOM.StaticText mTxtCurrency;
        private SAPbouiCOM.EditText mEdtCurrency;
        private SAPbouiCOM.StaticText mTxtChecksHeader;
        private SAPbouiCOM.Matrix mMtxPayChecks;
        private SAPbouiCOM.StaticText mTxtCreditHeader;
        private SAPbouiCOM.Matrix mMtxPayCreditCards;
        private SAPbouiCOM.EditText mEdtChecksAccount;
        private SAPbouiCOM.StaticText mTxtChecksAccount;
        private SAPbouiCOM.StaticText mTxtTransHeader;
        private SAPbouiCOM.StaticText mTxtTransAccount;
        private SAPbouiCOM.StaticText mTxtTransDate;
        private SAPbouiCOM.StaticText mTxtTransReference;
        private SAPbouiCOM.StaticText mTxtTransAmount;
        private SAPbouiCOM.EditText mEdtTransAccount;
        private SAPbouiCOM.EditText mEdtTransDate;
        private SAPbouiCOM.EditText mEdtTransReference;
        private SAPbouiCOM.EditText mEdtTransAmount;
        private SAPbouiCOM.StaticText mTxtCashHeader;
        private SAPbouiCOM.StaticText mTxtCashType;
        private SAPbouiCOM.StaticText mTxtCashAccount;
        private SAPbouiCOM.StaticText mTxtCashAmount;
        private SAPbouiCOM.StaticText mTxtCashReference;
        private SAPbouiCOM.EditText mEdtCashAccount;
        private SAPbouiCOM.EditText mEdtCashAmount;
        private SAPbouiCOM.EditText mEdtCashReference;
        private SAPbouiCOM.StaticText mTxtAdvancesHeader;
        private SAPbouiCOM.Matrix mMtxClientAccountPayments;
        private SAPbouiCOM.Matrix mMtxAdvances;
        private SAPbouiCOM.Matrix mMtxGeneratedDocs;
        private SAPbouiCOM.StaticText mTxtClient;
        private SAPbouiCOM.ComboBox mCmbPaymentMethod;
        private SAPbouiCOM.CheckBox mChkDrafts;
        private SAPbouiCOM.DataTable mDtGeneratedDocuments;
        #endregion

        public Payments()
        {
            Folder0.Item.Click();
        }

        #region Events
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
            /*try
            {*/
            if (!FormUID.Equals(this.UIAPIRawForm.UniqueID))
            {
                return;
            }
            if (!pVal.BeforeAction)
            {
                switch (pVal.EventType)
                {
                    // Choose from Lists
                    case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST:
                        if (pVal.Action_Success)
                        {
                            SAPbouiCOM.IChooseFromListEvent lObjCFLEvent = (SAPbouiCOM.IChooseFromListEvent)pVal;
                            if (lObjCFLEvent.SelectedObjects != null)
                            {
                                SAPbouiCOM.DataTable lObjDataTable = lObjCFLEvent.SelectedObjects;
                                // Client Selector
                                if (lObjDataTable.UniqueID == mEdtClient.ChooseFromListUID)
                                {
                                    mStrCardCode = System.Convert.ToString(lObjDataTable.GetValue(0, 0));
                                    mStrCardName = System.Convert.ToString(lObjDataTable.GetValue(1, 0));
                                    mEdtName.Value = mStrCardName;
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = mStrCardCode;
                                    LoadClient(mStrCardCode);
                                    ReloadAmounts();
                                }
                                // Bank Account selector for Checks
                                else if (lObjDataTable.UniqueID == mObjCFLBankAcct.UniqueID)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(4, 0));

                                    mDtPayChecks.SetValue("C_AcctNum", pVal.Row - 1, lObjDataTable.GetValue(4, 0));
                                    mMtxPayChecks.LoadFromDataSource();
                                }
                                // GL Account selector for Check Deposit
                                else if (lObjDataTable.UniqueID == mObjCFLDepositAcct.UniqueID)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(4, 0));

                                    mDtPayChecks.SetValue("C_DpAcct", pVal.Row - 1, lObjDataTable.GetValue(0, 0));
                                    mMtxPayChecks.LoadFromDataSource();
                                }
                                // GL Account selectors for Checks, Transfers, Cash and others
                                else if (lObjDataTable.UniqueID == mObjCFLTransfersGL.UniqueID || lObjDataTable.UniqueID == mObjCFLCashGL.UniqueID || lObjDataTable.UniqueID == mObjCFLChecksGL.UniqueID)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(0, 0));
                                }
                                // Credit Card selector
                                else if (lObjDataTable.UniqueID == mObjCFLCreditCardGL.UniqueID)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(0, 0));

                                    mDtPayCreditCards.SetValue("C_CrdAcct", pVal.Row - 1, lObjDataTable.GetValue(0, 0));
                                    mMtxPayCreditCards.LoadFromDataSource();
                                }
                            }
                        }
                        break;
                    case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED:
                        // Drafts Checkbox
                        if (pVal.ItemUID == mChkDrafts.Item.UniqueID && mStrCardCode != null)
                        {
                            LoadClient(mStrCardCode);
                        }
                        // Client Invoices Checkbox
                        else if (pVal.ColUID == "C_Selected" && pVal.ItemUID == mMtxClientInvoices.Item.UniqueID)
                        {
                            if (pVal.Row > mMtxClientInvoices.RowCount)
                            {
                                return;
                            }
                            this.FlushValueToSource(mMtxClientInvoices, pVal.ColUID, pVal.Row);
                            ReloadAmounts();
                        }
                        // Client Account Payments Checkbox
                        else if (pVal.ColUID == "C_Selected" && pVal.ItemUID == mMtxClientAccountPayments.Item.UniqueID)
                        {
                            if (pVal.Row > mMtxClientAccountPayments.RowCount)
                            {
                                return;
                            }
                            this.FlushValueToSource(mMtxClientAccountPayments, pVal.ColUID, pVal.Row);
                            ReloadAmounts();
                        }
                        // Checks deposit checkboxx
                        else if (pVal.ColUID == "C_Deposit")
                        {
                            if (pVal.Row > mMtxPayChecks.RowCount)
                            {
                                return;
                            }
                            bool lBolChecked = (mMtxPayChecks.Columns.Item("C_Deposit").Cells.Item(pVal.Row).Specific as SAPbouiCOM.CheckBox).Checked;
                            mDtPayChecks.Columns.Item("C_Deposit").Cells.Item(pVal.Row - 1).Value = lBolChecked ? "Y" : "N";
                        }
                        break;
                    case SAPbouiCOM.BoEventTypes.et_COMBO_SELECT:
                        // Bank selector
                        if (pVal.ColUID == "C_BankCode" && pVal.ItemUID == mMtxPayChecks.Item.UniqueID)
                        {
                            this.FlushValueToSource(mMtxPayChecks, pVal.ColUID, pVal.Row);
                        }
                        // Credit Card selector
                        else if (pVal.ColUID == "C_CrdCard" && pVal.ItemUID == mMtxPayCreditCards.Item.UniqueID)
                        {
                            string lStrCreditCard = (mMtxPayCreditCards.GetCellSpecific(pVal.ColUID, pVal.Row) as SAPbouiCOM.ComboBox).Value;
                            this.FlushValueToSource(mMtxPayCreditCards, pVal.ColUID, pVal.Row);
                            // Update Account
                            string lStrAcct = mQueryManager.GetValue("AcctCode", "CreditCard", lStrCreditCard, "OCRC");
                            mDtPayCreditCards.Columns.Item("C_CrdAcct").Cells.Item(pVal.Row - 1).Value = lStrAcct;
                            mMtxPayCreditCards.LoadFromDataSource();
                        }
                        break;
                    case SAPbouiCOM.BoEventTypes.et_VALIDATE:
                        //Update datatable when altering invoice's amount to pay and update amounts
                        if (pVal.ItemUID == mMtxClientInvoices.Item.UniqueID && pVal.ItemChanged && pVal.ColUID == "C_DocRem")
                        {
                            this.FlushValueToSource(mMtxClientInvoices, pVal.ColUID, pVal.Row);
                            ReloadAmounts();
                        }
                        //Update datatable when altering invoice's amount to pay and update amounts
                        else if (pVal.ItemUID == mMtxClientAccountPayments.Item.UniqueID && pVal.ItemChanged && pVal.ColUID == "C_Amount")
                        {
                            this.FlushValueToSource(mMtxClientAccountPayments, pVal.ColUID, pVal.Row);
                            ReloadAmounts();
                        }
                        // Add new row to checks matrix when the last row is modified
                        else if (pVal.ItemUID == mMtxPayChecks.Item.UniqueID && pVal.ItemChanged)
                        {
                            mMtxPayChecks.FlushToDataSource();
                            if (pVal.Row == mMtxPayChecks.RowCount)
                            {
                                mDtPayChecks.Rows.Add();
                                mDtPayChecks.SetValue("#", mDtPayChecks.Rows.Count - 1, mDtPayChecks.Rows.Count);
                            }
                            ReloadAmounts();
                            mMtxPayChecks.LoadFromDataSource();

                        }
                        // Add new row to checks matrix when the last row is modified
                        else if (pVal.ItemUID == mMtxPayCreditCards.Item.UniqueID && pVal.ItemChanged)
                        {
                            mMtxPayCreditCards.FlushToDataSource();
                            if (pVal.Row == mMtxPayCreditCards.RowCount)
                            {
                                mDtPayCreditCards.Rows.Add();
                                mDtPayCreditCards.SetValue("#", mDtPayCreditCards.Rows.Count - 1, mDtPayCreditCards.Rows.Count);
                            }
                            ReloadAmounts();
                            mMtxPayCreditCards.LoadFromDataSource();

                        }
                        // Update totals when editing transfers or cash amounts
                        else if (pVal.ItemUID == mEdtTransAmount.Item.UniqueID || pVal.ItemUID == mEdtCashAmount.Item.UniqueID)
                        {
                            ReloadAmounts();
                        }
                        break;
                    case SAPbouiCOM.BoEventTypes.et_CLICK:
                        if (pVal.ItemUID == mBtnSave.Item.UniqueID && mBtnSave.Item.Enabled)
                        {
                            GenerateDocuments();
                        }
                        break;
                    case SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED:
                        if (pVal.ItemUID == mMtxClientInvoices.Item.UniqueID)
                        {
                            string lStrDocEntry = mDtClientInvoices.GetCellValue<string>("C_DocEntry", pVal.Row - 1);
                            int lIntObjType = mDtClientInvoices.GetCellValue<int>("C_ObjType", pVal.Row - 1);
                            if (mChkDrafts.Checked)
                            {
                                lIntObjType = 112;
                            }
                            Application.SBO_Application.OpenForm((SAPbouiCOM.BoFormObjectEnum)lIntObjType, "", lStrDocEntry);
                        }
                        else if (pVal.ItemUID == mMtxClientAccountPayments.Item.UniqueID)
                        {
                            string lStrDocEntry = mDtClientAccountPayments.GetCellValue<string>("C_DocEntry", pVal.Row - 1);
                            //int lIntObjType = GetCellValue<int>(mDtClientAccountPayments, "C_ObjType", pVal.Row - 1);
                            Application.SBO_Application.OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_Receipt, "", lStrDocEntry);
                        }
                        else if (pVal.ItemUID == mMtxGeneratedDocs.Item.UniqueID)
                        {
                            string lStrDocEntry = mDtGeneratedDocuments.GetCellValue<string>("C_DocEntry", pVal.Row - 1);
                            int lIntObjType = mDtGeneratedDocuments.GetCellValue<int>("C_ObjType", pVal.Row - 1);
                            Application.SBO_Application.OpenForm((SAPbouiCOM.BoFormObjectEnum)lIntObjType, "", lStrDocEntry);
                        }
                        else if (pVal.ItemUID == mMtxClientChecks.Item.UniqueID)
                        {
                            string lStrDocEntry = mDtClientChecks.GetCellValue<string>("C_DocNum", pVal.Row - 1);
                            Application.SBO_Application.OpenForm((SAPbouiCOM.BoFormObjectEnum)140, "", lStrDocEntry);
                        }
                        break;

                    case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                        UnloadEvents();
                        break;
                }
            }
            else
            {
                switch (pVal.EventType)
                {
                    case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST:
                        SAPbouiCOM.IChooseFromListEvent lObjCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pVal;
                        string lStrCflUID = lObjCFLEvento.ChooseFromListUID;
                        SAPbouiCOM.ChooseFromList lObjCFL = UIAPIRawForm.ChooseFromLists.Item(lStrCflUID);
                        if (lStrCflUID == mObjCFLBankAcct.UniqueID)
                        {
                            PopulateAccountCFL(lObjCFL, pVal);
                        }
                        break;
                }
            }
            /*
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(ex.Message);
            }*/
        }

        private void PopulateAccountCFL(SAPbouiCOM.ChooseFromList pObjCFL, SAPbouiCOM.ItemEvent pVal)
        {
            SAPbouiCOM.Conditions lObjCons = new SAPbouiCOM.Conditions();
            pObjCFL.SetConditions(lObjCons);

            SAPbouiCOM.Condition lObjCondition = lObjCons.Add();
            lObjCondition.Alias = "CardCode";
            lObjCondition.CondVal = mStrCardCode;
            lObjCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

            lObjCondition = lObjCons.Add();
            lObjCondition.Alias = "BankCode";
            lObjCondition.CondVal = (mMtxPayChecks.Columns.Item("C_BankCode").Cells.Item(pVal.Row).Specific as SAPbouiCOM.ComboBox).Value.ToString();
            lObjCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;

            pObjCFL.SetConditions(lObjCons);
        }

        private void LoadClient(string pCardCode)
        {
            this.UIAPIRawForm.Freeze(true);
            try
            {
                mDtClientInvoices.Rows.Clear();
                int i = 0;
                if (!mChkDrafts.Checked)
                {
                    IList<InvoiceDTO> lLstInvoices = mDocumentDAO.GetClientInvoices(pCardCode, "O");
                    foreach (InvoiceDTO lObjinvoice in lLstInvoices)
                    {
                        mDtClientInvoices.Rows.Add();
                        mDtClientInvoices.SetValue("#", i, i + 1);
                        mDtClientInvoices.SetValue("C_DocNum", i, lObjinvoice.DocNum);
                        mDtClientInvoices.SetValue("C_DocDate", i, lObjinvoice.DocDate);
                        mDtClientInvoices.SetValue("C_DcDueDt", i, lObjinvoice.DocDueDate);
                        mDtClientInvoices.SetValue("C_DocCur", i, lObjinvoice.DocCur);
                        mDtClientInvoices.SetValue("C_OcrCode", i, lObjinvoice.OcrCode);
                        mDtClientInvoices.SetValue("C_DocTotal", i, lObjinvoice.DocTotal);
                        mDtClientInvoices.SetValue("C_DocRem", i, lObjinvoice.DocRemaining);
                        mDtClientInvoices.SetValue("C_Series", i, lObjinvoice.SeriesName);
                        // The following are not shown in the matrix, only internal
                        mDtClientInvoices.SetValue("C_DocEntry", i, lObjinvoice.DocEntry);
                        mDtClientInvoices.SetValue("C_ObjType", i, lObjinvoice.ObjType);
                        i++;

                    }
                }
                else
                {
                    IList<DraftDTO> lLstDrafts = mDocumentDAO.GetClientInvoiceDrafts(pCardCode);
                    int lIntUserSign = DIApplication.Company.UserSignature;
                    string lStrCostingCenter = mQueryManager.GetValue("U_GLO_CostCenter", "USERID", lIntUserSign.ToString(), "OUSR");
                    bool lBolHasRole = mAuctionDAO.HasRole(DIApplication.Company.UserSignature, "CGRAL");
                    foreach (DraftDTO lObjDraft in lLstDrafts)
                    {
                        if (lObjDraft.OcrCode == lStrCostingCenter || lObjDraft.UserSign == lIntUserSign || (lBolHasRole && lObjDraft.U_GLO_CashRegister == "Y"))
                        {
                            mDtClientInvoices.Rows.Add();
                            mDtClientInvoices.SetValue("#", i, i + 1);
                            mDtClientInvoices.SetValue("C_DocNum", i, lObjDraft.DocNum);
                            mDtClientInvoices.SetValue("C_DocDate", i, lObjDraft.DocDate);
                            mDtClientInvoices.SetValue("C_DcDueDt", i, lObjDraft.DocDueDate);
                            mDtClientInvoices.SetValue("C_DocCur", i, lObjDraft.DocCur);
                            mDtClientInvoices.SetValue("C_OcrCode", i, lObjDraft.OcrCode);
                            mDtClientInvoices.SetValue("C_DocTotal", i, lObjDraft.DocTotal);
                            mDtClientInvoices.SetValue("C_DocRem", i, lObjDraft.DocTotal);
                            mDtClientInvoices.SetValue("C_Series", i, lObjDraft.SeriesName);
                            // The following are not shown in the matrix, only internal
                            mDtClientInvoices.SetValue("C_DocEntry", i, lObjDraft.DocEntry);
                            mDtClientInvoices.SetValue("C_ObjType", i, lObjDraft.ObjType);
                            i++;
                        }
                    }
                }
                mMtxClientInvoices.LoadFromDataSource();
                mMtxClientInvoices.AutoResizeColumns();

                IList<CheckDraftDTO> lLstCheckDrafts = mCheckDraftDAO.GetCheckDraftsByClient(pCardCode);
                mDtClientChecks.Rows.Clear();
                i = 0;
                foreach (CheckDraftDTO lObjCheck in lLstCheckDrafts)
                {
                    mDtClientChecks.Rows.Add();
                    mDtClientChecks.SetValue("#", i, i + 1);
                    mDtClientChecks.SetValue("C_DocNum", i, lObjCheck.DraftDocEntry);
                    mDtClientChecks.SetValue("C_DueDate", i, lObjCheck.DueDate);
                    mDtClientChecks.SetValue("C_CheckNum", i, lObjCheck.CheckNum);
                    mDtClientChecks.SetValue("C_CheckSum", i, lObjCheck.CheckSum);
                    mDtClientChecks.SetValue("C_DocDate", i, lObjCheck.DocDate);
                    mDtClientChecks.SetValue("C_Days", i, (lObjCheck.DueDate - DateTime.Now).Days);
                    i++;
                }
                mMtxClientChecks.LoadFromDataSource();
                mMtxClientChecks.AutoResizeColumns();

                LoadChecks(pCardCode);
                LoadTrasnfers();
                LoadCash();
                LoadCreditCards();
                LoadAccountPayments(pCardCode);

                mCmbPaymentMethod.Item.Enabled = true;
                mBtnSave.Item.Enabled = true;
                ReloadAmounts();
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void LoadAccountPayments(string pCardCode)
        {
            mMtxClientAccountPayments.Item.Enabled = true;
            IList<PaymentDTO> lLstPayments = mDocumentDAO.GetClientNoDocPayments(pCardCode);
            mDtClientAccountPayments.Rows.Clear();
            int i = 0;
            foreach (PaymentDTO lObjPayment in lLstPayments)
            {
                mDtClientAccountPayments.Rows.Add();
                mDtClientAccountPayments.SetValue("#", i, i + 1);
                mDtClientAccountPayments.SetValue("C_DocNum", i, lObjPayment.DocNum);
                mDtClientAccountPayments.SetValue("C_DocDate", i, lObjPayment.DocDate);
                mDtClientAccountPayments.SetValue("C_DocCur", i, lObjPayment.DocCur);
                mDtClientAccountPayments.SetValue("C_DocTotal", i, lObjPayment.DocTotal);
                mDtClientAccountPayments.SetValue("C_OpenBal", i, lObjPayment.OpenBal);
                mDtClientAccountPayments.SetValue("C_Amount", i, lObjPayment.OpenBal);
                // The following are not shown in the matrix, only internal
                mDtClientAccountPayments.SetValue("C_DocEntry", i, lObjPayment.DocEntry);
                mDtClientAccountPayments.SetValue("C_TransId", i, lObjPayment.TransId);
                i++;
            }
            mMtxClientAccountPayments.LoadFromDataSource();
            mMtxClientAccountPayments.AutoResizeColumns();
        }

        private void LoadCash()
        {
            mEdtCashAccount.Item.Enabled = true;
            mEdtCashAccount.DataBind.SetBound(true, "", mObjCFLCashGL.UniqueID);
            mEdtCashAccount.ChooseFromListUID = mObjCFLCashGL.UniqueID;

            mEdtCashAmount.Item.Enabled = true;
            mEdtCashReference.Item.Enabled = true;
        }

        private void LoadTrasnfers()
        {
            mEdtTransAccount.Item.Enabled = true;
            mEdtTransAccount.DataBind.SetBound(true, "", mObjCFLTransfersGL.UniqueID);
            mEdtTransAccount.ChooseFromListUID = mObjCFLTransfersGL.UniqueID;

            mEdtTransAmount.Item.Enabled = true;
            mEdtTransDate.Item.Enabled = true;
            mEdtTransReference.Item.Enabled = true;
        }

        private void LoadChecks(string pCardCode)
        {
            mEdtChecksAccount.Item.Enabled = true;
            mEdtChecksAccount.DataBind.SetBound(true, "", mObjCFLChecksGL.UniqueID);
            mEdtChecksAccount.ChooseFromListUID = mObjCFLChecksGL.UniqueID;

            string lStrChkAcct = mCheckDAO.GetCheckAcct(UIApplication.GetCompany().UserName);
            if (!string.IsNullOrEmpty(lStrChkAcct))
            {
                this.UIAPIRawForm.DataSources.UserDataSources.Item(mObjCFLChecksGL.UniqueID).ValueEx = lStrChkAcct;
                this.UIAPIRawForm.DataSources.UserDataSources.Item(mObjCFLCashGL.UniqueID).ValueEx = lStrChkAcct;
            }

            mMtxPayChecks.Item.Enabled = true;
            mDtPayChecks.Rows.Clear();
            mDtPayChecks.Rows.Add();
            mDtPayChecks.SetValue("#", 0, 1);

            mDtPayChecks.BindToMatrix(mMtxPayChecks);
            mMtxPayChecks.LoadFromDataSource();

            SAPbouiCOM.Column lObjColumn = mMtxPayChecks.Columns.Item("C_BankCode");
            int lIntValuesLength = lObjColumn.ValidValues.Count;
            for (int i = 0; i < lIntValuesLength; i++)
            {
                lObjColumn.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
            }

            IList<BankDTO> lLstObjBanks = mBankDAO.GetClientBanks(pCardCode);
            foreach (BankDTO lObjBank in lLstObjBanks)
            {
                lObjColumn.ValidValues.Add(lObjBank.BankCode, lObjBank.BankName);
            }
            
        }

        private void LoadCreditCards()
        {
            mMtxPayCreditCards.Item.Enabled = true;
            mDtPayCreditCards.Rows.Clear();
            mDtPayCreditCards.Rows.Add();
            mDtPayCreditCards.SetValue("#", 0, 1);

            mDtPayCreditCards.BindToMatrix(mMtxPayCreditCards);

            mMtxPayCreditCards.LoadFromDataSource();

            SAPbouiCOM.Column lObjColumn = mMtxPayCreditCards.Columns.Item("C_CrdCard");
            lObjColumn.DisplayDesc = true;
            lObjColumn.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            int lIntValuesLength = lObjColumn.ValidValues.Count;
            for (int i = 0; i < lIntValuesLength; i++)
            {
                lObjColumn.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
            }

            IList<CreditCardDTO> lLstCreditCards = new CreditCardDAO().GetCreditCards();
            foreach (CreditCardDTO lObjCreditCard in lLstCreditCards)
            {
                lObjColumn.ValidValues.Add(lObjCreditCard.CreditCard.ToString(), lObjCreditCard.CardName);
            }

            AddGLAccountsConditions(mObjCFLCreditCardGL);
        }

        private void ReloadAmounts()
        {
            double lDblTotal = 0;
            // Reload Total
            for (int i = 0; i < mDtClientInvoices.Rows.Count; i++)
            {
                if (mDtClientInvoices.GetCellValue<string>("C_Selected", i) == "Y")
                {
                    lDblTotal += mDtClientInvoices.GetCellValue<double>("C_DocRem", i);
                }
            }
            this.UIAPIRawForm.DataSources.UserDataSources.Item("UD_Total").ValueEx = lDblTotal.ToString(CultureInfo.InvariantCulture);

            // Reload Current
            double lDblCurrent = 0;
            // Checks
            for (int i = 0; i < mDtPayChecks.Rows.Count; i++)
            {
                lDblCurrent += mDtPayChecks.GetCellValue<double>("C_CheckSum", i);
            }
            // Transfer
            double lDbl = Convert.ToDouble("0.00", CultureInfo.InvariantCulture);
            lDblCurrent += this.GetDataSourceValue<double>("UD_TAmount");
            // Cash/Other
            lDblCurrent += this.GetDataSourceValue<double>("UD_CAmount");
            // Credit Cards
            for (int i = 0; i < mDtPayCreditCards.Rows.Count; i++)
            {
                lDblCurrent += mDtPayCreditCards.GetCellValue<double>("C_CrdSum", i);
            }
            // No Doc Payments
            for (int i = 0; i < mDtClientAccountPayments.Rows.Count; i++)
            {
                if (mDtClientAccountPayments.GetCellValue<string>("C_Selected", i) == "Y")
                {
                    lDblCurrent += mDtClientAccountPayments.GetCellValue<double>("C_Amount", i);
                }
            }

            this.UIAPIRawForm.DataSources.UserDataSources.Item("UD_Current").ValueEx = lDblCurrent.ToString(CultureInfo.InvariantCulture);
            this.UIAPIRawForm.DataSources.UserDataSources.Item("UD_Pending").ValueEx = (lDblTotal - lDblCurrent).ToString(CultureInfo.InvariantCulture);
        }
        #endregion


        #region Initializers
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mTxtClient = ((SAPbouiCOM.StaticText)(this.GetItem("txtClient").Specific));
            this.mTxtName = ((SAPbouiCOM.StaticText)(this.GetItem("txtName").Specific));
            this.mEdtClient = ((SAPbouiCOM.EditText)(this.GetItem("edtClient").Specific));
            this.mEdtName = ((SAPbouiCOM.EditText)(this.GetItem("edtName").Specific));
            this.mEdtDate = ((SAPbouiCOM.EditText)(this.GetItem("edtDate").Specific));
            this.mTxtDate = ((SAPbouiCOM.StaticText)(this.GetItem("txtDate").Specific));
            this.Folder0 = ((SAPbouiCOM.Folder)(this.GetItem("Item_7").Specific));
            this.Folder1 = ((SAPbouiCOM.Folder)(this.GetItem("Item_8").Specific));
            this.Folder2 = ((SAPbouiCOM.Folder)(this.GetItem("Item_9").Specific));
            this.Folder3 = ((SAPbouiCOM.Folder)(this.GetItem("Item_10").Specific));
            this.mMtxClientInvoices = ((SAPbouiCOM.Matrix)(this.GetItem("mtxInvs").Specific));
            this.mMtxClientChecks = ((SAPbouiCOM.Matrix)(this.GetItem("mtxDrafts").Specific));
            this.mEdtTotal = ((SAPbouiCOM.EditText)(this.GetItem("edtTotal").Specific));
            this.mTxtTotal = ((SAPbouiCOM.StaticText)(this.GetItem("txtTotal").Specific));
            this.mTxtAdvance = ((SAPbouiCOM.StaticText)(this.GetItem("txtAdvance").Specific));
            this.mTxtPending = ((SAPbouiCOM.StaticText)(this.GetItem("txtPend").Specific));
            this.mEdtAdvance = ((SAPbouiCOM.EditText)(this.GetItem("edtAdvance").Specific));
            this.mEdtPending = ((SAPbouiCOM.EditText)(this.GetItem("edtPend").Specific));
            this.mBtnSave = ((SAPbouiCOM.Button)(this.GetItem("btnSave").Specific));
            this.mTxtCurrency = ((SAPbouiCOM.StaticText)(this.GetItem("txtCurr").Specific));
            this.mEdtCurrency = ((SAPbouiCOM.EditText)(this.GetItem("edtCurr").Specific));
            this.mTxtChecksHeader = ((SAPbouiCOM.StaticText)(this.GetItem("txtChHead").Specific));
            this.mMtxPayChecks = ((SAPbouiCOM.Matrix)(this.GetItem("mtxChecks").Specific));
            this.mTxtCreditHeader = ((SAPbouiCOM.StaticText)(this.GetItem("txtCrHead").Specific));
            this.mMtxPayCreditCards = ((SAPbouiCOM.Matrix)(this.GetItem("mtxCards").Specific));
            this.mTxtTransHeader = ((SAPbouiCOM.StaticText)(this.GetItem("txtTHead").Specific));
            this.mTxtTransAccount = ((SAPbouiCOM.StaticText)(this.GetItem("txtTAcct").Specific));
            this.mTxtTransDate = ((SAPbouiCOM.StaticText)(this.GetItem("txtTDate").Specific));
            this.mTxtTransReference = ((SAPbouiCOM.StaticText)(this.GetItem("txtTRef").Specific));
            this.mTxtTransAmount = ((SAPbouiCOM.StaticText)(this.GetItem("txtTAmount").Specific));
            this.mEdtTransAccount = ((SAPbouiCOM.EditText)(this.GetItem("edtTAcct").Specific));
            this.mEdtTransDate = ((SAPbouiCOM.EditText)(this.GetItem("edtTDate").Specific));
            this.mEdtTransReference = ((SAPbouiCOM.EditText)(this.GetItem("edtTRef").Specific));
            this.mEdtTransAmount = ((SAPbouiCOM.EditText)(this.GetItem("edtTAmnt").Specific));
            this.mTxtCashHeader = ((SAPbouiCOM.StaticText)(this.GetItem("txtCHead").Specific));
            this.mTxtCashType = ((SAPbouiCOM.StaticText)(this.GetItem("txtCType").Specific));
            this.mTxtCashAccount = ((SAPbouiCOM.StaticText)(this.GetItem("txtCAccnt").Specific));
            this.mTxtCashAmount = ((SAPbouiCOM.StaticText)(this.GetItem("txtCAmount").Specific));
            this.mTxtCashReference = ((SAPbouiCOM.StaticText)(this.GetItem("txtCRef").Specific));
            this.mEdtCashAccount = ((SAPbouiCOM.EditText)(this.GetItem("edtCAcct").Specific));
            this.mEdtCashAmount = ((SAPbouiCOM.EditText)(this.GetItem("edtCAmount").Specific));
            this.mEdtCashReference = ((SAPbouiCOM.EditText)(this.GetItem("edtCRef").Specific));
            this.mTxtAdvancesHeader = ((SAPbouiCOM.StaticText)(this.GetItem("txtAHead").Specific));
            this.mMtxClientAccountPayments = ((SAPbouiCOM.Matrix)(this.GetItem("mtxAcctPay").Specific));
            this.mMtxAdvances = ((SAPbouiCOM.Matrix)(this.GetItem("mtxAdvance").Specific));
            this.mMtxGeneratedDocs = ((SAPbouiCOM.Matrix)(this.GetItem("mtxGenDocs").Specific));
            this.mEdtChecksAccount = ((SAPbouiCOM.EditText)(this.GetItem("edtCheckGL").Specific));
            this.mTxtChecksAccount = ((SAPbouiCOM.StaticText)(this.GetItem("txtCheckGL").Specific));
            this.mCmbPaymentMethod = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbCType").Specific));
            this.mChkDrafts = ((SAPbouiCOM.CheckBox)(this.GetItem("cbDraft").Specific));
            //this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            //this.Button0.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
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

            mObjCFLClients = InitChooseFromLists(false, "2", "CFL_Client", this.UIAPIRawForm.ChooseFromLists);
            mEdtClient.DataBind.SetBound(true, "", mObjCFLClients.UniqueID);
            mEdtClient.ChooseFromListUID = mObjCFLClients.UniqueID;

            SAPbouiCOM.Conditions lObjCons = mObjCFLClients.GetConditions();
            SAPbouiCOM.Condition lObjCon = lObjCons.Add();
            lObjCon.Alias = "CardType";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "C";
            mObjCFLClients.SetConditions(lObjCons);

            mObjCFLTransfersGL = InitChooseFromLists(false, "1", "CFL_GLTrns", this.UIAPIRawForm.ChooseFromLists);
            AddGLAccountsConditions(mObjCFLTransfersGL);
            mObjCFLCashGL = InitChooseFromLists(false, "1", "CFL_GLCash", this.UIAPIRawForm.ChooseFromLists);
            AddGLAccountsConditions(mObjCFLCashGL);
            mObjCFLChecksGL = InitChooseFromLists(false, "1", "CFL_GLChck", this.UIAPIRawForm.ChooseFromLists);
            AddGLAccountsConditions(mObjCFLChecksGL);

            mDtClientInvoices = this.UIAPIRawForm.DataSources.DataTables.Item("DT_ClientInvoices");
            mMtxClientInvoices.AutoResizeColumns();
            mDtClientInvoices.BindToMatrix(mMtxClientInvoices);

            mDtClientChecks = this.UIAPIRawForm.DataSources.DataTables.Item("DT_ClientChecks");
            mMtxClientInvoices.AutoResizeColumns();
            mDtClientChecks.BindToMatrix(mMtxClientChecks);

            InitChecksMatrix();
            mMtxAdvances.AutoResizeColumns();
            mMtxGeneratedDocs.AutoResizeColumns();

            mDtClientAccountPayments = this.UIAPIRawForm.DataSources.DataTables.Item("DT_PayAccountPayments");
            mMtxClientAccountPayments.AutoResizeColumns();
            mDtClientAccountPayments.BindToMatrix(mMtxClientAccountPayments);

            InitCreditCardsMatrix();


            IList<PaymentMethodDTO> mLstPaymentMethods = mPaymentMethodDAO.GetPaymentMethods();
            foreach (PaymentMethodDTO lPaymentMethod in mLstPaymentMethods)
            {
                mCmbPaymentMethod.ValidValues.Add(lPaymentMethod.PayMethCod, lPaymentMethod.Descript);
            }

            mDtGeneratedDocuments = this.UIAPIRawForm.DataSources.DataTables.Item("DT_Documents");
            mMtxGeneratedDocs.AutoResizeColumns();
            mDtGeneratedDocuments.BindToMatrix(mMtxGeneratedDocs);
        }

        private void InitChecksMatrix()
        {
            mMtxPayChecks.AutoResizeColumns();
            mDtPayChecks = this.UIAPIRawForm.DataSources.DataTables.Item("DT_PayChecks");

            mObjCFLBankAcct = InitChooseFromLists(false, "187", "CFL_Acct", this.UIAPIRawForm.ChooseFromLists);
            SAPbouiCOM.Column lObjColumnItem = mMtxPayChecks.Columns.Item("C_AcctNum");
            lObjColumnItem.DataBind.SetBound(true, "", mObjCFLBankAcct.UniqueID);
            lObjColumnItem.ChooseFromListUID = mObjCFLBankAcct.UniqueID;

            mObjCFLDepositAcct = InitChooseFromLists(false, "1", "CFL_DpAcct", this.UIAPIRawForm.ChooseFromLists);
            lObjColumnItem = mMtxPayChecks.Columns.Item("C_DpAcct");
            lObjColumnItem.DataBind.SetBound(true, "", mObjCFLDepositAcct.UniqueID);
            lObjColumnItem.ChooseFromListUID = mObjCFLDepositAcct.UniqueID;
            AddGLAccountsConditions(mObjCFLDepositAcct);

            SAPbouiCOM.Column lObjColumn = mMtxPayChecks.Columns.Item("C_BankCode");
            lObjColumn.DisplayDesc = true;
            lObjColumn.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
        }

        private void InitCreditCardsMatrix()
        {
            mMtxPayCreditCards.AutoResizeColumns();
            mDtPayCreditCards = this.UIAPIRawForm.DataSources.DataTables.Item("DT_PayCreditCards");

            mObjCFLCreditCardGL = InitChooseFromLists(false, "1", "CFL_GLCard", this.UIAPIRawForm.ChooseFromLists);
            SAPbouiCOM.Column lObjColumnItem = mMtxPayCreditCards.Columns.Item("C_CrdAcct");
            lObjColumnItem.DataBind.SetBound(true, "", mObjCFLCreditCardGL.UniqueID);
            lObjColumnItem.ChooseFromListUID = mObjCFLCreditCardGL.UniqueID;
        }

        /// <summary>
        /// inicia los datos a un ChooseFromList
        /// <summary>
        public SAPbouiCOM.ChooseFromList InitChooseFromLists(bool pbolMultiselecction, string pStrType, string pStrID, SAPbouiCOM.ChooseFromListCollection pObjCFLs) //
        {
            SAPbouiCOM.ChooseFromList lObjoCFL = null;
            try
            {
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = (SAPbouiCOM.ChooseFromListCreationParams)UIApplication.GetApplication().CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams);

                oCFLCreationParams.MultiSelection = pbolMultiselecction;
                oCFLCreationParams.ObjectType = pStrType;
                oCFLCreationParams.UniqueID = pStrID;

                lObjoCFL = pObjCFLs.Add(oCFLCreationParams);

                this.UIAPIRawForm.DataSources.UserDataSources.Add(pStrID, SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 254);
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitCustomerChooseFromListException: {0}", ex.Message));
                LogService.WriteError("(InitChooseFromLists): " + ex.Message);
                LogService.WriteError(ex);
            }
            return lObjoCFL;
        }

        private void AddGLAccountsConditions(SAPbouiCOM.ChooseFromList pCFL)
        {
            SAPbouiCOM.Conditions lObjConditions = pCFL.GetConditions();

            SAPbouiCOM.Condition lObjCon = lObjConditions.Add();
            lObjCon.Alias = "Postable";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "Y";
            pCFL.SetConditions(lObjConditions);
        }



        #endregion

        private void GenerateDocuments()
        {
            mBtnSave.Item.Enabled = false;

            List<int> lChecksToDeposit = new List<int>();
            int lIntDocLine = 0;
            string lStrUsername = DIApplication.Company.UserName;
            string lStrCostingCenter = mQueryManager.GetValue("U_GLO_CostCenter", "USER_CODE", lStrUsername, "OUSR");

            SAPbobsCOM.Payments lObjPayment = (SAPbobsCOM.Payments)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);
            lObjPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
            lObjPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments;
            lObjPayment.CardCode = mStrCardCode;
            if (mEdtCashReference.Value != "")
            {
                lObjPayment.CounterReference = mEdtCashReference.Value;
            }

            lObjPayment.UserFields.Fields.Item("U_GLO_PaymentType").Value = "GLPGO";
            lObjPayment.UserFields.Fields.Item("U_FZ_AuxiliarType").Value = "1";
            lObjPayment.UserFields.Fields.Item("U_FZ_Auxiliar").Value = mStrCardCode;
            lObjPayment.UserFields.Fields.Item("U_GLO_CostCenter").Value = lStrCostingCenter;

            if (this.GetDataSourceValue<double>("UD_Total") <= 0 || this.GetDataSourceValue<double>("UD_Current") <= 0)
            {
                UIApplication.ShowError("La cantidad a pagar es cero.");
                mBtnSave.Item.Enabled = true;
                return;
            }
            if (this.GetDataSourceValue<double>("UD_Pending") > 0)
            {
                UIApplication.ShowError("No se ha cubierto el saldo pendiente.");
                mBtnSave.Item.Enabled = true;
                return;
            }

            #region Cash
            double lDlbCashSum = this.GetDataSourceValue<double>("UD_CAmount");
            // Ignore if zero
            if (lDlbCashSum > 0)
            {
                // Payment Method
                if (mCmbPaymentMethod.Value == "")
                {
                    UIApplication.ShowError("No se especifico metodo de pago.");
                    mBtnSave.Item.Enabled = true;
                    return;
                }

                // GL Account
                if (mEdtCashAccount.Value == "")
                {
                    UIApplication.ShowError("No se especifico cuenta contable para efectivo/otros.");
                    mBtnSave.Item.Enabled = true;
                    return;
                }
                lObjPayment.CashAccount = mEdtCashAccount.Value;
                lObjPayment.CashSum = lDlbCashSum;
            }
            #endregion

            #region Bank Transfer
            double lDblTransferSum = this.GetDataSourceValue<double>("UD_TAmount");
            // Ignore if zero
            if (lDblTransferSum > 0)
            {
                // Reference
                if (mEdtTransReference.Value == "")
                {
                    UIApplication.ShowError("No se especifico referencia para transferencia bancaria.");
                    mBtnSave.Item.Enabled = true;
                    return;
                }
                // GL Account
                if (mEdtTransAccount.Value == "")
                {
                    UIApplication.ShowError("No se especifico cuenta contable para transferencia bancaria.");
                    mBtnSave.Item.Enabled = true;
                    return;
                }
                // Transfer date
                DateTime lDteTransfer = this.GetDataSourceValue<DateTime>("UD_TDate");
                if (lDteTransfer == null)
                {
                    UIApplication.ShowError("No se especifico fecha de transferencia bancaria.");
                    mBtnSave.Item.Enabled = true;
                    return;
                }
                if (lDteTransfer > DateTime.Now)
                {
                    UIApplication.ShowError("La fecha de transferencia bancaria debe ser una fecha anterior.");
                    mBtnSave.Item.Enabled = true;
                    return;
                }
                lObjPayment.TransferAccount = mEdtTransAccount.Value;
                lObjPayment.TransferSum = lDblTransferSum;
                lObjPayment.TransferReference = mEdtTransReference.Value;
                lObjPayment.TransferDate = lDteTransfer;
            }
            #endregion

            #region Checks
            double lDblCheckTotal = 0;
            for (int i = 0; i < mMtxPayChecks.RowCount; i++)
            {
                double lDblCheckSum = mDtPayChecks.GetCellValue<double>("C_CheckSum", i);
                if (lDblCheckSum <= 0)
                {
                    continue;
                }
                // GL Account
                if (mEdtChecksAccount.Value == "")
                {
                    UIApplication.ShowError("No se especifico una cuenta para cheques.");
                    mBtnSave.Item.Enabled = true;
                    return;
                }

                // Due Date
                DateTime lDteDueDate = mDtPayChecks.GetCellValue<DateTime>("C_DueDate", i);
                if (lDteDueDate == null)
                {
                    UIApplication.ShowError(String.Format("No se especifico fecha de vencimiento en el cheque no. {0}", i + 1));
                    mBtnSave.Item.Enabled = true;
                    return;
                }
                if (lDteDueDate < DateTime.Now)
                {
                    UIApplication.ShowError(String.Format("El cheque no. {0} esta vencido o vence hoy.", i + 1));
                    mBtnSave.Item.Enabled = true;
                    return;
                }


                // Bank
                string lStrBankCode = mDtPayChecks.GetCellValue<string>("C_BankCode", i);
                if (lStrBankCode == "")
                {
                    UIApplication.ShowError(String.Format("No se especifico banco en el cheque no. {0}", i + 1));
                    mBtnSave.Item.Enabled = true;
                    return;
                }


                // Bank Account
                string lStrAccountCode = mDtPayChecks.GetCellValue<string>("C_AcctNum", i);
                if (lStrAccountCode == "")
                {
                    UIApplication.ShowError(String.Format("No se especifico cuenta en el cheque no. {0}", i + 1));
                    mBtnSave.Item.Enabled = true;
                    return;
                }

                // Check Number
                int lStrCheckNum = mDtPayChecks.GetCellValue<int>("C_CheckNum", i);
                if (lStrCheckNum == 0)
                {
                    UIApplication.ShowError(String.Format("No se especifico folio en cheque no. {0}", i + 1));
                    mBtnSave.Item.Enabled = true;
                    return;
                }

                // Deposit Check
                if (mDtPayChecks.GetCellValue<string>("C_Deposit", i) == "Y")
                {
                    if (mDtPayChecks.GetCellValue<string>("C_DpAcct", i) == "")
                    {
                        UIApplication.ShowError(String.Format("No se cuenta de deposito para cheque {0}", i + 1));
                        mBtnSave.Item.Enabled = true;
                        return;
                    }
                    lChecksToDeposit.Add(i);
                }
                lObjPayment.Checks.CheckSum = lDblCheckSum;
                lObjPayment.CheckAccount = mEdtChecksAccount.Value;
                lObjPayment.Checks.BankCode = lStrBankCode;
                lObjPayment.Checks.DueDate = lDteDueDate;
                lObjPayment.Checks.CheckNumber = lStrCheckNum;
                lObjPayment.Checks.AccounttNum = lStrAccountCode;
                lObjPayment.Checks.Add();
                lDblCheckTotal += lDblCheckSum;
            }
            #endregion

            #region Credit Cards
            double lDblCreditTotal = 0;
            for (int i = 0; i < mMtxPayCreditCards.RowCount; i++)
            {
                double lDblCreditSum = mDtPayCreditCards.GetCellValue<double>("C_CrdSum", i);
                if (lDblCreditSum <= 0)
                {
                    continue;
                }

                // Credit Card
                int lIntCreditCard = mDtPayCreditCards.GetCellValue<int>("C_CrdCard", i);
                if (lIntCreditCard == 0)
                {
                    UIApplication.ShowError(String.Format("No se especifico tarjeta de crédito en fila no. {0}", i + 1));
                    mBtnSave.Item.Enabled = true;
                    return;
                }

                // GL Account
                string lStrAccount = mDtPayCreditCards.GetCellValue<string>("C_CrdAcct", i);
                if (lStrAccount == "")
                {
                    UIApplication.ShowError(String.Format("No se especifico cuenta contable en tarjeta de crédito no. {0}", i + 1));
                    mBtnSave.Item.Enabled = true;
                    return;
                }

                // Card Expiration Date
                DateTime lDteValid = mDtPayCreditCards.GetCellValue<DateTime>("C_CrdValid", i);
                if (lDteValid == null)
                {
                    UIApplication.ShowError(String.Format("No se especifico fecha de vencimiento en la tarjeta no. {0}", i + 1));
                    mBtnSave.Item.Enabled = true;
                    return;
                }
                if (lDteValid < DateTime.Now)
                {
                    UIApplication.ShowError(String.Format("La tarjeta no. {0} esta vencida o vence hoy.", i + 1));
                    mBtnSave.Item.Enabled = true;
                    return;
                }

                // Card Number
                string lStrCardNum = mDtPayCreditCards.GetCellValue<string>("C_CrdNum", i);
                int lIntCardNum;
                bool lBolNumeric = int.TryParse(lStrCardNum, out lIntCardNum);
                if (!lBolNumeric)
                {
                    UIApplication.ShowError(String.Format("Número de tarjeta invalido en la fila no. {0}.", i + 1));
                    mBtnSave.Item.Enabled = true;
                    return;
                }
                if (lStrCardNum.Length < 4)
                {
                    UIApplication.ShowError(String.Format("Número de tarjeta incompleto en la fila no. {0}. Se necesitan los ultimos 4 digitos.", i + 1));
                    mBtnSave.Item.Enabled = true;
                    return;
                }


                // Voucher Number
                string lStrVoucherNum = mDtPayCreditCards.GetCellValue<string>("C_VchNum", i);
                if (lStrVoucherNum == "")
                {
                    UIApplication.ShowError(String.Format("No se especifico número de voucher en la tarjeta no. {0}.", i + 1));
                    mBtnSave.Item.Enabled = true;
                    return;
                }

                lObjPayment.CreditCards.CreditCard = lIntCreditCard;
                lObjPayment.CreditCards.CreditAcct = lStrAccount;
                lObjPayment.CreditCards.CardValidUntil = lDteValid;
                lObjPayment.CreditCards.CreditSum = lDblCreditSum;
                lObjPayment.CreditCards.CreditCardNumber = lStrCardNum;
                lObjPayment.CreditCards.VoucherNum = lStrVoucherNum;
                lObjPayment.CreditCards.Add();
                lDblCreditTotal += lDblCreditSum;

            }
            #endregion

            double lDblSum = 0;
            if (!mChkDrafts.Checked)
            {
                #region Invoices
                // Add Invoice for every selected item in the matrix
                for (int i = 0; i < mDtClientInvoices.Rows.Count; i++)
                {
                    if (mDtClientInvoices.GetCellValue<string>("C_Selected", i) == "Y" && mDtClientInvoices.GetCellValue<double>("C_DocRem", i) > 0)
                    {
                        lObjPayment.Invoices.DocLine = lIntDocLine++;
                        lObjPayment.Invoices.DocEntry = mDtClientInvoices.GetCellValue<int>("C_DocEntry", i);
                        lObjPayment.Invoices.InvoiceType = (SAPbobsCOM.BoRcptInvTypes)mDtClientInvoices.GetCellValue<int>("C_ObjType", i); ;
                        double lDblSumApplied = mDtClientInvoices.GetCellValue<double>("C_DocRem", i);
                        lObjPayment.Invoices.SumApplied = lDblSumApplied;
                        lObjPayment.Invoices.Add();
                        lDblSum += lDblSumApplied;
                    }
                }
                #endregion
            }
            else
            {
                #region Drafts
                for (int i = 0; i < mDtClientInvoices.Rows.Count; i++)
                {
                    if (mDtClientInvoices.GetCellValue<string>("C_Selected", i) == "Y" && mDtClientInvoices.GetCellValue<double>("C_DocRem", i) > 0)
                    {
                        SAPbobsCOM.Documents lDraft = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                        lDraft.GetByKey(Convert.ToInt32(mDtClientInvoices.GetCellValue<double>("C_DocEntry", i)));
                        //lDraft.EDocExportFormat = -96;
                        //lDraft.EDocGenerationType = SAPbobsCOM.EDocGenerationTypeEnum.edocGenerate;
                        lDraft.EDocGenerationType = SAPbobsCOM.EDocGenerationTypeEnum.edocNotRelevant;
                        //lDraft.EDocSeries = lDraft.Series;
                        int lIntErrorDraft = lDraft.Update();
                        string lStrErrMsgDraft;
                        if (lIntErrorDraft != 0)
                        {
                            DIApplication.Company.GetLastError(out lIntErrorDraft, out lStrErrMsgDraft);
                            UIApplication.ShowError(String.Format("Error generando borrador: {0}", lStrErrMsgDraft));
                            mBtnSave.Item.Enabled = true;
                            return;
                        }
                        lIntErrorDraft = lDraft.SaveDraftToDocument();
                        int lDocEntry;
                        if (lIntErrorDraft != 0)
                        {
                            DIApplication.Company.GetLastError(out lIntErrorDraft, out lStrErrMsgDraft);
                            UIApplication.ShowError(String.Format("Error generando factura: {0}", lStrErrMsgDraft));
                            mBtnSave.Item.Enabled = true;
                            return;
                        }
                        else
                        {
                            lDocEntry = Convert.ToInt32(DIApplication.Company.GetNewObjectKey().ToString());
                        }
                        lObjPayment.Invoices.DocLine = lIntDocLine++;
                        lObjPayment.Invoices.DocEntry = lDocEntry;
                        lObjPayment.Invoices.InvoiceType = (SAPbobsCOM.BoRcptInvTypes)mDtClientInvoices.GetCellValue<int>("C_ObjType", i);
                        double lDblSumApplied = mDtClientInvoices.GetCellValue<double>("C_DocRem", i);
                        lObjPayment.Invoices.SumApplied = lDblSumApplied;
                        lObjPayment.Invoices.Add();
                        lDblSum += lDblSumApplied;
                    }
                }
                #endregion
            }

            #region Account Payments
            bool lBolUsingAccPay = false;
            for (int i = 0; i < mMtxClientAccountPayments.RowCount; i++)
            {
                if (mDtClientAccountPayments.GetCellValue<string>("C_Selected", i) == "Y" && mDtClientAccountPayments.GetCellValue<double>("C_Amount", i) > 0)
                {
                    lObjPayment.Invoices.DocLine = lIntDocLine++;
                    //lObjPayment.Invoices.DocEntry = GetCellValue<int>(mDtClientAccountPayments, "C_DocEntry", i);
                    lObjPayment.Invoices.DocEntry = mDtClientAccountPayments.GetCellValue<int>("C_TransId", i);
                    //lObjPayment.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_JournalEntry;
                    lObjPayment.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Receipt;
                    lObjPayment.Invoices.SumApplied = mDtClientAccountPayments.GetCellValue<double>("C_Amount", i) * -1;
                    lObjPayment.Invoices.Add();
                    lBolUsingAccPay = true;
                }
            }
            #endregion

            #region Payment Method
            // Payment Method based on the highest used
            if (lDblCheckTotal > Math.Max(Math.Max(lDblCreditTotal, lDblTransferSum), lDlbCashSum))
            {
                lObjPayment.UserFields.Fields.Item("U_B1SYS_PmntMethod").Value = "02";
            }
            else if (lDblCreditTotal > Math.Max(Math.Max(lDlbCashSum, lDblTransferSum), lDblCheckTotal))
            {
                lObjPayment.UserFields.Fields.Item("U_B1SYS_PmntMethod").Value = "04";
            }
            else if (lDblTransferSum > Math.Max(Math.Max(lDblCreditTotal, lDblCheckTotal), lDlbCashSum))
            {
                lObjPayment.UserFields.Fields.Item("U_B1SYS_PmntMethod").Value = "03";
            }
            else
            {
                lObjPayment.UserFields.Fields.Item("U_B1SYS_PmntMethod").Value = mCmbPaymentMethod.Value;
            }
            #endregion

            int intError = lObjPayment.Add();
            string lStrErrMsg;
            if (intError != 0)
            {
                mBtnSave.Item.Enabled = true;
                DIApplication.Company.GetLastError(out intError, out lStrErrMsg);
                UIApplication.ShowError(String.Format("Error generando pago: {0}", lStrErrMsg));
            }
            else
            {
                UIApplication.ShowSuccess("Pago generado exitosamente.");
                string lStrLastDoc = DIApplication.Company.GetNewObjectKey().ToString();
                mDtGeneratedDocuments.Rows.Clear();
                mDtGeneratedDocuments.Rows.Add();
                mDtGeneratedDocuments.SetValue("#", 0, "1");
                mDtGeneratedDocuments.SetValue("C_DocEntry", 0, lStrLastDoc);
                mDtGeneratedDocuments.SetValue("C_Status", 0, "OK");
                mDtGeneratedDocuments.SetValue("C_Comments", 0, "Pago recibido.");
                mDtGeneratedDocuments.SetValue("C_ObjType", 0, SAPbouiCOM.BoFormObjectEnum.fo_Receipt);
                if (lChecksToDeposit.Count > 0)
                {
                    foreach (int i in lChecksToDeposit)
                    {
                        CheckDTO lObjCheck = mCheckDAO.GetCheckByAttributes(
                            Convert.ToInt32(lStrLastDoc),
                            mDtPayChecks.GetCellValue<string>("C_AcctNum", i),
                            mDtPayChecks.GetCellValue<int>("C_CheckNum", i),
                            mDtPayChecks.GetCellValue<double>("C_CheckSum", i)
                        );
                        if (lObjCheck == null)
                        {
                            continue;
                        }
                        SAPbobsCOM.CompanyService lObjService = DIApplication.Company.GetCompanyService();
                        SAPbobsCOM.DepositsService lObjDepositService = lObjService.GetBusinessService(SAPbobsCOM.ServiceTypes.DepositsService) as SAPbobsCOM.DepositsService;
                        SAPbobsCOM.Deposit lObjDeposit = lObjDepositService.GetDataInterface(SAPbobsCOM.DepositsServiceDataInterfaces.dsDeposit) as SAPbobsCOM.Deposit;

                        lObjDeposit.DepositType = SAPbobsCOM.BoDepositTypeEnum.dtChecks;
                        lObjDeposit.DepositAccountType = SAPbobsCOM.BoDepositAccountTypeEnum.datBankAccount;
                        lObjDeposit.ReconcileAfterDeposit = SAPbobsCOM.BoYesNoEnum.tYES;
                        lObjDeposit.DepositAccount = mDtPayChecks.GetCellValue<string>("C_DpAcct", i);
                        SAPbobsCOM.CheckLine lObjCheckLine = lObjDeposit.Checks.Add();
                        lObjCheckLine.CheckKey = lObjCheck.CheckKey;

                        SAPbobsCOM.DepositParams lObjDepositParams = lObjDepositService.AddDeposit(lObjDeposit);

                        mDtGeneratedDocuments.Rows.Add();
                        int lIntRow = mDtGeneratedDocuments.Rows.Count - 1;
                        mDtGeneratedDocuments.SetValue("#", lIntRow, lIntRow + 1);
                        mDtGeneratedDocuments.SetValue("C_DocEntry", lIntRow, lObjDepositParams.DepositNumber);
                        mDtGeneratedDocuments.SetValue("C_Status", lIntRow, "OK");
                        mDtGeneratedDocuments.SetValue("C_Comments", lIntRow,
                            String.Format("Depósito de Cheque #{0}, de ${1:0.00}",
                            mDtPayChecks.GetCellValue<int>("C_CheckNum", i),
                            mDtPayChecks.GetCellValue<double>("C_CheckSum", i))
                            );
                        mDtGeneratedDocuments.SetValue("C_ObjType", lIntRow, SAPbouiCOM.BoFormObjectEnum.fo_Deposit);
                    }
                }
                mMtxGeneratedDocs.LoadFromDataSource();
            }
        }

        private void GenerateCheckDocuments(string pStrCostingCenter)
        {
            try
            {
                SAPbobsCOM.Payments lObjPayment = (SAPbobsCOM.Payments)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);
                lObjPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                lObjPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments;
                lObjPayment.CardCode = mStrCardCode;
                if (mEdtCashReference.Value != "")
                {
                    lObjPayment.CounterReference = mEdtCashReference.Value;
                }

                lObjPayment.UserFields.Fields.Item("U_GLO_PaymentType").Value = "GLPGO";
                lObjPayment.UserFields.Fields.Item("U_FZ_AuxiliarType").Value = "1";
                lObjPayment.UserFields.Fields.Item("U_FZ_Auxiliar").Value = mStrCardCode;
                lObjPayment.UserFields.Fields.Item("U_GLO_CostCenter").Value = pStrCostingCenter;
            }
            catch (Exception lObjException)
            {
                throw new Exception(string.Format("Error al crear los pagos para la sección de cheques: {0}", lObjException.Message));
            }
        }

        private List<int> GetSelectedIndexes(SAPbouiCOM.DataTable pDataTable, string pColumn)
        {
            List<int> lLstIntIndexes = new List<int>();
            for (int i = 0; i < pDataTable.Rows.Count; i++)
            {
                bool lBolSelected = pDataTable.Columns.Item(pColumn).Cells.Item(i).Value.ToString() == "Y";
                if (lBolSelected)
                {
                    lLstIntIndexes.Add(i);
                }
            }
            return lLstIntIndexes;
        }

        private SAPbouiCOM.Button Button0;

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                (sboObject as SAPbouiCOM.Button).Item.Enabled = false;
                SAPbobsCOM.Documents lObjInvoice = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                lObjInvoice.CardCode = "CL00000002";
                lObjInvoice.DocObjectCode = SAPbobsCOM.BoObjectTypes.oInvoices;
                lObjInvoice.DocDate = DateTime.Now;
                lObjInvoice.DocDueDate = DateTime.Now;
                lObjInvoice.HandWritten = SAPbobsCOM.BoYesNoEnum.tNO;
                lObjInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;
                lObjInvoice.Series = 4;
                lObjInvoice.PaymentMethod = "99";
                lObjInvoice.PaymentGroupCode = -1;
                lObjInvoice.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = "Por Definir";

                lObjInvoice.Lines.SetCurrentLine(0);
                lObjInvoice.Lines.ItemCode = "A00000001";
                lObjInvoice.Lines.Quantity = 1;
                lObjInvoice.Lines.UnitPrice = 10;   
                lObjInvoice.Lines.TaxCode = "V0";
                lObjInvoice.Lines.WarehouseCode = "CUNO";
                lObjInvoice.Lines.COGSCostingCode = "OG_GRAL";
                lObjInvoice.Lines.Add();

                lObjInvoice.EDocGenerationType = SAPbobsCOM.EDocGenerationTypeEnum.edocGenerate;
                lObjInvoice.EDocExportFormat = 1;
                //lObjInvoice.EDocSeries = 407;
                int lLongErr = lObjInvoice.Add();
                string lStrErrMsg;
                if (lLongErr != 0)
                {
                    DIApplication.Company.GetLastError(out lLongErr, out lStrErrMsg);
                    UIApplication.ShowError(lStrErrMsg);
                }
                else
                {
                    string lStrLastDoc = DIApplication.Company.GetNewObjectKey().ToString();
                    UIApplication.ShowSuccess(String.Format("Documento {0} creado", lStrLastDoc));
                }
                (sboObject as SAPbouiCOM.Button).Item.Enabled = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

    }
}
