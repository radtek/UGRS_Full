using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.Core.SDK.DI.Auctions.DTO;
using UGRS.Core.SDK.DI.Auctions;
using SAPbouiCOM;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.SDK.DI.Auctions.Tables;
using UGRS.Core.SDK.DI;

namespace UGRS.AddOn.Auctions.Forms
{
    [FormAttribute("UGRS.AddOn.Auctions.Forms.frmPayment", "Forms/frmPayment.b1f"), System.Runtime.InteropServices.GuidAttribute("8CBE704F-FAD5-42F6-9629-94F99F2BD842")]
    class frmPayment : UserFormBase
    {
        #region Properties
        PaymentServiceFactory mObjPaymentServiceFactory = new PaymentServiceFactory();
        int mIntRowSelected;
        string mStrAuctionId;
        int mIntSignature = DIApplication.Company.UserSignature;
        List<JournalEntryDTO> mLstJournalEntryDTO;
        #endregion

        public frmPayment()
        {
            mLstJournalEntryDTO = new List<JournalEntryDTO>();
        }

        #region Initialize
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtxSellers = ((SAPbouiCOM.Matrix)(this.GetItem("mtxSellers").Specific));
            this.mtxSellers.DoubleClickAfter += new SAPbouiCOM._IMatrixEvents_DoubleClickAfterEventHandler(this.mtxSellers_DoubleClickAfter);
            this.mtxSellers.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.mtxSellers_ClickBefore);
            this.mtxAuction = ((SAPbouiCOM.Matrix)(this.GetItem("mtxAuction").Specific));
            this.mtxAuction.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.mtxAuction_ValidateBefore);
            this.lblSeller = ((SAPbouiCOM.StaticText)(this.GetItem("lblSeller").Specific));
            this.lblAuction = ((SAPbouiCOM.StaticText)(this.GetItem("lblAuction").Specific));
            this.txtAuction = ((SAPbouiCOM.EditText)(this.GetItem("txtAuction").Specific));
            this.txtAuction.KeyDownAfter += new SAPbouiCOM._IEditTextEvents_KeyDownAfterEventHandler(this.txtAuction_KeyDownAfter);
            this.lblBuyers = ((SAPbouiCOM.StaticText)(this.GetItem("lblBuyers").Specific));
            this.btnCreate = ((SAPbouiCOM.Button)(this.GetItem("btnCreate").Specific));
            this.btnCreate.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.btnCreate_ClickAfter);
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCancel").Specific));
            this.btnFinish = ((SAPbouiCOM.Button)(this.GetItem("btnFinish").Specific));
            this.btnFinish.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.btnFinish_ClickAfter);
            this.btnSeacrh = ((SAPbouiCOM.Button)(this.GetItem("btnSearch").Specific));
            this.btnSeacrh.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.btnSeacrh_ClickAfter);
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
            mtxAuction.AutoResizeColumns();
            mtxSellers.AutoResizeColumns();
            if (DtMatrixSellers == null)
            {
                CreateDataTableMatrixSellers();
            }
            //txtAuction.Value = "S-HMO-180012";
            string lStrLastAuction = mObjPaymentServiceFactory.GetPaymentService().GetLastAuction();
            if (!string.IsNullOrEmpty(lStrLastAuction))
            {
                txtAuction.Value = lStrLastAuction;
                mStrAuctionId = lStrLastAuction;
            }

        }
        #endregion

        #region Events
        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                mtxSellers.Item.Height = UIAPIRawForm.Height / 2 - 75;
                mtxAuction.Item.Top = UIAPIRawForm.Height / 2;
                mtxAuction.Item.Height = UIAPIRawForm.Height / 2 - 85;
                lblBuyers.Item.Top = UIAPIRawForm.Height / 2 - 20;
                btnCreate.Item.Top = UIAPIRawForm.Height - 75;
                btnCancel.Item.Top = UIAPIRawForm.Height - 75;
                btnFinish.Item.Top = UIAPIRawForm.Height - 75;
                mtxAuction.AutoResizeColumns();
                mtxSellers.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (Form_ResizeAfter)" + ex.Message);
                LogService.WriteError("frmPayment (Form_ResizeAfter)" + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        private void txtAuction_KeyDownAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
           

        }

        private void mtxSellers_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.Row > 0)
                {
                    mtxSellers.SelectRow(pVal.Row, true, false);

                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (mtxSellers_ClickBefore) " + ex.Message);
                LogService.WriteError("frmPayment (mtxSellers_ClickBefore) " + ex.Message);
                LogService.WriteError(ex);
            }

        }

        private void mtxSellers_DoubleClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                mIntRowSelected = pVal.Row;
                if (DtMatrixAuctions == null)
                {
                    CreateDataTableMatrixAuctions();
                }
                List<AuctionsDTO> lLstAuctionDTO = mObjPaymentServiceFactory.GetPaymentService().GetAuctions(DtMatrixSellers.GetValue("C_Code", pVal.Row - 1).ToString(),mIntSignature).ToList();
                lLstAuctionDTO = AddChargeToList(lLstAuctionDTO);
                FillMatrixAuction(lLstAuctionDTO);
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (mtxSellers_DoubleClickAfter) " + ex.Message);
                LogService.WriteError("frmPayment (mtxSellers_DoubleClickAfter) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        private void mtxAuction_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.ColUID == "C_TotalC")
                {
                    string lStrPayment = (mtxAuction.Columns.Item("C_TotalC").Cells.Item(pVal.Row).Specific as EditText).Value.Trim();
                    
                    decimal lDblPayment = Convert.ToDecimal(lStrPayment == "" ? "0" : lStrPayment);
                    decimal lDblCompra = Convert.ToDecimal(DtMatrixAuctions.GetValue("C_TotalB", pVal.Row-1));

                    if (lDblPayment <= lDblCompra && lDblPayment >= 0)
                    {
                        UpdateMatrix();
                        JounralEntry(pVal.Row);
                    }
                    else
                    {
                        BubbleEvent = false;
                        UIApplication.ShowError("Cantidad incorrecta favor de revisar");
                    }

                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (mtxAuction_ValidateBefore) " + ex.Message);
                LogService.WriteError("frmPayment (mtxAuction_ValidateBefore) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        private void btnCreate_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                IList<string> lLstStrCreateJounalEntry = mObjPaymentServiceFactory.CreateDocument().CreateDocument(mLstJournalEntryDTO);
                if (lLstStrCreateJounalEntry.Count() == 0)
                {
                    UIApplication.ShowMessageBox("Guardado correcto");
                    UIApplication.ShowSuccess("Creación de asiento realizado correctamente");
                    LogService.WriteSuccess("Creación de asiento realizado correctamente");

                    try
                    {
                        UIAPIRawForm.Freeze(true);
                        DtMatrixAuctions.Rows.Clear();
                        List<PaymentDTO> lLstPaymentDTO = mObjPaymentServiceFactory.GetPaymentService().GetPayments(mStrAuctionId, mIntSignature).ToList();
                        FillMatrix(lLstPaymentDTO);
                        mtxAuction.LoadFromDataSource();
                        mLstJournalEntryDTO = new List<JournalEntryDTO>();

                    }
                    catch (Exception ex)
                    {
                        UIApplication.ShowError("frmPayment (btnCreate_ClickAfter) " + ex.Message);
                        LogService.WriteError("frmPayment (btnCreate_ClickAfter) " + ex.Message);
                        LogService.WriteError(ex);

                    }
                    finally
                    {
                        UIAPIRawForm.Freeze(false);
                    }
                }
                else
                {
                    string lStrMessage = string.Format(" \n{0}",
                      string.Join("\n", lLstStrCreateJounalEntry.Select(x => string.Format("{0}", x)).ToArray()));
                    UIApplication.ShowMessageBox("Error al crear el asiento: " + lStrMessage);
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (btnCreate_ClickAfter) " + ex.Message);
                LogService.WriteError("frmPayment (btnCreate_ClickAfter) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        private void btnSeacrh_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (mObjPaymentServiceFactory.GetPaymentService().ExistConfiguration("SU_DEUDORA") && mObjPaymentServiceFactory.GetPaymentService().ExistConfiguration("SU_ACREEDORA"))
                {
                    List<PaymentDTO> lLstPaymentDTO = mObjPaymentServiceFactory.GetPaymentService().GetPayments(txtAuction.Value,mIntSignature).ToList();
                    if (lLstPaymentDTO.Count() > 0)
                    {
                        mStrAuctionId = txtAuction.Value;
                        LogService.WriteInfo("Carga de subastas correctamente");
                    }
                    else
                    {
                        UIApplication.ShowWarning("No se encontraron registros para esa subasta");
                        LogService.WriteInfo("No se encontraron registros para esa subasta");
                    }
                    FillMatrix(lLstPaymentDTO);
                }
                else
                {
                    UIApplication.ShowMessageBox("Los registros SU_DEUDORA o SU_ACREEDORA no existen en la tabla de configuración");
                    LogService.WriteInfo("Los registros SU_DEUDORA o SU_ACREEDORA no existen en la tabla de configuración");
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (txtAuction_KeyDownAfter)" + ex.Message);
                LogService.WriteError("frmPayment (txtAuction_KeyDownAfter)" + ex.Message);
                LogService.WriteError(ex);
            }

        }

        private void btnFinish_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {

            string lStrMessage = mLstJournalEntryDTO.Count() > 0 ? " Aun tiene registros por guardar " : "";
            Auction lObjAuction = mObjPaymentServiceFactory.GetPaymentService().GetAuction(mStrAuctionId);
            if (!lObjAuction.AutAuction)
            {
                if (UIApplication.ShowOptionBox(lStrMessage + "¿Desea terminar cobro? ") == 1)
                {

                    lObjAuction.AutAuction = true;
                    int lIntResult = mObjPaymentServiceFactory.GetAuctionService().Update(lObjAuction);

                    if (lIntResult == 0)//&& lObjAuction.AutCorral && lObjAuction.AutTransp)
                    {
                        List<MessageDTO> lLstMessagesDTO = mObjPaymentServiceFactory.GetPaymentService().GetMessages(mStrAuctionId);
                        bool lBolAlert = false;
                        foreach (MessageDTO lObjMessage in lLstMessagesDTO)
                        {
                            if (mObjPaymentServiceFactory.GetAlertService().SaveAlert(lObjMessage))
                            {
                                lBolAlert = true;
                            }
                        }
                        if (lBolAlert)
                        {
                            UIApplication.ShowMessageBox("Proceso terminado \n Se envió una alerta al departamento de crédito y cobranza");
                        }
                    }
                }
            }
            else
            {
                UIApplication.ShowMessageBox("Proceso ya se ha terminado anteriormente");
            }

        }
        #endregion

        #region Methods

        private void CreateDataTableMatrixSellers()
        {
            try
            {
                this.UIAPIRawForm.DataSources.DataTables.Add("DsPayments");
                DtMatrixSellers = this.UIAPIRawForm.DataSources.DataTables.Item("DsPayments");
                DtMatrixSellers.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_ShortNumber);
                DtMatrixSellers.Columns.Add("C_Code", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                DtMatrixSellers.Columns.Add("C_Name", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                DtMatrixSellers.Columns.Add("C_TotalS", SAPbouiCOM.BoFieldsType.ft_Price); //Venta
                DtMatrixSellers.Columns.Add("C_TotalB", SAPbouiCOM.BoFieldsType.ft_Price); //Compra
                DtMatrixSellers.Columns.Add("C_TotalC", SAPbouiCOM.BoFieldsType.ft_Price); //Cobrar
                DtMatrixSellers.Columns.Add("C_Total", SAPbouiCOM.BoFieldsType.ft_Price);
                DtMatrixSellers.Columns.Add("C_AccountD", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                DtMatrixSellers.Columns.Add("C_AccountC", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (CreateDataTableMatrixSellers) " + ex.Message);
                LogService.WriteError("frmPayment (CreateDataTableMatrixSellers) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        private void CreateDataTableMatrixAuctions()
        {
            try
            {
                this.UIAPIRawForm.DataSources.DataTables.Add("DsAuctions");
                DtMatrixAuctions = this.UIAPIRawForm.DataSources.DataTables.Item("DsAuctions");
                DtMatrixAuctions.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_ShortNumber);
                DtMatrixAuctions.Columns.Add("C_Auction", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                DtMatrixAuctions.Columns.Add("C_TotalB", SAPbouiCOM.BoFieldsType.ft_Price);
                DtMatrixAuctions.Columns.Add("C_TotalC", SAPbouiCOM.BoFieldsType.ft_Price);
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (CreateDataTableMatrixAuctions) " + ex.Message);
                LogService.WriteError("frmPayment (CreateDataTableMatrixAuctions) " + ex.Message);
                LogService.WriteError(ex);
               
            }
        }

        private void FillMatrix(List<PaymentDTO> pLstPaymentDTO)
        {
            try
            {
                DtMatrixSellers.Rows.Clear();
                int i = 0;
                this.UIAPIRawForm.Freeze(true);
                foreach (PaymentDTO lObjPaymentDTO in pLstPaymentDTO)
                {
                    DtMatrixSellers.Rows.Add();
                    DtMatrixSellers.SetValue("#", i, i + 1);
                    DtMatrixSellers.SetValue("C_Code", i, lObjPaymentDTO.CardCode);
                    DtMatrixSellers.SetValue("C_Name", i, lObjPaymentDTO.CardName);
                    DtMatrixSellers.SetValue("C_TotalS", i, lObjPaymentDTO.TotalSell);
                    DtMatrixSellers.SetValue("C_TotalB", i, lObjPaymentDTO.TotalBuy);
                    DtMatrixSellers.SetValue("C_TotalC", i, lObjPaymentDTO.TotalCharge == null ? "0" : lObjPaymentDTO.TotalCharge);
                    DtMatrixSellers.SetValue("C_Total", i, lObjPaymentDTO.Total == null ? "0" : lObjPaymentDTO.Total);
                    DtMatrixSellers.SetValue("C_AccountD", i, lObjPaymentDTO.AccountD);
                    DtMatrixSellers.SetValue("C_AccountC", i, lObjPaymentDTO.AccountC);
                    i++;
                }

                mtxSellers.Columns.Item("#").DataBind.Bind("DsPayments", "#");
                mtxSellers.Columns.Item("C_Code").DataBind.Bind("DsPayments", "C_Code");
                mtxSellers.Columns.Item("C_Name").DataBind.Bind("DsPayments", "C_Name");
                mtxSellers.Columns.Item("C_TotalS").DataBind.Bind("DsPayments", "C_TotalS");
                mtxSellers.Columns.Item("C_TotalB").DataBind.Bind("DsPayments", "C_TotalB");
                mtxSellers.Columns.Item("C_TotalC").DataBind.Bind("DsPayments", "C_TotalC");
                mtxSellers.Columns.Item("C_Total").DataBind.Bind("DsPayments", "C_Total");
                mtxSellers.LoadFromDataSource();
                mtxSellers.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (FillMatrix) " + ex.Message);
                LogService.WriteError("frmPayment (FillMatrix) " + ex.Message);
                LogService.WriteError(ex);
                
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void FillMatrixAuction(List<AuctionsDTO> pLstAuctionDTO)
        {
            try
            {
                DtMatrixAuctions.Rows.Clear();
                int i = 0;
                this.UIAPIRawForm.Freeze(true);
                foreach (AuctionsDTO lObjAuction in pLstAuctionDTO)
                {
                    DtMatrixAuctions.Rows.Add();
                    DtMatrixAuctions.SetValue("#", i, i + 1);
                    DtMatrixAuctions.SetValue("C_Auction", i, lObjAuction.Folio);
                    DtMatrixAuctions.SetValue("C_TotalB", i, lObjAuction.TotalBuyer);
                    DtMatrixAuctions.SetValue("C_TotalC", i, lObjAuction.TotalCharge == null ? "0" : lObjAuction.TotalCharge);
                    i++;
                }
                mtxAuction.Columns.Item("#").DataBind.Bind("DsAuctions", "#");
                mtxAuction.Columns.Item("C_Auction").DataBind.Bind("DsAuctions", "C_Auction");
                mtxAuction.Columns.Item("C_TotalB").DataBind.Bind("DsAuctions", "C_TotalB");
                mtxAuction.Columns.Item("C_TotalC").DataBind.Bind("DsAuctions", "C_TotalC");
                mtxAuction.LoadFromDataSource();
                mtxAuction.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (FillMatrixAuction) " + ex.Message);
                LogService.WriteError("frmPayment (FillMatrixAuction) " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private List<AuctionsDTO> AddChargeToList(List<AuctionsDTO> pLstAuctionDTO)
        {
            try
            {
                string lStrAccount = DtMatrixSellers.GetValue("C_AccountC", mIntRowSelected - 1).ToString();
                string lStrAux = DtMatrixSellers.GetValue("C_Code", mIntRowSelected - 1).ToString();
                foreach (AuctionsDTO lObjAuctionsDTO in pLstAuctionDTO)
                {
                    JournalEntryDTO lObjJournalEntry = mLstJournalEntryDTO.Where(x => x.Credit > 0 && x.Account == lStrAccount && x.Aux == lStrAux && x.AuctionId == lObjAuctionsDTO.Folio).FirstOrDefault();

                    if (lObjJournalEntry != null)
                    {
                        lObjAuctionsDTO.TotalCharge = lObjJournalEntry.Credit.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (AddChargeToList) " + ex.Message);
                LogService.WriteError("frmPayment (AddChargeToList) " + ex.Message);
                LogService.WriteError(ex);
            }
            return pLstAuctionDTO;
        }

        private void UpdateMatrix()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);
                decimal lDecPaymentPayment = 0;
                for (int i = 0; i < DtMatrixAuctions.Rows.Count; i++)
                {
                    string lStrPayment = (mtxAuction.Columns.Item("C_TotalC").Cells.Item(i + 1).Specific as EditText).Value.Trim();
                    decimal lDblPayment = Convert.ToDecimal(lStrPayment == "" ? "0" : lStrPayment);
                    DtMatrixAuctions.SetValue("C_TotalC", i, lDblPayment.ToString());
                    lDecPaymentPayment += lDblPayment;
                }
                decimal lDecPaymentSell = Convert.ToDecimal(DtMatrixSellers.GetValue("C_TotalS", mIntRowSelected - 1).ToString());
                DtMatrixSellers.SetValue("C_TotalC", mIntRowSelected - 1, lDecPaymentPayment.ToString());

                DtMatrixSellers.SetValue("C_Total", mIntRowSelected - 1, (lDecPaymentSell - lDecPaymentPayment).ToString());
                mtxSellers.LoadFromDataSource();
                mtxAuction.LoadFromDataSource();
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (UpdateMatrix) " + ex.Message);
                LogService.WriteError("frmPayment (UpdateMatrix) " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void JounralEntry(int pIntRow)
        {
            try
            {
                string lStrAuction = DtMatrixAuctions.GetValue("C_Auction", pIntRow - 1).ToString();
                string lStrAux = DtMatrixSellers.GetValue("C_Code", mIntRowSelected - 1).ToString();
                decimal lDecTotalC = Convert.ToDecimal(DtMatrixAuctions.GetValue("C_TotalC", pIntRow - 1).ToString());
                if (lDecTotalC > 0)
                {
                    JournalEntryDTO lObjJournalEntry = mLstJournalEntryDTO.Where(x => x.Aux == lStrAux && x.AuctionId == lStrAuction).FirstOrDefault();
                    if (lObjJournalEntry == null)
                    {
                        JournalEntryDTO lObjJournalEntryDTO = new JournalEntryDTO();
                        lObjJournalEntryDTO.AuctionId = DtMatrixAuctions.GetValue("C_Auction", pIntRow - 1).ToString();
                        lObjJournalEntryDTO.Credit = Convert.ToDecimal(DtMatrixAuctions.GetValue("C_TotalC", pIntRow - 1).ToString());
                        lObjJournalEntryDTO.Debit = 0;
                        lObjJournalEntryDTO.Area = "SU_HERMO";
                        lObjJournalEntryDTO.AuxType = "1";
                        lObjJournalEntryDTO.Aux = DtMatrixSellers.GetValue("C_Code", mIntRowSelected - 1).ToString();
                        lObjJournalEntryDTO.AuctionId = DtMatrixAuctions.GetValue("C_Auction", pIntRow - 1).ToString();
                        lObjJournalEntryDTO.Account = DtMatrixSellers.GetValue("C_AccountC", mIntRowSelected - 1).ToString();
                        lObjJournalEntry = lObjJournalEntryDTO;
                        mLstJournalEntryDTO.Add(lObjJournalEntryDTO);
                    }
                    else
                    {
                        lObjJournalEntry.Credit = Convert.ToDecimal(DtMatrixAuctions.GetValue("C_TotalC", pIntRow - 1).ToString());
                    }
                    UpdateJorunalEntryDebit(lObjJournalEntry);
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (JounralEntry) " + ex.Message);
                LogService.WriteError("frmPayment (JounralEntry) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        private void UpdateJorunalEntryDebit(JournalEntryDTO pObjJournalEntry)
        {
            try
            {
                string lStrAccountC = DtMatrixSellers.GetValue("C_AccountC", mIntRowSelected - 1).ToString();
                string lStrAccountD = DtMatrixSellers.GetValue("C_AccountD", mIntRowSelected - 1).ToString();
                JournalEntryDTO lObjJournalEntry = mLstJournalEntryDTO.Where(x => x.Aux == pObjJournalEntry.Aux && x.Account == lStrAccountD).FirstOrDefault();
                if (lObjJournalEntry == null)
                {
                    lObjJournalEntry = new JournalEntryDTO();
                    lObjJournalEntry.AuctionId = mStrAuctionId;
                    lObjJournalEntry.Area = pObjJournalEntry.Area;
                    lObjJournalEntry.AuxType = pObjJournalEntry.AuxType;
                    lObjJournalEntry.Aux = pObjJournalEntry.Aux;
                    lObjJournalEntry.Debit = pObjJournalEntry.Credit;
                    lObjJournalEntry.Credit = 0;
                    lObjJournalEntry.AuctionId = mStrAuctionId;
                    lObjJournalEntry.Account = lStrAccountD;
                    mLstJournalEntryDTO.Add(lObjJournalEntry);
                }
                else
                {
                    decimal lDecDebit = mLstJournalEntryDTO.Where(x => x.Aux == pObjJournalEntry.Aux && x.Account == lStrAccountC).Sum(y => y.Credit);
                    lObjJournalEntry.Debit = lDecDebit;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (UpdateJorunalEntryDebit) " + ex.Message);
                LogService.WriteError("frmPayment (UpdateJorunalEntryDebit) " + ex.Message);
                LogService.WriteError(ex);
            }
        }
        #endregion

        #region Controls
        private SAPbouiCOM.Matrix mtxSellers;
        private SAPbouiCOM.Matrix mtxAuction;
        private SAPbouiCOM.StaticText lblSeller;
        private SAPbouiCOM.StaticText lblAuction;
        private SAPbouiCOM.EditText txtAuction;
        private SAPbouiCOM.StaticText lblBuyers;
        private SAPbouiCOM.Button btnCreate;
        private SAPbouiCOM.Button btnCancel;
        private SAPbouiCOM.Button btnFinish;
        private SAPbouiCOM.DataTable DtMatrixSellers;
        private SAPbouiCOM.DataTable DtMatrixAuctions;
        private Button btnSeacrh;
        #endregion
      
      

    

    }
}
