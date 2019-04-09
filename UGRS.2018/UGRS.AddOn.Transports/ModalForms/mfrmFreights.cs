using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using UGRS.Core.Extension.Enum;
using UGRS.Core.Services;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.Transports.Enums;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.SDK.DI.Transports;
using UGRS.Core.SDK.DI.Transports.Tables;
using UGRS.Core.SDK.DI.Transports.Utility;
using UGRS.Core.Utility;
using UGRS.Core.SDK.UI;

namespace UGRS.AddOn.Transports.ModalForms
{
    public class mfrmFreights
    {
        mfrmRouteFinder mModalFrmRouteFinder = null;
        mfrmCFL mModalFrmCFL = null;
        Utils lObjUtility = new Utils();
        TransportServiceFactory mObjTransportsFactory = new TransportServiceFactory();
        List<VehiclesDTO> mLstVehicles;

        private bool mBoolLoaded = false;
        private int mIntUserSignature = DIApplication.Company.UserSignature;
        private int mIntActualRoute = 0;
        private float mFlKm = 0;
        private InsuranceDTO mObjInsurance;
        private List<SalesOrderLinesDTO> mLstSalesOrderLines = null;

        private SAPbouiCOM.Form mObjMFreights;

        #region Sales order form
        SAPbouiCOM.Form mSalesOrderForm;
        SAPbouiCOM.Matrix mObjSOMtx;
        SAPbouiCOM.ComboBox mObjCboPyloadType;
        SAPbouiCOM.ComboBox mObjCboVehicleType;
        SAPbouiCOM.ComboBox mObjCboRoute;
        SAPbouiCOM.EditText mObjTxtTotKm;
        SAPbouiCOM.EditText mObjTxtKmA;
        SAPbouiCOM.EditText mObjTxtKmB;
        SAPbouiCOM.EditText mObjTxtKmC;
        SAPbouiCOM.EditText mObjTxtKmD;
        SAPbouiCOM.EditText mObjTxtKmE;
        SAPbouiCOM.EditText mObjTxtKmF;
        //SAPbouiCOM.EditText mObjTxtHeads;
        SAPbouiCOM.EditText mObjTxtTotKG;
        SAPbouiCOM.EditText mObjTxtExtra;
        SAPbouiCOM.EditText mObjTxtAnthPyld;
        SAPbouiCOM.EditText mObjTxtDestination;
        #endregion

        #region Constructor
        public mfrmFreights(string pStrFrmName)
        {
            LoadXmlForm(pStrFrmName);
        }
        #endregion

        #region Methods
        private void LoadXmlForm(string pStrFrmName)
        {
            System.Xml.XmlDocument lObjXmlDoc = new System.Xml.XmlDocument();
            string lStrPath = PathUtilities.GetCurrent("ModalForms") + "\\" + pStrFrmName + ".xml";

            lObjXmlDoc.Load(lStrPath);

            SAPbouiCOM.FormCreationParams lObjCreationPackage =
                (SAPbouiCOM.FormCreationParams)SAPbouiCOM.Framework.Application.SBO_Application.CreateObject
                (SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);


            lObjCreationPackage.XmlData = lObjXmlDoc.InnerXml;

            if (pStrFrmName.Equals(pStrFrmName))
            {
                if (!lObjUtility.FormExists(pStrFrmName))
                {
                    lObjCreationPackage.UniqueID = pStrFrmName;
                    lObjCreationPackage.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Fixed;
                    lObjCreationPackage.Modality = SAPbouiCOM.BoFormModality.fm_Modal;
                    lObjCreationPackage.FormType = pStrFrmName;

                    mObjMFreights = SAPbouiCOM.Framework.Application.SBO_Application.Forms.AddEx(lObjCreationPackage);
                    mObjMFreights.Title = "Busqueda de rutas";
                    mObjMFreights.Left = 400;
                    mObjMFreights.Top = 100;
                    mObjMFreights.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                    mObjMFreights.Visible = true;
                    InitializeXmlForm();
                }
            }
            else
            {
                mObjMFreights.Select();
            }
        }

