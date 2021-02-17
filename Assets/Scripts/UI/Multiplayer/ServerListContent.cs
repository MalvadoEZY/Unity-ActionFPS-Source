using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class ServerListContent : MonoBehaviour
{

    [SerializeField] private Text serverName;
    [SerializeField] private Text mapName;
    [SerializeField] private Text gamemodeName;
    [SerializeField] private Text playerCounting;
    [SerializeField] private Text ping;



    public void fillServerValue(RoomInfo roomInfo)
    {
        serverName.text = roomInfo.Name;
        playerCounting.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
        
    }
}
