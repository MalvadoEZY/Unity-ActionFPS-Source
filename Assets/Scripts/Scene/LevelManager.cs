using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Photon Libraries
using Photon.Pun;
using Photon.Realtime;

//Game Libraries
using com.Core.Configuration;
using com.Core.Client;
public class LevelManager : MonoBehaviourPunCallbacks
{
    private GameObject Level;
    [SerializeField] private Text respawningText;
    [SerializeField] private GameObject killfeed;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private string playerPrefab;
    [SerializeField] private GameObject leaderBoard;
    [SerializeField] private LeaderBoardPlayer _ScoreBoard; //Stores each player on score board
    [SerializeField] private Transform scoreBoard_Parent;

    private GameObject PlayerObject;
    private PhotonView PV;
    private GameObject idleCamera;

    private bool isScoreBoardOpen = false;

    //Score board 

    private List<LeaderBoardPlayer> _playerLeaderBoard = new List<LeaderBoardPlayer>();

    //Main Menu
    private protected bool isMainMenuOpen = false;
    private protected bool isSpawningInProgress = false;
    public static bool isPlayerAlive { get; set; }


    void Start()
    {
        leaderBoard.SetActive(false);
        mainMenu.SetActive(false);
        Level = GameObject.Find("SpawnPoints");
        idleCamera = GameObject.Find("MapCamera");
        isPlayerAlive = false;
        idleCamera.SetActive(true);
        checkSpawnPoints();
    }


    private void Update()
    {
        if(PV != null)
        {
            if(PV.IsMine)
            {
                //if(!isPlayerAlive && !isSpawningInProgress) checkSpawnPoints();
                checkKeybinds();
                checkVars();

            }

        }
        
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        
    }

    private protected void checkVars()
    {
        mainMenu.SetActive(isMainMenuOpen);
    }

    private protected void checkKeybinds()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isMainMenuOpen = !isMainMenuOpen;
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            isScoreBoardOpen = true;
        } else if (Input.GetKeyUp(KeyCode.Tab))
        {
            isScoreBoardOpen = false;
        }

        leaderBoard.SetActive(isScoreBoardOpen);
    }

    public void checkSpawnPoints()
    {
        isSpawningInProgress = true;
        Debug.Log(Level.transform.childCount + " spawnpoints carregados");
        System.Random randomNumber = new System.Random();
        int index = randomNumber.Next(Level.transform.childCount);

        if(PhotonNetwork.InRoom)
        {
            GetGameData();
            StartCoroutine(SpawnPlayer(playerPrefab, Level.transform.GetChild(index).gameObject.transform, 5f));
        }
    }

    //Loads the leaderboard
    private void GetGameData()
    {
        Player[] playerList = PhotonNetwork.PlayerList;

        for (int i = 0; i < playerList.Length; i++)
        {
           //If player is not playing will not be fetched
           object playerState = PlayerProperties.GetProperty(playerList[i], "playerLobby_status");
           if (playerState.ToString() != "Playing") return;

           int index = _playerLeaderBoard.FindIndex(x => x.Player == playerList[i]);

            if (index != -1) return; //If found any player it will not create
           LeaderBoardPlayer playerScore = Instantiate(_ScoreBoard, scoreBoard_Parent);
           playerScore.setPlayerData(playerList[i]);
           _playerLeaderBoard.Add(playerScore);
        }
    }

    IEnumerator SpawnPlayer(string playerPrefab, Transform spawn, float delay)
    {
        isSpawningInProgress = true;

        float timeElapsed = 0f;

        //Ignore the spawn timer if ignore menus is enabled
        if(!ConfigurationFile.IgnoreMenus)
        {
            while(timeElapsed < delay)
            {
                respawningText.gameObject.SetActive(true);
                respawningText.text = "Spawn in " + Mathf.Floor((delay - timeElapsed));
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }

        //Spawn the player
        PlayerObject = PhotonNetwork.Instantiate(playerPrefab, spawn.position, Quaternion.identity);
        respawningText.gameObject.SetActive(false);
        PV = PlayerObject.GetComponent<PhotonView>();
        idleCamera.SetActive(false);
        yield return null;
    }

    public void LeaveGame()
    {
        Application.Quit();
        //Leave game
    }
}
