using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace ScrapMechanicDedicated
{

    public partial class Form1 : Form
    {
        

        

        class gamePlayer
        {
            public string? username { get; set; }
            public string? steamId { get; set; }

            public string? state { get; set; }
        }

        List<gamePlayer> gamePlayers = new List<gamePlayer>();

        

        


    }
}