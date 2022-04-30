using UnityEngine;

public class CGDAreaDenialPowerUpHazard : MonoBehaviour
{
    [SerializeField]
    float _ultPerDecrPerSecond;
    [SerializeField]
    float _selfImmuneDelay;
    public HazardType Hazard;
    [System.NonSerialized]
    public GameObject OwnPlayer;

    void Update()
    {
        if (_selfImmuneDelay > 0.0f)
        {
            _selfImmuneDelay -= Time.deltaTime;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && (other.gameObject != OwnPlayer || _selfImmuneDelay <= 0.0f))
        {
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(-_ultPerDecrPerSecond * Time.fixedDeltaTime);
            if (!other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.activeSelf && Hazard == HazardType.LavaPool)
            {
                other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.SetActive(true);
            }
            else if (!other.gameObject.GetComponent<CGDPlayer>().PoisonBurnFX.activeSelf && Hazard == HazardType.PoisonCloud)
            {
                other.gameObject.GetComponent<CGDPlayer>().PoisonBurnFX.SetActive(true);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && (other.gameObject != OwnPlayer || _selfImmuneDelay <= 0.0f))
        {
            Debug.Log("Player entered the area denial hazard");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && (other.gameObject != OwnPlayer || _selfImmuneDelay <= 0.0f))
        {
            if (Hazard == HazardType.LavaPool)
            {
                other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.SetActive(false);
            }
            else if (Hazard == HazardType.PoisonCloud)
            {
                other.gameObject.GetComponent<CGDPlayer>().PoisonBurnFX.SetActive(false);
            }
            Debug.Log("Player exited the area denial hazard");
        }
    }

    public enum HazardType
    {
        LavaPool,
        PoisonCloud
    }
}
