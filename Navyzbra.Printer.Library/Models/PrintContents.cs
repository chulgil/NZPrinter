using System;
using System.Linq;

namespace Navyzbra.Printer.Library.Models
{
    public class PrintContents
    {
        readonly string header;
        readonly string body;
        readonly string footer;
        public string Header { get { return this.header; } }
        public string Body { get { return this.body; } }
        public string Footer { get { return this.footer; } }

        public PrintContents(string header="", string body="", string footer="")
        {
            this.header = header;
            this.body = body;
            this.footer = footer; 
        }
    }
}
