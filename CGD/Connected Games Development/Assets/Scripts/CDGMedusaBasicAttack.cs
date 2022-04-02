using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDGMedusaBasicAttack : MonoBehaviour
{
    // todo rename this because its being applied to everyone
    public GameObject OwnPlayer;
    Collider _repelCollider;
    [SerializeField]
    float _repelBubbleDuration;
    float _repelBubbleDurationTimer;
    [SerializeField]
    float _repelForce;
    [SerializeField]
    float _repelCooldown;
    float _repelCooldownTimer;
    public bool _readyToRepel;
    Rigidbody _tempRb;
    public AudioClip AttackSFX1;
    public AudioClip AttackSFX2;
    public AudioClip AttackHitPlayer;
    public AudioClip AttackHitGate;


    void Start()
    {
        _repelCollider = GetComponent<Collider>();
        _repelCollider.enabled = false;
        _repelBubbleDurationTimer = 0.0f;
        _repelCooldownTimer = 0.0f;
        _readyToRepel = true;
    }

    void Update()
    {
        if (_repelCollider.enabled)
        {
            if (_repelBubbleDurationTimer > _repelBubbleDuration)
            {
                _repelBubbleDurationTimer = 0.0f;
                _repelCollider.enabled = false;
            }
            else
            {
                _repelBubbleDurationTimer += Time.deltaTime;
            }
        }
        else
        {
            if (!_readyToRepel)
            {
                if (_repelCooldownTimer > _repelCooldown)
                {
                    _repelCooldownTimer = 0.0f;
                    _readyToRepel = true;
                }
                else
                {
                    _repelCooldownTimer += Time.deltaTime;
                }
            }
        }
        if (Input.GetMouseButtonDown(0) && _readyToRepel && OwnPlayer.GetComponent<CGDPlayer>()._enabledControls && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
        {
            print("clicked");
            int randSoundEffect = Random.Range(0, 2);
            if (randSoundEffect == 0)
            {
                AudioSource.PlayClipAtPoint(AttackSFX1, OwnPlayer.transform.position, CGDGameSettings.SoundVolume);
            }
            else
            {
                AudioSource.PlayClipAtPoint(AttackSFX2, OwnPlayer.transform.position, CGDGameSettings.SoundVolume);
            }
            _repelCollider.enabled = true;
            _readyToRepel = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && collider.gameObject != OwnPlayer)
        {
            AudioSource.PlayClipAtPoint(AttackHitPlayer, OwnPlayer.transform.position, CGDGameSettings.SoundVolume);
            print("hit other player");
            Vector3 playerToEnemyDirection = Vector3.Normalize(collider.gameObject.transform.position - OwnPlayer.transform.position);
            Vector3 forceToAdd = playerToEnemyDirection * _repelForce;
            int photonViewID = collider.gameObject.GetComponent<PhotonView>().ViewID;
            OwnPlayer.GetComponent<CGDPlayer>().KnockbackOtherPlayer(forceToAdd, photonViewID);
            OwnPlayer.GetComponent<CGDPlayer>().ModifyUltimateCharge(20.0f);
            collider.gameObject.GetComponent<CGDPlayer>().DisableControlsForSecondsToGivenPlayer(0.8f, photonViewID, true);
        }
        else if (collider.tag == "Gate")
        {
            print("hit gate");
            AudioSource.PlayClipAtPoint(AttackHitGate, OwnPlayer.transform.position, CGDGameSettings.SoundVolume);
            collider.gameObject.GetComponent<CGDGate>().ReduceHealthOfGateForAllPlayers();
        }
    }






    // maybe move actual physics calculation into fixedupdate
}
