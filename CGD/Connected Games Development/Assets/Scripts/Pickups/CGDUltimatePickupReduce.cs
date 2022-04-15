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
            Destroy(gameObject);
        }
    }
}
