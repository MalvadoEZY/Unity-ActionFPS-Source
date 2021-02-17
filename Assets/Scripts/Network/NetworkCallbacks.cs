using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using com.Core.UI;
namespace com.Core.Network
{
    public class NetworkCallbacks : MonoBehaviourPunCallbacks
    {
        //Triggers when player connects to the main server
        public override void OnConnectedToMaster()
        {
            NetworkManager.ConnectToLobby();
        }

        //Called when the client establishes connection on lobby
        public override void OnJoinedLobby()
        {
            //Changes to multiplayer server list menu
            MenuManager.ChangeMenu(MenuManager.s_multiplayer);
            MenuManager.updateUsername();
        }

        //Triggers when player connects to a Lobby
        public override void OnConnected()
        {
        
        }
    }
}