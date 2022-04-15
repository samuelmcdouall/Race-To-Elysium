using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;
using TMPro;

public class CGDPlayer : MonoBehaviour
{
    [Header("Physics")]
    Rigidbody _playerRb;
    [SerializeField]
    float _playerMoveForce;
    [SerializeField]
    float _playerTopSpeed;
    [SerializeField]
    float _playerJumpForce;
    [SerializeField]
    float _playerFallForce;
    public CGDGroundCheck GroundCheck;
    bool _ableToJumpOffGround;
    bool _sliding = false;
    float _speedModifier;
    float _jumpModifier;
    bool _shouldJumpUp;
    [System.NonSerialized]
    public bool _enabledControls;

    [Header("Ultimate Attack")]
    [System.NonSerialized]
    public float UltimateCharge;
    public CGDUIBar UltimateBar;
    float _ultPickupDelay = 0.5f; // precaution delay so multiple colliders don't get hit in same frame todo may remove this comment
    bool _ableToPickupUlt = true;

    [Header("General")]
    [System.NonSerialized]
    public PhotonView View;
    [System.NonSerialized]
    public Character NewCharacter;
    [System.NonSerialized]
    public Character ThisCharacter;
    public GameObject MedusaPlayer;
    public GameObject MidasPlayer;
    public GameObject NarcissusPlayer;
    public GameObject ArachnePlayer;

    [Header("UI")]
    public GameObject WonScreen;
    public GameObject LossScreen;
    public GameObject BlindScreen;
    public TextMeshProUGUI NameText;

    [Header("Checkpoint")]
    [System.NonSerialized]
    public Vector3 CheckpointPosition;
    float _checkpointOffset;

    [Header("FX/SFX")]
    public AudioClip JumpSFX;
    public AudioClip UltSFX;
    public GameObject LavaBurnFX;
    public GameObject PoisonBurnFX;
    public AudioClip NewPlayerSFX;
    public GameObject NewPlayerFX;
    [System.NonSerialized]
    public Outline PlayerOutline;

    [Header("Camera")]
    public GameObject MainCamera;
    [System.NonSerialized]
    public Transform CameraTr;

    void Start()
    {
        InitialPlayerSetup();
    }

    void FixedUpdate()
    {
        if (View.IsMine)
        {
            MovePlayerToCheckpointIfBehind();
            HandleMovementAndJumping();
        }
    }
    public virtual void Update()
    {
        if (View.IsMine)
        {
            if (SceneManager.GetActiveScene().name == "GameScene" && !CGDSpawnGateTimer._gameStarted && Input.GetKeyDown(KeyCode.E))
            {
                ChangeCharacter();
            }
            if (_enabledControls && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                DetectJumpInput();
            }
            TintSkyboxBasedOnVerticalPosition();
        }
    }

    public virtual void InitialPlayerSetup()
    {
        _playerRb = GetComponent<Rigidbody>();
        View = GetComponent<PhotonView>();
        Cursor.visible = false;
        CameraTr = Camera.main.transform;
        _ableToJumpOffGround = true;
        _speedModifier = 1.0f;
        _jumpModifier = 1.0f;
        _shouldJumpUp = false;
        _enabledControls = true;
        UltimateCharge = 0.0f;
        Cursor.lockState = CursorLockMode.Locked;
        NewCharacter = Character.None;
        PlayerOutline = GetComponent<Outline>();
        PlayerOutline.enabled = false;
        CheckpointPosition = new Vector3(0.0f, 0.0f, 0.0f);
        _checkpointOffset = 2.0f;

        if (!View.IsMine)
        {
            print(View.Owner.NickName + " has joined the game");
            NameText.text = View.Owner.NickName;
        }
        else
        {
            NameText.text = "";
        }

        if (!View.IsMine)
        {
            Destroy(MainCamera);
        }
    }

    // FixedUpdate functions
    void MovePlayerToCheckpointIfBehind()
    {
        if (transform.position.y < CheckpointPosition.y - _checkpointOffset)
        {
            float randX = Random.Range(-5.0f, 5.0f);
            transform.position = new Vector3(CheckpointPosition.x + randX, CheckpointPosition.y, CheckpointPosition.z);
        }
    }

