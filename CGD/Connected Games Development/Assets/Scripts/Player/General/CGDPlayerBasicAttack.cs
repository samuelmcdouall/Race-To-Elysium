using Photon.Pun;
using UnityEngine;

public class CGDPlayerBasicAttack : MonoBehaviour
{
    public GameObject OwnPlayer;
    [SerializeField]
    float _repelBubbleDuration;
    float _repelBubbleDurationTimer;
    [SerializeField]
    float _repelForce;
    [SerializeField]
    float _repelCooldown;
    float _repelCooldownTimer;
    public AudioClip AttackSFX1;
    public AudioClip AttackSFX2;
    public AudioClip AttackHitPlayer;
    public AudioClip AttackHitGate;
    bool _readyToRepel;
    Collider _repelCollider;

    void Start()
    {
        _repelCollider = GetComponent<Collider>();
        _repelCollider.enabled = false;
        _repelBubbleDurationTimer = 0.0f;
        _repelCooldownTimer = 0.0f;
        _readyToRepel = true;
    }

    void Update()
    {
        if (_repelCollider.enabled)
        {
            RepelColliderEnabled();
        }
        else
        {
            RepelColliderDisabled();
        }
        if (Input.GetMouseButtonDown(0) 
            && _readyToRepel 
            && OwnPlayer.GetComponent<CGDPlayer>()._enabledControls 
            && !CGDGameOverScreenManager.GameOver 
            && !CGDPauseManager.Paused
        )
        {
            print("Basic attack pressed");
            PeformBasicAttack();
        }
    }

    void RepelColliderEnabled()
    {
        if (_repelBubbleDurationTimer > _repelBubbleDuration)
        {
            _repelBubbleDurationTimer = 0.0f;
            _repelCollider.enabled = false;
        }
        else
        {
            _repelBubbleDurationTimer += Time.deltaTime;
        }
    }

    void RepelColliderDisabled()
    {
        if (!_readyToRepel)
        {
            if (_repelCooldownTimer > _repelCooldown)
            {
                _repelCooldownTimer = 0.0f;
                _readyToRepel = true;
            }
            else
            {
                _repelCooldownTimer += Time.deltaTime;
            }
        }
    }

    void PeformBasicAttack()
    {
        int randSoundEffect = Random.Range(0, 2);
        if (randSoundEffect == 0)
        {
            AudioSource.PlayClipAtPoint(AttackSFX1, OwnPlayer.transform.position, CGDGameSettings.SoundVolume);
        }
        else
        {
            AudioSource.PlayClipAtPoint(AttackSFX2, OwnPlayer.transform.position, CGDGameSettings.SoundVolume);
        }
        _repelCollider.enabled = true;
        _readyToRepel = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && collider.gameObject != OwnPlayer)
        {
            print("Hit other player");
            AudioSource.PlayClipAtPoint(AttackHitPlayer, OwnPlayer.transform.position, CGDGameSettings.SoundVolume);
            Vector3 forceToAdd = DetermineForceVectorToApply(collider);
            ApplyForceToPlayer(collider, forceToAdd);
        }
        else if (collider.tag == "Gate")
        {
            print("Hit gate");
            AudioSource.PlayClipAtPoint(AttackHitGate, OwnPlayer.transform.position, CGDGameSettings.SoundVolume);
            collider.gameObject.GetComponent<CGDGate>().ReduceHealthOfGateForAllPlayers();
        }
    }

    Vector3 DetermineForceVectorToApply(Collider collider)
    {
        Vector3 playerToEnemyDirection = Vector3.Normalize(collider.gameObject.transform.position - OwnPlayer.transform.position);
        Vector3 forceToAdd = playerToEnemyDirection * _repelForce;
        return forceToAdd;
    }

    void ApplyForceToPlayer(Collider collider, Vector3 forceToAdd)
    {
        //Debug.LogError("apply force to player"); // todo once know this is the way to do it then can get rid of this
        int photonViewID = collider.gameObject.GetComponent<PhotonView>().ViewID;
        //Debug.LogError("This is my view id: " + OwnPlayer.GetComponent<CGDPlayer>().View.ViewID);
        //Debug.LogError("This is my targets view id: " + collider.gameObject.GetComponent<PhotonView>().ViewID);
        //Debug.LogError("This is my targets view id (other method): " + collider.gameObject.GetComponent<CGDPlayer>().View.ViewID);
        OwnPlayer.GetComponent<CGDPlayer>().SendKnockbackCommandToOtherPlayers(forceToAdd, photonViewID);
        OwnPlayer.GetComponent<CGDPlayer>().ModifyUltimateCharge(20.0f);
        //collider.gameObject.GetComponent<CGDPlayer>().DisableControlsForSecondsToGivenPlayer(0.8f, photonViewID, true); //todo I don't think use this
    }
}
