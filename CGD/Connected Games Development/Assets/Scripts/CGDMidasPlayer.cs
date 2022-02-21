using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDMidasPlayer : CGDPlayer
{
    [Header("Basic Attack")]
    [SerializeField]
    float _basicAttackFireInterval;
    float _basicAttackFireIntervalTimer;
    [SerializeField]
    float _basicAttackForce;
    [SerializeField]
    float _basicAttackRange;
    bool _readyToFireBasicAttack;
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

            if (Input.GetMouseButtonDown(0))
            {
                if (_readyToFireBasicAttack)
                {
                    BasicAttack();
                }
                else
                {
                    print("Need to recharge projectile");
                }
            }

            if (!_readyToFireBasicAttack)
            {
                if (_basicAttackFireIntervalTimer > _basicAttackFireInterval)
                {
                    _basicAttackFireIntervalTimer = 0.0f;
                    _readyToFireBasicAttack = true;
                }
                else
                {
                    _basicAttackFireIntervalTimer += Time.deltaTime;
                }
            }
        }
    }

    public override void BasicAttack()
    {
        // todo will need to do the photon version of this in multiplayer
        print("Midas basic attack!");
        RaycastHit hit;
        Vector3 ForwardDirection = new Vector3(_cameraTr.forward.x, 0.0f, _cameraTr.forward.z);
        ForwardDirection = ForwardDirection.normalized;
        Debug.DrawRay(transform.position, ForwardDirection * _basicAttackRange, Color.cyan, 1.0f);
        if (Physics.Raycast(transform.position, ForwardDirection, out hit, _basicAttackRange))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                print("I just hit a player, knock them back!");
                Vector3 PlayerToEnemyDirection = Vector3.Normalize(hit.transform.gameObject.transform.position - transform.position);
                hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(PlayerToEnemyDirection * _basicAttackForce);
                //hit.transform.gameObject.GetComponent<CGDPlayer>().ApplySpeedModifierForSeconds(100.0f, _freezeDuration);
                // todo may want to do other things such as block camera rotation or abilites too
            }
        }
        else
        {
            print("Didn't hit anything!");
        }
        _readyToFireBasicAttack = false;
    }

    public override void InitialPlayerSetup()
    {
        PlayerRb = GetComponent<Rigidbody>();
        Cursor.visible = _showCursor;
        _cameraTr = Camera.main.transform;
        _ableToJumpOffGround = true;
        _speedModifier = 1.0f;
        _readyToFireBasicAttack = true;
        _basicAttackFireIntervalTimer = 0.0f;
    }
}
