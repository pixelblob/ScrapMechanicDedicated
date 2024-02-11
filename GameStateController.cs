using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ScrapMechanicDedicated.GameStateManager;
using static ScrapMechanicDedicated.Program;
using static ScrapMechanicDedicated.WaitStateManager;
using static ScrapMechanicDedicated.Util;
using static ScrapMechanicDedicated.GameLogFileWatcher;
using System.Globalization;

namespace ScrapMechanicDedicated
{
    static class GameStateController
    {

        public static event ServerStateHandler ServerStarted;
        public static event ServerStateHandler ServerStopped;
        public static event ServerStateHandler ServerSuspended;
        public static event ServerStateHandler ServerResumed;
        public static event LoadingFinishedHandler LoadingFinished;
        public static event LoadingFinishedHandler LoadingScreenFinished;
        public delegate void LoadingFinishedHandler(float ms);
        public delegate void ServerStateHandler();

        static void onLoadingFinished(float ms)
        {
            LoadingFinished?.Invoke(ms);
            logLine($"World has finished loading in ${ms}ms");
        }

        static void onLoadingScreenFinished(float ms)
        {
            LoadingScreenFinished?.Invoke(ms);
            logLine($"Loading Screen has lifted after ${ms}ms");
        }

        static void OnServerStopped()
        {
            playerCount = 0;
            playersList.Clear();
            updatePlayerCount();

            ServerStopped?.Invoke();
        }

        static void OnServerStarted()
        {
            playerCount = 0;
            playersList.Clear();
            updatePlayerCount();
            StartGameServerLogWatcher();

            ServerLogLine -= GameStateController_ServerLogLine;
            ServerLogLine += GameStateController_ServerLogLine;

            ServerStarted?.Invoke();
        }

        static float? loadingScreenTimeMs;
        static float? loadingFinishedTimeMs;

        private static void GameStateController_ServerLogLine(string line, string cleanLine)
        {
            if (loadingFinishedTimeMs == null)
            {
                if (!cleanLine.StartsWith("Load finished :")) return;

                var match = loadingFinishedReg().Match(cleanLine);
                loadingFinishedTimeMs = float.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                onLoadingFinished((float)loadingFinishedTimeMs);
            }
            else if (loadingScreenTimeMs == null)
            {
                if (!cleanLine.StartsWith("Loading screen time:")) return;

                var match = loadingScreenTimeReg().Match(cleanLine);
                loadingScreenTimeMs = float.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture) * 1000;
                onLoadingScreenFinished((float)loadingScreenTimeMs);
            }
        }

        static void OnServerSuspended()
        {

            ServerSuspended?.Invoke();
        }

        static void OnServerResumed()
        {

            ServerResumed?.Invoke();
        }

        public static void stopServer()
        {
            if (!serverRunning) return;
            serverRunning = false;
            intentionallyStopped = true;
            OnServerStopped();

            updateServerState();
            proc.Kill();
        }

        public static void startServer()
        {
            if (serverRunning) return;
            serverRunning = true;
            intentionallyStopped = false;

            updateServerState();



            Properties.Settings.Default.lastSaveGame = currentSaveGamePath;
            Properties.Settings.Default.Save();



            proc = new Process();
            proc.StartInfo.FileName = Path.Combine(GameUtil.serverExecutablePath, "ScrapMechanic.exe");
            proc.StartInfo.CreateNoWindow = true;
            //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

            proc.StartInfo.WorkingDirectory = GameUtil.serverExecutablePath;
            proc.StartInfo.Arguments = $" -open \"{currentSaveGamePath}\"";
            proc.EnableRaisingEvents = true;
            // proc.StartInfo.RedirectStandardOutput = true;
            //proc.StartInfo.RedirectStandardError = true;
            proc.Exited += gameServerExited;
            proc.Start();

            OnServerStarted();



            //Action safeWrite = delegate
            //{

            //    richLogBox.Clear();

            //};
            //Invoke(safeWrite);



            while (proc.MainWindowHandle == IntPtr.Zero) Thread.Sleep(1);

            hWnd = proc.MainWindowHandle.ToInt32();
            ShowWindow(hWnd, SW_HIDE);
            //updateGameServerVisibility();

            //new Thread(new ThreadStart(threadStateWatcher)).Start();

            ThreadWaitStateChanged -= server_ThreadWaitStateChanged;
            ThreadWaitStateChanged += server_ThreadWaitStateChanged; // register with an event
            StartProcess();

            //inactiveTimer.Enabled = true;

            //offlineTimerCount = 0;

            //AttachConsole(proc.Id);

            //backupTimer.Enabled = true;
        }

        public static void server_ThreadWaitStateChanged()
        {
            Debug.WriteLine("Process Completed STATE CHANGE!");
            if (proc.Threads[0].ThreadState == System.Diagnostics.ThreadState.Wait)
            {
                if (proc.Threads[0].WaitReason == System.Diagnostics.ThreadWaitReason.Suspended)
                {
                    if (serverSuspended != true) ServerSuspended();
                    serverSuspended = true;
                }
            }
            else
            {
                if (serverSuspended != false) ServerResumed();
                serverSuspended = false;
            }


            updateServerState();
        }

        private static void gameServerExited(object? sender, EventArgs e)
        {
            serverRunning = false;

            logLine(
            $"Exit time    : {proc.ExitTime}\n" +
            $"Exit code    : {proc.ExitCode}\n" +
            $"Elapsed time : {Math.Round((proc.ExitTime - proc.StartTime).TotalMilliseconds)}");

            ServerStopped();

            updateServerState();

            if (!intentionallyStopped)
            {
                logLine("Server was unintentionally Stopped!");
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

        public static void createServerBackup()
        {
            string backupFileDirectory = Path.Combine(GameUtil.serverExecutablePath, "../Backups/");
            System.IO.Directory.CreateDirectory(backupFileDirectory);
            string backupFilePath = Path.Combine(backupFileDirectory, DateTime.Now.ToString("yyyy-dd-M_HH-mm-ss") + ".zip");
            string tempFilePath = Path.Combine(backupFileDirectory, "temp.db");

            File.Copy(currentSaveGamePath, tempFilePath, true);

            using (ZipArchive zip = ZipFile.Open(backupFilePath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(tempFilePath, Path.GetFileName(currentSaveGamePath));
                File.Delete(tempFilePath);
            }

        }

        public static void resetRestartAttempts()
        {
            restartAttempts = 0;
        }

        public static void suspendServer()
        {
            ApplicationSuspension.SuspendProcess(proc.Id);
            updateServerState();
        }

        public static void resumeServer()
        {
            ApplicationSuspension.ResumeProcess(proc.Id);
            updateServerState();
        }

    }
}
