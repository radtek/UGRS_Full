using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.Utility;
using System.Globalization;
using UGRS.AddOn.Finances.Utils;

namespace UGRS.AddOn.Finances
{
    public class ComissionModal
    {
        private SAPbouiCOM.Company mObjCompany;

        private SAPbouiCOM.Form mFrmInvoice;
        private SAPbouiCOM.Item mItmBtnCard;
        private SAPbouiCOM.EditText mEdtTotal;
        private SAPbouiCOM.Matrix mMtxLines;
        private SAPbouiCOM.EditText mEdtClient;

        private SAPbouiCOM.Form mFrmDialog;
        // Modal items
        private SAPbouiCOM.ComboBox mCmbType;
        private SAPbouiCOM.EditText mEdtAmnt;
        private SAPbouiCOM.Button mBtnAdd;

        // Item codes for comissions
        string mStrItemCode2; //2%
        string mStrItemCode15; //1.5%

        private QueryManager mObjQueryManager = new QueryManager();

        public ComissionModal(SAPbouiCOM.Company pCompany, SAPbouiCOM.Form pForm)
        {
            mObjCompany = pCompany;
            mFrmInvoice = pForm;
            AddComissionButton();
        }

        /// <summary>
        /// Injects the comission button in the form
        /// </summary>
        private void AddComissionButton()
        {
            string lStrMatrixId = "38";
            string lStrTotalId = "29";
            string lStrClientId = "4";
            try
            {
                mFrmInvoice.Freeze(true);
                if (mItmBtnCard == null)
                {
                    mItmBtnCard = mFrmInvoice.Items.Add("btnBnk", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    mItmBtnCard.Top = mFrmInvoice.Items.Item(lStrMatrixId).Top + 169;
                    mItmBtnCard.Left = mFrmInvoice.Items.Item(lStrMatrixId).Left;
                    mItmBtnCard.FromPane = 1;
                    mItmBtnCard.ToPane = 1;
                    mItmBtnCard.Width = 145;
                    mItmBtnCard.Enabled = false;
                    (mItmBtnCard.Specific as SAPbouiCOM.Button).Caption = "Comisión por pago con tarjeta";
                    (mItmBtnCard.Specific as SAPbouiCOM.Button).ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.mItmBtnCard_ClickBefore);
                }
                mEdtTotal = (mFrmInvoice.Items.Item(lStrTotalId).Specific as SAPbouiCOM.EditText);
                mMtxLines = (mFrmInvoice.Items.Item(lStrMatrixId).Specific as SAPbouiCOM.Matrix);
                mEdtClient = (mFrmInvoice.Items.Item(lStrClientId).Specific as SAPbouiCOM.EditText);
                mEdtClient.ValidateAfter += new SAPbouiCOM._IEditTextEvents_ValidateAfterEventHandler(this.OnClientEdit);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[ComissionModal - AddComissionButton] Error al agregar el botón de Comisión: {0}", lObjException.Message));
                UIApplication.ShowMessageBox(string.Format("Error al agregar el botón de Comisión: {0}", lObjException.Message));
            }
            finally
            {
                mFrmInvoice.Freeze(false);
            }
        }

        private void OnClientEdit(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            mItmBtnCard.Enabled = (sboObject as SAPbouiCOM.EditText).Value != "";
        }


        private void mItmBtnCard_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (!(sboObject as SAPbouiCOM.Button).Item.Enabled)
            {
                return;
            }
            LoadModal();
            BindElements();
            InitElements();
        }

        /// <summary>
        /// Loads a form from an XML file.
        /// </summary>
        private void LoadModal()
        {
            mFrmDialog = Application.SBO_Application.Forms.AddEx(XmlLoader.LoadFromXml("fmCrdCom"));
            mFrmDialog.Title = "Agregar comisión";
            mFrmDialog.Top = (UIApplication.GetApplication().Desktop.Height - mFrmDialog.Height) / 2;
            mFrmDialog.Left = (UIApplication.GetApplication().Desktop.Width - mFrmDialog.Width) / 2;
            mFrmDialog.Visible = true;
        }

        /// <summary>
        /// Binds elements in the form-
        /// </summary>
        private void BindElements()
        {
            mCmbType = ((SAPbouiCOM.ComboBox)mFrmDialog.Items.Item("cmbType").Specific);
            mBtnAdd = ((SAPbouiCOM.Button)mFrmDialog.Items.Item("btnAdd").Specific);
            mEdtAmnt = ((SAPbouiCOM.EditText)mFrmDialog.Items.Item("edtAmnt").Specific);
        }

