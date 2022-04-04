using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDAreaDenialPowerUpHazard : MonoBehaviour
{
    // covers the poison cloud and the lava power ups
    [System.NonSerialized]
    public GameObject OwnPlayer;
    [SerializeField]
    float _ultPerDecrPerSecond;
    [SerializeField]
    float _selfImmuneDelay;

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
            //other.gameObject.GetComponent<CGDPlayer>().UltimateCharge -= _ultPerDecrPerSecond * Time.fixedDeltaTime; todo shouldn't need this but double check with a test
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(-_ultPerDecrPerSecond * Time.fixedDeltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && (other.gameObject != OwnPlayer || _selfImmuneDelay <= 0.0f))
        {
            print("player entered the area denial hazard");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && (other.gameObject != OwnPlayer || _selfImmuneDelay <= 0.0f))
        {
            print("player exited the area denial hazard");
        }
    }
}
