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
        AudioSource.PlayClipAtPoint(DestroySFX, transform.position, CGDGameSettings.SoundVolume);
        Instantiate(DestroyFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
