using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDUltimatePickupIncrease : MonoBehaviour
{
    [SerializeField]
    float _increasePercentage;
    public GameObject PickupPlatformSpawner;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(_increasePercentage);
            if (PickupPlatformSpawner)
            {
                PickupPlatformSpawner.GetComponent<CGDPickupSpawner>().SpawnedPickup = false;
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
