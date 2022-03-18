using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDVictoryPickup : MonoBehaviour
{
    bool _hit = false; // extra precaution todo maybe not needed
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && !_hit)
        {
            _hit = true;
            print("hit the goal, can now destroy object (should only happen once)");
            int winnerPhotonViewID = collider.gameObject.GetComponent<PhotonView>().ViewID;
            // this is getting called twice for each player (it won't make a difference but it shouldn't be doing so)
            collider.gameObject.GetComponent<CGDPlayer>().DisplayGameOverScreen();
            Destroy(gameObject);
        }
    }
}
