using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.Core.SDK.DI.Finances.DAO;
using UGRS.Core.SDK.DI.Finances.DTO;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI;
using UGRS.AddOn.Finances.Utils;
using UGRS.Core.SDK.UI;
using UGRS.Core.Utility;

namespace UGRS.AddOn.Finances.Forms
{
    [FormAttribute("UGRS.AddOn.Finances.Forms.Form1", "Forms/CheckGeneration.b1f")]
    class CheckGeneration : UserFormBase
    {
        private SAPbouiCOM.StaticText mTxtFolio;
        private SAPbouiCOM.EditText mEdtFolio;
        private SAPbouiCOM.StaticText mTxtBank;
        private SAPbouiCOM.StaticText mTxtAccnt;
        private SAPbouiCOM.ComboBox mCmbBank;
        private SAPbouiCOM.ComboBox mCmbAcct;
        private SAPbouiCOM.Matrix mMtxSellers;
        private SAPbouiCOM.DataTable mDtSellers;
        private SAPbouiCOM.Button mBtnGenerate;
        private SAPbouiCOM.Button lObjBtnCC;

        private QueryManager mQueryManager = new QueryManager();
        private AuctionDAO mAuctionDAO = new AuctionDAO();
        private BankDAO mBankDAO = new BankDAO();

        private bool mBolGenerated = false;
        private int mIntUserSign = DIApplication.Company.UserSignature;

        IList<AuctionSellerDTO> mLstSellers;
        AuctionDTO mObjLastAuction;

        public CheckGeneration()
        {
            LoadEvents();
        }

