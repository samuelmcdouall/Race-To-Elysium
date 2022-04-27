using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CGDPowerUp : MonoBehaviour
{
    public AudioClip CollectSFX;
    float _tolerance;

    void Start()
    {
        _tolerance = 0.1f;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && false) //delete this script won't be used as pickup is handled as generator
        {
            if (other.gameObject.GetComponent<CGDPowerUpManager>()._powerUpHeld == CGDPowerUpManager.PowerUpHeld.None)
            {
                print("power up obtained");
                AudioSource.PlayClipAtPoint(CollectSFX, transform.position, CGDGameSettings.SoundVolume);
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                List<float> playerListHeights = new List<float>();
                foreach (GameObject p in players)
                {
                    playerListHeights.Add(p.transform.position.y);
                }
                float movementProbability;
                float selfHeight = other.gameObject.transform.position.y;
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
                    movementProbability = playerPosition;
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
                            other.gameObject.GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.SpeedBoost;
                            other.gameObject.GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.SpeedBoost);
                        }
                        else if (0.2f <= randomMovementPowerUp && randomMovementPowerUp < 0.4f)
                        {
                            other.gameObject.GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.JumpBoost;
                            other.gameObject.GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.JumpBoost);
                        }
                        else
                        {
                            other.gameObject.GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.SpeedAndJumpBoost;
                            other.gameObject.GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.SpeedAndJumpBoost);
                        }
                    }
                    else
                    {
                        if (0.0f <= randomMovementPowerUp && randomMovementPowerUp < 0.45f)
                        {
                            other.gameObject.GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.SpeedBoost;
                            other.gameObject.GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.SpeedBoost);
                        }
                        else if (0.45f <= randomMovementPowerUp && randomMovementPowerUp < 0.9f)
                        {
                            other.gameObject.GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.JumpBoost;
                            other.gameObject.GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.JumpBoost);
                        }
                        else
                        {
                            other.gameObject.GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.SpeedAndJumpBoost;
                            other.gameObject.GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.SpeedAndJumpBoost);
                        }
                    }

                }
                else
                {
                    int randomAreaDenialPowerUp = Random.Range(0, 18);
                    if (randomAreaDenialPowerUp <= 5)
                    {
                        other.gameObject.GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.Peel;
                        other.gameObject.GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.Peel);
                    }
                    else if (6 <= randomAreaDenialPowerUp && randomAreaDenialPowerUp <= 10)
                    {
                        other.gameObject.GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.Spikes;
                        other.gameObject.GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.Spikes);
                    }
                    else if (11 <= randomAreaDenialPowerUp && randomAreaDenialPowerUp <= 14)
                    {
                        other.gameObject.GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.PoisonCloud;
                        other.gameObject.GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.PoisonCloud);
                    }
                    else if (15 <= randomAreaDenialPowerUp && randomAreaDenialPowerUp <= 17)
                    {
                        other.gameObject.GetComponent<CGDPowerUpManager>()._powerUpHeld = CGDPowerUpManager.PowerUpHeld.LavaPool;
                        other.gameObject.GetComponent<CGDPowerUpManager>().DisplayPowerUpIcon(CGDPowerUpManager.PowerUpHeld.LavaPool);
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}
