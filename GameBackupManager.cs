using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScrapMechanicDedicated.GameStateManager;
using static ScrapMechanicDedicated.GameStateController;
using static ScrapMechanicDedicated.Program;

namespace ScrapMechanicDedicated
{
    static class GameBackupManager
    {
        static System.Timers.Timer GameBackupTimer = new(interval: 10 * 60 * 1000);

        static bool playerOnlineSinceLastBackup = false;

        public static void initGameBackupManager()
        {
            ServerStarted += GameBackupManager_ServerStarted;
            ServerStopped += GameBackupManager_ServerStopped;
            ServerResumed += GameBackupManager_ServerResumed;
            ServerSuspended += GameBackupManager_ServerSuspended;
            ServerPlayerJoined += GameBackupManager_ServerPlayerJoined;
            GameBackupTimer.Elapsed += GameBackupTimer_Elapsed;
        }

        private static void GameBackupManager_ServerPlayerJoined(string name)
        {
            playerOnlineSinceLastBackup = true;

            //  Re-enable game backup timer if disabled due to lack of players.
            if (!GameBackupTimer.Enabled) GameBackupTimer.Start();
        }

        private static void GameBackupTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (playerOnlineSinceLastBackup) {
                if (playerCount == 0) playerOnlineSinceLastBackup = false;
                CreateServerBackup();
            } else
            {
                logLine("No players online since last backup skipping!");
                GameBackupTimer.Stop();
            }
        }

        private static void GameBackupManager_ServerSuspended()
        {
            GameBackupTimer.Stop();
        }

        private static void GameBackupManager_ServerResumed()
        {
            GameBackupTimer.Start();
        }

        private static void GameBackupManager_ServerStopped(bool intentional = true)
        {
            GameBackupTimer.Stop();
            playerOnlineSinceLastBackup = false;
        }

        private static void GameBackupManager_ServerStarted()
        {
            GameBackupTimer.Start();
            playerOnlineSinceLastBackup = false;
        }

        public static void CreateServerBackup()
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
    }
}
