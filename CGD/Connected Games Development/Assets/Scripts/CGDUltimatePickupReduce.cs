using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDUltimatePickupReduce : MonoBehaviour
{
    [SerializeField]
    float _reductionPercentage;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(-_reductionPercentage);
            Destroy(gameObject);
        }
    }
}
