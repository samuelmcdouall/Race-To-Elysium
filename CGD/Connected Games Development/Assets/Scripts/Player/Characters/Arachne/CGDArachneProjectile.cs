using UnityEngine;
using Photon.Pun;

public class CGDArachneProjectile : MonoBehaviour
{
    [SerializeField]
    float _lifetime;
    public GameObject WebObject;
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
            print("Web exploded");
            GameObject webObject = PhotonNetwork.Instantiate(WebObject.name, gameObject.transform.position, Quaternion.identity);
            webObject.GetComponent<CGDArachneWeb>().OwnPlayer = OwnPlayer;
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
