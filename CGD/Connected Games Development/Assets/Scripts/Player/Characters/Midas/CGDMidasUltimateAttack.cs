using UnityEngine;
using Photon.Pun;

public class CGDMidasUltimateAttack : MonoBehaviour
{
    public GameObject OwnPlayer;
    [SerializeField]
    float _slowColliderDuration;
    [SerializeField]
    float _slowDuration;
    [SerializeField]
    float _slowPercentageModifier;
    Collider _slowCollider;

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
            int photonViewID = collider.gameObject.GetComponent<PhotonView>().ViewID;
            collider.gameObject.GetComponent<CGDPlayer>().ApplySpeedModifierForSecondsToGivenPlayer(_slowPercentageModifier, _slowDuration, photonViewID, true);
        }
    }
}
