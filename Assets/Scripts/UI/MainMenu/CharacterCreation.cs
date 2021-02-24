using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.Core.UI;
using com.Core.Network;
using com.Core.Cache.Player;
public class CharacterCreation : MonoBehaviour
{
    [SerializeField] private Text usernameInput;

    private static string p_usernameInput;

    protected static int maxUsernameLenght = 20;
    protected static int minUsernameLenght = 3;

    private void Start()
    {
        //Reassign variables
        p_usernameInput = usernameInput.text;
    }

    public void createCharacter()
    {
        //Updates the user on cache
        p_usernameInput = usernameInput.text;

        Debug.Log(p_usernameInput);
        if (p_usernameInput.Length < minUsernameLenght || p_usernameInput.Length > maxUsernameLenght) return; //WIP: Send notification to advice the user
        //Add username in cache
        PlayerCache.playerUsername = p_usernameInput;
        PlayerPrefs.SetString("cfg.username", p_usernameInput);

        //Connect to the main server & Lobby
        NetworkManager.ConnectToMaster();
        //Hides all menus until the connection is stablished in NetworkCallback Script.
        MenuManager.DisableMenus();
    }


}
