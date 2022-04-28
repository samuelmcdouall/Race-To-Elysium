using UnityEngine;
using Photon.Pun;

public class CGDFallingHazardSpawner : MonoBehaviour
{
    public GameObject FallingHazard;
    [SerializeField]
    float _spawnInterval;
    float _spawnIntervalTimer;

    void Start()
    {
        _spawnIntervalTimer = 0.0f;
    }

    void Update()
    {
        if (_spawnIntervalTimer > _spawnInterval)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                float randSpawnX = Random.Range(0.0f, 1.5f);
                float randSpawnZ = Random.Range(0.0f, 1.5f);
                Vector3 spawnPosition = new Vector3(randSpawnX, 0.0f, randSpawnZ) + transform.position;
                PhotonNetwork.Instantiate(FallingHazard.name, spawnPosition, FallingHazard.transform.rotation);
            }
            _spawnIntervalTimer = 0.0f;
        }
        else
        {
            _spawnIntervalTimer += Time.deltaTime;
        }
    }
}
