using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDPeel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject OwnPlayer;
    public AudioClip DestroySFX;
    public GameObject DestroyFX;
    [SerializeField]
    float _disableControlDuration;
    [SerializeField]
    float _slideForce;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject != OwnPlayer && OwnPlayer != null)
        {
            int photonViewID = other.gameObject.GetComponent<PhotonView>().ViewID;
            Vector3 forceToAdd = other.gameObject.GetComponent<Rigidbody>().velocity.normalized;
            forceToAdd = forceToAdd * _slideForce;
            OwnPlayer.GetComponent<CGDPlayer>().KnockbackOtherPlayer(forceToAdd, photonViewID);
            other.gameObject.GetComponent<CGDPlayer>().DisableControlsForSecondsToGivenPlayer(_disableControlDuration, photonViewID, true);
            Destroy(gameObject);
        }
    }
}
