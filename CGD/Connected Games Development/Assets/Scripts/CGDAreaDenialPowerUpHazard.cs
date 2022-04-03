using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDAreaDenialPowerUpHazard : MonoBehaviour
{
    // covers the poison cloud and the lava power ups
    public GameObject OwnPlayer;
    [SerializeField]
    float _ultPerDecrPerSecond;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject != OwnPlayer && OwnPlayer != null)
        {
            //other.gameObject.GetComponent<CGDPlayer>().UltimateCharge -= _ultPerDecrPerSecond * Time.fixedDeltaTime; todo shouldn't need this but double check with a test
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(-_ultPerDecrPerSecond * Time.fixedDeltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject != OwnPlayer)
        {
            print("player entered the area denial hazard");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject != OwnPlayer)
        {
            print("player exited the area denial hazard");
        }
    }
}
