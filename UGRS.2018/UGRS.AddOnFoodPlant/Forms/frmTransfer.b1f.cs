/*
 * Autor: LCC Abraham SaÚL Sandoval Meneses
 * Description: Transfer of Transit Inventory to Area Form
 * Date: 31/08/2018
 */


using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGRS.AddOnFoodPlant.Utils;
using UGRS.Core.SDK.DI.FoodPlant.DAO;
using UGRS.Core.SDK.DI.FoodPlant.DTO;
using UGRS.Core.SDK.DI.FoodPlant.Services;
using UGRS.Core.SDK.UI;
using System;

namespace UGRS.AddOnFoodPlant.Forms {
    [FormAttribute("UGRS.AddOnFoodPlant.Forms.frmTransfer", "Forms/frmTransfer.b1f")]
    class frmTransfer : UserFormBase {

        #region Properties
        int selectedRow = 0;
        Dictionary<string, BoFieldsType> columns0;
        Dictionary<string, BoFieldsType> columns1;
        FoodPlantDAO foodPlantDAO = new FoodPlantDAO();
        PendingTransfer[] pendingTransfers;
        TransferItem[] transferItems;
        #endregion

        #region Constructor
        public frmTransfer() {

            //whs = foodPlantDAO.GetUserDefaultWarehouse();
            Task.Factory.StartNew(FillMatrix0);
            Task.Run(() => PrepareMatrix1());
        }
        #endregion

        #region Matrix
        public void FillMatrix0() {
            columns0 = new Dictionary<string, BoFieldsType>() { { "#", BoFieldsType.ft_ShortNumber }, { "Folio", BoFieldsType.ft_ShortNumber }, { "DocDate", BoFieldsType.ft_AlphaNumeric }, { "Comments", BoFieldsType.ft_AlphaNumeric } };
            dt0 = SAPMatrix.CreateDataTable("DT0", columns0, this);
            pendingTransfers = foodPlantDAO.GetPendingTransfers();
            this.UIAPIRawForm.Freeze(true);
            SAPMatrix.Fill("DT0", dt0, mtx0, columns0.Keys.ToList(), pendingTransfers);
            this.UIAPIRawForm.Freeze(false);
        }

        public void PrepareMatrix1() {
            columns1 = new Dictionary<string, BoFieldsType>() { { "#", BoFieldsType.ft_ShortNumber }, { "Item", BoFieldsType.ft_AlphaNumeric }, { "Desc", BoFieldsType.ft_AlphaNumeric }, { "Quantity", BoFieldsType.ft_AlphaNumeric }, { "Bags", BoFieldsType.ft_Float } };
            dt1 = SAPMatrix.CreateDataTable("DT1", columns1, this);
            this.UIAPIRawForm.Freeze(true);
            mtx1.AutoResizeColumns();
            this.UIAPIRawForm.Freeze(false);
        }
        #endregion

        #region Events
        public override void OnInitializeComponent() {
            this.mtx0 = ((SAPbouiCOM.Matrix)(this.GetItem("mtx0").Specific));
            this.mtx0.ClickAfter += new SAPbouiCOM._IMatrixEvents_ClickAfterEventHandler(this.mtx0_ClickAfter);
            this.mtx1 = ((SAPbouiCOM.Matrix)(this.GetItem("mtx1").Specific));
            this.btnAccept = ((SAPbouiCOM.Button)(this.GetItem("btnAccept").Specific));
            this.btnReturn = ((SAPbouiCOM.Button)(this.GetItem("btnReturn").Specific));
            this.lblComment = ((SAPbouiCOM.StaticText)(this.GetItem("lblComment").Specific));
            this.txtComment = ((SAPbouiCOM.EditText)(this.GetItem("txtComment").Specific));
            this.btnAccept.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAccept_ClickBefore);
            this.btnReturn.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnReturn_ClickBefore);
            this.OnCustomInitialize();
        }

        public override void OnInitializeFormEvents() { }

        private void OnCustomInitialize() { }

        private void btnAccept_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent) {
            BubbleEvent = true;
            CreatePlantTransfer(false);
        }

        private void btnReturn_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent) {
            BubbleEvent = true;
            CreatePlantTransfer(false);
           

        }

        private void CreatePlantTransfer(bool switchWhs) {

            var transferDocument = new DocumentTransfer();
            transferDocument.Document = pendingTransfers[selectedRow - 1];
            transferDocument.Lines = transferItems;

            if(!String.IsNullOrEmpty(txtComment.Value)) {
                transferDocument.Document.Comments = txtComment.Value;
            }

            UIApplication.ShowMessageBox(StockTransferDI.TransferPlant(transferDocument, switchWhs).Message);
        }

        private void mtx0_ClickAfter(object sboObject, SBOItemEventArg pVal) {
            SelectRow(mtx0, pVal.Row);
            transferItems = foodPlantDAO.GetTransferItems(dt0.GetValue("C_Folio", selectedRow - 1).ToString());
            Task.Factory.StartNew(() => { 
                this.UIAPIRawForm.Freeze(true); 
                SAPMatrix.Fill("DT1", dt1, mtx1, columns1.Keys.ToList(), transferItems); 
                this.UIAPIRawForm.Freeze(false);});
        }


        #endregion

        #region Controls
        private Matrix mtx0;
        private Matrix mtx1;
        private Button btnAccept;
        private Button btnReturn;
        private StaticText lblComment;
        private EditText txtComment;
        private DataTable dt0;
        private DataTable dt1;
        #endregion

        #region Other Methods
        private void SelectRow(Matrix mtx, int row) {
            if(row != 0) {
                mtx.SelectRow(row, true, false);
            }
            else {
                mtx.SelectRow(row, false, false);
            }
            selectedRow = row;
        }
        #endregion
    }
}

