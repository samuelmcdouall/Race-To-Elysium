using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CGDGameSceneLoader : MonoBehaviour
{
    [SerializeField]
    float _countDownTime;
    float _countDownTimer;
    public Text _countDownText;
    bool _beginCountDown;
    bool _begunLoadingLevel;
    PhotonView _view;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _beginCountDown = false;
        _begunLoadingLevel = false;
        _view = GetComponent<PhotonView>();
        _countDownText.text = "";
        _countDownTimer = _countDownTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && PhotonNetwork.IsMasterClient) // todo decide whether to keep the L in to force a load / this is more of a debugging
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
                    // todo not sure of the comment below, since this is determined in the Level Generator script, can probably just delete it
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
        print("I'm the last player/the master host has told us to start the game, telling everyone to start counting down");
        _view.RPC("BeginCountDown", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void BeginCountDown()
    {
        _beginCountDown = true;
    }
}
