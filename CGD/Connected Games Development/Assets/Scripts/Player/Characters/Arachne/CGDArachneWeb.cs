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

    void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject != OwnPlayer && OwnPlayer != null)
        {
            print("Stuck in the web");
            int photonViewID = other.gameObject.GetComponent<PhotonView>().ViewID;
            other.gameObject.GetComponent<CGDPlayer>().ApplySpeedModifierForSecondsToGivenPlayer(_slowPerMod, 1.0f, photonViewID, true);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject != OwnPlayer)
        {
            print("Entered the web");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject != OwnPlayer)
        {
            print("Exited the web");
        }
    }
}
