using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDPeel : MonoBehaviour
{
    // Start is called before the first frame update
    [System.NonSerialized]
    public GameObject OwnPlayer;
    public AudioClip DestroySFX;
    public GameObject DestroyFX;
    [SerializeField]
    float _slideDuration;
    [SerializeField]
    float _selfImmuneDelay;

    void Update()
    {
        if (_selfImmuneDelay > 0.0f)
        {
            _selfImmuneDelay -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && (other.gameObject != OwnPlayer || _selfImmuneDelay <= 0.0f))
        {
            //todo fx and sfx + don't need view, just do locally maybe, though it would be on all
            Instantiate(DestroyFX, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(DestroySFX, transform.position, CGDGameSettings.SoundVolume);
            //int photonViewID = other.gameObject.GetComponent<PhotonView>().ViewID;
            //Vector3 forceToAdd = other.gameObject.GetComponent<Rigidbody>().velocity.normalized;
            //forceToAdd = forceToAdd * _slideForce;
            //other.gameObject.GetComponent<Rigidbody>().AddForce(forceToAdd);
            //OwnPlayer.GetComponent<CGDPlayer>().KnockbackOtherPlayer(forceToAdd, photonViewID);
            other.gameObject.GetComponent<CGDPlayer>().DisableControlsForSeconds(_slideDuration);
            other.gameObject.GetComponent<CGDPlayer>().StartSliding(_slideDuration);
            Destroy(gameObject);
        }
    }
}
