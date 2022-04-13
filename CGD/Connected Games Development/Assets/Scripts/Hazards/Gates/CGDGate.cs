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
            HealthBar.SetActive(false);
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject player in players)
            {
                player.GetComponent<CGDPlayer>().CheckpointPosition = Checkpoint.position;
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
