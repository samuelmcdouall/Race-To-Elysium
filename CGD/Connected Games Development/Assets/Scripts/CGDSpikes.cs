using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDSpikes : MonoBehaviour
{
    public AudioClip DestroySFX;
    public GameObject DestroyFX;
    public GameObject OwnPlayer;
    [SerializeField]
    float _ultPerDecr;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject != OwnPlayer && OwnPlayer != null)
        {
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(-_ultPerDecr);
            Destroy(gameObject);
        }
    }
}
