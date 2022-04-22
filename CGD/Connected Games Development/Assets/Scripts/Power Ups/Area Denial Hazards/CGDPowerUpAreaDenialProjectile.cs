using Photon.Pun;
using UnityEngine;

public class CGDPowerUpAreaDenialProjectile : MonoBehaviour
{
    [SerializeField]
    float _lifetime;
    public GameObject PeelObject;
    public GameObject SpikesObject;
    public GameObject PoisonCloudObject;
    public GameObject LavaPoolObject;
    public AudioClip CollideSFX;
    [System.NonSerialized]
    public GameObject OwnPlayer;
    [System.NonSerialized]
    public AreaDenialProjectileType ProjectileType;
    int _invisibleColliderLayer;
    PhotonView _view;
    void Start()
    {
        _invisibleColliderLayer = 6;
        _view = GetComponent<PhotonView>();
        Destroy(gameObject, _lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _invisibleColliderLayer && other.gameObject != OwnPlayer && _view.IsMine)
        {
            AudioSource.PlayClipAtPoint(CollideSFX, transform.position, CGDGameSettings.SoundVolume); //todo soundmul maybe
            switch (ProjectileType)
            {
                case AreaDenialProjectileType.Peel:
                    {
                        GameObject peel = PhotonNetwork.Instantiate(PeelObject.name, PeelObject.transform.position + transform.position, PeelObject.transform.rotation);
                        peel.GetComponent<CGDPeel>().OwnPlayer = OwnPlayer;
                        break;
                    }

                case AreaDenialProjectileType.Spikes:
                    {
                        GameObject spikes = PhotonNetwork.Instantiate(SpikesObject.name, SpikesObject.transform.position + transform.position, SpikesObject.transform.rotation);
                        spikes.GetComponent<CGDSpikes>().OwnPlayer = OwnPlayer;
                        break;
                    }

                case AreaDenialProjectileType.PoisonCloud:
                    {
                        GameObject poisonCloud = PhotonNetwork.Instantiate(PoisonCloudObject.name, PoisonCloudObject.transform.position + transform.position, PoisonCloudObject.transform.rotation);
                        poisonCloud.GetComponent<CGDAreaDenialPowerUpHazard>().OwnPlayer = OwnPlayer;
                        break;
                    }

                case AreaDenialProjectileType.LavaPool:
                    {
                        GameObject lavaPool = PhotonNetwork.Instantiate(LavaPoolObject.name, LavaPoolObject.transform.position + transform.position, LavaPoolObject.transform.rotation);
                        lavaPool.GetComponent<CGDAreaDenialPowerUpHazard>().OwnPlayer = OwnPlayer;
                        break;
                    }

                default:
                    print("Error, not an area denial projectile");
                    break;
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public enum AreaDenialProjectileType
    {
        Peel,
        Spikes,
        PoisonCloud,
        LavaPool,
        Undefined
    }
}
