using Aprovi.Business.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Application.Helpers
{
    public static class CashDrawerHelper
    {
        static byte[] _codeOpenCashDrawer;

        static CashDrawerHelper()
        {
            _codeOpenCashDrawer = new byte[] { 7, 80 };
        }

        public static bool OpenDrawer()
        {
            bool success;
            IntPtr pUnmanagedBytes = new IntPtr(0);
            pUnmanagedBytes = Marshal.AllocCoTaskMem(2);
            Marshal.Copy(_codeOpenCashDrawer, 0, pUnmanagedBytes, 2);
            success = RawPrinterHelper.SendBytesToPrinter(Reports.TicketsPrinter, pUnmanagedBytes, 5);
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return success;
        }

        public static bool PrintFile(string file)
        {
            return RawPrinterHelper.SendFileToPrinter(Reports.TicketsPrinter, file);
        }

        public static bool PrintText(string texto)
        {
            return RawPrinterHelper.SendStringToPrinter(Reports.TicketsPrinter, texto);
        }
    }
}
