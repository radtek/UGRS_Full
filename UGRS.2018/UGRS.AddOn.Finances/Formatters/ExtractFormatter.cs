using System.Collections.Generic;
using System.Windows.Forms;
using UGRS.AddOn.Finances.Entities;

namespace UGRS.AddOn.Finances.Formatters
{
    /// <summary>
    /// Represents a bank extract formatter.
    /// </summary>
    public abstract class ExtractFormatter
    {
        /// <summary>
        /// Gets the valid file extensions filter.
        /// </summary>
        /// <returns>The file filter.</returns>
        public abstract string GetFileDialogFilter();

        /// <summary>
        /// Gets the displayed title for the select file dialog.
        /// </summary>
        /// <returns>The dialog title.</returns>
        public abstract string GetFileDialogTitle();

        /// <summary>
        /// Parses a bank extract file into a list of bank movements.
        /// </summary>
        /// <param name="pPath">Absolute path to the file.</param>
        /// <param name="pAcctCode">The selected GL Account.</param>
        /// <returns>A list of parses bank movements.</returns>
        public abstract IList<BankStatement> ParseFile(string pPath, string pAcctCode);
    }
}

