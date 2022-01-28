using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDPlayer : MonoBehaviour
{
    [Header("Physics")]
    [System.NonSerialized]
    public Rigidbody PlayerRb;
    public float PlayerMoveForce;
    public float _playerTopSpeed;
    public float _playerJumpForce;
    public float _playerFallForce;
    [System.NonSerialized]
    public bool _ableToJumpOffGround;
    //[SerializeField]
    //float BounceForce;
    //float jump_delay_timer;
    //[SerializeField]
    //float jump_delay;

    // Camera
    [System.NonSerialized]
    public Transform _cameraTr;
    //public static Transform last_camera_tr;

    // Animations
    //[System.NonSerialized]
    //public Animator player_ani;
    //[System.NonSerialized]
    //public string running_animation;
    //string jump_up_animation;
    //string falling_down_animation;
    //string kneel_animation;
    //string stand_up_animation;

    //[Header("Timeline")]
    //[SerializeField]
    //float initial_disable_control_period;

    //[Header("UI")]
    //public GameObject pause_menu;
    //public GameObject options_menu;
    //public GameObject controls_menu;

    [Header("Debug Testing")]
    public bool _playerCanMoveFast;
    public float _playerTopFastMoveSpeed;
    public bool _showCursor;
    public bool _enabledControls;

    protected float _speedModifier;

    void Start()
    {
        InitialPlayerSetup();
    }

    public virtual void FixedUpdate()
    {
        LimitSpeedToMaximum();
        HandleGroundCheckMechanics();
        if (_enabledControls)
        {
            HandleMovementMechanics();
        }
    }

    public void ApplySpeedModifierForSeconds(float ModiferPercentage, float Duration)
    {
        // can only have one slow at a time
        if (_speedModifier == 1.0f)
        {
            _speedModifier = 1.0f - (ModiferPercentage / 100.0f);
            Invoke("ResetSpeedModifier", Duration);
        }
    }
    public void ResetSpeedModifier()
    {
        _speedModifier = 1.0f;
        print("returned to normal speed");
    }
    public virtual void Update()
    {
        if (_enabledControls)
        {
            HandleJumpMechanics();
        }
    }
    public virtual void BasicAttack()
    {
        print("Player basic attack");
    }
    public virtual void UltimateAttack()
    {
        print("Player ultimate attack");
    }
    public virtual void RechargeUltimateAttack()
    {
        print("Player recharge ultimate attack");
    }



    public virtual void InitialPlayerSetup()
    {
        PlayerRb = GetComponent<Rigidbody>();
        Cursor.visible = _showCursor;
        _cameraTr = Camera.main.transform;
        _ableToJumpOffGround = true;
        _speedModifier = 1.0f;
    }

    public void LimitSpeedToMaximum()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _playerCanMoveFast)
        {
            if (PlayerRb.velocity.magnitude > _playerTopFastMoveSpeed)
            {
                float original_vertical_speed = PlayerRb.velocity.y;
                PlayerRb.velocity = PlayerRb.velocity.normalized * _playerTopFastMoveSpeed;
                PlayerRb.velocity = new Vector3(PlayerRb.velocity.x, original_vertical_speed, PlayerRb.velocity.z);
            }
        }
        else
        {
            if (PlayerRb.velocity.magnitude > _playerTopSpeed)
            {
                float original_vertical_speed = PlayerRb.velocity.y;
                PlayerRb.velocity = PlayerRb.velocity.normalized * _playerTopSpeed;
                PlayerRb.velocity = new Vector3(PlayerRb.velocity.x, original_vertical_speed, PlayerRb.velocity.z);
            }
        }
        PlayerRb.velocity = PlayerRb.velocity * _speedModifier;
    }

    public void HandleGroundCheckMechanics()
    {
        if (CGDGroundCheck.IsGrounded)
        {
            DetermineIfReadyToJumpOffGround();
        }
        else if (PlayerRb.velocity.y < -0.1f)
        {
            HandlePlayerFallingDown();
        }
    }

    public void DetermineIfReadyToJumpOffGround()
    {
        if (!_ableToJumpOffGround)
        {
            _ableToJumpOffGround = true;
        }
        else if (!_ableToJumpOffGround)
        {
            //jump_delay_timer += Time.fixedDeltaTime;
        }
    }

    public void HandlePlayerFallingDown()
    {
        //PlayerRb.AddForce(0.0f, -PlayerFallForce, 0.0f);
    }

    public void HandleMovementMechanics()
    {
        if (Camera.main)
        {
            ApplyNormalAngleMovement();
        }
    }

    public void ApplyNormalAngleMovement()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            DetermineYIndependentVelocity(_cameraTr.forward - _cameraTr.right);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            DetermineYIndependentVelocity(_cameraTr.forward + _cameraTr.right);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            DetermineYIndependentVelocity(_cameraTr.forward - _cameraTr.right);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            DetermineYIndependentVelocity(-_cameraTr.forward + _cameraTr.right);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            DetermineYIndependentVelocity(-_cameraTr.forward - _cameraTr.right);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            DetermineYIndependentVelocity(_cameraTr.forward);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            DetermineYIndependentVelocity(-_cameraTr.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            DetermineYIndependentVelocity(_cameraTr.right);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            DetermineYIndependentVelocity(-_cameraTr.right);
        }
        else
        {
            //player_ani.SetBool(running_animation, false);
        }
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    PlayerRb.AddForce(Vector3.forward * BounceForce);
        //}
    }

    public void DetermineYIndependentVelocity(Vector3 horizontal_direction)
    {
        horizontal_direction = new Vector3(horizontal_direction.x, 0.0f, horizontal_direction.z);
        horizontal_direction = horizontal_direction.normalized * PlayerMoveForce;
        PlayerRb.AddForce(horizontal_direction);
    }

    public void HandleJumpMechanics()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CGDGroundCheck.IsGrounded && _ableToJumpOffGround)
            {
                JumpUp();
                //Invoke("JumpUp", 0.0f);
                _ableToJumpOffGround = false;
            }
        }
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    PlayerRb.AddForce(Vector3.forward * BounceForce);
        //}
    }

    public void JumpUp()
    {
        PlayerRb.AddForce(0.0f, _playerJumpForce, 0.0f, ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MovingPlatform" && transform.position.y > collision.gameObject.transform.position.y)
        {
            transform.SetParent(collision.gameObject.transform);
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            transform.SetParent(null);
        }
    }
}
