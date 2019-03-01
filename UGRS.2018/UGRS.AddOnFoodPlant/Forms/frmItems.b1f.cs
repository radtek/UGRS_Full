/*
 * Autor: LCC Abraham SaÚL Sandoval Meneses
 * Description: Transfer of Transit Inventory to Area Form
 * Date: 31/08/2018
 */


using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGRS.AddOnFoodPlant.Utils;
using UGRS.Core.SDK.DI.FoodPlant.DAO;
using UGRS.Core.SDK.DI.FoodPlant.DTO;
using UGRS.Core.SDK.DI.FoodPlant.Services;
using UGRS.Core.SDK.UI;

namespace UGRS.AddOnFoodPlant.Forms {
    [FormAttribute("UGRS.AddOnFoodPlant.Forms.frmItems", "Forms/frmItems.b1f")]
    class frmItems : UserFormBase {

        #region Properties
        FoodPlantDAO foodPlantDAO = new FoodPlantDAO();
        Dictionary<string, BoFieldsType> columns;
        RequestTransfer[] requestTransfers = null;
        User user = null;
        #endregion

        #region Cosntructor
        public frmItems(string docNum) {
            requestTransfers = foodPlantDAO.GetTransferRequests(docNum);
            Parallel.Invoke(PrepareMatrix, SetTextValues, () => { user = new User(); });
        }
        #endregion

        #region PrepareMatrix
        public void PrepareMatrix() {
            columns = new Dictionary<string, BoFieldsType>() { { "#", BoFieldsType.ft_ShortNumber }, { "Item", BoFieldsType.ft_AlphaNumeric }, { "Desc", BoFieldsType.ft_AlphaNumeric }, { "Quantity", BoFieldsType.ft_Quantity } };
            dt0 = SAPMatrix.CreateDataTable("DT0", columns, this);
            SAPMatrix.Fill("DT0", dt0, mtx0, columns.Keys.ToList(), requestTransfers);
        }
        #endregion

        #region SetTextValues
        public void SetTextValues() {
            if(!Object.ReferenceEquals(requestTransfers, null)) {
                txtFolio.Value = requestTransfers[0].Folio.ToString();
                txtFWhs.Value = requestTransfers[0].FromWhs;
                txtTWhs.Value = requestTransfers[0].ToWhs;
            }
        }
        #endregion

        #region SetStockTransferQuantities
        public void SetStockTransferQuantities(string column) {
            Task.Factory.StartNew(() => {
                Parallel.ForEach(Partitioner.Create(1, mtx0.RowCount + 1), (range, state) => {
                    for(int j = range.Item1; j < range.Item2; j++) {
                        requestTransfers[j - 1].Quantity = Convert.ToDouble(((EditText)mtx0.Columns.Item(column).Cells.Item(j).Specific).Value == "" ? "0" : ((EditText)mtx0.Columns.Item(column).Cells.Item(j).Specific).Value);
                    }
                });
            }).Wait();
        }
        #endregion

        #region OpenStockTransferForm
        public void OpenStockTransferForm() {

            if(!Object.ReferenceEquals(requestTransfers, null)) {
                SetStockTransferQuantities("C_Quantity");
                requestTransfers[0].Observations = txtObserv.Value;
                var result = StockTransferDI.CreateDraft(requestTransfers, user);

                if(result.Success) {
                    Form form = SAPbouiCOM.Framework.Application.SBO_Application.OpenForm((BoFormObjectEnum)112, "", result.Message);
                }
                else {
                    UIApplication.ShowMessageBox(result.Message);
                }
            }
        }
        #endregion

        #region Events
        public override void OnInitializeFormEvents() { }

        private void OnCustomInitialize() { }

        private void btnCreate_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent) {
            BubbleEvent = true;
            OpenStockTransferForm();

        }

        private void btnCancel_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent) {
            BubbleEvent = true;
            this.UIAPIRawForm.Close();
        }

        private void btnSeries_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent) {
            BubbleEvent = true;

        }

        public override void OnInitializeComponent() {
            this.btnCreate = ((SAPbouiCOM.Button)(this.GetItem("btnCreate").Specific));
            this.btnCreate.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCreate_ClickBefore);
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCancel").Specific));
            this.btnCancel.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCancel_ClickBefore);
            this.lblObserv = ((SAPbouiCOM.StaticText)(this.GetItem("lblObserv").Specific));
            this.txtObserv = ((SAPbouiCOM.EditText)(this.GetItem("txtObserv").Specific));
            this.mtx0 = ((SAPbouiCOM.Matrix)(this.GetItem("mtx0").Specific));
            this.lblFolio = ((SAPbouiCOM.StaticText)(this.GetItem("lblFolio").Specific));
            this.lblFromWhs = ((SAPbouiCOM.StaticText)(this.GetItem("lblFromWhs").Specific));
            this.lblToWhs = ((SAPbouiCOM.StaticText)(this.GetItem("lblToWhs").Specific));
            this.txtFolio = ((SAPbouiCOM.EditText)(this.GetItem("txtFolio").Specific));
            this.txtFWhs = ((SAPbouiCOM.EditText)(this.GetItem("txtFWhs").Specific));
            this.txtTWhs = ((SAPbouiCOM.EditText)(this.GetItem("txtTWhs").Specific));
            this.OnCustomInitialize();
        }
        #endregion

        #region Constrols
        private Button btnCreate;
        private Button btnCancel;
        private EditText txtObserv;
        private Matrix mtx0;
        private StaticText lblObserv;
        private StaticText lblFolio;
        private StaticText lblFromWhs;
        private StaticText lblToWhs;
        private EditText txtFolio;
        private EditText txtFWhs;
        private EditText txtTWhs;
        private DataTable dt0;
        #endregion
    }
}
