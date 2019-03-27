using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.Core.Extension.Enum;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using SAPbouiCOM;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.Transports.Enums;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.SDK.DI.Transports;
using UGRS.AddOn.Transports.ModalForms;
using UGRS.Core.SDK.DI.Transports.Tables;
using UGRS.Core.Utility;

namespace UGRS.AddOn.Transports.Forms
{
    [FormAttribute("UGRS.AddOn.Transports.Forms.frmFreights", "Forms/frmFreights.b1f")]
    class frmFreights : UserFormBase
    {
        private mfrmRouteFinder mModalFrmRouteFinder = null;
        private mfrmCFL mModalFrmCFL = null;
        TransportServiceFactory mObjTransportsFactory = new TransportServiceFactory();
        List<VehiclesDTO> mLstVehicles;

        private bool mBoolRoadLoaded = false;
        private bool mBoolFormLoaded = false;
        private int mIntUserSignature = 0;
        private int mIntActualRoute = 0;
        private float mFlKm = 0;
        private bool mBoolInsurance;
        private bool mBoolInternal;
        private bool mBoolSearchMode;
        private int mIntFormType;
        private string mStrFolio = string.Empty;
        private string mStrKey = string.Empty;
        private string mStrCardCode = string.Empty;
        private float mFlRetention = 0;


        private InsuranceDTO mObjInsurance;
        private List<SalesOrderLinesDTO> mLstSalesOrderLines = null;

        private CFLParamsDTO mObjCFLParameters = null;
        private SalesOrderLinesDTO mObjSalesOrderLines = null;
        private SalesOrderLinesDTO mObjRoutes = null;

        //Sales Order Form
        SAPbouiCOM.Form mSalesOrderForm;
        SAPbouiCOM.Form mUserFieldsForm;
        SAPbouiCOM.EditText mObjTxtFolio;

        SAPbouiCOM.Matrix mObjSOMtx;
        SAPbouiCOM.ComboBox mObjCboPyloadType;
        SAPbouiCOM.ComboBox mObjCboVehicleType;
        SAPbouiCOM.EditText mObjTxtRoute;
        SAPbouiCOM.EditText mObjTxtTotKm;
        SAPbouiCOM.EditText mObjTxtKmA;
        SAPbouiCOM.EditText mObjTxtKmB;
        SAPbouiCOM.EditText mObjTxtKmC;
        SAPbouiCOM.EditText mObjTxtKmD;
        SAPbouiCOM.EditText mObjTxtKmE;
        SAPbouiCOM.EditText mObjTxtKmF;
        SAPbouiCOM.EditText mObjTxtTotKG;
        SAPbouiCOM.EditText mObjTxtExtra;
        SAPbouiCOM.EditText mObjTxtAnthPyld;
        SAPbouiCOM.EditText mObjTxtDestination;
        SAPbouiCOM.EditText mObjTxtItem;
        SAPbouiCOM.EditText mObjTxtQtty;
        SAPbouiCOM.EditText mObjTxtPrice;
        SAPbouiCOM.ComboBox mObjCboEmployee;
        SAPbouiCOM.EditText mObjTxtoAF;

        SAPbobsCOM.UserTable mObjInternalFreight;

        public SAPbouiCOM.Button pObjBtn;
        public bool pBoolFreightsModal;


        #region Constructor
        //public frmFreights(int pIntFormType, int pIntUserSign = 0, bool pBoolInternal = false)
        //{
        //    mIntUserSignature = pIntUserSign != 0 ? pIntUserSign : DIApplication.Company.UserSignature;
        //    mBoolInternal = pBoolInternal;
        //    mIntFormType = pIntFormType;

        //    if (mBoolInternal)
        //    {
        //        SetInternalConfig();
        //    }
        //    else
        //    {
        //        pBoolFreightsModal = true;
        //    }
        //    InitializeForm();
        //}

        //public frmFreights(int pIntFormType, int pIntUserSignature, SalesOrderLinesDTO pObjSalesOrderLines, bool pBoolInsurance)
        //{
        //    mIntFormType = pIntFormType;
        //    mIntUserSignature = pIntUserSignature;
        //    mObjSalesOrderLines = pObjSalesOrderLines;
        //    mBoolInsurance = pBoolInsurance;
        //    LoadForm();
        //}

        public frmFreights(FreightsParamsDTO pObjParameters)
        {
            mStrCardCode = pObjParameters.CardCode;
            if (pObjParameters.Loaded)
            {
                mIntFormType = pObjParameters.FormType;
                mIntUserSignature = pObjParameters.UserSign;
                mObjSalesOrderLines = pObjParameters.SalesOrderLines;
                mBoolInsurance = pObjParameters.Insurance;
                mBoolFormLoaded = pObjParameters.Loaded;
                LoadForm();
            }
            else
            {
                mIntUserSignature = pObjParameters.UserSign != 0 ? pObjParameters.UserSign : DIApplication.Company.UserSignature;
                mBoolInternal = pObjParameters.Internal;
                mIntFormType = pObjParameters.FormType;

                if (mBoolInternal)
                {
                    SetInternalConfig();
                }
                else
                {
                    pBoolFreightsModal = true;
                }
                InitializeForm();
            }
        }


        #endregion

        #region Methods
        #region Load & Unload Events
        private void LoadEvents()
        {
            UIApplication.GetApplication().ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            UIApplication.GetApplication().MenuEvent += frmFreights_MenuEvent;
            this.lObjCboShared.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.lObjCboShared_ComboSelectAfter);
            this.lObjCboVehicleType.ComboSelectAfter += new _IComboBoxEvents_ComboSelectAfterEventHandler(this.lObjCboVehicleType_ComboSelectAfter);
            this.lObjBtnUpdate.ClickBefore += new _IButtonEvents_ClickBeforeEventHandler(this.lObjBtnUpdate_ClickBefore);
            this.lObjBtnRoute.ClickBefore += new _IButtonEvents_ClickBeforeEventHandler(this.lObjBtnRoute_ClickBefore);
            this.lObjTxtKmA.ValidateBefore += new _IEditTextEvents_ValidateBeforeEventHandler(this.lObjTxt_Validate);
            this.lObjTxtKmB.ValidateBefore += this.lObjTxt_Validate;
            this.lObjTxtKmC.ValidateBefore += this.lObjTxt_Validate;
            this.lObjTxtKmD.ValidateBefore += this.lObjTxt_Validate;
            this.lObjTxtKmE.ValidateBefore += this.lObjTxt_Validate;
            this.lObjTxtKmF.ValidateBefore += this.lObjTxt_Validate;
            this.lObjTxtOrigin.ValidateBefore += this.lObjTxt_Validate;
            this.lObjTxtDestination.ValidateBefore += this.lObjTxt_Validate;
            this.lObjMorign.ValidateBefore += this.lObjTxt_Validate;
            this.lObjMDest.ValidateBefore += this.lObjTxt_Validate;
            this.lObjTxtKmA.LostFocusAfter += new _IEditTextEvents_LostFocusAfterEventHandler(this.lObjTxt_LostFocusAfter);
            this.lObjTxtKmB.LostFocusAfter += this.lObjTxt_LostFocusAfter;
            this.lObjTxtKmC.LostFocusAfter += this.lObjTxt_LostFocusAfter;
            this.lObjTxtKmD.LostFocusAfter += this.lObjTxt_LostFocusAfter;
            this.lObjTxtKmE.LostFocusAfter += this.lObjTxt_LostFocusAfter;
            this.lObjTxtKmF.LostFocusAfter += this.lObjTxt_LostFocusAfter;
            this.lObjTxtExtra.LostFocusAfter += this.lObjTxt_LostFocusAfter;
            this.lObjTxtAmountEns.LostFocusAfter += this.lObjTxt_LostFocusAfter;
            this.lObjChkEnsm.ClickAfter += new _ICheckBoxEvents_ClickAfterEventHandler(this.lObjChkIns_ClickAfter);
            this.lObjBtnAccept.ClickAfter += new _IButtonEvents_ClickAfterEventHandler(this.lObjBtnAccept_ClickAfter);
            this.lObjBtnExit.ClickAfter += lObjBtnExit_ClickAfter;
            this.lObjBtnCancel.ClickAfter += lObjBtnCancel_ClickAfter;

            if (!mBoolInternal)
            {
                this.lObjTxtArticle.LostFocusAfter += lObjTxtArticle_LostFocusAfter;
                this.lObjTxtArticle.KeyDownAfter += lObjTxt_KeyDownAfter;
            }
            else
            {
                this.lObjTxtInternal.KeyDownAfter += lObjTxt_KeyDownAfter;
            }
            this.lObjTxtFolio.KeyDownAfter += lObjTxt_KeyDownAfter;
            this.lObjTxtEcNum.KeyDownAfter += lObjTxt_KeyDownAfter;
            this.lObjMorign.KeyDownAfter += lObjTxt_KeyDownAfter;
            this.lObjMDest.KeyDownAfter += lObjTxt_KeyDownAfter;
            this.lObjTxtDriver.KeyDownAfter += lObjTxt_KeyDownAfter;
            //}

        }

