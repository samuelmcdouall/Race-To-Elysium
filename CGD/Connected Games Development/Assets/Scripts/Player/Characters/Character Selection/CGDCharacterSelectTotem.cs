using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDCharacterSelectTotem : MonoBehaviour
{
    public TotemCharacter _character;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (_character == TotemCharacter.Medusa)
            {
                other.gameObject.GetComponent<CGDPlayer>().NewCharacter = CGDPlayer.Character.Medusa;
            }

            else if (_character == TotemCharacter.Midas)
            {
                other.gameObject.GetComponent<CGDPlayer>().NewCharacter = CGDPlayer.Character.Midas;
            }

            else if (_character == TotemCharacter.Narcissus)
            {
                other.gameObject.GetComponent<CGDPlayer>().NewCharacter = CGDPlayer.Character.Narcissus;
            }

            else if (_character == TotemCharacter.Arachne)
            {
                other.gameObject.GetComponent<CGDPlayer>().NewCharacter = CGDPlayer.Character.Arachne;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CGDPlayer>().NewCharacter = CGDPlayer.Character.None;
        }
    }

    public enum TotemCharacter
    {
        Medusa,
        Midas,
        Narcissus,
        Arachne
    }
}
