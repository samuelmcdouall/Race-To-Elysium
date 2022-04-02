using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDPowerUp : MonoBehaviour
{
    // this might be combined into a script that randomises an effect, this is just for proof of concept
    [SerializeField]
    float _speedBoostPerModifier;
    [SerializeField]
    float _speedBoostDuration;
    GameObject _speedBoostUI;

    void Start()
    {
        _speedBoostUI = GameObject.FindGameObjectWithTag("SpeedBoostPowerUpCue");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("WEEEEE");
            other.gameObject.GetComponent<CGDPlayer>().ApplySpeedModifierForSeconds(-_speedBoostPerModifier, _speedBoostDuration);
            other.gameObject.GetComponent<CGDPlayer>().DisplayUI(_speedBoostDuration);
            Destroy(gameObject);
        }
    }
}
