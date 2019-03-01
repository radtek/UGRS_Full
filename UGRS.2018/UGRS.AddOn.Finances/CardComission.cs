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

namespace UGRS.AddOn.Finances
{
    public class CardComission
    {
        public static string STR_AR_INVOICE_FORM = "133";
        public static string STR_ADVANCE_FORM = "65300";
        public static string STR_RESERVE_FOR = "60091";

        private SAPbouiCOM.Company mObjCompany;

        private SAPbouiCOM.Form mObjForm;
        private SAPbouiCOM.Item mItmBtnCard;
        private SAPbouiCOM.EditText mEdtTotal;
        private SAPbouiCOM.Matrix mMtxLines;

        private SAPbouiCOM.Form mObjModal;
        // Modal items
        private SAPbouiCOM.ComboBox mCmbType;
        private SAPbouiCOM.EditText mEdtAmnt;
        private SAPbouiCOM.Button mBtnAdd;

        // Item codes for comissions
        string mStrItemCode2; //2%
        string mStrItemCode15; //1.5%

        private QueryManager mObjQueryManager = new QueryManager();

        public CardComission(SAPbouiCOM.Company pCompany, SAPbouiCOM.Form pForm)
        {
            mObjCompany = pCompany;
            mObjForm = pForm;
            AddComissionButton();
        }

        /// <summary>
        /// Injects the comission button in the form
        /// </summary>
        private void AddComissionButton()
        {
            string lStrMatrixId = "38";
            string lStrTotalId = "29";
            try
            {
                mObjForm.Freeze(true);
                if (mItmBtnCard == null)
                {
                    mItmBtnCard = mObjForm.Items.Add("btnBnk", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    mItmBtnCard.Top = mObjForm.Items.Item(lStrMatrixId).Top + 168;
                    mItmBtnCard.Left = mObjForm.Items.Item(lStrMatrixId).Left;
                    mItmBtnCard.FromPane = 1;
                    mItmBtnCard.ToPane = 1;
                    mItmBtnCard.Width = 145;
                    (mItmBtnCard.Specific as SAPbouiCOM.Button).Caption = "Comisión por pago con tarjeta";
                    (mItmBtnCard.Specific as SAPbouiCOM.Button).ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.mItmBtnCard_ClickBefore);
                }
                mEdtTotal = (mObjForm.Items.Item(lStrTotalId).Specific as SAPbouiCOM.EditText);
                mMtxLines = (mObjForm.Items.Item(lStrMatrixId).Specific as SAPbouiCOM.Matrix);
            }
            finally
            {
                mObjForm.Freeze(false);
            }
        }


        private void mItmBtnCard_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            LoadFromXml("fmCrdCom");
            BindElements();
            InitElements();
        }

        /// <summary>
        /// Loads a form from an XML file.
        /// </summary>
        /// <param name="pName">Name of the xml file containig the form definition.</param>
        private void LoadFromXml(string pName)
        {
            XmlDocument lObjXmlDoc = new XmlDocument();

            Type lObjBaseType = this.GetType();

            if (lObjBaseType.Assembly.IsDynamic)
                lObjBaseType = lObjBaseType.BaseType;

            string lStrNamespace = lObjBaseType.Namespace;
            string lStrContent = "";
            using (var lObjStream = lObjBaseType.Assembly.GetManifestResourceStream(lStrNamespace + ".XmlForms." + pName + ".xml"))
            {
                if (lObjStream != null)
                {
                    using (var lObjStreamReader = new StreamReader(lObjStream))
                    {
                        lStrContent = lObjStreamReader.ReadToEnd();
                    }
                }
            }

            lObjXmlDoc.LoadXml(lStrContent);

            SAPbouiCOM.FormCreationParams lObjCreationPackage = (SAPbouiCOM.FormCreationParams)Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);
            lObjCreationPackage.XmlData = lObjXmlDoc.InnerXml;
            lObjCreationPackage.UniqueID = pName;
            lObjCreationPackage.Modality = SAPbouiCOM.BoFormModality.fm_Modal;
            lObjCreationPackage.FormType = pName;

            mObjModal = Application.SBO_Application.Forms.AddEx(lObjCreationPackage);
            mObjModal.Title = "Agregar comisión";
            mObjModal.Visible = true;
            mObjModal.Top = 200;
            mObjModal.Left = 400;
        }

        /// <summary>
        /// Binds elements in the form-
        /// </summary>
        private void BindElements()
        {
            mCmbType = ((SAPbouiCOM.ComboBox)mObjModal.Items.Item("cmbType").Specific);
            mBtnAdd = ((SAPbouiCOM.Button)mObjModal.Items.Item("btnAdd").Specific);
            mEdtAmnt = ((SAPbouiCOM.EditText)mObjModal.Items.Item("edtAmnt").Specific);
            mBtnAdd.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.mBtnAdd_ClickBefore);
            mCmbType.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.mCmbType_ComboSelectAfter);
        }

        private void mCmbType_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            string lStrItemCode = (sboObject as SAPbouiCOM.ComboBox).Value;
            double lDblPercent = Convert.ToDouble(mObjQueryManager.GetValue("CommisPcnt", "ItemCode", lStrItemCode, "OITM"));
            double lDblTotal = Convert.ToDouble(mEdtTotal.Value.ToString().Replace("MXP", ""), CultureInfo.InvariantCulture);
            mObjModal.DataSources.UserDataSources.Item("UD_Cost").ValueEx = (lDblTotal * (lDblPercent / 100)).ToString(CultureInfo.InvariantCulture);
        }

        private void mBtnAdd_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            string lStrItemCode = mCmbType.Value.ToString();
            string lStrPrice = mObjModal.DataSources.UserDataSources.Item("UD_Cost").ValueEx;
            mObjModal.Close();
            int lIntRow = mMtxLines.RowCount;
            SetValueColumnEditText("1", lIntRow, lStrItemCode);
            SetValueColumnEditText("11", lIntRow, "1");
            SetValueColumnEditText("14", lIntRow, lStrPrice);
        }

        private void InitElements()
        {
            mStrItemCode2 = mObjQueryManager.GetValue("U_VALUE", "Name", "GLO_CREDTARJ2", "[@UG_CONFIG]");
            string lStrArtCred2Desc = mObjQueryManager.GetValue("U_Comentario", "Name", "GLO_CREDTARJ2", "[@UG_CONFIG]");
            mStrItemCode15 = mObjQueryManager.GetValue("U_VALUE", "Name", "GLO_CREDTARJ15", "[@UG_CONFIG]");
            string lStrArtCred15Desc = mObjQueryManager.GetValue("U_Comentario", "Name", "GLO_CREDTARJ15", "[@UG_CONFIG]");
            mCmbType.ValidValues.Add(mStrItemCode2, lStrArtCred2Desc);
            mCmbType.ValidValues.Add(mStrItemCode15, lStrArtCred15Desc);
        }

        private void SetValueColumnEditText(string pStrCol, int pIntRow, string pStrValue)
        {
            SAPbouiCOM.EditText lObjEditText = (SAPbouiCOM.EditText)mMtxLines.Columns.Item(pStrCol).Cells.Item(pIntRow).Specific;
            lObjEditText.Active = true;
            lObjEditText.Value = pStrValue;
        }
    }
}
