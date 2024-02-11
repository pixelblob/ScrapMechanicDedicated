using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapMechanicDedicated
{
    static class GameUtil
    {
        public static List<string> gameLocations = [
            "D:\\Program Files (x86)\\Steam\\steamapps\\common\\Scrap Mechanic\\Release\\",
            ".\\Release"
        ];
        public static string? serverExecutablePath = FindGameServerExePath();
        

        public static string FindGameServerExePath()
        {
            for (int i = 0; i < gameLocations.Count; i++)
            {
                var exePath = Path.Combine(gameLocations[i], "ScrapMechanic.exe");
                if (File.Exists(exePath))
                {
                    return gameLocations[i];
                }
            }
            return "";
        }
    }
}
