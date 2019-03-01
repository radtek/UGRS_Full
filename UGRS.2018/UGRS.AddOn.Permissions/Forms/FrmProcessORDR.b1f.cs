using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Permissions.DAO;
using UGRS.Core.SDK.DI.Permissions.DTO;
using UGRS.Core.SDK.DI.WebServicePermissions.Services;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.UI.ProgressBar;
using UGRS.Core.Services;

namespace UGRS.AddOn.Permissions.Forms {
    [FormAttribute("UGRS.AddOn.Permissions.Forms.FrmProcessORDR", "Forms/FrmProcessORDR.b1f")]
    class FrmProcessORDR : UserFormBase {

        #region PROPETIES
        PendingOrderDTO[] pendingOrders = null;
        #endregion

        #region CONTRUCTOR
        public FrmProcessORDR() {

            Task.Run(() => pendingOrders = new PendingSalesOrdersDAO().GetPendingSalesOrders());

        }
        #endregion

        #region EVENTS
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent() {
            this.btnProcess = ((SAPbouiCOM.Button)(this.GetItem("btnProcess").Specific));
            this.btnProcess.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnProcess_ClickBefore);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents() {
        }


        private void OnCustomInitialize() {

        }

        private void btnProcess_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent) {

            BubbleEvent = true;

            if(btnProcess.Item.Enabled) {

                try {
                    this.ProcessPendingOrders();
                    //this.ParallelProcessPendingOrders();
                }
                catch(AggregateException ae) {
                    ae.Handle(ex => {
                        PendingSalesOrdersDAO.HandleException(ex, "btnProcess_ClickBefore");
                        return true;
                    });
                }
            }
        }
        #endregion

        #region METHOD
        public void ProcessPendingOrders() {

            ProgressBarManager oProgressBar = null;
            var timer = Stopwatch.StartNew();

            try {

                if(pendingOrders != null && pendingOrders.Length > 0) {

                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Procesando Ordenes de Venta", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    oProgressBar = new ProgressBarManager(UIApplication.GetApplication(), "Processing Sales Orders, please wait...", pendingOrders.Length);
                    foreach(var request in pendingOrders) {

                        var result = new PermissionRequestService().CreateSaleOrder(request.ID);
                        LogService.WriteInfo(String.Format("Orden {3}-{0}-{1}: Resultado: {2}", request.ID, request.Type, result, request.Folio));
                        oProgressBar.NextPosition();
                    }

                    UIApplication.ShowMessageBox(String.Format("Se procesaron un total de {0} ordenes de venta \n Tiempo Transcurrido {1}", pendingOrders.Length, timer.Elapsed));

                }
                else {
                    UIApplication.ShowMessageBox("No hay ordenes de venta pendientes o las ordenes aun no se han terminado cargar");
                }
            }
            catch(Exception ex) {
                PendingSalesOrdersDAO.HandleException(ex, "ProcessPendingOrders");
            }
            finally {
                oProgressBar.Stop();
                btnProcess.Item.Enabled = false;
            }
        }

        //public void ParallelProcessPendingOrders() {

        //    ProgressBarManager oProgressBar = null;
        //    var timer = Stopwatch.StartNew();

        //    try {

        //        if(pendingOrders != null && pendingOrders.Length > 0) {

        //            SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Procesando Ordenes de Venta", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
        //            oProgressBar = new ProgressBarManager(UIApplication.GetApplication(), "Processing Sales Orders, please wait...", pendingOrders.Length);
        //            Parallel.ForEach(Partitioner.Create(0, pendingOrders.Length), (range, state) => {
        //                for(int i = range.Item1; i < range.Item2; i++) {

        //                    var result = new PermissionRequestService().CreateSaleOrder(pendingOrders[i].ID);
        //                    oProgressBar.NextPosition();
        //                }
        //            });

        //            UIApplication.ShowMessageBox(String.Format("Se procesaron un total de {0} ordenes de venta \n Tiempo Transcurrido {1}", pendingOrders.Length, timer.Elapsed));

        //        }
        //        else {
        //            UIApplication.ShowMessageBox("No hay ordenes de venta pendientes o las ordenes aun no se han terminado cargar");
        //        }
        //    }
        //    catch(Exception ex) {
        //        PendingSalesOrdersDAO.HandleException(ex, "ProcessPendingOrders");
        //    }
        //    finally {
        //        oProgressBar.Stop();
        //        btnProcess.Item.Enabled = false;
        //    }
        //}
        #endregion

        #region CONTROLS
        private Button btnProcess;
        #endregion

    }
}
