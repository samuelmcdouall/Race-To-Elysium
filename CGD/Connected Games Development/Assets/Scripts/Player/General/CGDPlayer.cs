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
    public CGDGroundCheck GroundCheck;
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
    float _ultPickupDelay = 0.5f; // Precaution delay so multiple colliders don't get hit in same frame
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
    public GameObject BlindScreen;
    public TextMeshProUGUI NameText;

    [Header("Checkpoint")]
    [System.NonSerialized]
    public Vector3 CheckpointPosition;
    float _checkpointOffset;

    [Header("FX/SFX")]
    public AudioClip JumpSFX;
    public GameObject LavaBurnFX;
    public GameObject PoisonBurnFX;
    public AudioClip NewPlayerSFX;
    public GameObject NewPlayerFX;
    [System.NonSerialized]
    public Outline PlayerOutline;
    public AudioClip MedusaUltSFX;
    public GameObject MedusaUltFX;
    public AudioClip MidasUltSFX;
    public GameObject MidasUltFX;
    public AudioClip NarcissusUltSFX;
    public GameObject NarcissusUltFX;
    public AudioClip ArachneUltSFX;
    public AudioClip AttackSFX1;
    public AudioClip AttackSFX2;
    public AudioClip AttackHitPlayer;
    public AudioClip AttackHitGate;
    public AudioClip AreaDenialProjectileLaunchSFX;

    [Header("Animation")]
    public Animator PlayerAnimator;
    string _currentState;
    string _idleState = "Character Idle";
    string _moveForwardsState = "Move Forwards";
    string _moveBackwardsState = "Move Backwards";
    string _moveLeftState = "Move Left";
    string _moveRightState = "Move Right";
    string _jumpUpState = "Jump Up";
    string _fallDownState = "Fall Down";
    string _getHitState = "Get Hit";
    [System.NonSerialized]
    public string _basicAttackState = "Basic Attack";
    [System.NonSerialized]
    public string _arachneUltimateAttackState = "Arachne Ultimate Attack";
    [System.NonSerialized]
    public string _midasUltimateAttackState = "Midas Ultimate Attack";
    [System.NonSerialized]
    public string _medusaUltimateAttackState = "Medusa Ultimate Attack";
    [System.NonSerialized]
    public string _narcissusUltimateAttackState = "Narcissus Ultimate Attack";
    [System.NonSerialized]
    public string _deployPowerUpState = "Deploy Power Up";
    [System.NonSerialized]
    public bool _ignoreStateChange = false;
    [SerializeField]
    float _getHitAnimationDelay;
    public float UltAttackAnimationDelay;

    [Header("Camera")]
    public GameObject MainCamera;
    [System.NonSerialized]
    public Transform CameraTr;

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

        if (NameText.text == "") // Done here to avoid race condition with username not being given in time to player to display nametag
        {
            if (!View.IsMine)
            {
                Debug.Log(View.Owner.NickName + " has joined the game");
                NameText.text = View.Owner.NickName;
            }
            else
            {
                NameText.text = "";
            }
        }
    }

    public virtual void InitialPlayerSetup()
    {
        _playerRb = GetComponent<Rigidbody>();
        View = GetComponent<PhotonView>();
        Cursor.visible = false;
        CameraTr = MainCamera.transform;
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
        _currentState = _idleState;



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
            float randX = Random.Range(-1.0f, 1.0f);
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
        if (_enabledControls)
        {
            HandleMovementMechanics();
        }
        HandleGroundCheckMechanics();
        if (_shouldJumpUp)
        {
            JumpUp();
            PlaySoundClipForEveryone(transform.position.x, transform.position.y, transform.position.z, "JumpSFX", true);
            _shouldJumpUp = false;
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
            if (GroundCheck.IsGrounded && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                SwitchAnimationStateTo(_moveForwardsState, true);
            }
            DetermineYIndependentVelocity(CameraTr.forward - CameraTr.right);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            if (GroundCheck.IsGrounded && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                SwitchAnimationStateTo(_moveForwardsState, true);
            }
            DetermineYIndependentVelocity(CameraTr.forward + CameraTr.right);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            if (GroundCheck.IsGrounded && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                SwitchAnimationStateTo(_idleState, true);
            }
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            if (GroundCheck.IsGrounded && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                SwitchAnimationStateTo(_moveBackwardsState, true);
            }
            DetermineYIndependentVelocity(-CameraTr.forward + CameraTr.right);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            if (GroundCheck.IsGrounded && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                SwitchAnimationStateTo(_moveBackwardsState, true);
            }
            DetermineYIndependentVelocity(-CameraTr.forward - CameraTr.right);
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            if (GroundCheck.IsGrounded && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                SwitchAnimationStateTo(_idleState, true);
            }
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (GroundCheck.IsGrounded && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                SwitchAnimationStateTo(_moveForwardsState, true);
            }
            DetermineYIndependentVelocity(CameraTr.forward);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (GroundCheck.IsGrounded && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                SwitchAnimationStateTo(_moveBackwardsState, true);
            }
            DetermineYIndependentVelocity(-CameraTr.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (GroundCheck.IsGrounded && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                SwitchAnimationStateTo(_moveRightState, true);
            }
            DetermineYIndependentVelocity(CameraTr.right);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (GroundCheck.IsGrounded && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                SwitchAnimationStateTo(_moveLeftState, true);
            }
            DetermineYIndependentVelocity(-CameraTr.right);
        }
        else
        {
            if (GroundCheck.IsGrounded)
            {
                SwitchAnimationStateTo(_idleState, true);
            }
        }
    }

    void DetermineYIndependentVelocity(Vector3 horizontal_direction)
    {
        if (!CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
        {
            horizontal_direction = new Vector3(horizontal_direction.x, 0.0f, horizontal_direction.z);
            horizontal_direction = horizontal_direction.normalized * _playerMoveForce;
            _playerRb.AddForce(horizontal_direction);
        }
    }

    void HandleGroundCheckMechanics()
    {
        if (!GroundCheck.IsGrounded && _playerRb.velocity.y < -0.1f)
        {
            HandlePlayerFallingDown();
        }
    }

    void HandlePlayerFallingDown()
    {
        SwitchAnimationStateTo(_fallDownState, true);
        if (_ignoreStateChange == true)
        {
            _ignoreStateChange = false;
        }
    }

    void JumpUp()
    {
        SwitchAnimationStateTo(_jumpUpState, true);
        _ignoreStateChange = true;
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
            if (GroundCheck.IsGrounded)
            {
                _shouldJumpUp = true;
            }
        }
    }

    void TintSkyboxBasedOnVerticalPosition()
    {
        float scaledColorPosition = transform.position.y / 180.0f; // 180 is approximately as high as the map goes
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
    public void SwitchAnimationStateTo(string newState, bool sendToOthers)
    {
        if (newState != _currentState && !_ignoreStateChange)
        {
            if (_currentState == _fallDownState)
            {
                _ignoreStateChange = true;
                Invoke("FallComplete", 0.1f);   // Delay to make falling to idle state not happen twice
            }
            if (_currentState == _idleState)
            {
                PlayerAnimator.CrossFade(newState, 0.02f);
            }
            else
            {
                PlayerAnimator.CrossFade(newState, 0.1f);
            }
            _currentState = newState;
            if (sendToOthers)
            {
                View.RPC("SwitchAnimationStateTo", RpcTarget.Others, newState, false);
            }
        }
    }

    public void FallComplete()
    {
        _ignoreStateChange = false;
    }

    public void SendKnockbackCommandToOtherPlayers(Vector3 forceToAdd, int photonViewID)
    {
        View.RPC("SearchOtherPlayersToKnockback", RpcTarget.Others, forceToAdd, photonViewID);
    }

    [PunRPC]
    public void SearchOtherPlayersToKnockback(Vector3 forceToAdd, int photonViewID)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if (player.GetComponent<CGDPlayer>().View.IsMine && player.GetComponent<CGDPlayer>().View.ViewID == photonViewID)
            {
                player.GetComponent<Rigidbody>().AddForce(forceToAdd);
                player.GetComponent<CGDPlayer>().PlayHitAnimation();
            }
        }
    }

    public void PlayHitAnimation()
    {
        SwitchAnimationStateTo(_getHitState, true);
        _ignoreStateChange = true;
        Invoke("GetHitComplete", _getHitAnimationDelay);
    }
    public void GetHitComplete()
    {
        _ignoreStateChange = false;
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
    }

    [PunRPC]
    public void DisableControlsForSecondsToGivenPlayer(float Duration, int photonViewID, bool sendToOtherPlayers)
    {
        PhotonView photonView = PhotonView.Find(photonViewID);
        photonView.gameObject.GetComponent<CGDPlayer>().DisableControlsForSeconds(Duration);
        if (sendToOtherPlayers)
        {
            Debug.Log("Send disable controls for seconds instruction to other players");
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
            Debug.Log("Send blind for seconds instruction to other players");
            View.RPC("DisplayBlindScreenForSecondsToGivenPlayer", RpcTarget.OthersBuffered, fullBlindDuration, fadeOutDuration, photonViewID, false);
        }
        else
        {
            Debug.Log("Got this blind for seconds instruction from other player");
        }
    }

    void DisplayFullBlindScreenForSeconds(float fullBlindDuration, float fadeOutDuration)
    {
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
        BlindScreen.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    public void DisplayGameOverScreenForEveryone()
    {
        DisplayGameOverScreen();
        View.RPC("DisplayGameOverScreen", RpcTarget.Others);
    }

    [PunRPC]
    public void DisplayGameOverScreen()
    {
        if (View.IsMine)
        {
            Debug.Log("Send victory instruction to other players, means I won");
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

    [PunRPC]
    public void PlaySoundClipForEveryone(float xPos, float yPos, float zPos, string soundClipName, bool sendToOthers)
    {
        Vector3 soundPos = new Vector3(xPos, yPos, zPos);

        AudioClip chosenAudioClip;
        switch (soundClipName)
        {
            case "MedusaUltSFX":
                chosenAudioClip = MedusaUltSFX;
                break;
            case "MidasUltSFX":
                chosenAudioClip = MidasUltSFX;
                break;
            case "NarcissusUltSFX":
                chosenAudioClip = NarcissusUltSFX;
                break;
            case "ArachneUltSFX":
                chosenAudioClip = ArachneUltSFX;
                break;
            case "JumpSFX":
                chosenAudioClip = JumpSFX;
                break;
            case "AttackSFX1":
                chosenAudioClip = AttackSFX1;
                break;
            case "AttackSFX2":
                chosenAudioClip = AttackSFX2;
                break;
            case "AttackHitPlayer":
                chosenAudioClip = AttackHitPlayer;
                break;
            case "AttackHitGate":
                chosenAudioClip = AttackHitGate;
                break;
            case "AreaDenialProjectileLaunchSFX":
                chosenAudioClip = AreaDenialProjectileLaunchSFX;
                break;
            default:
                chosenAudioClip = null;
                Debug.Log("Error selecting clip");
                break;
        }

        if (chosenAudioClip != null)
        {
            AudioSource.PlayClipAtPoint(chosenAudioClip, soundPos, CGDGameSettings.SoundVolume);
        }

        if (sendToOthers)
        {
            View.RPC("PlaySoundClipForEveryone", RpcTarget.Others, xPos, yPos, zPos, soundClipName, false);
        }
    }

    [PunRPC]
    public void PlayFXForEveryone(float xPos, float yPos, float zPos, string fxName, bool sendToOthers)
    {
        Vector3 fxPos = new Vector3(xPos, yPos, zPos);

        GameObject chosenFX;
        switch (fxName)
        {
            case "MedusaUltFX":
                chosenFX = MedusaUltFX;
                break;
            case "MidasUltFX":
                chosenFX = MidasUltFX;
                break;
            case "NarcissusUltFX":
                chosenFX = NarcissusUltFX;
                break;
            default:
                chosenFX = null;
                Debug.Log("Error selecting fx");
                break;
        }

        if (chosenFX)
        {
            Instantiate(chosenFX, fxPos, Quaternion.identity);
        }

        if (sendToOthers)
        {
            View.RPC("PlayFXForEveryone", RpcTarget.Others, xPos, yPos, zPos, fxName, false);
        }
    }

    // Other helper functions
    public void StartSliding(float Duration)
    {
        _sliding = true;
        Invoke("StopSliding", Duration);
    }

    void StopSliding()
    {
        _sliding = false;
    }

    public void ApplyJumpModifierForSeconds(float modiferPercentage, float duration)
    {
        _jumpModifier = 1.0f - (modiferPercentage / 100.0f);
        Invoke("ResetJumpModifier", duration);
    }

    void ResetJumpModifier()
    {
        _jumpModifier = 1.0f;
    }

    public virtual void UltimateAttack()
    {
        Debug.Log("Player Ultimate Attack");
    }

    public void ModifyUltimateChargeFromPickup(float chargeAmount)
    {
        if (_ableToPickupUlt)
        {
            _ableToPickupUlt = false;
            Invoke("AbleToPickUltAgain", _ultPickupDelay);
            if (chargeAmount > 0.0f)
            {
                Debug.Log("Increased charge by: " + chargeAmount);
            }
            else
            {
                Debug.Log("Decreased charge by: " + -chargeAmount);
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
            Debug.Log("Ultimate Charge: " + UltimateCharge);
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
            Debug.Log("Increased charge by: " + chargeAmount);
        }
        else
        {
            Debug.Log("Decreased charge by: " + -chargeAmount);
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
        Debug.Log("Ultimate Charge: " + UltimateCharge);
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
                Debug.Log("Updated stats sent off to database " + jsonData);
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
