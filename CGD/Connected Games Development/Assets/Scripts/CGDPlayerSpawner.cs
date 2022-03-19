using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CGDPlayerSpawner : MonoBehaviour
{

    public GameObject PlayerPrefab; // will need multiple depending on which character has been chosen , need to discuss when you actually *choose* which character to play

    [SerializeField]
    float _minSpawnX;
    [SerializeField]
    float _maxSpawnX;
    [SerializeField]
    float _minSpawnZ;
    [SerializeField]
    float _maxSpawnZ;
    int _maxPlayers = 2;
    GameObject _gameSceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        _gameSceneLoader = GameObject.FindGameObjectWithTag("GameSceneLoader");
        Vector2 randomPosition = new Vector3(Random.Range(_minSpawnX, _maxSpawnX), PlayerPrefab.transform.position.y, Random.Range(_minSpawnZ, _maxSpawnZ));
        PhotonNetwork.Instantiate(PlayerPrefab.name, randomPosition, Quaternion.identity);
        // check how many players there are in the scene here, if equal to 4 then set to tru on game scene loader
        if (SceneManager.GetActiveScene().name == "PlayerLobbyScene")
        {
            //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            print("There are now: " + playerCount + " players in the lobby");
            if (playerCount == _maxPlayers)
            {
                print("Enough players (" + _maxPlayers + ") to start the game");
                _gameSceneLoader.GetComponent<CGDGameSceneLoader>().BeginCountDownForAllPlayers();

            }
        }
    }
}
