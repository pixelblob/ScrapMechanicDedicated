﻿using System.Runtime.InteropServices;
using static ScrapMechanicDedicated.Program;

namespace ScrapMechanicDedicated
{
    internal static class ApplicationWakeLockFunctions
    {

        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }


        public static void AllowSleep()
        {
            form1.Invoke(delegate
            {
                ApplicationWakeLockFunctions.
                                // Prevent Idle-to-Sleep (monitor not affected) (see note above)
                                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            });


        }

        public static void PreventSleep()
        {
            form1.Invoke(delegate
            {
                ApplicationWakeLockFunctions.
                                // Prevent Idle-to-Sleep (monitor not affected) (see note above)
                                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);

            });
        }




        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
    }
}