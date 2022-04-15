using UnityEngine;
using Photon.Pun;

public class CGDArachnePlayer : CGDPlayer
{
    [Header("Ultimate Attack")]
    public GameObject ArachneProjectile;
    public Transform ProjectileSpawnPoint;
    [SerializeField]
    float _projectileSpeed;

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
            print("Arachne Ultimate Attack!");
            UltimateCharge = 0.0f;
            UltimateBar.SetBar(UltimateCharge);
            GameObject projectile = PhotonNetwork.Instantiate(ArachneProjectile.name, ProjectileSpawnPoint.position, Quaternion.identity);
            projectile.GetComponent<CGDArachneProjectile>().OwnPlayer = gameObject;
            Vector3 forwardDirection = new Vector3(CameraTr.forward.x, CameraTr.forward.y, CameraTr.forward.z);
            forwardDirection = forwardDirection.normalized;
            projectile.GetComponent<Rigidbody>().velocity = new Vector3(forwardDirection.x, forwardDirection.y, forwardDirection.z) * _projectileSpeed;
            
        }
        else
        {
            print("Not enough charge!");
        }
    }

    public override void InitialPlayerSetup()
    {
        base.InitialPlayerSetup();
        ThisCharacter = Character.Arachne;
    }
}
