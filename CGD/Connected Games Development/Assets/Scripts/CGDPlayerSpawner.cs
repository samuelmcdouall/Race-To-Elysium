using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        Vector2 randomPosition = new Vector3(Random.Range(_minSpawnX, _maxSpawnX), PlayerPrefab.transform.position.y, Random.Range(_minSpawnZ, _maxSpawnZ));
        PhotonNetwork.Instantiate(PlayerPrefab.name, randomPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
