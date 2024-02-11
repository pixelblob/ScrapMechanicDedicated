using System.Diagnostics;
using System.Runtime.InteropServices;

using static ScrapMechanicDedicated.GameUtil;
using static ScrapMechanicDedicated.GameStateManager;
using static ScrapMechanicDedicated.GameStateController;
using static ScrapMechanicDedicated.TcpServer;
using static ScrapMechanicDedicated.Program;

namespace ScrapMechanicDedicated
{

    static class Util
    {

        public static void parseCliArgs()
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (var arg in args)
            {
                Debug.WriteLine("ARG: " + arg);
                if (arg == "-autostart")
                {
                    if (Properties.Settings.Default.lastSaveGame == "")
                    {
                        Debug.WriteLine("Last Save Game Not Set, Ignoring: -autostart");
                        continue;
                    }
                    Debug.WriteLine(Properties.Settings.Default.lastSaveGame);
                    currentSaveGamePath = Properties.Settings.Default.lastSaveGame;
                    startServer();
                }
                else if (arg == "-tray")
                {
                    startHidden = true;
                }
            }
        }


        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;
        [DllImport("User32")]
        public static extern int ShowWindow(int hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static public extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;


        internal static void ScrollToBottom(RichTextBox richTextBox)
        {
            SendMessage(richTextBox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
            richTextBox.SelectionStart = richTextBox.Text.Length;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();
    }

}