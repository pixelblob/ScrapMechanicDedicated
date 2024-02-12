using log4net;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using static ScrapMechanicDedicated.GameStateManager;
using static ScrapMechanicDedicated.GameStateController;
using static ScrapMechanicDedicated.Util;
using static ScrapMechanicDedicated.GameLogFileWatcher;
using static ScrapMechanicDedicated.GameInactivityManager;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace ScrapMechanicDedicated
{

    public partial class Form1 : Form
    {

        /* -------------------------    TODO LIST    -------------------------

         Implement https://stackoverflow.com/questions/3342941/kill-child-process-when-parent-process-is-killed to allow windows to kill the scrapmechanic process
         if this process dies unexpectedly.

         */

        public Form1()
        {

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            AppDomain.CurrentDomain.FirstChanceException += FirstChanceHandler;
            AppDomain.CurrentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
            System.Windows.Forms.Application.ThreadException += GlobalThreadExceptionHandler;

            InitializeComponent();

        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = (Exception)e.ExceptionObject;
            ILog log = LogManager.GetLogger(typeof(Program));
            log.Error(ex.Message + "\n" + ex.StackTrace);
        }

        private static void GlobalThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = e.Exception;
            ILog log = LogManager.GetLogger(typeof(Program)); //Log4NET
            log.Error(ex.Message + "\n" + ex.StackTrace);
        }

        public void OnProcessExit(object sender, EventArgs e)
        {
            proc?.Kill();
        }

        public void FirstChanceHandler(object source, FirstChanceExceptionEventArgs e)
        {
            proc?.Kill();
        }


        private void updateSaveGamesList()
        {
            Debug.WriteLine("LAST SLECTED GAME:" + Properties.Settings.Default.lastSelectedSaveGame);
            var i = 0;
            Debug.WriteLine($"Save Games: {saveGames.Count}");
            foreach (var saveGame in saveGames)
            {
                saveGamesListBox.Items.Add(Path.GetFileNameWithoutExtension(saveGame));
                if (Properties.Settings.Default.lastSelectedSaveGame == saveGame) saveGamesListBox.SelectedIndex = i;
                i++;
            }


        }

        private void updateGameServerVisibility()
        {
            var visible = IsWindowVisible(hWnd);
            if (visible)
            {
                showGameServer.Text = "Hide";
            }
            else
            {
                showGameServer.Text = "Show";
            }
        }

        

        public void updateGuiPlayersList(string name)
        {
            updateApplicationStatusTitle();

            listBox1.Items.Clear();

            foreach (var player in playersList)
            {
                listBox1.Items.Add(player);
            }
        }

        private void updateApplicationStatusTitle()
        {
            var title = "Scrap Mechanic Dedicated";
            if (serverRunning)
            {
                if (!serverSuspended) title += " [Running]";
            }
            else
            {
                title += " [Stopped]";
            }
            if (serverSuspended)
            {
                title += " [Suspended]";
            }
            title += $" [{playerCount}]";
            if (lastInactiveTimeoutDate != null) {
                var inactiveTimeoutCurrentSeconds = ((TimeSpan)(DateTime.Now - lastInactiveTimeoutDate)).TotalSeconds;
                title += $" [{Math.Floor(inactiveTimeoutCurrentSeconds)}/{inactivityTimeoutMs / 1000}]";
            }
            


            if (IsHandleCreated) Invoke(delegate
            {
                this.Text = title;
            });

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {


            //MessageBox.Show(e.CloseReason.ToString());   // && !System.Diagnostics.Debugger.IsAttached
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //return;
                //MessageBox.Show(e.CloseReason.ToString());
                //notifyIcon1.Visible = true;

                e.Cancel = true;

                this.Hide();

                notifyIcon1.BalloonTipTitle = "Form is minimized";
                notifyIcon1.BalloonTipText = "Left-click tray icon to open.";
                notifyIcon1.ShowBalloonTip(500);

            }
            else
            {
                proc?.Kill();
                Environment.Exit(Environment.ExitCode);
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                this.Activate();
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void show_Click(object sender, EventArgs e)
        {
            if (hWnd != 0)
            {
                //ShowWindow(hWnd, SW_SHOW);
                //hWnd = 0;
                var visible = IsWindowVisible(hWnd);
                if (visible)
                {
                    ShowWindow(hWnd, SW_HIDE);
                }
                else
                {
                    ShowWindow(hWnd, SW_SHOW);
                }
                updateGameServerVisibility();
            }
        }

        private void displaySaveGameData(string saveGameFullPath)
        {
            saveGameLabel.Text = saveGameFullPath.Replace(userDirectory + "\\", "");

            double gametick = 0;
            int? savegameversion = null;

            using (var connection = new SqliteConnection("Data Source=" + saveGameFullPath))
            {
                connection.Open();

                var gametickcommand = connection.CreateCommand();
                gametickcommand.CommandText =
                @"
                    SELECT gametick
                    FROM Game
                ";

                var savegameversioncommand = connection.CreateCommand();
                savegameversioncommand.CommandText =
                @"
                    SELECT savegameversion
                    FROM Game
                ";

                try
                {
                    using (var reader = savegameversioncommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            savegameversion = reader.GetInt32(0);
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                try
                {
                    using (var reader = gametickcommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gametick = reader.GetInt32(0);
                        }
                    }
                }
                catch (Exception ex)
                {

                }


            }

            gameTickLabel.Text = gametick.ToString();
            saveGameLabelVersion.Text = savegameversion.ToString();
            double decimaldays = Math.Floor(gametick / 57600) + 1;
            double rem = gametick % 57600;
            gameDayLabel.Text = decimaldays.ToString();

            double seconds = Math.Floor(rem / 40);

            double minutes = Math.Floor(rem / 40 / 60);

            double hours = Math.Floor(rem / 40 / 60 / 60 + 2);

            Debug.WriteLine("Seconds: " + seconds.ToString());
            Debug.WriteLine("Minutes: " + minutes.ToString());
            Debug.WriteLine("Hours: " + hours.ToString());

            gameTimeLabel.Text = $"{hours.ToString().PadLeft(2, '0')}:{minutes.ToString().PadLeft(2, '0')}";

            double totalSeconds = Math.Floor(gametick / 40) % 60;
            double totalMinutes = Math.Floor(gametick / 40 / 60) % 60;
            double totalHours = Math.Floor(gametick / 40 / 60 / 60) % 24;
            double totalDays = Math.Floor(gametick / 40 / 60 / 60 / 24);

            gamePlaytimeLabel.Text = $"{totalDays} Days, {totalHours} Hours, {totalMinutes} Minutes, {totalSeconds} Seconds";

            var lastWriteTime = File.GetLastWriteTime(saveGameFullPath);

            gameLastModifiedDate.Text = lastWriteTime.ToString("yyyy-MM-dd");
            gameLastModifiedTime.Text = lastWriteTime.ToString("HH:mm");
        }

        private void saveGamesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var saveGameFullPath = saveGames[saveGamesListBox.SelectedIndex];
            currentSaveGamePath = saveGameFullPath;
            //updateServerState();
            displaySaveGameData(saveGameFullPath);

            Properties.Settings.Default.lastSelectedSaveGame = saveGameFullPath;
            Properties.Settings.Default.Save();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            updateSaveGamesList();

            ServerStarted += Form1_ServerStarted;
            ServerStopped += Form1_ServerStopped;
            ServerSuspended += Form1_ServerSuspended;
            ServerResumed += Form1_ServerResumed;
            ServerLogLine += Form1_ServerLogLine;
            ServerPlayerJoined += updateGuiPlayersList;
            ServerPlayerLeft += updateGuiPlayersList;
            ServerInactvityStarted += Form1_ServerInactvityStarted;
            ServerInactvityStopped += Form1_ServerInactvityStopped;

            updateApplicationStatusTitle();
            updateFormState();

            //When the form is opened for the first time while the server has been running for a bit (-autostart, -tray) the form will be out of sync since it
            //was not initialized while the server events were taking place...

            if (lastInactiveTimeoutDate != null && !serverSuspended && serverRunning)
            {
                Form1_ServerInactvityStarted();
            }

        }


        private static readonly System.Timers.Timer InactivityTimerStatusUpdate = new(interval: 1000);
        private void Form1_ServerInactvityStopped()
        {
            updateApplicationStatusTitle();
            InactivityTimerStatusUpdate.Stop();
        }

        private void Form1_ServerInactvityStarted()
        {
            
            updateApplicationStatusTitle();
            InactivityTimerStatusUpdate.Elapsed -= InactivityTimerStatusUpdate_Elapsed;
            InactivityTimerStatusUpdate.Elapsed += InactivityTimerStatusUpdate_Elapsed;
            InactivityTimerStatusUpdate.Start();
        }

        private void InactivityTimerStatusUpdate_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            updateApplicationStatusTitle();
        }

        private void Form1_ServerLogLine(string line, string cleanLine)
        {
            this.Invoke(delegate
            {
                richLogBox.AppendText(line + "\r\n");
                ScrollToBottom(richLogBox);
            });
        }

        private void Form1_ServerStarted()
        {
            updateFormState();
            updateApplicationStatusTitle();
        }
        private void Form1_ServerStopped(bool intentional = true)
        {
            updateFormState();
            updateApplicationStatusTitle();
        }
        private void Form1_ServerResumed()
        {
            updateFormState();
            updateApplicationStatusTitle();
        }
        private void Form1_ServerSuspended()
        {
            updateFormState();
            updateApplicationStatusTitle();
        }

        private void updateFormState()
        {
            Action safeWrite = delegate
            {
                if (serverRunning)
                {
                    stopServerButton.Enabled = true;
                    startServerButton.Enabled = false;
                    startGameServerCtx.Enabled = false;
                    saveGamesListBox.Enabled = false;
                    richLogBox.Visible = true;

                    if (serverSuspended)
                    {
                        suspendServerButton.Enabled = false;
                        resumeServerButton.Enabled = true;
                    } else
                    {
                        suspendServerButton.Enabled = true;
                        resumeServerButton.Enabled = false;
                    }

                } else
                {
                    richLogBox.Visible = false;
                    saveGamesListBox.Enabled = true;
                    stopServerButton.Enabled = false;
                    suspendServerButton.Enabled = false;
                    resumeServerButton.Enabled = false;

                    if (saveGamesListBox.SelectedIndex >= 0)
                    {
                        startServerButton.Enabled = true;
                        startGameServerCtx.Enabled = true;
                    }
                    else
                    {
                        startGameServerCtx.Enabled = false;
                        startServerButton.Enabled = false;
                    }
                }

            };
            this.Invoke(safeWrite);
        }


        private void backupServerBtn_Click(object sender, EventArgs e)
        {
            GameBackupManager.CreateServerBackup();
        }

        private void startServerButton_Click(object sender, EventArgs e)
        {
            startServer();
        }

        private void stopServerButton_Click(object sender, EventArgs e)
        {
            stopServer();
        }

        private void suspendServerButton_Click(object sender, EventArgs e)
        {
            suspendServer();
        }

        private void resumeServerButton_Click(object sender, EventArgs e)
        {
            resumeServer();
        }
    }
}
