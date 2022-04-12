using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGDSpawnGateTimer : MonoBehaviour
{
    [SerializeField]
    float _countDownTime;
    [SerializeField]
    float _countDownTimeVisible;
    float _countDownTimer;
    public Text _countDownText;
    bool _beginCountDown;
    PhotonView _view;
    public static bool _gameStarted;
    public GameObject Gate1;
    public GameObject Gate2;
    public GameObject Gate3;
    public GameObject Gate4;
    void Start()
    {
        _beginCountDown = false;
        _gameStarted = false;
        //_view = GetComponent<PhotonView>();
        _countDownText.text = "";
        _countDownTimer = _countDownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameStarted)
        {
            _countDownText.text = "";
            _countDownText.gameObject.SetActive(false);
        }
        else
        {
            _countDownTimer -= Time.deltaTime;
            float roundedCountDownTimer = Mathf.Ceil(_countDownTimer);

            if (_countDownTimer <= 0.0f)
            {
                _gameStarted = true;
                _countDownText.text = "";
                Gate1.GetComponent<CGDSpawnGate>().Moving = true;
                Gate2.GetComponent<CGDSpawnGate>().Moving = true;
                Gate3.GetComponent<CGDSpawnGate>().Moving = true;
                Gate4.GetComponent<CGDSpawnGate>().Moving = true;
                // move the gates

                //if (PhotonNetwork.IsMasterClient)
                //{
                //    //_countDownTimer = _countDownTime; todo might not be needed

                //}
            }
            else if (_countDownTimer <= _countDownTimeVisible)
            {
                _countDownText.text = ((int)roundedCountDownTimer).ToString();
            }
            else
            {
                _countDownText.text = "";
            }
        }

        
    }
}
