using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using com.Core.Client;
public class LeaderBoardPlayer : MonoBehaviourPunCallbacks
{

    [SerializeField] private  Text playerName;
    [SerializeField] private  Text playerKills;
    [SerializeField] private  Text playerDeaths;
    [SerializeField] private  Text playerPing;
    public Player Player { get; private set; }

    public void setPlayerData(Player player)
    {
        Player = player;
        playerName.text = player.NickName;
        playerKills.text = PlayerProperties.GetProperty(player, "player_kill").ToString();
        playerDeaths.text = PlayerProperties.GetProperty(player, "player_death").ToString();
        playerPing.text = PhotonNetwork.GetPing().ToString();
    }

}
