using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CGDMedusaPlayer : CGDPlayer
{
    [Header("Ultimate Attack")]
    [SerializeField]
    float _freezeDuration;
    [SerializeField]
    float _freezeRange;
    PhotonView _view;
    public GameObject MainCamera;

    void Awake()
    {
        InitialPlayerSetup();
    }

    public override void FixedUpdate()
    {
        if (_view.IsMine)
        {
            base.FixedUpdate();
        }
    }
    public override void Update()
    {
        if (_view.IsMine)
        {
            if (_enabledControls)
            {
                HandleJumpMechanics();

                if (Input.GetMouseButtonDown(1))
                {
                    UltimateAttack();
                }
            }
        }
    }
    public override void UltimateAttack()
    {
        if (UltimateCharge == 100.0f)
        {
            print("Medusa ultimate attack!");
            UltimateCharge = 0.0f;
            UltimateBar.SetUltBar(UltimateCharge);
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
        else
        {
            print("Not enough charge!");
        }
    }
    public override void RechargeUltimateAttack()
    {
        //todo debug only
        print("Ready to freeze again!");
    }

    public override void InitialPlayerSetup()
    {
        PlayerRb = GetComponent<Rigidbody>();
        _view = GetComponent<PhotonView>();
        Cursor.visible = _showCursor;
        _cameraTr = Camera.main.transform;
        _ableToJumpOffGround = true;
        _speedModifier = 1.0f;
        UltimateCharge = 0.0f;
        if (!_view.IsMine)
        {
            Destroy(MainCamera);
        }
    }
}
