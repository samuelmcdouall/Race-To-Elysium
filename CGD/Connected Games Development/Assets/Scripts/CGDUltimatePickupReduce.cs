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
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(-_reductionPercentage);
            if (PickupPlatformSpawner)
            {
                PickupPlatformSpawner.GetComponent<CGDPickupSpawner>().SpawnedPickup = false;
            }
            Destroy(gameObject);
        }
    }
}