    void HandleMovementAndJumping()
    {
        LimitSpeedToMaximum();
        if (_sliding)
        {
            _playerRb.velocity = _playerRb.velocity.normalized * _playerTopSpeed * 2.0f;
        }
        if (_enabledControls && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
        {
            HandleMovementMechanics();
        }
        HandleGroundCheckMechanics();
        if (_shouldJumpUp)
        {
            JumpUp();
            AudioSource.PlayClipAtPoint(JumpSFX, transform.position, CGDGameSettings.SoundVolume);
            _shouldJumpUp = false;
            _ableToJumpOffGround = false;
        }
    }

    void LimitSpeedToMaximum()
    {
        if (_playerRb.velocity.magnitude > _playerTopSpeed * _speedModifier)
        {
            float original_vertical_speed = _playerRb.velocity.y;
            _playerRb.velocity = _playerRb.velocity.normalized * _playerTopSpeed * _speedModifier;
            _playerRb.velocity = new Vector3(_playerRb.velocity.x, original_vertical_speed, _playerRb.velocity.z);
        }
    }

    void HandleMovementMechanics()
    {
        if (Camera.main)
        {
            ApplyNormalAngleMovement();
        }
    }

    void ApplyNormalAngleMovement()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            DetermineYIndependentVelocity(CameraTr.forward - CameraTr.right);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            DetermineYIndependentVelocity(CameraTr.forward + CameraTr.right);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            DetermineYIndependentVelocity(CameraTr.forward - CameraTr.right);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            DetermineYIndependentVelocity(-CameraTr.forward + CameraTr.right);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            DetermineYIndependentVelocity(-CameraTr.forward - CameraTr.right);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            DetermineYIndependentVelocity(CameraTr.forward);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            DetermineYIndependentVelocity(-CameraTr.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            DetermineYIndependentVelocity(CameraTr.right);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            DetermineYIndependentVelocity(-CameraTr.right);
        }
        else
        {
            //player_ani.SetBool(running_animation, false); todo where animation set
        }
    }

    void DetermineYIndependentVelocity(Vector3 horizontal_direction)
    {
        horizontal_direction = new Vector3(horizontal_direction.x, 0.0f, horizontal_direction.z);
        horizontal_direction = horizontal_direction.normalized * _playerMoveForce;
        _playerRb.AddForce(horizontal_direction);
    }

    void HandleGroundCheckMechanics()
    {
        if (GroundCheck.IsGrounded && !_ableToJumpOffGround)
        {
            _ableToJumpOffGround = true;
        }
        else if (_playerRb.velocity.y < -0.1f)
        {
            HandlePlayerFallingDown();
        }
    }

    void HandlePlayerFallingDown()
    {
        //PlayerRb.AddForce(0.0f, -PlayerFallForce, 0.0f); //todo keep this in as will need to do animation here as well
    }

    void JumpUp()
    {
        _playerRb.AddForce(0.0f, _playerJumpForce * _jumpModifier, 0.0f, ForceMode.Impulse);
    }


    // Update functions
    void ChangeCharacter()
    {
        if (NewCharacter == Character.Medusa && ThisCharacter != Character.Medusa)
        {
            SwitchCharacterTo(MedusaPlayer.name);
        }
        if (NewCharacter == Character.Midas && ThisCharacter != Character.Midas)
        {
            SwitchCharacterTo(MidasPlayer.name);
        }
        if (NewCharacter == Character.Narcissus && ThisCharacter != Character.Narcissus)
        {
            SwitchCharacterTo(NarcissusPlayer.name);
        }
        if (NewCharacter == Character.Arachne && ThisCharacter != Character.Arachne)
        {
            SwitchCharacterTo(ArachnePlayer.name);
        }
    }

    void SwitchCharacterTo(string newPlayerName)
    {
        GameObject newPlayer = PhotonNetwork.Instantiate(newPlayerName, transform.position, transform.rotation);
        newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseX = MainCamera.GetComponent<CGDRotateCamera>()._mouseX;
        newPlayer.GetComponent<CGDPlayer>().MainCamera.GetComponent<CGDRotateCamera>()._mouseY = MainCamera.GetComponent<CGDRotateCamera>()._mouseY;
        AudioSource.PlayClipAtPoint(NewPlayerSFX, MainCamera.transform.position, CGDGameSettings.SoundVolume);
        Instantiate(NewPlayerFX, transform.position, Quaternion.identity);
        PhotonNetwork.Destroy(gameObject);
    }

