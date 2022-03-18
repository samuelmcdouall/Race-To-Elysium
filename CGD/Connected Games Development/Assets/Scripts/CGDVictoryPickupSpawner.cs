using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDVictoryPickupSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Pickup;
    void Start()
    {
        Vector3 spawn_position = new Vector3(0.0f, 1.5f, 0.0f) + gameObject.transform.position;
        PhotonNetwork.Instantiate(Pickup.name, spawn_position, Quaternion.identity);
    }
}
