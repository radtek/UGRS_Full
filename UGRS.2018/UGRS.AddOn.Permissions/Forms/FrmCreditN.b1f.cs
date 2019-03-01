using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.Permissions;
using UGRS.Core.SDK.DI.Permissions.DTO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.AddOn.Permissions.Forms {
    [FormAttribute("UGRS.AddOn.Permissions.Forms.FrmCreditN", "Forms/FrmCreditN.b1f")]
    class FrmCreditN : UserFormBase
    {

        #region properties
        PermissionsFactory mObjPermissionsFactory = new PermissionsFactory();
        int mIntRow = 0;
        SAPbouiCOM.DataTable DtMatrixInvoice;
        #endregion

        #region Initialize
        public FrmCreditN()
        {
            mtxInvoices.AutoResizeColumns();
        }
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);

        }
        private void OnCustomInitialize()
        {
            LoadMatrix();
        }
        #endregion

        #region Control Components
        private Button btnCreditN;
        private Matrix mtxInvoices;

        public override void OnInitializeComponent()
        {
            this.btnCreditN = ((SAPbouiCOM.Button)(this.GetItem("btnCreditN").Specific));
            this.btnCreditN.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCreditN_ClickBefore);
           
            this.mtxInvoices = ((SAPbouiCOM.Matrix)(this.GetItem("mtx0").Specific));
            this.mtxInvoices.LinkPressedAfter += new SAPbouiCOM._IMatrixEvents_LinkPressedAfterEventHandler(this.mtxInvoices_LinkPressedAfter);
            this.mtxInvoices.ClickAfter += new SAPbouiCOM._IMatrixEvents_ClickAfterEventHandler(this.mtx0_ClickAfter);
            // this.UIAPIRawForm.ReportType = "RIN3";
            this.OpenReport();
            //typeof(UGRS.Core.SDK.UI.UIApplication).GetApplication().LayoutKeyEvent += new SAPbouiCOM._IApplicationEvents_LayoutKeyEventEventHandler(this.FrmCreditN_LayoutKeyEvent);
            this.OnCustomInitialize();

        }

        //void FrmCreditN_LayoutKeyEvent(ref LayoutKeyInfo eventInfo, out bool BubbleEvent)
        //{
        //    BubbleEvent = true;
           
          
        //}
        #endregion

        #region Events

        private void btnCreditN_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
          
            CreateCreditNote();

        }

        private void btnReport_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void mtx0_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            mIntRow = pVal.Row;
            if (mIntRow > 0)
            {
                mtxInvoices.SelectRow(mIntRow, true, false);
            }

        }

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            mtxInvoices.AutoResizeColumns();

        }

        private void mtxInvoices_LinkPressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.ColUID == "C_Fact")
                {
                    string lStrDocEntry = DtMatrixInvoice.GetValue("C_DocEntry", pVal.Row - 1).ToString();
                    SAPbouiCOM.Framework.Application.SBO_Application.OpenForm(BoFormObjectEnum.fo_Invoice, "", lStrDocEntry);
                }
            }
            catch (Exception ex)
            {


            }
        }


        #endregion

        #region Methods
        private void CreateCreditNote()
        {
            bool lbolResult = false;
            try
            {
                DIApplication.Company.StartTransaction();

                string lStrDocEntry = DtMatrixInvoice.GetValue("C_DocEntry", mIntRow - 1).ToString();
                double lDblPaid = Convert.ToDouble(DtMatrixInvoice.GetValue("C_Paid", mIntRow - 1).ToString());

                bool lBolPaid = lDblPaid > 0 ? true : false;
                if (mObjPermissionsFactory.GetCreditNoteService().CrateCreditNote(lStrDocEntry, lBolPaid) == 0)
                {
                    string pStrCert = DtMatrixInvoice.GetValue("C_Cert", mIntRow - 1).ToString();
                    List<string> lLstCertificates = mObjPermissionsFactory.GetPermissionsService().GetCertificates(pStrCert);


                    foreach (string lStrCert in lLstCertificates)
                    {
                        if (!UpdateCertificate(lStrCert))
                        {
                            lbolResult = false;
                            break;
                        }
                        else
                        {
                            lbolResult = true;
                        }
                    }
                    if (lbolResult)
                    {
                        lbolResult = true;
                        UIApplication.ShowMessageBox("Nota de crédito realizada con éxito");
                        LoadMatrix();
                    }

                }
            }
            catch (Exception ex)
            {
                lbolResult = false;
                LogService.WriteError("PurchasesDAO (UpdateStatus): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                try
                {
                    if (lbolResult)
                    {
                        DIApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                    }
                    else
                    {
                        if (DIApplication.Company.InTransaction)
                        {
                            DIApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogService.WriteError("PurchasesDAO (UpdateStatus): " + ex.Message);
                    LogService.WriteError(ex);
                }
            }
        }

        private bool UpdateCertificate(string pStrKey)
        {
            SAPbobsCOM.UserTable lObjsboTable = (SAPbobsCOM.UserTable)DIApplication.Company.UserTables.Item("UG_CU_CERT");
            if (lObjsboTable.GetByKey(pStrKey))
            {
                lObjsboTable.UserFields.Fields.Item("U_CreditNote").Value = "Y";

                if (lObjsboTable.Update() != 0)
                {
                    string lStrError = DIApplication.Company.GetLastErrorDescription();
                    UIApplication.ShowError(lStrError);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                string lStrError = "Registro: " + pStrKey + " No encontrado";
            }
            return false;
        }

        private void LoadMatrix()
        {
            List<InvoiceExpDTO> lLstInvoice = new List<InvoiceExpDTO>();
            lLstInvoice = mObjPermissionsFactory.GetPermissionsService().GetInvoices();

            SetDataTableValuesAuction(lLstInvoice);
            BindMatrixAuction();
        }

        private void BindMatrixAuction()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);
                mtxInvoices.Columns.Item("C_#").DataBind.Bind("DT_NC", "#");
                mtxInvoices.Columns.Item("C_Client").DataBind.Bind("DT_NC", "C_BP");
                mtxInvoices.Columns.Item("C_Cert").DataBind.Bind("DT_NC", "C_Cert");
                mtxInvoices.Columns.Item("C_Fact").DataBind.Bind("DT_NC", "C_Inv");
                mtxInvoices.Columns.Item("C_HeadsF").DataBind.Bind("DT_NC", "C_HeadInv");
                mtxInvoices.Columns.Item("C_HeadsE").DataBind.Bind("DT_NC", "C_HeadExp");
                mtxInvoices.Columns.Item("C_HeadsN").DataBind.Bind("DT_NC", "C_HeadNoCr");
                mtxInvoices.Columns.Item("C_Import").DataBind.Bind("DT_NC", "C_Amount");

                mtxInvoices.LoadFromDataSource();
                mtxInvoices.AutoResizeColumns();

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

        //SetValue
        private void SetDataTableValuesAuction(List<InvoiceExpDTO> pLstAuctionDTO)
        {
            try
            {
                DtMatrixInvoice = this.UIAPIRawForm.DataSources.DataTables.Item("DT_NC");
                DtMatrixInvoice.Rows.Clear();
                int i = 0;

                foreach (InvoiceExpDTO lObjInvoiceExp in pLstAuctionDTO)
                {
                    DtMatrixInvoice.Rows.Add();
                    DtMatrixInvoice.SetValue("#", i, i + 1);
                    DtMatrixInvoice.SetValue("C_BP", i, lObjInvoiceExp.BussinesPartner);
                    DtMatrixInvoice.SetValue("C_Cert", i, lObjInvoiceExp.Certificate);
                    DtMatrixInvoice.SetValue("C_Inv", i, lObjInvoiceExp.DocNum);
                    DtMatrixInvoice.SetValue("C_HeadInv", i, lObjInvoiceExp.HeadInvoice);
                    DtMatrixInvoice.SetValue("C_HeadExp", i, lObjInvoiceExp.HeatExport);
                    DtMatrixInvoice.SetValue("C_HeadNoCr", i, lObjInvoiceExp.HeadNoCruz);
                    DtMatrixInvoice.SetValue("C_Amount", i, lObjInvoiceExp.Amount);
                    DtMatrixInvoice.SetValue("C_DocEntry", i, lObjInvoiceExp.DocEntry);
                    DtMatrixInvoice.SetValue("C_Paid", i, lObjInvoiceExp.PaidToDate);
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

        private void OpenReport()
        {
            try
            {
                SAPbobsCOM.ReportTypesService rptTypeService = (SAPbobsCOM.ReportTypesService)
                    DIApplication.Company.GetCompanyService().GetBusinessService(SAPbobsCOM.ServiceTypes.ReportTypesService);
                SAPbobsCOM.ReportTypeParams rptParams = (SAPbobsCOM.ReportTypeParams)
                    rptTypeService.GetDataInterface(SAPbobsCOM.ReportTypesServiceDataInterfaces.rtsReportTypeParams);

                rptParams.TypeCode = "RIN3";

                SAPbobsCOM.ReportType updateType = rptTypeService.GetReportType(rptParams);
                updateType.DefaultReportLayout = "RIN30004";

                rptTypeService.UpdateReportType(updateType);

                this.UIAPIRawForm.ReportType = "RIN3";
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox("No fue posible cargar el reporte");
                LogService.WriteError(ex);

            }
        }
        #endregion
    
    }
}
