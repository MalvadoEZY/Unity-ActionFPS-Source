using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using com.Core.UI;
using com.Core.UI.PassiveMenu;
using com.Core.Configuration;
using com.Core.SceneHelper;

namespace com.Core.Network.Callbacks
{
    public class NetworkCallbacks : MonoBehaviourPunCallbacks
    {
        [SerializeField] private playerBarListing playerListing;
        [SerializeField] private SceneLoader sceneLoader;

        //Triggers when player connects to the main server
        public override void OnConnectedToMaster()
        {
            NetworkManager.ConnectToLobby();
        }

        //Called when the client establishes connection on lobby
        public override void OnJoinedLobby()
        {
            //Changes to multiplayer server list menu
            PhotonNetwork.NickName = PlayerPrefs.GetString("cfg.username");
            init_properties();
            MenuManager.updateUsername();

            //dont let the player load the menus if the dev mode is activated
            if (ConfigurationFile.IgnoreMenus)
            {
                Debug.Log("Will create a dev room for testing ");
                NetworkManager.CreateDevRoom();
                return;
            } else
            {
                MenuManager.ChangeMenu(MenuManager.s_searchServers);
            }

        }

        //Initializing user properties
        protected void init_properties()
        {
            PhotonNetwork.LocalPlayer.CustomProperties["playerLobby_status"] = "Lobby";
        }

        //Triggers when player connects to a Lobby
        public override void OnConnected()
        {
        
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined room");
            if (ConfigurationFile.IgnoreMenus)
            {
                sceneLoader.LoadMultiplayerLevel(1); //Loads the default level
                return;
            }
            playerBarListing.JoiningRoom();
        }

        //Called when a player created a room
        public override void OnCreatedRoom()
        {
            MenuManager.s_loading.SetActive(false);
            MenuManager.ChangeMenu(MenuManager.s_playerLobby);

            //mp_MenuManager.ShowMpMenu(mp_MenuManager.s_playerRoom);
        }

    }
}