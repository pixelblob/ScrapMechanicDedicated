using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScrapMechanicDedicated.GameStateController;
using static ScrapMechanicDedicated.Program;
using static ScrapMechanicDedicated.GameStateManager;

namespace ScrapMechanicDedicated
{
    static class GameInactivityManager
    {

        public static event GameInactivityStateHandler ServerInactvityStarted;
        public static event GameInactivityStateHandler ServerInactvityStopped;
        public delegate void GameInactivityStateHandler();

        public static int inactivityTimeoutMs = 10 * 60 * 1000;
        private static readonly System.Timers.Timer InactivityTimer = new(interval: inactivityTimeoutMs);

        public static DateTime? lastInactiveTimeoutDate;

        static void OnServerInactvityStarted()
        {
            logLine("Inactivity Timer Started!");
            lastInactiveTimeoutDate = DateTime.Now;
            InactivityTimer.Start();
            ServerInactvityStarted?.Invoke();
        }

        static void OnServerInactvityStopped()
        {
            logLine("Inactivity Timer Stopped!");
            lastInactiveTimeoutDate = null;
            InactivityTimer.Stop();
            ServerInactvityStopped?.Invoke();
        }

        public static void initGameInactivityManager()
        {
            logLine("Inactivity Manager Started!");
            InactivityTimer.Elapsed += InactivityTimer_Elapsed;
            ServerStarted += GameInactivityManager_ServerStarted;
            ServerStopped += GameInactivityManager_ServerStopped;
            ServerResumed += GameInactivityManager_ServerResumed;
            ServerSuspended += GameInactivityManager_ServerSuspended;
            ServerPlayerJoined += GameInactivityManager_ServerPlayerJoined;
            ServerPlayerLeft += GameInactivityManager_ServerPlayerLeft;
        }

        private static void GameInactivityManager_ServerResumed()
        {
            if (playerCount > 0) return;
            OnServerInactvityStarted();
        }

        private static void GameInactivityManager_ServerPlayerLeft(string name)
        {
            if (playerCount > 0) return;
            logLine("All players have left server start inactivity timer!");
            OnServerInactvityStarted();

        }

        private static void GameInactivityManager_ServerPlayerJoined(string name)
        {
            if (InactivityTimer.Enabled) return;

            logLine("A player has joined the server stop inactivity timer!");
            OnServerInactvityStopped();
        }

        private static void GameInactivityManager_ServerSuspended()
        {
            if (!InactivityTimer.Enabled) return;
            logLine("The server has suspended stop inactivity timer!");
            OnServerInactvityStopped();
        }

        private static void GameInactivityManager_ServerStopped(bool intentional = true)
        {
            logLine("The server has stopped stop inactivity timer!");
            OnServerInactvityStopped();
        }

        private static void GameInactivityManager_ServerStarted()
        {
            logLine("The server has started start inactivity timer!");
            OnServerInactvityStarted();
        }

        private static void InactivityTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            OnServerInactvityStopped();

            logLine("Server is idle, Suspending!");
            suspendServer();
        }
    }
}


/*  ----------------------------------   OLD CODE FOR REFERENCE WHEN REIMPLEMENTING   ----------------------------------

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
 
 
 */