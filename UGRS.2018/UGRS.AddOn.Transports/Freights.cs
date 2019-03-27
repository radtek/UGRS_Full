using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.Transports;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.SDK.DI.Transports.Utility;

namespace UGRS.AddOn.Transports
{
    public class Freights
    {
        SAPbouiCOM.Form lObjSOForm;
        SAPbouiCOM.Form lObjUFForm;
        SAPbouiCOM.Form lObjRetentions;
        SAPbouiCOM.Item lObjItm;
        SAPbouiCOM.Button lObjBtnFreight;
        SAPbouiCOM.EditText lObjTxtCardCode;
        SAPbouiCOM.ComboBox lObjCboDoc;

        SAPbouiCOM.Matrix lObjMtxSO;
        SAPbouiCOM.EditText lObjTxtItem;

        Utils mObjUtility = new Utils();
        TransportServiceFactory mObjTransportService = new TransportServiceFactory();
        Forms.frmFreights mFrmFreights = null;

        private SalesOrderLinesDTO mObjSalesOrderLines = null;
        private FreightsParamsDTO mObjFreightsParams = null;
        private int mIntUserSignature = DIApplication.Company.UserSignature;
        private int mIntFormType = 0;
        private string mStrCardCode = string.Empty;

        bool mBoolInsurance = false;
        bool mBoolFreightsModal = false;

        #region Constructor
        public Freights()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void SetFormParameters(int pIntFormType, bool pBoolInsurance, bool pBoolInternal, bool pBoolLoaded)
        {
            mObjFreightsParams = new FreightsParamsDTO();

            mObjFreightsParams.UserSign = mIntUserSignature;
            mObjFreightsParams.FormType = pIntFormType;
            mObjFreightsParams.Insurance = pBoolInsurance;
            mObjFreightsParams.Internal = pBoolInternal;
            mObjFreightsParams.Loaded = pBoolLoaded;
            mObjFreightsParams.SalesOrderLines = mObjSalesOrderLines;
            mObjFreightsParams.CardCode = mStrCardCode;
        }
        #endregion

        #region Methods
        private void UnloadEvents()
        {
            try
            {

                //SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
                lObjBtnFreight.ClickBefore -= new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnFreight_ClickBefore);
                this.lObjTxtCardCode.LostFocusAfter -= lObjTxtCardCode_LostFocusAfter;
            }
            catch (Exception ex)
            {
                //Ignore

            }
        }

