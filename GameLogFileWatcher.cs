using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ScrapMechanicDedicated.GameStateManager;
using static ScrapMechanicDedicated.Program;

namespace ScrapMechanicDedicated
{
    static partial class GameLogFileWatcher
    {
        public delegate void LogEventHandler(string line, string cleanLine);
        public static event LogEventHandler ServerLogLine;

        static int logLinesToCollect = 0;
        static List<string> collectedLogLines = new();
        
        static void OnServerLogLine(string line)
        {
            var strippedLine = logLineReg().Replace(line, "");
            ServerLogLine?.Invoke(line, strippedLine);
            

            if (logLinesToCollect > 0)
            {
                logLinesToCollect--;
                collectedLogLines.Add(strippedLine);
            }

            if (collectedLogLines.Count > 0 && logLinesToCollect == 0)
            {
                logLine($"Collected {collectedLogLines.Count} Lines!");

                Match matchedconHandle = conHandleReg().Match(collectedLogLines[0]);
                Match matchedconState = conStateReg().Match(collectedLogLines[1]);

                var handle = matchedconHandle.Groups[1].Value;
                var steamId = matchedconHandle.Groups[2].Value;

                var prevState = matchedconState.Groups[1].Value;
                var currentState = matchedconState.Groups[2].Value;

                logLine($"handle: {handle}, steamId: {steamId}, prevState: {prevState}, currentState: {currentState}");

                collectedLogLines.Clear();
            }

            if (strippedLine == "Connection Status Changed") logLinesToCollect = 2;

            if (strippedLine.Contains("joined the game") || strippedLine.Contains("left the game"))
            {
                Match matchedJoin = joinReg().Match(strippedLine);

                if (matchedJoin.Success)
                {
                    var username = matchedJoin.Groups[1].Value;
                    var type = matchedJoin.Groups[2].Value;
                    int count = int.Parse(matchedJoin.Groups[3].Value);

                    Debug.WriteLine(username, type, count.ToString());
                    playerCount = count;
                    if (type == "joined")
                    {
                        if (count > 0) playersList.Add(username);

                    }
                    else
                    {
                        playersList.Remove(username);
                    }

                    updatePlayerCount();
                }
            }

            
        }

        public static void StartGameServerLogWatcher()
        {
            Debug.WriteLine("Waiting for log file!");

            var startTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

            string? logFile = null;

            while (true)
            {
                var latestFiles = new DirectoryInfo(Path.Combine(GameUtil.serverExecutablePath, "../Logs/")).GetFiles()
                    .OrderByDescending(f => f.LastWriteTime);

                FileInfo? latestFile = null;

                foreach (var file in latestFiles)
                {
                    if (file.Name.StartsWith("game"))
                    {
                        latestFile = file;
                        break;
                    }
                }

                if (latestFile == null) continue;

                var offset = new DateTimeOffset(latestFile.LastWriteTime).ToUnixTimeMilliseconds() - startTime;
                if (offset > 0)
                {
                    logFile = latestFile.ToString();
                    break;
                }
                Thread.Sleep(1000);
            }


            Debug.WriteLine("Found Log File: " + logFile);

            void logFileWatcher()
            {

                long lastFileSize = 0;

                while (true)
                {
                    var fileSize = new FileInfo(logFile).Length;
                    if (fileSize > lastFileSize)
                    {

                        //Debug.WriteLine("New Data!");
                        using (var fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            var size = fileSize - lastFileSize;
                            //Debug.WriteLine(size);
                            byte[] bytes = new byte[fs.Length - size];

                            try
                            {
                                fs.Seek(lastFileSize, SeekOrigin.Begin);
                                fs.Read(bytes, 0, (int)(fs.Length - size));
                            }
                            catch (Exception)
                            {
                                Debug.WriteLine("Failed to Get Log Line!");
                                lastFileSize = fileSize;
                                continue;
                            }

                            var text = Encoding.UTF8.GetString(bytes);

                            using (StringReader sr = new StringReader(text))
                            {
                                string line;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    OnServerLogLine(line);
                                }
                            }


                        }
                        lastFileSize = fileSize;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }

                }
            }

            new Thread(new ThreadStart(logFileWatcher)).Start();

        }

        [GeneratedRegex(@"(.*) (joined|left) the game \[(\d+)\]")]
        private static partial Regex joinReg();
        [GeneratedRegex(@"Connection handle: ([0-9]+), user: ([0-9]+)")]
        private static partial Regex conHandleReg();
        [GeneratedRegex(@"State: ([A-z ]+) -> ([A-z ]+)")]
        private static partial Regex conStateReg();
        [GeneratedRegex(@"([0-9]{2}:[0-9]{2}:[0-9]{2}) \(([0-9]+)\/([0-9]+)\) (\[[A-z]+\]) (?:	)?")]
        private static partial Regex logLineReg();
        [GeneratedRegex(@"Loading screen time: ([0-9]+.[0-9]+)s")]
        public static partial Regex loadingScreenTimeReg();
        [GeneratedRegex(@"Load finished : ([0-9]+.[0-9]+)ms")]
        public static partial Regex loadingFinishedReg();
    }
}