        private void LoadComboAuctions()
        {
            try
            {

                List<string> lLstAuctions = mAuctionDAO.GetLastAuctions();

                foreach (var lStrFolio in lLstAuctions)
                {

                    CboAuctions.ValidValues.Add(lStrFolio, "");
                    CboAuctions.Item.DisplayDesc = false;
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[CheckGeneration - LoadComboAuctions] Error al cargar las subastas: {0}", ex.Message));
                UIApplication.ShowMessageBox(string.Format("Error al cargar las subastas: {0}", ex.Message));
                //UIApplication.ShowError("frmMailSender (LoadCboAuctions) " + ex.Message);
                //LogService.WriteError("frmMailSender (LoadCboAuctions) " + ex.Message);
                //LogService.WriteError(ex);
            }
        }

        private void LoadEvents()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void UnLoadEvents()
        {
            Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (!FormUID.Equals(this.UIAPIRawForm.UniqueID))
                {
                    return;
                }
                if (!pVal.BeforeAction)
                {
                    switch (pVal.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_COMBO_SELECT:
                            // Selected Bank on ComboBox
                            if (pVal.ItemUID.Equals(mCmbBank.Item.UniqueID))
                            {
                                LoadAccounts();
                            }
                            // Selected Bank Account on ComboBox
                            if (pVal.ItemUID.Equals(mCmbAcct.Item.UniqueID))
                            {
                                ValidateFields();
                            }
                            break;
                        case SAPbouiCOM.BoEventTypes.et_CLICK:
                            // Clicked Generate Button
                            if (pVal.ItemUID.Equals(mBtnGenerate.Item.UniqueID) && mBtnGenerate.Item.Enabled)
                            {
                                GeneratePayments();
                            }
                            if (pVal.ItemUID.Equals(lObjBtnCC.Item.UniqueID) && lObjBtnCC.Item.Enabled)
                            {
                                ReOpenCC();
                            }
                            break;
                        case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                            UnLoadEvents();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[CheckGeneration - SBO_Application_ItemEvent] Error: {0}", ex.Message));
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(string.Format("ItemEventException: {0}", ex.Message));
            }
        }


        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mTxtFolio = ((SAPbouiCOM.StaticText)(this.GetItem("txtFolio").Specific));
            this.mEdtFolio = ((SAPbouiCOM.EditText)(this.GetItem("edtFolio").Specific));
            this.mTxtBank = ((SAPbouiCOM.StaticText)(this.GetItem("txtBank").Specific));
            this.mTxtAccnt = ((SAPbouiCOM.StaticText)(this.GetItem("txtAccnt").Specific));
            this.mMtxSellers = ((SAPbouiCOM.Matrix)(this.GetItem("mtxSellers").Specific));
            this.mCmbBank = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbBank").Specific));
            this.mCmbAcct = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbAcct").Specific));
            this.mBtnGenerate = ((SAPbouiCOM.Button)(this.GetItem("btnGen").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            this.lObjBtnCC = ((SAPbouiCOM.Button)(this.GetItem("btnOpCC").Specific));
            this.CboAuctions = ((SAPbouiCOM.ComboBox)(this.GetItem("cboAuct").Specific));
            this.CboAuctions.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cboAuctions_ComboSelectAfter);
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
            try
            {
                mCmbBank.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
                mCmbAcct.ExpandType = SAPbouiCOM.BoExpandType.et_ValueOnly;

                IList<BankDTO> lLstObjBanks = mBankDAO.GetBanks();
                foreach (BankDTO lObjBank in lLstObjBanks)
                {
                    mCmbBank.ValidValues.Add(lObjBank.BankCode, lObjBank.BankName);
                }

                mCmbAcct.ValidValues.Add("", "");

                LoadComboAuctions();

                //mObjLastAuction = mAuctionDAO.GetLastAuction();


                this.UIAPIRawForm.DataSources.DataTables.Add("SellersDataTable");
                mDtSellers = this.UIAPIRawForm.DataSources.DataTables.Item("SellersDataTable");
                mDtSellers.Columns.Add("Name", SAPbouiCOM.BoFieldsType.ft_Text);
                mDtSellers.Columns.Add("Amount", SAPbouiCOM.BoFieldsType.ft_Price);

                mMtxSellers.Columns.Item("Col_Seller").DataBind.Bind("SellersDataTable", "Name");
                mMtxSellers.Columns.Item("Col_Amount").DataBind.Bind("SellersDataTable", "Amount");
                mMtxSellers.AutoResizeColumns();

                //mObjLastAuction = mAuctionDAO.GetLastAuction();
                //if (mObjLastAuction != null)
                //{
                //    LoadSellers(mObjLastAuction.Folio);
                //    mEdtFolio.Value = mObjLastAuction.Folio;
                //}
                //else
                //{
                //    UIApplication.ShowError("No hay subastas activas.");
                //}
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[CheckGeneration - OnCustomInitialize] Error: {0}", ex.Message));
                UIApplication.ShowMessageBox(ex.Message);
            }
        }


        /// <summary>
        /// Loads the sellers list into the matrix.
        /// </summary>
        /// <param name="pFolio">The auction's folio.</param>
        public void LoadSellers(string pFolio)
        {
            mLstSellers = mAuctionDAO.GetAuctionSellers(pFolio, mIntUserSign);
            mDtSellers.Rows.Clear();
            int i = 0;
            foreach (AuctionSellerDTO lObjAuctionSeller in mLstSellers)
            {
                mDtSellers.Rows.Add();
                mDtSellers.SetValue("Name", i, lObjAuctionSeller.CardName);
                mDtSellers.SetValue("Amount", i, lObjAuctionSeller.Amount);
                i++;
            }

            mMtxSellers.LoadFromDataSource();
            mMtxSellers.AutoResizeColumns();
        }


        /// <summary>
        /// Loads the available bank accounts in the comoboxes.
        /// </summary>

        private void cboAuctions_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                if (!string.IsNullOrEmpty(CboAuctions.Value))
                {

                    mObjLastAuction = mAuctionDAO.GetAuctionByFolio(CboAuctions.Value);

                    if (mObjLastAuction != null)
                    {
                        LoadSellers(mObjLastAuction.Folio);
                        mEdtFolio.Value = mObjLastAuction.Folio;
                        lObjBtnCC.Item.Enabled = true;
                    }
                    else
                    {
                        UIApplication.ShowError("No hay subastas activas.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[CheckGeneration - cboAuctions_ComboSelectAfter] Error: {0}", ex.Message));
                UIApplication.ShowMessageBox(ex.Message);
                //UIApplication.ShowError("frm (LoadMatrix)" + ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }


        public void LoadAccounts()
        {
            mCmbAcct.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            //Remove old valid values
            int lIntValuesLength = mCmbAcct.ValidValues.Count - 1;
            for (int i = 0; i < lIntValuesLength; i++)
            {
                mCmbAcct.ValidValues.Remove(1, SAPbouiCOM.BoSearchKey.psk_Index);
            }

            mCmbAcct.Item.Enabled = true;

            IList<AccountDTO> ListAccounts = mBankDAO.GetBankAccounts(mCmbBank.Value.ToString());
            foreach (AccountDTO lObjAccount in ListAccounts)
            {
                mCmbAcct.ValidValues.Add(lObjAccount.Account, lObjAccount.Account);
            }
        }

        /// <summary>
        /// Generates a payment for every auction seller.
        /// </summary>
        public void GeneratePayments()
        {
            mBtnGenerate.Item.Enabled = false;

            string lStrAccount = string.Empty;
            string lStrCostingCode = mObjLastAuction.Location;

            if (lStrCostingCode == "SU_HERMO")
            {
                lStrAccount = mQueryManager.GetValue("U_Value", "Name", "SU_VENDEDOR", Constants.STR_CONFIG_TABLE);
            }
            else
            {
                lStrAccount = mQueryManager.GetValue("U_Value", "Name", "SU_VENDEDORSS", Constants.STR_CONFIG_TABLE);
            }

            string lStrUsername = DIApplication.Company.UserName;
            //string lStrCostingCenter = mQueryManager.GetValue("U_GLO_CostCenter", "USER_CODE", lStrUsername, "OUSR");
            DIApplication.Company.StartTransaction();
            bool lBolSuccess = true;
            int i = 0;
            try
            {
                foreach (AuctionSellerDTO lObjSeller in mLstSellers)
                {
                    i++;
                    SAPbobsCOM.Payments lObjPayment = (SAPbobsCOM.Payments)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
                    lObjPayment.DocType = SAPbobsCOM.BoRcptTypes.rAccount;
                    lObjPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_OutgoingPayments;

                    //lObjPayment.CardCode = lObjSeller.CardCode;

                    lObjPayment.CardName = lObjSeller.CardName;

                    lObjPayment.AccountPayments.SetCurrentLine(0);
                    lObjPayment.AccountPayments.AccountCode = lStrAccount;
                    lObjPayment.AccountPayments.SumPaid = lObjSeller.Amount;
                    lObjPayment.BankCode = mCmbBank.Value.ToString(); /// Linea necesaria para que genere el pago con cualquier cuenta de banco
                    lObjPayment.AccountPayments.Add();

                    //lObjPayment.CheckAccount = lStrAccount;

                    lObjPayment.Checks.SetCurrentLine(0);
                    lObjPayment.Checks.CheckSum = lObjSeller.Amount;
                    lObjPayment.Checks.BankCode = mCmbBank.Value.ToString();
                    lObjPayment.Checks.AccounttNum = mCmbAcct.Value.ToString();
                    lObjPayment.Checks.Trnsfrable = SAPbobsCOM.BoYesNoEnum.tYES;
                    lObjPayment.Checks.Add();

                    lObjPayment.UserFields.Fields.Item("U_GLO_PaymentType").Value = "GLPGO";
                    lObjPayment.UserFields.Fields.Item("U_FZ_AuxiliarType").Value = "1";
                    lObjPayment.UserFields.Fields.Item("U_FZ_Auxiliar").Value = lObjSeller.CardCode;
                    lObjPayment.UserFields.Fields.Item("U_FZ_FolioAuction").Value = mObjLastAuction.Folio;

                    lObjPayment.UserFields.Fields.Item("U_GLO_CostCenter").Value = mObjLastAuction.Location;

                    lObjPayment.Remarks = mObjLastAuction.Folio;



                    int intError = lObjPayment.Add();
                    string lStrErrMsg;
                    if (intError != 0)
                    {
                        DIApplication.Company.GetLastError(out intError, out lStrErrMsg);
                        LogUtility.WriteError(String.Format("[CheckGeneration - GeneratePayments] Error generando cheque para {0}: {1}", lObjSeller.CardName, lStrErrMsg));
                        throw new Exception(String.Format("Error generando cheque para {0}: {1}", lObjSeller.CardName, lStrErrMsg));
                        /*lBolSuccess = false;
                        break;*/
                    }
                    else
                    {
                        int lIntDocEntry = int.Parse(DIApplication.Company.GetNewObjectKey());
                        LogUtility.WriteError(String.Format("[CheckGeneration - GeneratePayments] Pago generado correctamente con DocEntry {0} para el cliente {1}", lIntDocEntry, lObjSeller.CardName));
                    }
                }
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[CheckGeneration - GeneratePayments] Error al crear el pago: {0}", e.Message));
                throw new Exception(string.Format("Error al crear el pago: {0}", e.Message));
                //lBolSuccess = false;
            }
            if (lBolSuccess)
            {
                mAuctionDAO.AutorizeAuction(mObjLastAuction.Folio, 'Y');
                UIApplication.ShowMessageBox("Pagos generados exitosamente.");
                mBolGenerated = true;
                DIApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                LogUtility.WriteSuccess("[CheckGeneration - GeneratePayments] Pagos generados exitosamente");
            }
            else
            {
                //DIApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                UIApplication.ShowMessageBox("Ocurrio un error generando cheques.");
            }
            ValidateFields();
        }

        /// <summary>
        /// Checks if the user has permissions to manage auction checks.
        /// </summary>
        private void ValidateFields()
        {
            bool lBolHasRole = mAuctionDAO.HasRole(DIApplication.Company.UserSignature, "SUCH");
            mBtnGenerate.Item.Enabled = mCmbAcct.Value != "" && !mBolGenerated && mObjLastAuction != null && lBolHasRole;
        }

        private void ReOpenCC()
        {
            try
            {
                if (ShowConfirmDialog())
                {
                    //DIApplication.Company.StartTransaction();
                    mAuctionDAO.EnableAuctionForCC(mObjLastAuction.Folio);
                    mAuctionDAO.AutorizeAuction(mObjLastAuction.Folio, 'N');
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                UIApplication.ShowMessageBox("Subasta Abierta para Credito y Cobranza");
                this.UIAPIRawForm.Close();
                //DIApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
            }

        }


        private bool ShowConfirmDialog()
        {
            int result = SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Esta seguro que desea abrir subasta para Credito y Cobranza?", 1, "Ok", "Cancelar", "");
            if (result == 1)
            {
                return true;
            }
            else { return false; }
        }


        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.ComboBox CboAuctions;


    }
}
