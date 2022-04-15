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
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateChargeUltPickup(_increasePercentage);
            Destroy(transform.parent.gameObject);
        }
    }
}
