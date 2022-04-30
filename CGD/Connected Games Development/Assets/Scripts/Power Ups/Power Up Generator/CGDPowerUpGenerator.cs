using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Linq;

public class CGDPowerUpGenerator : MonoBehaviour
{
    [SerializeField]
    float _interval;
    float _timer;
    public AudioClip GenerateSFX;
    float _tolerance;
    PhotonView _view;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _tolerance = 0.1f;
        _timer = 0.0f;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene" && _view.IsMine && CGDSpawnGateTimer._gameStarted)
        {
            if (_timer < _interval)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                _timer = 0.0f;
                if (GetComponent<CGDPowerUpManager>()._powerUpHeld == CGDPowerUpManager.PowerUpHeld.None)
                {
                    AudioSource.PlayClipAtPoint(GenerateSFX, transform.position, CGDGameSettings.SoundVolume);
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    List<float> playersVerticalPositions = new List<float>();
                    foreach (GameObject player in players)
                    {
                        playersVerticalPositions.Add(player.transform.position.y);
                    }
                    float movementProbability;
                    float selfHeight = transform.position.y;
                    float firstPlaceHeight = playersVerticalPositions.Max();
                    bool selfInFirstPlace = false;
                    float lastPlaceHeight = playersVerticalPositions.Min();
                    float distSelfToFirstPlace = firstPlaceHeight - selfHeight;
                    float distFirstToLastPlace = firstPlaceHeight - lastPlaceHeight;
                    float playerPosition = distSelfToFirstPlace / (distFirstToLastPlace + Mathf.Epsilon); // Epsilon used to avoid divide by 0 issue
                    if (distSelfToFirstPlace < _tolerance)
                    {
                        selfInFirstPlace = true;
                    }
                    if (selfInFirstPlace)
                    {
                        movementProbability = 0.0f;
                    }
                    else
                    {
                        movementProbability = playerPosition;
                        movementProbability = Mathf.Max(movementProbability, 0.1f);
                    }
                    float randPowerUpType = Random.Range(0.0f, 1.0f);

                    bool giveMovementPowerUp = false;
                    if (randPowerUpType < movementProbability)
                    {
                        giveMovementPowerUp = true;
                    }
                    if (giveMovementPowerUp)
                    {
                        float randomMovementPowerUp = Random.Range(0.0f, 1.0f);
                        if (playerPosition > 0.7f)
                        {
                            if (randomMovementPowerUp < 0.2f)
                            {
                                GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.SpeedBoost;
                                GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.SpeedBoost);
                            }
                            else if (randomMovementPowerUp < 0.4f)
                            {
                                GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.JumpBoost;
                                GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.JumpBoost);
                            }
                            else
                            {
                                GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.SpeedAndJumpBoost;
                                GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.SpeedAndJumpBoost);
                            }
                        }
                        else
                        {
                            if (randomMovementPowerUp < 0.45f)
                            {
                                GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.SpeedBoost;
                                GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.SpeedBoost);
                            }
                            else if (randomMovementPowerUp < 0.9f)
                            {
                                GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.JumpBoost;
                                GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.JumpBoost);
                            }
                            else
                            {
                                GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.SpeedAndJumpBoost;
                                GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.SpeedAndJumpBoost);
                            }
                        }

                    }
                    else
                    {
                        int randomAreaDenialPowerUp = Random.Range(0, 18);
                        if (randomAreaDenialPowerUp < 6)
                        {
                            GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.Peel;
                            GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.Peel);
                        }
                        else if (randomAreaDenialPowerUp < 11)
                        {
                            GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.Spikes;
                            GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.Spikes);
                        }
                        else if (randomAreaDenialPowerUp < 15)
                        {
                            GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.PoisonCloud;
                            GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.PoisonCloud);
                        }
                        else
                        {
                            GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.LavaPool;
                            GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.LavaPool);
                        }
                    }
                }
            }
        }
    }
}
