using System.Collections;
using System.Collections.Generic;
//Unity libs
using UnityEngine;

//Photon Libs
using Photon.Pun;
using Photon.Realtime;

//Game library
using com.Core.Configuration;
using com.Core.UI;
using com.Core.Client;
using com.Core.SceneHelper;
namespace com.Core.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static string currentRoom;
        private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();
        [SerializeField] private SceneLoader sceneLoader;

        private static SceneLoader s_sceneLoader;

        // Start is called before the first frame update
        void Start()
        {

            s_sceneLoader = sceneLoader;
            PhotonNetwork.GameVersion = ConfigurationFile.GameVersion;

            if (PhotonNetwork.GameVersion != ConfigurationFile.GameVersion)
            {
                Debug.Log("Incorrect version");
                return;
            }
        }

        //Connects to master server 1st step
        public static void ConnectToMaster()
        {
            Debug.Log("Connected to the master");
            //Need to have a character before connecting to the main server
            PhotonNetwork.SendRate = 20;
            PhotonNetwork.SerializationRate = 5;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = ConfigurationFile.GameVersion;
            PhotonNetwork.ConnectUsingSettings();
            PlayerProperties.SetProperty(PhotonNetwork.LocalPlayer, "playerLobby_status", "Lobby");
        }


        public static void ConnectToLobby()
        {
            //Joins Lobby
            PhotonNetwork.JoinLobby();
        }
        
        public static void LeaveLobby()
        {
            PhotonNetwork.LeaveRoom();
            MenuManager.ChangeMenu(MenuManager.s_searchServers);
            
        }

        public static void CreateServer(string nameServer, byte maxPlayers, bool isVisible, bool isPrivateServer)
        {
            RoomOptions roomOptions = new RoomOptions();
           
            roomOptions.IsVisible = isVisible;
            roomOptions.MaxPlayers = maxPlayers;
            roomOptions.IsOpen = isPrivateServer;
            PhotonNetwork.CreateRoom(nameServer, roomOptions, TypedLobby.Default);
            MenuManager.s_loading.SetActive(true);

        }

        //Request leaving the room
        public void NetworkLeaveRoom()
        {
            currentRoom = null;
            PhotonNetwork.LeaveRoom(true);
        }

        public static void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
            MenuManager.DisableMenus();
            Debug.Log("Trying to join server: " + roomName);
            MenuManager.ChangeMenu(MenuManager.s_playerLobby);
        }

        public static void CreateDevRoom()
        {

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 10;
            roomOptions.IsVisible = false;

            PhotonNetwork.JoinOrCreateRoom("DEVELOPMENT ROOM", roomOptions, TypedLobby.Default);

        }

        public static void JoinGame(int level_name)
        {
            Debug.Log("IS JOINING MISSON");
            MenuManager.ChangeMenu(MenuManager.s_mapLoading);
            PlayerProperties.ChangeProperty(PhotonNetwork.LocalPlayer, "playerLobby_status", "Playing");
            PlayerProperties.SetProperty(PhotonNetwork.LocalPlayer, "player_kill", 0);
            PlayerProperties.SetProperty(PhotonNetwork.LocalPlayer, "player_death", 0);
            //Loads the level
            s_sceneLoader.LoadMultiplayerLevel(level_name);
        }
    }
}
