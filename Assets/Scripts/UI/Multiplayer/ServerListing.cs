using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServerListing : MonoBehaviourPunCallbacks
{

    [SerializeField] private Transform _content;
    [SerializeField] private ServerListContent _serverRoom;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomInfo info in roomList)
        {
            if (!info.IsVisible) return;

            ServerListContent serverBar = Instantiate(_serverRoom, _content);
            
            if(serverBar != null)
            {
                serverBar.fillServerValue(info);
            }
        }
    }

}
