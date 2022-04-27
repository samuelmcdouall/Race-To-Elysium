using UnityEngine;

public class CGDUltimatePickupIncrease : MonoBehaviour
{
    [SerializeField]
    float _incrPer;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateChargeFromPickup(_incrPer);
            Destroy(transform.root.gameObject);
        }
    }
}
