using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Photon Libs
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;
//Game library
using com.Core.Configuration;
using com.Core.UI;
using com.Core.Cache.Player;

namespace com.Core.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        // Start is called before the first frame update
        void Start()
        {
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
            //Need to have a character before connecting to the main server    
            PhotonNetwork.ConnectUsingSettings();
        }

        public static void ConnectToLobby()
        {
            //Joins Lobby
            PhotonNetwork.JoinLobby();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
