using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDAreaDenialProjectile : MonoBehaviour
{
    [SerializeField]
    float _lifetime;
    [System.NonSerialized]
    public AreaDenialProjectileType ProjectileType;
    public GameObject PeelObject;
    public GameObject SpikesObject;
    public GameObject PoisonCloudObject;
    public GameObject LavaPoolObject;
    public AudioClip CollideSFX;
    [System.NonSerialized]
    public GameObject OwnPlayer;
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
            print("area denial projectile hit surface");
            AudioSource.PlayClipAtPoint(CollideSFX, transform.position, CGDGameSettings.SoundVolume);
            switch (ProjectileType)
            {
                case AreaDenialProjectileType.Peel:
                    {
                        GameObject peel = PhotonNetwork.Instantiate(PeelObject.name, transform.position, Quaternion.identity);
                        peel.GetComponent<CGDPeel>().OwnPlayer = OwnPlayer;
                        break;
                    }

                case AreaDenialProjectileType.Spikes:
                    {
                        GameObject spikes = PhotonNetwork.Instantiate(SpikesObject.name, transform.position, Quaternion.identity);
                        spikes.GetComponent<CGDSpikes>().OwnPlayer = OwnPlayer;
                        break;
                    }

                case AreaDenialProjectileType.PoisonCloud:
                    {
                        GameObject poisonCloud = PhotonNetwork.Instantiate(PoisonCloudObject.name, transform.position, Quaternion.identity);
                        poisonCloud.GetComponent<CGDAreaDenialPowerUpHazard>().OwnPlayer = OwnPlayer;
                        break;
                    }

                case AreaDenialProjectileType.LavaPool:
                    {
                        GameObject lavaPool = PhotonNetwork.Instantiate(LavaPoolObject.name, transform.position, Quaternion.identity);
                        lavaPool.GetComponent<CGDAreaDenialPowerUpHazard>().OwnPlayer = OwnPlayer;
                        break;
                    }

                default:
                    print("ERROR, PROJECTILE UNDEFINED");
                    break;
            }

            Destroy(gameObject);
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