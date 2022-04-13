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
    public HazardType Hazard;

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
            if (!other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.activeSelf && Hazard == HazardType.LavaPool)
            {
                other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.SetActive(true);
            }
            else if (!other.gameObject.GetComponent<CGDPlayer>().PoisonBurnFX.activeSelf && Hazard == HazardType.PoisonCloud)
            {
                other.gameObject.GetComponent<CGDPlayer>().PoisonBurnFX.SetActive(true);
            }
            else
            {
                print("no effects");
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && (other.gameObject != OwnPlayer || _selfImmuneDelay <= 0.0f))
        {
            //if (Hazard == HazardType.LavaPool)
            //{
            //    other.gameObject.GetComponent<CGDPlayer>().LavaBurnFX.SetActive(true);
            //}
            //else if (Hazard == HazardType.PoisonCloud)
            //{
            //    other.gameObject.GetComponent<CGDPlayer>().PoisonBurnFX.SetActive(true);
            //}
            //else
            //{
            //    print("no effects");
            //}
            print("player entered the area denial hazard");
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
            else
            {
                print("no effects");
            }
            print("player exited the area denial hazard");
        }
    }

    public enum HazardType
    {
        LavaPool,
        PoisonCloud
    }
}
