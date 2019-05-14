using SAPbouiCOM.Framework;
using System;
using UGRS.Core.SDK.DI.Transports;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.SDK.DI.Transports.Utility;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.Utility;

namespace UGRS.AddOn.Transports.ModalForms
{
    public class mfrmCFL
    {
        TransportServiceFactory mObjTransportFactory = new TransportServiceFactory();
        Utils lObjUtility = new Utils();

        #region SAP Items
        private SAPbouiCOM.Form lObjCFLModalForm;
        public SAPbouiCOM.Matrix pObjMtxCFL;
        public SAPbouiCOM.Button pObjBtnSelect;
        private SAPbouiCOM.Button lObjBtnCancel;
        private SAPbouiCOM.Button lObjBtnSearch;
        private SAPbouiCOM.EditText lObjTxtSearch;
        #endregion

        #region Attributes
        private int mIntUserSignature = 0;// DIApplication.Company.UserSignature;
        private string mStrCostingCode = string.Empty;
        private string mStrEquip = string.Empty;
        private string mStrWHS = string.Empty;
        private string mStrCardCode = string.Empty;
        public string mStrCFLType = string.Empty;
        public string mStrFrmName = string.Empty;
        public int mIntRow = 0;

        #endregion

        #region Constructor
        public mfrmCFL(string pStrFrmName, string pStrCFLType, int pIntUserSign, string pStrEquip = "")
        {
            mIntUserSignature = pIntUserSign;
            mStrFrmName = pStrFrmName;
            mStrEquip = pStrEquip;
            mStrCFLType = pStrCFLType;
            LoadXmlForm(pStrFrmName);
        }

        public mfrmCFL(CFLParamsDTO pObjParameters)
        {
            mStrCardCode = pObjParameters.CardCode;
            mIntUserSignature = pObjParameters.UserSign;
            mStrFrmName = pObjParameters.FormName;
            mStrEquip = pObjParameters.Equip;
            mStrCFLType = pObjParameters.CFLType;
            LoadXmlForm(pObjParameters.FormName);
        }

        #endregion

        #region Events

        private void lObjBtnCancel_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                CloseForm();
            }
            catch (Exception ex)
            {
                LogService.WriteError(ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(ex.Message);
            }
            
        }