    void DetectJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GroundCheck.IsGrounded && _ableToJumpOffGround)
            {
                _shouldJumpUp = true;
            }
        }
    }

    void TintSkyboxBasedOnVerticalPosition()
    {
        float scaledColorPosition = transform.position.y / 50.0f;
        if (scaledColorPosition > 1.0f)
        {
            scaledColorPosition = 1.0f;
        }
        else if (scaledColorPosition < 0.0f)
        {
            scaledColorPosition = 0.0f;
        }

        Color tartarusColor = new Color(1.0f, 0.0f, 0.0f);
        Color normalColour = new Color(0.45f, 0.45f, 0.45f);
        Color skyboxColor = Color.Lerp(tartarusColor, normalColour, scaledColorPosition);
        RenderSettings.skybox.SetColor("_Tint", skyboxColor);
    }

    // RPC functions
    [PunRPC]
    public void KnockbackOtherPlayer(Vector3 forceToAdd, int photonViewID)
    {
        PhotonView photonView = PhotonView.Find(photonViewID);
        photonView.gameObject.GetComponent<Rigidbody>().AddForce(forceToAdd);
        if (View.IsMine)
        {
            Debug.Log("send message to other players");
            View.RPC("KnockbackOtherPlayer", RpcTarget.OthersBuffered, forceToAdd, photonViewID);
        }
        else
        {
            Debug.Log("Got this instruction from other player");
        }
    }

    [PunRPC]
    public void ApplySpeedModifierForSecondsToGivenPlayer(float modiferPercentage, float duration, int photonViewID, bool sendToOtherPlayers)
    {
        PhotonView photonView = PhotonView.Find(photonViewID);
        photonView.gameObject.GetComponent<CGDPlayer>().ApplySpeedModifierForSeconds(modiferPercentage, duration);
        if (sendToOtherPlayers)
        {
            Debug.Log("Send apply speed mod for seconds instruction to other players");
            View.RPC("ApplySpeedModifierForSecondsToGivenPlayer", RpcTarget.OthersBuffered, modiferPercentage, duration, photonViewID, false);
        }
        else
        {
            Debug.Log("Got this apply speed mod for seconds instruction from other player");
        }
    }

    public void ApplySpeedModifierForSeconds(float modiferPercentage, float duration)
    {
        _speedModifier = 1.0f - (modiferPercentage / 100.0f);
        Invoke("ResetSpeedModifier", duration);
    }

    public void ResetSpeedModifier()
    {
        _speedModifier = 1.0f;
        print("returned to normal speed");
    }

    [PunRPC]
    public void DisableControlsForSecondsToGivenPlayer(float Duration, int photonViewID, bool sendToOtherPlayers)
    {
        PhotonView photonView = PhotonView.Find(photonViewID);
        photonView.gameObject.GetComponent<CGDPlayer>().DisableControlsForSeconds(Duration);
        if (sendToOtherPlayers)
        {
            Debug.Log("send disable controls for seconds instruction to other players");
            View.RPC("DisableControlsForSecondsToGivenPlayer", RpcTarget.OthersBuffered, Duration, photonViewID, false);
        }
        else
        {
            Debug.Log("Got this disable controls for seconds instruction from other player");
        }
    }

    public void DisableControlsForSeconds(float Duration)
    {
        _enabledControls = false;
        Invoke("EnableControls", Duration);
    }

    void EnableControls()
    {
        _enabledControls = true;
        print("returned to normal speed");
    }

    [PunRPC]
    public void DisplayBlindScreenForSecondsToGivenPlayer(float fullBlindDuration, float fadeOutDuration, int photonViewID, bool sendToOtherPlayers)
    {
        if (photonViewID == View.ViewID && View.IsMine)
        {
            DisplayFullBlindScreenForSeconds(fullBlindDuration, fadeOutDuration);
        }
        if (sendToOtherPlayers)
        {
            Debug.Log("send blind for seconds instruction to other players");
            View.RPC("DisplayBlindScreenForSecondsToGivenPlayer", RpcTarget.OthersBuffered, fullBlindDuration, fadeOutDuration, photonViewID, false);
        }
        else
        {
            Debug.Log("Got this blind for seconds instruction from other player");
        }
    }

    void DisplayFullBlindScreenForSeconds(float fullBlindDuration, float fadeOutDuration)
    {
        print("Blinded!");
        BlindScreen.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        HoldAndFadeOutBlindScreen(fullBlindDuration, fadeOutDuration);
    }

    void HoldAndFadeOutBlindScreen(float fullBlindDuration, float fadeOutDuration)
    {
        StartCoroutine(HoldBlindScreen(fullBlindDuration, fadeOutDuration));
    }

    IEnumerator HoldBlindScreen(float fullBlindDuration, float fadeOutDuration)
    {
        yield return new WaitForSeconds(fullBlindDuration);
        yield return StartCoroutine(FadeOutBlindScreen(fadeOutDuration));
    }

    IEnumerator FadeOutBlindScreen(float fadeOutDuration)
    {
        float fadeOutTimer = 0.0f;

        while (fadeOutTimer < fadeOutDuration)
        {
            float faded_opacity = Mathf.Lerp(1.0f, 0.0f, fadeOutTimer / fadeOutDuration);
            BlindScreen.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, faded_opacity);
            fadeOutTimer += Time.deltaTime;
            yield return null;
        }
        SetBlindScreenToFullyTransparent();
        yield return null;
    }

    void SetBlindScreenToFullyTransparent()
    {
        print("No longer blinded");
        BlindScreen.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    public void DisplayGameOverScreenForEveryone()
    {
        DisplayGameOverScreen();
        View.RPC("DisplayGameOverScreen", RpcTarget.Others);
    }

    [PunRPC]
    void DisplayGameOverScreen()
    {
        if (View.IsMine)
        {
            Debug.Log("send victory instruction to other players, means I won");
            if (!CGDGameSettings.PlayingAsGuest)
            {
                StartCoroutine(UpdateStats(CGDGameSettings.Username, true, 5));
            }
            CGDGameOverScreenManager.DisplayWinScreen();
        }
        else
        {
            Debug.Log("Got this victory instruction from other player, means I lost");
            if (!CGDGameSettings.PlayingAsGuest)
            {
                StartCoroutine(UpdateStats(CGDGameSettings.Username, false, 1));
            }
            CGDGameOverScreenManager.DisplayLossScreen();
        }
    }

    // Other helper functions
    public void StartSliding(float Duration)
    {
        _sliding = true;
        print("now sliding");
        Invoke("StopSliding", Duration);
    }

    void StopSliding()
    {
        _sliding = false;
        print("no longer sliding");
    }

    public void ApplyJumpModifierForSeconds(float modiferPercentage, float duration)
    {
        _jumpModifier = 1.0f - (modiferPercentage / 100.0f);
        Invoke("ResetJumpModifier", duration);
    }

    void ResetJumpModifier()
    {
        _jumpModifier = 1.0f;
        print("returned to normal jump power");
    }

    public virtual void UltimateAttack()
    {
        print("Player Ultimate Attack");
    }

    public void ModifyUltimateChargeUltFromPickup(float chargeAmount)
    {
        if (_ableToPickupUlt)
        {
            _ableToPickupUlt = false;
            Invoke("AbleToPickUltAgain", _ultPickupDelay);
            if (chargeAmount > 0.0f)
            {
                print("Increased charge by: " + chargeAmount);
            }
            else
            {
                print("Decreased charge by: " + -chargeAmount);
            }
            UltimateCharge += chargeAmount;
            if (UltimateCharge > 100.0f)
            {
                UltimateCharge = 100.0f;
            }
            else if (UltimateCharge < 0.0f)
            {
                UltimateCharge = 0.0f;
            }
            if (View.IsMine)
            {
                UltimateBar.SetBar(UltimateCharge);
            }
            print("Ultimate Charge: " + UltimateCharge);
        }
    }

    void AbleToPickUltAgain()
    {
        _ableToPickupUlt = true;
    }

    public void ModifyUltimateCharge(float chargeAmount)
    {
        if (chargeAmount > 0.0f)
        {
            print("Increased charge by: " + chargeAmount);
        }
        else
        {
            print("Decreased charge by: " + -chargeAmount);
        }
        UltimateCharge += chargeAmount;
        if (UltimateCharge > 100.0f)
        {
            UltimateCharge = 100.0f;
        }
        else if (UltimateCharge < 0.0f)
        {
            UltimateCharge = 0.0f;
        }
        if (View.IsMine)
        {
            UltimateBar.SetBar(UltimateCharge);
        }
        print("Ultimate Charge: " + UltimateCharge);
    }

    IEnumerator UpdateStats(string username, bool won, int silver)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernameupdatestats", username);
        form.AddField("wonupdatestats", won.ToString());
        form.AddField("silverupdatestats", silver.ToString());

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/CGDPHP/UpdateStats.php", form))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                string jsonData = webRequest.downloadHandler.text;
                print("Updated stats sent off to database " + jsonData);
            }
        }
    }

    // Enum
    public enum Character
    {
        Medusa,
        Midas,
        Narcissus,
        Arachne,
        None
    }
}
