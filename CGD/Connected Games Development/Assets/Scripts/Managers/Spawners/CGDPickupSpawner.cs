using Photon.Pun;
using UnityEngine;

public class CGDPickupSpawner : MonoBehaviour
{
    public GameObject Pickup;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Pickup.name, transform.position, Pickup.transform.rotation);
        }
    }
}
