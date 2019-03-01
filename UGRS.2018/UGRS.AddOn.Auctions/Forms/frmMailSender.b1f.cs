using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.Core.SDK.DI.Auctions;
using UGRS.Core.SDK.DI.Auctions.DTO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.SDK.DI;

namespace UGRS.AddOn.Auctions.Forms
{
    [FormAttribute("UGRS.AddOn.Auctions.Forms.frmMailSender", "Forms/frmMailSender.b1f")]
    class frmMailSender : UserFormBase
    {
        MailSenderServiceFactory mObjMailSenderServiceFactory = new MailSenderServiceFactory();
        string lStrUserName = DIApplication.Company.UserName;


        public frmMailSender()
        {
            LoadEvents();
            LoadInitialObjects();
            LoadLastAuction();
            LoadMatrix();
        }

        #region Load & Unload Events
        private void LoadEvents()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void UnLoadEvents()
        {
            Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            lObjComboAuctions.ComboSelectAfter -= new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cboFolio_ComboSelectAfter);
        }
        #endregion

        #region Events
        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (FormUID.Equals(this.UIAPIRawForm.UniqueID))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_CLICK:
                                if (pVal.ItemUID.Equals("btnSearch"))
                                {
                                    SearchSellers();
                                }
                                else if (pVal.ItemUID.Equals("btnSend"))
                                {
                                    if(CheckValidMails())
                                    SendMails();
                                }
                                break;

                            case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED:
                                if (pVal.ItemUID.Equals("chk_All"))
                                {
                                    SelectAllSellers();
                                }
                                break;

