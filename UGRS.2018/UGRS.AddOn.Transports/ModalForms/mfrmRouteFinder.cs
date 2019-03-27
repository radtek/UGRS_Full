using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Transports;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.SDK.DI.Transports.Utility;
using UGRS.Core.Utility;

namespace UGRS.AddOn.Transports.ModalForms
{
    public class mfrmRouteFinder
    {
        #region SAP items
        SAPbouiCOM.Form lObjModalForm;
        SAPbouiCOM.EditText lObjTxtRouteName;
        SAPbouiCOM.EditText lObjTxtOrign;
        SAPbouiCOM.EditText lObjTxtMOrign;
        SAPbouiCOM.EditText lObjTxtDestiny;
        SAPbouiCOM.EditText lObjTxtMDestiny;
        SAPbouiCOM.Button lObjBtnSearch;
        public SAPbouiCOM.Button pObjBtnSelect;
        SAPbouiCOM.Button lObjBtnCancel;
        SAPbouiCOM.Button lObjBtnNew;
        public SAPbouiCOM.Matrix pObjMtxRoutes;
        SAPbouiCOM.DataTable lObjDtRoutes;

        #endregion

        TransportServiceFactory mObjTransportFactory = new TransportServiceFactory();
        Utils lObjUtility = new Utils();

        #region Attributes
        public int pIntRow = 0;
        public int pIntCode = 0;
        public string mStrFrmName = string.Empty;

        public SalesOrderLinesDTO pRoutes = null;
        #endregion

        #region Constructor
        public mfrmRouteFinder(string pStrFrmName)
        {
            mStrFrmName = pStrFrmName;
            LoadXmlForm(pStrFrmName);
        }


        #endregion

        #region Events
        private void lObjBtnSearch_ClickBefore(object sboObjct, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            SearchByFilters();
        }

        //private void lObjBtnSelect_ClickBefore(object sboObjct, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        //{
        //    BubbleEvent = true;
        //}

        private void lObjBtnCancel_ClickBefore(object sboObjct, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            CloseForm();
        }

        private void lObjBtnNew_ClickBefore(object sboObjct, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void pObjMtxRoutes_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.Row > 0)
            {
                SetRoutes(pVal.Row);
                pObjMtxRoutes.SelectRow(pVal.Row, true, false);
                //pIntCode = (int)(lObjDtRoutes.Columns.Item("Code").Cells.Item(pVal.Row - 1).Value);
                //pIntRow = pVal.Row;
            }
            else
            {
                pRoutes = null;
                //pIntCode = 0;
                //pIntRow = 0;
            }
        }

