using System;
using System.Linq;

namespace Navyzbra.Printer.Library.Models
{
    public class PrintPreference
    {
        public readonly string PrinterName;
        public readonly System.Drawing.Font PrintFont;
        public readonly System.Drawing.Printing.PaperSize PaperSize;

        public PrintPreference(
            string printerName,
            System.Drawing.Printing.PaperSize paper = null,
            System.Drawing.Font font = null)
        {
            this.PrinterName = printerName;
            this.PaperSize = paper;
            this.PrintFont = font;
            if (paper == null) this.PaperSize = new System.Drawing.Printing.PaperSize("Roll Paper", 280, 3276);
            if (font == null) this.PrintFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
        }
    }
}