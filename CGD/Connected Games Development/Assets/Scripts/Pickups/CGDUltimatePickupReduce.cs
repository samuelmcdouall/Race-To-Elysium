using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDUltimatePickupReduce : MonoBehaviour
{
    [SerializeField]
    float _reductionPercentage;
    public GameObject PickupPlatformSpawner;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateChargeUltPickup(-_reductionPercentage);
            if (PickupPlatformSpawner) // this may not be used, trying to figure out other solution
            {
                PickupPlatformSpawner.GetComponent<CGDPickupSpawner>().SpawnedPickup = false;
            }
            Destroy(gameObject);
        }
    }
}
