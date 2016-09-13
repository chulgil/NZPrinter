using System.Collections.Generic;
using Navyzbra.Printer.Library.Models;
using System;
using System.Linq;

namespace Navyzbra.Printer.Library.Interface
{
    public interface IPrintRepository
    {
        Enum PrintType {get; set;}
        List<int> GetAll();
        PrintContents GetPrintData(int id);
        void SetPrintData(int id, Enum status);
        PrintPreference GetPreference();
    }
}