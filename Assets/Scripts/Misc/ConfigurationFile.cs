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




        //getters
        public static string GameVersion { get { return p_gameVersion; } }
        public static string GameName { get { return gameName; } }
        public static int s_slots { get { return c_slots; } } //Room creation max_slots



    }
}
