using UnityEngine;

public class CGDUltimatePickupReduce : MonoBehaviour
{
    [SerializeField]
    float _decrPer;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateChargeUltFromPickup(-_decrPer);
            Destroy(gameObject);
        }
    }
}
