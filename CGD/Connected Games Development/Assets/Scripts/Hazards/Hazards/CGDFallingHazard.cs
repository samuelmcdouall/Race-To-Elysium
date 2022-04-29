using Photon.Pun;
using UnityEngine;

public class CGDFallingHazard : MonoBehaviour
{
    [SerializeField]
    float _speed;
    [SerializeField]
    float _decrPer;
    public AudioClip DestroySFX;
    public GameObject DestroyFX;
    Rigidbody _fallingHazardRB;
    int IgnoreFallingHazardLayer1 = 8;
    int IgnoreFallingHazardLayer2 = 7;

    void Start()
    {
        _fallingHazardRB = GetComponent<Rigidbody>();
        _fallingHazardRB.velocity = new Vector3(0.0f, -_speed, 0.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(-_decrPer);
        }
        if (other.gameObject.layer != IgnoreFallingHazardLayer1 && other.gameObject.layer != IgnoreFallingHazardLayer2)
        {
            DestroyHazard(true);
        }
    }

    [PunRPC]
    public void DestroyHazard(bool sendToOthers)
    {
        AudioSource.PlayClipAtPoint(DestroySFX, transform.position, CGDGameSettings.SoundVolume);
        Instantiate(DestroyFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
        if (sendToOthers)
        {
            GetComponent<PhotonView>().RPC("DestroyHazard", RpcTarget.Others, false);
        }
    }
}
