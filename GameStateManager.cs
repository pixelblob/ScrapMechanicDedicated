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
        public static Process? proc;
        public static int playerCount = 0;
        public static List<string> playersList = [];
        public static string currentSaveGamePath = Properties.Settings.Default.lastSaveGame;
        public static int hWnd = 0;
        public static string userDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Axolot Games\\Scrap Mechanic\\User");
        public static List<string> saveGames = getSaveGameList();

        
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
