using UnityEngine;

public class CGDPeel : MonoBehaviour
{
    public AudioClip DestroySFX;
    public GameObject DestroyFX;
    [SerializeField]
    float _slideDuration;
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
            Instantiate(DestroyFX, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(DestroySFX, transform.position, CGDGameSettings.SoundVolume);
            other.gameObject.GetComponent<CGDPlayer>().DisableControlsForSeconds(_slideDuration);
            other.gameObject.GetComponent<CGDPlayer>().StartSliding(_slideDuration);
            Destroy(gameObject);
        }
    }
}