        private void SetRoutes(int pIntRow)
        {
            pRoutes = new SalesOrderLinesDTO();

            pRoutes.Route = (int)(lObjDtRoutes.Columns.Item("Code").Cells.Item(pIntRow - 1).Value);

            pRoutes.RouteName = ((SAPbouiCOM.EditText)pObjMtxRoutes.Columns.Item("cName").Cells.Item(pIntRow).Specific).Value.ToString();
            pRoutes.Origin = ((SAPbouiCOM.EditText)pObjMtxRoutes.Columns.Item("cOrign").Cells.Item(pIntRow).Specific).Value.ToString();
            pRoutes.MOrigin = ((SAPbouiCOM.EditText)pObjMtxRoutes.Columns.Item("cMOrgn").Cells.Item(pIntRow).Specific).Value.ToString();
            pRoutes.Destination = ((SAPbouiCOM.EditText)pObjMtxRoutes.Columns.Item("cDest").Cells.Item(pIntRow).Specific).Value.ToString();
            pRoutes.MDestination = ((SAPbouiCOM.EditText)pObjMtxRoutes.Columns.Item("cMDest").Cells.Item(pIntRow).Specific).Value.ToString();

            pRoutes.KmA = ((SAPbouiCOM.EditText)pObjMtxRoutes.Columns.Item("cKmA").Cells.Item(pIntRow).Specific).Value.ToString();
            pRoutes.KmB = ((SAPbouiCOM.EditText)pObjMtxRoutes.Columns.Item("cKmB").Cells.Item(pIntRow).Specific).Value.ToString();
            pRoutes.KmC = ((SAPbouiCOM.EditText)pObjMtxRoutes.Columns.Item("cKmC").Cells.Item(pIntRow).Specific).Value.ToString();
            pRoutes.KmD = ((SAPbouiCOM.EditText)pObjMtxRoutes.Columns.Item("cKmD").Cells.Item(pIntRow).Specific).Value.ToString();
            pRoutes.KmE = ((SAPbouiCOM.EditText)pObjMtxRoutes.Columns.Item("cKmE").Cells.Item(pIntRow).Specific).Value.ToString();
            pRoutes.KmF = ((SAPbouiCOM.EditText)pObjMtxRoutes.Columns.Item("cKmF").Cells.Item(pIntRow).Specific).Value.ToString();
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (FormUID.Equals(this.lObjModalForm.UniqueID))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                UnloadEvents();
                                break;
                            //case SAPbouiCOM.BoEventTypes.et_CLICK:
                            //    if (pVal.ItemUID.Equals(pObjMtxRoutes.Item.UniqueID))
                            //    {
                            //        //string d = lObjDtRoutes.Columns.Item("U_Origen").Cells.Item(pVal.Row-1).Value -1
                            //        pIntCode = (int)(lObjDtRoutes.Columns.Item("Code").Cells.Item(pVal.Row - 1).Value);
                            //        pIntRow = pVal.Row;
                            //    }
                            //    break;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(string.Format("ItemEventException: {0}", ex.Message));
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

                    lObjModalForm = Application.SBO_Application.Forms.AddEx(lObjCreationPackage);
                    lObjModalForm.Title = "Busqueda de rutas";
                    lObjModalForm.Left = 400;
                    lObjModalForm.Top = 100;
                    lObjModalForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                    lObjModalForm.Visible = true;
                    InitializeXmlForm();
                }
            }
            else
            {
                lObjModalForm.Select();
            }


        }

        private void InitializeXmlForm()
        {
            lObjModalForm.Freeze(true);
            SetItems();
            InitializeEvents();
            InitialForm();
            lObjModalForm.Freeze(false);
        }

        private void InitialForm()
        {
            lObjTxtDestiny.Value = string.Empty;
            lObjTxtMDestiny.Value = string.Empty;
            lObjTxtMOrign.Value = string.Empty;
            lObjTxtOrign.Value = string.Empty;

            FillDataSource();
            LoadRoutesMatrix();
        }

        private void LoadRoutesMatrix()
        {

            pObjMtxRoutes.Columns.Item("cName").DataBind.Bind("DsRoutes", "Name");
            pObjMtxRoutes.Columns.Item("cOrign").DataBind.Bind("DsRoutes", "U_Origen");
            pObjMtxRoutes.Columns.Item("cMOrgn").DataBind.Bind("DsRoutes", "U_TR_TOWNORIG");
            pObjMtxRoutes.Columns.Item("cDest").DataBind.Bind("DsRoutes", "U_Destino");
            pObjMtxRoutes.Columns.Item("cMDest").DataBind.Bind("DsRoutes", "U_TR_TOWNDES");
            pObjMtxRoutes.Columns.Item("cKmA").DataBind.Bind("DsRoutes", "U_TypeA");
            pObjMtxRoutes.Columns.Item("cKmB").DataBind.Bind("DsRoutes", "U_TypeB");
            pObjMtxRoutes.Columns.Item("cKmC").DataBind.Bind("DsRoutes", "U_TypeC");
            pObjMtxRoutes.Columns.Item("cKmD").DataBind.Bind("DsRoutes", "U_TypeD");
            pObjMtxRoutes.Columns.Item("cKmE").DataBind.Bind("DsRoutes", "U_TypeE");
            pObjMtxRoutes.Columns.Item("cKmF").DataBind.Bind("DsRoutes", "U_TypeF");

            lObjDtRoutes = lObjModalForm.DataSources.DataTables.Item("DsRoutes");

            pObjMtxRoutes.AutoResizeColumns();
            pObjMtxRoutes.LoadFromDataSource();

        }

        private void FillDataSource(bool lBoolFiltered = false)
        {
            if (lBoolFiltered)
            {
                lObjModalForm.DataSources.DataTables.Item("DsRoutes").ExecuteQuery(mObjTransportFactory.GetRouteService()
                    .GetRoutesByFiltersQuery(lObjTxtOrign.Value, lObjTxtDestiny.Value, lObjTxtMOrign.Value, lObjTxtMDestiny.Value));
            }
            else
            {
                lObjModalForm.DataSources.DataTables.Item("DsRoutes").ExecuteQuery(mObjTransportFactory.GetRouteService().GetRouteQuery());
            }


        }

        private void SetItems()
        {
            lObjTxtOrign = ((SAPbouiCOM.EditText)lObjModalForm.Items.Item("txtOrign").Specific);
            lObjTxtMOrign = ((SAPbouiCOM.EditText)lObjModalForm.Items.Item("txtMOrign").Specific);
            lObjTxtDestiny = ((SAPbouiCOM.EditText)lObjModalForm.Items.Item("txtDest").Specific);
            lObjTxtMDestiny = ((SAPbouiCOM.EditText)lObjModalForm.Items.Item("txtMDest").Specific);
            pObjMtxRoutes = ((SAPbouiCOM.Matrix)lObjModalForm.Items.Item("mtxRoutes").Specific);
            lObjBtnSearch = ((SAPbouiCOM.Button)lObjModalForm.Items.Item("btnSearch").Specific);
            pObjBtnSelect = ((SAPbouiCOM.Button)lObjModalForm.Items.Item("btnSelect").Specific);
            lObjBtnCancel = ((SAPbouiCOM.Button)lObjModalForm.Items.Item("btnCancel").Specific);
            lObjBtnNew = ((SAPbouiCOM.Button)lObjModalForm.Items.Item("btnNew").Specific);
            lObjModalForm.DataSources.DataTables.Add("DsRoutes");
        }

        private void InitializeEvents()
        {
            lObjBtnSearch.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnSearch_ClickBefore);
            pObjMtxRoutes.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.pObjMtxRoutes_ClickBefore);
            lObjBtnCancel.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnCancel_ClickBefore);
            lObjBtnNew.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnNew_ClickBefore);
            //Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }



        //private bool FormExists(string pStrFrmName)
        //{
        //    try
        //    {
        //        Application.SBO_Application.Forms.Item(pStrFrmName);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        private void SearchByFilters()
        {
            if (!string.IsNullOrEmpty(lObjTxtOrign.Value) || !string.IsNullOrEmpty(lObjTxtDestiny.Value) || !string.IsNullOrEmpty(lObjTxtMOrign.Value) || !string.IsNullOrEmpty(lObjTxtMDestiny.Value))
            {
                FillDataSource(true);
                LoadRoutesMatrix();
            }

        }

        public void CloseForm()
        {
            UnloadEvents();
            lObjModalForm.Close();
        }

        public void UnloadEvents()
        {
            lObjBtnSearch.ClickBefore -= new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnSearch_ClickBefore);
            lObjBtnCancel.ClickBefore -= new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnCancel_ClickBefore);
            lObjBtnNew.ClickBefore -= new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnNew_ClickBefore);
            //Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }
        #endregion

    }
}
