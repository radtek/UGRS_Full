using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using UGRS.AddOn.Finances.Entities;
using UGRS.AddOn.Finances.Formatters;
using UGRS.AddOn.Finances.Services;
using UGRS.AddOn.Finances.Utils;
using UGRS.Core.SDK.DI.Finances.DAO;
using UGRS.Core.SDK.DI.Finances.DTO;
using UGRS.Core.SDK.DI.Finances.Services;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.UI.ProgressBar;
using UGRS.Core.Utility;


namespace UGRS.AddOn.Finances
{
    public class BankExtractsImporting
    {
        private string STR_SEARCH_UID = "18";
        private string STR_IMPORT_BUTTON_UID = "btnImport";
        private ExtractFormatDTO mObjExtractFormat;
        private ExtractFormatter mObjExtractFormatter;

        private SAPbouiCOM.EditText mEdtAccountField;
        private SAPbouiCOM.Button mBtnImport;
        private SAPbouiCOM.StaticText mTxtSelectedFormat;

        private ExtractFormatDAO mObjExtractFormatDAO = new ExtractFormatDAO();

        private SAPbouiCOM.Form mObjForm;

        private SAPbobsCOM.Company mObjCompany = null;
        private SAPbobsCOM.BankPages mObjBankPage = null;
        private ProgressBarManager mObjProgressBar = null;
        string lStrAccountCode = "";
        int mIntCountCarga = 0;
        int mIntTotalCarga = 0;
        SetupService mObjSetupService = new SetupService();


        public BankExtractsImporting(SAPbouiCOM.Company pObjCompany, SAPbouiCOM.Form pForm)
        {
            mObjCompany = (SAPbobsCOM.Company)pObjCompany.GetDICompany();
            mObjForm = pForm;
            LoadForm();
        }

        private void LoadForm()
        {
            SAPbouiCOM.Item lItmLblBanco = null;
            mEdtAccountField = mObjForm.Items.Item(STR_SEARCH_UID).Specific as SAPbouiCOM.EditText;

            lItmLblBanco = mObjForm.Items.Add("txtFormat", SAPbouiCOM.BoFormItemTypes.it_STATIC);
            SAPbouiCOM.Item lItmBtnImport = mObjForm.Items.Add(STR_IMPORT_BUTTON_UID, SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            mBtnImport = lItmBtnImport.Specific as SAPbouiCOM.Button;
            mTxtSelectedFormat = lItmLblBanco.Specific as SAPbouiCOM.StaticText;

            lItmLblBanco.Top = mObjForm.Items.Item("6").Top;
            lItmLblBanco.Left = mObjForm.Items.Item("6").Left + 40;
            lItmLblBanco.Width = 200;
            lItmLblBanco.Visible = false;

            lItmBtnImport.Top = lItmLblBanco.Top + 15;
            lItmBtnImport.Left = lItmLblBanco.Left;
            lItmBtnImport.Visible = false;
            mBtnImport.Caption = "Importar";

            LoadEvents();
        }

        private void LoadEvents()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void UnloadEvents()
        {
            Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.FormUID != mObjForm.UniqueID)
                {
                    return;
                }
                if (!pVal.BeforeAction)
                {
                    switch (pVal.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_VALIDATE:
                            if (pVal.ItemUID == mEdtAccountField.Item.UniqueID)
                            {
                                string lStrGLAcct = (mObjForm.Items.Item(STR_SEARCH_UID).Specific as SAPbouiCOM.EditText).Value;
                                mObjExtractFormat = mObjExtractFormatDAO.GetAccountExtractFormat(lStrGLAcct);
                                mBtnImport.Item.Visible = mObjExtractFormat != null;
                                mTxtSelectedFormat.Item.Visible = mObjExtractFormat != null;
                                if (mObjExtractFormat != null)
                                {
                                    mTxtSelectedFormat.Caption = string.Format("Formato: {0}", mObjExtractFormat.Name);
                                }
                            }
                            break;
                        case SAPbouiCOM.BoEventTypes.et_CLICK:
                            if (pVal.ItemUID == mBtnImport.Item.UniqueID)
                            {
                                if (mEdtAccountField.Value != "")
                                {
                                    // Based on the sellected account's bank, select the proper formatter.
                                    switch (mObjExtractFormat.Code)
                                    {
                                        case "BANAMEX":
                                            mObjExtractFormatter = new BanamexFormatter();
                                            break;
                                        case "BANCOMER":
                                            mObjExtractFormatter = new BancomerFormatter();
                                            break;
                                        case "BANORTE":
                                            mObjExtractFormatter = new BanorteFormatter();
                                            break;
                                        case "SANTANDER":
                                            mObjExtractFormatter = new SantanderFormatter();
                                            break;
                                        case "SCOTIABANK":
                                            mObjExtractFormatter = new ScotiabankFormatter();
                                            break;
                                        default:
                                            Application.SBO_Application.MessageBox("Formato no soportado.");
                                            return;
                                    }
                                    showOpenFileDialog();
                                }
                                else
                                {
                                    Application.SBO_Application.MessageBox("No se selecciono una Cuenta Contable.");
                                }
                            }
                            break;
                    }
                }
                else
                {
                    switch (pVal.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                            UnloadEvents();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message);
            }
        }
        
