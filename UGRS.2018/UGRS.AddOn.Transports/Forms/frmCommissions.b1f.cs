using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using SAPbouiCOM;
using UGRS.Core.SDK.DI.Transports;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.SDK.DI.Transports.Tables;
using System.Globalization;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.UI.ProgressBar;
using UGRS.Core.SDK.DI.Tramsport.DTO;
using System.Drawing;
using UGRS.Core.SDK.DI.Transports.Enums;
using UGRS.Core.Extension.Enum;

namespace UGRS.AddOn.Transports.Forms
{
    [FormAttribute("UGRS.AddOn.Transports.Forms.frmCommissions", "Forms/frmCommissions.b1f")]
    class frmCommissions : UserFormBase
    {
        #region Properties
        int mIntSelectedRow;
        int mIntSelectedComissionRow;
        TransportServiceFactory mObjTransportsFactory = new TransportServiceFactory();
        int mIntFirstDay;
        DateTime mDtmFirstDay;
        DateTime mDtmLastDay;
        string mStrFolio;
        const int mIntColorGreen = 39219;
        const int mIntColorWhite = 16777215;
        const int mIntColorRed = 13369344;
        StatusEnum mEnumStatus;
        AuthorizerEnum mAuthorizerEnum;

        #endregion
      
        #region Constructor
        public frmCommissions()
        {
            LoadEvents();
            
        }

        #endregion

