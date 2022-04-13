using Photon.Pun;
using UnityEngine;

public class CGDPickupSpawner : MonoBehaviour
{
    [SerializeField]
    float _minSpawnDelay;
    [SerializeField]
    float _maxSpawnDelay;
    [SerializeField]
    float _minXSpawnDis;
    [SerializeField]
    float _maxXSpawnDis;
    [SerializeField]
    float _minZSpawnDis;
    [SerializeField]
    float _maxZSpawnDis;
    float _spawnDelay;
    float _spawnDelayTimer;
    public bool SpawnedPickup; // for when to next spawn in
    public GameObject Pickup;
    void Start()
    {
        //if (!PhotonNetwork.IsMasterClient)
        //{
        //    Destroy(gameObject);
        //}
        //_spawnDelayTimer = 0.0f;
        //_spawnDelay = Random.Range(_minSpawnDelay, _maxSpawnDelay);
        //print("initial spawn delay for platform: " + _spawnDelay);
        //SpawnedPickup = false;

        //todo may need to figure out exactly how it works, i.e. only spawn this if its the master client if just doing on start up, 
        //or is it coming in continuously or what in which case we need to figure out more stuff
        float random_x_pos = Random.Range(_minXSpawnDis, _maxXSpawnDis);
        float random_z_pos = Random.Range(_minZSpawnDis, _maxZSpawnDis);
        Vector3 spawn_position = new Vector3(random_x_pos, 1.0f, random_z_pos) + gameObject.transform.position;
        GameObject pickup = PhotonNetwork.InstantiateRoomObject(Pickup.name, spawn_position, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        ////print(_spawnDelay);
        //if (PhotonNetwork.IsMasterClient) // still issue here with platform reference when a player leaves
        //{
        //    if (!SpawnedPickup)
        //    {
        //        if (_spawnDelayTimer > _spawnDelay)
        //        {
        //            float random_x_pos = Random.Range(_minXSpawnDis, _maxXSpawnDis);
        //            float random_z_pos = Random.Range(_minZSpawnDis, _maxZSpawnDis);
        //            Vector3 spawn_position = new Vector3(random_x_pos, 1.0f, random_z_pos) + gameObject.transform.position;
        //            GameObject pickup = PhotonNetwork.InstantiateRoomObject(Pickup.name, spawn_position, Quaternion.identity);
        //            pickup.GetComponent<CGDUltimatePickupReduce>().PickupPlatformSpawner = gameObject;
        //            pickup.transform.Find("IncreaseCollider").GetComponent<CGDUltimatePickupIncrease>().PickupPlatformSpawner = gameObject;
        //            SpawnedPickup = true;
        //            _spawnDelayTimer = 0.0f;
        //            _spawnDelay = Random.Range(_minSpawnDelay, _maxSpawnDelay);
        //            print("new spawn delay for platform: " + _spawnDelay);
        //        }
        //        else
        //        {
        //            _spawnDelayTimer += Time.deltaTime;
        //        }
        //    }
        //}
    }
}
