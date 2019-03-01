using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UGRS.AddOn.Finances.Utils
{
    public class XmlLoader
    {
        public static SAPbouiCOM.FormCreationParams LoadFromXml(string pName)
        {
            XmlDocument lObjXmlDoc = new XmlDocument();

            Type lObjBaseType = typeof(Program);

            if (lObjBaseType.Assembly.IsDynamic)
                lObjBaseType = lObjBaseType.BaseType;

            string lStrNamespace = lObjBaseType.Namespace;
            string lStrPath = lStrNamespace + ".XmlForms." + pName + ".xml";
            string lStrContent = "";
            using (var lObjStream = lObjBaseType.Assembly.GetManifestResourceStream(lStrPath))
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

            return lObjCreationPackage;
        }
    }
}
