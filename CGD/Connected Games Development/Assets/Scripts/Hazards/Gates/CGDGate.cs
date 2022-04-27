using Photon.Pun;
using UnityEngine;

public class CGDGate : MonoBehaviour
{
    [SerializeField]
    float _maxHitPoints;
    float _currHitPoints;
    public GameObject HealthBar;
    public Transform Checkpoint;
    public GameObject Hazard;
    PhotonView _view;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _currHitPoints = _maxHitPoints;
        HealthBar.GetComponent<CGDUIBar>().SliderBar.maxValue = _maxHitPoints;
        HealthBar.GetComponent<CGDUIBar>().SetBar(_maxHitPoints);
    }

    public void ReduceHealthOfGateForAllPlayers()
    {
        _view.RPC("ReduceHealthOfGate", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ReduceHealthOfGate()
    {
        _currHitPoints--;
        if (_currHitPoints == 0.0f)
        {
            HealthBar.SetActive(false);
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject player in players)
            {
                player.GetComponent<CGDPlayer>().CheckpointPosition = Checkpoint.position;
            }
            if (Hazard)
            {
                Hazard.GetComponent<CGDGateHazardSweeping>().Completed = true;
            }
            Destroy(gameObject);
        }
        else
        {
            HealthBar.GetComponent<CGDUIBar>().SetBar(_currHitPoints);
        }
    }
}