        public void UnloadEvents()
        {
            UIApplication.GetApplication().ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            UIApplication.GetApplication().MenuEvent -= frmFreights_MenuEvent;
            this.lObjCboShared.ComboSelectAfter -= new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.lObjCboShared_ComboSelectAfter);
            this.lObjCboVehicleType.ComboSelectAfter -= new _IComboBoxEvents_ComboSelectAfterEventHandler(this.lObjCboVehicleType_ComboSelectAfter);
            this.lObjBtnUpdate.ClickBefore -= new _IButtonEvents_ClickBeforeEventHandler(this.lObjBtnUpdate_ClickBefore);
            this.lObjBtnRoute.ClickBefore -= new _IButtonEvents_ClickBeforeEventHandler(this.lObjBtnRoute_ClickBefore);
            this.lObjTxtKmA.ValidateBefore -= new _IEditTextEvents_ValidateBeforeEventHandler(this.lObjTxt_Validate);
            this.lObjTxtKmB.ValidateBefore -= this.lObjTxt_Validate;
            this.lObjTxtKmC.ValidateBefore -= this.lObjTxt_Validate;
            this.lObjTxtKmD.ValidateBefore -= this.lObjTxt_Validate;
            this.lObjTxtKmE.ValidateBefore -= this.lObjTxt_Validate;
            this.lObjTxtKmF.ValidateBefore -= this.lObjTxt_Validate;
            this.lObjTxtOrigin.ValidateBefore -= this.lObjTxt_Validate;
            this.lObjTxtDestination.ValidateBefore -= this.lObjTxt_Validate;
            this.lObjMorign.ValidateBefore -= this.lObjTxt_Validate;
            this.lObjMDest.ValidateBefore -= this.lObjTxt_Validate;
            this.lObjTxtKmA.LostFocusAfter -= new _IEditTextEvents_LostFocusAfterEventHandler(this.lObjTxt_LostFocusAfter);
            this.lObjTxtKmB.LostFocusAfter -= this.lObjTxt_LostFocusAfter;
            this.lObjTxtKmC.LostFocusAfter -= this.lObjTxt_LostFocusAfter;
            this.lObjTxtKmD.LostFocusAfter -= this.lObjTxt_LostFocusAfter;
            this.lObjTxtKmE.LostFocusAfter -= this.lObjTxt_LostFocusAfter;
            this.lObjTxtKmF.LostFocusAfter -= this.lObjTxt_LostFocusAfter;
            this.lObjTxtExtra.LostFocusAfter -= this.lObjTxt_LostFocusAfter;
            this.lObjTxtAmountEns.LostFocusAfter -= this.lObjTxt_LostFocusAfter;
            this.lObjChkEnsm.ClickAfter -= new _ICheckBoxEvents_ClickAfterEventHandler(this.lObjChkIns_ClickAfter);
            this.lObjBtnAccept.ClickAfter -= new _IButtonEvents_ClickAfterEventHandler(this.lObjBtnAccept_ClickAfter);
            this.lObjBtnCancel.ClickAfter -= lObjBtnCancel_ClickAfter;
            this.lObjBtnExit.ClickAfter -= lObjBtnExit_ClickAfter;




            if (!mBoolInternal)
            {
                this.lObjTxtArticle.LostFocusAfter -= lObjTxtArticle_LostFocusAfter;
                this.lObjTxtArticle.KeyDownAfter -= lObjTxt_KeyDownAfter;
            }
            else
            {
                this.lObjTxtInternal.KeyDownAfter -= lObjTxt_KeyDownAfter;
            }