        /// <summary>
        /// Initializes the form items, subscribing to events and loadig comboboxes values.
        /// </summary>
        private void InitElements()
        {
            try
            {
                mBtnAdd.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.mBtnAdd_ClickBefore);
                mCmbType.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.mCmbType_ComboSelectAfter);
                mStrItemCode2 = mObjQueryManager.GetValue("U_VALUE", "Name", Constants.STR_ENTRY_COMISSION_2, Constants.STR_CONFIG_TABLE);
                string lStrArtCred2Desc = mObjQueryManager.GetValue("U_Comentario", "Name", Constants.STR_ENTRY_COMISSION_2, Constants.STR_CONFIG_TABLE);
                mStrItemCode15 = mObjQueryManager.GetValue("U_VALUE", "Name", Constants.STR_ENTRY_COMISSION_15, Constants.STR_CONFIG_TABLE);
                string lStrArtCred15Desc = mObjQueryManager.GetValue("U_Comentario", "Name", Constants.STR_ENTRY_COMISSION_15, Constants.STR_CONFIG_TABLE);
                mCmbType.ValidValues.Add(mStrItemCode2, lStrArtCred2Desc);
                mCmbType.ValidValues.Add(mStrItemCode15, lStrArtCred15Desc);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[ComissionModal - InitElements] Error al obtener los valores de las parametrizaciones: {0}", lObjException.Message));
                UIApplication.ShowMessageBox(string.Format("Error al obtener los valores de las parametrizaciones: {0}", lObjException.Message));
            }
        }

        /// <summary>
        /// Event for the comission combobox. Calculates the commisionr rates.
        /// </summary>
        private void mCmbType_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                mBtnAdd.Item.Enabled = true;
                string lStrItemCode = (sboObject as SAPbouiCOM.ComboBox).Value;
                double lDblPercent = Convert.ToDouble(mObjQueryManager.GetValue("CommisPcnt", "ItemCode", lStrItemCode, "OITM"));
                double lDblTotal = Convert.ToDouble(mEdtTotal.Value.ToString().Replace("MXP", ""), CultureInfo.InvariantCulture);
                mFrmDialog.DataSources.UserDataSources.Item("UD_Cost").ValueEx = (lDblTotal * (lDblPercent / 100)).ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[ComissionModal - mCmbType_ComboSelectAfter] Error al calcular el porcentaje de comisión: {0}", lObjException.Message));
                UIApplication.ShowMessageBox(string.Format("Error al calcular el porcentaje de comisión: {0}", lObjException.Message));
            }
        }

        private void mBtnAdd_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (!(sboObject as SAPbouiCOM.Button).Item.Enabled)
            {
                return;
            }

            try
            {
                string lStrItemCode = mCmbType.Value.ToString();
                string lStrPrice = mEdtAmnt.Value;
                if (mFrmInvoice.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE && mFrmInvoice.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                {
                    UIApplication.ShowMessageBox("El documento actual no es editable.");
                    return;
                }
                if (mFrmInvoice.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE && mFrmInvoice.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                {
                    UIApplication.ShowMessageBox("El documento actual no es editable.");
                    return;
                }
                if (Convert.ToDouble(lStrPrice) <= 0)
                {
                    UIApplication.ShowMessageBox("La comisión debe ser mayor a cero.");
                    return;
                }

                mFrmDialog.Close();

                int lIntRow = mMtxLines.RowCount;
                if ((mMtxLines.Columns.Item("1").Cells.Item(lIntRow).Specific as SAPbouiCOM.EditText).Value != "")
                {
                    mMtxLines.AddRow();
                    lIntRow++;
                }
                mMtxLines.SetCellFocus(lIntRow, 1);
                SetValueColumnEditText("1", lIntRow, lStrItemCode);
                SetValueColumnEditText("14", lIntRow, lStrPrice);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[ComissionModal - mBtnAdd_ClickBefore] Error al agregar el item con la comisión: {0}", lObjException.Message));
                UIApplication.ShowMessageBox(string.Format("Error al agregar el item con la comisión: {0}", lObjException.Message));
            }
        }

        private void SetValueColumnEditText(string pStrCol, int pIntRow, string pStrValue)
        {
            SAPbouiCOM.EditText lObjEditText = (SAPbouiCOM.EditText)mMtxLines.Columns.Item(pStrCol).Cells.Item(pIntRow).Specific;
            lObjEditText.Value = pStrValue;
        }
    }
}
