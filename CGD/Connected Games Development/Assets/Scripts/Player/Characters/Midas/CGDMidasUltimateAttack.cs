using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDMidasUltimateAttack : MonoBehaviour
{
    public GameObject OwnPlayer;
    Collider _slowCollider;
    [SerializeField]
    float _slowColliderDuration;
    [SerializeField]
    float _slowDuration;
    [SerializeField]
    float _slowPercentageModifier;


    void Start()
    {
        _slowCollider = GetComponent<Collider>();
        _slowCollider.enabled = false;
    }

    public void ActivateUltimateCollider()
    {
        _slowCollider.enabled = true;
        Invoke("DeactivateUltimateCollider", _slowColliderDuration);
    }
    void DeactivateUltimateCollider()
    {
        _slowCollider.enabled = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && collider.gameObject != OwnPlayer)
        {
            print("bubble slow affected other player");
            int photonViewID = collider.gameObject.GetComponent<PhotonView>().ViewID;
            collider.gameObject.GetComponent<CGDPlayer>().ApplySpeedModifierForSecondsToGivenPlayer(_slowPercentageModifier,_slowDuration, photonViewID, true);
        }
    }
    // maybe move actual physics calculation into fixedupdate
}
