using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDVictoryPickup : MonoBehaviour
{
    bool _hit = false; // extra precaution todo maybe not needed
    PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && !_hit && _view.IsMine)
        {
            _hit = true;
            print("hit the goal, can now destroy object (should only happen once)");
            int winnerPhotonViewID = collider.gameObject.GetComponent<PhotonView>().ViewID;
            // todo I think this is sorted, it was getting done x amount of times where x is the number of players, check
            collider.gameObject.GetComponent<CGDPlayer>().DisplayGameOverScreen();
            Destroy(gameObject);
        }
    }
}
