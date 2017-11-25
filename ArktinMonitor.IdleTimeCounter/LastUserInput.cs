using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ArktinMonitor.IdleTimeCounter
{
    public class LastUserInput
    {
        private struct LastInputInfo
        {
            public uint CbSize;
            public uint DwTime;
        }

        private static LastInputInfo lastInPutNfo;

        static LastUserInput()
        {
            lastInPutNfo = new LastInputInfo();
            lastInPutNfo.CbSize = (uint)Marshal.SizeOf(lastInPutNfo);
        }

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LastInputInfo plii);

        /// <summary>
        /// Idle time in ticks
        /// </summary>
        /// <returns></returns>
        public static uint GetIdleTickCount()
        {
            return ((uint)Environment.TickCount - GetLastInputTime());
        }

        /// <summary>
        /// Last input time in ticks
        /// </summary>
        /// <returns></returns>
        public static uint GetLastInputTime()
        {
            if (!GetLastInputInfo(ref lastInPutNfo))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return lastInPutNfo.DwTime;
        }
    }
}