        #region InitializeComponents
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mObjlblFolio = ((SAPbouiCOM.StaticText)(this.GetItem("lblFolio").Specific));
            this.mObjTxtFolio = ((SAPbouiCOM.EditText)(this.GetItem("txtFolio").Specific));
            this.mObjTxtFolio.ValidateAfter += new SAPbouiCOM._IEditTextEvents_ValidateAfterEventHandler(this.mObjTxtFolio_ValidateAfter);
            this.mObjBtnSearch = ((SAPbouiCOM.Button)(this.GetItem("btnSearch").Specific));
            this.mObjBtnSearch.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.mObjBtnSearch_ClickAfter);
            this.mObjTxtStatus = ((SAPbouiCOM.EditText)(this.GetItem("txtStatus").Specific));
            this.mObjlblStat = ((SAPbouiCOM.StaticText)(this.GetItem("lblStat").Specific));
            this.mObjTxtArea = ((SAPbouiCOM.EditText)(this.GetItem("txtAuArea").Specific));
            this.mObjBtnAuth = ((SAPbouiCOM.Button)(this.GetItem("btnAuth").Specific));
            this.mObjBtnAuth.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.mObjBtnAuth_ClickAfter);
            this.mObjBtnReject = ((SAPbouiCOM.Button)(this.GetItem("BtnRej").Specific));
            this.mObjlblArea = ((SAPbouiCOM.StaticText)(this.GetItem("lblAuArea").Specific));
            this.mObjMtxDrv = ((SAPbouiCOM.Matrix)(this.GetItem("mtxDrv").Specific));
            this.mObjMtxDrv.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.mObjMtxDrv_ClickBefore);
            this.mObjMtxCommission = ((SAPbouiCOM.Matrix)(this.GetItem("mtxCom").Specific));
            this.mObjMtxCommission.LinkPressedAfter += new SAPbouiCOM._IMatrixEvents_LinkPressedAfterEventHandler(this.mObjMtxCommission_LinkPressedAfter);
            this.mObjMtxCommission.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.mObjMtxCommission_ClickBefore);
            this.mObjBtnOk = ((SAPbouiCOM.Button)(this.GetItem("btnOk").Specific));
            this.mObjBtnOk.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.mObjBtnOk_ClickBefore);
            this.mObjBtnOk.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.mObjBtnOk_ClickAfter);
            this.mObjlblCmnt = ((SAPbouiCOM.StaticText)(this.GetItem("lblCmnt").Specific));
            this.mObjTxtCmnt = ((SAPbouiCOM.EditText)(this.GetItem("txtCmnt").Specific));
            this.mObjBtnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCancel").Specific));
            this.lblDates = ((SAPbouiCOM.StaticText)(this.GetItem("lblDates").Specific));
            this.txtAuOp = ((SAPbouiCOM.EditText)(this.GetItem("txtAuOp").Specific));
            this.lblAuOp = ((SAPbouiCOM.StaticText)(this.GetItem("lblAuOp").Specific));
            this.lblAuFz = ((SAPbouiCOM.StaticText)(this.GetItem("lblAuFz").Specific));
            this.txtAuFz = ((SAPbouiCOM.EditText)(this.GetItem("txtAuFz").Specific));
            // this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("Item_4").Specific));
            //  this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_5").Specific));
            this.cboWeek = ((SAPbouiCOM.ComboBox)(this.GetItem("cboWeek").Specific));
            this.lblWeek = ((SAPbouiCOM.StaticText)(this.GetItem("lblWeek").Specific));
            this.lblYear = ((SAPbouiCOM.StaticText)(this.GetItem("lblYear").Specific));
            this.cboYear = ((SAPbouiCOM.ComboBox)(this.GetItem("cboYear").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);

        }


        private void OnCustomInitialize()
        {
            try
            {
                //LoadChooseFromList();
               // SetChooseToTxt();
                
                mStrFolio = GetFolioWeek();
                mObjTxtFolio.Value = mStrFolio;
                LoadFirstDay();
                VisibleAuthControls(false);
                VisibleStatusControls(false);
                
                //Obtiene el tipo permiso
                GetUserPermissionAuth();

              
               
            }
            catch (Exception ex)
            {
                LogService.WriteError("(frmCommissions): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(ex.Message);

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
                mObjMtxDrv.Item.Width = UIAPIRawForm.Width - 50;
                mObjMtxCommission.Item.Width = UIAPIRawForm.Width - 50;

                //Height
                mObjMtxDrv.Item.Height = UIAPIRawForm.Height / 3;
                mObjMtxCommission.Item.Height = (int)(UIAPIRawForm.Height /3.5);

                ////Left
                //mOBjMtxDrv.Item.Left = mOBjMtxDrv.Item.Width + 5;
                //mObjMtxCommission.Item.Left = mObjMtxCommission.Item.Width + ;5w
                
                //Top
                mObjMtxCommission.Item.Top = mObjMtxDrv.Item.Top + mObjMtxDrv.Item.Height + 20;
              

                mObjMtxDrv.AutoResizeColumns();
                mObjMtxCommission.AutoResizeColumns();

                
            }
            catch (Exception lObjException)
            {
                UIApplication.ShowError("frmCommissions (Form_ResizeAfter)" + lObjException.Message);
                LogService.WriteError("frmCommissions (Form_ResizeAfter)" + lObjException.Message);
                LogService.WriteError(lObjException);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        private void mObjMtxDrv_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (pVal.Row > 0)
                {
                    mIntSelectedRow = pVal.Row;
                    mObjMtxDrv.SelectRow(pVal.Row, true, false);
                    BindMatrixInv();
                    LoadDtCommissionInfo();

                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("(frmCommissions): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void mObjMtxCommission_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (pVal.Row > 0)
                {
                    mIntSelectedComissionRow = pVal.Row;
                    mObjMtxCommission.SelectRow(pVal.Row, true, false);

                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("(frmCommissions): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }

        }

        private void mObjMtxCommission_LinkPressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                mDtMatrixCommisssion = this.UIAPIRawForm.DataSources.DataTables.Item("Dt_Cmsn");
                string lStrDocEntry = mDtMatrixCommisssion.GetValue("cDocEntry", pVal.Row - 1).ToString();
                SAPbouiCOM.Framework.Application.SBO_Application.OpenForm(BoFormObjectEnum.fo_Invoice, "", lStrDocEntry);
                //SAPbouiCOM.Framework.Application.SBO_Application.OpenForm(BoFormObjectEnum.fo_JournalPosting, "", lStrDocEntry);
            }
            catch (Exception ex)
            {
                LogService.WriteError("(mObjMtxCommission_LinkPressedAfter): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(ex.Message);
            }

        }

        private void mObjBtnOk_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            string lStrDocEntry = String.Empty;
            bool lBolSuccess = false;
          
            try
            {
                DIApplication.Company.StartTransaction();
                this.UIAPIRawForm.Freeze(true);
                lBolSuccess = SaveCommission();
                

            }
            catch (Exception ex)
            {
                lBolSuccess = false;
                LogService.WriteError("SaveCommission " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(ex.Message);
            }
            finally
            {

                try
                {
                    if (lBolSuccess)
                    {
                        DIApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                        UIApplication.ShowMessageBox(string.Format("Proceso realizado correctamente"));

                    }
                    else
                    {
                        //mStrCodeVoucher = string.Empty;
                        if (DIApplication.Company.InTransaction)
                        {
                            DIApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }
                    }

                }
                catch (Exception ex)
                {
                    UIApplication.ShowMessageBox(ex.Message);
                    LogService.WriteError("(btnSave_ClickBefore): " + ex.Message);
                    LogService.WriteError(ex);
                }


                this.UIAPIRawForm.Freeze(false);
            }

        }
 
        private void mObjBtnOk_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

        }

        private void mObjBtnSearch_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);
                BindMatrixDriv();
                Commissions lObjCommissionHeader =  GetCommisionHeader(mObjTxtFolio.Value);
                if (lObjCommissionHeader == null)
                {
                    VisibleAuthControls(false);
                    VisibleStatusControls(false);
                  
                    SetTxtStatus(StatusEnum.OPEN);
                }
                else
                { 
                    SetStatusControls(lObjCommissionHeader);
                    SetTxtStatus(StatusEnum.OPEN);
                    VisibleStatusControls(true);

                    //Coloca los controles dependiendo del permiso
                    SetControlsByPermissionUser();

                } 
                LoadDtDivInfo(GetComisssionDirverList());
               
            }
            catch (Exception ex)
            {
                LogService.WriteError("mObjBtnSearch_ClickBefore " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }

        }

        private void mObjTxtFolio_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                string lStrFolio =  mObjTxtFolio.Value;
                int lIntWeek = Convert.ToInt32(lStrFolio.Substring(0, 2));
                int lIntYear = Convert.ToInt32(lStrFolio.Substring(3, 2));

                mDtmFirstDay = new DateTime(lIntYear+2000, 1, 1);
                mDtmFirstDay = mDtmFirstDay.AddDays(((lIntWeek - 1) * 7));
                mDtmLastDay = mDtmFirstDay.AddDays(6);

                mStrFolio = lIntWeek.ToString("00") + "-" + lIntYear.ToString("00");
                lblDates.Caption = " Fechas:   Del " + mDtmFirstDay.ToString("dd/MM/yyyy") + "   Al   " + mDtmLastDay.ToString("dd/MM/yyyy");

            }
            catch (Exception ex )
            {
                LogService.WriteError("mObjTxtFolio_ValidateAfter " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(ex.Message);
            }

        }

        private void mObjBtnAuth_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                Commissions lObjCommissionHeader = GetCommisionHeader(mObjTxtFolio.Value);
                if (UpdateCommission(lObjCommissionHeader))
                {
                    SendAlertByAuthorizer(lObjCommissionHeader);
                    UIApplication.ShowMessageBox(string.Format("Folio: {0} fue autorizado correctamente", lObjCommissionHeader.Folio));
                    SetStatusControls(GetCommisionHeader(mObjTxtFolio.Value));

                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("mObjBtnAuth_ClickAfter " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(ex.Message);
            }



        }

        #endregion

        #region Methods
        #region Load & Unload Events
        public void LoadEvents()
        {
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);
        }

        public void UnLoadEvents()
        {
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);
        }
        #endregion

         /* #region ChooseFromList
        private void SetChooseToTxt()
        {
            mObjTxtArea.DataBind.SetBound(true, "", "CFL_Area");
            mObjTxtArea.ChooseFromListUID = "CFL_Area";
            mObjTxtArea.ChooseFromListAlias = "PrcCode";
        }

        private void LoadChooseFromList()
        {
            ChooseFromList lObjCFLProjectArea = InitChooseFromLists(false, "61", "CFL_Area", this.UIAPIRawForm.ChooseFromLists);
            AddConditionChoseFromListArea(lObjCFLProjectArea);
        }

        /// <summary>
        /// Condiciones de chooseFromList
        /// <summary>
        private void AddConditionChoseFromListArea(ChooseFromList pCFL)
        {

            SAPbouiCOM.Condition lObjCon = null;
            SAPbouiCOM.Conditions lObjCons = null;
            lObjCons = pCFL.GetConditions();

            //DimCode
            lObjCon = lObjCons.Add();
            lObjCon.Alias = "DimCode";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "1";

            lObjCon.Relationship = BoConditionRelationship.cr_AND;

            lObjCon = lObjCons.Add();
            lObjCon.Alias = "Active";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "Y";
            //lObjCon.Relationship = BoConditionRelationship.cr_AND;

            //lObjCon = lObjCons.Add();
            //lObjCon.Alias = "GrpCode";
            //lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL;

            pCFL.SetConditions(lObjCons);

        }

        /// <summary>
        /// inicia los datos a un ChooseFromList
        /// <summary>
        public ChooseFromList InitChooseFromLists(bool pbolMultiselecction, string pStrType, string pStrID, SAPbouiCOM.ChooseFromListCollection pObjCFLs) //
        {
            SAPbouiCOM.ChooseFromList lObjoCFL = null;

            SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
            oCFLCreationParams = (SAPbouiCOM.ChooseFromListCreationParams)UIApplication.GetApplication().CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams);

            oCFLCreationParams.MultiSelection = pbolMultiselecction;
            oCFLCreationParams.ObjectType = pStrType;
            oCFLCreationParams.UniqueID = pStrID;

            lObjoCFL = pObjCFLs.Add(oCFLCreationParams);

            this.UIAPIRawForm.DataSources.UserDataSources.Add(pStrID, SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 254);

            return lObjoCFL;
        }

        #endregion
        */

        #region Matrix
        private List<CommissionDriverDTO> GetComisssionDirverList()
        {
            mDtMatrixDrv = this.UIAPIRawForm.DataSources.DataTables.Item("Dt_Driv");
            mDtMatrixDrv.Rows.Clear();
            List<CommissionDriverDTO> lLstCommisssionDriverDTO = mObjTransportsFactory.GetCommissionService().GetCommissionDriver(mDtmFirstDay.ToString("yyyyMMdd"), mDtmLastDay.ToString("yyyyMMdd"));

            lLstCommisssionDriverDTO = lLstCommisssionDriverDTO.GroupBy(x => x.DriverId).Select(y => new CommissionDriverDTO
            {
                DriverId = y.First().DriverId,
                Driver = y.First().Driver,
                FrgAm = y.Sum(s => s.FrgAm),
                InsAm = y.Sum(s => s.InsAm),
                LstDisc = y.First().LstDisc, //Pendiente
                WkDisc = y.First().WkDisc,
                TotDisc = y.First().LstDisc + y.First().WkDisc,
                Comm = y.Sum(s => s.Comm),
                TotComm = y.Sum(s => s.Comm) - (y.First().LstDisc + y.First().WkDisc) > 0 ? y.Sum(s => s.Comm) - (y.First().LstDisc + y.First().WkDisc) : 0,
                Doubt = y.Sum(s => s.Comm) - (y.First().LstDisc + y.First().WkDisc) < 0 ? (y.First().LstDisc + y.First().WkDisc) - y.Sum(s => s.Comm) : 0,
            }).ToList();

            return lLstCommisssionDriverDTO;
        }



        private void LoadDtDivInfo(List<CommissionDriverDTO> lLstCommisssionDriverDTO)
        {

            int i = 0;

            mObjProgressBar = new ProgressBarManager(UIApplication.GetApplication(), "Consultando comisiones", lLstCommisssionDriverDTO.Count + 2);
            foreach (CommissionDriverDTO lObjCommissionDrvierDTO in lLstCommisssionDriverDTO)
            {
                mDtMatrixDrv.Rows.Add();
                mDtMatrixDrv.SetValue("#", i, i + 1);
                mDtMatrixDrv.SetValue("cDrvr", i, lObjCommissionDrvierDTO.Driver);
                mDtMatrixDrv.SetValue("cDrvrId", i, lObjCommissionDrvierDTO.DriverId);
                mDtMatrixDrv.SetValue("cFrgAm", i, lObjCommissionDrvierDTO.FrgAm);
                mDtMatrixDrv.SetValue("cInsAm", i, lObjCommissionDrvierDTO.InsAm);
                mDtMatrixDrv.SetValue("cLstDisc", i, lObjCommissionDrvierDTO.LstDisc);
                mDtMatrixDrv.SetValue("cWkDisc", i, lObjCommissionDrvierDTO.WkDisc);
                mDtMatrixDrv.SetValue("cTotDisc", i, lObjCommissionDrvierDTO.TotDisc);
                mDtMatrixDrv.SetValue("cComm", i, lObjCommissionDrvierDTO.Comm);
                mDtMatrixDrv.SetValue("cTotComm", i, lObjCommissionDrvierDTO.TotComm);
                mDtMatrixDrv.SetValue("cDoubt", i, lObjCommissionDrvierDTO.Doubt);
                i++;
                mObjProgressBar.NextPosition();
            }
            AddTotalLine(i, lLstCommisssionDriverDTO);
            mObjProgressBar.NextPosition();

            mObjMtxDrv.LoadFromDataSource();
            mObjMtxDrv.AutoResizeColumns();
            mObjProgressBar.Dispose();
        }

        private void AddTotalLine(int i, List<CommissionDriverDTO> pLstCommisssionDriverDTO)
        {
            mDtMatrixDrv.Rows.Add();
            mDtMatrixDrv.SetValue("#", i, i + 1);
            mDtMatrixDrv.SetValue("cDrvr", i, "TOTAL");
            mDtMatrixDrv.SetValue("cDrvrId", i, "");
            mDtMatrixDrv.SetValue("cFrgAm", i, pLstCommisssionDriverDTO.Sum(x => x.FrgAm));
            mDtMatrixDrv.SetValue("cInsAm", i, pLstCommisssionDriverDTO.Sum(x => x.InsAm));
            mDtMatrixDrv.SetValue("cLstDisc", i, pLstCommisssionDriverDTO.Sum(x => x.LstDisc));
            mDtMatrixDrv.SetValue("cWkDisc", i, pLstCommisssionDriverDTO.Sum(x => x.WkDisc));
            mDtMatrixDrv.SetValue("cTotDisc", i, pLstCommisssionDriverDTO.Sum(x => x.TotDisc));
            mDtMatrixDrv.SetValue("cComm", i, pLstCommisssionDriverDTO.Sum(x => x.Comm));
            mDtMatrixDrv.SetValue("cTotComm", i, pLstCommisssionDriverDTO.Sum(x => x.TotComm));
            mDtMatrixDrv.SetValue("cDoubt", i, pLstCommisssionDriverDTO.Sum(x => x.Doubt));
        }

        private void BindMatrixDriv()
        {
            mObjMtxDrv.Columns.Item("#").DataBind.Bind("Dt_Driv", "#");
            mObjMtxDrv.Columns.Item("cDrvr").DataBind.Bind("Dt_Driv", "cDrvr");
            mObjMtxDrv.Columns.Item("cFrgAm").DataBind.Bind("Dt_Driv", "cFrgAm");
            mObjMtxDrv.Columns.Item("cInsAm").DataBind.Bind("Dt_Driv", "cInsAm");
            mObjMtxDrv.Columns.Item("cLstDisc").DataBind.Bind("Dt_Driv", "cLstDisc");
            mObjMtxDrv.Columns.Item("cWkDisc").DataBind.Bind("Dt_Driv", "cWkDisc");
            mObjMtxDrv.Columns.Item("cTotDisc").DataBind.Bind("Dt_Driv", "cTotDisc");
            mObjMtxDrv.Columns.Item("cComm").DataBind.Bind("Dt_Driv", "cComm");
            mObjMtxDrv.Columns.Item("cTotComm").DataBind.Bind("Dt_Driv", "cTotComm");
            mObjMtxDrv.Columns.Item("cDoubt").DataBind.Bind("Dt_Driv", "cDoubt");
        }

        private void BindMatrixInv()
        {
            mObjMtxCommission.Columns.Item("#").DataBind.Bind("Dt_Cmsn", "#");
            mObjMtxCommission.Columns.Item("cDate").DataBind.Bind("Dt_Cmsn", "cDate");
            mObjMtxCommission.Columns.Item("cInvFol").DataBind.Bind("Dt_Cmsn", "cInvFol");
            mObjMtxCommission.Columns.Item("cOpType").DataBind.Bind("Dt_Cmsn", "cOpType");
            mObjMtxCommission.Columns.Item("cRoute").DataBind.Bind("Dt_Cmsn", "cRoute");
            mObjMtxCommission.Columns.Item("cVcle").DataBind.Bind("Dt_Cmsn", "cVcle");
            mObjMtxCommission.Columns.Item("cPyld").DataBind.Bind("Dt_Cmsn", "cPyld");
            mObjMtxCommission.Columns.Item("cAmnt").DataBind.Bind("Dt_Cmsn", "cAmnt");
            mObjMtxCommission.Columns.Item("cIns").DataBind.Bind("Dt_Cmsn", "cIns");
            mObjMtxCommission.Columns.Item("cCmsn").DataBind.Bind("Dt_Cmsn", "cCmsn");
        }

        private void LoadDtCommissionInfo()
        {
            mDtMatrixCommisssion = this.UIAPIRawForm.DataSources.DataTables.Item("Dt_Cmsn");
            string lStrDriverId = mDtMatrixDrv.GetValue("cDrvrId", mIntSelectedRow - 1).ToString();
            List<CommissionDTO> lLstCommisssionDTO = mObjTransportsFactory.GetCommissionService().GetCommission(lStrDriverId, mDtmFirstDay.ToString("yyyyMMdd"), mDtmLastDay.ToString("yyyyMMdd"));
            int i = 0;

            mObjProgressBar = new ProgressBarManager(UIApplication.GetApplication(), "Consultando comisiones", lLstCommisssionDTO.Count + 1);
            mDtMatrixCommisssion.Rows.Clear();
            foreach (CommissionDTO CommissionDTO in lLstCommisssionDTO)
            {
                mDtMatrixCommisssion.Rows.Add();
                mDtMatrixCommisssion.SetValue("#", i, i + 1);
                mDtMatrixCommisssion.SetValue("cDate", i, CommissionDTO.Date);
                mDtMatrixCommisssion.SetValue("cInvFol", i, CommissionDTO.InvFol);
                mDtMatrixCommisssion.SetValue("cOpType", i, CommissionDTO.OpType);
                mDtMatrixCommisssion.SetValue("cRoute", i, CommissionDTO.Route);
                mDtMatrixCommisssion.SetValue("cVcle", i, CommissionDTO.Vcle);
                mDtMatrixCommisssion.SetValue("cPyld", i, CommissionDTO.PyId);
                mDtMatrixCommisssion.SetValue("cAmnt", i, CommissionDTO.Amnt);
                mDtMatrixCommisssion.SetValue("cIns", i, CommissionDTO.Ins);
                mDtMatrixCommisssion.SetValue("cCmsn", i, CommissionDTO.Cmsn);
                mDtMatrixCommisssion.SetValue("cDocEntry", i, CommissionDTO.DocEntry);
                i++;
                mObjProgressBar.NextPosition();
            }

            mObjMtxCommission.LoadFromDataSource();
            mObjProgressBar.NextPosition();
            mObjProgressBar.Dispose();
        }

        #endregion

        #region Save
        private bool SaveCommission()
        {
            bool lBolSuccess = false;
            Commissions lObjCommission = GetCommissionData();

           //Guardado de encabezado
            if (mObjTransportsFactory.GetCommissionService().AddCommission(lObjCommission) == 0)
            {
                //Guardado de lineas
                mObjProgressBar = new ProgressBarManager(UIApplication.GetApplication(), "Guardando comisiones", lObjCommission.LstCommissionLine.Count);
                string lStrCommissionId = mObjTransportsFactory.GetCommissionService().GetLastCommissionId();
                bool lBolsuccessLine = true;
                foreach (CommissionLine lObjCommisionLine in lObjCommission.LstCommissionLine)
                {
                    lObjCommisionLine.CommisionId = lStrCommissionId;
                    if (mObjTransportsFactory.GetCommissionLineService().AddCommissionLine(lObjCommisionLine) != 0)
                    {
                        lBolsuccessLine = false;
                    }
                    mObjProgressBar.NextPosition();
                }
                mObjProgressBar.NextPosition();
                mObjProgressBar.Dispose();
                lBolSuccess = lBolsuccessLine;
            }

            if (lBolSuccess)
            {
                //Creacion de alerta
                CreateAlertArea(lObjCommission);
            }

            return lBolSuccess;
        }

        private Commissions GetCommissionData()
        {
            List<CommissionLine> lLstComissionList = GetCommissionsLineData();
            Commissions lObjCommission = new Commissions();
            lObjCommission.Amount = lLstComissionList.Sum(x => x.Amount);
            lObjCommission.Folio = lLstComissionList.First().Folio;
            lObjCommission.Status =(int) StatusEnum.CLOSED;
            lObjCommission.LstCommissionLine = lLstComissionList;
            return lObjCommission;
        }

        private List<CommissionLine> GetCommissionsLineData()
        {
            List<CommissionLine> lLstCommissionsLine = new List<CommissionLine>();
            List<CommissionDriverDTO> lLstCommisssionDriverDTO = mObjTransportsFactory.GetCommissionService().GetCommissionDriver(mDtmFirstDay.ToString("yyyyMMdd"), mDtmLastDay.ToString("yyyyMMdd"));

            foreach (CommissionDriverDTO lObjLine in lLstCommisssionDriverDTO)
            {
                 CommissionLine lObjCommisionLine = new CommissionLine();
                 lObjCommisionLine.DriverId = lObjLine.DriverId;
                 lObjCommisionLine.Folio = mStrFolio;
                 lObjCommisionLine.DocEntry = lObjLine.DocEntry;
                 lObjCommisionLine.Amount = lObjLine.FrgAm;
                 lLstCommissionsLine.Add(lObjCommisionLine);
            }
            return lLstCommissionsLine;
        }

      
        #endregion
       
        #region Alerts
        private void CreateAlertArea(Commissions pObjCommission)
        {
            MessageDTO lObjMsgDTO = new MessageDTO();
            if (!pObjCommission.AutTrans)
            {
                List<string> lLstUsers = GetUsers("TR_AU_Area");
                if (lLstUsers.Count > 0)
                {
                    lObjMsgDTO.Message = string.Format("Transportes: Mensaje al area {0}", pObjCommission.Folio); //Get Message
                    lObjMsgDTO.UserCode = lLstUsers;
                    mObjTransportsFactory.GetAlertService().SaveAlert(lObjMsgDTO);
                }
            }
        }

        private void CreateAlertOperaciones(Commissions pObjCommission)
        {
            MessageDTO lObjMsgDTO = new MessageDTO();

            List<string> lLstUsers = GetUsers("TR_AU_OPE");
            if (lLstUsers.Count > 0)
            {
                lObjMsgDTO.Message = string.Format("Transportes: Mensaje a operaciones {0}", pObjCommission.Folio); //Get Message
                lObjMsgDTO.UserCode = lLstUsers;
                mObjTransportsFactory.GetAlertService().SaveAlert(lObjMsgDTO);
            }
        }

        private void CreateAlertFinanzas(Commissions pObjCommission)
        {
            MessageDTO lObjMsgDTO = new MessageDTO();

            List<string> lLstUsers = GetUsers("TR_AU_FZ");
            if (lLstUsers.Count > 0)
            {
                lObjMsgDTO.Message = string.Format("Transportes: Mensaje a Finanzas {0}", pObjCommission.Folio); //Get Message
                lObjMsgDTO.UserCode = lLstUsers;
                mObjTransportsFactory.GetAlertService().SaveAlert(lObjMsgDTO);
            }
        }


        private List<string> GetUsers(string pStrConfigName)
        {
            List<string> lLstUsers = new List<string>();
            lLstUsers = mObjTransportsFactory.GetCommissionService().GetAuthorizers(pStrConfigName);
            if (lLstUsers.Count > 0)
            {
                return lLstUsers;
            }
            else
            {
                LogService.WriteError("No se encontro la configuración " + pStrConfigName);
                UIApplication.ShowMessageBox("No se encontro la configuración " + pStrConfigName);
            }

            return lLstUsers;
        }
        #endregion

        #region Permmission

        /// <summary>
        /// Coloca visible o no visible los contorles de status.
        /// </summary>
        private void VisibleStatusControls(bool pBoolVisible)
        {
            lblAuFz.Item.Visible = pBoolVisible;
            lblAuOp.Item.Visible = pBoolVisible;
            mObjlblArea.Item.Visible = pBoolVisible;
            mObjTxtArea.Item.Visible = pBoolVisible;
            txtAuFz.Item.Visible = pBoolVisible;
            txtAuOp.Item.Visible = pBoolVisible;
        }


        /// <summary>
        /// Coloca visible o no visible los contorles de autorizacion.
        /// </summary>
        private void VisibleAuthControls(bool pBoolVisible)
        {
            mObjBtnAuth.Item.Visible = pBoolVisible;
            mObjBtnReject.Item.Visible = pBoolVisible;
        }


        /// <summary>
        /// Coloca el color del edittect.
        /// </summary>
        private void ColorAuth(EditText pTxtEditText )
        {
            pTxtEditText.BackColor = mIntColorGreen;
            pTxtEditText.ForeColor = mIntColorWhite;
            pTxtEditText.Value = "Autorizado";
        }


        /// <summary>
        /// </summary>
        private void SetStatusControls(Commissions pObjCommissions)
        {
            if (pObjCommissions.AutTrans)
            {
                ColorAuth(mObjTxtArea);
            }
            else
            {
                mObjTxtArea.Value = "Pendiente";
            }

            if (pObjCommissions.AutOperations)
            {

                ColorAuth(txtAuOp);
            }
            else
            {
                txtAuOp.Value = "Pendiente";
            }

            if (pObjCommissions.AutBanks)
            {
                ColorAuth(txtAuFz);
            }
            else
            {
                txtAuFz.Value = "Pendiente";
            }

        }



        /// <summary>
        /// Establece el estatus de la comision
        /// </summary>
        private void SetTxtStatus(StatusEnum pObjStatus)
        {
            mEnumStatus = pObjStatus;
            mObjTxtStatus.Value = mEnumStatus.GetDescription();
        }

        /// <summary>
        /// Obtiene el permisos de autorizacion del usuario
        /// </summary>
        private void GetUserPermissionAuth()
        {
            string lStrUserAuth = mObjTransportsFactory.GetCommissionService().GetUSerAuthorization(DIApplication.Company.UserName);

            switch (lStrUserAuth)
            {
                case "TR_AU_AREA":
                    mAuthorizerEnum = AuthorizerEnum.AutTrans;
                    break;

                case "TR_AU_OPE":
                    mAuthorizerEnum = AuthorizerEnum.AutOperations;
                    break;

                case "TR_AU_FZ":
                    mAuthorizerEnum = AuthorizerEnum.AutBanks;
                    break;

                default:
                    mAuthorizerEnum = AuthorizerEnum.NoAut;
                    break;

            }
        }

        /// <summary>
        /// Establece el permiso de autorizacion del usuario
        /// </summary>
        private void SetControlsByPermissionUser()
        {
            if (mAuthorizerEnum == AuthorizerEnum.NoAut)
            {
                VisibleAuthControls(false);
            }
            else
            {
                VisibleAuthControls(true);
            }
        }
        #endregion


        private Commissions GetCommisionHeader(string pStrFolio)
        {

            IList<Commissions> lObjCommissions = mObjTransportsFactory.GetCommissionService().GetCommissionByFolio(pStrFolio);
            if (lObjCommissions.Count > 0)
            {
                return lObjCommissions.First();
            }

            return null;
        }

        private void LoadFirstDay()
        {
            string lStrFirstDay = mObjTransportsFactory.GetCommissionService().GetFirstDay(DateTime.Now.Year);
            if (string.IsNullOrEmpty(lStrFirstDay))
            {
                UIApplication.ShowMessageBox("Favor de agregar el primer día del año a la tabla TR_Day");
            }
            else
            {
                mIntFirstDay = Convert.ToInt32(lStrFirstDay);
                lblDates.Caption = " Fechas: Del " + mDtmFirstDay.ToString("dd/MM/yyyy") + " Al " + mDtmLastDay.ToString("dd/MM/yyyy");
            }
        }

       private string GetFolioWeek()
        {
            DateTime lDtmNow = DateTime.Now;
            int lIntQtyDays = lDtmNow.DayOfYear;
            int lIntWeek = ((int)(lIntQtyDays - mIntFirstDay) / 7) + 1;
            mDtmFirstDay = new DateTime(DateTime.Now.Year, 1, 1);
            mDtmFirstDay = mDtmFirstDay.AddDays(((lIntWeek - 1) * 7));
            mDtmLastDay = mDtmFirstDay.AddDays(6);

            return lIntWeek.ToString("00") + "-" + DateTime.Now.ToString("yy");
        }



        #endregion

      
        #region Controls
        private SAPbouiCOM.StaticText mObjlblFolio;
        private SAPbouiCOM.EditText mObjTxtFolio;
        private SAPbouiCOM.Button mObjBtnSearch;
        private SAPbouiCOM.EditText mObjTxtStatus;
        private SAPbouiCOM.StaticText mObjlblStat;
        private SAPbouiCOM.EditText mObjTxtArea;
        private SAPbouiCOM.Button mObjBtnAuth;
        private SAPbouiCOM.Button mObjBtnReject;
        private SAPbouiCOM.StaticText mObjlblArea;
        private SAPbouiCOM.Matrix mObjMtxDrv;
        private SAPbouiCOM.Matrix mObjMtxCommission;
        private SAPbouiCOM.Button mObjBtnOk;
        private SAPbouiCOM.StaticText mObjlblCmnt;
        private SAPbouiCOM.EditText mObjTxtCmnt;
        private SAPbouiCOM.Button mObjBtnCancel;
        private SAPbouiCOM.DataTable mDtMatrixDrv = null;
        private SAPbouiCOM.DataTable mDtMatrixCommisssion = null;
        private SAPbouiCOM.StaticText lblDates;
        private EditText txtAuOp;
        private StaticText lblAuOp;
        private StaticText lblAuFz;
        private EditText txtAuFz;
        //private EditText EditText2;
        //private Button Button0;
        private ComboBox cboWeek;
        private StaticText lblWeek;
        private StaticText lblYear;
        private ComboBox cboYear;
        private UGRS.Core.SDK.UI.ProgressBar.ProgressBarManager mObjProgressBar = null;
        #endregion

        private void SendAlertByAuthorizer(Commissions pObjCommissionHeader)
        {
            switch (mAuthorizerEnum)
            {
                case AuthorizerEnum.AutTrans:
                    CreateAlertOperaciones(pObjCommissionHeader);
                    break;
                case AuthorizerEnum.AutOperations:
                    CreateAlertFinanzas(pObjCommissionHeader);
                    break;

                case AuthorizerEnum.AutBanks:
                    break;
            }
        }
      
        private bool UpdateCommission(Commissions pObjCommissionHeader)
        {
            bool lBolSuccess = false;
            bool lBolIsUpdate = true;
            switch (mAuthorizerEnum)
            {
                case AuthorizerEnum.AutTrans:
                    if (!pObjCommissionHeader.AutTrans)
                    {
                        pObjCommissionHeader.AutTrans = true;
                    }
                    else
                    {
                        lBolIsUpdate = false;
                    }
                    break;
                case AuthorizerEnum.AutOperations:

                    if (!pObjCommissionHeader.AutOperations)
                    {
                        pObjCommissionHeader.AutOperations = true;
                    }
                    else
                    {
                        lBolIsUpdate = false;
                    }
                    break;

                case AuthorizerEnum.AutBanks:

                    if (!pObjCommissionHeader.AutBanks)
                    {
                        pObjCommissionHeader.AutBanks = true;
                    }
                    else
                    {
                        lBolIsUpdate = false;
                    }
                    break;
            }

            if (lBolIsUpdate == true)
            {
                if (mObjTransportsFactory.GetCommissionService().UpdateCommission(pObjCommissionHeader) == 0)
                {
                    LogService.WriteSuccess(string.Format("UpdateCommission ok Folio: {0}, Estatus: {1}", pObjCommissionHeader.Folio, mAuthorizerEnum.GetDescription()));
                    lBolSuccess = true;

                }
                else
                {
                    string lStrError = DIApplication.Company.GetLastErrorDescription();
                    LogService.WriteError(string.Format("UpdateCommission: {0}", lStrError));
                    UIApplication.ShowMessageBox(lStrError);
                    lBolSuccess = false;
                }
            }
            else
            {
                LogService.WriteError(string.Format("Ya se encuentra autorizado por el departamento"));
                UIApplication.ShowMessageBox("El folio ya se encuentra autorizado por el departamento");
            }
            return lBolSuccess;
        }
     

       
       
    }
}
