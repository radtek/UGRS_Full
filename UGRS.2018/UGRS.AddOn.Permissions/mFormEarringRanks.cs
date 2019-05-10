using SAPbobsCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UGRS.AddOn.Permissions.DTO;
using UGRS.AddOn.Permissions.Tables;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.Permissions;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.Utility;


namespace UGRS.AddOn.Permissions
{
    public class mFormEarringRanks
    {
        #region SAP objects
        SAPbouiCOM.Form mObjRanksForm = null;
        SAPbouiCOM.Grid mObjRanksGrid = null;
        SAPbouiCOM.EditText mObjtxtFrom = null;
        SAPbouiCOM.EditText mObjtxtTo = null;
        SAPbouiCOM.EditText mObjtxtTotal = null;
        SAPbouiCOM.DataTable mObjDtRanks = null;
        SAPbouiCOM.EditText mTxtCert = null;
        #endregion

        PermissionsFactory mObjPermissionFactory = new PermissionsFactory();

        //Services.EarringRanksService lObjEarRankService = new Services.EarringRanksService();
        // DAO.EarringRanksDAO lObjEaRanksDAO = new DAO.EarringRanksDAO();

        #region lists
        List<EarringRanksT> mLstEarRnksT = null;
        EarringRanksT mObjEarringRanksT = null;
        List<int> mLstRowCodes = null;
        List<EarringRanksT> mLstDeletedRows = null;
        string lStrETActivePrefix = null;
        #endregion

        #region variables
        string mStrBaseEntry = "";
        string mStrValue = "";
       // int mIntCertHeadsCounter = 0;
        int mIntDocEntry = 0;
        int mIntHeadsInCertificate = 0;
        //int mIntEarrings = 0;
        #endregion

        public mFormEarringRanks(string pStrBaseEntry, int pIntTop, int pIntLeft)
        {
            try
            {
                this.mStrBaseEntry = pStrBaseEntry;
                this.mIntDocEntry = mObjPermissionFactory.GetPermissionsService().GetDocEntry(mStrBaseEntry);
                this.mIntHeadsInCertificate = mObjPermissionFactory.GetPermissionsService().GetTotalCertHeads(mIntDocEntry);
               
                LoadFromXml("mFrmEarringRanks.xml", "mFrmEarringRanks", pIntTop, pIntLeft);
                mTxtCert.Value = mIntHeadsInCertificate.ToString();
                mObjtxtFrom.Item.Click();
                SetPrefix();
            }
            catch (Exception ex)
            {
                LogService.WriteError("mFormEarringRanks" + ex.Message);
                LogService.WriteError(ex);
                //UIApplication.ShowError(ex.Message);
            }
        }

        #region Initialize Modal Form
        private void SetPrefix()
        {
            lStrETActivePrefix = mObjPermissionFactory.GetPermissionsService().GetPrefix();// "SON";
        }

        private void LoadFromXml(string FileName, string FormName, int pIntTop, int pIntLeft)
        {

            System.Xml.XmlDocument oXmlDoc = new System.Xml.XmlDocument();
            //string sPath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]).ToString();
            string sPath = PathUtilities.GetCurrent("XmlForms");

            oXmlDoc.Load(sPath + "\\" + FileName);

            SAPbouiCOM.FormCreationParams creationPackage = (SAPbouiCOM.FormCreationParams)Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);

            creationPackage.XmlData = oXmlDoc.InnerXml;

            if (FormName.Equals("mFrmEarringRanks"))
            {
                //UGRS.AddOn.Cuarentenarias.Utils.utils.FormExists(FormName);

                creationPackage.UniqueID = FormName;
                creationPackage.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Fixed;
                //creationPackage.Modality = SAPbouiCOM.BoFormModality.fm_Modal;
                creationPackage.FormType = "mFrmEarringRanks";

                //creationPackage.
                var lObjForms = SAPbouiCOM.Framework.Application.SBO_Application.Forms;
                foreach (SAPbouiCOM.Form lObjForm in lObjForms)
                {
                    if (lObjForm.UniqueID == "mFrmEarringRanks")
                    {
                        SAPbouiCOM.Framework.Application.SBO_Application.Forms.Item("mFrmEarringRanks").Close();
                    }
                }

                mObjRanksForm = Application.SBO_Application.Forms.AddEx(creationPackage);

                mObjRanksForm.Title = "Rangos de aretes";
                mObjRanksForm.Left = pIntLeft - mObjRanksForm.Width / 2;
                mObjRanksForm.Top = pIntTop - mObjRanksForm.Height / 2;
                mObjRanksForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                mObjRanksForm.Visible = true;
                initFormXml();
            }
            else
            {
                //lObjModalForm.Select();
            }

        }