                            case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS:
                                if (pVal.ColUID.Equals("Mail"))
                                {
                                    ValidMail(pVal.Row);
                                }
                                break;

                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                UnLoadEvents();
                                break;
                        }
                    }
       
                }
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(string.Format("ItemEventException: {0}", ex.Message));
            }
        }

        private void cboFolio_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            LoadMatrix();
        }

        #endregion

        #region Methods
        private void LoadInitialObjects()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DsSellers");
            lObjDsSellers = null;
        }

        private void LoadLastAuction()
        {
            try
            {
                //lObjEdTxtAuction.Value = mObjMailSenderServiceFactory.GetAuctionSellersService().GetLastAuction();
                List<string> lLstAuctions = mObjMailSenderServiceFactory.GetAuctionSellersService().GetLastAuctions(GetCostingCode());

                foreach (var lStrFolio in lLstAuctions)
                {
                    lObjComboAuctions.ValidValues.Add(lStrFolio, "");
                    lObjComboAuctions.Item.DisplayDesc = false;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError("frmMailSender (LoadCboAuctions) " + ex.Message);
                LogService.WriteError("frmMailSender (LoadCboAuctions) " + ex.Message);
                LogService.WriteError(ex);
            }



        }

        private string GetCostingCode()
        {
           return mObjMailSenderServiceFactory.GetMailSenderService().GetCostingCode(lStrUserName);
        }

        private void SelectAllSellers()
        {

            for (int i = 1; i <= lObjMtxSellers.RowCount; i++)
            {

                lObjChkBx = (SAPbouiCOM.CheckBox)lObjMtxSellers.Columns.Item("ColSlct").Cells.Item(i).Specific;
                lObjChkBx.Caption = lObjChckAll.Checked ? "Y" : "N";
            }

        }

        private void SearchSellers()
        {
            LoadMatrix();
        }

        private void LoadMatrix()
        {
            FillDataSource();

            lObjMtxSellers.Columns.Item("ColSlct").DataBind.Bind("DsSellers", "Checked");
            lObjMtxSellers.Columns.Item("Seller").DataBind.Bind("DsSellers", "CardName");
            lObjMtxSellers.Columns.Item("Amount").DataBind.Bind("DsSellers", "Amount");
            lObjMtxSellers.Columns.Item("Mail").DataBind.Bind("DsSellers", "E_Mail");

            lObjMtxSellers.LoadFromDataSource();
            lObjMtxSellers.AutoResizeColumns();
        }

        private void FillDataSource()
        {
            this.UIAPIRawForm.DataSources.DataTables.Item("DsSellers").ExecuteQuery(mObjMailSenderServiceFactory.GetAuctionSellersService().GetSellers(lObjComboAuctions.Value));
            lObjDsSellers = this.UIAPIRawForm.DataSources.DataTables.Item("DsSellers");
        }

        private bool CheckValidMails()
        {
            for (int i = 1; i <= lObjMtxSellers.RowCount; i++)
            {
                 if (((dynamic)lObjMtxSellers.Columns.Item(1).Cells.Item(i).Specific).Checked)
                 {
                     string lStrMail =(string)(lObjMtxSellers.Columns.Item("Mail").Cells.Item(i).Specific as SAPbouiCOM.EditText).Value.Trim();
                     if (!string.IsNullOrEmpty(lStrMail))
                     {
                         if (!ValidMail(i))
                         {
                             return false;
                         }
                     }
                     else
                     {
                         Application.SBO_Application.StatusBar.SetText("Favor de verificar los correos"
                                          , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                         return false;
                     }
                 }
            }

            return true;
        }

        private bool ValidMail(int lIntRow)
        {
            string lStrMail = (string)(lObjMtxSellers.Columns.Item("Mail").Cells.Item(lIntRow).Specific as SAPbouiCOM.EditText).Value.Trim();
            bool lboolValid = false;

            try
            {
                if (!string.IsNullOrEmpty(lStrMail))
                {
                    var lVarAddress = new System.Net.Mail.MailAddress(lStrMail);
                    lboolValid = lVarAddress.Address == lStrMail;
                }
                else
                {
                    lboolValid = true;
                }
            }
            catch (Exception)
            {
                lboolValid = false;
            }


            if (!lboolValid)
            {
                Application.SBO_Application.StatusBar.SetText("La direccion de correo no es valida en linea: " + lIntRow.ToString()
                                           , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                //lObjMtxSellers.Columns.Item("Mail").Cells.Item(lIntRow).Click();
            }

            return lboolValid;
        }

        private void SendMails()
        {
            

            List<SellerSenderDTO> lLstStrSellers = new List<SellerSenderDTO>();
            try
            {
                this.UIAPIRawForm.Freeze(true);

                for (int i = 1; i <= lObjMtxSellers.RowCount; i++)
                {
                    if (((dynamic)lObjMtxSellers.Columns.Item(1).Cells.Item(i).Specific).Checked)
                    {
                        lLstStrSellers.Add(new SellerSenderDTO()
                        {
                            Seller = (string)lObjDsSellers.GetValue("CardCode", i - 1),
                            Mail = (string)(lObjMtxSellers.Columns.Item("Mail").Cells.Item(i).Specific as SAPbouiCOM.EditText).Value.Trim()
                        });
                    }
                }

                if (lLstStrSellers.Count > 0)
                {
                    mObjMailSenderServiceFactory.GetAuctionSellersService().GetSellersBatches(lLstStrSellers, lObjComboAuctions.Value);
                    Application.SBO_Application.StatusBar.SetText("El envío se realizo correctamente"
                                                           , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
            }
            catch (Exception)
            {
                Application.SBO_Application.StatusBar.SetText("Hubo un error en el envío"
                                                           , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }


            //mObjMailSenderServiceFactory.GetAuctionSellersService().GenerateReport()
        }
        #endregion

        #region Components
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lObjMtxSellers = ((SAPbouiCOM.Matrix)(this.GetItem("mtxSllrs").Specific));
            this.lObjStAuction = ((SAPbouiCOM.StaticText)(this.GetItem("lblAuct").Specific));
            this.lObjBtnSend = ((SAPbouiCOM.Button)(this.GetItem("btnSend").Specific));
            this.lObjChckAll = ((SAPbouiCOM.CheckBox)(this.GetItem("chk_All").Specific));
            this.lObjBtnSearch = ((SAPbouiCOM.Button)(this.GetItem("btnSearch").Specific));
            this.lObjComboAuctions = ((SAPbouiCOM.ComboBox)(this.GetItem("cboAuct").Specific));
            this.lObjComboAuctions.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cboFolio_ComboSelectAfter);
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

        private SAPbouiCOM.ComboBox lObjComboAuctions;
        private SAPbouiCOM.Matrix lObjMtxSellers;
        private SAPbouiCOM.StaticText lObjStAuction;
        private SAPbouiCOM.Button lObjBtnSend;
        private SAPbouiCOM.CheckBox lObjChckAll;
        private SAPbouiCOM.Button lObjBtnSearch;
        private SAPbouiCOM.DataTable lObjDsSellers;
        private SAPbouiCOM.CheckBox lObjChkBx;
        #endregion
  
    }
}
