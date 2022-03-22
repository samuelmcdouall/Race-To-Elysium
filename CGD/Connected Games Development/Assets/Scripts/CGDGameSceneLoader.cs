using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGDGameSceneLoader : MonoBehaviour
{
    [SerializeField]
    float _countDownTime;
    float _countDownTimer;
    public Text _countDownText;
    bool _beginCountDown;
    PhotonView _view;
    bool _begunLoadingLevel;
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _beginCountDown = false;
        _begunLoadingLevel = false;
        _view = GetComponent<PhotonView>();
        _countDownText.text = "";
        _countDownTimer = _countDownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && PhotonNetwork.IsMasterClient) // this is more of a debugging
        {
            BeginCountDownForAllPlayers();
        }
        if (_beginCountDown)
        {
            _countDownTimer -= Time.deltaTime;
            float roundedCountDownTimer = Mathf.Ceil(_countDownTimer);
            if (_begunLoadingLevel)
            {
                _countDownText.text = "";
            }
            else if (_countDownTimer <= 0.0f)
            {
                _begunLoadingLevel = true;
                _countDownText.text = "";
                if (PhotonNetwork.IsMasterClient)
                {
                    _countDownTimer = _countDownTime;
                    // here randomly determine the game preset + apply to the game settings + send to everyone, then in the game scene use that to enable the groups
                    PhotonNetwork.LoadLevel("GameScene");
                }
            }
            else
            {
                _countDownText.text = ((int)roundedCountDownTimer).ToString();
            }
        }
    }
    public void BeginCountDownForAllPlayers()
    {
        print("I'm the last player/the master host has told us to join, telling everyone to start counting down");
        _view.RPC("BeginCountDown", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void BeginCountDown()
    {
        _beginCountDown = true;
    }
}
