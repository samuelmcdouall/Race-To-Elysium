using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDGMedusaBasicAttack : MonoBehaviour
{
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
        if (Input.GetMouseButtonDown(0) && _readyToRepel)
        {
            print("clicked");
            _repelCollider.enabled = true;
            _readyToRepel = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //ChangeColor(new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && collider.gameObject != OwnPlayer)
        {
            print("hit other player");
            Vector3 playerToEnemyDirection = Vector3.Normalize(collider.gameObject.transform.position - OwnPlayer.transform.position);
            Vector3 forceToAdd = playerToEnemyDirection * _repelForce;
            int photonViewID = collider.gameObject.GetComponent<PhotonView>().ViewID;
            OwnPlayer.GetComponent<CGDMedusaPlayer>().KnockbackOtherPlayer(forceToAdd, photonViewID);
        }
    }






    // maybe move actual physics calculation into fixedupdate
}
