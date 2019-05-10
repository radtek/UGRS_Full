using System;
using SAPbouiCOM.Framework;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.SDK.DI.CyC;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.CyC.DTO;
using SAPbouiCOM;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.CyC.Tables;

namespace UGRS.AddOn.CyC
{
    [FormAttribute("UGRS.AddOn.CyC.frmCollection", "Forms/frmCollection.b1f")]
    class frmCollection : UserFormBase
    {
        #region Properties
        CyCServiceFactory mObjCyCServiceFactory = new CyCServiceFactory();
        int mIntSelectedRowAuction;
        int mIntSelectedRowComent;
        double mDblPayment = 0;
        string mStrSelectCardCode = string.Empty;
        Auction mObjAuction = new Auction();
        UserDTO mObjUserDTO = new UserDTO();
        SAPbouiCOM.Form lObjFormDraft = null;
        #endregion

        #region Constructor
        public frmCollection()
        {
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lblFolio = ((SAPbouiCOM.StaticText)(this.GetItem("lblFolio").Specific));
            this.cboFolio = ((SAPbouiCOM.ComboBox)(this.GetItem("CboFolio").Specific));
            this.cboFolio.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cboFolio_ComboSelectAfter);
            this.mtxAuction = ((SAPbouiCOM.Matrix)(this.GetItem("MtxAuction").Specific));
            //    this.mtxAuction.DoubleClickAfter += new SAPbouiCOM._IMatrixEvents_DoubleClickAfterEventHandler(this.mtxAuction_DoubleClickAfter);
            this.mtxAuction.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.mtxAuction_ClickBefore);
            this.mtxAuction.ClickAfter += new SAPbouiCOM._IMatrixEvents_ClickAfterEventHandler(this.mtxAuction_ClickAfter);
            this.lblComent = ((SAPbouiCOM.StaticText)(this.GetItem("lblComent").Specific));
            this.txtComent = ((SAPbouiCOM.EditText)(this.GetItem("txtComent").Specific));
            this.btnAdd = ((SAPbouiCOM.Button)(this.GetItem("btnAdd").Specific));
            this.btnAdd.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAdd_ClickBefore);
            this.btnFinish = ((SAPbouiCOM.Button)(this.GetItem("btnFinish").Specific));
            this.btnFinish.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnFinish_ClickBefore);
            this.mtxComent = ((SAPbouiCOM.Matrix)(this.GetItem("MtxComent").Specific));
            this.mtxComent.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.mtxComent_ClickBefore);
            this.mtxInv = ((SAPbouiCOM.Matrix)(this.GetItem("mtxInv").Specific));
            this.mtxInv.DoubleClickBefore += new SAPbouiCOM._IMatrixEvents_DoubleClickBeforeEventHandler(this.mtxInv_DoubleClickBefore);
            this.mtxInv.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.mtxInv_ClickBefore);
            this.mtxInv.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.mtxInv_ValidateBefore);
            this.mtxInv.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.mtxInv_ValidateAfter);
            this.mtxInv.LinkPressedAfter += new SAPbouiCOM._IMatrixEvents_LinkPressedAfterEventHandler(this.mtxInv_LinkPressedAfter);
            this.mtxCollect = ((SAPbouiCOM.Matrix)(this.GetItem("mtxCollect").Specific));
            this.mtxCollect.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.mtxCollect_ClickBefore);
            this.mtxCollect.LinkPressedAfter += new SAPbouiCOM._IMatrixEvents_LinkPressedAfterEventHandler(this.mtxCollect_LinkPressedAfter);
            this.lblAmount = ((SAPbouiCOM.StaticText)(this.GetItem("lblAmount").Specific));
            this.lblBalance = ((SAPbouiCOM.StaticText)(this.GetItem("lblBalance").Specific));
            this.txtAmount = ((SAPbouiCOM.EditText)(this.GetItem("txtAmount").Specific));
            this.txtBalance = ((SAPbouiCOM.EditText)(this.GetItem("txtBalance").Specific));
            this.btnCollect = ((SAPbouiCOM.Button)(this.GetItem("btnCollect").Specific));
            this.btnCollect.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.btnCollect_ClickAfter);
            this.btnEnd = ((SAPbouiCOM.Button)(this.GetItem("btnEnd").Specific));
            this.btnEnd.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnEnd_ClickBefore);
            this.btnDelete = ((SAPbouiCOM.Button)(this.GetItem("btnDelete").Specific));
            this.btnDelete.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnDelete_ClickBefore);
            this.lObjBtnOpAct = ((SAPbouiCOM.Button)(this.GetItem("btnOpAct").Specific));
            this.lObjBtnOpAct.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnOpAct_ClickBefore);
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
            string lStrUserID = UIApplication.GetCompany().UserName;
            mObjUserDTO = mObjCyCServiceFactory.GetCyCServices().GetUser(lStrUserID);


            //mObjUserDTO.CostigCode = "OG_CRYCO";
            //mObjUserDTO.UserCode = "auxcyc";
            //mObjUserDTO.CYC = 'Y';

            LoadCboAuctions();
            //if (!mObjCyCServiceFactory.GetCyCServices().GetUserCyC(UIApplication.GetCompany().UserName))
            if (mObjUserDTO.CYC != 'Y')
            {
                mtxAuction.Columns.Item("C_ImpSub").Visible = false;
                mtxAuction.Columns.Item("C_ImpDeud").Visible = false;
                btnCollect.Item.Visible = false;
                btnEnd.Item.Visible = false;
                mtxInv.Columns.Item("C_Amount").Editable = false;
                txtAmount.Item.Visible = false;
                txtBalance.Item.Visible = false;
                mtxCollect.Item.Visible = false;
                lblBalance.Item.Visible = false;
                lblAmount.Item.Visible = false;
                lObjBtnOpAct.Item.Visible = false;
                lObjBtnOpAct.Item.Enabled = false;
            }
            else
            {
                btnAdd.Item.Visible = false;
                btnFinish.Item.Visible = false;
                btnDelete.Item.Visible = false;
                lObjBtnOpAct.Item.Visible = false;
                cboFolio.Item.Click();
                txtComent.Item.Enabled = false;
                lObjBtnOpAct.Item.Visible = true;
                lObjBtnOpAct.Item.Enabled = false;
                btnEnd.Item.Enabled = false;
                btnCollect.Item.Enabled = false;
            }
        }
        #endregion

        #region Events
        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                //Width
                mtxAuction.Item.Width = UIAPIRawForm.Width / 2 - 100;
                mtxComent.Item.Width = UIAPIRawForm.Width / 2;
                mtxCollect.Item.Width = UIAPIRawForm.Width / 2;
                mtxInv.Item.Width = UIAPIRawForm.Width / 2;


                //Height
                mtxAuction.Item.Height = UIAPIRawForm.Height - mtxAuction.Item.Top - 50;
                mtxCollect.Item.Height = UIAPIRawForm.Height / 5;
                mtxComent.Item.Height = UIAPIRawForm.Height / 5;
                mtxInv.Item.Height = UIAPIRawForm.Height / 5;

                //Left
                mtxComent.Item.Left = mtxAuction.Item.Width + 50;
                mtxCollect.Item.Left = mtxAuction.Item.Width + 50;
                mtxInv.Item.Left = mtxAuction.Item.Width + 50;
                lblAmount.Item.Left = mtxInv.Item.Left;
                txtAmount.Item.Left = lblAmount.Item.Left + lblAmount.Item.Width + 10;
                lblBalance.Item.Left = mtxInv.Item.Left;
                txtBalance.Item.Left = lblBalance.Item.Left + lblBalance.Item.Width + 10;
                btnCollect.Item.Left = txtBalance.Item.Left + txtBalance.Item.Width + 10;
                btnEnd.Item.Left = UIAPIRawForm.Width - btnEnd.Item.Width - 60;
                btnDelete.Item.Left = mtxComent.Item.Left + mtxComent.Item.Width - 50;
                lObjBtnOpAct.Item.Left = UIAPIRawForm.Width - lObjBtnOpAct.Item.Width - 190;

                //Top
                btnDelete.Item.Top = mtxComent.Item.Top + mtxComent.Item.Height;
                mtxInv.Item.Top = mtxComent.Item.Top + mtxComent.Item.Height + 20;
                lblAmount.Item.Top = mtxInv.Item.Top + mtxInv.Item.Height + 10;
                txtAmount.Item.Top = lblAmount.Item.Top;

                lblBalance.Item.Top = lblAmount.Item.Top + 20;
                txtBalance.Item.Top = lblAmount.Item.Top + 20;
                btnCollect.Item.Top = lblAmount.Item.Top + 20;

                mtxCollect.Item.Top = btnCollect.Item.Top + btnCollect.Item.Height + 20;
                btnEnd.Item.Top = mtxCollect.Item.Top + mtxCollect.Item.Height + 20;
                lObjBtnOpAct.Item.Top = mtxCollect.Item.Top + mtxCollect.Item.Height + 20;

                //Autoresize
                mtxAuction.AutoResizeColumns();
                mtxCollect.AutoResizeColumns();
                mtxComent.AutoResizeColumns();
                mtxInv.AutoResizeColumns();

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

        private void cboFolio_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);
                LoadMatrixAuction();
                if (mtxAuction.RowCount > 0)
                {
                    LoadMatrixPayment();
                    btnEnd.Item.Enabled = true;
                    lObjBtnOpAct.Item.Enabled = true;
                }
                else
                {
                    ClearMatrixes();

                }

                mObjAuction = mObjCyCServiceFactory.GetCyCServices().GetAuction(cboFolio.Value);
                if (mObjAuction.AutCyC)
                {
                    btnEnd.Caption = "Abrir cobro";
                    btnEnd.Item.Enabled = true;
                    lObjBtnOpAct.Item.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frm (LoadMatrix)" + ex.Message);
                LogService.WriteError("frm (LoadMatrix)" + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void ClearMatrixes()
        {
            SAPbouiCOM.DataTable DtMatrixPayment = this.UIAPIRawForm.DataSources.DataTables.Item("DtCollect");
            DtMatrixPayment.Rows.Clear();
            BindMatrixPayment();
            SAPbouiCOM.DataTable DtMatrixInvoice = this.UIAPIRawForm.DataSources.DataTables.Item("DtInv");
            DtMatrixInvoice.Rows.Clear();
            BindMatrixInvoice();
            SAPbouiCOM.DataTable DtMatrixComent = this.UIAPIRawForm.DataSources.DataTables.Item("DtComent");
            DtMatrixComent.Rows.Clear();
            BindMatrixComents();
        }


        private void mtxAuction_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (mIntSelectedRowAuction > 0)
                {
                    SAPbouiCOM.DataTable DtAuctions = this.UIAPIRawForm.DataSources.DataTables.Item("DtAuction");
                    mStrSelectCardCode = DtAuctions.GetValue("C_Sell", pVal.Row - 1).ToString();
                    txtBalance.Value = Convert.ToDecimal((mtxAuction.Columns.Item("C_ImpSub").Cells.Item(pVal.Row).Specific as EditText).Value.Trim()).ToString();
                    LoadMatrixInvoice(mStrSelectCardCode);
                    LoadMatrixComents();
                    txtAmount.Value = string.Empty;
                    btnCollect.Item.Enabled = true;
                }
            }
            catch (Exception ex)
            {

                UIApplication.ShowError("Clic " + ex.Message);
                LogService.WriteError("Clic " + ex.Message);
                LogService.WriteError(ex);
            }
        }


        //private void mtxAuction_DoubleClickAfter(object sboObject, SBOItemEventArg pVal)
        //{
        //    try
        //    {
        //        if (mIntSelectedRowAuction > 0)
        //        {
        //            SAPbouiCOM.DataTable DtAuctions = this.UIAPIRawForm.DataSources.DataTables.Item("DtAuction");
        //            mStrSelectCardCode = DtAuctions.GetValue("C_Sell", pVal.Row - 1).ToString();
        //            txtBalance.Value = Convert.ToDecimal((mtxAuction.Columns.Item("C_ImpSub").Cells.Item(pVal.Row).Specific as EditText).Value.Trim()).ToString();
        //            LoadMatrixInvoice(mStrSelectCardCode);
        //            LoadMatrixComents();
        //            txtAmount.Value = string.Empty;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        UIApplication.ShowError("Double clic " + ex.Message);
        //        LogService.WriteError("Double clic " + ex.Message);
        //        LogService.WriteError(ex);
        //    }
        //}

        private void mtxInv_LinkPressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "C_DocNum")
            {
                SAPbouiCOM.DataTable DtMatrixInvoice = this.UIAPIRawForm.DataSources.DataTables.Item("DtInv");
                string lStrDocEntry = DtMatrixInvoice.GetValue("C_DocEntry", pVal.Row - 1).ToString();
                SAPbouiCOM.Framework.Application.SBO_Application.OpenForm(BoFormObjectEnum.fo_Invoice, "", lStrDocEntry);
            }
        }

        private void mtxCollect_LinkPressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "C_DocNum")
            {
                SAPbouiCOM.DataTable DtMatrixInvoice = this.UIAPIRawForm.DataSources.DataTables.Item("DtCollect");
                string lStrDocEntry = DtMatrixInvoice.GetValue("C_DocEntry", pVal.Row - 1).ToString();
                SAPbouiCOM.Framework.Application.SBO_Application.OpenForm(BoFormObjectEnum.fo_Receipt, "", lStrDocEntry);
            }
        }

        private void mtxInv_ValidateBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.ColUID == "C_Amount" && pVal.ItemChanged)
                {
                    string lStrValue = (mtxInv.Columns.Item("C_Amount").Cells.Item(pVal.Row).Specific as EditText).Value.Trim();
                    if (!string.IsNullOrEmpty(lStrValue))
                    {
                        decimal lDcmValue = Convert.ToDecimal((mtxInv.Columns.Item("C_Amount").Cells.Item(pVal.Row).Specific as EditText).Value.Trim());
                        decimal lDcmBalance = Convert.ToDecimal((mtxInv.Columns.Item("C_Balance").Cells.Item(pVal.Row).Specific as EditText).Value.Trim());
                        if (lDcmValue <= lDcmBalance && lDcmValue >= 0)
                        {
                            SAPbouiCOM.DataTable DtMatrixInvoice = this.UIAPIRawForm.DataSources.DataTables.Item("DtInv");
                            DtMatrixInvoice.SetValue(pVal.ColUID, pVal.Row - 1, lDcmValue.ToString());
                            SumTotalPayment();
                        }
                        else
                        {

                            BubbleEvent = false;
                            UIApplication.ShowError("Favor de ingresar una cantidad menor al de la factura");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("mtxInv_ValidateBefore  " + ex.Message);
                LogService.WriteError("mtxInv_ValidateBefore " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        private void mtxInv_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {


        }

        private void btnCollect_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (btnCollect.Item.Enabled)
                {
                    if (btnCollect.Caption != "Abrir cobro")
                    {
                        CreatePayment();
                    }
                    else
                    {
                        UIApplication.ShowMessageBox("No es posible crear el pago debido a que ya ha terminado el proceso");
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("CreatePayment  " + ex.Message);
                LogService.WriteError("CreatePayment " + ex.Message);
                LogService.WriteError(ex);
            }

        }

        private void btnAdd_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            mObjAuction = mObjCyCServiceFactory.GetCyCServices().GetAuction(cboFolio.Value);
            try
            {
                if (!string.IsNullOrEmpty(mStrSelectCardCode))
                {
                    if (!string.IsNullOrEmpty(txtComent.Value.Trim()))
                    {
                        switch (mObjUserDTO.CostigCode)
                        {
                            case "CR_CORRA":
                                if (!mObjAuction.AutCorral)
                                {
                                    AddComent();
                                }
                                else
                                {
                                    UIApplication.ShowMessageBox("No es posible agregar un comentario, el proceso de corrales ya ha finalizado");
                                }
                                break;

                            case "TR_TRANS":
                                if (!mObjAuction.AutTransp)
                                {
                                    AddComent();
                                }
                                else
                                {
                                    UIApplication.ShowMessageBox("No es posible agregar un comentario, el proceso de transporte ya ha finalizado");
                                }
                                break;
                            case "OG_CRYCO":
                                AddComent();
                                break;
                        }
                    }
                    else
                    {
                        UIApplication.ShowMessageBox("Favor de agregar un comentario");
                    }
                }
                else
                {
                    UIApplication.ShowMessageBox("Debe seleccionar un vendedor");
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frm (Add coment) " + ex.Message);
                LogService.WriteError("frm (Add coment) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        private void btnDelete_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            mObjAuction = mObjCyCServiceFactory.GetCyCServices().GetAuction(cboFolio.Value);
            try
            {
                if (mIntSelectedRowComent > 0)
                {

                    switch (mObjUserDTO.CostigCode)
                    {
                        case "CR_CORRA":
                            if (!mObjAuction.AutCorral)
                            {
                                DeleteComment();
                            }
                            else
                            {
                                UIApplication.ShowMessageBox("No es posible borrar un comentario, el proceso de corrales ya ha finalizado");
                            }
                            break;

                        case "TR_TRANS":
                            if (!mObjAuction.AutTransp)
                            {
                                DeleteComment();
                            }
                            else
                            {
                                UIApplication.ShowMessageBox("No es posible borrar un comentario, el proceso de transporte ya ha finalizado");
                            }
                            break;
                        case "OG_CRYCO":
                            DeleteComment();
                            break;
                    }
                }
                else
                {
                    UIApplication.ShowMessageBox("Favor de seleccionar un comentario");
                }

            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frm (Add coment) " + ex.Message);
                LogService.WriteError("frm (Add coment) " + ex.Message);
                LogService.WriteError(ex);
            }


        }

        private void DeleteComment()
        {
            if (SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("¿Desea elimiar el item seleccionado?", 2, "Si", "No", "") == 1)
            {
                try
                {
                    this.UIAPIRawForm.Freeze(true);
                    //DtMatrix.Rows.Remove(mIntRowSelected - 1);
                    SAPbouiCOM.DataTable DtMatrixComent = this.UIAPIRawForm.DataSources.DataTables.Item("DtComent");
                    if (mIntSelectedRowComent > 0)
                    {
                        string lStrComentCode = DtMatrixComent.GetValue("C_Code", mIntSelectedRowComent - 1).ToString();
                        if (mObjCyCServiceFactory.GetComentService().Remove(lStrComentCode) == 0)
                        {
                            UIApplication.ShowMessageBox("Comentario borrado correctamente");
                            LoadMatrixComents();
                        }
                        else
                        {
                            UIApplication.ShowError("Error al eliminar un comentario");
                            LogService.WriteError("Error al eliminar un comentario");
                        }
                    }
                    else
                    {
                        UIApplication.ShowMessage("Favor de seleccionar un comentario");
                    }
                }
                catch (Exception ex)
                {
                    this.UIAPIRawForm.Freeze(false);
                    LogService.WriteError("(SBO_Application_MenuEvent 1293 true): " + ex.Message);
                    LogService.WriteError(ex);
                }
                finally
                {
                    this.UIAPIRawForm.Freeze(false);
                }
            }
        }

        private void btnEnd_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (!string.IsNullOrEmpty(cboFolio.Value) && btnEnd.Item.Enabled)
                {
                    if (btnEnd.Caption == "Abrir cobro")
                    {
                        if (UIApplication.ShowOptionBox("¿Desea abrir el cobro? ") == 1)
                        {
                            OpenPayment();
                        }
                    }
                    else
                    {
                        if (UIApplication.ShowOptionBox("¿Desea terminar? ") == 1)
                        {
                            EndPayment();
                        }
                    }
                }
                else
                {
                    UIApplication.ShowMessage("Favor de seleccionar una subasta");
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("EndPayment " + ex.Message);
                LogService.WriteError("EndPayment " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        private void btnFinish_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {

            BubbleEvent = true;
            if (!string.IsNullOrEmpty(cboFolio.Value))
            {
                if (UIApplication.ShowOptionBox("¿Desea terminar el proceso? ") == 1)
                {
                    Auction lObjAuction = mObjCyCServiceFactory.GetCyCServices().GetAuction(cboFolio.Value);


                    if ("CR_CORRA" == mObjUserDTO.CostigCode)
                    {
                        if (!lObjAuction.AutCorral)
                        {
                            lObjAuction.AutCorral = true;
                            if (mObjCyCServiceFactory.GetAuctionService().Update(lObjAuction) == 0)
                            {
                                UIApplication.ShowMessageBox("Proceso finalizado por el área de corrales");
                            }
                        }
                        else
                        {
                            UIApplication.ShowMessageBox("El proceso de corales ya ha sido finalizado");
                        }
                    }


                    if ("TR_TRANS" == mObjUserDTO.CostigCode)
                    {
                        if (!lObjAuction.AutTransp)
                        {
                            lObjAuction.AutTransp = true;
                            if (mObjCyCServiceFactory.GetAuctionService().Update(lObjAuction) == 0)
                            {
                                UIApplication.ShowMessageBox("Proceso finalizado por el área de transporte");
                            }
                        }
                        else
                        {
                            UIApplication.ShowMessageBox("El proceso de transporte ya ha sido finalizado");
                        }
                    }


                    if (lObjAuction.AutCorral && lObjAuction.AutTransp && lObjAuction.AutAuction)
                    {
                        List<MessageDTO> lLstMessagesDTO = mObjCyCServiceFactory.GetCyCServices().GetMessageDTO(cboFolio.Value);
                        bool lBolSaveAlert = false;
                        foreach (MessageDTO lObjMessage in lLstMessagesDTO)
                        {
                            if (mObjCyCServiceFactory.GetAlertService().SaveAlert(lObjMessage))
                            {
                                lBolSaveAlert = true;
                            }
                        }
                        if (lBolSaveAlert)
                        {
                            UIApplication.ShowMessageBox("Proceso terminado \n Se envió una alerta al departamento de crédito y cobranza");
                        }
                    }
                    else
                    {
                        UIApplication.ShowMessage("Alerta no enviada, faltan procesos por autorizar");
                    }

                    CloseForm();
                }
            }
            else
            {
                UIApplication.ShowMessage("Favor de seleccionar una subasta");
            }


        }

        private void lObjBtnOpAct_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (!string.IsNullOrEmpty(cboFolio.Value) && lObjBtnOpAct.Item.Enabled)
                {

                    if (UIApplication.ShowOptionBox("¿Desea abrir el cobro para subasta? ") == 1)
                    {
                        OpenPayment();
                    }

                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("EndPayment " + ex.Message);
                LogService.WriteError("EndPayment " + ex.Message);
                LogService.WriteError(ex);
            }

        }


        private void CloseForm()
        {
            this.UIAPIRawForm.Close();
        }

        //private void ClearForm()
        //{
        //    mtxAuction.Clear();
        //    mtxComent.
        //}




        #region SelectRow
        private void mtxAuction_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.Row > 0)
                {
                    mtxAuction.SelectRow(pVal.Row, true, false);
                    mIntSelectedRowAuction = pVal.Row;
                }
                else
                {
                    mIntSelectedRowAuction = 0;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmPayment (mtxSellers_ClickBefore) " + ex.Message);
                LogService.WriteError("frmPayment (mtxSellers_ClickBefore) " + ex.Message);
                LogService.WriteError(ex);
            }

        }


        private void mtxCollect_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.Row > 0)
                {
                    mtxCollect.SelectRow(pVal.Row, true, false);
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("SelectRow Payment " + ex.Message);
                LogService.WriteError("SelectRow Payment " + ex.Message);
                LogService.WriteError(ex);
            }

        }

        private void mtxInv_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.Row > 0)
                {
                    mtxInv.SelectRow(pVal.Row, true, false);
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("SelectRow Inv " + ex.Message);
                LogService.WriteError("SelectRow Inv " + ex.Message);
                LogService.WriteError(ex);
            }

        }

        private void mtxComent_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.Row > 0)
                {
                    mtxComent.SelectRow(pVal.Row, true, false);
                    mIntSelectedRowComent = pVal.Row;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("SelectRow Coment " + ex.Message);
                LogService.WriteError("SelectRow Coment " + ex.Message);
                LogService.WriteError(ex);
            }

        }

        private void mtxInv_DoubleClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            SetTotalAmount(pVal.Row);
        }

        #endregion

        #endregion

        #region Methods
        private void LoadCboAuctions()
        {
            try
            {
                List<string> lLstAuctions = mObjCyCServiceFactory.GetCyCServices().GetAuctions(mObjUserDTO.CostigCode, mObjUserDTO.UserCode);

                foreach (string lStrFolio in lLstAuctions)
                {
                    cboFolio.ValidValues.Add(lStrFolio, "");
                    cboFolio.Item.DisplayDesc = false;
                }

                //cboFolio.ValidValues.Add("S-HMO-180031", "Prueba");
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmCollection (LoadCboAuctions) " + ex.Message);
                LogService.WriteError("frmCollection (LoadCboAuctions) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        #region MatrixAuction
        //Load 
        private void LoadMatrixAuction()
        {
            try
            {
                List<AuctionDTO> lLstAuctionDTO = SearchAuction();
                SetDataTableValuesAuction(lLstAuctionDTO);
                BindMatrixAuction();

            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmCollection (LoadMatrixAuction) " + ex.Message);
                LogService.WriteError("frmCollection (LoadMatrixAuction) " + ex.Message);
                LogService.WriteError(ex);

            }
        }

        //Search
        private List<AuctionDTO> SearchAuction()
        {
            if (!string.IsNullOrEmpty(cboFolio.Value))
            {
                return mObjCyCServiceFactory.GetCyCServices().GetAuctionByCustomer(cboFolio.Value, mObjUserDTO.CostigCode, mObjUserDTO.CYC);
            }
            else
            {
                return new List<AuctionDTO>();
            }
        }

        //SetValue
        public void SetDataTableValuesAuction(List<AuctionDTO> pLstAuctionDTO)
        {
            try
            {
                SAPbouiCOM.DataTable DtMatrixAuction = this.UIAPIRawForm.DataSources.DataTables.Item("DtAuction");
                DtMatrixAuction.Rows.Clear();
                int i = 0;
                foreach (AuctionDTO lObjAuctionDTO in pLstAuctionDTO)
                {
                    DtMatrixAuction.Rows.Add();
                    DtMatrixAuction.SetValue("#", i, i + 1);
                    DtMatrixAuction.SetValue("C_ImpSub", i, lObjAuctionDTO.TotalSell);
                    DtMatrixAuction.SetValue("C_ImpDeud", i, lObjAuctionDTO.TotalBuy);
                    DtMatrixAuction.SetValue("C_Sell", i, lObjAuctionDTO.CardCode);
                    DtMatrixAuction.SetValue("C_AccountD", i, lObjAuctionDTO.AccountD);
                    DtMatrixAuction.SetValue("C_SellName", i, lObjAuctionDTO.CardName);
                    i++;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frm (BindDataMatrix) " + ex.Message);
                LogService.WriteError("frm (BindDataMatrix) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        //BindMatrix
        private void BindMatrixAuction()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                mtxAuction.Columns.Item("#").DataBind.Bind("DtAuction", "#");
                mtxAuction.Columns.Item("C_ImpSub").DataBind.Bind("DtAuction", "C_ImpSub");
                mtxAuction.Columns.Item("C_ImpDeud").DataBind.Bind("DtAuction", "C_ImpDeud");
                mtxAuction.Columns.Item("C_Sell").DataBind.Bind("DtAuction", "C_SellName");

                mtxAuction.LoadFromDataSource();
                mtxAuction.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frm (BindDataMatrix) " + ex.Message);
                LogService.WriteError("frm (BindDataMatrix) " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        #endregion

        #region MatrixInvoice
        //Load
        private void LoadMatrixInvoice(string pStrCardCode)
        {
            try
            {
                List<InvoiceDTO> lLstInvoiceDTO = SearchInvoice(pStrCardCode);
                SetDataTableValuesInvoice(lLstInvoiceDTO);
                BindMatrixInvoice();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Search
        private List<InvoiceDTO> SearchInvoice(string pStrCardCode)
        {
            if (!string.IsNullOrEmpty(pStrCardCode))
            {
                return mObjCyCServiceFactory.GetCyCServices().GetInvoices(pStrCardCode, mObjUserDTO.CostigCode, mObjUserDTO.CYC);

                //if (mObjUserDTO.CYC == 'Y')
                //{

                //    return mObjCyCServiceFactory.GetCyCServices().GetInvoices(pStrCardCode, mObjUserDTO.CostigCode, "1");
                //}
                //else
                //{
                //    return mObjCyCServiceFactory.GetCyCServices().GetInvoices(pStrCardCode, "", "2");
                //}
            }
            else
            {
                return new List<InvoiceDTO>();
            }
        }

        //SetValue
        public void SetDataTableValuesInvoice(List<InvoiceDTO> pLstInvoiceDTO)
        {
            try
            {
                SAPbouiCOM.DataTable DtMatrixInvoice = this.UIAPIRawForm.DataSources.DataTables.Item("DtInv");
                DtMatrixInvoice.Rows.Clear();
                int i = 0;
                foreach (InvoiceDTO lObjInvoice in pLstInvoiceDTO)
                {
                    DtMatrixInvoice.Rows.Add();
                    DtMatrixInvoice.SetValue("#", i, i + 1);
                    DtMatrixInvoice.SetValue("C_DocEntry", i, lObjInvoice.DocEntry);
                    DtMatrixInvoice.SetValue("C_DocNum", i, lObjInvoice.DocNum);
                    DtMatrixInvoice.SetValue("C_Days", i, lObjInvoice.Days);
                    DtMatrixInvoice.SetValue("C_Date", i, lObjInvoice.Date);
                    DtMatrixInvoice.SetValue("C_Area", i, lObjInvoice.Area);
                    DtMatrixInvoice.SetValue("C_Balance", i, Convert.ToDouble(lObjInvoice.Balance));
                    DtMatrixInvoice.SetValue("C_Amount", i, Convert.ToDouble(lObjInvoice.Amount));
                    i++;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frm (SetDataTableValuesInvoice) " + ex.Message);
                LogService.WriteError("frm (SetDataTableValuesInvoice) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        //BindMatrix
        public void BindMatrixInvoice()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);
                mtxInv.Columns.Item("#").DataBind.Bind("DtInv", "#");
                mtxInv.Columns.Item("C_DocNum").DataBind.Bind("DtInv", "C_DocNum");
                mtxInv.Columns.Item("C_Days").DataBind.Bind("DtInv", "C_Days");
                mtxInv.Columns.Item("C_Date").DataBind.Bind("DtInv", "C_Date");
                mtxInv.Columns.Item("C_Area").DataBind.Bind("DtInv", "C_Area");
                mtxInv.Columns.Item("C_Balance").DataBind.Bind("DtInv", "C_Balance");
                mtxInv.Columns.Item("C_Amount").DataBind.Bind("DtInv", "C_Amount");
                mtxInv.LoadFromDataSource();
                mtxInv.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frm (BindDataMatrix) " + ex.Message);
                LogService.WriteError("frm (BindDataMatrix) " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        //Set 0 or total amount per line
        private void SetTotalAmount(int pIntRow)
        {
            decimal lDbTot = Convert.ToDecimal((mtxInv.Columns.Item("C_Amount").Cells.Item(pIntRow).Specific as EditText).Value.Trim());
            if(lDbTot>0)
            {
                (mtxInv.Columns.Item("C_Amount").Cells.Item(pIntRow).Specific as EditText).Value = "0";
            }
            else
            {
                (mtxInv.Columns.Item("C_Amount").Cells.Item(pIntRow).Specific as EditText).Value =
                (mtxInv.Columns.Item("C_Balance").Cells.Item(pIntRow).Specific as EditText).Value;
            }

        }
        #endregion

        #region MatrixPayment
        //Load
        private void LoadMatrixPayment()
        {
            try
            {
                List<PaymentsDTO> lLstPaymentDTO = new List<PaymentsDTO>();
                lLstPaymentDTO = SearchPayments();
                SetDataTableValuesPayments(lLstPaymentDTO);
                BindMatrixPayment();

            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmCollection (LoadMatrixPayment) " + ex.Message);
                LogService.WriteError("frmCollection (LoadMatrixPayment) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        //Search
        private List<PaymentsDTO> SearchPayments()
        {
            if (!string.IsNullOrEmpty(cboFolio.Value))
            {
                return mObjCyCServiceFactory.GetCyCServices().GetPays(cboFolio.Value);
            }
            else
            {
                return new List<PaymentsDTO>();
            }
        }

        //SetValue
        private void SetDataTableValuesPayments(List<PaymentsDTO> pLstPaymentsDTO)
        {
            try
            {
                SAPbouiCOM.DataTable DtMatrixPayment = this.UIAPIRawForm.DataSources.DataTables.Item("DtCollect");
                DtMatrixPayment.Rows.Clear();
                int i = 0;
                foreach (PaymentsDTO lObjPaymentDTO in pLstPaymentsDTO)
                {
                    DtMatrixPayment.Rows.Add();
                    DtMatrixPayment.SetValue("#", i, i + 1);
                    DtMatrixPayment.SetValue("C_DocEntry", i, lObjPaymentDTO.DocEntry);
                    DtMatrixPayment.SetValue("C_DocNum", i, lObjPaymentDTO.DocNum);
                    DtMatrixPayment.SetValue("C_DocDate", i, lObjPaymentDTO.DocDate);
                    DtMatrixPayment.SetValue("C_DocTotal", i, lObjPaymentDTO.DocTotal);
                    i++;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frm (SetDataTable Payment) " + ex.Message);
                LogService.WriteError("frm (SetDataTable Payment) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        //BindMatrix
        private void BindMatrixPayment()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);
                mtxCollect.Columns.Item("#").DataBind.Bind("DtCollect", "#");
                mtxCollect.Columns.Item("C_DocNum").DataBind.Bind("DtCollect", "C_DocNum");
                mtxCollect.Columns.Item("C_DocDate").DataBind.Bind("DtCollect", "C_DocDate");
                mtxCollect.Columns.Item("C_DocTotal").DataBind.Bind("DtCollect", "C_DocTotal");
                mtxCollect.LoadFromDataSource();
                mtxCollect.AutoResizeColumns();

            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frm (BindMatrixPayment) " + ex.Message);
                LogService.WriteError("frm (BindMatrixPayment) " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        #endregion

        #region MatrixComents
        //Load
        private void LoadMatrixComents()
        {
            try
            {
                List<Coments> lLstComentsDTO = new List<Coments>();
                lLstComentsDTO = SearchComents();
                SetDatatableValuesComents(lLstComentsDTO);
                BindMatrixComents();
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frm (LoadMatrixComent) " + ex.Message);
                LogService.WriteError("frm (LoadMatrixComent) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        //Search
        private List<Coments> SearchComents()
        {
            if (!string.IsNullOrEmpty(cboFolio.Value))
            {
                return mObjCyCServiceFactory.GetCyCServices().GetComents(cboFolio.Value, mObjUserDTO.CYC, mObjUserDTO.CostigCode, mStrSelectCardCode);
            }
            else
            {
                return new List<Coments>();
            }
        }

        //SetValue
        private void SetDatatableValuesComents(List<Coments> pLstComents)
        {
            try
            {
                SAPbouiCOM.DataTable DtMatrixComent = this.UIAPIRawForm.DataSources.DataTables.Item("DtComent");
                DtMatrixComent.Rows.Clear();
                int i = 0;
                foreach (Coments lObjComents in pLstComents)
                {
                    DtMatrixComent.Rows.Add();
                    DtMatrixComent.SetValue("#", i, i + 1);
                    DtMatrixComent.SetValue("C_Dep", i, lObjComents.Department);
                    DtMatrixComent.SetValue("C_DepName", i, lObjComents.DepartmentName);
                    DtMatrixComent.SetValue("C_User", i, lObjComents.User);
                    DtMatrixComent.SetValue("C_Coment", i, lObjComents.Coment);
                    DtMatrixComent.SetValue("C_Code", i, lObjComents.RowCode);
                    i++;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frm (SetDataTable Coments) " + ex.Message);
                LogService.WriteError("frm (SetDataTable Coments) " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        //BindMatrix
        private void BindMatrixComents()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);
                mtxComent.Columns.Item("#").DataBind.Bind("DtComent", "#");
                mtxComent.Columns.Item("C_DepName").DataBind.Bind("DtComent", "C_DepName");
                //mtxComent.Columns.Item("C_User").DataBind.Bind("DtComent", "C_User");
                mtxComent.Columns.Item("C_Coment").DataBind.Bind("DtComent", "C_Coment");
                mtxComent.LoadFromDataSource();
                mtxComent.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frm (BindMatrixComent) " + ex.Message);
                LogService.WriteError("frm (BindMatrixcoment) " + ex.Message);
                LogService.WriteError(ex);

            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }


        #endregion

        private void SumTotalPayment()
        {
            double lDblTotal = 0;
            SAPbouiCOM.DataTable DtMatrixInvoice = this.UIAPIRawForm.DataSources.DataTables.Item("DtInv");
            SAPbouiCOM.DataTable DtMatrixAuction = this.UIAPIRawForm.DataSources.DataTables.Item("DtAuction");
            for (int i = 0; i < DtMatrixInvoice.Rows.Count; i++)
            {
                lDblTotal += Convert.ToDouble(DtMatrixInvoice.GetValue("C_Amount", i));
            }
            mDblPayment = lDblTotal;

            txtAmount.Value = mDblPayment.ToString();
            txtBalance.Value = (Convert.ToDouble(DtMatrixAuction.GetValue("C_ImpSub", mIntSelectedRowAuction - 1)) - mDblPayment).ToString();

        }

        private void CreatePayment()
        {
            
            AuctionDTO lObjAuctionDTO = new AuctionDTO();
            List<InvoiceDTO> lLstObjInvoice = new List<InvoiceDTO>();
            SAPbouiCOM.DataTable DtMatrixInvoice = this.UIAPIRawForm.DataSources.DataTables.Item("DtInv");
            SAPbouiCOM.DataTable DtMatrixAuction = this.UIAPIRawForm.DataSources.DataTables.Item("DtAuction");

            lObjAuctionDTO.AuctionID = cboFolio.Value;
            lObjAuctionDTO.CardCode = mStrSelectCardCode;
            lObjAuctionDTO.TotalSell = mDblPayment.ToString();
            lObjAuctionDTO.AccountD = DtMatrixAuction.GetValue("C_AccountD", mIntSelectedRowAuction - 1).ToString();
            lObjAuctionDTO.LocationId = mObjAuction.Location;

            for (int i = 0; i < DtMatrixInvoice.Rows.Count; i++)
            {
                InvoiceDTO lObjInvoiceDTO = new InvoiceDTO();
                lObjInvoiceDTO.Amount = Convert.ToDecimal(DtMatrixInvoice.GetValue("C_Amount", i));
                lObjInvoiceDTO.DocEntry = DtMatrixInvoice.GetValue("C_DocEntry", i).ToString();
                if (lObjInvoiceDTO.Amount > 0)
                {
                    lLstObjInvoice.Add(lObjInvoiceDTO);
                }
            }

            if (mObjCyCServiceFactory.GetPaymentService().CreatePayment(lObjAuctionDTO, lLstObjInvoice))
            {
                UIApplication.ShowMessageBox("Pago generado correctamente");

                //OpenPaymentDraft(mStrSelectCardCode, cboFolio.Value);

                LoadMatrixAuction();
                LoadMatrixInvoice(mStrSelectCardCode);
                LoadMatrixPayment();
                if (mtxAuction.RowCount > 0 && CheckCurrentSeller())
                {
                    mtxAuction.SelectRow(mIntSelectedRowAuction, true, false);
                    SumTotalPayment();
                    txtBalance.Value = DtMatrixAuction.GetValue("C_ImpSub", mIntSelectedRowAuction - 1).ToString();
                }
            }
        }

        private void OpenPaymentDraft(string pStrCardCode, string pStrAuctionFolio)
        {
            int lIntKeyD = mObjCyCServiceFactory.GetCyCServices().GetPaymentDraft(mStrSelectCardCode, cboFolio.Value);
            if (lIntKeyD != 0)
            {
                //UIApplication.GetApplication().OpenForm((SAPbouiCOM.BoFormObjectEnum)140, "", DocEntry);

                lObjFormDraft = UIApplication.GetApplication().OpenForm((SAPbouiCOM.BoFormObjectEnum)140, "", lIntKeyD.ToString());
            }
        }

        private bool CheckCurrentSeller()
        {
            SAPbouiCOM.DataTable DtAuctions = this.UIAPIRawForm.DataSources.DataTables.Item("DtAuction");


            for (int i = 0; i < DtAuctions.Rows.Count; i++)
            {
                string lStrSeller = DtAuctions.GetValue("C_Sell", i).ToString();
                if (lStrSeller == mStrSelectCardCode)
                {
                    mIntSelectedRowAuction = i + 1;
                    return true;
                }
            }


            return false;
        }

        private void AddComent()
        {
            if (!string.IsNullOrEmpty(cboFolio.Value))
            {
                Coments lObjComents = new Coments();


                lObjComents.Coment = txtComent.Value;
                lObjComents.Folio = cboFolio.Value;
                lObjComents.User = UIApplication.GetCompany().UserName;
                lObjComents.Department = mObjUserDTO.Department;
                lObjComents.DepartmentName = mObjUserDTO.DepartmentName;
                lObjComents.Cardcocde = mStrSelectCardCode;
                lObjComents.CostCenter = mObjUserDTO.CostigCode;

                if (mObjCyCServiceFactory.GetComentService().Add(lObjComents) == 0)
                {
                    txtComent.Value = string.Empty;
                    LoadMatrixComents();
                    UIApplication.ShowMessageBox("Guardado correcto");
                }
                else
                {
                    UIApplication.ShowError("Error al guardar el comentarios");
                    LogService.WriteError("Error al guardar el comentarios");
                }
            }
            else
            {
                UIApplication.ShowMessage("Favor de seleccionar una subasta");
            }
        }


        private void EndPayment()
        {
            Auction lObjAuction = mObjCyCServiceFactory.GetCyCServices().GetAuction(cboFolio.Value);
            if (lObjAuction.AutCorral && lObjAuction.AutTransp && lObjAuction.AutAuction)
            {
                if (!lObjAuction.AutCyC)
                {
                    lObjAuction.AutCyC = true;
                    int lIntResult = mObjCyCServiceFactory.GetAuctionService().Update(lObjAuction);
                    if (lIntResult == 0)
                    {
                        List<MessageDTO> lLstMessagesDTO = mObjCyCServiceFactory.GetCyCServices().GetMessageDTO(cboFolio.Value);
                        bool lBolMsgOk = false;
                        foreach (MessageDTO lObjMessage in lLstMessagesDTO)
                        {
                            if (lObjMessage.UserCode == "TESORERIA4" || lObjMessage.UserCode == "TESORERIA3" || lObjMessage.UserCode == "SUBHE" || lObjMessage.UserCode == "SUBHE3") 
                            { 
                                if (mObjCyCServiceFactory.GetAlertService().SaveAlert(lObjMessage))
                                {
                                    lBolMsgOk = true;
                                }
                            }
                        }
                        if (lBolMsgOk)
                        {
                            UIApplication.ShowMessageBox("Proceso terminado \n Se envió una alerta al departamento de subasta");
                        }
                        //btnEnd.Caption = "Abrir cobro";
                        CloseForm();
                    }
                }
                else
                {
                    UIApplication.ShowMessage("El proceso ya a sido autorizado");
                }

            }
            else
            {
                UIApplication.ShowMessageBox("Faltan procesos por autorizar");
            }
        }

        private void OpenPayment()
        {
            Auction lObjAuction = mObjCyCServiceFactory.GetCyCServices().GetAuction(cboFolio.Value);
            if (!lObjAuction.AutFz)
            {
                if (lObjAuction.AutCyC)
                {
                    lObjAuction.AutCyC = false;
                    lObjAuction.AutAuction = false;

                    int lIntResult = mObjCyCServiceFactory.GetAuctionService().Update(lObjAuction);
                    if (lIntResult == 0)
                    {
                        UIApplication.ShowMessageBox("El cobro ha sido abierto");
                        btnEnd.Caption = "Terminar Cobro";
                        //LoadCboAuctions();
                        LoadMatrixAuction();
                        LoadMatrixComents();
                        LoadMatrixInvoice("");
                        LoadMatrixPayment();
                    }
                }
                else
                {
                    UIApplication.ShowMessage("El pago ya se encuentra abierto");
                }
            }
            else
            {
                UIApplication.ShowMessage("No es posible abrir el cobro debido a que finanzas ya termino su proceso");
            }
        }
        #endregion

        #region Controls
        private SAPbouiCOM.StaticText lblFolio;
        private SAPbouiCOM.ComboBox cboFolio;
        private SAPbouiCOM.Matrix mtxAuction;
        private SAPbouiCOM.StaticText lblComent;
        private SAPbouiCOM.EditText txtComent;
        private SAPbouiCOM.Button btnAdd;
        private SAPbouiCOM.Button btnFinish;
        private SAPbouiCOM.Matrix mtxComent;
        private SAPbouiCOM.Matrix mtxInv;
        private SAPbouiCOM.Matrix mtxCollect;
        private SAPbouiCOM.StaticText lblAmount;
        private SAPbouiCOM.StaticText lblBalance;
        private SAPbouiCOM.EditText txtAmount;
        private SAPbouiCOM.EditText txtBalance;
        private SAPbouiCOM.Button btnCollect;
        private SAPbouiCOM.Button btnEnd;
        private Button btnDelete;
        private Button lObjBtnOpAct;
        #endregion






    }
}
