using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Core.Configuration
{
    public class ConfigurationFile
    {
        //Main information
        protected static string gameName = "ActionGames FPS";
        protected static string p_gameVersion = "0.0.0";


        //Room creation
        protected static int c_slots = 10;

        //Development tools
        protected private static bool ignoreMenus = false;  //Only for development
        protected private static bool ignorePhoton = false;
        protected private static bool showLogs = false;



        //Getters
        public static string GameVersion { get { return p_gameVersion; } }
        public static string GameName { get { return gameName; } }
        
        public static int s_slots { get { return c_slots; } }
        
        public static bool IgnoreMenus { get { return ignoreMenus; } }
        public static bool ShowLogs { get { return showLogs; } }



    }
}
