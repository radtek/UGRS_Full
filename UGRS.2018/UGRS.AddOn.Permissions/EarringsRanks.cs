using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.Permissions;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.AddOn.Permissions
{
    public class EarringsRanks
    {

        SAPbouiCOM.Form mObjEarringsForm = null;
        SAPbouiCOM.Item mObjAddRank = null;
        SAPbouiCOM.EditText mObjETxtBaseEntry = null;


        mFormEarringRanks pObjMFrmEarringR = null;
        
        //DAO.EarringRanksDAO lObjEarringRanksDAO = new DAO.EarringRanksDAO();
        PermissionsFactory mObjPermisssionsFactory = new PermissionsFactory();
        Menu pObjMenu = new Menu();

        public EarringsRanks()
        {
            try
            {
                if (DIApplication.Company.Connected)
                {
                    LoadEvents();
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("EarringsRanks" + ex.Message);
                LogService.WriteError(ex);
            }
        }

        private void SetEditTxtBE()
        {
            mObjETxtBaseEntry = ((SAPbouiCOM.EditText)mObjEarringsForm.Items.Item("8").Specific);
        }

        #region Load & Unload Events
        private void LoadEvents()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void UnLoadEvents()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }
        #endregion

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.FormType.Equals(139))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_FORM_LOAD:
                                mObjEarringsForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);
                                AddButton();
                                break;

                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                UnLoadEvents();
                                pObjMenu.lStrTypeEx = "";
                                pObjMenu.lIntTypeCount = 0;

                                var lObjForms = SAPbouiCOM.Framework.Application.SBO_Application.Forms;

                                foreach (Form lObjForm in lObjForms)
                                {
                                    if (lObjForm.UniqueID == "mFrmEarringRanks")
                                    {
                                        SAPbouiCOM.Framework.Application.SBO_Application.Forms.Item("mFrmEarringRanks").Close();
                                    }
                                }

                                break;  
                              
                        }

                        //var lObjForms2 = SAPbouiCOM.Framework.Application.SBO_Application.Forms;

                        //foreach (Form lObjForm in lObjForms2)
                        //{
                        //    if (lObjForm.UniqueID == "mFrmEarringRanks")
                        //    {
                        //        SAPbouiCOM.Framework.Application.SBO_Application.Forms.Item("mFrmEarringRanks").Select();
                        //    }
                        //}
                        
                    }
                    else
                    {
                       

                    }
                }
                else if (FormUID.Equals("mFrmEarringRanks"))
                {
                    
                    if (!pVal.BeforeAction && pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_ACTIVATE)
                    {

                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_CLICK:
                                if (pVal.ItemUID.Equals("BtnAdd"))
                                {
                                    pObjMFrmEarringR.AddRow();
                                }
                                else if (pVal.ItemUID.Equals("BtnOk"))
                                {
                                    pObjMFrmEarringR.SaveRanks();
                                    SAPbouiCOM.Framework.Application.SBO_Application.Forms.Item("mFrmEarringRanks").Close();
                                }
                                else if (pVal.ItemUID.Equals("btnDel"))
                                {
                                    pObjMFrmEarringR.CancelRow();
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_KEY_DOWN:

                                int i = 0;
                                if (pVal.CharPressed == (char)System.Windows.Forms.Keys.Enter)
                                {
                                    if (pVal.ItemUID.Equals("TxtFrom"))
                                    {
                                        pObjMFrmEarringR.SelectControl(pVal.ItemUID);
                                    }
                                    if (pVal.ItemUID.Equals("TxtTo"))
                                    {
                                        pObjMFrmEarringR.AddRow();
                                        pObjMFrmEarringR.SelectControl(pVal.ItemUID);
                                       
                                    }

                                }
                                break;
                            //pObjMFrmEarringR.ValidateOnlyNumbers(pVal.CharPressed, pVal.ItemUID);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                LogService.WriteError("SBO_Application_ItemEvent" + ex.Message);
                LogService.WriteError(ex);
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(string.Format("ItemEventException: {0}", ex.Message));
            }

        }

        private void AddButton()
        {
            try
            {
                SetEditTxtBE();
                mObjAddRank = mObjEarringsForm.Items.Add("btnRank", SAPbouiCOM.BoFormItemTypes.it_BUTTON);

                (mObjAddRank.Specific as Button).Caption = "Rangos";
               

              
                mObjAddRank.Top = 120;

                mObjAddRank.Left = mObjEarringsForm.Width - 200;

                SAPbouiCOM.Button lBtnAddRank = ((SAPbouiCOM.Button)mObjEarringsForm.Items.Item("btnRank").Specific);
                lBtnAddRank.ClickBefore += lBtnAddRank_ClickBefore;
            }
            catch (Exception ex)
            {
                LogService.WriteError("AddButton" + ex.Message);
                LogService.WriteError(ex);
            }
        
        }

        void lBtnAddRank_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {

            BubbleEvent = true;
            try
            {
                
                if(!UIApplication.IsActiveForm(mObjEarringsForm.UniqueID)){
                mObjEarringsForm = UIApplication.GetApplication().Forms.ActiveForm;
                SetEditTxtBE();
                }
                if (mObjPermisssionsFactory.GetPermissionsService().CheckBaseEntry(mObjETxtBaseEntry.Value))
                {
                    pObjMFrmEarringR = new mFormEarringRanks(mObjETxtBaseEntry.Value,
                        mObjEarringsForm.Top + mObjEarringsForm.Height / 2,
                        mObjEarringsForm.Left + mObjEarringsForm.Width / 2);
                    
                }
                else
                {
                    UIApplication.ShowError("Favor de guardar la orden de venta");
                }
            }
            catch (Exception ex) 
            {
                LogService.WriteError("lBtnAddRank_ClickBefore" + ex.Message);
                LogService.WriteError(ex);
            }
        }

    }
}
