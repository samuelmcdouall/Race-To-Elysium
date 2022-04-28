using UnityEngine;

public class CGDHazard : MonoBehaviour
{
    [SerializeField]
    float _ultPerDecrPerSecond;
    public Hazard HazardType;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(-_ultPerDecrPerSecond * Time.fixedDeltaTime);
            if (!other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.activeSelf && HazardType == Hazard.LavaPool)
            {
                other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.SetActive(true);
            }
            else if (!other.gameObject.GetComponent<CGDPlayer>().PoisonBurnFX.activeSelf && HazardType == Hazard.PoisonCloud)
            {
                other.gameObject.GetComponent<CGDPlayer>().PoisonBurnFX.SetActive(true);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("player entered the hazard");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.SetActive(false);
            other.gameObject.GetComponent<CGDPlayer>().PoisonBurnFX.SetActive(false);
            print("player exited the hazard");
        }
    }

    public enum Hazard
    {
        LavaPool,
        PoisonCloud
    }
}
