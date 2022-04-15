using UnityEngine;
using Photon.Pun;

public class CGDNarcissusUltimateAttack : MonoBehaviour
{
    public GameObject OwnPlayer;
    [SerializeField]
    float _blindColliderDuration;
    [SerializeField]
    float _fullBlindDuration;
    [SerializeField]
    float _fadeOutDuration;
    Collider _blindCollider;

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
            print("Blind affected other player");
            int photonViewID = collider.gameObject.GetComponent<PhotonView>().ViewID;
            collider.gameObject.GetComponent<CGDPlayer>().DisplayBlindScreenForSecondsToGivenPlayer(_fullBlindDuration, _fadeOutDuration, photonViewID, true);
        }
    }
}
