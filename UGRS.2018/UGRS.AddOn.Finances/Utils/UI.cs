using System;
using System.Globalization;

namespace UGRS.AddOn.Finances.Utils
{
    public static class UI
    {
        /// <summary>
        /// Gets the value of a DataSource, automatically handling conversion
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="objForm">The form where the datasource is in.</param>
        /// <param name="pUID">The UniqueId of the DataSource</param>
        /// <returns>The DataSource's value</returns>
        public static T GetDataSourceValue<T>(this SAPbouiCOM.Framework.FormBase objForm, string pUID) where T : IConvertible
        {
            string objValue = objForm.UIAPIRawForm.DataSources.UserDataSources.Item(pUID).ValueEx;
            if (objValue == "" && typeof(T) != typeof(string))
            {
                return default(T);
            }
            if (typeof(T) == typeof(DateTime))
            {
                
                DateTime lDtValue = DateTime.ParseExact(objValue, "yyyyMMdd", CultureInfo.InvariantCulture);
                objValue = lDtValue.ToString("MM/dd/yyyy");
            }
            return (T)Convert.ChangeType(objValue, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the value of datatable's cell, handing conversion.
        /// </summary>
        /// <typeparam name="T">The type of the stored value</typeparam>
        /// <param name="pDataTable">The datatable</param>
        /// <param name="pColumn">The column's id.</param>
        /// <param name="pRow">The row position.</param>
        /// <returns>The value in the cell</returns>
        public static T GetCellValue<T>(this SAPbouiCOM.DataTable pDataTable, string pColumn, int pRow) where T : IConvertible
        {
            object lObjValue = pDataTable.Columns.Item(pColumn).Cells.Item(pRow).Value;
            if (lObjValue == null)
            {
                return default(T);
            }
            return (T)Convert.ChangeType(lObjValue, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Binds a datable's columns to a matrix's columns.
        /// </summary>
        /// <remarks>Column names must match. Any column names that don't match will be silently ignored.</remarks>
        /// <param name="pDatatable">The datatable to bind</param>
        /// <param name="pMatrix">The matrix the datatable will be bound to.</param>
        public static void BindToMatrix(this SAPbouiCOM.DataTable pDatatable, SAPbouiCOM.Matrix pMatrix)
        {

            foreach (SAPbouiCOM.Column lObjColumn in pMatrix.Columns)
            {
                try
                {
                    lObjColumn.DataBind.Bind(pDatatable.UniqueID, lObjColumn.UniqueID);
                }
                catch (System.Runtime.InteropServices.COMException)
                {

                }

            }
        }

        /// <summary>
        /// Flushes a matrix cell to its bound datatable cell
        /// </summary>
        /// <remarks>This allows flushing a single cell instaed of flsuing the whole matrix.</remarks>
        /// <param name="objForm">The form where the matrix is found.</param>
        /// <param name="pMatrix">The matrix containing the cell.</param>
        /// <param name="pColumn">The id of the column.</param>
        /// <param name="pRow">The row number (One-based index)</param>
        public static void FlushValueToSource(this SAPbouiCOM.Framework.FormBase objForm, SAPbouiCOM.Matrix pMatrix, string pColumn, int pRow)
        {
            string lStrTableName = pMatrix.Columns.Item(pColumn).DataBind.TableName;
            if (lStrTableName == null)
            {
                return;
            }
            object lObjSpecific = pMatrix.GetCellSpecific(pColumn, pRow);
            string lStrValue = null;
            if (lObjSpecific is SAPbouiCOM.EditText)
            {
                lStrValue = (lObjSpecific as SAPbouiCOM.EditText).Value;
            }
            else if (lObjSpecific is SAPbouiCOM.ComboBox)
            {
                lStrValue = (lObjSpecific as SAPbouiCOM.ComboBox).Value;
            }
            else if (lObjSpecific is SAPbouiCOM.CheckBox)
            {
                lStrValue = (lObjSpecific as SAPbouiCOM.CheckBox).Checked ? "Y" : "N";
            }
            try
            {
                objForm.UIAPIRawForm.DataSources.DataTables.Item(lStrTableName).Columns.Item(pColumn).Cells.Item(pRow - 1).Value = lStrValue;
            }catch(System.Runtime.InteropServices.COMException){

            }
        }
    }
}