        private void SetFormSettings(ItemEvent pVal)
        {
            lObjSOForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);
            mIntFormType = pVal.FormType;
            lObjTxtCardCode = (SAPbouiCOM.EditText)lObjSOForm.Items.Item("4").Specific;
            this.lObjTxtCardCode.LostFocusAfter += lObjTxtCardCode_LostFocusAfter;
            lObjMtxSO = (SAPbouiCOM.Matrix)lObjSOForm.Items.Item("38").Specific;
            lObjCboDoc = (SAPbouiCOM.ComboBox)lObjSOForm.Items.Item("81").Specific;

        }

        private void AddButton()
        {
            if (!mObjUtility.ItemExist("btnFrgh", lObjSOForm))
            {
                SAPbouiCOM.Item txtUUID = lObjSOForm.Items.Item("70");
                lObjItm = lObjSOForm.Items.Add("btnFrgh", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                lObjItm.FromPane = 1;
                lObjItm.ToPane = 1;
                lObjItm.Top = txtUUID.Top + 25;
                lObjItm.Left = txtUUID.Left;

                lObjBtnFreight = ((SAPbouiCOM.Button)lObjSOForm.Items.Item("btnFrgh").Specific);
                lObjBtnFreight.Caption = "Fletes";
                lObjBtnFreight.Item.Enabled = false;
                lObjBtnFreight.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.lObjBtnFreight_ClickBefore);

            }
        }

        private bool EmptyOrder()
        {
            if (lObjMtxSO.RowCount >= 1)
            {
                if (CheckMtxItems() || !string.IsNullOrEmpty(lObjTxtItem.Value))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckMtxItems()
        {
            mObjSalesOrderLines = null;
            for (int i = 1; i <= lObjMtxSO.RowCount; i++)
            {
                lObjTxtItem = (SAPbouiCOM.EditText)lObjMtxSO.Columns.Item("1").Cells.Item(i).Specific;
                if (!string.IsNullOrEmpty(lObjTxtItem.Value) && TransportsItem(lObjTxtItem.Value))
                {
                    SetSalesOrderLine(i);
                }
                else if (Insuranceline(lObjTxtItem.Value))
                {
                    mBoolInsurance = true;
                }
            }
            return mObjSalesOrderLines != null || mBoolInsurance ? true : false;
        }

        private bool Insuranceline(string pStrItemCode)
        {
            return mObjTransportService.GetRouteService().CheckInsuranceLine(pStrItemCode);
        }

        private bool TransportsItem(string pStrItemCode)
        {
            return mObjTransportService.GetRouteService().CheckTransportsItem(pStrItemCode, mIntUserSignature);
        }

        private void SetSalesOrderLine(int pIntRow)
        {
            mObjSalesOrderLines = new SalesOrderLinesDTO();

            mObjSalesOrderLines.ItemCode = ((SAPbouiCOM.EditText)lObjMtxSO.Columns.Item("1").Cells.Item(pIntRow).Specific).Value;
            mObjSalesOrderLines.Description = ((SAPbouiCOM.EditText)lObjMtxSO.Columns.Item("3").Cells.Item(pIntRow).Specific).Value;
            mObjSalesOrderLines.Folio = ((SAPbouiCOM.EditText)lObjUFForm.Items.Item("U_GLO_Ticket").Specific).Value;
            mObjSalesOrderLines.Shared = mObjTransportService.GetRouteService().CheckIfShared(mObjSalesOrderLines.Folio) ? true : false;
            mObjSalesOrderLines.PayloadType = ((SAPbouiCOM.ComboBox)lObjMtxSO.Columns.Item("U_TR_LoadType").Cells.Item(pIntRow).Specific).Value;
            mObjSalesOrderLines.VehicleType = ((SAPbouiCOM.ComboBox)lObjMtxSO.Columns.Item("U_TR_VehicleType").Cells.Item(pIntRow).Specific).Value;
            mObjSalesOrderLines.Route = Convert.ToInt32(((SAPbouiCOM.EditText)lObjMtxSO.Columns.Item("U_TR_Paths").Cells.Item(pIntRow).Specific).Value);
            mObjSalesOrderLines.Employee = ((SAPbouiCOM.ComboBox)lObjMtxSO.Columns.Item("27").Cells.Item(pIntRow).Specific).Selected.Description;
            mObjSalesOrderLines.Asset = ((SAPbouiCOM.EditText)lObjMtxSO.Columns.Item("2003").Cells.Item(pIntRow).Specific).Value;
            mObjSalesOrderLines.TotKm = ((SAPbouiCOM.EditText)lObjMtxSO.Columns.Item("U_TR_TotKm").Cells.Item(pIntRow).Specific).Value;
            mObjSalesOrderLines.Extra = ((SAPbouiCOM.EditText)lObjMtxSO.Columns.Item("U_TR_AdditionalExpen").Cells.Item(pIntRow).Specific).Value;
        }

        private void OpenFreightsForm()
        {
            if (!string.IsNullOrEmpty(lObjTxtCardCode.Value) && (lObjItm.Specific as SAPbouiCOM.Button).Item.Enabled)
            {
                mStrCardCode = lObjTxtCardCode.Value;
                SetFormParameters(mIntFormType, false, false, false);
                if (!mObjUtility.FormExists("freights"))
                {
                    mFrmFreights = new Forms.frmFreights(mObjFreightsParams);
                    mFrmFreights.Show();
                    mBoolFreightsModal = mFrmFreights.pBoolFreightsModal;
                }
            }
        }

        private void LoadFreightsForm()
        {
            if (!string.IsNullOrEmpty(lObjTxtCardCode.Value) && (lObjItm.Specific as SAPbouiCOM.Button).Item.Enabled)
            {
                SetFormParameters(mIntFormType, mBoolInsurance, false, true);
                if (!mObjUtility.FormExists("freights"))
                {
                    mFrmFreights = new Forms.frmFreights(mObjFreightsParams);
                    mFrmFreights.Show();
                    mBoolFreightsModal = mFrmFreights.pBoolFreightsModal;
                }
            }
        }

        #endregion

        #region Events
        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            #region AddButton In Sales Order
            if (pVal.FormType == 139 || pVal.FormType == 133 || pVal.FormType == 149)
            {
                if (!pVal.BeforeAction)
                {
                    switch (pVal.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_FORM_LOAD:
                            SetFormSettings(pVal);
                            AddButton();
                            break;

                        case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                            UnloadEvents();
                            lObjUFForm = null;
                            lObjRetentions = null;
                            break;
                    }
                }
            }
            else if (pVal.FormType == -139 || pVal.FormType == -133 || pVal.FormType == -149)
            {
                if (lObjUFForm == null)
                {
                    switch (pVal.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_FORM_LOAD:
                            lObjUFForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);
                            break;
                    }
                }
            }
            else if (pVal.FormType == 60504)
            {
                if (lObjRetentions == null)
                {
                    switch (pVal.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_FORM_LOAD:
                            lObjRetentions = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);
                            break;
                    }
                }
            }
            #endregion
        }

        void lObjTxtCardCode_LostFocusAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (!string.IsNullOrEmpty(lObjTxtCardCode.Value) && !lObjCboDoc.Item.Description.Equals("Cerrado"))
            {
                (lObjItm.Specific as SAPbouiCOM.Button).Item.Enabled = true;
            }
            else
            {
                (lObjItm.Specific as SAPbouiCOM.Button).Item.Enabled = false;
            }
        }

        private void lObjBtnFreight_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (EmptyOrder())
            {
                OpenFreightsForm();
            }
            else
            {
                LoadFreightsForm();
            }
        }

        #endregion
    }
}
