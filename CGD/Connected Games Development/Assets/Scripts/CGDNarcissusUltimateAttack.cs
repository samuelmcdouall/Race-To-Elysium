using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CGDNarcissusUltimateAttack : MonoBehaviour
{
    public GameObject OwnPlayer;
    Collider _blindCollider;
    [SerializeField]
    float _blindColliderDuration;
    [SerializeField]
    float _fullBlindDuration;
    [SerializeField]
    float _fadeOutDuration;

    void Start()
    {
        _blindCollider = GetComponent<Collider>();
        _blindCollider.enabled = false;
    }

    public void ActivateUltimateCollider()
    {
        _blindCollider.enabled = true;
        Invoke("DeactivateUltimateCollider", _blindColliderDuration);
    }
    void DeactivateUltimateCollider()
    {
        _blindCollider.enabled = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && collider.gameObject != OwnPlayer)
        {
            print("bubble slow affected other player");
            int photonViewID = collider.gameObject.GetComponent<PhotonView>().ViewID;
            collider.gameObject.GetComponent<CGDPlayer>().DisplayBlindScreenForSecondsToGivenPlayer(_fullBlindDuration, _fadeOutDuration, photonViewID, true);
            //OwnPlayer.GetComponent<CGDPlayer>().DisplayBlindScreenForSecondsToGivenPlayer(_fullBlindDuration, _fadeOutDuration, photonViewID, true);
        }
    }
    // maybe move actual physics calculation into fixedupdate
}
