using UnityEngine;

public class CGDHazard : MonoBehaviour
{
    [SerializeField]
    float _ultPerDecrPerSecond;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(-_ultPerDecrPerSecond * Time.fixedDeltaTime);
            if (!other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.activeSelf)
            {
                other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.SetActive(true);
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
            print("player exited the hazard");
        }
    }
}
