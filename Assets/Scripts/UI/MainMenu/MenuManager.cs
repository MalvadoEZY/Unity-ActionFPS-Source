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
        [SerializeField] private GameObject options;
        [SerializeField] private GameObject characterCreation;
        [SerializeField] private GameObject loading;
        //Game title on menus
        [SerializeField] Text[] gameTitles;
        [SerializeField] Text userName;

        //Inside of the multiplayer
        [SerializeField] private GameObject searchServers;
        [SerializeField] private GameObject creatingRoom;
        [SerializeField] private GameObject playerLobby;

        [SerializeField] private GameObject mapLoading;

        //Static of the variables
        public static GameObject s_options;
        public static GameObject s_loading;
        public static GameObject s_characterCreation;

        public static GameObject s_searchServers;
        public static GameObject s_playerLobby;
        public static GameObject s_playerCreatingRoom;

        public static GameObject s_mapLoading;

        public static Text s_userName;

        //WIP ADMIN TOOLS - [SerializeField] private GameObject administrationPrefab;

        private GameObject currentWindow;
        
        // Start is called before the first frame update
        void Start()
        {

            //Reassign variables
            s_characterCreation = characterCreation;
    
            s_options = options;
            s_loading = loading;
            s_userName = userName;
            //Active the main menu

            s_searchServers = searchServers;
            s_playerLobby = playerLobby;
            s_playerCreatingRoom = creatingRoom;

            s_mapLoading = mapLoading;

            setAllMultiplayerData();
        }

        //Set the correct game title into all gameTitle Text types
        public void setAllMultiplayerData()
        {
            userName.text = PlayerCache.playerUsername;
            if(ConfigurationFile.IgnoreMenus)
            {
                skipAllMenusToTesting();
                return;
            }
            if(userName.text.Length == 0)
            {
                ChangeMenu(characterCreation);
            } else
            {
                //Connect to the main server & Lobby
                NetworkManager.ConnectToMaster();
                //Hides all menus until the connection is stablished in NetworkCallback Script.
                DisableMenus();
            }


            //Set the all the game title on the game
            for (int i = 0; i < gameTitles.Length; i++)
            {

                gameTitles[i].text = ConfigurationFile.GameName;
            }

        }

        //Only activated if the development tools are set to true **Faster and easier development IN-GAME
        private static void skipAllMenusToTesting() 
        {
            DisableMenus();
            s_loading.SetActive(false);
            NetworkManager.ConnectToMaster();
        }

        //Disables all the menus available
        public static void DisableMenus()
        {
            if (s_options != null) s_options.SetActive(false);
            if (s_characterCreation != null) s_characterCreation.SetActive(false);

            if (s_searchServers != null) s_searchServers.SetActive(false);
            if (s_playerLobby != null) s_playerLobby.SetActive(false);
            if (s_playerCreatingRoom != null) s_playerCreatingRoom.SetActive(false);

            if (s_mapLoading != null) s_mapLoading.SetActive(false);

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
            if(PlayerCache.playerUsername == null)
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

        //Exiting current player lobby
        public void exitPlayerLobby()
        {
            NetworkManager.LeaveLobby();
        }

        //Join to the room scene

        public void startCreatingHost()
        {
            ChangeMenu(creatingRoom);
        }
        //Retrieve the player name from the cache class
        public static void updateUsername()
        {
            s_userName.text = PlayerCache.playerUsername;
        }

        #region Getters
        public static string getUserName()
        {
            return s_userName.text;
        }
        #endregion
    }
}
