using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScrapMechanicDedicated.Program;
using static ScrapMechanicDedicated.GameStateController;
using static ScrapMechanicDedicated.GameStateManager;

namespace ScrapMechanicDedicated
{
    internal class GameCrashManager
    {
        public static bool intentionallyStopped = false;
        public static int restartAttempts = 0;
        private static readonly System.Timers.Timer ResetFailedAttemptsTimer = new(interval: 5 * 60 * 1000);

        public GameCrashManager()
        {

            //proc.Exited += Proc_Exited;
            //ServerStarted += GameCrashManager_ServerStarted;
            ServerStopped += GameCrashManager_ServerStopped;
            ServerStarted += GameCrashManager_ServerStarted;
            ResetFailedAttemptsTimer.Elapsed += ResetFailedAttemptsTimer_Elapsed;
        }

        private void GameCrashManager_ServerStarted()
        {
            ResetFailedAttemptsTimer.Start();
        }

        private void ResetFailedAttemptsTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            ResetFailedAttemptsTimer.Stop();
            resetRestartAttempts();
        }

        private void GameCrashManager_ServerStopped(bool intentional = true)
        {
            ResetFailedAttemptsTimer.Stop();

            if (intentional)
            {
                logLine("Server intentionally Stopped!");
            } else
            {
                logLine("Server UNintentionally Stopped!");

                if (restartAttempts >= 5)
                {
                    logLine("Not Restarting as restart attempts is too high!");
                    resetRestartAttempts();
                    return;
                }

                logLine("Starting Again!");
                restartAttempts++;
                startServer();
            }
        }

        public static void resetRestartAttempts()
        {
            logLine("Resetting Failed Start Attempts!");
            restartAttempts = 0;
        }
    }
}
