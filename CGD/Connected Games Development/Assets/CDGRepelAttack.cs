using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDGRepelAttack : MonoBehaviour
{
    public GameObject OwnPlayer;
    Collider _repelCollider;
    [SerializeField]
    float RepelBubbleDuration;
    float _repelBubbleDurationTimer;
    [SerializeField]
    float RepelForce;
    [SerializeField]
    float RepelCooldown;
    float _repelCooldownTimer;
    public bool _readyToRepel;


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
            if (_repelBubbleDurationTimer > RepelBubbleDuration)
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
                if (_repelCooldownTimer > RepelCooldown)
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
            _repelCollider.enabled = true;
            _readyToRepel = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && collider.gameObject != OwnPlayer)
        {
            print("hit other player");
            Vector3 PlayerToEnemyDirection = Vector3.Normalize(collider.gameObject.transform.position - transform.position);
            collider.gameObject.GetComponent<Rigidbody>().AddForce(PlayerToEnemyDirection * RepelForce, ForceMode.Force);
        }
    }
    // maybe move actual physics calculation into fixedupdate
}
