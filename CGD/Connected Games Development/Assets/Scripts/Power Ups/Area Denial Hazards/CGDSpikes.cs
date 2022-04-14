using UnityEngine;

public class CGDSpikes : MonoBehaviour
{
    public AudioClip DestroySFX;
    public GameObject DestroyFX;
    [SerializeField]
    float _ultPerDecr;
    [SerializeField]
    float _selfImmuneDelay;
    [System.NonSerialized]
    public GameObject OwnPlayer;

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
            other.gameObject.GetComponent<CGDPlayer>().ModifyUltimateCharge(-_ultPerDecr);
            AudioSource.PlayClipAtPoint(DestroySFX, transform.position, CGDGameSettings.SoundVolume);
            Instantiate(DestroyFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
