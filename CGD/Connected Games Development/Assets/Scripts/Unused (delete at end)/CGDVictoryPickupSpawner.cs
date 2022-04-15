using Photon.Pun;
using UnityEngine;

public class CGDVictoryPickupSpawner : MonoBehaviour //todo this is now the exact same as the pickup spawner so can probably just delete it and use the pickupspawner script instead
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
