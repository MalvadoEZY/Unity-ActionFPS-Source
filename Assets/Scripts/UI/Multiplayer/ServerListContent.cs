using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using com.Core.Network;
using com.Core.UI.PassiveMenu;
public class ServerListContent : MonoBehaviour
{

    [SerializeField] private Text serverName;
    [SerializeField] private Text mapName;
    [SerializeField] private Text gamemodeName;
    [SerializeField] private Text playerCounting;
    [SerializeField] private Button serverSelector;

    public RoomInfo RoomInfo { get; private set; }
    private Text currentSelected;

    public void fillServerValue(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        serverName.text = roomInfo.Name;
        playerCounting.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;

    }

    public void onServerSelected()
    {
        currentSelected = serverName;
        ServerRoomJoin.selectedServer = serverName.text;
    }
}
