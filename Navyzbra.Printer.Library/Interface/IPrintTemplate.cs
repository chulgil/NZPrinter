using Navyzbra.Printer.Library.Models;
using System;
using System.Drawing.Printing;

namespace Navyzbra.Printer.Library.Interface
{
    public interface IPrintTemplate
    {
        PrintContents Contents { get; set; }
        PrintPreference Preference { get; set; }

        void PrintPage(object sender, PrintPageEventArgs e);
    }
}