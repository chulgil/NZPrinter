using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;

using Navyzbra.Printer.Library.Common;
using Navyzbra.Printer.Library.Models;
using Navyzbra.Printer.Library.Interface;

namespace Navyzbra.Printer.Library.Template
{
    public class DefaultPrintPage : IPrintTemplate
    {
        private string[] header;
        private string[] body;
        private string[] footer;

        public PrintContents Contents {
            get { return Contents; }
            set
            {
                 header = NZHelper.ConvertPrintFormat(value.Header.ToString());
                 body = NZHelper.ConvertPrintFormat(value.Body.ToString());
                 footer = NZHelper.ConvertPrintFormat(value.Footer.ToString());
            }
        }
        public PrintPreference Preference { get; set; }

        public void PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Font printFont = Preference.PrintFont;
                int charactersOnPage = 0;
                int linesPerPage = 0;
                float fTop = 0;
                float fontHight = e.Graphics.MeasureString("HIGHT", printFont).Height;
                int leftMargin = 0;

                Pen dotPattern = new Pen(Color.Black, 1);
                dotPattern.DashStyle = DashStyle.DashDot;

                Pen solidPattern = new Pen(Color.Black, 2);
                solidPattern.DashStyle = DashStyle.Solid;


                #region Header
                {
                    foreach (string text in this.header)
                    {
                        string strPrint = text;

                        SizeF size = e.Graphics.MeasureString(strPrint, printFont);
                        int nLeft = Convert.ToInt32((e.PageBounds.Width / 2) - (size.Width / 2));
                        fTop = fTop + fontHight;

                        Rectangle rect = new Rectangle(nLeft, Convert.ToInt32(fTop), e.PageBounds.Width, e.PageBounds.Height);
                        StringFormat sf = new StringFormat();
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Alignment = StringAlignment.Center;

                        e.Graphics.MeasureString(strPrint, printFont, e.MarginBounds.Size, StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);

                        // Draws the string within the bounds of the page
                        e.Graphics.DrawString(strPrint, printFont, Brushes.Black, rect, StringFormat.GenericTypographic);
                    }
                }
                #endregion Header

                #region Body
                {
                    fTop = fTop + 30;
                    foreach (string text in body)
                    {
                        string strPrint = text;

                        SizeF size = e.Graphics.MeasureString(strPrint, printFont);
                        Rectangle rect = new Rectangle(leftMargin, Convert.ToInt32(fTop), e.PageBounds.Width, e.PageBounds.Height);
                        fTop = fTop + fontHight;

                        if (strPrint == "{{line}}")
                        {
                            e.Graphics.DrawLine(dotPattern, leftMargin, rect.Top, leftMargin + rect.Right, rect.Top);
                        }
                        else if (strPrint == "{{doubleline}}")
                        {
                            e.Graphics.DrawLine(solidPattern, leftMargin, rect.Top, leftMargin + rect.Right, rect.Top);
                        }
                        else if (strPrint.Contains("{{$}}"))
                        {
                            string[] spl = new string[1];
                            spl[0] = "{{$}}";

                            string[] spt = strPrint.Split(spl, StringSplitOptions.None);
                            int cflf = Convert.ToInt16(Math.Truncate(size.Width / e.PageBounds.Width));

                            // Left
                            e.Graphics.MeasureString(spt[0], printFont, e.MarginBounds.Size, StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);
                            e.Graphics.DrawString(spt[0], printFont, Brushes.Black, rect, StringFormat.GenericTypographic);

                            // Right
                            var format = new StringFormat() { Alignment = StringAlignment.Far };
                            string dollar = string.Format("${0}", spt[1]);
                            Rectangle dollarRect = rect;
                            SizeF dollarSize = e.Graphics.MeasureString(dollar, printFont);

                            if (cflf > 0)
                            {
                                if (e.PageBounds.Width < (dollarSize.Width + size.Width))
                                {
                                    dollarRect = new Rectangle(leftMargin, Convert.ToInt32(fTop + fontHight), e.PageBounds.Width, e.PageBounds.Height);
                                    fTop = fTop + (fontHight);
                                }
                                else
                                {
                                    dollarRect = new Rectangle(leftMargin, Convert.ToInt32(fTop), e.PageBounds.Width, e.PageBounds.Height);
                                }
                            }

                            e.Graphics.MeasureString(dollar, printFont, e.MarginBounds.Size, format, out charactersOnPage, out linesPerPage);
                            e.Graphics.DrawString(dollar, printFont, Brushes.Black, dollarRect, format);

                            if (cflf > 0)
                            {
                                fTop = fTop + (fontHight * (cflf));
                                rect = new Rectangle(leftMargin, Convert.ToInt32(fTop), e.PageBounds.Width, e.PageBounds.Height);
                            }
                        }
                        else
                        {
                            e.Graphics.MeasureString(strPrint, printFont, e.MarginBounds.Size, StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);
                            e.Graphics.DrawString(strPrint, printFont, Brushes.Black, rect, StringFormat.GenericTypographic);

                            int cflf = Convert.ToInt16(Math.Truncate(size.Width / e.PageBounds.Width));
                            if (cflf > 0)
                            {
                                fTop = fTop + (fontHight * (cflf));
                                rect = new Rectangle(leftMargin, Convert.ToInt32(fTop), e.PageBounds.Width, e.PageBounds.Height);
                            }
                        }
                    }
                }
                #endregion Body
                
                #region Footer
                {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    foreach (string text in footer)
                    {
                        string strPrint = text;
                        SizeF size = e.Graphics.MeasureString(strPrint, printFont);
                        int nLeft = Convert.ToInt32((e.PageBounds.Width / 2) - (size.Width / 2));
                        fTop = fTop + fontHight;
                        Rectangle ractFont = new Rectangle(nLeft, Convert.ToInt32(fTop), e.PageBounds.Width, e.PageBounds.Height);
                        Rectangle rect = new Rectangle(leftMargin, Convert.ToInt32(fTop), e.PageBounds.Width, e.PageBounds.Height);
                        switch (text)
                        {
                            case "{{line}}":
                                e.Graphics.DrawLine(Pens.Black, leftMargin, rect.Top, rect.Right, rect.Top);
                                break;
                            case "{{signature}}":
                                Rectangle rectSig = new Rectangle(leftMargin+20, Convert.ToInt32(fTop), e.PageBounds.Width-40, e.PageBounds.Height);
                                e.Graphics.DrawString("X", printFont, Brushes.Black, rectSig, StringFormat.GenericTypographic);
                                e.Graphics.DrawLine(Pens.Black, rectSig.Left, rectSig.Top + fontHight, rectSig.Right, rectSig.Top + fontHight);
                                break;
                            case "":
                                e.Graphics.DrawString(strPrint, printFont, Brushes.Black, ractFont, StringFormat.GenericTypographic);
                                break;
                            default : 
                                e.Graphics.MeasureString(strPrint, printFont, e.MarginBounds.Size, StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);
                                e.Graphics.DrawString(strPrint, printFont, Brushes.Black, ractFont, StringFormat.GenericTypographic);
                                break;
                        }
                    }
                }
                #endregion Footer
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
                return;
            }
        }
    }
}
