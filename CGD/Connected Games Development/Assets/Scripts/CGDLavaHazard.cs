using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDLavaHazard : MonoBehaviour
{
    [SerializeField]
    float _ultPerDecrPerSecond;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //print("player in the lava");
            other.gameObject.GetComponent<CGDPlayer>().UltimateCharge -= _ultPerDecrPerSecond * Time.fixedDeltaTime;
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(-_ultPerDecrPerSecond * Time.fixedDeltaTime);
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
            print("player exited the lava");
        }
    }
}
