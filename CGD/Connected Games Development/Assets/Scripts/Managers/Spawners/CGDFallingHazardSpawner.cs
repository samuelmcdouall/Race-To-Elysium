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
                PhotonNetwork.Instantiate(FallingHazard.name, transform.position, FallingHazard.transform.rotation);
            }
            _spawnIntervalTimer = 0.0f;
        }
        else
        {
            _spawnIntervalTimer += Time.deltaTime;
        }
    }
}
