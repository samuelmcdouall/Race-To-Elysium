using UnityEngine;
using UnityEngine.UI;

public class CGDCharacterSelectStatue : MonoBehaviour
{
    public CharacterType Character;
    public Text CharTextDescBox;
    public GameObject SpawnGateTimer;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Character == CharacterType.Medusa)
            {
                other.gameObject.GetComponent<CGDPlayer>().NewCharacter = CGDPlayer.Character.Medusa;
            }
            else if (Character == CharacterType.Midas)
            {
                other.gameObject.GetComponent<CGDPlayer>().NewCharacter = CGDPlayer.Character.Midas;
            }
            else if (Character == CharacterType.Narcissus)
            {
                other.gameObject.GetComponent<CGDPlayer>().NewCharacter = CGDPlayer.Character.Narcissus;
            }
            else if (Character == CharacterType.Arachne)
            {
                other.gameObject.GetComponent<CGDPlayer>().NewCharacter = CGDPlayer.Character.Arachne;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<CGDPlayer>().View.IsMine)
            {
                if (SpawnGateTimer.GetComponent<CGDSpawnGateTimer>().TextFreeForCharDesc)
                {
                    if (Character == CharacterType.Medusa)
                    {
                        CharTextDescBox.text = "Receive the blessing of Medusa and freeze your enemies!";
                    }
                    else if (Character == CharacterType.Midas)
                    {
                        CharTextDescBox.text = "Receive the blessing of Midas and slow your enemies!";
                    }
                    else if (Character == CharacterType.Narcissus)
                    {
                        CharTextDescBox.text = "Receive the blessing of Narcissus and blind your enemies!";
                    }
                    else if (Character == CharacterType.Arachne)
                    {
                        CharTextDescBox.text = "Receive the blessing of Arachne and ensnare your enemies!";
                    }
                }
                else
                {
                    CharTextDescBox.text = "";
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CGDPlayer>().NewCharacter = CGDPlayer.Character.None;
            if (SpawnGateTimer.GetComponent<CGDSpawnGateTimer>().TextFreeForCharDesc && other.gameObject.GetComponent<CGDPlayer>().View.IsMine)
            {
                CharTextDescBox.text = "";
            }
        }
    }

    public enum CharacterType
    {
        Medusa,
        Midas,
        Narcissus,
        Arachne
    }
}
