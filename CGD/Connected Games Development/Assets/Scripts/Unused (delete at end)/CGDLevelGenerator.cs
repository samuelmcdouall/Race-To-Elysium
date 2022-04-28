using Photon.Pun;
using UnityEngine;

public class CGDLevelGenerator : MonoBehaviour
{
    public GameObject TartarusPreset0;
    public GameObject TartarusPreset1;
    public GameObject TartarusPreset2;
    public GameObject GaiaPreset0;
    public GameObject GaiaPreset1;
    public GameObject GaiaPreset2;
    public GameObject ElysiumPreset0;
    public GameObject ElysiumPreset1;
    public GameObject ElysiumPreset2;
    PhotonView _view;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            GenerateLevelForAllPlayers();
        }
    }

    void GenerateLevelForAllPlayers()
    {
        int tartarusPreset = Random.Range(0, 1);
        int gaiaPreset = Random.Range(0, 1);
        int elysiumPreset = Random.Range(0, 1);
        _view.RPC("GenerateLevel", RpcTarget.AllBuffered, tartarusPreset, gaiaPreset, elysiumPreset);
    }

    [PunRPC]
    public void GenerateLevel(int tartarusPreset, int gaiaPreset, int elysiumPreset)
    {
        print("Tartarus preset: " + tartarusPreset);
        print("Gaia preset: " + gaiaPreset);
        print("Elysium preset: " + elysiumPreset);

        switch (tartarusPreset)
        {
            case 0:
                TartarusPreset0.SetActive(true);
                break;
            case 1:
                TartarusPreset1.SetActive(true);
                break;
            case 2:
                TartarusPreset2.SetActive(true);
                break;
            default:
                print("Error generating Tartarus preset");
                break;
        }
        switch (gaiaPreset)
        {
            case 0:
                GaiaPreset0.SetActive(true);
                break;
            case 1:
                GaiaPreset1.SetActive(true);
                break;
            case 2:
                GaiaPreset2.SetActive(true);
                break;
            default:
                print("Error generating Gaia preset");
                break;
        }
        switch (elysiumPreset)
        {
            case 0:
                ElysiumPreset0.SetActive(true);
                break;
            case 1:
                ElysiumPreset1.SetActive(true);
                break;
            case 2:
                ElysiumPreset2.SetActive(true);
                break;
            default:
                print("Error generating Elysium preset");
                break;
        }
    }
}