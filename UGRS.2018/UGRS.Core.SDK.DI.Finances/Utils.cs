using System;
using System.Collections.Generic;
using System.Globalization;
using UGRS.Core.Extension;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Finances
{
    public static class Utils
    {
        /// <summary>
        /// Creates a User Defined Field if it doesn't exist.
        /// </summary>
        /// <param name="pTableName">The table where the field will be created.</param>
        /// <param name="pFieldName">The name of the field.</param>
        /// <param name="pDescription">The description (display name) of the field.</param>
        /// <param name="pType">The field's type</param>
        /// <param name="pSize">The field's size</param>
        /// <returns>The result obtained from adding the field, or -1 if it already exists.</returns>
        public static int CreateUserField(string pTableName, string pFieldName, string pDescription, SAPbobsCOM.BoFieldTypes pType, int pSize)
        {
            SAPbobsCOM.UserFieldsMD lObjUserFields = (SAPbobsCOM.UserFieldsMD)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
            if (!ExistsUFD(pTableName, pFieldName))
            {
                lObjUserFields.TableName = pTableName;
                lObjUserFields.Name = pFieldName;
                lObjUserFields.Description = pDescription;
                lObjUserFields.Type = pType;
                lObjUserFields.Size = pSize;
                lObjUserFields.EditSize = pSize;

                return lObjUserFields.Add();
            }
            return -1;
        }

        /// <summary>
        /// Checks if a User Defined Field exists.
        /// </summary>
        /// <param name="tableName">The table of the field.</param>
        /// <param name="ufdName">The name of the field.</param>
        /// <returns><c>true</c> if it exists, <c>false</c> otherwise.</returns>
        public static bool ExistsUFD(string tableName, string ufdName)
        {
            SAPbobsCOM.Recordset rs = DIApplication.GetRecordset();
            try
            {
                rs.DoQuery(string.Format("SELECT \"AliasID\" FROM \"CUFD\" WHERE \"TableID\" = '{0}' AND \"AliasID\" = '{1}'", tableName, ufdName));
                if (rs.RecordCount > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                
            }
            finally
            {
                MemoryUtility.ReleaseComObject(rs);
            }
            return false;
        }

        /// <summary>
        /// Gets the value of a column, converted to the desired value.
        /// </summary>
        /// <typeparam name="T">The type of the column.</typeparam>
        /// <param name="pRecordSet">The recordeset.</param>
        /// <param name="pColumn">The name of the column.</param>
        /// <returns>The value of the column.</returns>
        public static T GetColumnValue<T>(this SAPbobsCOM.Recordset pRecordSet, string pColumn) where T : IConvertible
        {
            object lObjValue = pRecordSet.Fields.Item(pColumn).Value;
            if (lObjValue == null)
            {
                return default(T);
            }
            return (T)Convert.ChangeType(lObjValue, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
