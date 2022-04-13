using Photon.Pun;
using UnityEngine;

public class CGDVictoryPickup : MonoBehaviour
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
            int winnerPhotonViewID = collider.gameObject.GetComponent<PhotonView>().ViewID;
            collider.gameObject.GetComponent<CGDPlayer>().DisplayGameOverScreenForEveryone();
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
