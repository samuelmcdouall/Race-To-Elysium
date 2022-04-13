using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CGDArachneWeb : MonoBehaviour
{
    [SerializeField]
    float _slowPerMod;
    [SerializeField]
    float _lifetime;
    [System.NonSerialized]
    public GameObject OwnPlayer;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject != OwnPlayer && OwnPlayer != null)
        {
            print("stuck in the web");
            int photonViewID = other.gameObject.GetComponent<PhotonView>().ViewID;
            other.gameObject.GetComponent<CGDPlayer>().ApplySpeedModifierForSecondsToGivenPlayer(_slowPerMod, 1.0f, photonViewID, true);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject != OwnPlayer)
        {
            print("player entered the web");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject != OwnPlayer)
        {
            print("player exited the web");
        }
    }
}
