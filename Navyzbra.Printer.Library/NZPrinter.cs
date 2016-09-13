using System;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Navyzbra.Printer.Library.Interface;
using Navyzbra.Printer.Library.Models;

namespace Navyzbra.Printer.Library
{
    public class NZPrinter
    {
        private readonly ILogger logger;
        private readonly IPrintRepository printRepository;
        private readonly IPrintTemplate pageTemplate;
        public enum PrintState { Running = 200, ParseError = 400, PrintFail = 404, ConnectionFail = 500 };

        public NZPrinter(IPrintRepository repo, IPrintTemplate template, ILogger log)
        {
            this.printRepository = repo;
            this.pageTemplate = template;
            this.logger = log;
        }

        public void Print(Enum printType){
            PrintDialog pdi = new PrintDialog();
            PrintDocument printDoc = new PrintDocument();
            PrinterSettings printSetting = new PrinterSettings();
            this.printRepository.PrintType = printType;
            PrintPreference preference = printRepository.GetPreference();

            this.pageTemplate.Preference = preference;
            printSetting.DefaultPageSettings.PaperSize = preference.PaperSize;
            printSetting.PrinterName = preference.PrinterName;
            printDoc.PrinterSettings = printSetting;
            printDoc.DefaultPageSettings.PaperSize = preference.PaperSize;
            pdi.Document = printDoc;
            try
            {
                foreach (int id in printRepository.GetAll())
                {
                    this.pageTemplate.Contents = printRepository.GetPrintData(id);
                    printDoc.PrintPage += new PrintPageEventHandler(this.pageTemplate.PrintPage);
                    //if (pdi.ShowDialog() == DialogResult.OK)
                    //{
                        printDoc.Print();
                    //}
                    this.printRepository.SetPrintData(id,PrintState.Running);
                    logger.LogMessage(string.Format("Get Print Number : {0}", id));
                }
            }
            catch (Exception e)
            {
                logger.LogMessage(string.Format("Error : {0}", e.Message));
            }
        }
    }
}
