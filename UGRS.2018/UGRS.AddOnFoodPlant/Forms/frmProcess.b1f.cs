using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using SAPbouiCOM;

namespace UGRS.AddOnFoodPlant.Forms {
    [FormAttribute("UGRS.AddOnFoodPlant.Forms.frmProcess", "Forms/frmProcess.b1f")]
    class frmProcess : UserFormBase {

        public frmProcess() {
            mtx0.AutoResizeColumns();

        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent() {
            this.lblFolio = ((SAPbouiCOM.StaticText)(this.GetItem("lblFolio").Specific));
            this.lblItem = ((SAPbouiCOM.StaticText)(this.GetItem("lblItem").Specific));
            this.lblDate = ((SAPbouiCOM.StaticText)(this.GetItem("lblDate").Specific));
            this.lblQReal = ((SAPbouiCOM.StaticText)(this.GetItem("lblQReal").Specific));
            this.lblQPlan = ((SAPbouiCOM.StaticText)(this.GetItem("lblQPlan").Specific));
            this.lblQDif = ((SAPbouiCOM.StaticText)(this.GetItem("lblQDif").Specific));
            this.txtFolio = ((SAPbouiCOM.EditText)(this.GetItem("txtFolio").Specific));
            this.txtItem = ((SAPbouiCOM.EditText)(this.GetItem("txtItem").Specific));
            this.txtDate = ((SAPbouiCOM.EditText)(this.GetItem("txtDate").Specific));
            this.txtQReal = ((SAPbouiCOM.EditText)(this.GetItem("txtQReal").Specific));
            this.txtQPlan = ((SAPbouiCOM.EditText)(this.GetItem("txtQPlan").Specific));
            this.txtDif = ((SAPbouiCOM.EditText)(this.GetItem("txtDif").Specific));
            this.btnChange = ((SAPbouiCOM.Button)(this.GetItem("btnChange").Specific));
            this.btnChange.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnChange_ClickBefore);
            this.btnCreate = ((SAPbouiCOM.Button)(this.GetItem("btnCreate").Specific));
            this.btnCreate.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCreate_ClickBefore);
            this.mtx0 = ((SAPbouiCOM.Matrix)(this.GetItem("mtx0").Specific));
            this.OnCustomInitialize();

        }

        private StaticText lblFolio;
        private StaticText lblItem;
        private StaticText lblDate;
        private StaticText lblQReal;
        private StaticText lblQPlan;
        private StaticText lblQDif;
        private EditText txtFolio;
        private EditText txtItem;
        private EditText txtDate;
        private EditText txtQReal;
        private EditText txtQPlan;
        private EditText txtDif;
        private Button btnChange;
        private Button btnCreate;
        private Matrix mtx0;
      

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents() {
        }

        

        private void OnCustomInitialize() {

        }

        private void btnCreate_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent) {
            BubbleEvent = true;
           

        }

        private void btnChange_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent) {
            BubbleEvent = true;
           

        }

        

       
    }
}
