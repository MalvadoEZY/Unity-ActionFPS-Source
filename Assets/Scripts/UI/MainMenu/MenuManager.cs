using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.Core.Network;
using com.Core.Cache.Player;
using com.Core.Configuration;

namespace com.Core.UI
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] GameObject mainMenu;
        [SerializeField] GameObject options;
        [SerializeField] GameObject multiplayer;
        [SerializeField] GameObject characterCreation;
        [SerializeField] GameObject loading;

        //Game title on menus
        [SerializeField] Text[] gameTitles;
        [SerializeField] Text userName;

        //Static of the variables
        public static GameObject s_mainMenu;
        public static GameObject s_options;
        public static GameObject s_characterCreation;
        public static GameObject s_multiplayer;
        public static GameObject s_loading;
        public static Text s_userName;

        private GameObject currentWindow;
        // Start is called before the first frame update
        void Start()
        {

            //Reassign variables
            s_characterCreation = characterCreation;
            s_multiplayer = multiplayer;
            s_mainMenu = mainMenu;
            s_options = options;
            s_loading = loading;

            s_userName = userName;
            //Active the main menu
            s_mainMenu.SetActive(true);
            setAllMultiplayerData();
        }

        //Set the correct game title into all gameTitle Text types
        public void setAllMultiplayerData()
        {
            userName.text = PlayerCache.PlayerName;
            if(userName.text.Length == 0)
            {
                ChangeMenu(mainMenu);
            }

            for (int i = 0; i < gameTitles.Length; i++)
            {
                gameTitles[i].text = ConfigurationFile.GameName;
            }

        }

        //Disables all the menus available
        public static void DisableMenus()
        {
            if (s_mainMenu != null) s_mainMenu.SetActive(false);
            if (s_options != null) s_options.SetActive(false);
            if (s_multiplayer != null) s_multiplayer.SetActive(false);
            if (s_characterCreation != null) s_characterCreation.SetActive(false);
            //Enables the loading screen
            if (s_loading != null) s_loading.SetActive(true);
        }

        //Change the menu to one of the static menus defined
        public static void ChangeMenu(GameObject newMenu)
        {
            DisableMenus();

            if (newMenu != null)
            {
                s_loading.SetActive(false);
                newMenu.SetActive(true);
            } else
            {
                Debug.Log("[MenuManager] - The menu requested doesn't exist");
            }

        }

        public void HandshakeMasterServer()
        {
            DisableMenus();

            //Before Attempting to connect to the main server will check if the user is registed
            if(PlayerCache.PlayerName == null)
            {
                ChangeMenu(s_characterCreation);
            } else
            {
                NetworkManager.ConnectToMaster();
            }

        }

        //When player clicks the button the exit the game
        public void exitGame()
        {
            Application.Quit();
        }

        //Retrieve the player name from the cache class
        public static void updateUsername()
        {
            s_userName.text = PlayerCache.PlayerName;
        }

        #region Getters
        public static string getUserName()
        {
            return s_userName.text;
        }
        #endregion
    }
}
