using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ScrapMechanicDedicated.Program;
using static ScrapMechanicDedicated.TcpServer;
using static ScrapMechanicDedicated.GameStateController;

namespace ScrapMechanicDedicated
{
    static class GameStateManager
    {

        public static bool serverSuspended = false;
        public static bool serverRunning = false;
        public static bool serverClientVisible = false;
        public static bool intentionallyStopped = false;
        public static Process? proc;
        public static int playerCount = 0;
        public static List<string> playersList = [];
        public static string currentSaveGamePath = Properties.Settings.Default.lastSaveGame;
        public static int restartAttempts = 0;
        public static int hWnd = 0;
        public static string userDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Axolot Games\\Scrap Mechanic\\User");
        public static List<string> saveGames = getSaveGameList();

        public static System.Timers.Timer InactiveTimer = new System.Timers.Timer(interval: 5 * 60 * 1000);

        public static int inactiveTimeoutMS = 5 * 60 * 1000;




        public static void initInactiveTimer() {
            InactiveTimer.Interval = inactiveTimeoutMS;
            InactiveTimer.Elapsed -= newInactiveTimer_Tick;
            InactiveTimer.Elapsed += newInactiveTimer_Tick;
        }

        private static void newInactiveTimer_Tick(object sender, EventArgs e)
        {
            InactiveTimer.Stop();

            logLine("Server is idle, Suspending!");
            suspendServer();
        }

        private static void updateInactiveTimeout()
        {
            if (playerCount == 0 && !GameStateManager.serverSuspended && GameStateManager.serverRunning)
            {
                logLine("Start Inactive Timer!");

                if (InactiveTimer.Enabled)
                {
                    logLine("TIMER ALREADY ENABLED!");
                    return;
                }

                InactiveTimer.Start();
                //logLine("Start InactiveTimerStatusUpdate Timer!");
                //InactiveTimerStatus.Start();
                //lastInactiveTimeoutDate = DateTime.Now;

            }
            else
            {
                logLine("Stop Inactive Timer!");
                InactiveTimer.Stop();
                //logLine("Stop InactiveTimerStatusUpdate Timer!");
                //InactiveTimerStatus.Stop();
                //inactiveTimeoutCurrentSeconds = 0;
                //updateApplicationStatusTitle();
            }
        }


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

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

        public static void PreventSleep()
        {
            form1.Invoke(delegate
            {
                // Prevent Idle-to-Sleep (monitor not affected) (see note above)
                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);

            });
        }

        public static void AllowSleep()
        {
            form1.Invoke(delegate
            {
                // Prevent Idle-to-Sleep (monitor not affected) (see note above)
                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            });


        }

        private static List<string> getSaveGameList()
        {
            var gameList = new List<string>();

            //Debug.WriteLine(userDirectory);
            if (!Directory.Exists(userDirectory)) return gameList;
            string[] subdirectoryEntries = Directory.GetDirectories(userDirectory);
            foreach (var subdirectory in subdirectoryEntries)
            {
                var saveDirectory = Path.Combine(subdirectory, "Save");

                if (!Directory.Exists(saveDirectory)) continue;
                Debug.WriteLine(saveDirectory);

                processSaveFiles(saveDirectory);
                processSaveFiles(Path.Combine(saveDirectory, "Survival"));
                processSaveFiles(Path.Combine(saveDirectory, "Custom"));

                string[] fileEntries = Directory.GetFiles(saveDirectory);
                Debug.WriteLine(fileEntries.Length);
            }

            void processSaveFiles(string path)
            {
                string[] files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    if (!file.EndsWith(".db")) continue;
                    Debug.WriteLine(file);
                    gameList.Add(file);
                }
            }

            return gameList;
        }

    }
}
