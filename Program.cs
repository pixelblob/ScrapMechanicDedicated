using ScrapMechanicDedicated.Properties;
using System.Drawing;
using System.Windows.Forms;
using static ScrapMechanicDedicated.GameUtil;
using static ScrapMechanicDedicated.GameStateManager;
using static ScrapMechanicDedicated.TcpServer;
using static ScrapMechanicDedicated.Util;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ScrapMechanicDedicated
{
    internal static class Program
    {

        public static bool startHidden = false;

        public static Form1 form1 = new Form1();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());

            form1.notifyIcon1.Visible = true;

            _ = form1.Handle;

            //updateServerState();

            startTcpServer();

            parseCliArgs();

            initInactiveTimer();

            if (!startHidden) form1.Show();

            Application.Run();


        }

        public static void logLine(String line)
        {
            Console.WriteLine(line);
            Debug.WriteLine(line);
        }

    }
}