using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine.SceneManagement;

using com.Core.Network;
using com.Core.Client;
using com.Core.SceneHelper;
namespace com.Core.UI.PassiveMenu
{
    public class playerBarListing : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform _content;
        [SerializeField] private playerBar _playerBar;

        private static Transform s_content;
        private static playerBar s_playerBar;
        
        

        private static List<playerBar> _playerList = new List<playerBar>();

        public void Awake()
        {
            s_content = _content;
            s_playerBar = _playerBar;

            _playerList.Clear();
            _content.DestroyChildren(true);
        }

        //Player requests to join level
        public void JoinMission()
        {

            NetworkManager.JoinGame(1);
            
        }


        //Request to leave the room
        public void LeaveRoom()
        {
            NetworkManager.LeaveLobby();
        }

        #region Callback events
        //Will be called when local player joins the room   

        public static void JoiningRoom()
        {
            
            Debug.Log("OnJoinedRoom callback");
            Player[] playerList = PhotonNetwork.PlayerList;

            for (int i = 0; i < playerList.Length; i++)
            {
                int index = _playerList.FindIndex(x => x.Player.NickName == playerList[i].NickName);



                if (index != -1)
                {
                    PlayerProperties.ChangeProperty(playerList[index], "playerLobby_status", "In Room");
                    return;
                } // if the player already exists on lobby

                if(s_playerBar != null)
                {
                    playerBar instanciatePlayer = Instantiate(s_playerBar, s_content);
                    
                    instanciatePlayer.setPlayerData(playerList[i]);
                    _playerList.Add(instanciatePlayer);
                }
            }
        }

        //Called when a new player enters on the room
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {

            Debug.Log(" ON PLAYER ENTERED ROOM ");

            int index = _playerList.FindIndex(x => x.Player == newPlayer);

            if (index != -1) return;

            PhotonNetwork.LocalPlayer.CustomProperties["playerLobby_status"] = "idle"; //Set custom lobby status to the local player

            playerBar playerInstancieted = Instantiate(_playerBar, _content);
            playerInstancieted.setPlayerData(newPlayer);
            _playerList.Add(playerInstancieted);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            int index = _playerList.FindIndex(x => x.Player == otherPlayer);
            if (index == -1) return;
            Debug.Log("PLAYER DETETADO A SAIR E VAI SER REMOVIDO");
            Destroy(_playerList[index].gameObject);
            _playerList.RemoveAt(index);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("FAILED TO JOIN ROOM " + message);
            
        }

        public override void OnLeftRoom()
        {
            _playerList.Clear();
            PlayerProperties.ChangeProperty(PhotonNetwork.LocalPlayer, "playerLobby_status", "Lobby");
        }

        #endregion
    }
}
