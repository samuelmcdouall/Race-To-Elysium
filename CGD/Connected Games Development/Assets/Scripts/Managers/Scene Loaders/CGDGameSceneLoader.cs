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
        // Force load into a game with lower than 4 players 
        // NOTE: This is used as a debug tool. In a real game this would be taken out but has been left in here to help with play testing
        if (Input.GetKeyDown(KeyCode.L) && PhotonNetwork.IsMasterClient)
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
        Debug.Log("I'm the last player/the master host has told us to start the game, telling everyone to start counting down");
        _view.RPC("BeginCountDown", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void BeginCountDown()
    {
        _beginCountDown = true;
    }
}