        /// <summary>
        /// Reads the contents of the selected file and parses the bank pages.
        /// </summary>
        /// <param name="pStrPath">The absolute path to the file.</param>
        private void ReadFile(string pStrPath)
        {
            mIntCountCarga = 0;
            mIntTotalCarga = 0;
            try
            {
                IList<BankStatement> mLstObjLines = mObjExtractFormatter.ParseFile(pStrPath, mEdtAccountField.Value);
                mObjProgressBar = new ProgressBarManager(Application.SBO_Application, "Cargando extractos bancarios", mLstObjLines.Count);
                foreach (BankStatement lObjlObjExtractBanking in mLstObjLines)
                {
                    mObjBankPage = PopulateBankPages(lObjlObjExtractBanking);
                    int result = mObjBankPage.Add();
                    mIntTotalCarga++;
                    if (result == 0)
                        mIntCountCarga++;
                    if (result != 0)
                    {
                        Application.SBO_Application.MessageBox("No se cargo el movimiento número: " + mIntTotalCarga);
                        Application.SBO_Application.StatusBar.SetText("No se cargo el movimiento número: " + mIntTotalCarga, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    }
                    mObjProgressBar.NextPosition();
                }
                ((SAPbouiCOM.EditText)mObjForm.Items.Item(STR_SEARCH_UID).Specific).Value = "";
                ((SAPbouiCOM.EditText)mObjForm.Items.Item(STR_SEARCH_UID).Specific).Value = lStrAccountCode;
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                LogUtility.WriteError(string.Format("[BankExtractsImporting - ReadFile] Error al leer el archivo: {0}", ex.Message));

            }
            finally
            {
                mObjForm.Freeze(false);
                MemoryUtility.ReleaseComObject(mObjBankPage);
            }
            Application.SBO_Application.MessageBox("Proceso Terminado. Se cargaron " + mIntCountCarga + " de " + mIntTotalCarga + " movimientos bancarios.");
        }

        /// <summary>
        /// Converts a Bank Extract line to a Bank Page.
        /// </summary>
        /// <param name="pObjExtractBanking"></param>
        /// <returns></returns>
        private SAPbobsCOM.BankPages PopulateBankPages(BankStatement pObjExtractBanking)
        {
            mObjBankPage = (SAPbobsCOM.BankPages)mObjCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBankPages);

            mObjBankPage.AccountCode = pObjExtractBanking.AccountCode;
            mObjBankPage.DueDate = pObjExtractBanking.Date;
            mObjBankPage.Reference = pObjExtractBanking.Reference;
            mObjBankPage.Memo = pObjExtractBanking.Detail;

            if (pObjExtractBanking.DebitAmount > 0)
            {
                mObjBankPage.DebitAmount = pObjExtractBanking.DebitAmount;
            }

            if (pObjExtractBanking.CreditAmount > 0)
            {
                mObjBankPage.CreditAmount = pObjExtractBanking.CreditAmount;
            }

            return mObjBankPage;
        }

        /// <summary>
        /// Shows the Open File Dialog.
        /// </summary>
        private void showOpenFileDialog()
        {
            try
            {
                Thread ShowFolderBroserThread = new Thread(ShowFolderBrowser);
                if (ShowFolderBroserThread.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    ShowFolderBroserThread.SetApartmentState(System.Threading.ApartmentState.STA);
                    ShowFolderBroserThread.Start();
                }
                else
                {
                    ShowFolderBroserThread.Start();
                    ShowFolderBroserThread.Join();

                }
                while (ShowFolderBroserThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[BankExtractsImporting - showOpenFileDialog] Error al abrir FileDialog: {0}", ex.Message));
                Application.SBO_Application.MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private void ShowFolderBrowser()
        {
            using (System.Windows.Forms.OpenFileDialog oFile = new System.Windows.Forms.OpenFileDialog())
            {
                string fileName = "";
                try
                {
                    IntPtr sapProc = GetForegroundWindow();
                    WindowWrapper MyWindow = null;

                    MyWindow = new WindowWrapper(sapProc);

                    oFile.Multiselect = false;
                    oFile.Filter = mObjExtractFormatter.GetFileDialogFilter();
                    oFile.Title = mObjExtractFormatter.GetFileDialogTitle();
                    oFile.RestoreDirectory = true;
                    var dialogResult = oFile.ShowDialog(MyWindow);

                    if (dialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        fileName = oFile.FileName;
                        string lStrExtencion = Path.GetExtension(fileName);
                        if (mObjExtractFormatter == null)
                        {
                            UIApplication.ShowMessageBox("Formato no soportado.");
                        }
                        ReadFile(fileName);
                    }
                    else
                    {
                        System.Windows.Forms.Application.ExitThread();
                    }
                }
                catch (Exception e)
                {
                    fileName = "";
                    UIApplication.ShowMessageBox("El archivo seleccionado tiene un formato incorrecto.");
                    LogUtility.WriteError(string.Format("[BankExtractsImporting - ShowFolderBrowser] Error al abrir FolderBrowser: {0}", e.Message));
                    //BankStatementsLogService.WriteError(e.Message);
                    System.Windows.Forms.Application.ExitThread();
                }
            }
        }
    }
}
