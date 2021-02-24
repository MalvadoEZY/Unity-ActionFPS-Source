using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
public class playerBar : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text status;
    [SerializeField] private Text playerNameObj;
    [SerializeField] private Text playerPingObj;

    public Player Player { get; private set; }

    public void setPlayerData(Player playerInfo)
    {
        Player = playerInfo;
        playerNameObj.text = playerInfo.NickName;
        InvokeRepeating("updatePing", 0f, 2f);
        status.text = playerInfo.CustomProperties["playerLobby_status"].ToString();
        status.color = checkStatusColor(status.text);


    }

    //Get status color
    private protected Color checkStatusColor(string value)
    {
        if(value == "In Room")
        {
            return Color.gray;
        } else if (value == "Lobby")
        {
            return Color.gray;
        } else if (value == "Playing")
        {
            return Color.green;
        }

        return Color.white;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer != null && targetPlayer == Player)
        {
            if(changedProps.ContainsKey("playerLobby_status"))
            {
                object customProperty = targetPlayer.CustomProperties["playerLobby_status"];
                if ((string)customProperty == "In Room")
                {
                    status.text = (string)customProperty;
                    status.color = Color.gray;
                } else if ((string)customProperty == "Playing")
                {
                    status.text = (string)customProperty;
                    status.color = Color.green;
                }
            }
        }

    }

    protected void updatePing()
    {
        playerPingObj.text = PhotonNetwork.GetPing().ToString();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(Player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
