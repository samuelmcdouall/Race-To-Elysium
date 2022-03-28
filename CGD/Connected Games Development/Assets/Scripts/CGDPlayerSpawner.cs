using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class CGDPlayerSpawner : MonoBehaviourPunCallbacks
{

    public GameObject MedusaPrefab; // will need multiple depending on which character has been chosen , need to discuss when you actually *choose* which character to play
    public GameObject MidasPrefab;
    public GameObject NarcissusPrefab;
    public GameObject ArachnePrefab;
    GameObject _chosenPrefab;

    [SerializeField]
    float _minSpawnX;
    [SerializeField]
    float _maxSpawnX;
    [SerializeField]
    float _minSpawnZ;
    [SerializeField]
    float _maxSpawnZ;
    int _maxPlayers = 3;
    GameObject _gameSceneLoader;
    [SerializeField]
    List<Transform> _spawnPositions;
    PhotonView _view;

    // Start is called before the first frame update
    void Start()
    {
        if (CGDGameSettings.CharacterNum == 1)
        {
            _chosenPrefab = MedusaPrefab;
        }
        else if (CGDGameSettings.CharacterNum == 2)
        {
            _chosenPrefab = MidasPrefab;
        }
        else if (CGDGameSettings.CharacterNum == 3)
        {
            _chosenPrefab = NarcissusPrefab;
        }
        else if (CGDGameSettings.CharacterNum == 4)
        {
            _chosenPrefab = ArachnePrefab;
        }
        _view = GetComponent<PhotonView>();
        // check how many players there are in the scene here, if equal to 4 then set to tru on game scene loader
        if (SceneManager.GetActiveScene().name == "PlayerLobbyScene")
        {
            //print("initial numBUH: " + PhotonNetwork.LocalPlayer.GetPlayerNumber());
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            print("There are now: " + playerCount + " players in the lobby");
            CGDGameSettings.PlayerNum = playerCount;
            _gameSceneLoader = GameObject.FindGameObjectWithTag("GameSceneLoader");
            
            Vector3 randomPosition = new Vector3(Random.Range(_minSpawnX, _maxSpawnX), 2.0f, Random.Range(_minSpawnZ, _maxSpawnZ));
            Vector3 constantPos = new Vector3(0.0f, 2.0f, 0.0f);
            PhotonNetwork.Instantiate(_chosenPrefab.name, randomPosition, Quaternion.identity);
            if (playerCount == _maxPlayers)
            {
                print("Enough players (" + _maxPlayers + ") to start the game");
                _gameSceneLoader.GetComponent<CGDGameSceneLoader>().BeginCountDownForAllPlayers();

            }
        }
        else
        {
            //print("numBUH: " + PhotonNetwork.LocalPlayer.GetPlayerNumber());
            Vector3 spawnPosition = new Vector3(_spawnPositions[CGDGameSettings.PlayerNum - 1].position.x, _chosenPrefab.transform.position.y, _spawnPositions[CGDGameSettings.PlayerNum - 1].position.z);
            PhotonNetwork.Instantiate(_chosenPrefab.name, spawnPosition, Quaternion.identity);
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    ReorderPlayersAndLeaveRoom();
        //}
    }
    //public void ReorderPlayersAndLeaveRoom()
    //{
    //    print("leaving this room");
    //    ModifiyPlayerNumForAllPlayers(CGDGameSettings.PlayerNum);
    //    StartCoroutine(LeaveRoom());
    //}
    

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    print("leaving this room (disconnected from server)");
    //    ModifiyPlayerNumForAllPlayers(CGDGameSettings.PlayerNum);
    //    base.OnDisconnected(cause);
    //}
    //void OnApplicationQuit()
    //{
    //    print("leaving this room (disconnected from server)");
    //    ModifiyPlayerNumForAllPlayers(CGDGameSettings.PlayerNum);
    //}

    void ModifiyPlayerNumForAllPlayers(int leftPlayerNum)
    {
        print("Leaving the room, tell the room order to adjust in my absence");
        _view.RPC("ModifiyPlayerNum", RpcTarget.All, leftPlayerNum);
    }

    [PunRPC]
    void ModifiyPlayerNum(int leftPlayerNum)
    {
        print("Another player left the room, adjusting order");
        if (CGDGameSettings.PlayerNum > leftPlayerNum)
        {
            CGDGameSettings.PlayerNum--;
            print("You are now player: " + CGDGameSettings.PlayerNum);
        }
    }

    public void OnClickMainMenuButton()
    {
        StartCoroutine(LeaveRoom());
    }
    IEnumerator LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        CGDGameOverScreenManager.GameOver = false;
        PhotonNetwork.LoadLevel("MainMenuScene");
    }
}
