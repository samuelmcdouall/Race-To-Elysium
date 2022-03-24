using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDSpeedBoostPickup : MonoBehaviour
{
    // this might be combined into a script that randomises an effect, this is just for proof of concept
    [SerializeField]
    float _speedBoostPerModifier;
    [SerializeField]
    float _speedBoostDuration;
    GameObject _speedBoostUI;
    GameObject _speedBoostUIText;

    void Start()
    {
        _speedBoostUI = GameObject.FindGameObjectWithTag("SpeedBoostPowerUpCue");
    }
    void OnTriggerEnter(Collider other)
    {
        print("WEEEEE");
        other.gameObject.GetComponent<CGDPlayer>().ApplySpeedModifierForSeconds(-_speedBoostPerModifier, _speedBoostDuration);
        _speedBoostUI.GetComponent<CGDUIDisplay>().DisplayUI(_speedBoostDuration);
        Destroy(gameObject);
    }
}
