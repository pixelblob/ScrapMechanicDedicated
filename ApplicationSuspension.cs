using System.Diagnostics;
using System.Runtime.InteropServices;
using static ScrapMechanicDedicated.GameStateManager;

namespace ScrapMechanicDedicated
{
    internal static class ApplicationSuspension
    {

        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }


        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        public static void ResumeProcess(int pid)
        {
            if (!serverRunning) return;
            var process = Process.GetProcessById(pid);

            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = ApplicationSuspension.OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                var suspendCount = 0;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                CloseHandle(pOpenThread);
            }
        }
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);


        public static void SuspendProcess(int pid)
        {
            if (!serverRunning) return;
            var process = Process.GetProcessById(pid); // throws exception if process does not exist

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = ApplicationSuspension.OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                SuspendThread(pOpenThread);

                CloseHandle(pOpenThread);
            }
        }
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
    }
}