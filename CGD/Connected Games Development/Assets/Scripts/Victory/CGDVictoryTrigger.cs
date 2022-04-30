using Photon.Pun;
using UnityEngine;

public class CGDVictoryTrigger : MonoBehaviour
{
    bool _hit = false;
    PhotonView _view;

    void Start()
    {
        _view = GetComponent<PhotonView>();
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && !_hit && _view.IsMine)
        {
            _hit = true;
            collider.gameObject.GetComponent<CGDPlayer>().DisplayGameOverScreenForEveryone();
            Destroy(gameObject);
        }
    }
}
