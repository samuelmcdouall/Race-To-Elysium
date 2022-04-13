using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDGate : MonoBehaviour
{
    PhotonView _view;
    [SerializeField]
    float _maxHitPoints;
    float _currHitPoints;
    public GameObject HealthBar;
    public Transform Checkpoint;
    public GameObject Hazard;
    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponent<PhotonView>();
        _currHitPoints = _maxHitPoints;
        HealthBar.GetComponent<CGDUltimateBar>().SliderBar.maxValue = _maxHitPoints;
        HealthBar.GetComponent<CGDUltimateBar>().SetBar(_maxHitPoints);
    }

    public void ReduceHealthOfGateForAllPlayers()
    {
        _view.RPC("ReduceHealthOfGate", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void ReduceHealthOfGate()
    {
        _currHitPoints--;
        if (_currHitPoints == 0.0f)
        {
            // update checkpoint
            HealthBar.SetActive(false);
            //HealthBar.GetComponent<CGDUltimateBar>().SetBar(_maxHitPoints);
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject p in players)
            {
                p.GetComponent<CGDPlayer>().CheckpointPosition = Checkpoint.position;
            }
            Hazard.GetComponent<CGDGateHazardSweeping>().Completed = true;
            Destroy(gameObject);
        }
        else
        {
            HealthBar.GetComponent<CGDUltimateBar>().SetBar(_currHitPoints);
        }
    }
}
