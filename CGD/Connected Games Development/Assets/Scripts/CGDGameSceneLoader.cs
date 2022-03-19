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
    bool _beginLoadingScene;
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _beginCountDown = false;
        _beginLoadingScene = false;
        _view = GetComponent<PhotonView>();
        _countDownText.text = "";
        _countDownTimer = _countDownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && PhotonNetwork.IsMasterClient)
        {
            BeginCountDown();
        }
        if (_beginCountDown)
        {
            _countDownTimer -= Time.deltaTime;
            float roundedCountDownTimer = Mathf.Ceil(_countDownTimer);
            if (_beginLoadingScene)
            {
                _countDownText.text = "";
            }
            else
            {
                _countDownText.text = ((int)roundedCountDownTimer).ToString();
            }
            if (PhotonNetwork.IsMasterClient && _countDownTimer <= 0.0f)
            {
                _countDownTimer = _countDownTime;
                _beginLoadingScene = true;
                _countDownText.text = "";
                PhotonNetwork.LoadLevel("GameScene");
            }
        }
    }
    [PunRPC]
    void BeginCountDown()
    {
        _beginCountDown = true;
        if (_view.IsMine)
        {
            _view.RPC("BeginCountDown", RpcTarget.OthersBuffered);
        }
    }
}
