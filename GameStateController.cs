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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ScrapMechanicDedicated
{
    static class GameStateController
    {

        public static event ServerStateHandler ServerStarted;
        public static event ServerStopStateHandler ServerStopped;
        public static event ServerStateHandler ServerSuspended;
        public static event ServerStateHandler ServerResumed;
        public static event PlayerJoinLeaveHandler ServerPlayerJoined;
        public static event PlayerJoinLeaveHandler ServerPlayerLeft;
        public static event LoadingFinishedHandler LoadingFinished;
        public static event LoadingFinishedHandler LoadingScreenFinished;
        public delegate void LoadingFinishedHandler(float ms);
        public delegate void PlayerJoinLeaveHandler(string name);
        public delegate void ServerStateHandler();
        public delegate void ServerStopStateHandler(bool intentional = true);

        static void OnServerPlayerJoined(string name) {

            if (playerCount > 0) playersList.Add(name);

            ServerPlayerJoined?.Invoke(name);
        }

        static void OnServerPlayerLeft(string name)
        {
            playersList.Remove(name);

            ServerPlayerLeft?.Invoke(name);
        }

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

        static void OnServerStopped(bool intentional = true)
        {
            serverRunning = false;
            playerCount = 0;
            playersList.Clear();

            form1.notifyIcon1.Icon = Properties.Resources.scrapmechdeactivated;

            form1.stopGameServerCtx.Enabled = false;

            form1.startGameServerCtx.Enabled = true;
            ApplicationWakeLockFunctions.
                        AllowSleep();
            serverSuspended = false;

            ServerStopped?.Invoke(intentional);
        }

        static void OnServerStarted()
        {
            serverRunning = true;
            playerCount = 0;
            playersList.Clear();
            StartGameServerLogWatcher();

            //These are the only exception for form functions called in server code...
            form1.stopGameServerCtx.Enabled = true;
            form1.startGameServerCtx.Enabled = false;
            ApplicationWakeLockFunctions.PreventSleep();
            form1.notifyIcon1.Icon = Properties.Resources.scrapmechanicnormal;

            ServerLogLine -= GameStateController_ServerLogLine;
            ServerLogLine += GameStateController_ServerLogLine;

            ServerStarted?.Invoke();
        }
        static void OnServerSuspended()
        {
            serverSuspended = true;
            ApplicationWakeLockFunctions.
                        AllowSleep();
            form1.notifyIcon1.Icon = Properties.Resources.scrapmechsuspended;

            ServerSuspended?.Invoke();
        }

        static void OnServerResumed()
        {
            serverSuspended = false;
            ApplicationWakeLockFunctions.
                        PreventSleep();
            form1.notifyIcon1.Icon = Properties.Resources.scrapmechanicnormal;

            ServerResumed?.Invoke();
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

            if (cleanLine.Contains("joined the game") || cleanLine.Contains("left the game"))
            {
                Match matchedJoin = joinReg().Match(cleanLine);

                if (matchedJoin.Success)
                {
                    var username = matchedJoin.Groups[1].Value;
                    var type = matchedJoin.Groups[2].Value;
                    int count = int.Parse(matchedJoin.Groups[3].Value);

                    Debug.WriteLine(username, type, count.ToString());
                    playerCount = count;
                    if (type == "joined")
                    {
                        OnServerPlayerJoined(username);
                    }
                    else
                    {
                        OnServerPlayerLeft(username);
                    }
                }
            }
        }


        public static void stopServer()
        {
            if (!serverRunning) return;
            serverRunning = false;
            OnServerStopped();

            //updateServerState();
            proc.Kill();
        }

        static GameCrashManager? crashManager;

        public static void startServer()
        {
            if (serverRunning) return;
            serverRunning = true;

            //updateServerState();



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
                    if (serverSuspended != true) OnServerSuspended();
                    serverSuspended = true;
                }
            }
            else
            {
                if (serverSuspended != false) OnServerResumed();
                serverSuspended = false;
            }


            //updateServerState();
        }

        private static void gameServerExited(object? sender, EventArgs e)
        {
            proc.Exited -= gameServerExited;
            if (serverRunning)
            {
                serverRunning = false;
                OnServerStopped(false);
            }
            
        }

        

        public static void suspendServer()
        {
            ApplicationSuspension.SuspendProcess(proc.Id);
            //updateServerState();
        }

        public static void resumeServer()
        {
            ApplicationSuspension.ResumeProcess(proc.Id);
            //updateServerState();
        }

    }
}
