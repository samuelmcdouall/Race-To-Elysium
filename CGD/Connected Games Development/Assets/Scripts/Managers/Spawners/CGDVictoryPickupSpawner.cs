using Photon.Pun;
using UnityEngine;

public class CGDVictoryPickupSpawner : MonoBehaviour
{
    public GameObject Pickup;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Pickup.name, transform.position, Quaternion.identity);
        }
    }
}
