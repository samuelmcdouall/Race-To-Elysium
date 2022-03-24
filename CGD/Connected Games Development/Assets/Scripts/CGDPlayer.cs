using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [System.NonSerialized]
    public float UltimateCharge;
    public CGDUltimateBar UltimateBar;
    float _ultPickupDelay = 0.5f; // can't modifiy ult charge for at least 0.5 seconds, not brilliant but will do for now
    bool _ableToPickupUlt = true;
    [System.NonSerialized]
    public PhotonView _view;
    public CGDGroundCheck GroundCheck;
    //[System.NonSerialized]
    public GameObject WonScreen;
    //[System.NonSerialized]
    public GameObject LossScreen;
    public CGDRotateCamera RotateCamera;

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
        if (_enabledControls && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
        {
            HandleMovementMechanics();
        }
    }

    [PunRPC] // todo maybe better way of doing this
    public void ApplySpeedModifierForSecondsToGivenPlayer(float modiferPercentage, float duration, int photonViewID, bool sendToOtherPlayers)
    {
        // can only have one slow at a time

        PhotonView photonView = PhotonView.Find(photonViewID);
        photonView.gameObject.GetComponent<CGDPlayer>().ApplySpeedModifierForSeconds(modiferPercentage, duration);
        if (sendToOtherPlayers)
        {
            Debug.Log("send apply speed mod for seconds instruction to other players");
            _view.RPC("ApplySpeedModifierForSecondsToGivenPlayer", RpcTarget.OthersBuffered, modiferPercentage, duration, photonViewID, false);
        }
        else
        {
            Debug.Log("Got this apply speed mod for seconds instruction from other player");
        }
    }
    public void ApplySpeedModifierForSeconds(float modiferPercentage, float duration)
    {
        //if (_speedModifier == 1.0f)
        //{
            _speedModifier = 1.0f - (modiferPercentage / 100.0f);
            Invoke("ResetSpeedModifier", duration);
        //}
    }
    public void ResetSpeedModifier()
    {
        _speedModifier = 1.0f;
        print("returned to normal speed");
    }
    [PunRPC] // todo maybe better way of doing this
    public void DisableControlsForSecondsToGivenPlayer(float Duration, int photonViewID, bool sendToOtherPlayers)
    {
        // can only have one slow at a time

        PhotonView photonView = PhotonView.Find(photonViewID);
        photonView.gameObject.GetComponent<CGDPlayer>().DisableControlsForSeconds(Duration);
        if (sendToOtherPlayers)
        {
            Debug.Log("send apply speed mod for seconds instruction to other players");
            _view.RPC("DisableControlsForSecondsToGivenPlayer", RpcTarget.OthersBuffered, Duration, photonViewID, false);
        }
        else
        {
            Debug.Log("Got this apply speed mod for seconds instruction from other player");
        }
    }
    public void DisableControlsForSeconds(float Duration)
    {
        _enabledControls = false;
        Invoke("EnableControls", Duration);
    }
    public void EnableControls()
    {
        _enabledControls = true;
        print("returned to normal speed");
    }
    public virtual void Update()
    {
        if (_enabledControls && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
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
            if (PlayerRb.velocity.magnitude > _playerTopSpeed * _speedModifier)
            {
                float original_vertical_speed = PlayerRb.velocity.y;
                PlayerRb.velocity = PlayerRb.velocity.normalized * _playerTopSpeed * _speedModifier;
                PlayerRb.velocity = new Vector3(PlayerRb.velocity.x, original_vertical_speed, PlayerRb.velocity.z);
            }
        }
        PlayerRb.velocity = PlayerRb.velocity * 1.0f; // todo should probably remove this, as its going to get continually faster
    }

    public void HandleGroundCheckMechanics()
    {
        if (GroundCheck.IsGrounded)
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
            if (GroundCheck.IsGrounded && _ableToJumpOffGround)
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

    public void ModifyUltimateCharge(float charge_amount)
    {
        if (_ableToPickupUlt)
        {
            _ableToPickupUlt = false;
            Invoke("AbleToPickUltAgain", _ultPickupDelay);
            if (charge_amount > 0.0f)
            {
                print("Increased charge by: " + charge_amount);
            }
            else
            {
                print("Decreased charge by: " + -charge_amount);
            }
            UltimateCharge += charge_amount;
            if (UltimateCharge > 100.0f)
            {
                UltimateCharge = 100.0f;
            }
            else if (UltimateCharge < 0.0f)
            {
                UltimateCharge = 0.0f;
            }
            if (_view.IsMine)
            {
                UltimateBar.SetUltBar(UltimateCharge);
            }
            print("Ultimate Charge: " + UltimateCharge);
            // set the new value here in the bar
        }
    }

    void AbleToPickUltAgain()
    {
        _ableToPickupUlt = true;
    }

    //[PunRPC]
    //public void DisplayWinScreen() // gonna have to do it by ID
    //{
    //    Debug.Log("send victory instruction to other players, means i won");
    //    WonScreen.SetActive(true);
    //    Cursor.visible = true;
    //    _enabledControls = false;
    //    RotateCamera._enabledCameraControls = false;
    //    _view.RPC("DisplayLossScreen", RpcTarget.OthersBuffered);
    //}

    //[PunRPC]
    //public void DisplayLossScreen() // gonna have to do it by ID
    //{
    //    Debug.Log("Got this victory instruction from other player, means i lost");
    //    LossScreen.SetActive(true);
    //    Cursor.visible = true;
    //    _enabledControls = false;
    //    RotateCamera._enabledCameraControls = false;
        
    //}
    [PunRPC]
    public void DisplayGameOverScreen() // gonna have to do it by ID
    {
        if (_view.IsMine)
        {
            Debug.Log("send victory instruction to other players, means i won");
            CGDGameOverScreenManager.DisplayWinScreen();
            //_view.RPC("GameOverScreen", RpcTarget.OthersBuffered);
        }
        else
        {
            Debug.Log("Got this victory instruction from other player, means i lost");
            CGDGameOverScreenManager.DisplayLossScreen();
            //LossScreen.SetActive(true);
            //Cursor.visible = true;
            //_enabledControls = false;
            //RotateCamera._enabledCameraControls = false;
        }
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

    [PunRPC]
    public void KnockbackOtherPlayer(Vector3 forceToAdd, int photonViewID)
    {
        PhotonView photonView = PhotonView.Find(photonViewID);
        photonView.gameObject.GetComponent<Rigidbody>().AddForce(forceToAdd);
        if (_view.IsMine)
        {
            Debug.Log("send message to other players");
            _view.RPC("KnockbackOtherPlayer", RpcTarget.OthersBuffered, forceToAdd, photonViewID);
        }
        else
        {
            Debug.Log("Got this instruction from other player");
        }
    }
}
