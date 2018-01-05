using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MGMPDV.Classes
{
    public class ImpCupom
    {
        //    Public Const eClear As String = Chr(27) + "@"
        //Public Const eCentre As String = Chr(27) + Chr(97) + "1"


        public static string vbCrLf = "0";

        public static string eNmlText = (char)27 + "!" + (char)0;
        public static string eClear = (char)27 + "@";
        public string eCentre = (char)27 + (char)97 + "1";
        public string eLeft = (char)27 + (char)97 + "0";
        public string eRight = (char)27 + (char)97 + "2";
        public string eDrawer = eClear + (char)27 + "p" + (char)0 + ".}";
        public string eCut = (char)27 + "i" + vbCrLf;
        public string eSmlText = (char)27 + "!" + (char)1;

        public string eInit = eNmlText + (char)13 + (char)27 + "c6" + (char)1 + (char)27 + "R3" + vbCrLf;
        public string eBigCharOn = (char)27 + "!" + (char)56;
        public string eBigCharOff = (char)27 + "!" + (char)0;

        RawPrinterHelper prn = new RawPrinterHelper();

        private string PrinterName = "EPSON TM-T20 Receipt";

        public void StartPrint()
        {
            string s = "";
            PrintDialog pd = new PrintDialog();
            s = s.Replace("\"", "");
            var pd1 = new PrinterSettings();
            RawPrinterHelper.SendStringToPrinter(pd1.PrinterName, s);
        }


        public void PrintHeader()
        {
            EscPos esc = new EscPos();
            string s = "";
            PrintDialog pd = new PrintDialog();
            s = s.Replace("\"", "");
            //pd.PrinterSettings = new PrinterSettings();
            //RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);

            //esc.printText(pd.PrinterSettings.PrinterName, s);

            //feedAndCutter(pd.PrinterSettings.PrinterName, 5);

        }
        //    Print(eInit + eCentre + "My Shop")
        //    Print("Tel:0123 456 7890")
        //    Print("Web: www.????.com")
        //    Print("sales@????.com")
        //    Print("VAT Reg No:123 4567 89" + eLeft)
        //    PrintDashes()
        //End Sub

        //Public Sub PrintBody()
        //    Print(eSmlText + "Tea                                          T1   1.30")

        //    PrintDashes()

        //    Print(eSmlText + "                                         Total:   1.30")

        //    Print("                                     Paid Card:   1.30")
        //End Sub

        //Public Sub PrintFooter()
        //    Print(eCentre + "Thank You For Your Support!" + eLeft)
        //    Print(vbLf + vbLf + vbLf + vbLf + vbLf + eCut + eDrawer)
        //End Sub

        //Public Sub Print(Line As String)
        //    prn.SendStringToPrinter(PrinterName, Line + vbLf)
        //End Sub

        //Public Sub PrintDashes()
        //    Print(eLeft + eNmlText + "-".PadRight(42, "-"))
        //End Sub

        //Public Sub EndPrint()
        //    prn.ClosePrint()
        //End Sub

        //Private Sub bnExit_Click(sender As System.Object, e As System.EventArgs) _
        //        Handles bnExit.Click
        //    prn.ClosePrint()

        //    Me.Close()
        //End Sub

        //Private Sub bnPrint_Click(sender As System.Object, e As System.EventArgs) _
        //        Handles bnPrint.Click
        //    StartPrint()

        //    If prn.PrinterIsOpen = True Then
        //        PrintHeader()

        //        PrintBody()

        //        PrintFooter()

        //        EndPrint()
        //    End If
        //End Sub
    }
}