        private void lObjBtnSearch_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                SelectMtxDataSource(lObjTxtSearch.Value);
            }
            catch (Exception ex)
            {
                LogService.WriteError(ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(ex.Message);
            }
          
        }


        private void pObjMtxCFL_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.Row > 0)
                {
                    mIntRow = pVal.Row > 0 ? pVal.Row : 0;
                    pObjMtxCFL.SelectRow(pVal.Row, true, false);
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError(ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowMessageBox(ex.Message);
            }
           
        }
        #endregion

        #region Methods
        private void LoadXmlForm(string pStrFrmName)
        {
            System.Xml.XmlDocument lObjXmlDoc = new System.Xml.XmlDocument();
            string lStrPath = PathUtilities.GetCurrent("ModalForms") + "\\" + pStrFrmName + ".xml";

            lObjXmlDoc.Load(lStrPath);

            SAPbouiCOM.FormCreationParams lObjCreationPackage =
                (SAPbouiCOM.FormCreationParams)Application.SBO_Application.CreateObject
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

                    lObjCFLModalForm = Application.SBO_Application.Forms.AddEx(lObjCreationPackage);
                    lObjCFLModalForm.Title = "Busqueda";
                    lObjCFLModalForm.Left = 400;
                    lObjCFLModalForm.Top = 100;
                    lObjCFLModalForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                    lObjCFLModalForm.Visible = true;
                    InitializeXmlForm();
                }
            }
            else
            {
                lObjCFLModalForm.Select();
            }
        }

        private void InitializeXmlForm()
        {
            lObjCFLModalForm.Freeze(true);
            SetItems();
            InitializeEvents();
            SelectMtxDataSource();

            lObjCFLModalForm.Freeze(false);
        }

        #region FillMtxs
        private void SelectMtxDataSource(string pStrSearch = "")
        {
            switch (mStrCFLType)
            {
                case "CFL_TownsA":
                    SetCFLTowns(pStrSearch);
                    break;
                case "CFL_TownsB":
                    SetCFLTowns(pStrSearch);
                    break;
                case "CFL_Items":
                    SetCFLItems(pStrSearch);
                    break;
                case "CFL_AF":
                    SetCFLAF(pStrSearch);
                    break;
                case "CFL_DR":
                    SetCFLDr(pStrSearch);
                    break;
                case "CFL_Folios":
                    SetCFLFolios();
                    break;
            }

            if (pObjMtxCFL.RowCount > 0)
            {
                mIntRow = 1;
                pObjMtxCFL.SelectRow(1, true, false);
            }
        }

        private void SetCFLFolios()
        {
            string ss = mObjTransportFactory.GetCFLService().GetCFLFoliosQuery();
            lObjCFLModalForm.DataSources.DataTables.Item("DsItems").ExecuteQuery(mObjTransportFactory.GetCFLService().GetCFLFoliosQuery());
            LoadFoliosMatrix();
        }

        private void SetCFLDr(string pStrSearch)
        {
            mStrCostingCode = mObjTransportFactory.GetCFLService().GetCostingCode(mIntUserSignature);

            lObjCFLModalForm.DataSources.DataTables.Item("DsItems").ExecuteQuery(mObjTransportFactory.GetCFLService().GetCFLDrivers(mStrCostingCode, pStrSearch));

            LoadDriversMatrix();
        }

        private void SetCFLAF(string pStrSearch)
        {
            mStrCostingCode = mObjTransportFactory.GetCFLService().GetCostingCode(mIntUserSignature);

            lObjCFLModalForm.DataSources.DataTables.Item("DsItems").ExecuteQuery(mObjTransportFactory.GetCFLService().GetCFLAFQuery(mStrCostingCode, mStrEquip, pStrSearch));
            LoadAFMatrix();
        }

        private void SetCFLItems(string pStrSearch)
        {
            mStrWHS = mObjTransportFactory.GetCFLService().GetWhs(mIntUserSignature);

            lObjCFLModalForm.DataSources.DataTables.Item("DsItems").ExecuteQuery(mObjTransportFactory.GetCFLService().GetCFLItemsQuery(mStrWHS,mStrCardCode, pStrSearch));
            LoadItemMatrix();
        }

        private void SetCFLTowns(string pStrSearch)
        {
            lObjCFLModalForm.DataSources.DataTables.Item("DsItems").ExecuteQuery(mObjTransportFactory.GetCFLService().GetCFLTownsQuery(pStrSearch));
            LoadTownMatrix();
        }

        private void LoadItemMatrix()
        {
            if (pObjMtxCFL.Columns.Count == 0)
            {
                //Add columns
                pObjMtxCFL.Columns.Add("cName", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                pObjMtxCFL.Columns.Add("cDesc", SAPbouiCOM.BoFormItemTypes.it_EDIT);

                //Setup clumns
                pObjMtxCFL.Columns.Item("cName").TitleObject.Caption = "Nombre";
                pObjMtxCFL.Columns.Item("cDesc").TitleObject.Caption = "Descripción";


                pObjMtxCFL.Columns.Item("cName").Editable = false;
                pObjMtxCFL.Columns.Item("cDesc").Editable = false;

                pObjMtxCFL.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            }
            //Bind
            pObjMtxCFL.Columns.Item("cName").DataBind.Bind("DsItems", "ItemCode");
            pObjMtxCFL.Columns.Item("cDesc").DataBind.Bind("DsItems", "ItemName");

            pObjMtxCFL.LoadFromDataSource();
            pObjMtxCFL.AutoResizeColumns();

            
        }

        private void LoadFoliosMatrix()
        {
            if (pObjMtxCFL.Columns.Count == 0)
            {
                //Add columns
                pObjMtxCFL.Columns.Add("cFolio", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                pObjMtxCFL.Columns.Add("cDocNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                pObjMtxCFL.Columns.Add("cStat", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                pObjMtxCFL.Columns.Add("cType", SAPbouiCOM.BoFormItemTypes.it_EDIT);

                //Setup clumns
                pObjMtxCFL.Columns.Item("cFolio").TitleObject.Caption = "Folio";
                pObjMtxCFL.Columns.Item("cDocNum").TitleObject.Caption = "Documento";
                pObjMtxCFL.Columns.Item("cStat").TitleObject.Caption = "Doc. estatus";
                pObjMtxCFL.Columns.Item("cType").TitleObject.Caption = "Tipo";

                pObjMtxCFL.Columns.Item("cFolio").Editable = false;
                pObjMtxCFL.Columns.Item("cDocNum").Editable = false;
                pObjMtxCFL.Columns.Item("cStat").Editable = false;
                pObjMtxCFL.Columns.Item("cType").Editable = false;

                pObjMtxCFL.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            }

            //Bind
            pObjMtxCFL.Columns.Item("cFolio").DataBind.Bind("DsItems", "U_GLO_Ticket");
            pObjMtxCFL.Columns.Item("cDocNum").DataBind.Bind("DsItems", "DocNum");
            pObjMtxCFL.Columns.Item("cStat").DataBind.Bind("DsItems", "Status");
            pObjMtxCFL.Columns.Item("cType").DataBind.Bind("DsItems", "Type");

            pObjMtxCFL.LoadFromDataSource();
            pObjMtxCFL.AutoResizeColumns();
        }

        private void LoadTownMatrix()
        {
            if (pObjMtxCFL.Columns.Count == 0)
            {
                //Add columns
                pObjMtxCFL.Columns.Add("cName", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                pObjMtxCFL.Columns.Add("cState", SAPbouiCOM.BoFormItemTypes.it_EDIT);
               // pObjMtxCFL.Columns.Add("cCom", SAPbouiCOM.BoFormItemTypes.it_EDIT);

                //Setup clumns
                pObjMtxCFL.Columns.Item("cName").TitleObject.Caption = "Nombre";
                pObjMtxCFL.Columns.Item("cState").TitleObject.Caption = "Estado";
                //pObjMtxCFL.Columns.Item("cCom").TitleObject.Caption = "Comite";

                pObjMtxCFL.Columns.Item("cName").Editable = false;
                pObjMtxCFL.Columns.Item("cState").Editable = false;
               // pObjMtxCFL.Columns.Item("cCom").Editable = false;

                pObjMtxCFL.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            }

            //Bind
            pObjMtxCFL.Columns.Item("cName").DataBind.Bind("DsItems", "Name");
            pObjMtxCFL.Columns.Item("cState").DataBind.Bind("DsItems", "U_State");
            //pObjMtxCFL.Columns.Item("cCom").DataBind.Bind("DsItems", "U_Commite");

            pObjMtxCFL.LoadFromDataSource();
            pObjMtxCFL.AutoResizeColumns();
        }

        private void LoadAFMatrix()
        {
            if (pObjMtxCFL.Columns.Count == 0)
            {
                pObjMtxCFL.Columns.Add("cName", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                pObjMtxCFL.Columns.Add("cItem", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                pObjMtxCFL.Columns.Add("cFrgN", SAPbouiCOM.BoFormItemTypes.it_EDIT);

                //Setup clumns
                pObjMtxCFL.Columns.Item("cName").TitleObject.Caption = "Nombre";
                pObjMtxCFL.Columns.Item("cItem").TitleObject.Caption = "Articulo";
                pObjMtxCFL.Columns.Item("cFrgN").TitleObject.Caption = "Nombre ext.";

                pObjMtxCFL.Columns.Item("cName").Editable = false;
                pObjMtxCFL.Columns.Item("cItem").Editable = false;
                pObjMtxCFL.Columns.Item("cFrgN").Editable = false;

                pObjMtxCFL.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            }
            //Bind
            pObjMtxCFL.Columns.Item("cName").DataBind.Bind("DsItems", "ItemName");
            pObjMtxCFL.Columns.Item("cItem").DataBind.Bind("DsItems", "PrcCode");
            pObjMtxCFL.Columns.Item("cFrgN").DataBind.Bind("DsItems", "FrgnName");

            pObjMtxCFL.LoadFromDataSource();
            pObjMtxCFL.AutoResizeColumns();
        }

        private void LoadDriversMatrix()
        {
            if (pObjMtxCFL.Columns.Count == 0)
            {
                pObjMtxCFL.Columns.Add("cName", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                pObjMtxCFL.Columns.Add("cSex", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                pObjMtxCFL.Columns.Add("cType", SAPbouiCOM.BoFormItemTypes.it_EDIT);

                //Setup clumns
                pObjMtxCFL.Columns.Item("cName").TitleObject.Caption = "Nombre";
                pObjMtxCFL.Columns.Item("cSex").TitleObject.Caption = "Sexo";
                pObjMtxCFL.Columns.Item("cType").TitleObject.Caption = "Tipo";

                pObjMtxCFL.Columns.Item("cName").Editable = false;
                pObjMtxCFL.Columns.Item("cSex").Editable = false;
                pObjMtxCFL.Columns.Item("cType").Editable = false;

                pObjMtxCFL.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            }

            //Bind
            pObjMtxCFL.Columns.Item("cName").DataBind.Bind("DsItems", "Name");
            pObjMtxCFL.Columns.Item("cSex").DataBind.Bind("DsItems", "sex");
            pObjMtxCFL.Columns.Item("cType").DataBind.Bind("DsItems", "type");

            pObjMtxCFL.LoadFromDataSource();
            pObjMtxCFL.AutoResizeColumns();
        }

        #endregion

        private void SetItems()
        {
            pObjBtnSelect = ((SAPbouiCOM.Button)lObjCFLModalForm.Items.Item("btnSelect").Specific);
            lObjBtnCancel = ((SAPbouiCOM.Button)lObjCFLModalForm.Items.Item("btnCancel").Specific);
            pObjMtxCFL = ((SAPbouiCOM.Matrix)lObjCFLModalForm.Items.Item("mtxItems").Specific);
            lObjBtnSearch = ((SAPbouiCOM.Button)lObjCFLModalForm.Items.Item("btnSrch").Specific);
            lObjTxtSearch = ((SAPbouiCOM.EditText)lObjCFLModalForm.Items.Item("txtSrch").Specific);

            lObjCFLModalForm.DataSources.DataTables.Add("DsItems");
        }

        public void CloseForm()
        {
            UnloadEvents();
            lObjCFLModalForm.Close();
        }

        private void InitializeEvents()
        {
            lObjBtnCancel.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnCancel_ClickBefore);
            lObjBtnSearch.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnSearch_ClickBefore);
            pObjMtxCFL.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.pObjMtxCFL_ClickBefore);

        }

        public void UnloadEvents()
        {
            lObjBtnCancel.ClickBefore -= new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnCancel_ClickBefore);
            lObjBtnSearch.ClickBefore -= new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnSearch_ClickBefore);
        }

        #endregion


    }
}
