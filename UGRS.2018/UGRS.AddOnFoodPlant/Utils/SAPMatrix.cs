/*
 * Autor: LCC Abraham SaÚL Sandoval Meneses
 * Description: SAP B1 Matrix
 * Date: 04/09/2018
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;
using System.Collections.Concurrent;
using UGRS.Core.SDK.UI;
using QualisysLog;
using UGRS.AddOnFoodPlant.Forms;
using SAPbouiCOM.Framework;

namespace UGRS.AddOnFoodPlant.Utils {
  
    public class SAPMatrix {
        public static void Fill<T>(string tableID, DataTable dataTable, Matrix mtx, List<string> columns, T[] data) {

            try {
                if(!Object.ReferenceEquals(data, null)) {

                    dataTable.Rows.Clear();

                    Parallel.For(0, data.Length, row => {
                        dataTable.Rows.Add();
                    });

                    Task.Factory.StartNew(() => {
                        Parallel.For(0, data.Length, row => {
                            dataTable.SetValue("C_#", row, row + 1);
                        });
                    });

                    Parallel.ForEach(Partitioner.Create(0, data.Length), (range, state) => {
                        for(int i = range.Item1; i < range.Item2; i++) {
                            Parallel.ForEach(columns.Skip(1), column => {
                                dataTable.SetValue("C_" + column, i, data[i].GetType().GetProperty(column).GetValue(data[i], null));
                            });
                        }
                    });
                    Bind(mtx, tableID, columns);
                }
                else {
                    ClearMtx(mtx);
                }
            }
            catch(AggregateException ae) {
                ae.Handle(e => {
                    SAPException.Handle(e, "(AE)");
                    return true;
                });
            }
            catch(Exception ex) {
                SAPException.Handle(ex, "(FillMatrix0)");
            }
        }

        public static void Bind(Matrix mtx, string tableID, List<string> columns) {

            Parallel.ForEach(columns, column => {
                mtx.Columns.Item("C_" + column).DataBind.Bind(tableID, "C_" + column);
            });

            mtx.LoadFromDataSource();
            mtx.AutoResizeColumns();
        }

        public static void ClearMtx(Matrix mtx) {
            mtx.Clear();
            mtx.AutoResizeColumns();
        }

        public static DataTable CreateDataTable(string tableID, Dictionary<string, BoFieldsType> columns, UserFormBase frm) {
            DataTable dataTable = null;
            try {
                frm.UIAPIRawForm.DataSources.DataTables.Add(tableID);
                dataTable = frm.UIAPIRawForm.DataSources.DataTables.Item(tableID);

                columns.AsParallel().ForAll(column => {
                    dataTable.Columns.Add("C_" + column.Key, column.Value);
                });
            }
            catch(Exception ex) {
                SAPException.Handle(ex, "(Create DataTable)");
            }
            return dataTable;
        }
    }
}
