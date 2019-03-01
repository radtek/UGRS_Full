using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace UGRS.Core.Extension
{
    public static class MatrixExtension
    {
        public static T FillMatrix<T>(this T lObjSource)
        {
            T lObjCopy = (T)Activator.CreateInstance(typeof(T));

            return lObjCopy;
        }

    }
}