            this.lObjTxtFolio.KeyDownAfter -= lObjTxt_KeyDownAfter;
            this.lObjTxtEcNum.KeyDownAfter -= lObjTxt_KeyDownAfter;
            this.lObjMorign.KeyDownAfter -= lObjTxt_KeyDownAfter;
            this.lObjMDest.KeyDownAfter -= lObjTxt_KeyDownAfter;
            this.lObjTxtDriver.KeyDownAfter -= lObjTxt_KeyDownAfter;
            //}
        }

        #endregion
        private void LoadForm()
        {
            InitializeForm();
            LoadLine();
        }

        private void LoadLine()
        {
            this.UIAPIRawForm.Freeze(true);
            //textboxes
            if (mObjSalesOrderLines != null)
            {
                int lIntShrd = (int)(mObjSalesOrderLines.Shared ? SharedEnum.Yes : SharedEnum.NO);


                lObjTxtArticle.Value = mObjSalesOrderLines.ItemCode;
                lObjTxtDesc.Value = mObjSalesOrderLines.Description;
                lObjCboPayload.Select(mObjSalesOrderLines.PayloadType, BoSearchKey.psk_ByValue);
                lObjCboVehicleType.Select(mObjSalesOrderLines.VehicleType, BoSearchKey.psk_ByValue);
                lObjCboShared.Select(lIntShrd.ToString(), BoSearchKey.psk_ByValue);
                lObjTxtDriver.Value = mObjSalesOrderLines.Employee;
                lObjTxtEcNum.Value = mObjSalesOrderLines.Asset;
                lObjTxtExtra.Value = mObjSalesOrderLines.Extra;
                lObjTxtFolio.Value = mObjSalesOrderLines.Folio;
                mObjRoutes = RoutesToDTO(mObjTransportsFactory.GetRouteService().GetRoute((long)mObjSalesOrderLines.Route));
                SetRoutesTextsBoxes();
            }
            lObjChkEnsm.Checked = mBoolInsurance ? true : false;
            this.UIAPIRawForm.Freeze(false);
        }

        private void LoadInternalFreight(string pStrFolio)
        {
            this.UIAPIRawForm.Freeze(true);
            try
            {
                if (!string.IsNullOrEmpty(pStrFolio))
                {
                    //mObjInternalFreight = mObjTransportsFactory.GetRouteService().GetFreight();

                    mObjInternalFreight = (SAPbobsCOM.UserTable)DIApplication.Company.UserTables.Item("UG_TR_INTLFRGHT");
                    if (mObjInternalFreight != null)
                    {
                        mObjInternalFreight.GetByKey(GetKeyByFolio(pStrFolio));
                    }
                }



                lObjTxtInternal.Value = mObjInternalFreight != null ? mObjInternalFreight.UserFields.Fields.Item("U_InternalFolio").Value.ToString() : mObjTransportsFactory.GetRouteService().GetFolio(pStrFolio);
                lObjTxtFolio.Value = mObjInternalFreight != null ? mObjInternalFreight.UserFields.Fields.Item("U_Ticket").Value.ToString() : string.Empty;
                lObjTxtEcNum.Value = mObjInternalFreight != null ? mObjInternalFreight.UserFields.Fields.Item("U_Asset").Value.ToString() : string.Empty;
                mObjRoutes = mObjInternalFreight != null ? RoutesToDTO(mObjTransportsFactory.GetRouteService().GetRoute(Convert.ToInt64(mObjInternalFreight.UserFields.Fields.Item("U_Route").Value.ToString()))) : null;
                SetRoutesTextsBoxes();
                lObjTxtDriver.Value = mObjInternalFreight != null ? mObjInternalFreight.UserFields.Fields.Item("U_Driver").Value.ToString() : string.Empty;
                lObjTxtExtra.Value = mObjInternalFreight != null ? mObjInternalFreight.UserFields.Fields.Item("U_Extra").Value.ToString() : string.Empty;
                lObjTxtComment.Value = mObjInternalFreight != null ? mObjInternalFreight.UserFields.Fields.Item("U_Comments").Value.ToString() : string.Empty;

                string lStrShared = mObjInternalFreight != null ? mObjInternalFreight.UserFields.Fields.Item("U_Shared").Value.ToString() : "";
                string lStrPayload = mObjInternalFreight != null ? mObjInternalFreight.UserFields.Fields.Item("U_PayloadType").Value.ToString() : "";
                string lStrCostingCode = mObjInternalFreight != null ? mObjInternalFreight.UserFields.Fields.Item("U_Area").Value.ToString() : "";
                string lStrVehicleType = mObjInternalFreight != null ? mObjInternalFreight.UserFields.Fields.Item("U_VehicleType").Value.ToString() : "";
                string lStrInsurance = mObjInternalFreight != null ? mObjInternalFreight.UserFields.Fields.Item("U_Insurance").Value.ToString() : "N";

                lObjCboShared.Select(lStrShared, SAPbouiCOM.BoSearchKey.psk_ByValue);
                lObjCboVehicleType.Select(lStrVehicleType, SAPbouiCOM.BoSearchKey.psk_ByValue);
                lObjCboPayload.Select(lStrPayload, SAPbouiCOM.BoSearchKey.psk_ByValue);
                lObjCboArea.Select(lStrCostingCode, SAPbouiCOM.BoSearchKey.psk_ByValue);
                lObjChkEnsm.Checked = lStrInsurance.Equals("Y") ? true : false;
                lObjTxtEcNum.Value = mObjInternalFreight != null ? mObjInternalFreight.UserFields.Fields.Item("U_Asset").Value.ToString() : string.Empty;

                SetInitialInternalConfig(lObjTxtInternal.Value);

                this.UIAPIRawForm.Freeze(false);
            }
            catch (Exception lObjException)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(lObjException.Message
                                    , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(mObjInternalFreight);
                mObjInternalFreight = null;
            }
        }

        private string GetKeyByFolio(string pStrFolio)
        {
            mStrKey = mObjTransportsFactory.GetRouteService().GetKeyByFolio(pStrFolio);

            return !string.IsNullOrEmpty(mStrKey) ? mStrKey : "";
        }

        private SalesOrderLinesDTO RoutesToDTO(Routes routes)
        {

            if (routes == null) return null;

            return new SalesOrderLinesDTO()
            {
                RouteName = routes.RowName.ToString(),
                KmA = routes.TypeA.ToString(),
                KmB = routes.TypeB.ToString(),
                KmC = routes.TypeC.ToString(),
                KmD = routes.TypeD.ToString(),
                KmE = routes.TypeE.ToString(),
                KmF = routes.TypeF.ToString(),
                Origin = routes.Origen,
                MOrigin = routes.TR_TOWNORIG,
                Destination = routes.Destino,
                MDestination = routes.TR_TOWNDES
            };
        }

        private void SetInternalConfig()
        {
            lObjlblArticle.Caption = "Folio interno";
            lObjTxtInternal.Item.Visible = true;
            lObjCboShared.Item.Click();
            lObjTxtArticle.Item.Visible = false;


            lObjlblDescription.Caption = "Estatus";
            lObjTxtDesc.Item.Visible = false;
            lObjTxtStatus.Item.Visible = true;

            lObjCboArea.Item.Visible = true;
            lObjlblArea.Item.Visible = true;
            lObjTxtComment.Item.Visible = true;
            lObjlblComment.Item.Visible = true;

            lObjBtnCancel.Item.Visible = true;

            this.UIAPIRawForm.EnableMenu("520", true); // Print
            this.UIAPIRawForm.EnableMenu("6659", false);  // Fax
            this.UIAPIRawForm.EnableMenu("1281", true); // Search Record
            this.UIAPIRawForm.EnableMenu("1282", true); // Add New Record
            this.UIAPIRawForm.EnableMenu("1288", true);  // Next Record
            this.UIAPIRawForm.EnableMenu("1289", true);  // Pevious Record
            this.UIAPIRawForm.EnableMenu("1290", true);  // First Record
            this.UIAPIRawForm.EnableMenu("1291", true);  // Last record

            SetInitialInternalConfig();
        }

        private void SetInitialInternalConfig(string pStrFolio = "")
        {
            if (string.IsNullOrEmpty(pStrFolio))
            {
                lObjTxtInternal.Value = mObjTransportsFactory.GetRouteService().GetFolio(pStrFolio);
            }

            lObjTxtStatus.Value = mObjTransportsFactory.GetRouteService().GetStatus(lObjTxtInternal.Value);

            if (lObjTxtStatus.Value.Equals(StatusEnum.CLOSED.GetDescription()) || lObjTxtStatus.Value.Equals(StatusEnum.CANCELED.GetDescription()))
            {
                this.UIAPIRawForm.Mode = BoFormMode.fm_VIEW_MODE;
            }
            else
            {
                CreationMode();
            }

            lObjBtnCancel.Item.Enabled = lObjTxtStatus.Value.Equals("Cerrado") ? true : false;
            lObjBtnExit.Item.Enabled = true;
        }

        private void CreationMode()
        {
            lObjCboShared.Item.Enabled = true;
            lObjTxtFolio.Item.Enabled = true;
            lObjTxtFolio.Item.Click();
            lObjCboPayload.Item.Enabled = true;
            lObjCboVehicleType.Item.Enabled = true;
            lObjTxtEcNum.Item.Enabled = true;
            lObjTxtOrigin.Item.Enabled = true;
            lObjTxtDestination.Item.Enabled = true;
            lObjMorign.Item.Enabled = true;
            lObjMDest.Item.Enabled = true;
            lObjTxtKmA.Item.Enabled = true;
            lObjTxtKmB.Item.Enabled = true;
            lObjTxtKmC.Item.Enabled = true;
            lObjTxtKmD.Item.Enabled = true;
            lObjTxtKmE.Item.Enabled = true;
            lObjTxtKmF.Item.Enabled = true;
            lObjTxtDriver.Item.Enabled = true;
            lObjTxtExtra.Item.Enabled = true;
            lObjChkEnsm.Item.Enabled = true;
            lObjBtnRoute.Item.Enabled = true;

            if (mBoolInternal)
            {
                lObjTxtInternal.Item.Enabled = false;
                lObjCboArea.Item.Enabled = true;
                lObjTxtComment.Item.Enabled = true;
            }

        }

        private void InitializeForm()
        {
            this.UIAPIRawForm.Freeze(true);
            LoadEvents();
            LoadCombos();
            mObjInsurance = mObjTransportsFactory.GetRouteService().GetInsuranceLine(mIntUserSignature);
            this.UIAPIRawForm.Freeze(false);
        }

        private void LoadCombos()
        {
            try
            {
                ///Combo Compartido
                List<SharedEnum> lLstSharedEnum = Enum.GetValues(typeof(SharedEnum)).Cast<SharedEnum>().ToList();
                lObjCboShared.ValidValues.Add("", "");
                foreach (SharedEnum lObjSharedEnum in lLstSharedEnum)
                {
                    lObjCboShared.ValidValues.Add(((int)lObjSharedEnum).ToString(), lObjSharedEnum.GetDescription());
                }
                //Combo Vehiculos
                mLstVehicles = mObjTransportsFactory.GetVehiclesService().GetVehiclesTypeList();
                lObjCboVehicleType.ValidValues.Add("", "");
                foreach (var item in mLstVehicles)
                {
                    lObjCboVehicleType.ValidValues.Add(item.EquipType, item.Name);
                }
                //Combo Tipo de carga
                List<PayLoadTypeDTO> lLstPayloadTypes = mObjTransportsFactory.GetVehiclesService().GetPayloadTypeList();
                lObjCboPayload.ValidValues.Add("", "");
                foreach (var item in lLstPayloadTypes)
                {
                    lObjCboPayload.ValidValues.Add(item.Code, item.Name);
                }

                if (mBoolInternal)
                {
                    List<CostingCodesDTO> lLstCostingCodes = mObjTransportsFactory.GetRouteService().GetCostingCodes();

                    lObjCboArea.ValidValues.Add("", "");
                    foreach (var item in lLstCostingCodes)
                    {
                        lObjCboArea.ValidValues.Add(item.Code, item.Name);
                    }
                }

                lObjCboVehicleType.Item.DisplayDesc = true;
                lObjCboShared.Item.DisplayDesc = true;
                lObjCboPayload.Item.DisplayDesc = true;
            }
            catch (Exception ex)
            {
                LogService.WriteError("(LoadCombobox): " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        private void SetRoutesTextsBoxes()
        {
            this.UIAPIRawForm.Freeze(true);
            if (mObjRoutes != null)
            {
                lObjTxtOrigin.Value = mObjRoutes.Origin;

                lObjMorign.Value = mObjRoutes.MOrigin;

                lObjMDest.Value = mObjRoutes.MDestination;

                lObjTxtDestination.Value = mObjRoutes.Destination;

                lObjTxtKmA.Value = mObjRoutes.KmA;
                lObjTxtKmB.Value = mObjRoutes.KmB;
                lObjTxtKmC.Value = mObjRoutes.KmC;
                lObjTxtKmD.Value = mObjRoutes.KmD;
                lObjTxtKmE.Value = mObjRoutes.KmE;
                lObjTxtKmF.Value = mObjRoutes.KmF;
                LoadConfig();
            }
            else
            {
                lObjTxtOrigin.Value = string.Empty;

                lObjMorign.Value = string.Empty;

                lObjMDest.Value = string.Empty;

                lObjTxtDestination.Value = string.Empty;

                lObjTxtKmA.Value = string.Empty;
                lObjTxtKmB.Value = string.Empty;
                lObjTxtKmC.Value = string.Empty;
                lObjTxtKmD.Value = string.Empty;
                lObjTxtKmE.Value = string.Empty;
                lObjTxtKmF.Value = string.Empty;
            }
            this.UIAPIRawForm.Freeze(false);
        }

        private void LoadConfig()
        {
            lObjBtnUpdate.Caption = "Actualizar";
            lObjBtnUpdate.Item.Enabled = false;
        }

        private void SetTextBox()
        {
            this.UIAPIRawForm.Freeze(true);
            switch (mModalFrmCFL.mStrCFLType)
            {
                case "CFL_TownsA":
                    lObjMorign.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cName")
                        .Cells.Item(mModalFrmCFL.pIntRow).Specific).Value.ToString();
                    break;
                case "CFL_TownsB":
                    lObjMDest.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cName")
                        .Cells.Item(mModalFrmCFL.pIntRow).Specific).Value.ToString();
                    break;
                case "CFL_Items":
                    lObjTxtArticle.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cName")
                        .Cells.Item(mModalFrmCFL.pIntRow).Specific).Value.ToString();
                    lObjTxtDesc.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cDesc")
                        .Cells.Item(mModalFrmCFL.pIntRow).Specific).Value.ToString();
                    break;
                case "CFL_AF":
                    lObjTxtEcNum.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cItem")
                        .Cells.Item(mModalFrmCFL.pIntRow).Specific).Value.ToString();
                    break;
                case "CFL_DR":
                    lObjTxtDriver.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cName")
                        .Cells.Item(mModalFrmCFL.pIntRow).Specific).Value.ToString();
                    break;
                case "CFL_Folios":
                    lObjTxtFolio.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cFolio")
                        .Cells.Item(mModalFrmCFL.pIntRow).Specific).Value.ToString();
                    break;
            }
            this.UIAPIRawForm.Freeze(false);
        }

        private void OpenCFLModal(string pStrModal, string pStrCFL, string pStrEquip)
        {
            SetModalParameters(pStrModal, pStrCFL, pStrEquip);
            //mModalFrmCFL = new ModalForms.mfrmCFL(pStrModal, pStrCFL, mIntUserSignature, pStrEquip);
            mModalFrmCFL = new ModalForms.mfrmCFL(mObjCFLParameters);
            pObjBtn = mModalFrmCFL.pObjBtnSelect;
        }

        private void SetModalParameters(string pStrModal, string pStrCFL, string pStrEquip)
        {
            mObjCFLParameters = new CFLParamsDTO();

            mObjCFLParameters.FormName = pStrModal;
            mObjCFLParameters.CFLType = pStrCFL;
            mObjCFLParameters.UserSign = mIntUserSignature;
            mObjCFLParameters.Equip = pStrEquip;
            mObjCFLParameters.CardCode = mStrCardCode;

        }

        private void ShowMfRouteFinder()
        {
            mModalFrmRouteFinder = new ModalForms.mfrmRouteFinder("frmMRouteFinder");
            pObjBtn = mModalFrmRouteFinder.pObjBtnSelect;
        }

        private void LoadVehiclePrices(bool pBoolValid)
        {
            VehiclesDTO lObjVehicle = mLstVehicles.Select(x => x).Where(x => x.EquipType == lObjCboVehicleType.Value).FirstOrDefault();

            lObjTxtPriceA.Value = pBoolValid ? lObjVehicle.PathA : string.Empty;
            lObjTxtPriceB.Value = pBoolValid ? lObjVehicle.PathB : string.Empty;
            lObjTxtPriceC.Value = pBoolValid ? lObjVehicle.PathC : string.Empty;
            lObjTxtPriceD.Value = pBoolValid ? lObjVehicle.PathD : string.Empty;
            lObjTxtPriceE.Value = pBoolValid ? lObjVehicle.PathE : string.Empty;
            lObjTxtPriceF.Value = pBoolValid ? lObjVehicle.PathF : string.Empty;
        }

        private void SetKmAmounts()
        {
            this.UIAPIRawForm.Freeze(true);
            SetKilometers();
            SetKilometerAmount();
            this.UIAPIRawForm.Freeze(false);
        }

        private void SetCashAmounts()
        {
            this.UIAPIRawForm.Freeze(true);
            if (float.Parse(lObjTxtAmountEns.Value) != 0)
            {
                LoadInsuranceAmount(true);
            }
            SetRetentions();
            SetTotalAmount();
            this.UIAPIRawForm.Freeze(false);
        }

        private void SetKilometers()
        {
            mFlKm = float.Parse(lObjTxtKmA.Value) + float.Parse(lObjTxtKmB.Value) + float.Parse(lObjTxtKmC.Value) + float.Parse(lObjTxtKmD.Value) + float.Parse(lObjTxtKmE.Value) + float.Parse(lObjTxtKmF.Value);
            lObjTxtTotKm.Value = mFlKm.ToString();
        }

        private void SetKilometerAmount()
        {
            float lFlA = float.Parse(lObjTxtKmA.Value) * float.Parse(lObjTxtPriceA.Value);
            float lFlB = float.Parse(lObjTxtKmB.Value) * float.Parse(lObjTxtPriceB.Value);
            float lFlC = float.Parse(lObjTxtKmC.Value) * float.Parse(lObjTxtPriceC.Value);
            float lFlD = float.Parse(lObjTxtKmD.Value) * float.Parse(lObjTxtPriceD.Value);
            float lFlE = float.Parse(lObjTxtKmE.Value) * float.Parse(lObjTxtPriceE.Value);
            float lFlF = float.Parse(lObjTxtKmF.Value) * float.Parse(lObjTxtPriceF.Value);

            lObjTxtAmountKm.Value = (lFlA + lFlB + lFlC + lFlD + lFlE + lFlF).ToString();
        }

        private void SetRetentions()
        {
            mObjTxtRetentions.Value = ((float.Parse(lObjTxtAmountKm.Value) + float.Parse(lObjTxtExtra.Value)) * (mFlRetention != 0 ? mFlRetention / 100 : 0)).ToString();
        }

        private void SetTotalAmount()
        {
            lObjTxtTotalFreight.Value = (float.Parse(lObjTxtAmountKm.Value)
                + float.Parse(lObjTxtExtra.Value) + float.Parse(lObjTxtAmountEns.Value)).ToString();
        }

        private void SharedConfig(bool pBoolShared)
        {
            lObjTxtAmountEns.Item.Enabled = pBoolShared;
            lObjTxtTotalFreight.Item.Enabled = pBoolShared;
        }

        private void LoadInsuranceAmount(bool pBoolCheck)
        {
            this.UIAPIRawForm.Freeze(true);
            lObjTxtAmountEns.Value = pBoolCheck ? (mObjInsurance.Price * mFlKm).ToString() : "0";
            //SetCashAmounts();
            this.UIAPIRawForm.Freeze(false);
        }

        private void NewRoute()
        {
            if (ValidateRouteFields())
            {
                int lIntResult = mObjTransportsFactory.GetRouteService().AddRoute(GetRouteObject());
                if (lIntResult == 0)
                {

                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Ruta añadida correctamente"
                    , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                    LoadConfig();
                }
                else
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Error: " + DIApplication.Company.GetLastErrorDescription()
                    , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    LogService.WriteError(DIApplication.Company.GetLastErrorDescription());
                }
            }
        }

        private void UpdateRoute()
        {
            if (ValidateRouteFields())
            {
                int lIntResult = mObjTransportsFactory.GetRouteService().UpdateRoute(GetRouteObject());
                if (lIntResult == 0)
                {

                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Ruta actualizada correctamente"
                    , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                    LoadConfig();
                }
                else
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Error: " + DIApplication.Company.GetLastErrorDescription()
                    , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    LogService.WriteError(DIApplication.Company.GetLastErrorDescription());
                }
            }
        }

        private void GetSalesOrderLines()
        {
            mLstSalesOrderLines = new List<SalesOrderLinesDTO>();

            mLstSalesOrderLines.Add(new SalesOrderLinesDTO()
            {
                ItemCode = lObjTxtArticle.Value,
                Quantity = 1,
                UnitPrice = (float.Parse(lObjTxtTotalFreight.Value)-float.Parse(lObjTxtAmountEns.Value)),
                PayloadType = lObjCboPayload.Value,
                VehicleType = lObjCboVehicleType.Value,
                Route = mIntActualRoute,
                TotKm = lObjTxtTotKm.Value,
                Folio = lObjTxtFolio.Value,
                KmA = lObjTxtKmA.Value,
                KmB = lObjTxtKmB.Value,
                KmC = lObjTxtKmC.Value,
                KmD = lObjTxtKmD.Value,
                KmE = lObjTxtKmE.Value,
                KmF = lObjTxtKmF.Value,
                Employee = lObjTxtDriver.Value,
                Asset = lObjTxtEcNum.Value,
                Extra = lObjTxtExtra.Value,
                Destination = lObjTxtDestination.Value,
                AnotherPyld = "",
                TotKg = ""
            });

            if (lObjChkEnsm.Checked)
            {
                mLstSalesOrderLines.Add(new SalesOrderLinesDTO()
                {
                    ItemCode = mObjInsurance.ItemCode,
                    Quantity = 1,
                    UnitPrice = float.Parse(lObjTxtAmountEns.Value)
                });
            }

        }

        private bool SetSalesOrder()
        {
            try
            {
                if (SetSalesOrderControls())
                {
                    FillSalesOrder();
                }
                return true;
            }
            catch (Exception ex)
            {
               
                LogService.WriteError("(OpenFormBP): " + ex.Message);
                LogService.WriteError(ex);
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
                return false;
            }
            finally
            {
                if (mSalesOrderForm != null)
                {
                    mSalesOrderForm.Freeze(false);
                }
            }
        }

        private bool SetSalesOrderControls()
        {
            try
            {
                mSalesOrderForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetFormByTypeAndCount(mIntFormType, -1);
                mUserFieldsForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetFormByTypeAndCount(-mIntFormType, -1);

                mObjTxtFolio = (SAPbouiCOM.EditText)mUserFieldsForm.Items.Item("U_GLO_Ticket").Specific;

                mObjSOMtx = (SAPbouiCOM.Matrix)mSalesOrderForm.Items.Item("38").Specific;
                return true;
            }
            catch (Exception lObjException)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Favor de abrir campos definidos de usuario");
                LogService.WriteError("(OpenFormBP): " + lObjException.Message);
                LogService.WriteError(lObjException.Message + "-" + lObjException.InnerException.Message);
                return false;
            }
        }

        private void FillSalesOrder()
        {
            mSalesOrderForm.Freeze(true);

            for (int i = 1; i <= mLstSalesOrderLines.Count; i++)
            {
                mObjTxtItem = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("1").Cells.Item(i).Specific;
                mObjTxtItem.Value = mLstSalesOrderLines[i - 1].ItemCode;

                mObjCboPyloadType = (SAPbouiCOM.ComboBox)mObjSOMtx.Columns.Item("U_TR_LoadType").Cells.Item(i).Specific;
                mObjCboPyloadType.Select(mLstSalesOrderLines[i - 1].PayloadType, SAPbouiCOM.BoSearchKey.psk_ByValue);

                mObjCboVehicleType = (SAPbouiCOM.ComboBox)mObjSOMtx.Columns.Item("U_TR_VehicleType").Cells.Item(i).Specific;
                mObjCboVehicleType.Select(mLstSalesOrderLines[i - 1].VehicleType, SAPbouiCOM.BoSearchKey.psk_ByValue);

                mObjTxtRoute = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("U_TR_Paths").Cells.Item(i).Specific;
                mObjTxtRoute.Value = mLstSalesOrderLines[i - 1].Route.ToString();

                mObjTxtTotKm = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("U_TR_TotKm").Cells.Item(i).Specific;
                mObjTxtTotKm.Value = mLstSalesOrderLines[i - 1].TotKm;

                mObjTxtKmA = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("U_TR_TypeA").Cells.Item(i).Specific;
                mObjTxtKmA.Value = mLstSalesOrderLines[i - 1].KmA;

                mObjTxtKmB = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("U_TR_TypeB").Cells.Item(i).Specific;
                mObjTxtKmB.Value = mLstSalesOrderLines[i - 1].KmB;

                mObjTxtKmC = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("U_TR_TypeC").Cells.Item(i).Specific;
                mObjTxtKmC.Value = mLstSalesOrderLines[i - 1].KmC;

                mObjTxtKmD = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("U_TR_TypeD").Cells.Item(i).Specific;
                mObjTxtKmD.Value = mLstSalesOrderLines[i - 1].KmD;

                mObjTxtKmE = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("U_TR_TypeE").Cells.Item(i).Specific;
                mObjTxtKmE.Value = mLstSalesOrderLines[i - 1].KmE;

                mObjTxtKmF = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("U_TR_TypeF").Cells.Item(i).Specific;
                mObjTxtKmF.Value = mLstSalesOrderLines[i - 1].KmF;

                mObjTxtTotKG = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("U_TR_TotKilos").Cells.Item(i).Specific;
                mObjTxtTotKG.Value = mLstSalesOrderLines[i - 1].TotKg;

                mObjTxtExtra = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("U_TR_AdditionalExpen").Cells.Item(i).Specific;
                mObjTxtExtra.Value = mLstSalesOrderLines[i - 1].Extra;

                mObjTxtAnthPyld = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("U_TR_OtherLoad").Cells.Item(i).Specific;
                mObjTxtAnthPyld.Value = mLstSalesOrderLines[i - 1].AnotherPyld;

                mObjTxtDestination = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("U_TR_Destination").Cells.Item(i).Specific;
                mObjTxtDestination.Value = mLstSalesOrderLines[i - 1].Destination;

                if (!string.IsNullOrEmpty(mLstSalesOrderLines[i - 1].Employee))
                {
                    mObjCboEmployee = (SAPbouiCOM.ComboBox)mObjSOMtx.Columns.Item("27").Cells.Item(i).Specific;
                    mObjCboEmployee.Select(mLstSalesOrderLines[i - 1].Employee, SAPbouiCOM.BoSearchKey.psk_Index);
                }

                mObjTxtoAF = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("2003").Cells.Item(i).Specific;
                mObjTxtoAF.Value = mLstSalesOrderLines[i - 1].Asset;

                mObjTxtQtty = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("11").Cells.Item(i).Specific;
                mObjTxtQtty.Value = mLstSalesOrderLines[i - 1].Quantity.ToString();

                mObjTxtPrice = (SAPbouiCOM.EditText)mObjSOMtx.Columns.Item("14").Cells.Item(i).Specific;
                mObjTxtPrice.Value = mLstSalesOrderLines[i - 1].UnitPrice.ToString();
            }

            if (mObjTxtFolio != null)
            {
                mObjTxtFolio.Value = mLstSalesOrderLines.Select(x => x.Folio).FirstOrDefault();
            }

            if (mBoolFormLoaded && mObjSOMtx.RowCount > 2)
            {
                if (!lObjChkEnsm.Checked)
                {
                    mObjSOMtx.DeleteRow(2);
                }
            }

            mSalesOrderForm.Freeze(false);
        }

        private Routes GetRouteObject()
        {
            Routes lObjRoute = new Routes();

            lObjRoute.RowCode = mIntActualRoute.ToString();
            lObjRoute.Activo = "S";
            lObjRoute.TypeA = float.Parse(lObjTxtKmA.Value);
            lObjRoute.TypeB = float.Parse(lObjTxtKmB.Value);
            lObjRoute.TypeC = float.Parse(lObjTxtKmC.Value);
            lObjRoute.TypeD = float.Parse(lObjTxtKmD.Value);
            lObjRoute.TypeE = float.Parse(lObjTxtKmE.Value);
            lObjRoute.TypeF = float.Parse(lObjTxtKmF.Value);
            lObjRoute.Origen = lObjTxtOrigin.Value;
            lObjRoute.Destino = lObjTxtDestination.Value;
            lObjRoute.TR_TOWNORIG = lObjMorign.Value.ToUpper();
            lObjRoute.TR_TOWNDES = lObjMDest.Value.ToUpper();
            lObjRoute.CasetaTC = 0;
            lObjRoute.CasetaRB = 0;
            lObjRoute.RowName = GetRouteName();

            return lObjRoute;

        }

        private string GetRouteName()
        {
            string lStrOrgn = lObjTxtOrigin.Value.Length >= 4 ? lObjTxtOrigin.Value.Substring(0, 4) : lObjTxtOrigin.Value.Substring(0, lObjTxtOrigin.Value.Length);
            string lStrDest = lObjTxtDestination.Value.Length >= 4 ? lObjTxtDestination.Value.Substring(0, 4) : lObjTxtDestination.Value.Substring(0, lObjTxtDestination.Value.Length);
            int lIntNextId = mObjTransportsFactory.GetRouteService().GetNextRouteId();

            return string.Format("{0}-{1}{2}", lStrOrgn, lStrDest, lIntNextId);
        }

        public void CloseForm()
        {
            UnloadEvents();
            this.UIAPIRawForm.Close();
        }

        public void SelectModal(string pStrItemUID)
        {
            switch (pStrItemUID)
            {
                case "txtMOrig":
                    OpenCFLModal("frmMCFL", "CFL_TownsA", "");
                    break;
                case "txtMDest":
                    OpenCFLModal("frmMCFL", "CFL_TownsB", "");
                    break;
                case "txtArt":
                    OpenCFLModal("frmMCFL", "CFL_Items", "");
                    break;
                case "txtEcNum":
                    if (!string.IsNullOrEmpty(lObjCboVehicleType.Value))
                    {
                        OpenCFLModal("frmMCFL", "CFL_AF", lObjCboVehicleType.Value);
                    }
                    break;
                case "txtDriv":
                    OpenCFLModal("frmMCFL", "CFL_DR", "");
                    break;
                case "txtFol":
                    if (!string.IsNullOrEmpty(lObjCboShared.Value) && (SharedEnum)Convert.ToInt32(lObjCboShared.Value) == SharedEnum.Yes)
                    {
                        OpenCFLModal("frmMCFL", "CFL_Folios", "");
                    }
                    break;
            }
        }

        public void SetRoutes()
        {
            mObjRoutes = mModalFrmRouteFinder.pRoutes;
            SetRoutesTextsBoxes();
            mModalFrmRouteFinder.CloseForm();
            mBoolRoadLoaded = true;
            mIntActualRoute = mObjRoutes != null ? mObjRoutes.Route : 0;
            lObjTxtDriver.Item.Click();
        }

        private void SaveDetails()
        {
            try
            {

                SAPbobsCOM.UserTable lObjInternalFreight = (SAPbobsCOM.UserTable)DIApplication.Company.UserTables.Item("UG_TR_INTLFRGHT");

                lObjInternalFreight.UserFields.Fields.Item("U_InternalFolio").Value = lObjTxtInternal.Value;
                lObjInternalFreight.UserFields.Fields.Item("U_Shared").Value = lObjCboShared.Value;
                lObjInternalFreight.UserFields.Fields.Item("U_Insurance").Value = lObjChkEnsm.Checked ? "Y" : "N";
                lObjInternalFreight.UserFields.Fields.Item("U_Status").Value = StatusEnum.CLOSED.GetDescription();
                lObjInternalFreight.UserFields.Fields.Item("U_Route").Value = mObjRoutes.Route;
                lObjInternalFreight.UserFields.Fields.Item("U_VehicleType").Value = lObjCboVehicleType.Value;
                lObjInternalFreight.UserFields.Fields.Item("U_Ticket").Value = lObjTxtFolio.Value;
                lObjInternalFreight.UserFields.Fields.Item("U_Comments").Value = string.Format("FI {0}: {1} ", mObjRoutes.RouteName, lObjTxtComment.Value);
                lObjInternalFreight.UserFields.Fields.Item("U_Area").Value = lObjCboArea.Value;
                lObjInternalFreight.UserFields.Fields.Item("U_Extra").Value = lObjTxtExtra.Value;
                lObjInternalFreight.UserFields.Fields.Item("U_Driver").Value = lObjTxtDriver.Value;
                lObjInternalFreight.UserFields.Fields.Item("U_PayloadType").Value = lObjCboPayload.Value;
                lObjInternalFreight.UserFields.Fields.Item("U_Asset").Value = lObjTxtEcNum.Value;

                lObjInternalFreight.Add();

            }
            catch (Exception lObjException)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Error: " + lObjException.Message
                    , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                LogService.WriteError(lObjException.Message);
            }


        }

        private bool CreateJournal()
        {
            return mObjTransportsFactory.GetJournalService().CreateNewJournal(lObjTxtInternal.Value, GetJournalLines());
        }


        private bool ReverseJournal()
        {
            return mObjTransportsFactory.GetJournalService().ReverseJournal(lObjTxtInternal.Value);
        }

        private List<JournalLineDTO> GetJournalLines()
        {
            List<JournalLineDTO> lLstJournalLines = new List<JournalLineDTO>();

            string lStrCreditAccount = mObjTransportsFactory.GetJournalService().GetCreditAccount();
            string lStrDebitAccount = mObjTransportsFactory.GetJournalService().GetDebitAccount();

            lLstJournalLines.Add(new JournalLineDTO()
            {
                AccountCode = lStrCreditAccount,
                ContraAccount = lStrDebitAccount,
                CostingCode = mObjTransportsFactory.GetRouteService().GetCostingCode(mIntUserSignature),
                Credit = double.Parse(lObjTxtTotalFreight.Value),
                Debit = 0,
            });

            lLstJournalLines.Add(new JournalLineDTO()
            {
                AccountCode = lStrDebitAccount,
                ContraAccount = lStrCreditAccount,
                CostingCode = lObjCboArea.Value,
                Credit = 0,
                Debit = double.Parse(lObjTxtTotalFreight.Value)
            });

            return lLstJournalLines;
        }

        public void SetCFLTextBoxes()
        {
            SetTextBox();
            mModalFrmCFL.CloseForm();
        }

        private int GetFirstFolio()
        {
            return mObjTransportsFactory.GetRouteService().GetFirstFolio();
        }

        private int GetLastFolio()
        {
            return mObjTransportsFactory.GetRouteService().GetLastFolio();
        }

        private void SearchMode()
        {
            this.UIAPIRawForm.Mode = BoFormMode.fm_VIEW_MODE;
            lObjTxtInternal.Item.Enabled = true;
            lObjTxtInternal.Value = "";
            mBoolSearchMode = true;
        }

        private void SearchFolio()
        {
            if (ValidateFolio())
            {
                LoadInternalFreight(lObjTxtInternal.Value);
                mBoolSearchMode = false;
            }
        }

        private bool ValidateFolio()
        {
            IList<string> lLstmssingFields = new List<string>();

            if (string.IsNullOrEmpty(lObjTxtInternal.Value))
            {
                lLstmssingFields.Add("Folio interno");
            }

            if (!mObjTransportsFactory.GetRouteService().FolioExists(lObjTxtInternal.Value))
            {
                lLstmssingFields.Add("El Folio ingresado no existe");
            }

            if (lLstmssingFields.Count > 0)
            {
                string message = string.Format("Favor de revisar {0}:\n{1}",
                    (lLstmssingFields.Count == 1 ? "el siguiente campo" : "los siguientes campos"),
                    string.Join("\n", lLstmssingFields.Select(x => string.Format("-{0}", x)).ToArray()));

                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(message, 1, "Ok", "Cancelar", "");

            }
            return lLstmssingFields.Count == 0 ? true : false;

        }

        private bool ValidateRouteFields()
        {
            IList<string> lLstmssingFields = new List<string>();

            if (string.IsNullOrEmpty(lObjTxtOrigin.Value))
            {
                lLstmssingFields.Add("Agregue origen");
            }

            if (string.IsNullOrEmpty(lObjTxtDestination.Value))
            {
                lLstmssingFields.Add("Agregue destino");
            }

            if (!ValidateTown(lObjMorign.Value))
            {
                lLstmssingFields.Add("El municipio origen no existe");
            }

            if (!ValidateTown(lObjMDest.Value))
            {
                lLstmssingFields.Add("El municipio destino no existe");
            }

            if (string.IsNullOrEmpty(lObjMorign.Value))
            {
                lLstmssingFields.Add("Agregue municipio de origen");
            }

            if (string.IsNullOrEmpty(lObjTxtDestination.Value))
            {
                lLstmssingFields.Add("Agregue municipio destino");
            }

            if (lLstmssingFields.Count > 0)
            {
                string message = string.Format("Favor de completar {0}:\n{1}",
                    (lLstmssingFields.Count == 1 ? "el siguiente campo" : "los siguientes campos"),
                    string.Join("\n", lLstmssingFields.Select(x => string.Format("-{0}", x)).ToArray()));

                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(message, 1, "Ok", "Cancelar", "");

            }
            return lLstmssingFields.Count == 0 ? true : false;

        }

        private bool ValidateTown(string pStrTown)
        {
            return mObjTransportsFactory.GetRouteService().CheckTown(pStrTown);
        }

        private bool ValidateLine()
        {
            IList<string> lLstmssingFields = new List<string>();

            if (!mBoolInternal && string.IsNullOrEmpty(lObjTxtArticle.Value))
            {
                lLstmssingFields.Add("Agregue un articulo");
            }

            if (mBoolInternal && string.IsNullOrEmpty(lObjTxtInternal.Value))
            {
                lLstmssingFields.Add("Folio Interno");
            }

            if (mBoolInternal && string.IsNullOrEmpty(lObjCboArea.Value))
            {
                lLstmssingFields.Add("Área");
            }

            if (string.IsNullOrEmpty(lObjTxtFolio.Value) && mIntFormType != 139)
            {
                lLstmssingFields.Add("Ingrese un folio de papeleta");
            }

            if (string.IsNullOrEmpty(lObjCboShared.Value))
            {
                lLstmssingFields.Add("Seleccione si es compartido");
            }

            if (string.IsNullOrEmpty(lObjCboPayload.Value))
            {
                lLstmssingFields.Add("Seleccione el tipo de carga");
            }

            if (string.IsNullOrEmpty(lObjCboVehicleType.Value))
            {
                lLstmssingFields.Add("Seleccione el tipo de vehiculo");
            }

            if (string.IsNullOrEmpty(lObjTxtEcNum.Value) && mIntFormType != 139)
            {
                lLstmssingFields.Add("Agregue el numero economico");
            }

            if (string.IsNullOrEmpty(lObjTxtDriver.Value) && mIntFormType != 139)
            {
                lLstmssingFields.Add("Ingrese a un chofer");
            }

            if (string.IsNullOrEmpty(lObjTxtOrigin.Value))
            {
                lLstmssingFields.Add("Seleccione o agregue una ruta");
            }


            if (lLstmssingFields.Count > 0)
            {
                string message = string.Format("Favor de completar {0}:\n{1}",
                    (lLstmssingFields.Count == 1 ? "el siguiente campo" : "los siguientes campos"),
                    string.Join("\n", lLstmssingFields.Select(x => string.Format("-{0}", x)).ToArray()));

                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(message, 1, "Ok", "Cancelar", "");

            }
            return lLstmssingFields.Count == 0 ? true : false;
        }

        #endregion

        #region Events
        private void lObjTxt_Validate(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if ((mBoolRoadLoaded && mObjRoutes != null) && (mIntActualRoute == mObjRoutes.Route))
            {
                if (pVal.ItemChanged)
                {
                    lObjBtnUpdate.Caption = "Actualizar";
                    lObjBtnUpdate.Item.Enabled = true;
                }
            }
            else
            {
                if (pVal.ItemChanged)
                {
                    lObjBtnUpdate.Caption = "Nuevo";
                    lObjBtnUpdate.Item.Enabled = true;
                }
            }


        }

        private void lObjTxt_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            SAPbouiCOM.EditText lObjTxt = sboObject as SAPbouiCOM.EditText;
            if (string.IsNullOrEmpty(lObjTxt.Value))
            {
                lObjTxt.Value = "0";
            }
            SetKmAmounts();
            SetCashAmounts();
        }

        private void lObjBtnCancel_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (lObjBtnCancel.Item.Enabled)
            {
                if (ReverseJournal())
                {
                    ChangeFreightStatus();
                    LoadInternalFreight(lObjTxtInternal.Value);
                }
            }
        }

        private void ChangeFreightStatus()
        {
            try
            {
                SAPbobsCOM.UserTable lObjInternalFreight = (SAPbobsCOM.UserTable)DIApplication.Company.UserTables.Item("UG_TR_INTLFRGHT");

                lObjInternalFreight.UserFields.Fields.Item("U_Status").Value = StatusEnum.CANCELED.GetDescription();
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void lObjCboShared_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (mBoolInternal)
            {
                if (!string.IsNullOrEmpty(lObjCboShared.Value) && (SharedEnum)Convert.ToInt32(lObjCboShared.Value) == SharedEnum.Yes)
                {
                    SharedConfig(true);
                }
                else
                {
                    SharedConfig(false);
                }
            }
        }

        private void lObjTxt_KeyDownAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.CharPressed == 13)
            {
                if (!mBoolSearchMode)
                {
                    SelectModal(pVal.ItemUID);
                }
                else
                {
                    SearchFolio();
                }
            }
        }

        private void SBO_Application_ItemEvent(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (FormUID.Equals(this.UIAPIRawForm.UniqueID))
            {
                if (pVal.BeforeAction)
                {
                    switch (pVal.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                            pBoolFreightsModal = false;
                            UnloadEvents();
                            break;
                    }
                }
            }

            #region CFL
            else if (mModalFrmCFL != null && (FormUID.Equals(mModalFrmCFL.mStrFrmName) && mModalFrmCFL.pIntRow > 0))
            {
                if (!pVal.BeforeAction)
                {
                    switch (pVal.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_CLICK:
                            if (pVal.ItemUID.Equals(mModalFrmCFL.pObjBtnSelect.Item.UniqueID))
                            {
                                SetCFLTextBoxes();
                            }
                            break;
                        case BoEventTypes.et_FORM_CLOSE:
                            mModalFrmCFL.UnloadEvents();
                            break;
                    }
                }
            }
            #endregion
            #region Routes
            else if (mModalFrmRouteFinder != null
                && (FormUID.Equals(mModalFrmRouteFinder.mStrFrmName)))
            {
                if (!pVal.BeforeAction)
                {
                    switch (pVal.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_CLICK:
                            if (pVal.ItemUID.Equals(pObjBtn.Item.UniqueID))
                            {
                                SetRoutes();
                            }
                            break;
                        case BoEventTypes.et_FORM_CLOSE:
                            mModalFrmRouteFinder.UnloadEvents();
                            break;
                    }
                }
            }
            #endregion

        }

        private void lObjCboVehicleType_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!string.IsNullOrEmpty(lObjCboVehicleType.Value))
            {
                lObjTxtEcNum.Value = string.Empty;
                LoadVehiclePrices(true);
                SetKmAmounts();
                SetCashAmounts();
            }
            else
            {
                LoadVehiclePrices(false);
            }

        }

        private void lObjBtnRoute_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (lObjBtnRoute.Item.Enabled)
                {
                    ShowMfRouteFinder();
                }
            }
            catch (Exception lObjException)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(lObjException.Message
                                      , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        private void lObjBtnUpdate_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (lObjBtnUpdate.Item.Enabled)
            {
                switch (lObjBtnUpdate.Caption)
                {
                    case "Nuevo":
                        NewRoute();
                        break;
                    case "Actualizar":
                        UpdateRoute();
                        break;
                }
            }

        }

        private void lObjChkIns_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (lObjChkEnsm.Item.Enabled)
            {
                if (lObjChkEnsm.Checked)
                {
                    LoadInsuranceAmount(false);
                    SetCashAmounts();
                }
                else
                {
                    LoadInsuranceAmount(true);
                    SetCashAmounts();
                }
            }
        }

        private void lObjBtnAccept_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (lObjBtnAccept.Item.Enabled && ValidateLine())
            {
                if (!mBoolInternal)
                {
                    GetSalesOrderLines();
                    if (SetSalesOrder())
                    {
                        CloseForm();
                    }
                }
                else
                {
                    if (CreateJournal())
                    {
                        SaveDetails();
                        LoadInternalFreight(string.Empty);
                    }

                }

            }
        }

        private void lObjBtnExit_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (lObjBtnExit.Item.Enabled)
            {
                CloseForm();
            }
        }

        private void frmFreights_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (mBoolInternal)
                {
                    int lIntFolio;
                    if (!pVal.BeforeAction && UIApplication.GetApplication().Forms.ActiveForm.UniqueID == this.UIAPIRawForm.UniqueID)
                    {
                        switch (pVal.MenuUID)
                        {
                            case "1281": //SearchRecord
                                SearchMode();
                                break;

                            case "1282": //New Record
                                LoadInternalFreight(string.Empty);
                                break;

                            case "1288": //Next Record
                                lIntFolio = Convert.ToInt32(lObjTxtInternal.Value) + 1;
                                if (lIntFolio <= GetLastFolio())
                                {
                                    LoadInternalFreight(lIntFolio.ToString());
                                }
                                else
                                {
                                    LoadInternalFreight(GetFirstFolio().ToString());
                                }
                                break;

                            case "1289"://Previous Record
                                lIntFolio = Convert.ToInt32(lObjTxtInternal.Value) - 1;
                                if (lIntFolio >= GetFirstFolio())
                                {
                                    LoadInternalFreight(lIntFolio.ToString());
                                }
                                else
                                {
                                    LoadInternalFreight(GetLastFolio().ToString());
                                }
                                break;

                            case "1290"://First Record
                                LoadInternalFreight(GetFirstFolio().ToString());
                                break;

                            case "1291": //Last Record
                                LoadInternalFreight(GetLastFolio().ToString());
                                break;

                        }
                    }
                }
            }
            catch (Exception lObjException)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(lObjException.Message
                                                    , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                this.UIAPIRawForm.Close();
            }
        }

        private void lObjTxtArticle_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!string.IsNullOrEmpty(lObjTxtArticle.Value))
            {
                mFlRetention = mObjTransportsFactory.GetRouteService().CheckIfRetention(lObjTxtArticle.Value);
            }
        }






        //public void Accept()
        //{
        //    GetSalesOrderLines();
        //    OpenSalesOrder();
        //}
        #endregion

        #region InitializeComponents
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lObjTxtArticle = ((SAPbouiCOM.EditText)(this.GetItem("txtArt").Specific));
            this.lObjTxtDesc = ((SAPbouiCOM.EditText)(this.GetItem("txtDesc").Specific));
            this.lObjCboShared = ((SAPbouiCOM.ComboBox)(this.GetItem("cboShrd").Specific));
            this.lObjlblArticle = ((SAPbouiCOM.StaticText)(this.GetItem("lblArt").Specific));
            this.lObjlblShared = ((SAPbouiCOM.StaticText)(this.GetItem("lblShrd").Specific));
            this.lObjCboVehicleType = ((SAPbouiCOM.ComboBox)(this.GetItem("txtVehT").Specific));
            this.lObjlblVehicleType = ((SAPbouiCOM.StaticText)(this.GetItem("lblVehT").Specific));
            this.lObjTxtFolio = ((SAPbouiCOM.EditText)(this.GetItem("txtFol").Specific));
            this.lObjlblFolio = ((SAPbouiCOM.StaticText)(this.GetItem("lblFol").Specific));
            this.lObjlblEcNum = ((SAPbouiCOM.StaticText)(this.GetItem("lblEcNum").Specific));
            this.lObjTxtEcNum = ((SAPbouiCOM.EditText)(this.GetItem("txtEcNum").Specific));
            this.lObjCboPayload = ((SAPbouiCOM.ComboBox)(this.GetItem("cboPyld").Specific));
            this.lObjlblPayload = ((SAPbouiCOM.StaticText)(this.GetItem("lblPyld").Specific));
            this.lObjlblOrigin = ((SAPbouiCOM.StaticText)(this.GetItem("lblOrgn").Specific));
            this.lObjlblDestination = ((SAPbouiCOM.StaticText)(this.GetItem("lblDest").Specific));
            this.lObjlblKmA = ((SAPbouiCOM.StaticText)(this.GetItem("lblKmA").Specific));
            this.lObjTxtOrigin = ((SAPbouiCOM.EditText)(this.GetItem("txtOrgn").Specific));
            this.lObjTxtDestination = ((SAPbouiCOM.EditText)(this.GetItem("txtDest").Specific));
            this.lObjTxtKmA = ((SAPbouiCOM.EditText)(this.GetItem("txtKmA").Specific));
            this.lObjlblKmC = ((SAPbouiCOM.StaticText)(this.GetItem("lblKmC").Specific));
            this.lObjlblKmE = ((SAPbouiCOM.StaticText)(this.GetItem("lblKmE").Specific));
            this.lObjlblPriceA = ((SAPbouiCOM.StaticText)(this.GetItem("lblPrcA").Specific));
            this.lObjlblPriceC = ((SAPbouiCOM.StaticText)(this.GetItem("lblPrcC").Specific));
            this.lObjlblPriceE = ((SAPbouiCOM.StaticText)(this.GetItem("lblPrcE").Specific));
            this.lObjTxtKmC = ((SAPbouiCOM.EditText)(this.GetItem("txtKmC").Specific));
            this.lObjTxtKmE = ((SAPbouiCOM.EditText)(this.GetItem("txtKmE").Specific));
            this.lObjTxtPriceA = ((SAPbouiCOM.EditText)(this.GetItem("txtPrcA").Specific));
            this.lObjTxtPriceC = ((SAPbouiCOM.EditText)(this.GetItem("txtPrcC").Specific));
            this.lObjTxtPriceE = ((SAPbouiCOM.EditText)(this.GetItem("txtPrcE").Specific));
            this.lObjTxtKmB = ((SAPbouiCOM.EditText)(this.GetItem("txtKmB").Specific));
            this.lObjTxtKmD = ((SAPbouiCOM.EditText)(this.GetItem("txtKmD").Specific));
            this.lObjTxtKmF = ((SAPbouiCOM.EditText)(this.GetItem("txtKmF").Specific));
            this.lObjTxtPriceB = ((SAPbouiCOM.EditText)(this.GetItem("txtPrcB").Specific));
            this.lObjTxtPriceD = ((SAPbouiCOM.EditText)(this.GetItem("txtPrcD").Specific));
            this.lObjTxtPriceF = ((SAPbouiCOM.EditText)(this.GetItem("txtPrcF").Specific));
            this.lObjlblKmB = ((SAPbouiCOM.StaticText)(this.GetItem("lblKmB").Specific));
            this.lObjlblKmD = ((SAPbouiCOM.StaticText)(this.GetItem("lblKmD").Specific));
            this.lObjlblKmF = ((SAPbouiCOM.StaticText)(this.GetItem("lblKmF").Specific));
            this.lObjlblPriceF = ((SAPbouiCOM.StaticText)(this.GetItem("lblPrcF").Specific));
            this.lObjlblPriceD = ((SAPbouiCOM.StaticText)(this.GetItem("lblPrcD").Specific));
            this.lObjlblPriceB = ((SAPbouiCOM.StaticText)(this.GetItem("lblPrcB").Specific));
            this.lObjBtnUpdate = ((SAPbouiCOM.Button)(this.GetItem("btnUpd").Specific));
            this.lObjlblDriver = ((SAPbouiCOM.StaticText)(this.GetItem("lblDriv").Specific));
            this.lObjTxtDriver = ((SAPbouiCOM.EditText)(this.GetItem("txtDriv").Specific));
            this.lObjlblTotkm = ((SAPbouiCOM.StaticText)(this.GetItem("lblTotKm").Specific));
            this.lObjlblAmountKm = ((SAPbouiCOM.StaticText)(this.GetItem("lblAmtKm").Specific));
            this.lObjlblExtra = ((SAPbouiCOM.StaticText)(this.GetItem("lblExtra").Specific));
            this.lObjlblAmountEns = ((SAPbouiCOM.StaticText)(this.GetItem("lblAmtEns").Specific));
            this.lObjlblTotalFreight = ((SAPbouiCOM.StaticText)(this.GetItem("lblTFrgh").Specific));
            this.lObjChkEnsm = ((SAPbouiCOM.CheckBox)(this.GetItem("chkEnsm").Specific));
            this.lObjlblFrom = ((SAPbouiCOM.StaticText)(this.GetItem("lblFrom").Specific));
            this.lObjlblTo = ((SAPbouiCOM.StaticText)(this.GetItem("lblTo").Specific));
            this.lObjBtnAccept = ((SAPbouiCOM.Button)(this.GetItem("btnAcpt").Specific));
            this.lObjTxtTotKm = ((SAPbouiCOM.EditText)(this.GetItem("txtTotKm").Specific));
            this.lObjTxtAmountKm = ((SAPbouiCOM.EditText)(this.GetItem("txtAmtKm").Specific));
            this.lObjTxtExtra = ((SAPbouiCOM.EditText)(this.GetItem("txtExtra").Specific));
            this.lObjTxtAmountEns = ((SAPbouiCOM.EditText)(this.GetItem("txtAmtEns").Specific));
            this.lObjTxtTotalFreight = ((SAPbouiCOM.EditText)(this.GetItem("txtTFrgh").Specific));
            this.lObjBtnRoute = ((SAPbouiCOM.Button)(this.GetItem("btnRoute").Specific));
            this.lObjlblDescription = ((SAPbouiCOM.StaticText)(this.GetItem("lblDesc").Specific));
            this.lObjMorign = ((SAPbouiCOM.EditText)(this.GetItem("txtMOrig").Specific));
            this.lObjMDest = ((SAPbouiCOM.EditText)(this.GetItem("txtMDest").Specific));
            this.lObjlblArea = ((SAPbouiCOM.StaticText)(this.GetItem("lblArea").Specific));
            this.lObjlblComment = ((SAPbouiCOM.StaticText)(this.GetItem("lblCmnt").Specific));
            this.lObjTxtComment = ((SAPbouiCOM.EditText)(this.GetItem("txtCmnt").Specific));
            this.lObjBtnExit = ((SAPbouiCOM.Button)(this.GetItem("bntExt").Specific));
            this.lObjTxtInternal = ((SAPbouiCOM.EditText)(this.GetItem("txtItrnl").Specific));
            this.lObjTxtStatus = ((SAPbouiCOM.EditText)(this.GetItem("txtStat").Specific));
            this.lObjBtnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCan").Specific));
            this.lObjCboArea = ((SAPbouiCOM.ComboBox)(this.GetItem("cboArea").Specific));
            this.mObjlblRetention = ((SAPbouiCOM.StaticText)(this.GetItem("lblRet").Specific));
            this.mObjTxtRetentions = ((SAPbouiCOM.EditText)(this.GetItem("txtRet").Specific));
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

        #region SAP objects
        private SAPbouiCOM.EditText lObjTxtArticle;
        private SAPbouiCOM.EditText lObjTxtDesc;
        private SAPbouiCOM.ComboBox lObjCboShared;
        private SAPbouiCOM.StaticText lObjlblArticle;
        private SAPbouiCOM.StaticText lObjlblShared;
        private SAPbouiCOM.ComboBox lObjCboVehicleType;
        private SAPbouiCOM.StaticText lObjlblVehicleType;
        private SAPbouiCOM.EditText lObjTxtFolio;
        private SAPbouiCOM.StaticText lObjlblFolio;
        private SAPbouiCOM.StaticText lObjlblEcNum;
        private SAPbouiCOM.EditText lObjTxtEcNum;
        private SAPbouiCOM.ComboBox lObjCboPayload;
        private SAPbouiCOM.StaticText lObjlblPayload;
        private SAPbouiCOM.StaticText lObjlblOrigin;
        private SAPbouiCOM.StaticText lObjlblDestination;
        private SAPbouiCOM.StaticText lObjlblKmA;
        private SAPbouiCOM.EditText lObjTxtOrigin;
        private SAPbouiCOM.EditText lObjTxtDestination;
        private SAPbouiCOM.EditText lObjTxtKmA;
        private SAPbouiCOM.EditText lObjMorign;
        private SAPbouiCOM.EditText lObjMDest;
        private SAPbouiCOM.StaticText lObjlblKmC;
        private SAPbouiCOM.StaticText lObjlblKmE;
        private SAPbouiCOM.StaticText lObjlblPriceA;
        private SAPbouiCOM.StaticText lObjlblPriceC;
        private SAPbouiCOM.StaticText lObjlblPriceE;
        private SAPbouiCOM.EditText lObjTxtKmC;
        private SAPbouiCOM.EditText lObjTxtKmE;
        private SAPbouiCOM.EditText lObjTxtPriceA;
        private SAPbouiCOM.EditText lObjTxtPriceC;
        private SAPbouiCOM.EditText lObjTxtPriceE;
        private SAPbouiCOM.EditText lObjTxtKmB;
        private SAPbouiCOM.EditText lObjTxtKmD;
        private SAPbouiCOM.EditText lObjTxtKmF;
        private SAPbouiCOM.EditText lObjTxtPriceB;
        private SAPbouiCOM.EditText lObjTxtPriceD;
        private SAPbouiCOM.EditText lObjTxtPriceF;
        private SAPbouiCOM.StaticText lObjlblKmB;
        private SAPbouiCOM.StaticText lObjlblKmD;
        private SAPbouiCOM.StaticText lObjlblKmF;
        private SAPbouiCOM.StaticText lObjlblPriceF;
        private SAPbouiCOM.StaticText lObjlblPriceD;
        private SAPbouiCOM.StaticText lObjlblPriceB;
        private SAPbouiCOM.Button lObjBtnUpdate;
        private SAPbouiCOM.StaticText lObjlblDriver;
        private SAPbouiCOM.EditText lObjTxtDriver;
        private SAPbouiCOM.StaticText lObjlblTotkm;
        private SAPbouiCOM.StaticText lObjlblAmountKm;
        private SAPbouiCOM.StaticText lObjlblExtra;
        private SAPbouiCOM.StaticText lObjlblAmountEns;
        private SAPbouiCOM.StaticText lObjlblTotalFreight;
        private SAPbouiCOM.CheckBox lObjChkEnsm;
        private SAPbouiCOM.StaticText lObjlblFrom;
        private SAPbouiCOM.StaticText lObjlblTo;
        private SAPbouiCOM.Button lObjBtnAccept;
        private SAPbouiCOM.EditText lObjTxtTotKm;
        private SAPbouiCOM.EditText lObjTxtAmountKm;
        private SAPbouiCOM.EditText lObjTxtExtra;
        private SAPbouiCOM.EditText lObjTxtAmountEns;
        private SAPbouiCOM.EditText lObjTxtTotalFreight;
        private SAPbouiCOM.Button lObjBtnRoute;
        private SAPbouiCOM.StaticText lObjlblDescription;
        private StaticText lObjlblArea;
        private StaticText lObjlblComment;
        private EditText lObjTxtComment;
        private Button lObjBtnExit;
        private EditText lObjTxtInternal;
        private EditText lObjTxtStatus;
        private Button lObjBtnCancel;
        private ComboBox lObjCboArea;
        private StaticText mObjlblRetention;

        #endregion
        private EditText mObjTxtRetentions;

    }
}
