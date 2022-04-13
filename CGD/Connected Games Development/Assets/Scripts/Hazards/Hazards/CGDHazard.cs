using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDHazard : MonoBehaviour
{
    [SerializeField]
    float _ultPerDecrPerSecond;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //print("player in the lava");
            //other.gameObject.GetComponent<CGDPlayer>().UltimateCharge -= _ultPerDecrPerSecond * Time.fixedDeltaTime; todo shouldn't need this but double check with a test
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(-_ultPerDecrPerSecond * Time.fixedDeltaTime);
            if (!other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.activeSelf)
            {
                other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.SetActive(true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("player entered the lava");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.SetActive(false);
            print("player exited the lava");
        }
    }
}
