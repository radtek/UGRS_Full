using System;
using SAPbouiCOM.Framework;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.DI.Purchases;
using UGRS.Core.Services;
using System.IO;

namespace UGRS.AddOn.Purchases
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application oApp = null;
                if (args.Length < 1)
                {
                    oApp = new Application();
                }
                else
                {
                    oApp = new Application(args[0]);
                }
                LogService.Filename("AddOnCompras");
               
                
                
                Menu MyMenu = new Menu();
                MyMenu.AddMenuItems();
                oApp.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);
                DIApplication.DIConnect((SAPbobsCOM.Company)Application.SBO_Application.Company.GetDICompany());
                Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
                //TestXML();
                //Initialize Tables 
                UIApplication.ShowSuccess(string.Format("Inicializar las tablas"));
                PurchasesServiceFactory lObjFoodProductionFactory = new PurchasesServiceFactory();
                lObjFoodProductionFactory.GetSetupService().InitializeTables();
               UIApplication.ShowSuccess(string.Format("AddonCompras 1.2.59 iniciado correctamente"));
                LogService.WriteSuccess(string.Format("AddonCompras 1.2.59 iniciado correctamente"));
                oApp.Run();
              
               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                System.Windows.Forms.Application.Exit();
            }
        }

        /*static void TestXML()
        {
            string lStrUri = @"C:\Users\amartinez\Documents\Proyectos\UGRS\Compras\XML\XML Enero- Feb";
            DirectoryInfo d = new DirectoryInfo(lStrUri);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.xml"); //Getting Text files
            DateTime mDblTimeGEneral = DateTime.Now;
            int i = 0;
            int Correcto = 0;
            int incorrecto = 0;
            foreach (FileInfo file in Files)
            {
                DateTime mDblTime = DateTime.Now;
                LogService.WriteInfo (i + " de " + Files.Length + "Leyendo archivo" + file.Name);
                UGRS.AddOn.Purchases.Services.ReadXMLService lObjReadXML = new Services.ReadXMLService();
                UGRS.Core.SDK.DI.Purchases.DTO.PurchaseXMLDTO lOBj =  lObjReadXML.ReadXML(file.FullName);

                if (lOBj == null || string.IsNullOrEmpty(lOBj.Total))
                {
                    LogService.WriteError("Carga fallida" + file.Name);
                    incorrecto++;
                }
                else
                {
                    LogService.WriteSuccess("Lectura correcta");
                    Correcto++;
                }

                LogService.WriteInfo("Correcto: " + Correcto + " Incorrecto: " + incorrecto);
                TimeSpan lTmsTime = DateTime.Now - mDblTime;
                LogService.WriteInfo(lTmsTime.Seconds + "." + lTmsTime.Milliseconds);
                i++;
            }
            TimeSpan lTmsTimeGral = DateTime.Now - mDblTimeGEneral;
            LogService.WriteInfo(lTmsTimeGral.Seconds + "." + lTmsTimeGral.Milliseconds);
        }*/

        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    //Exit Add-On
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    break;
                default:
                    break;
            }
        }
    }
}
