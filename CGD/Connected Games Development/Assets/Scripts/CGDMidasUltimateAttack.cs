using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDMidasUltimateAttack : MonoBehaviour
{
    public GameObject OwnPlayer;
    Collider _slowCollider;
    [SerializeField]
    float _slowBubbleDuration;
    float _slowBubbleDurationTimer;
    [SerializeField]
    float _slowDuration;
    [SerializeField]
    float _slowPercentage;
    public bool _readyToSlow;


    void Start()
    {
        _slowCollider = GetComponent<Collider>();
        _slowCollider.enabled = false;
        _slowBubbleDurationTimer = 0.0f;
        _readyToSlow = true;
    }

    void Update()
    {
        if (_slowCollider.enabled)
        {
            if (_slowBubbleDurationTimer > _slowBubbleDuration)
            {
                _slowBubbleDurationTimer = 0.0f;
                _slowCollider.enabled = false;
            }
            else
            {
                _slowBubbleDurationTimer += Time.deltaTime;
            }
        }
        if (Input.GetMouseButtonDown(1) && _readyToSlow)
        {
            _slowCollider.enabled = true;
            _readyToSlow = false;
        }
        if (Input.GetMouseButtonDown(2) && !_readyToSlow)
        {
            // todo actual mechanics for charging, not just middle clicking
            print("recharged ultimate");
            _readyToSlow = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && collider.gameObject != OwnPlayer)
        {
            print("bubble slow affected other player");
            collider.gameObject.GetComponent<CGDPlayer>().ApplySpeedModifierForSeconds(_slowPercentage, _slowDuration);
        }
    }
    // maybe move actual physics calculation into fixedupdate
}
