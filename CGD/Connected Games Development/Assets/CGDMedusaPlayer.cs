using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDMedusaPlayer : CGDPlayer
{
    [Header("Ultimate")]
    [SerializeField]
    float _freezeDuration;
    [SerializeField]
    float _freezeRange;
    bool _readyToFreeze;

    void Start()
    {
        InitialPlayerSetup();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void Update()
    {
        if (_enabledControls)
        {
            HandleJumpMechanics();

            if (Input.GetMouseButtonDown(1))
            {
                if (_readyToFreeze)
                {
                    UltimateAttack();
                }
                else
                {
                    print("Need to recharge ultimate");
                }
            }
            if (Input.GetMouseButtonDown(2) && !_readyToFreeze)
            {
                // todo actual mechanics for charging, not just middle clicking
                RechargeUltimateAttack();
            }
        }
    }
    public override void UltimateAttack()
    {
        print("Medusa ultimate attack!");
        _readyToFreeze = false;
        RaycastHit hit;
        Vector3 ForwardDirection = new Vector3(_cameraTr.forward.x, 0.0f, _cameraTr.forward.z);
        ForwardDirection = ForwardDirection.normalized;
        Debug.DrawRay(transform.position, ForwardDirection * _freezeRange, Color.cyan, 1.0f);
        if (Physics.Raycast(transform.position, ForwardDirection, out hit, _freezeRange))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                print("I just hit a player, freeze them!");
                hit.transform.gameObject.GetComponent<CGDPlayer>().ApplySpeedModifierForSeconds(100.0f, _freezeDuration);
                // todo may want to do other things such as block camera rotation or abilites too
            }
        }
        else
        {
            print("Didn't hit anything!");
        }
    }
    public override void RechargeUltimateAttack()
    {
        //todo debug only
        print("Ready to freeze again!");
        _readyToFreeze = true;
    }

    public override void InitialPlayerSetup()
    {
        PlayerRb = GetComponent<Rigidbody>();
        Cursor.visible = _showCursor;
        _cameraTr = Camera.main.transform;
        _ableToJumpOffGround = true;
        _speedModifier = 1.0f;
        _readyToFreeze = true;
    }
}
