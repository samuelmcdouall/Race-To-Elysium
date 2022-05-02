using UnityEngine;
using Photon.Pun;

public class CGDMedusaPlayer : CGDPlayer
{
    [Header("Ultimate Attack")]
    [SerializeField]
    float _freezeDuration;
    [SerializeField]
    float _freezeRange;
    public GameObject FreezeFX;
    public GameObject Crosshair;

    void Awake()
    {
        InitialPlayerSetup();
    }

    public override void Update()
    {
        base.Update();
        if (View.IsMine)
        {
            if (_enabledControls && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    UltimateAttack();
                }
            }
        }
    }

    public override void UltimateAttack()
    {
        if (UltimateCharge == 100.0f && _enabledControls && !CGDGameOverScreenManager.GameOver && !CGDPauseManager.Paused)
        {
            Debug.Log("Medusa Ultimate Attack!");
            UltimateCharge = 0.0f;
            UltimateBar.SetBar(UltimateCharge);
            SwitchAnimationStateTo(_medusaUltimateAttackState, true);
            _ignoreStateChange = true;
            Invoke("UltimateAttackComplete", UltAttackAnimationDelay);
            PlaySoundClipForEveryone(transform.position.x, transform.position.y, transform.position.z, "MedusaUltSFX", true);
            RaycastHit hit;
            Vector3 forwardDirection = new Vector3(CameraTr.forward.x, CameraTr.forward.y, CameraTr.forward.z);
            forwardDirection = forwardDirection.normalized;
            if (Physics.Raycast(CameraTr.position, forwardDirection, out hit, _freezeRange))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    Debug.Log("I just hit a player, freeze them!");
                    int photonViewID = hit.transform.gameObject.GetComponent<PhotonView>().ViewID;
                    PlayFXForEveryone(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z, "MedusaUltFX", true);
                    hit.transform.gameObject.GetComponent<CGDPlayer>().DisableControlsForSecondsToGivenPlayer(_freezeDuration, photonViewID, true);
                }
            }
            else
            {
                Debug.Log("Didn't hit anything!");
            }
        }
        else
        {
            Debug.Log("Not enough charge!");
        }
    }

    public override void InitialPlayerSetup()
    {
        base.InitialPlayerSetup();
        ThisCharacter = Character.Medusa;
        if (!View.IsMine)
        {
            Crosshair.SetActive(false);
        }
    }

    void UltimateAttackComplete()
    {
        _ignoreStateChange = false;
    }
}
