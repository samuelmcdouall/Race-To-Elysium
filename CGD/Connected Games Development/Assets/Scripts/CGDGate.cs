using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDGate : MonoBehaviour
{
    PhotonView _view;
    [SerializeField]
    int _hitPoints;
    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    public void ReduceHealthOfGateForAllPlayers()
    {
        _view.RPC("ReduceHealthOfGate", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void ReduceHealthOfGate()
    {
        _hitPoints--;
        if (_hitPoints == 0)
        {
            // update checkpoint
            Destroy(gameObject);
        }
    }
}
