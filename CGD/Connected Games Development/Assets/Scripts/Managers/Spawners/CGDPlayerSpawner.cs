using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CGDPlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject MedusaPrefab;
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
    [SerializeField]
    List<Transform> _spawnPositions;

    int _maxPlayers = 4;
    GameObject _gameSceneLoader;

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
        
        if (SceneManager.GetActiveScene().name == "PlayerLobbyScene")
        {
            _gameSceneLoader = GameObject.FindGameObjectWithTag("GameSceneLoader");

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            print("There are now: " + playerCount + " players in the lobby");
            CGDGameSettings.PlayerNum = playerCount;
            
            // Positioning slightly random so players don't spawn right on top of each other
            Vector3 randomPosition = new Vector3(Random.Range(_minSpawnX, _maxSpawnX), 2.0f, Random.Range(_minSpawnZ, _maxSpawnZ));
            GameObject player = PhotonNetwork.Instantiate(_chosenPrefab.name, randomPosition, Quaternion.identity);
            player.GetComponent<CGDPlayer>().View.Owner.NickName = CGDGameSettings.Username;

            if (playerCount == _maxPlayers)
            {
                print("Enough players present (" + _maxPlayers + ") to start the game");
                PhotonNetwork.CurrentRoom.IsOpen = false;
                _gameSceneLoader.GetComponent<CGDGameSceneLoader>().BeginCountDownForAllPlayers();

            }
        }
        else
        {
            Vector3 spawnPosition = new Vector3(_spawnPositions[CGDGameSettings.PlayerNum - 1].position.x, _spawnPositions[CGDGameSettings.PlayerNum - 1].position.y, _spawnPositions[CGDGameSettings.PlayerNum - 1].position.z);
            PhotonNetwork.Instantiate(_chosenPrefab.name, spawnPosition, Quaternion.identity);
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }
}
