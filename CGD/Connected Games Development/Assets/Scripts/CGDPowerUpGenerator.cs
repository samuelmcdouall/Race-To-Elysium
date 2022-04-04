using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using UnityEngine.SceneManagement;

public class CGDPowerUpGenerator : MonoBehaviour
{
    PhotonView _view;
    [SerializeField]
    float _interval;
    float _timer;
    public AudioClip GenerateSFX;
    float _tolerance;

    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponent<PhotonView>();
        _tolerance = 0.1f;
        _timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene" && _view.IsMine)
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
                    print("power up obtained");
                    AudioSource.PlayClipAtPoint(GenerateSFX, transform.position, CGDGameSettings.SoundVolume);
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    List<float> playerListHeights = new List<float>();
                    foreach (GameObject p in players)
                    {
                        playerListHeights.Add(p.transform.position.y);
                    }
                    float movementProbability;
                    float selfHeight = transform.position.y;
                    float firstPlaceHeight = playerListHeights.Max();
                    bool selfInFirstPlace = false;
                    float lastPlaceHeight = playerListHeights.Min();
                    float distSelfToFirstPlace = firstPlaceHeight - selfHeight;
                    float distFirstToLastPlace = firstPlaceHeight - lastPlaceHeight;
                    float playerPosition = distSelfToFirstPlace / distFirstToLastPlace;
                    if (firstPlaceHeight - selfHeight < _tolerance)
                    {
                        selfInFirstPlace = true;
                    }
                    if (selfInFirstPlace)
                    {
                        movementProbability = 0.0f;
                    }
                    else
                    {
                        movementProbability = playerPosition; //todo was 0.5f * originally
                        movementProbability = Mathf.Max(movementProbability, 0.1f);
                    }
                    print("Movement Probability: " + movementProbability);
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
                            if (0.0f <= randomMovementPowerUp && randomMovementPowerUp < 0.2f)
                            {
                                GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.SpeedBoost;
                                GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.SpeedBoost);
                            }
                            else if (0.2f <= randomMovementPowerUp && randomMovementPowerUp < 0.4f)
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
                            if (0.0f <= randomMovementPowerUp && randomMovementPowerUp < 0.45f)
                            {
                                GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.SpeedBoost;
                                GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.SpeedBoost);
                            }
                            else if (0.45f <= randomMovementPowerUp && randomMovementPowerUp < 0.9f)
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
                        if (randomAreaDenialPowerUp <= 5)
                        {
                            GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.Peel;
                            GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.Peel);
                        }
                        else if (6 <= randomAreaDenialPowerUp && randomAreaDenialPowerUp <= 10)
                        {
                            GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.Spikes;
                            GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.Spikes);
                        }
                        else if (11 <= randomAreaDenialPowerUp && randomAreaDenialPowerUp <= 14)
                        {
                            GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.PoisonCloud;
                            GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.PoisonCloud);
                        }
                        else if (15 <= randomAreaDenialPowerUp && randomAreaDenialPowerUp <= 17)
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
