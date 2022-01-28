using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDDummyMovingPlayer : CGDPlayer
{
    [Header("AI Moving")]
    public Transform StartPosition;
    public Transform EndPosition;
    bool _aiStartToEnd;

    void Start()
    {
        InitialPlayerSetup();
    }

    public override void FixedUpdate()
    {
        LimitSpeedToMaximum();
        if (_aiStartToEnd)
        {
            Vector3 DistanceToTarget = Vector3.Normalize(EndPosition.position - transform.position);
            PlayerRb.AddForce(DistanceToTarget * PlayerMoveForce * _speedModifier);
            if (Vector3.Distance(transform.position, EndPosition.position) <= 0.1f)
            {
                _aiStartToEnd = false;
            }
        }
        else
        {
            Vector3 DistanceToTarget = Vector3.Normalize(StartPosition.position - transform.position);
            PlayerRb.AddForce(DistanceToTarget * PlayerMoveForce * _speedModifier);
            if (Vector3.Distance(transform.position, StartPosition.position) <= 0.1f)
            {
                _aiStartToEnd = true;
            }
        }
        
    }

    public override void InitialPlayerSetup()
    {
        PlayerRb = GetComponent<Rigidbody>();
        _aiStartToEnd = true;
        _speedModifier = 1.0f;
    }
}
