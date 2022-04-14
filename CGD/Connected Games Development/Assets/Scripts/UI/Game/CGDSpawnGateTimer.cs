using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGDSpawnGateTimer : MonoBehaviour
{
    [SerializeField]
    float _countDownTime;
    [SerializeField]
    float _helpMessageVisible;
    [SerializeField]
    float _countDownTimeVisible;
    float _countDownTimer;
    public Text _countDownText;
    public List<GameObject> Gates;
    public static bool _gameStarted;

    void Start()
    {
        _gameStarted = false;
        _countDownText.text = "";
        _countDownTimer = _countDownTime;
    }

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
                foreach (GameObject gate in Gates)
                {
                    gate.GetComponent<CGDSpawnGate>().Moving = true;
                }
            }
            else if (_countDownTimer <= _countDownTimeVisible)
            {
                _countDownText.text = ((int)roundedCountDownTimer).ToString();
            }
            else if (_countDownTimer >= _helpMessageVisible)
            {
                _countDownText.text = "Press 'E' next to a totem to receive its blessing";
            }
            else
            {
                _countDownText.text = "";
            }
        }
    }
}
