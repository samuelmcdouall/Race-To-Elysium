using UnityEngine;

public class CGDUltimatePickupIncrease : MonoBehaviour
{
    [SerializeField]
    float _incrPer;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateChargeUltFromPickup(_incrPer);
            Destroy(transform.parent.gameObject);
        }
    }
}
