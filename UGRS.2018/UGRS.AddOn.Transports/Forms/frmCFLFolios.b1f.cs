using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.Transports;

namespace UGRS.AddOn.Transports.Forms
{
    [FormAttribute("UGRS.AddOn.Transports.Forms.frmCFLFolios", "Forms/frmCFLFolios.b1f")]
    class frmCFLFolios : UserFormBase
    {
        #region Properties
        public string mStrFolio = string.Empty;
        TransportServiceFactory mObjTransportServiceFactory = new TransportServiceFactory();
        #endregion

        #region Constructor
        public frmCFLFolios()
        {
            LoadEvents();
            LoadFolios();
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtxFolios = ((SAPbouiCOM.Matrix)(this.GetItem("Item_0").Specific));
            this.btnSelect = ((SAPbouiCOM.Button)(this.GetItem("btnSel").Specific));
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCan").Specific));
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

        #region Load & Unload Events
        private void LoadEvents()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }
        private void UnLoadEvents()
        {
            Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }
        #endregion

        #region EventsHandle
        /// <summary>
        /// SBO_Application_ItemEvent
        /// Metodo para controlar los eventos de la pantalla.
        /// @Author FranciscoFimbres
        /// </summary>
        /// <param name="FormUID"></param>
        /// <param name="pVal"></param>
        /// <param name="BubbleEvent"></param>
        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //string y = pVal.CharPressed.ToString();
            try
            {
                if (FormUID.Equals(this.UIAPIRawForm.UniqueID))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_CLICK:
                                if (pVal.ItemUID.Equals("btnSel"))
                                {
                                    SelectFolio();
                                }
                                if (pVal.ItemUID.Equals("btnCan"))
                                {
                                    this.UIAPIRawForm.Close();
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_FORM_RESIZE:
                                mtxFolios.AutoResizeColumns();
                                break;
                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                UnLoadEvents();
                                break;
                        }
                    }
                    else if (pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_CLICK:
                                if (pVal.ItemUID.Equals("Item_0"))
                                {
                                    if (pVal.Row > 0)
                                        mtxFolios.SelectRow(pVal.Row, true, false);
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK:
                                if (pVal.ItemUID.Equals("Item_0"))
                                {
                                    SelectFolio();
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(String.Format("[frmCFLFolios - SBO_Application_ItemEvent] Error: {0}", ex.Message));
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }
        #endregion

        #region Functions
        private void LoadFolios()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);
                Application.SBO_Application.StatusBar.SetText("Buscando folios...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                ClearMatrix();

                //FillDataSource(pStrDate1, pStrDate2);

                this.UIAPIRawForm.DataSources.DataTables.Item("DT_Fol").ExecuteQuery(mObjTransportServiceFactory.GetCommissionDriverService().GetCmsFoliosQuery());

                mtxFolios.AutoResizeColumns();
                mtxFolios.Columns.Item("ColFolio").DataBind.Bind("DT_Fol", "U_Folio");
                mtxFolios.Columns.Item("ColClient").DataBind.Bind("DT_Fol", "U_Amount");
                //mtxFolios.Columns.Item("ColEst").DataBind.Bind("DT_Fol", "Estatus");

                mtxFolios.LoadFromDataSource();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmCFLFolios - LoadFolios] Error: {0}", lObjException.Message));
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(string.Format("Error al obtener los folios: {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void SelectFolio()
        {
            int lIntRow = mtxFolios.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_SelectionOrder);
            if (lIntRow >= 0)
            {
                mStrFolio = (mtxFolios.Columns.Item(1).Cells.Item(lIntRow).Specific as SAPbouiCOM.EditText).Value.Trim();
                this.UIAPIRawForm.Close();
            }
        }

        /// <summary>
        /// Clean the matrix items.
        /// </summary>
        private void ClearMatrix()
        {
            if (!this.UIAPIRawForm.DataSources.DataTables.Item("DT_Fol").IsEmpty)
            {
                this.UIAPIRawForm.DataSources.DataTables.Item("DT_Fol").Rows.Clear();
                mtxFolios.Clear();
            }
        }
        #endregion

        #region Controls
        private SAPbouiCOM.Matrix mtxFolios;
        private SAPbouiCOM.Button btnSelect;
        private SAPbouiCOM.Button btnCancel;
        #endregion
    }
}