        private void InitializeXmlForm()
        {
            mObjMFreights.Freeze(true);
            SetItems();
            InitializeEvents();
            LoadCombos();
            mObjInsurance = mObjTransportsFactory.GetRouteService().GetInsuranceLine(mIntUserSignature);
            mObjMFreights.Freeze(false);
        }

        private void InitializeEvents()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            this.lObjCboShared.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.lObjCboShared_ComboSelectAfter);
            this.lObjCboVehicleType.ComboSelectAfter += new _IComboBoxEvents_ComboSelectAfterEventHandler(this.lObjCboVehicleType_ComboSelectAfter);
            this.lObjBtnUpdate.ClickBefore += new _IButtonEvents_ClickBeforeEventHandler(this.lObjBtnUpdate_ClickBefore);
            this.lObjBtnRoute.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnRoute_ClickBefore);
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
        }

        private void UnloadEvents()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            this.lObjCboShared.ComboSelectAfter -= new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.lObjCboShared_ComboSelectAfter);
            this.lObjCboVehicleType.ComboSelectAfter -= new _IComboBoxEvents_ComboSelectAfterEventHandler(this.lObjCboVehicleType_ComboSelectAfter);
            this.lObjBtnUpdate.ClickBefore -= new _IButtonEvents_ClickBeforeEventHandler(this.lObjBtnUpdate_ClickBefore);
            this.lObjBtnRoute.ClickBefore -= new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnRoute_ClickBefore);
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
        }

        private void SetItems()
        {

            this.lObjTxtArticle = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtArt").Specific));
            this.lObjTxtDesc = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtDesc").Specific));
            this.lObjCboShared = ((SAPbouiCOM.ComboBox)(mObjMFreights.Items.Item("cboShrd").Specific));
            this.lObjlblArticle = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblArt").Specific));
            this.lObjlblShared = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblShrd").Specific));
            this.lObjCboVehicleType = ((SAPbouiCOM.ComboBox)(mObjMFreights.Items.Item("txtVehT").Specific));
            this.lObjlblVehicleType = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblVehT").Specific));
            this.lObjTxtFolio = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtFol").Specific));
            this.lObjlblFolio = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblFolio").Specific));
            this.lObjlblEcNum = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblEcNum").Specific));
            this.lObjTxtEcNum = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtEcNum").Specific));
            this.lObjCboPayload = ((SAPbouiCOM.ComboBox)(mObjMFreights.Items.Item("cboPyld").Specific));
            this.lObjlblPayload = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblPyld").Specific));
            this.lObjlblOrigin = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblOrgn").Specific));
            this.lObjlblDestination = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblDest").Specific));
            this.lObjlblKmA = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblKmA").Specific));
            this.lObjTxtOrigin = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtOrgn").Specific));
            this.lObjTxtDestination = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtDest").Specific));
            this.lObjTxtKmA = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtKmA").Specific));
            this.lObjlblKmC = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblKmC").Specific));
            this.lObjlblKmE = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblKmE").Specific));
            this.lObjlblPriceA = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblPrcA").Specific));
            this.lObjlblPriceC = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblPrcC").Specific));
            this.lObjlblPriceE = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblPrcE").Specific));
            this.lObjTxtKmC = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtKmC").Specific));
            this.lObjTxtKmE = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtKmE").Specific));
            this.lObjTxtPriceA = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtPrcA").Specific));
            this.lObjTxtPriceC = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtPrcC").Specific));
            this.lObjTxtPriceE = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtPrcE").Specific));
            this.lObjTxtKmB = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtKmB").Specific));
            this.lObjTxtKmD = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtKmD").Specific));
            this.lObjTxtKmF = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtKmF").Specific));
            this.lObjTxtPriceB = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtPrcB").Specific));
            this.lObjTxtPriceD = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtPrcD").Specific));
            this.lObjTxtPriceF = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtPrcF").Specific));
            this.lObjlblKmB = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblKmB").Specific));
            this.lObjlblKmD = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblKmD").Specific));
            this.lObjlblKmF = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblKmF").Specific));
            this.lObjlblPriceF = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblPrcF").Specific));
            this.lObjlblPriceD = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblPrcD").Specific));
            this.lObjlblPriceB = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblPrcB").Specific));
            this.lObjBtnUpdate = ((SAPbouiCOM.Button)(mObjMFreights.Items.Item("btnUpd").Specific));
            this.lObjlblDriver = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblDriv").Specific));
            this.lObjTxtDriver = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtDriv").Specific));
            this.lObjlblTotkm = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblTotKm").Specific));
            this.lObjlblAmountKm = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblAmtKm").Specific));
            this.lObjlblExtra = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblExtra").Specific));
            this.lObjlblAmountEns = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblAmtEns").Specific));
            this.lObjlblTotalFreight = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblTFrgh").Specific));
            this.lObjChkEnsm = ((SAPbouiCOM.CheckBox)(mObjMFreights.Items.Item("chkEnsm").Specific));
            this.lObjlblFrom = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblFrom").Specific));
            this.lObjlblTo = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblTo").Specific));
            this.lObjBtnAccept = ((SAPbouiCOM.Button)(mObjMFreights.Items.Item("btnAcpt").Specific));
            this.lObjTxtTotKm = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtTotKm").Specific));
            this.lObjTxtAmountKm = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtAmtKm").Specific));
            this.lObjTxtExtra = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtExtra").Specific));
            this.lObjTxtAmountEns = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtAmtEns").Specific));
            this.lObjTxtTotalFreight = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtTFrgh").Specific));
            this.lObjBtnRoute = ((SAPbouiCOM.Button)(mObjMFreights.Items.Item("btnRoute").Specific));
            this.lObjlblDescription = ((SAPbouiCOM.StaticText)(mObjMFreights.Items.Item("lblDesc").Specific));
            this.lObjMorign = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtMOrig").Specific));
            this.lObjMDest = ((SAPbouiCOM.EditText)(mObjMFreights.Items.Item("txtMDest").Specific));
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
            lObjTxtOrigin.Value = ((SAPbouiCOM.EditText)mModalFrmRouteFinder.pObjMtxRoutes.Columns.Item("cOrign")
                        .Cells.Item(mModalFrmRouteFinder.mIntRow).Specific).Value.ToString();

            lObjMorign.Value = ((SAPbouiCOM.EditText)mModalFrmRouteFinder.pObjMtxRoutes.Columns.Item("cMOrgn")
                        .Cells.Item(mModalFrmRouteFinder.mIntRow).Specific).Value.ToString();

            lObjMDest.Value = ((SAPbouiCOM.EditText)mModalFrmRouteFinder.pObjMtxRoutes.Columns.Item("cMDest")
                        .Cells.Item(mModalFrmRouteFinder.mIntRow).Specific).Value.ToString();

            lObjTxtDestination.Value = ((SAPbouiCOM.EditText)mModalFrmRouteFinder.pObjMtxRoutes.Columns.Item("cDest")
                        .Cells.Item(mModalFrmRouteFinder.mIntRow).Specific).Value.ToString();

            lObjTxtKmA.Value = ((SAPbouiCOM.EditText)mModalFrmRouteFinder.pObjMtxRoutes.Columns.Item("cKmA")
                        .Cells.Item(mModalFrmRouteFinder.mIntRow).Specific).Value.ToString();
            lObjTxtKmB.Value = ((SAPbouiCOM.EditText)mModalFrmRouteFinder.pObjMtxRoutes.Columns.Item("cKmB")
                        .Cells.Item(mModalFrmRouteFinder.mIntRow).Specific).Value.ToString();
            lObjTxtKmC.Value = ((SAPbouiCOM.EditText)mModalFrmRouteFinder.pObjMtxRoutes.Columns.Item("cKmC")
                        .Cells.Item(mModalFrmRouteFinder.mIntRow).Specific).Value.ToString();
            lObjTxtKmD.Value = ((SAPbouiCOM.EditText)mModalFrmRouteFinder.pObjMtxRoutes.Columns.Item("cKmD")
                        .Cells.Item(mModalFrmRouteFinder.mIntRow).Specific).Value.ToString();
            lObjTxtKmE.Value = ((SAPbouiCOM.EditText)mModalFrmRouteFinder.pObjMtxRoutes.Columns.Item("cKmE")
                        .Cells.Item(mModalFrmRouteFinder.mIntRow).Specific).Value.ToString();
            lObjTxtKmF.Value = ((SAPbouiCOM.EditText)mModalFrmRouteFinder.pObjMtxRoutes.Columns.Item("cKmF")
                        .Cells.Item(mModalFrmRouteFinder.mIntRow).Specific).Value.ToString();

            LoadConfig();

        }

        private void LoadConfig()
        {
            lObjBtnUpdate.Caption = "Actualizar";
            lObjBtnUpdate.Item.Enabled = false;
        }

        private void SetTextBox()
        {
            switch (mModalFrmCFL.mStrCFLType)
            {
                case "CFL_TownsA":
                    lObjMorign.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cName")
                        .Cells.Item(mModalFrmCFL.mIntRow).Specific).Value.ToString();
                    break;
                case "CFL_TownsB":
                    lObjMDest.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cName")
                        .Cells.Item(mModalFrmCFL.mIntRow).Specific).Value.ToString();
                    break;
                case "CFL_Items":
                    lObjTxtArticle.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cName")
                        .Cells.Item(mModalFrmCFL.mIntRow).Specific).Value.ToString();
                    lObjTxtDesc.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cDesc")
                        .Cells.Item(mModalFrmCFL.mIntRow).Specific).Value.ToString();
                    break;
                case "CFL_AF":
                    lObjTxtEcNum.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cItem")
                        .Cells.Item(mModalFrmCFL.mIntRow).Specific).Value.ToString();
                    break;
                case "CFL_DR":
                    lObjTxtDriver.Value = ((SAPbouiCOM.EditText)mModalFrmCFL.pObjMtxCFL.Columns.Item("cName")
                        .Cells.Item(mModalFrmCFL.mIntRow).Specific).Value.ToString();
                    break;
            }
        }

        private void OpenCFLModal(string pStrModal, string pStrCFL, string pStrEquip)
        {
            mModalFrmCFL = new ModalForms.mfrmCFL(pStrModal, pStrCFL, DIApplication.Company.UserSignature, pStrEquip);
        }

        private void ShowMfRouteFinder()
        {
            mModalFrmRouteFinder = new ModalForms.mfrmRouteFinder("frmMRouteFinder");
        }

        private void LoadVehiclePrices()
        {
            VehiclesDTO lObjVehicle = mLstVehicles.Select(x => x).Where(x => x.EquipType == lObjCboVehicleType.Value).FirstOrDefault();

            lObjTxtPriceA.Value = lObjVehicle.PathA;
            lObjTxtPriceB.Value = lObjVehicle.PathB;
            lObjTxtPriceC.Value = lObjVehicle.PathC;
            lObjTxtPriceD.Value = lObjVehicle.PathD;
            lObjTxtPriceE.Value = lObjVehicle.PathE;
            lObjTxtPriceF.Value = lObjVehicle.PathF;
        }

        private void SetAmounts()
        {
            SetKilometers();
            SetKilometerAmount();
            SetTotalAmount();
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

        private void SetTotalAmount()
        {
            lObjTxtTotalFreight.Value = (float.Parse(lObjTxtAmountKm.Value) + float.Parse(lObjTxtAmountEns.Value) + float.Parse(lObjTxtExtra.Value)).ToString();
        }

        private void LoadInsuranceAmount(bool pBoolCheck)
        {
            mObjMFreights.Freeze(true);
            lObjTxtAmountEns.Value = pBoolCheck ? (mObjInsurance.Price * mFlKm).ToString() : "0";
            SetAmounts();
            mObjMFreights.Freeze(false);
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
                UnitPrice = 1,
                PayloadType = lObjCboPayload.Value,
                VehicleType = lObjCboVehicleType.Value,
                Route = mIntActualRoute,
                TotKm = lObjTxtTotKm.Value,
                KmA = lObjTxtKmA.Value,
                KmB = lObjTxtKmB.Value,
                KmC = lObjTxtKmC.Value,
                KmD = lObjTxtKmD.Value,
                KmE = lObjTxtKmE.Value,
                KmF = lObjTxtKmF.Value,
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
                    UnitPrice = mObjInsurance.Price
                });
            }

        }

        private void OpenSalesOrder()
        {
            try
            {
                SAPbouiCOM.Framework.Application.SBO_Application.ActivateMenuItem("2050");
                SetSalesOrderControls();
                FillSalesOrder();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
                LogService.WriteError("(OpenFormBP): " + ex.Message);
                LogService.WriteError(ex);
                //PayrollInvoiceLogService.WriteError("Abrir ventana de factura: " + ex.Message);
            }
        }

        private void SetSalesOrderControls()
        {
            mSalesOrderForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetFormByTypeAndCount(139, -1);
            mSalesOrderForm.Mode = BoFormMode.fm_ADD_MODE;
            mObjSOMtx = (SAPbouiCOM.Matrix)mSalesOrderForm.Items.Item("38").Specific;
            //SAPbouiCOM.EditText lll = (SAPbouiCOM.EditText)mSalesOrderForm.Items.Item("4").Specific;
            //lll.Value = "CL00000002";

        }

        private void FillSalesOrder()
        {
            mSalesOrderForm.Freeze(true);

            for (int i = 1; i <= mLstSalesOrderLines.Count; i++)
            {
                mObjCboPyloadType = (SAPbouiCOM.ComboBox)mObjSOMtx.Columns.Item("U_TR_LoadType").Cells.Item(i).Specific;
                mObjCboPyloadType.Select(mLstSalesOrderLines[i - 1].PayloadType, SAPbouiCOM.BoSearchKey.psk_ByValue);

                mObjCboVehicleType = (SAPbouiCOM.ComboBox)mObjSOMtx.Columns.Item("U_TR_VehicleType").Cells.Item(i).Specific;
                mObjCboVehicleType.Select(mLstSalesOrderLines[i - 1].VehicleType, SAPbouiCOM.BoSearchKey.psk_ByValue);

                mObjCboRoute = (SAPbouiCOM.ComboBox)mObjSOMtx.Columns.Item("U_TR_Paths").Cells.Item(i).Specific;
                mObjCboRoute.Select(mLstSalesOrderLines[i - 1].Route, SAPbouiCOM.BoSearchKey.psk_ByValue);

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
            lObjRoute.RowName = string.Empty;

            return lObjRoute;

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

        #endregion

        #region Events
        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (FormUID.Equals(mObjMFreights.UniqueID))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case BoEventTypes.et_KEY_DOWN:
                                if (pVal.CharPressed == 13)
                                {
                                    switch (pVal.ItemUID)
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
                                                OpenCFLModal("frmMCFL", "CFL_AF", mLstVehicles.Where(x => x.Name == lObjCboVehicleType.Value).Select(x => x.EquipType).FirstOrDefault());
                                            }
                                            break;
                                        case "txtDriv":
                                            OpenCFLModal("frmMCFL", "CFL_DR", "");
                                            break;
                                    }
                                }
                                break;

                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                UnloadEvents();
                                break;
                        }
                    }
                }
                else if (mModalFrmCFL != null && FormUID.Equals(mModalFrmCFL.mStrFrmName) && mModalFrmCFL.mIntRow > 0)
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_CLICK:
                                if (pVal.ItemUID.Equals(mModalFrmCFL.pObjBtnSelect.Item.UniqueID))
                                {
                                    SetTextBox();
                                    mModalFrmCFL.CloseForm();
                                }
                                break;
                        }
                    }
                }
                else if (mModalFrmRouteFinder != null && FormUID.Equals(mModalFrmRouteFinder.mStrFrmName) && mModalFrmRouteFinder.mIntRow > 0)
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_CLICK:
                                if (pVal.ItemUID.Equals(mModalFrmRouteFinder.pObjBtnSelect.Item.UniqueID))
                                {
                                    SetRoutesTextsBoxes();
                                    mModalFrmRouteFinder.CloseForm();
                                    mBoolLoaded = true;
                                    mIntActualRoute = mModalFrmRouteFinder.mIntCode;
                                    lObjTxtDriver.Item.Click();
                                }
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogService.WriteError(ex.Message);
                LogService.WriteError(ex);
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(string.Format("ItemEventException: {0}", ex.Message));
            }
        }

        private void lObjTxt_Validate(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (mBoolLoaded && mIntActualRoute == mModalFrmRouteFinder.mIntCode)
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
            catch (Exception ex)
            {
                LogService.WriteError("JournalService (CreateAction): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(string.Format("CreateAction: {0}", ex.Message));
            }
        }

        private void lObjTxt_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                SAPbouiCOM.EditText lObjTxt = sboObject as SAPbouiCOM.EditText;
                if (string.IsNullOrEmpty(lObjTxt.Value))
                {
                    lObjTxt.Value = "0";
                }
                SetAmounts();
            }
            catch (Exception ex)
            {
                LogService.WriteError("JournalService (CreateAction): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(string.Format("CreateAction: {0}", ex.Message));
            }
        }

        private void lObjCboShared_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (!string.IsNullOrEmpty(lObjCboShared.Value) && (SharedEnum)Convert.ToInt32(lObjCboShared.Value) == SharedEnum.Yes)
                {
                    lObjTxtFolio.Item.Enabled = true;
                }
                else
                {
                    lObjTxtFolio.Item.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("JournalService (CreateAction): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(string.Format("CreateAction: {0}", ex.Message));
            }
        }

        private void lObjCboVehicleType_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (!string.IsNullOrEmpty(lObjCboVehicleType.Value))
                {
                    lObjTxtEcNum.Value = string.Empty;
                    LoadVehiclePrices();
                    SetAmounts();
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("JournalService (CreateAction): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(string.Format("CreateAction: {0}", ex.Message));
            }
        }

        private void lObjBtnRoute_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                ShowMfRouteFinder();
            }
            catch (Exception ex)
            {
                LogService.WriteError("JournalService (CreateAction): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(string.Format("CreateAction: {0}", ex.Message));
            }
        }

        private void lObjBtnUpdate_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
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
            catch (Exception ex)
            {
                LogService.WriteError("JournalService (CreateAction): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(string.Format("CreateAction: {0}", ex.Message));
            }

        }

        private void lObjChkIns_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (lObjChkEnsm.Checked)
                {
                    LoadInsuranceAmount(false);
                }
                else
                {
                    LoadInsuranceAmount(true);
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("JournalService (CreateAction): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(string.Format("CreateAction: {0}", ex.Message));
            }
        }

        private void lObjBtnAccept_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                GetSalesOrderLines();
                OpenSalesOrder();
            }
            catch (Exception ex)
            {
                LogService.WriteError("JournalService (CreateAction): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(string.Format("CreateAction: {0}", ex.Message));
            }
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
       /* private SAPbouiCOM.EditText lObjTxtArea;
        private SAPbouiCOM.EditText lObjTxtComment;
        private SAPbouiCOM.StaticText lObjlblArea;
        private SAPbouiCOM.StaticText lObjlblComment;*/

        #endregion
    }
}
