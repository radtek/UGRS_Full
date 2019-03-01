using System;
using System.Collections.Generic;
using UGRS.AddOn.Finances.Utils;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.SDK.DI;

namespace UGRS.AddOn.Finances.Forms
{
    [FormAttribute("UGRS.AddOn.Finances.Forms.InvoiceTest", "Forms/InvoiceTest.b1f")]
    class InvoiceTest : UserFormBase
    {
        SAPbouiCOM.ChooseFromList mCFLClient;
        SAPbouiCOM.ChooseFromList mCFLPayMethod;
        SAPbouiCOM.ChooseFromList mCFLPayGroup;

        SAPbouiCOM.ChooseFromList mCFLItemCode;
        SAPbouiCOM.ChooseFromList mCFLItemTax;
        SAPbouiCOM.ChooseFromList mCFLItemWarehouse;
        SAPbouiCOM.ChooseFromList mCFLItemCosting;

        SAPbouiCOM.ChooseFromList mCFLEdocFormat;
        public InvoiceTest()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mTxtCardCode = ((SAPbouiCOM.StaticText)(this.GetItem("txtClient").Specific));
            this.mEdtCardCode = ((SAPbouiCOM.EditText)(this.GetItem("edtClient").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.mEdtDate = ((SAPbouiCOM.EditText)(this.GetItem("edtDate").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("txtDueDate").Specific));
            this.mEdtDueDate = ((SAPbouiCOM.EditText)(this.GetItem("edtDueDate").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("txtSeries").Specific));
            this.mEdtSeries = ((SAPbouiCOM.EditText)(this.GetItem("edtSeries").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("txtPayMthd").Specific));
            this.mEdtPaymentMethod = ((SAPbouiCOM.EditText)(this.GetItem("edtPayMthd").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("txtPayGrp").Specific));
            this.mEdtPaymentGroup = ((SAPbouiCOM.EditText)(this.GetItem("edtPayGrp").Specific));
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_12").Specific));
            this.StaticText7 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_13").Specific));
            this.StaticText8 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_14").Specific));
            this.StaticText9 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_15").Specific));
            this.StaticText10 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_16").Specific));
            this.mEdtItemCode = ((SAPbouiCOM.EditText)(this.GetItem("edtItmCode").Specific));
            this.mEdtItemQuantity = ((SAPbouiCOM.EditText)(this.GetItem("edtItmQty").Specific));
            this.mEdtItemPrice = ((SAPbouiCOM.EditText)(this.GetItem("edtItmPrc").Specific));
            this.mEdtItemTax = ((SAPbouiCOM.EditText)(this.GetItem("edtItmTax").Specific));
            this.mEdtItemWarehouse = ((SAPbouiCOM.EditText)(this.GetItem("edtItmWare").Specific));
            this.mEdtItemCosting = ((SAPbouiCOM.EditText)(this.GetItem("edtItmCst").Specific));
            this.StaticText11 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_23").Specific));
            this.StaticText13 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_26").Specific));
            this.mEdtEdocFormat = ((SAPbouiCOM.EditText)(this.GetItem("edtEForm").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_29").Specific));
            this.Button0.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.StaticText15 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_30").Specific));
            this.mCmbEdocType = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbEdocTyp").Specific));
            this.OnCustomInitialize();

        }


        private void LoadEvents()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void UnloadEvents()
        {
            Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            /*try
            {*/
            if (!FormUID.Equals(this.UIAPIRawForm.UniqueID))
            {
                return;
            }
            if (!pVal.BeforeAction)
            {
                switch (pVal.EventType)
                {
                    case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST:
                        if (pVal.Action_Success)
                        {
                            SAPbouiCOM.IChooseFromListEvent lObjCFLEvent = (SAPbouiCOM.IChooseFromListEvent)pVal;
                            if (lObjCFLEvent.SelectedObjects != null)
                            {
                                SAPbouiCOM.DataTable lObjDataTable = lObjCFLEvent.SelectedObjects;
                                // Client Selector
                                if (lObjDataTable.UniqueID == mEdtCardCode.ChooseFromListUID)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(0, 0));
                                }
                                // Payment Method
                                if (lObjDataTable.UniqueID == mEdtPaymentMethod.ChooseFromListUID)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(0, 0));
                                }
                                // Payment Group
                                if (lObjDataTable.UniqueID == mEdtPaymentGroup.ChooseFromListUID)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(0, 0));
                                }
                                // Item Code
                                if (lObjDataTable.UniqueID == mEdtItemCode.ChooseFromListUID)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(0, 0));
                                }
                                // Item Tax
                                if (lObjDataTable.UniqueID == mEdtItemTax.ChooseFromListUID)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(0, 0));
                                }
                                // Item Warehouse
                                if (lObjDataTable.UniqueID == mEdtItemWarehouse.ChooseFromListUID)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(0, 0));
                                }
                                // Item Costing Code
                                if (lObjDataTable.UniqueID == mEdtItemCosting.ChooseFromListUID)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(0, 0));
                                }
                                // EDocFormat
                                if (lObjDataTable.UniqueID == mEdtEdocFormat.ChooseFromListUID)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(0, 0));
                                }

                            }
                        }
                        break;
                    case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                        UnloadEvents();
                        break;
                }
            }
            else
            {
                switch (pVal.EventType)
                {

                }
            }
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.StaticText mTxtCardCode;

        private void OnCustomInitialize()
        {
            LoadEvents();
            mCFLClient = InitChooseFromLists(false, "2", "CFL_Client", this.UIAPIRawForm.ChooseFromLists);
            mEdtCardCode.DataBind.SetBound(true, "", mCFLClient.UniqueID);
            mEdtCardCode.ChooseFromListUID = mCFLClient.UniqueID;

            SAPbouiCOM.Conditions lObjCons = mCFLClient.GetConditions();
            SAPbouiCOM.Condition lObjCon = lObjCons.Add();
            lObjCon.Alias = "CardType";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "C";
            mCFLClient.SetConditions(lObjCons);

            mCFLPayMethod = InitChooseFromLists(false, "147", "CFL_PyMthd", this.UIAPIRawForm.ChooseFromLists);
            mEdtPaymentMethod.DataBind.SetBound(true, "", mCFLPayMethod.UniqueID);
            mEdtPaymentMethod.ChooseFromListUID = mCFLPayMethod.UniqueID;

            mCFLPayGroup = InitChooseFromLists(false, "40", "CFL_PayGrp", this.UIAPIRawForm.ChooseFromLists);
            mEdtPaymentGroup.DataBind.SetBound(true, "", mCFLPayGroup.UniqueID);
            mEdtPaymentGroup.ChooseFromListUID = mCFLPayGroup.UniqueID;

            mCFLItemCode = InitChooseFromLists(false, "4", "CFL_ItmCod", this.UIAPIRawForm.ChooseFromLists);
            mEdtItemCode.DataBind.SetBound(true, "", mCFLItemCode.UniqueID);
            mEdtItemCode.ChooseFromListUID = mCFLItemCode.UniqueID;

            lObjCons = mCFLItemCode.GetConditions();
            lObjCon = lObjCons.Add();
            lObjCon.Alias = "SellItem";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "Y";
            mCFLItemCode.SetConditions(lObjCons);

            mCFLItemTax = InitChooseFromLists(false, "128", "CFL_ItmTax", this.UIAPIRawForm.ChooseFromLists);
            mEdtItemTax.DataBind.SetBound(true, "", mCFLItemTax.UniqueID);
            mEdtItemTax.ChooseFromListUID = mCFLItemTax.UniqueID;

            lObjCons = mCFLItemTax.GetConditions();
            lObjCon = lObjCons.Add();
            lObjCon.Alias = "ValidForAR";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "Y";
            mCFLItemTax.SetConditions(lObjCons);

            mCFLItemWarehouse = InitChooseFromLists(false, "64", "CFL_ItmWhs", this.UIAPIRawForm.ChooseFromLists);
            mEdtItemWarehouse.DataBind.SetBound(true, "", mCFLItemWarehouse.UniqueID);
            mEdtItemWarehouse.ChooseFromListUID = mCFLItemWarehouse.UniqueID;

            mCFLItemCosting = InitChooseFromLists(false, "62", "CFL_ItmOcr", this.UIAPIRawForm.ChooseFromLists);
            mEdtItemCosting.DataBind.SetBound(true, "", mCFLItemCosting.UniqueID);
            mEdtItemCosting.ChooseFromListUID = mCFLItemCosting.UniqueID;

            mCFLEdocFormat = InitChooseFromLists(false, "410000005", "CFL_EForm", this.UIAPIRawForm.ChooseFromLists);
            mEdtEdocFormat.DataBind.SetBound(true, "", mCFLEdocFormat.UniqueID);
            mEdtEdocFormat.ChooseFromListUID = mCFLEdocFormat.UniqueID;

            lObjCons = mCFLEdocFormat.GetConditions();
            lObjCon = lObjCons.Add();
            lObjCon.Alias = "Type";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "CFDD";
            mCFLEdocFormat.SetConditions(lObjCons);
        }

        private SAPbouiCOM.EditText mEdtCardCode;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText mEdtDate;

        public SAPbouiCOM.ChooseFromList InitChooseFromLists(bool pbolMultiselecction, string pStrType, string pStrID, SAPbouiCOM.ChooseFromListCollection pObjCFLs)
        {
            SAPbouiCOM.ChooseFromList lObjoCFL = null;
            try
            {
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = (SAPbouiCOM.ChooseFromListCreationParams)UIApplication.GetApplication().CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams);

                oCFLCreationParams.MultiSelection = pbolMultiselecction;
                oCFLCreationParams.ObjectType = pStrType;
                oCFLCreationParams.UniqueID = pStrID;

                lObjoCFL = pObjCFLs.Add(oCFLCreationParams);

                this.UIAPIRawForm.DataSources.UserDataSources.Add(pStrID, SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 254);
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitCustomerChooseFromListException: {0}", ex.Message));
                LogService.WriteError("(InitChooseFromLists): " + ex.Message);
                LogService.WriteError(ex);
            }
            return lObjoCFL;
        }

        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.EditText mEdtDueDate;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.EditText mEdtSeries;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.EditText mEdtPaymentMethod;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.EditText mEdtPaymentGroup;
        private SAPbouiCOM.StaticText StaticText6;
        private SAPbouiCOM.StaticText StaticText7;
        private SAPbouiCOM.StaticText StaticText8;
        private SAPbouiCOM.StaticText StaticText9;
        private SAPbouiCOM.StaticText StaticText10;
        private SAPbouiCOM.EditText mEdtItemCode;
        private SAPbouiCOM.EditText mEdtItemQuantity;
        private SAPbouiCOM.EditText mEdtItemPrice;
        private SAPbouiCOM.EditText mEdtItemTax;
        private SAPbouiCOM.EditText mEdtItemWarehouse;
        private SAPbouiCOM.EditText mEdtItemCosting;
        private SAPbouiCOM.StaticText StaticText11;
        private SAPbouiCOM.StaticText StaticText13;
        private SAPbouiCOM.EditText mEdtEdocFormat;
        private SAPbouiCOM.Button Button0;

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                //SAPbobsCOM.Documents lObjInvoice2 = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                //lObjInvoice2.DocDate = DateTime.Now;
                //lObjInvoice2.DocDueDate = DateTime.Now;
                //lObjInvoice2.CardCode = "CL00000001";
                //lObjInvoice2.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;
                //lObjInvoice2.Lines.SetCurrentLine(0);
                //lObjInvoice2.Lines.ItemCode = "A00000001";
                //lObjInvoice2.Lines.Quantity = 1;
                //lObjInvoice2.Lines.UnitPrice = 10;
                //lObjInvoice2.Lines.TaxCode = "V0";
                //lObjInvoice2.Lines.AccountCode = "4010020002000";
                //lObjInvoice2.Lines.WarehouseCode = "CRHE";
                //lObjInvoice2.Lines.Add();


                ////lObjInvoice2.GetByKey(191);
                ////string lStrXmlOriginalInvoice = lObjInvoice2.GetAsXML();
                ////lStrXmlOriginalInvoice = "";
                //////Intialize a new invoice through your xml
                ////DIApplication.Company.XmlExportType = SAPbobsCOM.BoXmlExportTypes.xet_ExportImportMode;
                ////DIApplication.Company.XMLAsString = true;
                ////lObjInvoice2 = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObjectFromXML(lStrXmlOriginalInvoice, 0);

                //if (lObjInvoice2.Add() != 0)
                //{
                //    string lStrLastError = DIApplication.Company.GetLastErrorDescription();
                //}
                //else
                //{
                //    int lIntDocEntry = int.Parse(DIApplication.Company.GetNewObjectKey());
                //}


                if (!(sboObject as SAPbouiCOM.Button).Item.Enabled)
                {
                    return;
                }
                (sboObject as SAPbouiCOM.Button).Item.Enabled = false;
                SAPbobsCOM.Documents lObjInvoice = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                lObjInvoice.CardCode = mEdtCardCode.Value;
                lObjInvoice.DocObjectCode = SAPbobsCOM.BoObjectTypes.oInvoices;
               
                if (mEdtDate.Value != "")
                {
                    lObjInvoice.DocDate = this.GetDataSourceValue<DateTime>("UD_Date");
                }
                if (mEdtDueDate.Value != "")
                {
                    lObjInvoice.DocDueDate = this.GetDataSourceValue<DateTime>("UD_DueDate");
                }
                lObjInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;
                if (mEdtSeries.Value != "")
                {
                    lObjInvoice.Series = Convert.ToInt32(mEdtSeries.Value);
                }
                if (mEdtPaymentMethod.Value != "")
                {
                    lObjInvoice.PaymentMethod = mEdtPaymentMethod.Value;
                }
                if (mEdtPaymentGroup.Value != "")
                {
                    lObjInvoice.PaymentGroupCode = Convert.ToInt32(mEdtPaymentGroup.Value);
                }
                lObjInvoice.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = "P01";

                lObjInvoice.Lines.SetCurrentLine(0);
                lObjInvoice.Lines.ItemCode = mEdtItemCode.Value;
                lObjInvoice.Lines.Quantity = this.GetDataSourceValue<int>("UD_Qty");
                lObjInvoice.Lines.UnitPrice = this.GetDataSourceValue<double>("UD_Price");
                lObjInvoice.Lines.TaxCode = mEdtItemTax.Value;
                if (mEdtItemWarehouse.Value != "")
                {
                    lObjInvoice.Lines.WarehouseCode = mEdtItemWarehouse.Value;
                }
                if (mEdtItemCosting.Value != "")
                {
                    //lObjInvoice.Lines.COGSCostingCode = mEdtItemCosting.Value;
                    lObjInvoice.Lines.CostingCode = mEdtItemCosting.Value;
                }
                lObjInvoice.Lines.Add();
                if (mCmbEdocType.Value != "")
                {
                    lObjInvoice.EDocGenerationType = SAPbobsCOM.EDocGenerationTypeEnum.edocGenerate; //(SAPbobsCOM.EDocGenerationTypeEnum)Convert.ToInt32(mCmbEdocType.Value);
                }
                if (mEdtEdocFormat.Value != "")
                {
                    lObjInvoice.EDocExportFormat = Convert.ToInt32(mEdtEdocFormat.Value);
                }

                int lLongErr = lObjInvoice.Add();
                string lStrErrMsg;
                if (lLongErr != 0)
                {
                    DIApplication.Company.GetLastError(out lLongErr, out lStrErrMsg);
                    UIApplication.ShowError(lStrErrMsg);
                }
                else
                {
                    string lStrLastDoc = DIApplication.Company.GetNewObjectKey().ToString();
                    UIApplication.ShowSuccess(String.Format("Documento {0} creado", lStrLastDoc));
                }
                (sboObject as SAPbouiCOM.Button).Item.Enabled = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                (sboObject as SAPbouiCOM.Button).Item.Enabled = true;
            }
        }

        private SAPbouiCOM.StaticText StaticText15;
        private SAPbouiCOM.ComboBox mCmbEdocType;
    }
}