        private void initFormXml()
        {
            try
            {
                mObjRanksForm.Freeze(true);
                mObjRanksGrid = ((SAPbouiCOM.Grid)mObjRanksForm.Items.Item("GrdRanks").Specific);
                mObjtxtFrom = ((SAPbouiCOM.EditText)mObjRanksForm.Items.Item("TxtFrom").Specific);
                mObjtxtTo = ((SAPbouiCOM.EditText)mObjRanksForm.Items.Item("TxtTo").Specific);
                mObjtxtTotal = ((SAPbouiCOM.EditText)mObjRanksForm.Items.Item("txtTotal").Specific);
                mTxtCert = ((SAPbouiCOM.EditText)mObjRanksForm.Items.Item("txtCert").Specific);
                mLstEarRnksT = new List<EarringRanksT>();
                mLstDeletedRows = new List<EarringRanksT>();
                mLstRowCodes = new List<int>();

                mIntHeadsInCertificate = mObjPermissionFactory.GetPermissionsService().GetTotalCertHeads(mIntDocEntry);
                if (!HasLines())
                {

                    //int lIntTotalCertHeads = lObjEaRanksDAO.GetTotalCertHeads(lStrDocEntry);
                 
                    mObjtxtTotal.Value = "0";
                }
                else
                {
                    initGrid();
                   mObjtxtTotal.Value = GetTotal().ToString();
                }
                mObjRanksGrid.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                LogService.WriteError(ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                mObjRanksForm.Freeze(false);
            }
        }

        private int GetTotal()
        {
            int lIntCount = 0;
            for (int i = 0; i < mObjRanksGrid.DataTable.Rows.Count; i++)
            {
                
                string lStrDesde = mObjRanksGrid.DataTable.Columns.Item("Desde").Cells.Item(i).Value.ToString().Substring(4);


                string lStrHasta = string.Empty;
                lStrHasta = mObjRanksGrid.DataTable.Columns.Item("Hasta").Cells.Item(i).Value.ToString();
                if (!string.IsNullOrEmpty(lStrHasta))
                {
                    lStrHasta = mObjRanksGrid.DataTable.Columns.Item("Hasta").Cells.Item(i).Value.ToString().Substring(4);
                }
                else
                {
                    lStrHasta = lStrDesde;
                }

                int lIntHeadsPerLine = (Convert.ToInt32(lStrHasta) - Convert.ToInt32(lStrDesde)) + 1;

                lIntCount += lIntHeadsPerLine;
            }
            return lIntCount;
        }

        private void initGrid()
        {
            mLstEarRnksT = new List<EarringRanksT>();

            mObjRanksGrid.DataTable = mObjRanksForm.DataSources.DataTables.Item("DtRanks");



            mObjRanksGrid.Columns.Item("Code").Visible = false;
            mObjRanksGrid.Columns.Item("Desde").Editable = true;
            mObjRanksGrid.Columns.Item("Hasta").Editable = true;
            mObjRanksGrid.Columns.Item("Prefi").Visible = false;
            mObjRanksGrid.Columns.Item("EFrom").Visible = false;
            mObjRanksGrid.Columns.Item("ETo").Visible = false;
            mObjRanksGrid.AutoResizeColumns();


        }
        #endregion

        #region principal functions
        public void AddRow()
        {

            string lStrFrom = lStrETActivePrefix + mObjtxtFrom.Value;

            string lStrTo = string.Empty;
            if (!string.IsNullOrEmpty(mObjtxtTo.Value))
            {
                lStrTo = lStrETActivePrefix + mObjtxtTo.Value;
            }
            int lastrow = 0;
            if (ValidFields(mObjtxtFrom.Value, mObjtxtTo.Value, false, 0))
            {

                mObjEarringRanksT = new EarringRanksT();

                mObjDtRanks.Rows.Add();
                lastrow = mObjDtRanks.Rows.Count - 1;
                mObjDtRanks.Columns.Item("Desde").Cells.Item(lastrow).Value = lStrFrom;
                mObjDtRanks.Columns.Item("Hasta").Cells.Item(lastrow).Value = lStrTo;
              

                mObjEarringRanksT.EarringFrom = mObjtxtFrom.Value;
                mObjEarringRanksT.EarringTo = mObjtxtTo.Value;
                mObjEarringRanksT.Prefix = lStrETActivePrefix;
                mObjEarringRanksT.Row = lastrow;
                mLstEarRnksT.Add(mObjEarringRanksT);
                ClearFields();
            }
        }

        public void SaveRanks()
        {
            CancelRowList();

            foreach (var item in mLstEarRnksT)
            {
                Tables.EarringRanksT lObjEarringsRanksT = new Tables.EarringRanksT();

                lObjEarringsRanksT.BaseEntry = mIntDocEntry.ToString();// mStrBaseEntry;
                lObjEarringsRanksT.EarringFrom = item.EarringFrom;
                lObjEarringsRanksT.EarringTo = item.EarringTo;
                lObjEarringsRanksT.Prefix = item.Prefix;
                lObjEarringsRanksT.Cancelled = "N";
                int lIntResult = mObjPermissionFactory.GetEarringRanksService().SaveRanks(lObjEarringsRanksT);
                if (lIntResult != 0)
                {
                    string lStrError = DIApplication.Company.GetLastErrorDescription();
                    Application.SBO_Application.StatusBar.SetText(lStrError
                          , SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error);

                }
            }
        }

        private void CancelRowList()
        {
            try
            {
                foreach (var item in mLstDeletedRows)
                {
                    EarringRanksT lObjEarringRanksT = new EarringRanksT();

                    lObjEarringRanksT.RowCode = item.RowCode;
                    lObjEarringRanksT.EarringFrom = item.EarringFrom;
                    lObjEarringRanksT.EarringTo = item.EarringTo;
                    lObjEarringRanksT.Prefix = item.Prefix;
                    lObjEarringRanksT.BaseEntry = item.BaseEntry;
                    lObjEarringRanksT.Cancelled = "Y";

                    int lIntResult = mObjPermissionFactory.GetEarringRanksService().UpdateRanks(lObjEarringRanksT);
                    if (lIntResult != 0)
                    {
                        string lStrError = DIApplication.Company.GetLastErrorDescription();
                        Application.SBO_Application.StatusBar.SetText(lStrError
                              , SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error);

                    }
                }
            }
            catch
            {

            }
        }

        internal void CancelRow()
        {
            var lVarSelRows = mObjRanksGrid.Rows.SelectedRows;
            if (lVarSelRows.Count > 0)
            {
                int lIntSelRow = lVarSelRows.Item(0, SAPbouiCOM.BoOrderType.ot_RowOrder);

                string lStrEarringFrom = mObjRanksGrid.DataTable.Columns.Item("Desde").Cells.Item(lIntSelRow).Value.ToString().Substring(3);
                string lStrEarringTo = mObjRanksGrid.DataTable.Columns.Item("Hasta").Cells.Item(lIntSelRow).Value.ToString();
               
                if (!string.IsNullOrEmpty(lStrEarringTo))
                {
                   lStrEarringTo = mObjRanksGrid.DataTable.Columns.Item("Hasta").Cells.Item(lIntSelRow).Value.ToString().Substring(3);
                }
                else
                {
                    lStrEarringTo = lStrEarringFrom;
                }

                int lIntRowCode = Convert.ToInt32(mObjRanksGrid.DataTable.Columns.Item("Code").Cells.Item(lIntSelRow).Value);
                if (lIntSelRow >= 0)
                {
                    if (Convert.ToInt32(mObjRanksGrid.DataTable.Columns.Item("Code").Cells.Item(lIntSelRow).Value) != 0)
                    {
                        //lLstRowCodes.Add(lIntRowCode);

                        mObjEarringRanksT = new EarringRanksT();
                        mObjEarringRanksT.RowCode = lIntRowCode.ToString();
                        mObjEarringRanksT.EarringFrom = lStrEarringFrom;
                        mObjEarringRanksT.EarringTo = lStrEarringTo;
                        mObjEarringRanksT.BaseEntry = mStrBaseEntry;
                        mObjEarringRanksT.Prefix = lStrETActivePrefix;

                        mLstDeletedRows.Add(mObjEarringRanksT);

                        mObjRanksGrid.DataTable.Rows.Remove(lIntSelRow);

                    }
                    else
                    {
                        var lVarTempLst = mLstEarRnksT.Single(x => x.RowCode == null && x.EarringFrom == lStrEarringFrom && x.EarringTo == lStrEarringTo);
                        mLstEarRnksT.Remove(lVarTempLst);

                        mObjRanksGrid.DataTable.Rows.Remove(lIntSelRow);

                    }

                    mObjtxtTotal.Value = GetTotal().ToString();
                   // increaseHeadCounter(lStrEarringFrom.Substring(1), lStrEarringTo.Substring(1));

                }


            }
            else
            {
                Application.SBO_Application.StatusBar.SetText("Seleccionar una línea"
         , SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }

        }

        public void SelectControl(string pStrControlID)
        {
            if (pStrControlID == mObjtxtFrom.Item.UniqueID)
            {
                mObjtxtTo.Item.Click();
            }
            if (pStrControlID == mObjtxtTo.Item.UniqueID)
            {
                mObjtxtFrom.Item.Click();
            }
        }

      

        #endregion

        #region Queries
        public bool HasLines()
        {

            bool lBoolValid = false;
            mObjRanksForm.DataSources.DataTables.Add("DtRanks");
            SAPbouiCOM.DataTable lObjDataTable = mObjRanksForm.DataSources.DataTables.Item("DtRanks");
            mObjDtRanks = mObjRanksForm.DataSources.DataTables.Item("DtRanks");


            string lStrQuery = mObjPermissionFactory.GetPermissionsService().GetLinesQuery(mIntDocEntry.ToString());
            mObjRanksForm.DataSources.DataTables.Item("DtRanks").ExecuteQuery(lStrQuery);


            if (mObjRanksForm.DataSources.DataTables.Item("DtRanks").Rows.Count >= 1)
            {
                string y = mObjRanksForm.DataSources.DataTables.Item("DtRanks").Columns.Item("Desde").Cells.Item(0).Value.ToString();
                if (y == "")
                {
                    mObjDtRanks.Rows.Remove(0);
                    lBoolValid = false;
                }
                else
                {
                    lBoolValid = true;
                }
            }
            else
            {
                mObjDtRanks.Rows.Remove(0);
                lBoolValid = false;
            }


            initGrid();

            return lBoolValid;

        }




        #endregion

        #region validations
        private void ClearFields()
        {
            mObjtxtTo.Value = string.Empty;
            mObjtxtFrom.Value = string.Empty;
        }

        private bool ValidRanks(string lStrFrom, string lStrTo)
        {
            bool valid = false;

            if (mObjPermissionFactory.GetPermissionsService().CheckStoredRank(lStrFrom, lStrTo))
            {
                valid = false;
            }
            else
            {
                valid = true;
            }

            if (checkDeletedList(lStrFrom, lStrTo))
            {
                valid = true;
            }


            return valid;
        }

        private bool checkDeletedList(string lStrFrom, string lStrTo)
        {
            bool lBoolValid = false;
            foreach (var item in mLstDeletedRows)
            {
                if (Convert.ToInt32(item.EarringFrom.Substring(1)) < Convert.ToInt32(lStrFrom) || Convert.ToInt32(item.EarringTo.Substring(1)) > Convert.ToInt32(lStrFrom))
                {
                    if (Convert.ToInt32(item.EarringFrom.Substring(1)) < Convert.ToInt32(lStrTo) || Convert.ToInt32(item.EarringTo.Substring(1)) > Convert.ToInt32(lStrTo))
                    {
                        lBoolValid = true;
                    }
                    else
                    {
                        lBoolValid = false;
                        break;
                    }
                }
                else
                {
                    lBoolValid = false;
                    break;
                }
            }
            return lBoolValid;
        }

        internal void ValidateOnlyNumbers(int pCharPressed, string pStrUID)
        {

            SAPbouiCOM.EditText lObjETOnlyNumbers = ((SAPbouiCOM.EditText)mObjRanksForm.Items.Item(pStrUID).Specific);
            string lStrIntToChar = (Convert.ToChar(pCharPressed)).ToString();

            if (lObjETOnlyNumbers.Value.Length <= 5 && pCharPressed != 32)
            {
                if (!Regex.IsMatch(lObjETOnlyNumbers.Value, "^-?\\d*(\\.\\d+)?$") || lStrIntToChar == "-")
                {
                    lObjETOnlyNumbers.Value = new string(lObjETOnlyNumbers.Value.Where(c => char.IsDigit(c)).ToArray());
                }
                else
                {
                    mStrValue = lObjETOnlyNumbers.Value;
                }
            }
            else
            {
                lObjETOnlyNumbers.Value = mStrValue;
            }
        }

        public void Update(int pIntRow)
        {
            try
            {
                string lStrFrom = mObjDtRanks.GetValue("Desde", pIntRow).ToString();
                string lStrTo = mObjDtRanks.GetValue("Hasta", pIntRow).ToString();
                string lStrRowCode = mObjDtRanks.GetValue("Code", pIntRow).ToString();

                string ss = lStrFrom.Substring(4, lStrFrom.Count() - 4);
                if (ValidFields(lStrFrom.Substring(3, lStrFrom.Count() - 3), lStrTo.Substring(3, lStrTo.Count() - 3), true, pIntRow))
                {
                    EarringRanksT lObjEarringRanks = mLstEarRnksT.Where(x => x.Row == pIntRow).FirstOrDefault();
                    if (lObjEarringRanks != null)
                    {
                        lObjEarringRanks.EarringFrom = lStrFrom.Substring(3, lStrFrom.Count() - 3);
                        lObjEarringRanks.EarringTo = lStrTo.Substring(3, lStrTo.Count() - 3); 
                    }

                }

                if (lStrRowCode != "0")
                {
                    EarringRanksT lObjUptadeRank = mObjPermissionFactory.GetPermissionsService().GetEarring(lStrRowCode);
                    lObjUptadeRank.EarringFrom = lStrFrom.Substring(3, lStrFrom.Count() - 3);
                    lObjUptadeRank.EarringTo = lStrTo.Substring(3, lStrTo.Count() - 3);
                    mObjPermissionFactory.GetEarringRanksService().UpdateRanks(lObjUptadeRank);
                               
                }
            }

            catch (Exception ex)
            {
                LogService.WriteError(ex.Message);
                LogService.WriteError(ex);
            }

        }

        public bool ValidFields(string pStrFrom, string pStrTo, bool pBolIsGrid, int pIntRow)
        {
            bool lBoolValid = false;
            //if (mObjtxtFrom.Value != string.Empty )//&& mObjtxtTo.Value != string.Empty)
            //{
            //int lIntFrom = Convert.ToInt32(mObjtxtFrom.Value.Substring(1, mObjtxtFrom.Value.ToString().Count() - 1));
            int lIntFrom = Convert.ToInt32(pStrFrom.Substring(1, pStrFrom.ToString().Count() - 1));
            int lIntTo = 0;
            string lStrPref;
            char lStrFirstLeterFrom;
            char lStrFirstLeterTo;

            lStrFirstLeterFrom = pStrFrom[0];
            if (!string.IsNullOrEmpty(pStrTo))
            {
                lIntTo = Convert.ToInt32(pStrTo.Substring(1, pStrTo.Count() - 1));
                lStrFirstLeterTo = (pStrTo[0]);
                lStrPref = pStrTo.Substring(0, 1);
            }
            else
            {
                lIntTo = lIntFrom;
                lStrFirstLeterTo = lStrFirstLeterFrom;
                lStrPref = pStrFrom.Substring(0, 1);
            }

            if (char.IsLetter(lStrFirstLeterFrom) && char.IsLetter(lStrFirstLeterTo))
            {
                if (lIntTo >= lIntFrom)
                {
                    if (CheckGridValues(lIntFrom, lIntTo, lStrPref, pBolIsGrid, pIntRow))
                    {
                        //if (ValidRanks(lIntFrom.ToString(), lIntTo.ToString()))
                        //{
                        if (CheckHeadQuantity(lIntFrom, lIntTo))
                        {
                            lBoolValid = true;
                        }
                        //}
                        //else
                        //{
                        //    Application.SBO_Application.StatusBar.SetText("Ya se registro un rango similar"
                        //    , SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                        //    lBoolValid = false;
                        //}
                    }
                    else
                    {
                        Application.SBO_Application.StatusBar.SetText("Los rangos son incorrectos"
                        , SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                        lBoolValid = false;
                    }
                }
                else
                {
                    Application.SBO_Application.StatusBar.SetText("Agregar rangos válidos"
                    , SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    lBoolValid = false;
                }
            }
            else
            {
                Application.SBO_Application.StatusBar.SetText("Agregar la primera letra inicial"
                , SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                lBoolValid = false;
            }
            // }
            return lBoolValid;
        }

        private bool CheckHeadQuantity(int pIntFrom, int pIntTo)
        {
            bool lBolValid = false;
            //int lIntFrom =  Convert.ToInt32(mObjtxtFrom.Value.Substring(1, mObjtxtFrom.Value.ToString().Count() - 1));
            int lIntFrom = pIntFrom;// Convert.ToInt32(pStrFrom.Substring(1, pStrFrom.Count() - 1));


            int lIntTo = 0;
            if (pIntFrom != pIntTo)
            {
                lIntTo = pIntTo; //Convert.ToInt32(pStrTo.Substring(1, pStrTo.Count() - 1));
            }
            else
            {
                lIntTo = lIntFrom;
            }


            int lIntRankQuantity = (lIntTo - lIntFrom) + 1;
            int lInttotal = GetTotal();
            if (lInttotal <= mIntHeadsInCertificate)
            {
                lBolValid = true;
                mObjtxtTotal.Value = (lInttotal).ToString();
            }
            else
            {
                Application.SBO_Application.StatusBar.SetText("Supera el numero de cabezas por certificado"
                    , SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                lBolValid = false;
            }

            return lBolValid;
        }

        private bool CheckGridValues(int lIntFrom, int lIntTo, string pStrPref, bool pBolIsGrid, int pIntRow)
        {
            bool lBolValid = false;

            try
            {
                if (mObjRanksGrid.DataTable.Rows.Count >= 1)
                {
                    for (int i = 0; i < mObjRanksGrid.DataTable.Rows.Count; i++)
                    {
                        if (i != pIntRow || !pBolIsGrid)
                        {
                            string lStrDesde = mObjRanksGrid.DataTable.Columns.Item("Desde").Cells.Item(i).Value.ToString().Substring(4);
                            string lStrHasta = string.Empty;
                            if (!string.IsNullOrEmpty(mObjRanksGrid.DataTable.Columns.Item("Hasta").Cells.Item(i).Value.ToString()))
                            {
                                lStrHasta = mObjRanksGrid.DataTable.Columns.Item("Hasta").Cells.Item(i).Value.ToString().Substring(4);
                            }
                            else
                            {
                                lStrHasta = lStrDesde;
                            }
                            string lStrPrefi = mObjRanksGrid.DataTable.Columns.Item("Desde").Cells.Item(i).Value.ToString().Substring(3).Substring(0, 1);

                            if ((Convert.ToInt32(lStrDesde) > lIntFrom
                                || Convert.ToInt32(lStrHasta) < lIntFrom) || lStrPrefi != pStrPref)
                            {
                                if ((Convert.ToInt32(lStrDesde) > lIntTo
                                    || Convert.ToInt32(lStrHasta) < lIntTo) || lStrPrefi != pStrPref)
                                {
                                    lBolValid = true;
                                }
                                else
                                {
                                    lBolValid = false;
                                }
                            }
                            else
                            {
                                lBolValid = false;
                                break;
                            }
                        }
                        else
                        {
                            lBolValid = true;
                        }
                    }
                }
                else
                {
                    lBolValid = true;
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError(ex.Message);
                LogService.WriteError(ex);
            }
            return lBolValid;
        }
        #endregion



        
    }
}
