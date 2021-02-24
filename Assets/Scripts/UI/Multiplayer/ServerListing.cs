using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using com.Core.Client;

public class ServerListing : MonoBehaviourPunCallbacks
{

    [SerializeField] private Transform _content;
    [SerializeField] private ServerListContent _serverRoom;

    private List<ServerListContent> _serverList = new List<ServerListContent>();

    public override void OnLeftRoom()
    {
        _content.DestroyChildren(true);
        _serverList.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {PlayerProperties.ChangeProperty(PhotonNetwork.LocalPlayer, "playerLobby_status", "Lobby");
            if (!info.IsVisible || !info.IsOpen) return;

            //Removes the server when leaving
            if (info.RemovedFromList)
            {
                int index = _serverList.FindIndex(x => x.RoomInfo.Name == info.Name);
                Debug.Log(index);
                if(index != -1)
                {
                    Destroy(_serverList[index].gameObject);
                    _serverList.RemoveAt(index);
                }
            }
            else
            {
                //Doesn't add existing room
                int index = _serverList.FindIndex(x => x.RoomInfo.Name == info.Name);
                Debug.Log(index);

                // index: -1 means that doesn't exists
                if (index != -1) return;

                ServerListContent serverBar = Instantiate(_serverRoom, _content);

                if (serverBar != null)
                {
                    serverBar.fillServerValue(info);
                    _serverList.Add(serverBar);
                }
            }
        }
    }

}
